namespace ReportAndDocumentWRTesterTool
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
            this.RunReportsButton = new System.Windows.Forms.Button();
            this.TenantTextBox = new System.Windows.Forms.TextBox();
            this.TenantLabel = new System.Windows.Forms.Label();
            this.EmailTextBox = new System.Windows.Forms.TextBox();
            this.EmailLabel = new System.Windows.Forms.Label();
            this.NumberOfRecordsLabel = new System.Windows.Forms.Label();
            this.NumberOfRecordsText = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RunDocumentsButton = new System.Windows.Forms.Button();
            this.delaybtwlabel = new System.Windows.Forms.Label();
            this.DelayBTWText = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RunReportsButton
            // 
            this.RunReportsButton.Location = new System.Drawing.Point(33, 259);
            this.RunReportsButton.Name = "RunReportsButton";
            this.RunReportsButton.Size = new System.Drawing.Size(266, 53);
            this.RunReportsButton.TabIndex = 0;
            this.RunReportsButton.Text = "Run Reports";
            this.RunReportsButton.UseVisualStyleBackColor = true;
            this.RunReportsButton.Click += new System.EventHandler(this.RunReportsButton_Click);
            // 
            // TenantTextBox
            // 
            this.TenantTextBox.Location = new System.Drawing.Point(123, 82);
            this.TenantTextBox.Name = "TenantTextBox";
            this.TenantTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TenantTextBox.Size = new System.Drawing.Size(176, 22);
            this.TenantTextBox.TabIndex = 1;
            this.TenantTextBox.Text = "1";
            // 
            // TenantLabel
            // 
            this.TenantLabel.AutoSize = true;
            this.TenantLabel.Location = new System.Drawing.Point(30, 82);
            this.TenantLabel.Name = "TenantLabel";
            this.TenantLabel.Size = new System.Drawing.Size(53, 17);
            this.TenantLabel.TabIndex = 2;
            this.TenantLabel.Text = "Tenant";
            // 
            // EmailTextBox
            // 
            this.EmailTextBox.Location = new System.Drawing.Point(123, 37);
            this.EmailTextBox.Name = "EmailTextBox";
            this.EmailTextBox.Size = new System.Drawing.Size(176, 22);
            this.EmailTextBox.TabIndex = 3;
            this.EmailTextBox.Text = "admin@fnarsoft.com";
            // 
            // EmailLabel
            // 
            this.EmailLabel.AutoSize = true;
            this.EmailLabel.Location = new System.Drawing.Point(30, 40);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(42, 17);
            this.EmailLabel.TabIndex = 4;
            this.EmailLabel.Text = "Email";
            // 
            // NumberOfRecordsLabel
            // 
            this.NumberOfRecordsLabel.AutoSize = true;
            this.NumberOfRecordsLabel.Location = new System.Drawing.Point(30, 129);
            this.NumberOfRecordsLabel.Name = "NumberOfRecordsLabel";
            this.NumberOfRecordsLabel.Size = new System.Drawing.Size(89, 17);
            this.NumberOfRecordsLabel.TabIndex = 5;
            this.NumberOfRecordsLabel.Text = "# of Records";
            // 
            // NumberOfRecordsText
            // 
            this.NumberOfRecordsText.Location = new System.Drawing.Point(123, 126);
            this.NumberOfRecordsText.Name = "NumberOfRecordsText";
            this.NumberOfRecordsText.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.NumberOfRecordsText.Size = new System.Drawing.Size(176, 22);
            this.NumberOfRecordsText.TabIndex = 6;
            this.NumberOfRecordsText.Text = "10";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.DelayBTWText);
            this.panel1.Controls.Add(this.delaybtwlabel);
            this.panel1.Controls.Add(this.RunDocumentsButton);
            this.panel1.Controls.Add(this.RunReportsButton);
            this.panel1.Controls.Add(this.NumberOfRecordsText);
            this.panel1.Controls.Add(this.TenantTextBox);
            this.panel1.Controls.Add(this.NumberOfRecordsLabel);
            this.panel1.Controls.Add(this.TenantLabel);
            this.panel1.Controls.Add(this.EmailLabel);
            this.panel1.Controls.Add(this.EmailTextBox);
            this.panel1.Location = new System.Drawing.Point(33, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(329, 420);
            this.panel1.TabIndex = 7;
            // 
            // RunDocumentsButton
            // 
            this.RunDocumentsButton.Location = new System.Drawing.Point(33, 336);
            this.RunDocumentsButton.Name = "RunDocumentsButton";
            this.RunDocumentsButton.Size = new System.Drawing.Size(266, 53);
            this.RunDocumentsButton.TabIndex = 7;
            this.RunDocumentsButton.Text = "Run Documents (Hard Coded)";
            this.RunDocumentsButton.UseVisualStyleBackColor = true;
            this.RunDocumentsButton.Click += new System.EventHandler(this.RunDocumentsButton_Click);
            // 
            // delaybtwlabel
            // 
            this.delaybtwlabel.AutoSize = true;
            this.delaybtwlabel.Location = new System.Drawing.Point(30, 177);
            this.delaybtwlabel.Name = "delaybtwlabel";
            this.delaybtwlabel.Size = new System.Drawing.Size(99, 21);
            this.delaybtwlabel.TabIndex = 8;
            this.delaybtwlabel.Text = "Delay BTW";
            // 
            // DelayBTWText
            // 
            this.DelayBTWText.Location = new System.Drawing.Point(123, 174);
            this.DelayBTWText.Name = "DelayBTWText";
            this.DelayBTWText.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DelayBTWText.Size = new System.Drawing.Size(176, 22);
            this.DelayBTWText.TabIndex = 9;
            this.DelayBTWText.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(395, 483);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RunReportsButton;
        private System.Windows.Forms.TextBox TenantTextBox;
        private System.Windows.Forms.Label TenantLabel;
        private System.Windows.Forms.TextBox EmailTextBox;
        private System.Windows.Forms.Label EmailLabel;
        private System.Windows.Forms.Label NumberOfRecordsLabel;
        private System.Windows.Forms.TextBox NumberOfRecordsText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button RunDocumentsButton;
        private System.Windows.Forms.TextBox DelayBTWText;
        private System.Windows.Forms.Label delaybtwlabel;
    }
}

