using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CommunicationsWithEctron.Engine;
using CommunicationsWithEctron.Engine.GPIB_Device;
using CommunicationsWithEctron.Engine.GPIB_NI.USB;

namespace CommunicationsWithEctron
{
    public partial class Form1 : Form
    {
        GPIB m_GPIB = new GPIB();

        public Form1()
        {
            InitializeComponent();
            m_GPIB.CalibEquipment = new CalibEquipmentEctron1140A();
            m_Name.Text = m_GPIB.CalibEquipment.Name;
            m_SerialNumber.Text = m_GPIB.CalibEquipment.SerialNumber;
            m_Trace.Text = m_GPIB.CalibEquipment.CalibrationTrace;

        }

        private void m_ReadVoltage_Click(object sender, EventArgs e)
        {
            IList<IGPIBDevice> ids = m_GPIB.GetAvailableGPIBDevices();
            if (ids.Count == 0)
            {
                m_Voltage.Text = "No devices available";
            }
            else
            {                
                try
                {
                    m_GPIB.Open(ids[0]);
                    //This may be required
                    //m_GPIB.EnableVoltage();

                    m_Voltage.Text = string.Format("{0}V", m_GPIB.Voltage);
                    m_GPIB.Close();
                }
                catch (System.Exception ex)
                {
                    m_Voltage.Text = ex.Message;
                }
                
            }
        }

        private void m_ReadTemp_Click(object sender, EventArgs e)
        {
            IList<IGPIBDevice> ids = m_GPIB.GetAvailableGPIBDevices();
            if (ids.Count == 0)
            {
                m_Temp.Text = "No devices available";
            }
            else
            {
                try
                {
                    m_GPIB.Open(ids[0]);
                    //This may be required
                    //m_GPIB.EnableTemperature();
                    m_Temp.Text = string.Format("{0}C", m_GPIB.Temperature);
                    m_GPIB.Close();
                }
                catch (System.Exception ex)
                {
                    m_Temp.Text = ex.Message;
                }

            }
        }

        private void m_Finish_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
