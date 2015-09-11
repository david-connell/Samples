﻿namespace TestBounceApplication
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
            this.m_Connect = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.m_StartStop = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_RequestId = new System.Windows.Forms.Label();
            this.m_ResultBuffer = new System.Windows.Forms.Label();
            this.m_Exception = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_Connect
            // 
            this.m_Connect.Location = new System.Drawing.Point(12, 12);
            this.m_Connect.Name = "m_Connect";
            this.m_Connect.Size = new System.Drawing.Size(75, 23);
            this.m_Connect.TabIndex = 0;
            this.m_Connect.Text = "Connect";
            this.m_Connect.UseVisualStyleBackColor = true;
            this.m_Connect.Click += new System.EventHandler(this.m_Connect_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "GRO (VID 0403 PID 7B60)"});
            this.comboBox1.Location = new System.Drawing.Point(119, 13);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(166, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // m_StartStop
            // 
            this.m_StartStop.Enabled = false;
            this.m_StartStop.Location = new System.Drawing.Point(12, 67);
            this.m_StartStop.Name = "m_StartStop";
            this.m_StartStop.Size = new System.Drawing.Size(75, 23);
            this.m_StartStop.TabIndex = 2;
            this.m_StartStop.Text = "Start test";
            this.m_StartStop.UseVisualStyleBackColor = true;
            this.m_StartStop.Click += new System.EventHandler(this.m_StartStop_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.m_Exception);
            this.groupBox1.Controls.Add(this.m_ResultBuffer);
            this.groupBox1.Controls.Add(this.m_RequestId);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 96);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(583, 214);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Commands";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Request packet ID:";
            // 
            // m_RequestId
            // 
            this.m_RequestId.AutoSize = true;
            this.m_RequestId.Location = new System.Drawing.Point(145, 31);
            this.m_RequestId.Name = "m_RequestId";
            this.m_RequestId.Size = new System.Drawing.Size(13, 13);
            this.m_RequestId.TabIndex = 1;
            this.m_RequestId.Text = "0";
            // 
            // m_ResultBuffer
            // 
            this.m_ResultBuffer.AutoSize = true;
            this.m_ResultBuffer.Location = new System.Drawing.Point(152, 56);
            this.m_ResultBuffer.Name = "m_ResultBuffer";
            this.m_ResultBuffer.Size = new System.Drawing.Size(190, 13);
            this.m_ResultBuffer.TabIndex = 2;
            this.m_ResultBuffer.Text = "Result data from request goes here...";
            // 
            // m_Exception
            // 
            this.m_Exception.AutoSize = true;
            this.m_Exception.Location = new System.Drawing.Point(152, 87);
            this.m_Exception.Name = "m_Exception";
            this.m_Exception.Size = new System.Drawing.Size(99, 13);
            this.m_Exception.TabIndex = 3;
            this.m_Exception.Text = "Exceptions go here";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 322);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_StartStop);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.m_Connect);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form1";
            this.Text = "TQC USB Device Bounce";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button m_Connect;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button m_StartStop;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label m_RequestId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label m_ResultBuffer;
        private System.Windows.Forms.Label m_Exception;
    }
}

