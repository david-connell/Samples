namespace TQC.GOC.InterProcessCommunication.ToolTray
{
    partial class LogFormDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogFormDetails));
            this.m_OK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_LogFile = new System.Windows.Forms.TextBox();
            this.m_ViewAppLogFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.m_UsbLogfilePath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.m_Warning = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.m_IdealFinishLogFile = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // m_OK
            // 
            this.m_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_OK.Location = new System.Drawing.Point(404, 115);
            this.m_OK.Name = "m_OK";
            this.m_OK.Size = new System.Drawing.Size(75, 23);
            this.m_OK.TabIndex = 6;
            this.m_OK.Text = "OK";
            this.m_OK.UseVisualStyleBackColor = true;
            this.m_OK.Click += new System.EventHandler(this.m_OK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Application log file:";
            // 
            // m_LogFile
            // 
            this.m_LogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_LogFile.Location = new System.Drawing.Point(143, 13);
            this.m_LogFile.Name = "m_LogFile";
            this.m_LogFile.ReadOnly = true;
            this.m_LogFile.Size = new System.Drawing.Size(300, 20);
            this.m_LogFile.TabIndex = 8;
            // 
            // m_ViewAppLogFile
            // 
            this.m_ViewAppLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ViewAppLogFile.Image = ((System.Drawing.Image)(resources.GetObject("m_ViewAppLogFile.Image")));
            this.m_ViewAppLogFile.Location = new System.Drawing.Point(449, 12);
            this.m_ViewAppLogFile.Name = "m_ViewAppLogFile";
            this.m_ViewAppLogFile.Size = new System.Drawing.Size(27, 21);
            this.m_ViewAppLogFile.TabIndex = 9;
            this.m_ViewAppLogFile.UseVisualStyleBackColor = true;
            this.m_ViewAppLogFile.Click += new System.EventHandler(this.m_ViewAppLogFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "USB log file:";
            // 
            // m_UsbLogfilePath
            // 
            this.m_UsbLogfilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_UsbLogfilePath.Location = new System.Drawing.Point(143, 65);
            this.m_UsbLogfilePath.Name = "m_UsbLogfilePath";
            this.m_UsbLogfilePath.Size = new System.Drawing.Size(300, 20);
            this.m_UsbLogfilePath.TabIndex = 11;
            this.m_UsbLogfilePath.TextChanged += new System.EventHandler(this.m_UsbLogfilePath_TextChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(449, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 21);
            this.button1.TabIndex = 12;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // m_Warning
            // 
            this.m_Warning.AutoSize = true;
            this.m_Warning.Location = new System.Drawing.Point(143, 92);
            this.m_Warning.Name = "m_Warning";
            this.m_Warning.Size = new System.Drawing.Size(219, 13);
            this.m_Warning.TabIndex = 13;
            this.m_Warning.Text = "(Restart the application for this to take effect)";
            this.m_Warning.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Ideal Finish Analysis log file:";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Image = ((System.Drawing.Image)(resources.GetObject("button2.Image")));
            this.button2.Location = new System.Drawing.Point(449, 38);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(27, 21);
            this.button2.TabIndex = 16;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // m_IdealFinishLogFile
            // 
            this.m_IdealFinishLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_IdealFinishLogFile.Location = new System.Drawing.Point(143, 39);
            this.m_IdealFinishLogFile.Name = "m_IdealFinishLogFile";
            this.m_IdealFinishLogFile.ReadOnly = true;
            this.m_IdealFinishLogFile.Size = new System.Drawing.Size(300, 20);
            this.m_IdealFinishLogFile.TabIndex = 15;
            // 
            // LogFormDetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 150);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.m_IdealFinishLogFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_Warning);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.m_UsbLogfilePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_ViewAppLogFile);
            this.Controls.Add(this.m_LogFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_OK);
            this.Name = "LogFormDetails";
            this.Text = "Logging Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_OK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_LogFile;
        private System.Windows.Forms.Button m_ViewAppLogFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_UsbLogfilePath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label m_Warning;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox m_IdealFinishLogFile;
    }
}