using System;
using System.Collections.Generic;
using System.Text;

namespace UsbLibrary
{
    public class SpecifiedOutputReport : OutputReport
    {
        
        public SpecifiedOutputReport(HIDDevice oDev) : base(oDev) 
        {

        }

        public bool SendData(byte[] data)
        {
            byte[] arrBuff = new byte[data.Length];
            for (int i = 0; i < arrBuff.Length; i++)
            {
                arrBuff[i] = data[i];
            }

            // Set report id to 0x3F for TI's MSP430 USB stack
            //arrBuff[0] = 0x3F;

            Buffer = arrBuff;

            //returns false if the data does not fit in the buffer. else true
            if (arrBuff.Length < data.Length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
