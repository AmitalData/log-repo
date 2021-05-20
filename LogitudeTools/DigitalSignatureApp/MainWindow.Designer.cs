
namespace Cloud.Sign.App
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ni = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.cobTenants = new System.Windows.Forms.ComboBox();
            this.PNLLoginInfo = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PNLTenantInfo = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnContinue = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.PNLLoginInfo.SuspendLayout();
            this.PNLTenantInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // ni
            // 
            this.ni.Text = "notifyIcon1";
            this.ni.Visible = true;
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
            this.pictureBox1.Location = new System.Drawing.Point(237, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(148, 59);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 18);
            this.label1.TabIndex = 1;
            if (Environment == "DSV")
            {
                this.label1.Text = "DSV Sign Client";
            }
            else
            {
                this.label1.Text = "LogBox Sign Client";
            }
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(302, 186);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(83, 30);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(213, 186);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.ForeColor = System.Drawing.Color.Red;
            this.lblError.Location = new System.Drawing.Point(13, 195);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(0, 13);
            this.lblError.TabIndex = 8;
            this.lblError.Visible = false;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // cobTenants
            // 
            this.cobTenants.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cobTenants.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cobTenants.FormattingEnabled = true;
            this.cobTenants.Location = new System.Drawing.Point(115, 34);
            this.cobTenants.Name = "cobTenants";
            this.cobTenants.Size = new System.Drawing.Size(210, 21);
            this.cobTenants.TabIndex = 9;
            this.cobTenants.SelectedIndexChanged += new System.EventHandler(this.cobTenants_SelectedIndexChanged);
            // 
            // PNLLoginInfo
            // 
            this.PNLLoginInfo.Controls.Add(this.txtPassword);
            this.PNLLoginInfo.Controls.Add(this.txtEmail);
            this.PNLLoginInfo.Controls.Add(this.label3);
            this.PNLLoginInfo.Controls.Add(this.label2);
            this.PNLLoginInfo.Location = new System.Drawing.Point(12, 80);
            this.PNLLoginInfo.Name = "PNLLoginInfo";
            this.PNLLoginInfo.Size = new System.Drawing.Size(376, 100);
            this.PNLLoginInfo.TabIndex = 10;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(159, 53);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(214, 20);
            this.txtPassword.TabIndex = 2;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(159, 27);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(214, 20);
            this.txtEmail.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label3.Location = new System.Drawing.Point(0, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label2.Location = new System.Drawing.Point(0, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Email";
            // 
            // PNLTenantInfo
            // 
            this.PNLTenantInfo.Controls.Add(this.label4);
            this.PNLTenantInfo.Controls.Add(this.cobTenants);
            this.PNLTenantInfo.Location = new System.Drawing.Point(16, 77);
            this.PNLTenantInfo.Name = "PNLTenantInfo";
            this.PNLTenantInfo.Size = new System.Drawing.Size(372, 78);
            this.PNLTenantInfo.TabIndex = 11;
            this.PNLTenantInfo.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label4.Location = new System.Drawing.Point(15, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 18);
            this.label4.TabIndex = 10;
            this.label4.Text = "Tenant";
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(302, 186);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(83, 30);
            this.btnContinue.TabIndex = 11;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Visible = false;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(397, 221);
            this.Controls.Add(this.PNLTenantInfo);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.PNLLoginInfo);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);

            this.MaximizeBox = false;
            this.Name = "MainWindow";
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Resize += new System.EventHandler(this.MainWindow_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.PNLLoginInfo.ResumeLayout(false);
            this.PNLLoginInfo.PerformLayout();
            this.PNLTenantInfo.ResumeLayout(false);
            this.PNLTenantInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon ni;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ComboBox cobTenants;
        private System.Windows.Forms.Panel PNLLoginInfo;
        private System.Windows.Forms.Panel PNLTenantInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnContinue;
    }
}