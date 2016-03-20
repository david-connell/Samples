namespace TQC.TestBasic.Communication
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.m_Output = new System.Windows.Forms.TextBox();
            this.m_Close = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.m_Devices = new System.Windows.Forms.ComboBox();
            this.m_OpenPort = new System.Windows.Forms.Button();
            this.m_ClosePort = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(125, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "List USB Devices";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // m_Output
            // 
            this.m_Output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_Output.Location = new System.Drawing.Point(1, 179);
            this.m_Output.Multiline = true;
            this.m_Output.Name = "m_Output";
            this.m_Output.ReadOnly = true;
            this.m_Output.Size = new System.Drawing.Size(498, 82);
            this.m_Output.TabIndex = 1;
            // 
            // m_Close
            // 
            this.m_Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_Close.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_Close.Location = new System.Drawing.Point(413, 150);
            this.m_Close.Name = "m_Close";
            this.m_Close.Size = new System.Drawing.Size(75, 23);
            this.m_Close.TabIndex = 2;
            this.m_Close.Text = "Close";
            this.m_Close.UseVisualStyleBackColor = true;
            this.m_Close.Click += new System.EventHandler(this.m_Close_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Output:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(87, 17);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "USB via C++";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // m_Devices
            // 
            this.m_Devices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_Devices.FormattingEnabled = true;
            this.m_Devices.Location = new System.Drawing.Point(155, 37);
            this.m_Devices.Name = "m_Devices";
            this.m_Devices.Size = new System.Drawing.Size(191, 21);
            this.m_Devices.TabIndex = 5;
            // 
            // m_OpenPort
            // 
            this.m_OpenPort.Location = new System.Drawing.Point(12, 70);
            this.m_OpenPort.Name = "m_OpenPort";
            this.m_OpenPort.Size = new System.Drawing.Size(125, 23);
            this.m_OpenPort.TabIndex = 6;
            this.m_OpenPort.Text = "Open USB Port";
            this.m_OpenPort.UseVisualStyleBackColor = true;
            this.m_OpenPort.Click += new System.EventHandler(this.m_OpenPort_Click);
            // 
            // m_ClosePort
            // 
            this.m_ClosePort.Location = new System.Drawing.Point(12, 125);
            this.m_ClosePort.Name = "m_ClosePort";
            this.m_ClosePort.Size = new System.Drawing.Size(125, 23);
            this.m_ClosePort.TabIndex = 7;
            this.m_ClosePort.Text = "Close USB Port";
            this.m_ClosePort.UseVisualStyleBackColor = true;
            this.m_ClosePort.Click += new System.EventHandler(this.m_ClosePort_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(13, 96);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(124, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "Make Request:";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.m_Close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_Close;
            this.ClientSize = new System.Drawing.Size(500, 262);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.m_ClosePort);
            this.Controls.Add(this.m_OpenPort);
            this.Controls.Add(this.m_Devices);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_Close);
            this.Controls.Add(this.m_Output);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "TQC.TestUSB.Comms";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox m_Output;
        private System.Windows.Forms.Button m_Close;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox m_Devices;
        private System.Windows.Forms.Button m_OpenPort;
        private System.Windows.Forms.Button m_ClosePort;
        private System.Windows.Forms.Button button2;
    }
}

