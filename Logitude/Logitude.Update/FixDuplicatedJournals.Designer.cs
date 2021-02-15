
namespace Logitude.Update
{
    partial class FixDuplicatedJournals
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
            this.tenantTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GetChequesBtn = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.countLbl = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.fixDupJournals = new System.Windows.Forms.Button();
            this.statusLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tenantTextBox
            // 
            this.tenantTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tenantTextBox.Location = new System.Drawing.Point(81, 22);
            this.tenantTextBox.Name = "tenantTextBox";
            this.tenantTextBox.Size = new System.Drawing.Size(100, 26);
            this.tenantTextBox.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Tenant";
            // 
            // GetChequesBtn
            // 
            this.GetChequesBtn.Location = new System.Drawing.Point(678, 22);
            this.GetChequesBtn.Name = "GetChequesBtn";
            this.GetChequesBtn.Size = new System.Drawing.Size(252, 28);
            this.GetChequesBtn.TabIndex = 3;
            this.GetChequesBtn.Text = "Get ARPayment With Duplicated Journals";
            this.GetChequesBtn.UseVisualStyleBackColor = true;
            this.GetChequesBtn.Visible = false;
            this.GetChequesBtn.Click += new System.EventHandler(this.GetJournalsBtn_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 82);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(918, 436);
            this.dataGridView1.TabIndex = 6;
            // 
            // countLbl
            // 
            this.countLbl.AutoSize = true;
            this.countLbl.Location = new System.Drawing.Point(12, 66);
            this.countLbl.Name = "countLbl";
            this.countLbl.Size = new System.Drawing.Size(16, 13);
            this.countLbl.TabIndex = 9;
            this.countLbl.Text = "...";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(187, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(173, 28);
            this.button1.TabIndex = 10;
            this.button1.Text = "Get Duplicated Journals";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // fixDupJournals
            // 
            this.fixDupJournals.ForeColor = System.Drawing.Color.RoyalBlue;
            this.fixDupJournals.Location = new System.Drawing.Point(366, 22);
            this.fixDupJournals.Name = "fixDupJournals";
            this.fixDupJournals.Size = new System.Drawing.Size(173, 28);
            this.fixDupJournals.TabIndex = 11;
            this.fixDupJournals.Text = "Fix Duplicated Journals";
            this.fixDupJournals.UseVisualStyleBackColor = true;
            this.fixDupJournals.Click += new System.EventHandler(this.fixDupJournals_Click);
            // 
            // statusLbl
            // 
            this.statusLbl.AutoSize = true;
            this.statusLbl.Location = new System.Drawing.Point(363, 53);
            this.statusLbl.Name = "statusLbl";
            this.statusLbl.Size = new System.Drawing.Size(16, 13);
            this.statusLbl.TabIndex = 12;
            this.statusLbl.Text = "...";
            // 
            // FixDuplicatedJournals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 540);
            this.Controls.Add(this.statusLbl);
            this.Controls.Add(this.fixDupJournals);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.countLbl);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.tenantTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GetChequesBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FixDuplicatedJournals";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Fix Duplicated Journals Tool";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tenantTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button GetChequesBtn;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label countLbl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button fixDupJournals;
        private System.Windows.Forms.Label statusLbl;
    }
}