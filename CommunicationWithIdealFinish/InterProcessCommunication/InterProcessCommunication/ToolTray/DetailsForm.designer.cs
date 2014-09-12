namespace TQC.GOC.InterProcessCommunication
{
    partial class DetailsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailsForm));
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.label = new System.Windows.Forms.Label();
            this.m_ConnectionStatus = new System.Windows.Forms.Label();
            this.m_TimeSinceLastPing = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_Version = new System.Windows.Forms.Label();
            this.m_Path = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.m_ProtocolErrors = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.m_DataRate = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.m_ServerState = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.m_NumberOfSamples = new System.Windows.Forms.Label();
            this.m_QueueSize = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(0, 133);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(563, 72);
            this.dataGridView.TabIndex = 0;
            // 
            // label
            // 
            this.label.Location = new System.Drawing.Point(11, 9);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(100, 13);
            this.label.TabIndex = 1;
            this.label.Text = "Connection status:";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_ConnectionStatus
            // 
            this.m_ConnectionStatus.AutoSize = true;
            this.m_ConnectionStatus.Location = new System.Drawing.Point(109, 9);
            this.m_ConnectionStatus.Name = "m_ConnectionStatus";
            this.m_ConnectionStatus.Size = new System.Drawing.Size(51, 13);
            this.m_ConnectionStatus.TabIndex = 2;
            this.m_ConnectionStatus.Text = "Unknown";
            // 
            // m_TimeSinceLastPing
            // 
            this.m_TimeSinceLastPing.AutoSize = true;
            this.m_TimeSinceLastPing.Location = new System.Drawing.Point(287, 9);
            this.m_TimeSinceLastPing.Name = "m_TimeSinceLastPing";
            this.m_TimeSinceLastPing.Size = new System.Drawing.Size(13, 13);
            this.m_TimeSinceLastPing.TabIndex = 17;
            this.m_TimeSinceLastPing.Text = "0";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(179, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Time from last ping:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_Version
            // 
            this.m_Version.AutoSize = true;
            this.m_Version.Location = new System.Drawing.Point(109, 34);
            this.m_Version.Name = "m_Version";
            this.m_Version.Size = new System.Drawing.Size(51, 13);
            this.m_Version.TabIndex = 15;
            this.m_Version.Text = "Unknown";
            // 
            // m_Path
            // 
            this.m_Path.AutoSize = true;
            this.m_Path.Location = new System.Drawing.Point(110, 85);
            this.m_Path.Name = "m_Path";
            this.m_Path.Size = new System.Drawing.Size(51, 13);
            this.m_Path.TabIndex = 14;
            this.m_Path.Text = "Unknown";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(11, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Version:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Path:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 1000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // m_ProtocolErrors
            // 
            this.m_ProtocolErrors.AutoSize = true;
            this.m_ProtocolErrors.Location = new System.Drawing.Point(110, 60);
            this.m_ProtocolErrors.Name = "m_ProtocolErrors";
            this.m_ProtocolErrors.Size = new System.Drawing.Size(13, 13);
            this.m_ProtocolErrors.TabIndex = 19;
            this.m_ProtocolErrors.Text = "0";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Protocol errors:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_DataRate
            // 
            this.m_DataRate.AutoSize = true;
            this.m_DataRate.Location = new System.Drawing.Point(287, 34);
            this.m_DataRate.Name = "m_DataRate";
            this.m_DataRate.Size = new System.Drawing.Size(13, 13);
            this.m_DataRate.TabIndex = 21;
            this.m_DataRate.Text = "0";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(179, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Data rate:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(179, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Server State:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_ServerState
            // 
            this.m_ServerState.AutoSize = true;
            this.m_ServerState.Location = new System.Drawing.Point(287, 60);
            this.m_ServerState.Name = "m_ServerState";
            this.m_ServerState.Size = new System.Drawing.Size(51, 13);
            this.m_ServerState.TabIndex = 23;
            this.m_ServerState.Text = "Unknown";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(394, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(110, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Number Of Samples:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(353, 34);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(151, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Outstanding samples to send:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // m_NumberOfSamples
            // 
            this.m_NumberOfSamples.AutoSize = true;
            this.m_NumberOfSamples.Location = new System.Drawing.Point(505, 9);
            this.m_NumberOfSamples.Name = "m_NumberOfSamples";
            this.m_NumberOfSamples.Size = new System.Drawing.Size(13, 13);
            this.m_NumberOfSamples.TabIndex = 26;
            this.m_NumberOfSamples.Text = "0";
            // 
            // m_QueueSize
            // 
            this.m_QueueSize.AutoSize = true;
            this.m_QueueSize.Location = new System.Drawing.Point(505, 34);
            this.m_QueueSize.Name = "m_QueueSize";
            this.m_QueueSize.Size = new System.Drawing.Size(13, 13);
            this.m_QueueSize.TabIndex = 27;
            this.m_QueueSize.Text = "0";
            // 
            // DetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 204);
            this.Controls.Add(this.m_QueueSize);
            this.Controls.Add(this.m_NumberOfSamples);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.m_ServerState);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.m_DataRate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.m_ProtocolErrors);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.m_TimeSinceLastPing);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_Version);
            this.Controls.Add(this.m_Path);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_ConnectionStatus);
            this.Controls.Add(this.label);
            this.Controls.Add(this.dataGridView);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DetailsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Status";
            this.Load += new System.EventHandler(this.DetailsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label m_ConnectionStatus;
        private System.Windows.Forms.Label m_TimeSinceLastPing;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label m_Version;
        private System.Windows.Forms.Label m_Path;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label m_ProtocolErrors;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label m_DataRate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label m_ServerState;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label m_NumberOfSamples;
        private System.Windows.Forms.Label m_QueueSize;

    }
}

