using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using log4net;
using TQC.USBDevice.Utils;

namespace TQC.USBDevice
{

    public interface IUsbInterfaceForm
    {
        IntPtr Handle { get; }        
    }

    internal class USBCommunication : IDisposable
    {
        private UsbLibrary.UsbHidPort m_UsbHidPort1;
        USBLogger m_UsbLogger;
        private ILog m_Log = LogManager.GetLogger("TQC.USBDevice.USBCommunication");
        ManualResetEvent m_Event = new ManualResetEvent(false);
        byte[] m_Data;
        const byte BounceCommand = 0xFF;
        IUsbInterfaceForm m_MainWindowForm;

        public USBCommunication(IUsbInterfaceForm mainWindowForm, USBLogger logger)
        {
            m_UsbLogger = logger;
            m_UsbHidPort1 = new UsbLibrary.UsbHidPort();
            m_UsbHidPort1.OnSpecifiedDeviceArrived += m_UsbHidPort1_OnSpecifiedDeviceArrived;
            m_UsbHidPort1.OnDataRecieved += m_UsbHidPort1_OnDataRecieved;
            m_UsbHidPort1.OnSpecifiedDeviceRemoved += m_UsbHidPort1_OnDeviceRemoved;
            m_UsbHidPort1.OnDataSend += m_UsbHidPort1_OnDataSend;
            m_MainWindowForm = mainWindowForm;
            if (mainWindowForm != null)
            {
                m_UsbHidPort1.RegisterHandle(m_MainWindowForm.Handle);
            }            
        }

        ~USBCommunication()
        {
            Dispose(false);
        }

        internal void OnWindowsMessage(ref System.Windows.Forms.Message m)
        {
            m_UsbHidPort1.ParseMessages(ref m);
        }
        bool IsUsbCommsStyleCurvex3
        {
            get
            {
                return m_UsbLogger.IsThermocoupleSimulator || m_UsbLogger.IsCurvex3 || m_UsbLogger.IsGRO;
            }
        }

        void m_UsbHidPort1_OnDataSend(object sender, EventArgs e)
        {

        }

        void m_UsbHidPort1_OnDeviceRemoved(object sender, EventArgs e)
        {
            m_Log.Info("Device removed");
        }

        void m_UsbHidPort1_OnSpecifiedDeviceArrived(object sender, EventArgs e)
        {
            m_Log.Info("Device Arrived");
        }


        void m_UsbHidPort1_OnDataRecieved(object sender, UsbLibrary.DataRecievedEventArgs args)
        {
            try
            {
                m_Data = args.data;
            }
            finally
            {
                m_Event.Set();
            }
        }

        private byte[] DecodeData(byte[] inputData, byte request, byte conversationId)
        {
            byte[] response = null;
            LogResponsePacket(inputData);
            if (inputData == null)
            {
                
            }
            else if (inputData.Length < 10)
            {
                throw new ResponsePacketErrorBadLengthException();
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
                    if (inputData[i + 2] == conversationId)
                    {
                        byte responseCommand = inputData[i + 3];
                        if ((responseCommand == (0xFF - request)) || ((responseCommand == BounceCommand) && (request == BounceCommand)))
                        {
                            int requestLength = inputData[4 + i];

                            if (IsValidCrc(inputData, i, requestLength))
                            {
                                response = new byte[requestLength];
                                Buffer.BlockCopy(inputData, 5 + i, response, 0, response.Length);
                            }
                            else
                            {
                                throw new ResponsePacketErrorCRCException();
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
                                            response = new byte[0];
                                            break;
                                        case 1:
                                            throw new DeviceUnknownErrorException();

                                        case 2:
                                            throw new CommandCorruptException();

                                        case 3:
                                            throw new CommandOutOfSequenceException();

                                        case 4:
                                            throw new CommandUnexpectedException();

                                        case 5:
                                            throw new DeviceBusyException();

                                        case 6:
                                            throw new CommandNotSuportedException();

                                        case 7:
                                            throw new EnumerationNotSuportedException();

                                        case 8:
                                            throw new BatchNotAvailableException();

                                        case 9:
                                            throw new DataOutOfRangeException();

                                        case 10: throw new CommandModeNotSupportedException();

                                        default:
                                            throw new ResponsePacketErrorBadCommandException();
                                    }
                                }
                                else
                                {
                                    throw new ResponsePacketErrorCRCException();
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new ResponsePacketErrorBadCommandException();
                    }
                }
                else
                {
                    throw new ResponsePacketErrorBadCommandException();
                }
            }
            return response;
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
            byte[] dataRecieved;
            lock (m_UsbHidPort1)
            {
                if (m_UsbHidPort1.SpecifiedDevice != null)
                {
                    m_Data = null;
                    m_Event.Reset();
                    m_UsbHidPort1.SpecifiedDevice.SendData(GenerateRequest(command, request, conversationId));

                    if (!m_Event.WaitOne(GetTimeOutForCommand(command)))
                    {
                        throw new ResponsePacketErrorTimeoutException();
                    }
                }
                dataRecieved = m_Data;
                m_Data = null;
            }
            return DecodeData(dataRecieved, (byte)command, conversationId);

        }

        private byte[] GenerateRequest(TQC.USBDevice.USBLogger.Commands command, byte[] request, byte conversationId)
        {

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
            data[i++] = (byte)command;
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
            var arrayToSend = new byte[0x41];
            Buffer.BlockCopy(data, 0, arrayToSend, 0, arrayToSend.Length);
            LogRequestPacket(arrayToSend);

            return arrayToSend;
        }

        bool? m_IsLoggingEnabled;

        bool IsLoggingEnabled
        {
            get
            {
                if (!m_IsLoggingEnabled.HasValue)
                {
                    m_IsLoggingEnabled = m_Log.IsDebugEnabled;
                }
                return m_IsLoggingEnabled.Value;
            }
        }

        private void LogRequestPacket(byte[] data)
        {
            LogPacket("PC->USB", data);
        }

        private void LogResponsePacket(byte[] data)
        {
            LogPacket("USB->PC", data);
        }

        private void LogPacket(string text, byte[] data)
        {
            if (IsLoggingEnabled)
            {
                int lineLength = 16;
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(text);
                if (data == null)
                {
                    builder.Append(" DATA IS NULL");
                }
                else
                {
                    for (int line = 0; line < data.Length / lineLength; line++)
                    {
                        StringBuilder part1 = new StringBuilder();
                        StringBuilder part2 = new StringBuilder();
                        int pos = line * lineLength;
                        for (int col = 0; col < lineLength && ((pos + col) < data.Length); col++)
                        {
                            byte dataValue = data[pos + col];
                            part1.Append(dataValue.ToString("X2"));
                            part1.Append(" ");
                            part2.Append(dataValue < 32 ? '*' : (char)dataValue);
                        }
                        builder.Append(part1);
                        builder.Append(part2);
                        builder.AppendLine();
                    }
                }

                m_Log.Debug(builder.ToString());
            }

        }

        private TimeSpan GetTimeOutForCommand(USBLogger.Commands command)
        {
            TimeSpan time2Wait = new TimeSpan(0, 0, 0, 6, 0);
            if (m_UsbLogger.IsCurvex3)
            {
                time2Wait = new TimeSpan(0, 0, 0, 1, 0);
            }
            else if (m_UsbLogger.IsGlossmeter)
            {
                time2Wait = new TimeSpan(0, 0, 0, 2, 0);
            }
            else if (m_UsbLogger.IsGRO)
            {
                time2Wait = new TimeSpan(0, 0, 0, 0, 200);
            }
            return time2Wait;
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

        public bool Open(TQC.USBDevice.USBLogger.USBProductId id, bool minimumCommunications, string portName = "")
        {
            m_UsbHidPort1.VendorId = ((int)id >> 16);
            m_UsbHidPort1.ProductId = ((int)id & 0xFFFF);

            m_UsbHidPort1.CheckDevicePresent();
            return m_UsbHidPort1.SpecifiedDevice != null;
        }



        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool inDispose)
        {
            if (inDispose)
            {
                if (m_MainWindowForm != null)
                {
                    //m_MainWindowForm.MessageEvent -= m_MainWindowForm_MessageEvent;
                }
                if (m_UsbHidPort1 != null)
                {
                    m_UsbHidPort1.UnregisterHandle();
                }
                GC.SuppressFinalize(this);
            }
        }

    }
}
