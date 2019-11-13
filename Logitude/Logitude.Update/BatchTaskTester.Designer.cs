namespace Logitude.Update
{
    partial class BatchTaskTester
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
            this.OkButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.BatchTaskIdTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TenantTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.MessagesTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(548, 344);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "Ok";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(467, 344);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // BatchTaskIdTextBox
            // 
            this.BatchTaskIdTextBox.Location = new System.Drawing.Point(120, 37);
            this.BatchTaskIdTextBox.Name = "BatchTaskIdTextBox";
            this.BatchTaskIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.BatchTaskIdTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Batch Task Id";
            // 
            // TenantTextBox
            // 
            this.TenantTextBox.Location = new System.Drawing.Point(120, 75);
            this.TenantTextBox.Name = "TenantTextBox";
            this.TenantTextBox.Size = new System.Drawing.Size(100, 20);
            this.TenantTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Tenant";
            // 
            // MessagesTextBox
            // 
            this.MessagesTextBox.Location = new System.Drawing.Point(25, 123);
            this.MessagesTextBox.Multiline = true;
            this.MessagesTextBox.Name = "MessagesTextBox";
            this.MessagesTextBox.Size = new System.Drawing.Size(561, 215);
            this.MessagesTextBox.TabIndex = 6;
            // 
            // BatchTaskTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 379);
            this.Controls.Add(this.MessagesTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TenantTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BatchTaskIdTextBox);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OkButton);
            this.Name = "BatchTaskTester";
            this.Text = "BatchTaskTester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.TextBox BatchTaskIdTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TenantTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox MessagesTextBox;
    }
}