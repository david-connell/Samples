﻿namespace WindowsFormsApplication1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.m_SendData = new System.Windows.Forms.Button();
            this.m_Debug = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.m_SingleSample = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.m_SendSamples = new System.Windows.Forms.Button();
            this.SendSampleTimer = new System.Windows.Forms.Timer(this.components);
            this.m_EnableOutput = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
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
            // m_SingleSample
            // 
            this.m_SingleSample.Location = new System.Drawing.Point(106, 12);
            this.m_SingleSample.Name = "m_SingleSample";
            this.m_SingleSample.Size = new System.Drawing.Size(124, 23);
            this.m_SingleSample.TabIndex = 4;
            this.m_SingleSample.Text = "Send Sample";
            this.m_SingleSample.UseVisualStyleBackColor = true;
            this.m_SingleSample.Click += new System.EventHandler(this.SendSample);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(253, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(104, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Stop);
            // 
            // m_SendSamples
            // 
            this.m_SendSamples.Location = new System.Drawing.Point(106, 39);
            this.m_SendSamples.Name = "m_SendSamples";
            this.m_SendSamples.Size = new System.Drawing.Size(124, 23);
            this.m_SendSamples.TabIndex = 16;
            this.m_SendSamples.Text = "Start sending  samples";
            this.m_SendSamples.UseVisualStyleBackColor = true;
            this.m_SendSamples.Click += new System.EventHandler(this.button3_Click);
            // 
            // SendSampleTimer
            // 
            this.SendSampleTimer.Tick += new System.EventHandler(this.SendSampleTimer_Tick);
            // 
            // m_EnableOutput
            // 
            this.m_EnableOutput.AutoSize = true;
            this.m_EnableOutput.Checked = true;
            this.m_EnableOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_EnableOutput.Location = new System.Drawing.Point(12, 181);
            this.m_EnableOutput.Name = "m_EnableOutput";
            this.m_EnableOutput.Size = new System.Drawing.Size(92, 17);
            this.m_EnableOutput.TabIndex = 17;
            this.m_EnableOutput.Text = "Enable output";
            this.m_EnableOutput.UseVisualStyleBackColor = true;
            this.m_EnableOutput.CheckedChanged += new System.EventHandler(this.m_EnableOutput_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Speed of Sending data:";
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(147, 133);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(151, 45);
            this.trackBar1.TabIndex = 19;
            this.trackBar1.TickFrequency = 100;
            this.trackBar1.Value = 100;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 341);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_EnableOutput);
            this.Controls.Add(this.m_SendSamples);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.m_SingleSample);
            this.Controls.Add(this.m_Debug);
            this.Controls.Add(this.m_SendData);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_SendData;
        private System.Windows.Forms.TextBox m_Debug;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button m_SingleSample;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button m_SendSamples;
        private System.Windows.Forms.Timer SendSampleTimer;
        private System.Windows.Forms.CheckBox m_EnableOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBar1;
    }
}

