namespace AmitalCustomsWindowsService.Tester.ReqSheet
{
    partial class FormRepushQ
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
            this.buttonRepush = new System.Windows.Forms.Button();
            this.textBoxTenant = new System.Windows.Forms.TextBox();
            this.textBoxReqSheetId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonRepush
            // 
            this.buttonRepush.Location = new System.Drawing.Point(170, 111);
            this.buttonRepush.Name = "buttonRepush";
            this.buttonRepush.Size = new System.Drawing.Size(75, 23);
            this.buttonRepush.TabIndex = 0;
            this.buttonRepush.Text = "Repush";
            this.buttonRepush.UseVisualStyleBackColor = true;
            this.buttonRepush.Click += new System.EventHandler(this.buttonRepush_Click);
            // 
            // textBoxTenant
            // 
            this.textBoxTenant.Location = new System.Drawing.Point(97, 29);
            this.textBoxTenant.Name = "textBoxTenant";
            this.textBoxTenant.Size = new System.Drawing.Size(100, 20);
            this.textBoxTenant.TabIndex = 1;
            // 
            // textBoxReqSheetId
            // 
            this.textBoxReqSheetId.Location = new System.Drawing.Point(97, 57);
            this.textBoxReqSheetId.Name = "textBoxReqSheetId";
            this.textBoxReqSheetId.Size = new System.Drawing.Size(100, 20);
            this.textBoxReqSheetId.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "tenant";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "ReqSheet Id";
            // 
            // FormRepushQ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxReqSheetId);
            this.Controls.Add(this.textBoxTenant);
            this.Controls.Add(this.buttonRepush);
            this.Name = "FormRepushQ";
            this.Text = "FormRepushQ";
            this.Load += new System.EventHandler(this.FormRepushQ_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRepush;
        private System.Windows.Forms.TextBox textBoxTenant;
        private System.Windows.Forms.TextBox textBoxReqSheetId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}