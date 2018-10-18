namespace AmitalCustomsWindowsService.Tester.CustomMessage
{
    partial class Send380Form
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
            this.btnSendDCA = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxCntry = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxAccNum = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDate = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxAttachmentID = new System.Windows.Forms.TextBox();
            this.checkBoxOriginal = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBoxSourceFile = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSendDCA
            // 
            this.btnSendDCA.Location = new System.Drawing.Point(355, 227);
            this.btnSendDCA.Name = "btnSendDCA";
            this.btnSendDCA.Size = new System.Drawing.Size(75, 23);
            this.btnSendDCA.TabIndex = 0;
            this.btnSendDCA.Text = "SendDCA";
            this.btnSendDCA.UseVisualStyleBackColor = true;
            this.btnSendDCA.Click += new System.EventHandler(this.btnSendDCA_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = " ארץ חשבון";
            // 
            // textBoxCntry
            // 
            this.textBoxCntry.Location = new System.Drawing.Point(172, 9);
            this.textBoxCntry.Name = "textBoxCntry";
            this.textBoxCntry.Size = new System.Drawing.Size(100, 20);
            this.textBoxCntry.TabIndex = 2;
            this.textBoxCntry.Text = "US";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "מספר חשבון";
            // 
            // textBoxAccNum
            // 
            this.textBoxAccNum.Location = new System.Drawing.Point(172, 35);
            this.textBoxAccNum.Name = "textBoxAccNum";
            this.textBoxAccNum.Size = new System.Drawing.Size(100, 20);
            this.textBoxAccNum.TabIndex = 2;
            this.textBoxAccNum.Text = "5520";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "תאריך החשבון";
            // 
            // textBoxDate
            // 
            this.textBoxDate.Location = new System.Drawing.Point(172, 61);
            this.textBoxDate.Name = "textBoxDate";
            this.textBoxDate.Size = new System.Drawing.Size(100, 20);
            this.textBoxDate.TabIndex = 2;
            this.textBoxDate.Text = "20.04.14";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "האם מסמך מקורי";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "attachmentID";
            // 
            // textBoxAttachmentID
            // 
            this.textBoxAttachmentID.Location = new System.Drawing.Point(172, 113);
            this.textBoxAttachmentID.Name = "textBoxAttachmentID";
            this.textBoxAttachmentID.Size = new System.Drawing.Size(100, 20);
            this.textBoxAttachmentID.TabIndex = 2;
            this.textBoxAttachmentID.Text = "using-30";
            // 
            // checkBoxOriginal
            // 
            this.checkBoxOriginal.AutoSize = true;
            this.checkBoxOriginal.Location = new System.Drawing.Point(172, 88);
            this.checkBoxOriginal.Name = "checkBoxOriginal";
            this.checkBoxOriginal.Size = new System.Drawing.Size(15, 14);
            this.checkBoxOriginal.TabIndex = 3;
            this.checkBoxOriginal.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBoxSourceFile
            // 
            this.textBoxSourceFile.Location = new System.Drawing.Point(12, 147);
            this.textBoxSourceFile.Name = "textBoxSourceFile";
            this.textBoxSourceFile.Size = new System.Drawing.Size(418, 20);
            this.textBoxSourceFile.TabIndex = 4;
            this.textBoxSourceFile.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.textBoxSourceFile_MouseDoubleClick);
            // 
            // Send380Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 262);
            this.Controls.Add(this.textBoxSourceFile);
            this.Controls.Add(this.checkBoxOriginal);
            this.Controls.Add(this.textBoxAttachmentID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxAccNum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxCntry);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSendDCA);
            this.Name = "Send380Form";
            this.Text = "Send380Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendDCA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxCntry;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxAccNum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxAttachmentID;
        private System.Windows.Forms.CheckBox checkBoxOriginal;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBoxSourceFile;
    }
}