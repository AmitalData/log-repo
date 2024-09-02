namespace WindowsFormsApplication1
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtWorkerRoleName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtStatusCode = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(43, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(192, 82);
            this.button1.TabIndex = 0;
            this.button1.Text = "Run Worker Roles";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(325, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(192, 82);
            this.button2.TabIndex = 1;
            this.button2.Text = "Stop Worker Roles";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtWorkerRoleName
            // 
            this.txtWorkerRoleName.Location = new System.Drawing.Point(43, 167);
            this.txtWorkerRoleName.Name = "txtWorkerRoleName";
            this.txtWorkerRoleName.Size = new System.Drawing.Size(192, 20);
            this.txtWorkerRoleName.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Workerole";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(277, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Status Name(code check from query 1)";
            // 
            // txtStatusCode
            // 
            this.txtStatusCode.Location = new System.Drawing.Point(315, 169);
            this.txtStatusCode.Name = "txtStatusCode";
            this.txtStatusCode.Size = new System.Drawing.Size(192, 20);
            this.txtStatusCode.TabIndex = 7;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(5, 212);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(569, 132);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "QUERY 1:\r\nSELECT [CreateDate],[Name],[WaitingStatus] FROM [dbo].[WorkerRoleNames]" +
    "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 443);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtStatusCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtWorkerRoleName);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "WR Simulator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtWorkerRoleName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtStatusCode;
        private System.Windows.Forms.TextBox textBox1;
    }
}

