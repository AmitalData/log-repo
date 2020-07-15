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
            this.label1 = new System.Windows.Forms.Label();
            this.SourceConnectionTextBox = new System.Windows.Forms.TextBox();
            this.ApplyGrantonViewsCheckBox = new System.Windows.Forms.CheckBox();
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
            this.DeleteViewsButton.Location = new System.Drawing.Point(141, 74);
            this.DeleteViewsButton.Name = "DeleteViewsButton";
            this.DeleteViewsButton.Size = new System.Drawing.Size(246, 23);
            this.DeleteViewsButton.TabIndex = 12;
            this.DeleteViewsButton.Text = "Delete Views";
            this.DeleteViewsButton.UseVisualStyleBackColor = true;
            this.DeleteViewsButton.Click += new System.EventHandler(this.DeleteViewsButton_Click);
            // 
            // CreateViewsButton
            // 
            this.CreateViewsButton.Location = new System.Drawing.Point(141, 103);
            this.CreateViewsButton.Name = "CreateViewsButton";
            this.CreateViewsButton.Size = new System.Drawing.Size(246, 23);
            this.CreateViewsButton.TabIndex = 11;
            this.CreateViewsButton.Text = "Create Views";
            this.CreateViewsButton.UseVisualStyleBackColor = true;
            this.CreateViewsButton.Click += new System.EventHandler(this.CreateViewsButton_Click);
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
            // SourceConnectionTextBox
            // 
            this.SourceConnectionTextBox.Location = new System.Drawing.Point(141, 20);
            this.SourceConnectionTextBox.Name = "SourceConnectionTextBox";
            this.SourceConnectionTextBox.Size = new System.Drawing.Size(246, 20);
            this.SourceConnectionTextBox.TabIndex = 7;
            this.SourceConnectionTextBox.TextChanged += new System.EventHandler(this.SourceConnectionTextBox_TextChanged);
            // 
            // ApplyGrantonViewsCheckBox
            // 
            this.ApplyGrantonViewsCheckBox.AutoSize = true;
            this.ApplyGrantonViewsCheckBox.Location = new System.Drawing.Point(141, 47);
            this.ApplyGrantonViewsCheckBox.Name = "ApplyGrantonViewsCheckBox";
            this.ApplyGrantonViewsCheckBox.Size = new System.Drawing.Size(127, 17);
            this.ApplyGrantonViewsCheckBox.TabIndex = 14;
            this.ApplyGrantonViewsCheckBox.Text = "Apply Grant on Views";
            this.ApplyGrantonViewsCheckBox.UseVisualStyleBackColor = true;
            this.ApplyGrantonViewsCheckBox.CheckedChanged += new System.EventHandler(this.ApplyGrantonViewsCheckBox_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 241);
            this.Controls.Add(this.ApplyGrantonViewsCheckBox);
            this.Controls.Add(this.ResultLabel);
            this.Controls.Add(this.DeleteViewsButton);
            this.Controls.Add(this.CreateViewsButton);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SourceConnectionTextBox;
        private System.Windows.Forms.CheckBox ApplyGrantonViewsCheckBox;
    }
}

