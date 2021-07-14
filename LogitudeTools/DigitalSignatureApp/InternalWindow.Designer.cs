namespace Cloud.Sign.App
{
    partial class InternalWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InternalWindow));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLoggedEmail = new System.Windows.Forms.Label();
            this.lblLoggedCompany = new System.Windows.Forms.Label(); 
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabelPassWordRequired = new System.Windows.Forms.Button();
            this.btnChooseCert = new System.Windows.Forms.Button(); 
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBoxConnected = new System.Windows.Forms.PictureBox();
            this.pictureBoxCLoudStatus = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ni = new System.Windows.Forms.NotifyIcon(this.components);
            this.btnMinimize = new System.Windows.Forms.Button();
            this.lblCertName = new System.Windows.Forms.Label();
            this.lblCertDesc = new System.Windows.Forms.Label();
            this.lblCloudStatus = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBoxDisconnected = new System.Windows.Forms.PictureBox();
            this.pictureBoxWarning = new System.Windows.Forms.PictureBox();
            this.lblLastSigned = new System.Windows.Forms.Label();
            this.pictureBoxCLoudStatusDisConn = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxConnected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCLoudStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisconnected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCLoudStatusDisConn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            if (Environment == "DSV")
            {
                this.pictureBox1.Image = global::Cloud.Sign.App.Properties.Resources.HeaderLogo;
            }
            else
            {
                this.pictureBox1.Image = global::Cloud.Sign.App.Properties.Resources.LogBox;
            }
            this.pictureBox1.Location = new System.Drawing.Point(253, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(148, 59);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 18);
            this.label1.TabIndex = 2;
            if (Environment == "DSV")
            {
                this.label1.Text = "DSV Sign Client - Beta";
            }
            else
            {
                this.label1.Text = "LogBox Sign Client - Beta";
            }
            // 
            // lblLoggedEmail
            // 
            this.lblLoggedEmail.AutoSize = true;
            this.lblLoggedEmail.Location = new System.Drawing.Point(15, 94);
            this.lblLoggedEmail.Name = "lblLoggedEmail";
            this.lblLoggedEmail.Size = new System.Drawing.Size(141, 13);
            this.lblLoggedEmail.TabIndex = 3;
            //this.lblLoggedEmail.Text = "ahmada@logitudeworld.com";
            // 
            // lblLoggedEmail
            // 
            this.lblLoggedCompany.AutoSize = true;
            this.lblLoggedCompany.Location = new System.Drawing.Point(180, 94);
            this.lblLoggedCompany.Name = "lblLoggedCompany";
            this.lblLoggedCompany.Size = new System.Drawing.Size(141, 13);
            this.lblLoggedCompany.TabIndex = 3;
            
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.DeepSkyBlue;
            this.linkLabel1.Location = new System.Drawing.Point(326, 94);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(45, 13);
            this.linkLabel1.TabIndex = 4;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Log Out";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);

            // 
            // linkLabelPassWordRequired
            // 
            /*
              this.btnMinimize.Location = new System.Drawing.Point(326, 213);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(75, 23);
            this.btnMinimize.TabIndex = 11;
            this.btnMinimize.Text = "Minimize";
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
             btnChooseCert*/
            //this.linkLabelPassWordRequired.AutoSize = true;
            //this.linkLabelPassWordRequired.LinkColor = System.Drawing.Color.DeepSkyBlue;
            this.linkLabelPassWordRequired.Location = new System.Drawing.Point(326, 131);
            this.linkLabelPassWordRequired.Name = "linkLabelPassWordRequired";
            this.linkLabelPassWordRequired.Size = new System.Drawing.Size(75, 23);
            this.linkLabelPassWordRequired.TabIndex = 4;
            this.linkLabelPassWordRequired.UseVisualStyleBackColor = true;
            this.linkLabelPassWordRequired.Text = "Activate";
            this.linkLabelPassWordRequired.Visible = false;
            this.linkLabelPassWordRequired.Click += new System.EventHandler(this.linkLabelPassWordRequired_LinkClicked);
            // 
            // btnChooseCert
            // 
            this.btnChooseCert.Location = new System.Drawing.Point(326, 131);
            this.btnChooseCert.Name = "btnChooseCert";
            this.btnChooseCert.Size = new System.Drawing.Size(75, 23);
            this.btnChooseCert.TabIndex = 4;
            this.btnChooseCert.UseVisualStyleBackColor = true;
            this.btnChooseCert.Text = "Activate";
            this.btnChooseCert.Visible = false;
            this.btnChooseCert.Click += new System.EventHandler(this.btnChooseCert_Clicked);

            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Cloud.Sign.App.Properties.Resources.DocumentSignature;
            this.pictureBox2.Location = new System.Drawing.Point(21, 200);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(18, 22);
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBoxConnected
            // 
            this.pictureBoxConnected.Image = global::Cloud.Sign.App.Properties.Resources.OK;
            this.pictureBoxConnected.Location = new System.Drawing.Point(21, 129);
            this.pictureBoxConnected.Name = "pictureBoxConnected";
            this.pictureBoxConnected.Size = new System.Drawing.Size(18, 25);
            this.pictureBoxConnected.TabIndex = 6;
            this.pictureBoxConnected.TabStop = false;
            // 
            // pictureBoxCLoudStatus
            // 
            this.pictureBoxCLoudStatus.Image = global::Cloud.Sign.App.Properties.Resources.OK;
            this.pictureBoxCLoudStatus.Location = new System.Drawing.Point(21, 167);
            this.pictureBoxCLoudStatus.Name = "pictureBoxCLoudStatus";
            this.pictureBoxCLoudStatus.Size = new System.Drawing.Size(18, 29);
            this.pictureBoxCLoudStatus.TabIndex = 7;
            this.pictureBoxCLoudStatus.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(45, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Card Status";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(45, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Cloud Status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(45, 200);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Last signed";
            // 
            // ni
            // 
            this.ni.Text = "ni";
            this.ni.Visible = true;
            // 
            // btnMinimize
            // 
            this.btnMinimize.Location = new System.Drawing.Point(326, 213);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(75, 23);
            this.btnMinimize.TabIndex = 11;
            this.btnMinimize.Text = "Minimize";
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // lblCertName
            // 
            this.lblCertName.AutoSize = true;
            this.lblCertName.Location = new System.Drawing.Point(133, 131);
            this.lblCertName.Name = "lblCertName";
            this.lblCertName.Size = new System.Drawing.Size(0, 13);
            this.lblCertName.TabIndex = 12;

            // 
            // lblCertDesc
            // 
            this.lblCertDesc.AutoSize = true;
            this.lblCertDesc.Location = new System.Drawing.Point(133, 147);
            this.lblCertDesc.Name = "lblCertDesc";
            this.lblCertDesc.Size = new System.Drawing.Size(0, 13);
            this.lblCertDesc.TabIndex = 12;

            // 
            // lblCloudStatus
            // 
            this.lblCloudStatus.AutoSize = true;
            this.lblCloudStatus.Location = new System.Drawing.Point(133, 169);
            this.lblCloudStatus.Name = "lblCloudStatus";
            this.lblCloudStatus.Size = new System.Drawing.Size(0, 13);
            this.lblCloudStatus.TabIndex = 13;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // pictureBoxDisconnected
            // 
            this.pictureBoxDisconnected.Image = global::Cloud.Sign.App.Properties.Resources.Disconnected;
            this.pictureBoxDisconnected.Location = new System.Drawing.Point(21, 132);
            this.pictureBoxDisconnected.Name = "pictureBoxDisconnected";
            this.pictureBoxDisconnected.Size = new System.Drawing.Size(22, 25);
            this.pictureBoxDisconnected.TabIndex = 14;
            this.pictureBoxDisconnected.TabStop = false;
            this.pictureBoxDisconnected.Visible = false;

            // 
            // pictureBoxWarning
            // 
            this.pictureBoxWarning.Image = global::Cloud.Sign.App.Properties.Resources.Warning;
            this.pictureBoxWarning.Location = new System.Drawing.Point(21, 132);
            this.pictureBoxWarning.Name = "pictureBoxWarning";
            this.pictureBoxWarning.Size = new System.Drawing.Size(22, 25);
            this.pictureBoxWarning.TabIndex = 14;
            this.pictureBoxWarning.TabStop = false;
            this.pictureBoxWarning.Visible = false;

            // 
            // lblLastSigned
            // 
            this.lblLastSigned.AutoSize = true;
            this.lblLastSigned.Location = new System.Drawing.Point(134, 200);
            this.lblLastSigned.Name = "lblLastSigned";
            this.lblLastSigned.Size = new System.Drawing.Size(0, 13);
            this.lblLastSigned.TabIndex = 16;
            // 
            // pictureBoxCLoudStatusDisConn
            // 
            this.pictureBoxCLoudStatusDisConn.Image = global::Cloud.Sign.App.Properties.Resources.Disconnected;
            this.pictureBoxCLoudStatusDisConn.Location = new System.Drawing.Point(20, 167);
            this.pictureBoxCLoudStatusDisConn.Name = "pictureBoxCLoudStatusDisConn";
            this.pictureBoxCLoudStatusDisConn.Size = new System.Drawing.Size(22, 25);
            this.pictureBoxCLoudStatusDisConn.TabIndex = 15;
            this.pictureBoxCLoudStatusDisConn.TabStop = false;
            this.pictureBoxCLoudStatusDisConn.Visible = false;
            // 
            // InternalWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(413, 248);
            this.Controls.Add(this.lblLastSigned);
            this.Controls.Add(this.pictureBoxCLoudStatusDisConn);
            this.Controls.Add(this.pictureBoxDisconnected);
            this.Controls.Add(this.pictureBoxWarning);
            this.Controls.Add(this.lblCloudStatus);
            this.Controls.Add(this.lblCertName);
            this.Controls.Add(this.lblCertDesc); 
            this.Controls.Add(this.btnMinimize);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBoxCLoudStatus);
            this.Controls.Add(this.pictureBoxConnected);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.lblLoggedEmail);
            this.Controls.Add(this.lblLoggedCompany); 
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.linkLabelPassWordRequired);
            this.Controls.Add(this.btnChooseCert);
            

            this.MaximizeBox = false;
            this.Name = "InternalWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            if (Environment == "DSV")
            {
                this.Text = "DSV Sign App";
                this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.DsvIcon")));
            }
            else
            {
                this.Text = "LogBox Sign App";
                this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            } 
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InternalWindow_FormClosing);
            this.Load += new System.EventHandler(this.InternalWindow_Load);
            this.Resize += new System.EventHandler(this.InternalWindow_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxConnected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCLoudStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisconnected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCLoudStatusDisConn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLoggedEmail;
        private System.Windows.Forms.Label lblLoggedCompany;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button linkLabelPassWordRequired;
        private System.Windows.Forms.Button btnChooseCert;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBoxConnected;
        private System.Windows.Forms.PictureBox pictureBoxCLoudStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NotifyIcon ni;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Label lblCertName;
        private System.Windows.Forms.Label lblCertDesc;
        private System.Windows.Forms.Label lblCloudStatus;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.PictureBox pictureBoxDisconnected;
        private System.Windows.Forms.Label lblLastSigned;
        private System.Windows.Forms.PictureBox pictureBoxCLoudStatusDisConn;
        private System.Windows.Forms.PictureBox pictureBoxWarning;
    }
}