
namespace TestTenantConfiguration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.CreateTenant = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TenantEmailTextBox = new System.Windows.Forms.TextBox();
            this.TenantCompanyTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.OldPassword = new System.Windows.Forms.Label();
            this.NewPasswordText = new System.Windows.Forms.TextBox();
            this.ValidateCopy = new System.Windows.Forms.Label();
            this.TenantEmailValidation = new System.Windows.Forms.Label();
            this.TenantCompanyValidation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CreateTenant
            // 
            this.CreateTenant.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateTenant.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CreateTenant.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateTenant.Location = new System.Drawing.Point(177, 281);
            this.CreateTenant.Name = "CreateTenant";
            this.CreateTenant.Size = new System.Drawing.Size(210, 55);
            this.CreateTenant.TabIndex = 0;
            this.CreateTenant.Text = "Create Tenant";
            this.CreateTenant.UseVisualStyleBackColor = false;
            this.CreateTenant.Click += new System.EventHandler(this.CreateTenant_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Email:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(36, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(224, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tenant Company:";
            // 
            // TenantEmailTextBox
            // 
            this.TenantEmailTextBox.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TenantEmailTextBox.Location = new System.Drawing.Point(301, 39);
            this.TenantEmailTextBox.Name = "TenantEmailTextBox";
            this.TenantEmailTextBox.Size = new System.Drawing.Size(313, 34);
            this.TenantEmailTextBox.TabIndex = 3;
            this.TenantEmailTextBox.TextChanged += new System.EventHandler(this.TenantEmailTextBox_TextChanged);
            this.TenantEmailTextBox.LostFocus += new System.EventHandler(this.Validation_LostFocus);
            // 
            // TenantCompanyTextBox
            // 
            this.TenantCompanyTextBox.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TenantCompanyTextBox.Location = new System.Drawing.Point(301, 113);
            this.TenantCompanyTextBox.Name = "TenantCompanyTextBox";
            this.TenantCompanyTextBox.Size = new System.Drawing.Size(313, 34);
            this.TenantCompanyTextBox.TabIndex = 4;
            this.TenantCompanyTextBox.TextChanged += new System.EventHandler(this.TenantCompanyTextBox_TextChanged);
            this.TenantCompanyTextBox.LostFocus += new System.EventHandler(this.Validation_LostFocus);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(36, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 29);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password:";
            // 
            // OldPassword
            // 
            this.OldPassword.AutoSize = true;
            this.OldPassword.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OldPassword.Location = new System.Drawing.Point(37, 394);
            this.OldPassword.Name = "OldPassword";
            this.OldPassword.Size = new System.Drawing.Size(119, 19);
            this.OldPassword.TabIndex = 7;
            this.OldPassword.Text = "Old Password";
            // 
            // NewPasswordText
            // 
            this.NewPasswordText.BackColor = this.BackColor;
            this.NewPasswordText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.NewPasswordText.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewPasswordText.Location = new System.Drawing.Point(301, 192);
            this.NewPasswordText.Name = "NewPasswordText";
            this.NewPasswordText.ReadOnly = true;
            this.NewPasswordText.Size = new System.Drawing.Size(243, 25);
            this.NewPasswordText.TabIndex = 8;
            this.NewPasswordText.TabStop = false;
            this.NewPasswordText.Text = "New Password";
            // 
            // ValidateCopy
            // 
            this.ValidateCopy.AutoSize = true;
            this.ValidateCopy.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValidateCopy.ForeColor = System.Drawing.Color.ForestGreen;
            this.ValidateCopy.Location = new System.Drawing.Point(301, 220);
            this.ValidateCopy.Name = "ValidateCopy";
            this.ValidateCopy.Size = new System.Drawing.Size(0, 19);
            this.ValidateCopy.TabIndex = 9;
            // 
            // TenantEmailValidation
            // 
            this.TenantEmailValidation.AutoSize = true;
            this.TenantEmailValidation.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TenantEmailValidation.ForeColor = System.Drawing.Color.Red;
            this.TenantEmailValidation.Location = new System.Drawing.Point(301, 76);
            this.TenantEmailValidation.Name = "TenantEmailValidation";
            this.TenantEmailValidation.Size = new System.Drawing.Size(0, 19);
            this.TenantEmailValidation.TabIndex = 10;
            // 
            // TenantCompanyValidation
            // 
            this.TenantCompanyValidation.AutoSize = true;
            this.TenantCompanyValidation.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TenantCompanyValidation.ForeColor = System.Drawing.Color.Red;
            this.TenantCompanyValidation.Location = new System.Drawing.Point(301, 150);
            this.TenantCompanyValidation.Name = "TenantCompanyValidation";
            this.TenantCompanyValidation.Size = new System.Drawing.Size(0, 19);
            this.TenantCompanyValidation.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 440);
            this.Controls.Add(this.TenantCompanyValidation);
            this.Controls.Add(this.TenantEmailValidation);
            this.Controls.Add(this.ValidateCopy);
            this.Controls.Add(this.NewPasswordText);
            this.Controls.Add(this.OldPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TenantCompanyTextBox);
            this.Controls.Add(this.TenantEmailTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CreateTenant);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Tenant Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CreateTenant;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TenantEmailTextBox;
        private System.Windows.Forms.TextBox TenantCompanyTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label OldPassword;
        private System.Windows.Forms.TextBox NewPasswordText;
        private System.Windows.Forms.Label ValidateCopy;
        private System.Windows.Forms.Label TenantCompanyValidation;
        private System.Windows.Forms.Label TenantEmailValidation;
    }
}

