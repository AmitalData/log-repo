
namespace Logitude.Update
{
    partial class RedeemedCheques
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
            this.GetChequesBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tenantTextBox = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chequeIdTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // GetChequesBtn
            // 
            this.GetChequesBtn.Location = new System.Drawing.Point(173, 15);
            this.GetChequesBtn.Name = "GetChequesBtn";
            this.GetChequesBtn.Size = new System.Drawing.Size(238, 35);
            this.GetChequesBtn.TabIndex = 0;
            this.GetChequesBtn.Text = "Get reconciled cheques but not redeemed";
            this.GetChequesBtn.UseVisualStyleBackColor = true;
            this.GetChequesBtn.Click += new System.EventHandler(this.GetChequesBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tenant";
            // 
            // tenantTextBox
            // 
            this.tenantTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tenantTextBox.Location = new System.Drawing.Point(67, 24);
            this.tenantTextBox.Name = "tenantTextBox";
            this.tenantTextBox.Size = new System.Drawing.Size(100, 26);
            this.tenantTextBox.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(23, 129);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1145, 516);
            this.dataGridView1.TabIndex = 3;
            // 
            // chequeIdTextBox
            // 
            this.chequeIdTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chequeIdTextBox.Location = new System.Drawing.Point(903, 19);
            this.chequeIdTextBox.Name = "chequeIdTextBox";
            this.chequeIdTextBox.Size = new System.Drawing.Size(100, 26);
            this.chequeIdTextBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(841, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Cheque Id";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1009, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 26);
            this.button1.TabIndex = 6;
            this.button1.Text = "redeemCheque";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // RedeemedCheques
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 657);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chequeIdTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.tenantTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GetChequesBtn);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RedeemedCheques";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RedeemedCheques";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GetChequesBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tenantTextBox;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox chequeIdTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}