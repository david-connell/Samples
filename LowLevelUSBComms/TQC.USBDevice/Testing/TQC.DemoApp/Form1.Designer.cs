namespace TQC.DemoApp
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
            this.m_Start = new System.Windows.Forms.Button();
            this.m_Close = new System.Windows.Forms.Button();
            this.m_Open = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.mk_OKLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_CancelLabel = new System.Windows.Forms.Label();
            this.m_DeviceArrived = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_Start
            // 
            this.m_Start.Location = new System.Drawing.Point(123, 27);
            this.m_Start.Name = "m_Start";
            this.m_Start.Size = new System.Drawing.Size(75, 23);
            this.m_Start.TabIndex = 0;
            this.m_Start.Text = "Start";
            this.m_Start.UseVisualStyleBackColor = true;
            this.m_Start.Click += new System.EventHandler(this.m_Start_Click);
            // 
            // m_Close
            // 
            this.m_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_Close.Location = new System.Drawing.Point(488, 227);
            this.m_Close.Name = "m_Close";
            this.m_Close.Size = new System.Drawing.Size(75, 23);
            this.m_Close.TabIndex = 1;
            this.m_Close.Text = "Close";
            this.m_Close.UseVisualStyleBackColor = true;
            this.m_Close.Click += new System.EventHandler(this.m_Close_Click);
            // 
            // m_Open
            // 
            this.m_Open.Location = new System.Drawing.Point(31, 27);
            this.m_Open.Name = "m_Open";
            this.m_Open.Size = new System.Drawing.Size(75, 23);
            this.m_Open.TabIndex = 2;
            this.m_Open.Text = "Open";
            this.m_Open.UseVisualStyleBackColor = true;
            this.m_Open.Click += new System.EventHandler(this.m_Open_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "OK:";
            // 
            // mk_OKLabel
            // 
            this.mk_OKLabel.AutoSize = true;
            this.mk_OKLabel.Location = new System.Drawing.Point(87, 88);
            this.mk_OKLabel.Name = "mk_OKLabel";
            this.mk_OKLabel.Size = new System.Drawing.Size(35, 13);
            this.mk_OKLabel.TabIndex = 4;
            this.mk_OKLabel.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Cancel:";
            // 
            // m_CancelLabel
            // 
            this.m_CancelLabel.AutoSize = true;
            this.m_CancelLabel.Location = new System.Drawing.Point(87, 111);
            this.m_CancelLabel.Name = "m_CancelLabel";
            this.m_CancelLabel.Size = new System.Drawing.Size(35, 13);
            this.m_CancelLabel.TabIndex = 6;
            this.m_CancelLabel.Text = "label4";
            // 
            // m_DeviceArrived
            // 
            this.m_DeviceArrived.Location = new System.Drawing.Point(264, 26);
            this.m_DeviceArrived.Name = "m_DeviceArrived";
            this.m_DeviceArrived.Size = new System.Drawing.Size(132, 23);
            this.m_DeviceArrived.TabIndex = 7;
            this.m_DeviceArrived.Text = "DeviceArrivedMessage";
            this.m_DeviceArrived.UseVisualStyleBackColor = true;
            this.m_DeviceArrived.Click += new System.EventHandler(this.m_DeviceArrived_Click);
            // 
            // Form1
            // 
            this.AcceptButton = this.m_Close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_Close;
            this.ClientSize = new System.Drawing.Size(575, 262);
            this.Controls.Add(this.m_DeviceArrived);
            this.Controls.Add(this.m_CancelLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.mk_OKLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_Open);
            this.Controls.Add(this.m_Close);
            this.Controls.Add(this.m_Start);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_Start;
        private System.Windows.Forms.Button m_Close;
        private System.Windows.Forms.Button m_Open;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label mk_OKLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label m_CancelLabel;
        private System.Windows.Forms.Button m_DeviceArrived;
    }
}

