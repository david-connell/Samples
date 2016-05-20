using System;
namespace TQC.USBDevice.Curvex3
{
    public class Curvex3Standard : TQCUsbLogger
    {

        public Curvex3Standard(IUsbInterfaceForm mainWinForm)
            : base(mainWinForm, null)
        {
        }

        public void ResetRechargeableBatteryCapacity()
        {
            _ResetRechargeableBatteryCapacity(0);
        }


        internal void _ResetRechargeableBatteryCapacity(byte deviceId)
        {
            var response = Request(Commands.LoggerResetCommand, BitConverter.GetBytes((short)0x06), deviceId);
            if (response != null && response.Length > 0)
            {
                throw new NoDataReceivedException("_ResetRechargeableBatteryCapacity");
            }
            
        }
    }
}