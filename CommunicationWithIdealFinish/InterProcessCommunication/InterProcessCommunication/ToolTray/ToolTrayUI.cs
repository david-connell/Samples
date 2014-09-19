using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TQC.GOC.InterProcessCommunication.Model;

namespace TQC.GOC.InterProcessCommunication.ToolTray
{
    internal class ToolTrayUI : IDisposable
    {
        private NotifyIcon m_NotifyIcon;				            // the icon that sits in the system tray
        DetailsForm m_DetailsForm;
        Form m_HelpAboutForm;
        GOCServerImplementation m_Server;
        public event EventHandler Update;
        public event EventHandler DataRateUpdate;

        private string m_Status;
        private int m_protocolErrors;
        private string m_Version;
        private string m_Path;


        internal ToolTrayUI(GOCServerImplementation server, System.ComponentModel.IContainer mainFormComponents, System.Drawing.Icon icon)
        {
            m_Server = server;
            m_NotifyIcon = new System.Windows.Forms.NotifyIcon(mainFormComponents);
            // 
            // notifyIcon1
            // 
            m_NotifyIcon.Icon = icon;
            m_NotifyIcon.Text = "GOC->Ideal Finish Comunication";
            m_NotifyIcon.Visible = true;
            m_NotifyIcon.ContextMenuStrip = new ContextMenuStrip();

            m_NotifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            m_NotifyIcon.DoubleClick += notifyIcon_DoubleClick;

            m_Server.Connect += m_Server_Connect;
            m_Server.Disconnect += m_Server_Disconnect;
            m_Server.ExceptionThrown += m_Server_ExceptionThrown;
            m_Server.GOCServerStatus += m_Server_GOCServerStatus;            
        }


        public string Status
        {
            get
            {
                lock (m_Server)
                    return m_Status;
            }
            set
            {
                lock (m_Server)
                    m_Status = value;
            }
        }

        public string Version
        {
            get
            {
                lock (m_Server)
                    return m_Version;
            }
            set
            {
                lock (m_Server)
                    m_Version = value;
            }
        }
        public string Path
        {
            get
            {
                lock (m_Server)
                    return m_Path;
            }
            set
            {
                lock (m_Server)
                    m_Path = value;
            }
        }

        public int ProtocolErrors
        {
            get
            {
                lock (m_Server)
                    return m_protocolErrors;
            }
            set
            {
                lock (m_Server)
                    m_protocolErrors = value;
            }
        }

        private void HelpAboutForm()
        {
            if (m_HelpAboutForm == null)
            {
                m_HelpAboutForm = new HelpAbout();
                m_HelpAboutForm.Closed += helpAboutForm_Closed;
                m_HelpAboutForm.Show();
            }
            else { m_HelpAboutForm.Activate(); }
        }

        private void ShowDetailsForm()
        {            
            if (m_DetailsForm == null)
            {
                m_DetailsForm = new DetailsForm (this);
                m_DetailsForm.Closed += detailsForm_Closed; 
                m_DetailsForm.Show();
            }
            else { m_DetailsForm.Activate(); }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e) 
        {            
            HelpAboutForm();    
        }


        // attach to context menu items
        private void showHelpItem_Click(object sender, EventArgs e)     { HelpAboutForm();    }
        private void showDetailsItem_Click(object sender, EventArgs e)  { ShowDetailsForm();  }

        // null out the forms so we know to create a new one.
        private void detailsForm_Closed(object sender, EventArgs e)     { m_DetailsForm = null; }
        private void helpAboutForm_Closed(object sender, EventArgs e)        { m_HelpAboutForm = null;   }
        
        
        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            m_NotifyIcon.ContextMenuStrip.Items.Clear();

            //m_NotifyIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            
            m_NotifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("Show &current status", showDetailsItem_Click));
            m_NotifyIcon.ContextMenuStrip.Items.Add(ToolStripMenuItemWithHandler("&About", showHelpItem_Click));
        }


        private ToolStripMenuItem ToolStripMenuItemWithHandler(
           string displayText, int enabledCount, int disabledCount, EventHandler eventHandler)
        {
            var item = new ToolStripMenuItem(displayText);
            if (eventHandler != null) { item.Click += eventHandler; }

            
            item.ToolTipText = (enabledCount > 0 && disabledCount > 0) ?
                                                 string.Format("{0} enabled, {1} disabled", enabledCount, disabledCount)
                         : (enabledCount > 0) ? string.Format("{0} enabled", enabledCount)
                         : (disabledCount > 0) ? string.Format("{0} disabled", disabledCount)
                         : "";
            return item;
        }

        public ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, EventHandler eventHandler)
        {
            return ToolStripMenuItemWithHandler(displayText, 0, 0, eventHandler);
        }

        public void Dispose()
        {
            m_Server.Connect -= m_Server_Connect;
            m_Server.Disconnect -= m_Server_Disconnect;
            m_Server.ExceptionThrown -= m_Server_ExceptionThrown;
            m_Server.GOCServerStatus -= m_Server_GOCServerStatus;            
            m_NotifyIcon.Dispose();
        }

        void m_Server_Connect(object sender, EventArgs e)
        {
            Status = "CONNECTED";
            if (Update != null)
                Update(this, null);

        }

        void m_Server_Disconnect(object sender, EventArgs e)
        {
            Status = "DISCONNECTED";
            Version = "";
            Path = "";
            if (Update != null)
                Update(this, null);

        }

        void m_Server_GOCServerStatus(object sender, GOCServerStatusEventArgs e)
        {
            if (!e.ProtocolStatus)
            {
                ProtocolErrors++;
                
            }
            switch (e.Status)
            {
                case GOCServerStatus.DataFolderRecieved:
                    {
                        Version = m_Server.IdealFinishAnalysisVersion.ToString();
                        Path = m_Server.DataFolder;
                        if (Update != null)
                            Update(this, null);
                    }
                    break;
                case GOCServerStatus.PingIng:
                    //Ignore
                    break;
                case GOCServerStatus.SendingDataHeader:
                case GOCServerStatus.SendingDataSamples:
                case GOCServerStatus.SendEndOfData:
                    TransmittedData(1);
                    break;


            }
        }


        int m_CurrentSlotId;
        int [] m_DataRates = new int [2];

        DateTime m_StartDateTime = DateTime.Now;

        private int GetSlotId(DateTime dateTime)
        {
            return (int)(dateTime - m_StartDateTime).TotalSeconds;
        }

        DateTime m_CurrentPoint = DateTime.Now;
        

        private void TransmittedData(int packets)
        {
            int slotId = GetSlotId(DateTime.Now);
            lock (m_DataRates)
            {

                if (m_CurrentSlotId != slotId)
                {
                    m_CurrentSlotId = slotId;
                    m_DataRates[slotId%2] = 0;

                }
                m_DataRates[slotId%2]++;
            }

            if (DataRateUpdate != null)
                DataRateUpdate(this, null);
        }

        public int DataRate
        {
            get
            {
                int slotId = GetSlotId(DateTime.Now)-1;
                lock (m_DataRates)
                {
                    int numberOfSlotsAway = m_CurrentSlotId - slotId;
                    if ((numberOfSlotsAway >= 0) && (numberOfSlotsAway  <= 2))
                    {
                        return m_DataRates[slotId % 2];
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
        }

        void m_Server_ExceptionThrown(object sender, ExceptionEventArgs e)
        {
        }

        public DateTime LastPing
        {
            get
            {
                return m_Server.LastPing;
            }
        }

        public string ServerState 
        {
            get
            {
                return m_Server.ServerStatus.ToString() ;
            }
        }

        public SamplePoint CurrentReadings
        {
            get
            {
                return m_Server.CurrentReadings;
            }
        }
        public int QueueSize
        {
            get
            {
                return m_Server.SizeOfQueue;
            }
        }
        public int NumberOfSamples
        {
            get
            {
                return m_Server.NumberOfSamples;
            }
        }

    }
}
