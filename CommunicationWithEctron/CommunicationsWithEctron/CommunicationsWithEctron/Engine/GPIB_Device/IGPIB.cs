using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CommunicationsWithEctron.Engine.GPIB_Device
{
    public interface IGPIBDevice
    {
        
    }

    public interface IGPIB: IDisposable
    {
        IList<IGPIBDevice> GetAvailableGPIBDevices();

        void Open(IGPIBDevice device);
        void Close();
        bool IsOpen { get; }
        void Write(String write);
        String Read();

        ICalibEquipment CalibEquipment
        {
            set;
            get;
        }

        double Temperature
        {
            set;
            get;
        }

        double Voltage
        {
            get;
            set;
        }
        string EnableTemperature();
        string EnableVoltage();

        //double ReadCurrentTemp();
        //double ReadCurrentVoltage();
    }
}
