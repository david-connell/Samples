namespace CommunicationsWithEctron
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
            this.m_Temp = new System.Windows.Forms.Label();
            this.m_Voltage = new System.Windows.Forms.Label();
            this.m_ReadTemp = new System.Windows.Forms.Button();
            this.m_ReadVoltage = new System.Windows.Forms.Button();
            this.m_Finish = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_Name = new System.Windows.Forms.TextBox();
            this.m_Trace = new System.Windows.Forms.TextBox();
            this.m_SerialNumber = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // m_Temp
            // 
            this.m_Temp.AutoSize = true;
            this.m_Temp.Location = new System.Drawing.Point(156, 126);
            this.m_Temp.Name = "m_Temp";
            this.m_Temp.Size = new System.Drawing.Size(35, 13);
            this.m_Temp.TabIndex = 8;
            this.m_Temp.Text = "label1";
            // 
            // m_Voltage
            // 
            this.m_Voltage.AutoSize = true;
            this.m_Voltage.Location = new System.Drawing.Point(156, 97);
            this.m_Voltage.Name = "m_Voltage";
            this.m_Voltage.Size = new System.Drawing.Size(35, 13);
            this.m_Voltage.TabIndex = 7;
            this.m_Voltage.Text = "label1";
            // 
            // m_ReadTemp
            // 
            this.m_ReadTemp.Location = new System.Drawing.Point(12, 121);
            this.m_ReadTemp.Name = "m_ReadTemp";
            this.m_ReadTemp.Size = new System.Drawing.Size(137, 23);
            this.m_ReadTemp.TabIndex = 6;
            this.m_ReadTemp.Text = "Read Temperature";
            this.m_ReadTemp.UseVisualStyleBackColor = true;
            this.m_ReadTemp.Click += new System.EventHandler(this.m_ReadTemp_Click);
            // 
            // m_ReadVoltage
            // 
            this.m_ReadVoltage.Location = new System.Drawing.Point(12, 92);
            this.m_ReadVoltage.Name = "m_ReadVoltage";
            this.m_ReadVoltage.Size = new System.Drawing.Size(137, 23);
            this.m_ReadVoltage.TabIndex = 5;
            this.m_ReadVoltage.Text = "Read Voltage";
            this.m_ReadVoltage.UseVisualStyleBackColor = true;
            this.m_ReadVoltage.Click += new System.EventHandler(this.m_ReadVoltage_Click);
            // 
            // m_Finish
            // 
            this.m_Finish.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_Finish.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_Finish.Location = new System.Drawing.Point(390, 227);
            this.m_Finish.Name = "m_Finish";
            this.m_Finish.Size = new System.Drawing.Size(75, 23);
            this.m_Finish.TabIndex = 9;
            this.m_Finish.Text = "Finished";
            this.m_Finish.UseVisualStyleBackColor = true;
            this.m_Finish.Click += new System.EventHandler(this.m_Finish_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Serial Number:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "CalibrationTrace:";
            // 
            // m_Name
            // 
            this.m_Name.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_Name.Location = new System.Drawing.Point(110, 13);
            this.m_Name.Name = "m_Name";
            this.m_Name.ReadOnly = true;
            this.m_Name.Size = new System.Drawing.Size(355, 20);
            this.m_Name.TabIndex = 13;
            // 
            // m_Trace
            // 
            this.m_Trace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_Trace.Location = new System.Drawing.Point(110, 65);
            this.m_Trace.Name = "m_Trace";
            this.m_Trace.ReadOnly = true;
            this.m_Trace.Size = new System.Drawing.Size(355, 20);
            this.m_Trace.TabIndex = 14;
            // 
            // m_SerialNumber
            // 
            this.m_SerialNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_SerialNumber.Location = new System.Drawing.Point(110, 39);
            this.m_SerialNumber.Name = "m_SerialNumber";
            this.m_SerialNumber.ReadOnly = true;
            this.m_SerialNumber.Size = new System.Drawing.Size(355, 20);
            this.m_SerialNumber.TabIndex = 15;
            // 
            // Form1
            // 
            this.AcceptButton = this.m_Finish;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_Finish;
            this.ClientSize = new System.Drawing.Size(477, 262);
            this.Controls.Add(this.m_SerialNumber);
            this.Controls.Add(this.m_Trace);
            this.Controls.Add(this.m_Name);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_Finish);
            this.Controls.Add(this.m_Temp);
            this.Controls.Add(this.m_Voltage);
            this.Controls.Add(this.m_ReadTemp);
            this.Controls.Add(this.m_ReadVoltage);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_Temp;
        private System.Windows.Forms.Label m_Voltage;
        private System.Windows.Forms.Button m_ReadTemp;
        private System.Windows.Forms.Button m_ReadVoltage;
        private System.Windows.Forms.Button m_Finish;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_Name;
        private System.Windows.Forms.TextBox m_Trace;
        private System.Windows.Forms.TextBox m_SerialNumber;
    }
}

