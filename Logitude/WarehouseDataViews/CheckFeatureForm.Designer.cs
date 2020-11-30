namespace WarehouseDataViews
{
    partial class CheckFeatureForm
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
            this.FeatureNameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TenantTextBox = new System.Windows.Forms.Label();
            this.CheckFeatureButton = new System.Windows.Forms.Button();
            this.TenantNumberTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SourceStringtextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // FeatureNameTextBox
            // 
            this.FeatureNameTextBox.Location = new System.Drawing.Point(195, 71);
            this.FeatureNameTextBox.Name = "FeatureNameTextBox";
            this.FeatureNameTextBox.Size = new System.Drawing.Size(406, 22);
            this.FeatureNameTextBox.TabIndex = 0;
            this.FeatureNameTextBox.TextChanged += new System.EventHandler(this.FeatureNameTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Feature Name";
            // 
            // TenantTextBox
            // 
            this.TenantTextBox.AutoSize = true;
            this.TenantTextBox.Location = new System.Drawing.Point(62, 113);
            this.TenantTextBox.Name = "TenantTextBox";
            this.TenantTextBox.Size = new System.Drawing.Size(53, 17);
            this.TenantTextBox.TabIndex = 3;
            this.TenantTextBox.Text = "Tenant";
            // 
            // CheckFeatureButton
            // 
            this.CheckFeatureButton.Location = new System.Drawing.Point(257, 161);
            this.CheckFeatureButton.Name = "CheckFeatureButton";
            this.CheckFeatureButton.Size = new System.Drawing.Size(225, 23);
            this.CheckFeatureButton.TabIndex = 4;
            this.CheckFeatureButton.Text = "Check Feature";
            this.CheckFeatureButton.UseVisualStyleBackColor = true;
            this.CheckFeatureButton.Click += new System.EventHandler(this.CheckFeatureButton_Click);
            // 
            // TenantNumberTextBox
            // 
            this.TenantNumberTextBox.Location = new System.Drawing.Point(195, 113);
            this.TenantNumberTextBox.Name = "TenantNumberTextBox";
            this.TenantNumberTextBox.Size = new System.Drawing.Size(406, 22);
            this.TenantNumberTextBox.TabIndex = 5;
            this.TenantNumberTextBox.TextChanged += new System.EventHandler(this.TenantNumberTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Source";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // SourceStringtextBox
            // 
            this.SourceStringtextBox.Location = new System.Drawing.Point(195, 27);
            this.SourceStringtextBox.Name = "SourceStringtextBox";
            this.SourceStringtextBox.Size = new System.Drawing.Size(406, 22);
            this.SourceStringtextBox.TabIndex = 6;
            this.SourceStringtextBox.TextChanged += new System.EventHandler(this.SourceStringtextBox_TextChanged);
            // 
            // CheckFeatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SourceStringtextBox);
            this.Controls.Add(this.TenantNumberTextBox);
            this.Controls.Add(this.CheckFeatureButton);
            this.Controls.Add(this.TenantTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FeatureNameTextBox);
            this.Name = "CheckFeatureForm";
            this.Text = "CheckFeatureForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox FeatureNameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TenantTextBox;
        private System.Windows.Forms.Button CheckFeatureButton;
        private System.Windows.Forms.TextBox TenantNumberTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SourceStringtextBox;
    }
}