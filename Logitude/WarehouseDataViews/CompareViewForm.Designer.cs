namespace WarehouseDataViews
{
    partial class CompareViewForm
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
            this.SourceConnectiontextBox = new System.Windows.Forms.TextBox();
            this.DestinationtconnectionTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CompareButton = new System.Windows.Forms.Button();
            this.ResultRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // SourceConnectiontextBox
            // 
            this.SourceConnectiontextBox.Location = new System.Drawing.Point(353, 26);
            this.SourceConnectiontextBox.Name = "SourceConnectiontextBox";
            this.SourceConnectiontextBox.Size = new System.Drawing.Size(356, 22);
            this.SourceConnectiontextBox.TabIndex = 0;
            this.SourceConnectiontextBox.TextChanged += new System.EventHandler(this.SourceConnectiontextBox_TextChanged);
            // 
            // DestinationtconnectionTextBox
            // 
            this.DestinationtconnectionTextBox.Location = new System.Drawing.Point(353, 54);
            this.DestinationtconnectionTextBox.Name = "DestinationtconnectionTextBox";
            this.DestinationtconnectionTextBox.Size = new System.Drawing.Size(356, 22);
            this.DestinationtconnectionTextBox.TabIndex = 1;
            this.DestinationtconnectionTextBox.TextChanged += new System.EventHandler(this.DestinationtconnectionTextBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(230, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Source";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(230, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Destination";
            // 
            // CompareButton
            // 
            this.CompareButton.Location = new System.Drawing.Point(363, 82);
            this.CompareButton.Name = "CompareButton";
            this.CompareButton.Size = new System.Drawing.Size(334, 23);
            this.CompareButton.TabIndex = 4;
            this.CompareButton.Text = "Compare";
            this.CompareButton.UseVisualStyleBackColor = true;
            this.CompareButton.Click += new System.EventHandler(this.CompareButton_Click);
            // 
            // ResultRichTextBox
            // 
            this.ResultRichTextBox.Location = new System.Drawing.Point(25, 129);
            this.ResultRichTextBox.Name = "ResultRichTextBox";
            this.ResultRichTextBox.Size = new System.Drawing.Size(1047, 499);
            this.ResultRichTextBox.TabIndex = 5;
            this.ResultRichTextBox.Text = "";
            // 
            // CompareViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 640);
            this.Controls.Add(this.ResultRichTextBox);
            this.Controls.Add(this.CompareButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DestinationtconnectionTextBox);
            this.Controls.Add(this.SourceConnectiontextBox);
            this.Name = "CompareViewForm";
            this.Text = "CompareViewForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SourceConnectiontextBox;
        private System.Windows.Forms.TextBox DestinationtconnectionTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button CompareButton;
        private System.Windows.Forms.RichTextBox ResultRichTextBox;
    }
}