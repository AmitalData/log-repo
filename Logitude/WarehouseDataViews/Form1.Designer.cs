namespace WarehouseDataViews
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
            this.ResultLabel = new System.Windows.Forms.Label();
            this.DeleteViewsButton = new System.Windows.Forms.Button();
            this.CreateViewsButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DestinationConnectionTextBox = new System.Windows.Forms.TextBox();
            this.SourceConnectionTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.TenantTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ResultLabel
            // 
            this.ResultLabel.AutoSize = true;
            this.ResultLabel.Location = new System.Drawing.Point(256, 179);
            this.ResultLabel.Name = "ResultLabel";
            this.ResultLabel.Size = new System.Drawing.Size(0, 13);
            this.ResultLabel.TabIndex = 13;
            // 
            // DeleteViewsButton
            // 
            this.DeleteViewsButton.Location = new System.Drawing.Point(141, 108);
            this.DeleteViewsButton.Name = "DeleteViewsButton";
            this.DeleteViewsButton.Size = new System.Drawing.Size(246, 23);
            this.DeleteViewsButton.TabIndex = 12;
            this.DeleteViewsButton.Text = "Delete Views";
            this.DeleteViewsButton.UseVisualStyleBackColor = true;
            this.DeleteViewsButton.Click += new System.EventHandler(this.DeleteViewsButton_Click);
            // 
            // CreateViewsButton
            // 
            this.CreateViewsButton.Location = new System.Drawing.Point(141, 137);
            this.CreateViewsButton.Name = "CreateViewsButton";
            this.CreateViewsButton.Size = new System.Drawing.Size(246, 23);
            this.CreateViewsButton.TabIndex = 11;
            this.CreateViewsButton.Text = "Create Views";
            this.CreateViewsButton.UseVisualStyleBackColor = true;
            this.CreateViewsButton.Click += new System.EventHandler(this.CreateViewsButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Destination Connection";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Source Connection";
            // 
            // DestinationConnectionTextBox
            // 
            this.DestinationConnectionTextBox.Location = new System.Drawing.Point(141, 46);
            this.DestinationConnectionTextBox.Name = "DestinationConnectionTextBox";
            this.DestinationConnectionTextBox.Size = new System.Drawing.Size(246, 20);
            this.DestinationConnectionTextBox.TabIndex = 8;
            this.DestinationConnectionTextBox.TextChanged += new System.EventHandler(this.DestinationConnectionTextBox_TextChanged);
            // 
            // SourceConnectionTextBox
            // 
            this.SourceConnectionTextBox.Location = new System.Drawing.Point(141, 20);
            this.SourceConnectionTextBox.Name = "SourceConnectionTextBox";
            this.SourceConnectionTextBox.Size = new System.Drawing.Size(246, 20);
            this.SourceConnectionTextBox.TabIndex = 7;
            this.SourceConnectionTextBox.TextChanged += new System.EventHandler(this.SourceConnectionTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Tenant";
            // 
            // TenantTextBox
            // 
            this.TenantTextBox.Location = new System.Drawing.Point(141, 71);
            this.TenantTextBox.Name = "TenantTextBox";
            this.TenantTextBox.Size = new System.Drawing.Size(246, 20);
            this.TenantTextBox.TabIndex = 14;
            this.TenantTextBox.TextChanged += new System.EventHandler(this.TenantTextBox_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 241);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TenantTextBox);
            this.Controls.Add(this.ResultLabel);
            this.Controls.Add(this.DeleteViewsButton);
            this.Controls.Add(this.CreateViewsButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DestinationConnectionTextBox);
            this.Controls.Add(this.SourceConnectionTextBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ResultLabel;
        private System.Windows.Forms.Button DeleteViewsButton;
        private System.Windows.Forms.Button CreateViewsButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox DestinationConnectionTextBox;
        private System.Windows.Forms.TextBox SourceConnectionTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TenantTextBox;
    }
}

