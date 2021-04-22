
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.CreateTenantBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TenantEmailTextBox = new System.Windows.Forms.TextBox();
            this.TenantCompanyTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.NewPasswordText = new System.Windows.Forms.TextBox();
            this.ValidateCopy = new System.Windows.Forms.Label();
            this.TenantEmailValidation = new System.Windows.Forms.Label();
            this.TenantCompanyValidation = new System.Windows.Forms.Label();
            this.Timerlbl = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ValidatePrepareData = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CreateTenantBtn
            // 
            this.CreateTenantBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CreateTenantBtn.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CreateTenantBtn.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreateTenantBtn.Location = new System.Drawing.Point(177, 281);
            this.CreateTenantBtn.Name = "CreateTenantBtn";
            this.CreateTenantBtn.Size = new System.Drawing.Size(210, 55);
            this.CreateTenantBtn.TabIndex = 0;
            this.CreateTenantBtn.Text = "Create Tenant";
            this.CreateTenantBtn.UseVisualStyleBackColor = false;
            this.CreateTenantBtn.Click += new System.EventHandler(this.CreateTenantBtn_Click);
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
            this.TenantEmailTextBox.LostFocus += new System.EventHandler(this.TenantEmailTextBox_LostFocus);
            // 
            // TenantCompanyTextBox
            // 
            this.TenantCompanyTextBox.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TenantCompanyTextBox.Location = new System.Drawing.Point(301, 113);
            this.TenantCompanyTextBox.Name = "TenantCompanyTextBox";
            this.TenantCompanyTextBox.Size = new System.Drawing.Size(313, 34);
            this.TenantCompanyTextBox.TabIndex = 4;
            this.TenantCompanyTextBox.TextChanged += new System.EventHandler(this.TenantCompanyTextBox_TextChanged);
            this.TenantCompanyTextBox.LostFocus += new System.EventHandler(this.TenantCompanyTextBox_LostFocus);
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
            // Timerlbl
            // 
            this.Timerlbl.AutoSize = true;
            this.Timerlbl.Location = new System.Drawing.Point(407, 317);
            this.Timerlbl.Name = "Timerlbl";
            this.Timerlbl.Size = new System.Drawing.Size(24, 19);
            this.Timerlbl.TabIndex = 12;
            this.Timerlbl.Text = "...";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ValidatePrepareData
            // 
            this.ValidatePrepareData.AutoSize = true;
            this.ValidatePrepareData.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ValidatePrepareData.ForeColor = System.Drawing.Color.Green;
            this.ValidatePrepareData.Location = new System.Drawing.Point(301, 249);
            this.ValidatePrepareData.Name = "ValidatePrepareData";
            this.ValidatePrepareData.Size = new System.Drawing.Size(0, 19);
            this.ValidatePrepareData.TabIndex = 13;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 440);
            this.Controls.Add(this.ValidatePrepareData);
            this.Controls.Add(this.Timerlbl);
            this.Controls.Add(this.TenantCompanyValidation);
            this.Controls.Add(this.TenantEmailValidation);
            this.Controls.Add(this.ValidateCopy);
            this.Controls.Add(this.NewPasswordText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.TenantCompanyTextBox);
            this.Controls.Add(this.TenantEmailTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CreateTenantBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Tenant Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CreateTenantBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TenantEmailTextBox;
        private System.Windows.Forms.TextBox TenantCompanyTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox NewPasswordText;
        private System.Windows.Forms.Label ValidateCopy;
        private System.Windows.Forms.Label TenantCompanyValidation;
        private System.Windows.Forms.Label TenantEmailValidation;
        private System.Windows.Forms.Label Timerlbl;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label ValidatePrepareData;
    }
}

