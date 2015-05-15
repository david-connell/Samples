namespace TQC.GOC.InterProcessCommunication.ToolTray
{
    partial class HelpAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpAbout));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.m_Name = new System.Windows.Forms.Label();
            this.m_Copyright = new System.Windows.Forms.Label();
            this.m_Version = new System.Windows.Forms.Label();
            this.m_OK = new System.Windows.Forms.Button();
            this.m_VersionInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(26, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 25);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(63, 13);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(26, 26);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // m_Name
            // 
            this.m_Name.AutoSize = true;
            this.m_Name.Location = new System.Drawing.Point(26, 54);
            this.m_Name.Name = "m_Name";
            this.m_Name.Size = new System.Drawing.Size(34, 13);
            this.m_Name.TabIndex = 2;
            this.m_Name.Text = "Name";
            // 
            // m_Copyright
            // 
            this.m_Copyright.AutoSize = true;
            this.m_Copyright.Location = new System.Drawing.Point(26, 71);
            this.m_Copyright.Name = "m_Copyright";
            this.m_Copyright.Size = new System.Drawing.Size(54, 13);
            this.m_Copyright.TabIndex = 3;
            this.m_Copyright.Text = "Copyright";
            // 
            // m_Version
            // 
            this.m_Version.AutoSize = true;
            this.m_Version.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.m_Version.Location = new System.Drawing.Point(26, 88);
            this.m_Version.Name = "m_Version";
            this.m_Version.Size = new System.Drawing.Size(42, 13);
            this.m_Version.TabIndex = 4;
            this.m_Version.Text = "Version";
            // 
            // m_OK
            // 
            this.m_OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.m_OK.Location = new System.Drawing.Point(439, 209);
            this.m_OK.Name = "m_OK";
            this.m_OK.Size = new System.Drawing.Size(75, 23);
            this.m_OK.TabIndex = 5;
            this.m_OK.Text = "OK";
            this.m_OK.UseVisualStyleBackColor = true;
            this.m_OK.Click += new System.EventHandler(this.m_OK_Click);
            // 
            // m_VersionInfo
            // 
            this.m_VersionInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_VersionInfo.Location = new System.Drawing.Point(26, 136);
            this.m_VersionInfo.Name = "m_VersionInfo";
            this.m_VersionInfo.Size = new System.Drawing.Size(488, 70);
            this.m_VersionInfo.TabIndex = 6;
            this.m_VersionInfo.Text = "label1";
            this.m_VersionInfo.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // HelpAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 244);
            this.Controls.Add(this.m_VersionInfo);
            this.Controls.Add(this.m_OK);
            this.Controls.Add(this.m_Version);
            this.Controls.Add(this.m_Copyright);
            this.Controls.Add(this.m_Name);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(542, 253);
            this.Name = "HelpAbout";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Help About";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label m_Name;
        private System.Windows.Forms.Label m_Copyright;
        private System.Windows.Forms.Label m_Version;
        private System.Windows.Forms.Button m_OK;
        private System.Windows.Forms.Label m_VersionInfo;
    }
}