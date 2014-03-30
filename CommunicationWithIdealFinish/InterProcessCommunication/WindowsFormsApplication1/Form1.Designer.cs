namespace WindowsFormsApplication1
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
            this.components = new System.ComponentModel.Container();
            this.m_SendData = new System.Windows.Forms.Button();
            this.m_Debug = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.m_Connected = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_Path = new System.Windows.Forms.Label();
            this.m_Version = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.m_DataRate = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_SendData
            // 
            this.m_SendData.Location = new System.Drawing.Point(12, 12);
            this.m_SendData.Name = "m_SendData";
            this.m_SendData.Size = new System.Drawing.Size(72, 23);
            this.m_SendData.TabIndex = 0;
            this.m_SendData.Text = "Start";
            this.m_SendData.UseVisualStyleBackColor = true;
            this.m_SendData.Click += new System.EventHandler(this.Start);
            // 
            // m_Debug
            // 
            this.m_Debug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_Debug.BackColor = System.Drawing.Color.Black;
            this.m_Debug.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_Debug.ForeColor = System.Drawing.Color.White;
            this.m_Debug.Location = new System.Drawing.Point(0, 204);
            this.m_Debug.Multiline = true;
            this.m_Debug.Name = "m_Debug";
            this.m_Debug.Size = new System.Drawing.Size(410, 137);
            this.m_Debug.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // m_Connected
            // 
            this.m_Connected.Location = new System.Drawing.Point(12, 178);
            this.m_Connected.Name = "m_Connected";
            this.m_Connected.Size = new System.Drawing.Size(104, 23);
            this.m_Connected.TabIndex = 3;
            this.m_Connected.Text = "NOT CONNECTED";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(106, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Send Sample";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.SendSample);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(229, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Stop);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Version:";
            // 
            // m_Path
            // 
            this.m_Path.AutoSize = true;
            this.m_Path.Location = new System.Drawing.Point(70, 125);
            this.m_Path.Name = "m_Path";
            this.m_Path.Size = new System.Drawing.Size(35, 13);
            this.m_Path.TabIndex = 8;
            this.m_Path.Text = "label3";
            // 
            // m_Version
            // 
            this.m_Version.AutoSize = true;
            this.m_Version.Location = new System.Drawing.Point(70, 151);
            this.m_Version.Name = "m_Version";
            this.m_Version.Size = new System.Drawing.Size(35, 13);
            this.m_Version.TabIndex = 9;
            this.m_Version.Text = "label3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Time from last ping:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(120, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "label4";
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Data rate:";
            // 
            // m_DataRate
            // 
            this.m_DataRate.AutoSize = true;
            this.m_DataRate.Location = new System.Drawing.Point(123, 57);
            this.m_DataRate.Name = "m_DataRate";
            this.m_DataRate.Size = new System.Drawing.Size(35, 13);
            this.m_DataRate.TabIndex = 13;
            this.m_DataRate.Text = "label6";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 341);
            this.Controls.Add(this.m_DataRate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_Version);
            this.Controls.Add(this.m_Path);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.m_Connected);
            this.Controls.Add(this.m_Debug);
            this.Controls.Add(this.m_SendData);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_SendData;
        private System.Windows.Forms.TextBox m_Debug;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label m_Connected;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label m_Path;
        private System.Windows.Forms.Label m_Version;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label m_DataRate;
    }
}

