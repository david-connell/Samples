using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TQC.USBDevice.Utils;

namespace TQC.USBDevice
{
    internal class USBCommunication
    {
        private UsbLibrary.UsbHidPort m_UsbHidPort1;
        USBLogger m_UsbLogger;

        public USBCommunication(USBLogger logger)
        {
            m_UsbLogger = logger;
            m_UsbHidPort1 = new UsbLibrary.UsbHidPort();
            m_UsbHidPort1.OnSpecifiedDeviceArrived += m_UsbHidPort1_OnSpecifiedDeviceArrived;
            m_UsbHidPort1.OnDataRecieved += m_UsbHidPort1_OnDataRecieved;
            m_UsbHidPort1.OnDeviceRemoved += m_UsbHidPort1_OnDeviceRemoved;
            m_UsbHidPort1.OnDataSend += m_UsbHidPort1_OnDataSend;

        }

        bool IsUsbCommsStyleCurvex3
        {
            get
            {
                return m_UsbLogger.IsThermocoupleSimulator || m_UsbLogger.IsCurvex3;
            }
        }

        void m_UsbHidPort1_OnDataSend(object sender, EventArgs e)
        {

        }

        void m_UsbHidPort1_OnDeviceRemoved(object sender, EventArgs e)
        {

        }

        void m_UsbHidPort1_OnSpecifiedDeviceArrived(object sender, EventArgs e)
        {

        }


        ManualResetEvent m_Event = new ManualResetEvent(false);
        byte[] m_Data;
        Exception m_DataException;
        byte m_ConversationId;
        byte m_Request;
        const byte BounceCommand = 0xFF;

        void m_UsbHidPort1_OnDataRecieved(object sender, UsbLibrary.DataRecievedEventArgs args)
        {
            try
            {
                DecodeData(args.data);
            }
            finally
            {
                m_Event.Set();
            }
        }

        private void DecodeData(byte[] inputData)
        {
            if (inputData.Length < 10)
            {
                m_DataException = new ResponsePacketErrorBadLengthException();
            }
            else
            {
                int i = 0;
                if (IsUsbCommsStyleCurvex3)
                {
                    i += 2;
                }
                else
                {
                    if (inputData[0] == 0)
                    {
                        i++;
                    }
                }
                if (inputData[i] == 0xCD && inputData[i + 1] == 0xCD)
                {
                    if (inputData[i + 2] == m_ConversationId)
                    {
                        byte responseCommand = inputData[i + 3];
                        if ((responseCommand == (0xFF - m_Request)) || ((responseCommand == BounceCommand) && (m_Request == BounceCommand)))
                        {
                            int requestLength = inputData[4 + i];

                            if (IsValidCrc(inputData, i, requestLength))
                            {
                                m_Data = new byte[requestLength];
                                Buffer.BlockCopy(inputData, 5 + i, m_Data, 0, m_Data.Length);
                            }
                            else
                            {
                                m_DataException = new ResponsePacketErrorCRCException();
                            }
                        }
                        else if (responseCommand == 0) //This is a status result
                        {
                            int requestLength = inputData[4 + i];
                            if (requestLength == 4)
                            {
                                if (IsValidCrc(inputData, i, requestLength))
                                {
                                    switch (inputData[5 + i])
                                    {
                                        case 0:
                                            m_Data = new byte[0];
                                            break;
                                        case 1:
                                            m_DataException = new DeviceUnknownErrorException(); break;

                                        case 2:
                                            m_DataException = new CommandCorruptException(); break;

                                        case 3:
                                            m_DataException = new CommandOutOfSequenceException(); break;

                                        case 4:
                                            m_DataException = new CommandUnexpectedException(); break;

                                        case 5:
                                            m_DataException = new DeviceBusyException(); break;

                                        case 6:
                                            m_DataException = new CommandNotSuportedException(); break;

                                        case 7:
                                            m_DataException = new EnumerationNotSuportedException(); break;

                                        case 8:
                                            m_DataException = new BatchNotAvailableException(); break;

                                        case 9:
                                            m_DataException = new DataOutOfRangeException(); break;

                                        case 10: m_DataException = new CommandModeNotSupportedException(); break;

                                        default:
                                            m_DataException = new ResponsePacketErrorBadCommandException();
                                            break;
                                    }
                                }
                                else
                                {
                                    m_DataException = new ResponsePacketErrorCRCException();
                                }
                            }
                        }
                    }
                    else
                    {
                        m_DataException = new ResponsePacketErrorBadCommandException();
                    }
                }
                else
                {
                    m_DataException = new ResponsePacketErrorBadCommandException();
                }
            }

        }

        private bool IsValidCrc(byte[] inputData, int i, int requestLength)
        {
            var crcFromLogger = BitConverter.ToUInt32(inputData, i + requestLength + 5);
            var crcFromPacket = GetCRC32(inputData, i, requestLength + 5);
            bool isValidCrc = crcFromPacket == crcFromLogger;
            return isValidCrc;
        }

        private UInt32 GetCRC32(byte[] inputData, int offset, int requestLength)
        {
            var buffer = new byte[requestLength];
            Buffer.BlockCopy(inputData, offset, buffer, 0, buffer.Length);
            return Crc32.Calculate(buffer);
        }


        public byte[] IssueRequest(TQC.USBDevice.USBLogger.Commands command, byte[] request, byte conversationId)
        {
            lock (m_UsbHidPort1)
            {
                m_Data = null;
                m_DataException = null;

                m_ConversationId = conversationId;
                m_Request = (byte)command;
                if (request.Length > 65 - 6 - 4)
                {
                    throw new ResponsePacketErrorBadLengthException();
                }
                byte[] data = new byte[65];
                int i = 0;
                if (IsUsbCommsStyleCurvex3)
                {
                    data[i++] = 63;
                    data[i++] = 64;
                }
                else
                {
                    data[i++] = 0x00;
                }
                data[i++] = 0xCD;
                data[i++] = 0xCD;
                data[i++] = conversationId;
                data[i++] = m_Request;
                data[i++] = (byte)request.Length;
                foreach (byte val in request)
                {
                    data[i++] = val;
                }

                byte[] crcAsBits = null; ;
                if (IsUsbCommsStyleCurvex3)
                {
                    var buffer = new byte[request.Length + 5];
                    Buffer.BlockCopy(data, 2, buffer, 0, buffer.Length);
                    var crc = Crc32.Calculate(buffer);
                    crcAsBits = BitConverter.GetBytes(crc);

                }
                else
                {
                    var buffer = new byte[request.Length + 6];
                    Buffer.BlockCopy(data, 0, buffer, 0, buffer.Length);
                    var crc = Crc32.Calculate(buffer);
                    crcAsBits = BitConverter.GetBytes(crc);
                }
                foreach (byte val in crcAsBits)
                {
                    data[i++] = val;
                }
                m_Event.Reset();
                var arrayToSend = new byte[0x41];
                Buffer.BlockCopy(data, 0, arrayToSend, 0, arrayToSend.Length);
                m_UsbHidPort1.SpecifiedDevice.SendData(arrayToSend);

                if (!m_Event.WaitOne(2000))
                {
                    throw new ResponsePacketErrorTimeoutException();
                }
                if (m_DataException != null)
                {
                    throw m_DataException;
                }
                return m_Data;
            }

        }

        public void Close()
        {
            if (m_UsbHidPort1 != null)
            {
                if (m_UsbHidPort1.SpecifiedDevice != null)
                {
                    m_UsbHidPort1.SpecifiedDevice.Dispose();
                }
                m_UsbHidPort1 = null;
            }               
        }

        public bool Open(TQC.USBDevice.USBLogger.USBProductId id, bool minimumCommunications, string portName="")
        {
            m_UsbHidPort1.VendorId = ((int)id >> 16);
            m_UsbHidPort1.ProductId = ((int)id & 0xFFFF);

            m_UsbHidPort1.CheckDevicePresent();
            return m_UsbHidPort1.SpecifiedDevice != null;
        }


    }
}
