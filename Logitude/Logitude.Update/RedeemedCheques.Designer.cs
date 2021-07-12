
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
            this.components = new System.ComponentModel.Container();
            this.GetChequesBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tenantTextBox = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chequeIdTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tenantsTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.countLbl = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GetChequesBtn
            // 
            this.GetChequesBtn.Location = new System.Drawing.Point(254, 91);
            this.GetChequesBtn.Name = "GetChequesBtn";
            this.GetChequesBtn.Size = new System.Drawing.Size(268, 26);
            this.GetChequesBtn.TabIndex = 0;
            this.GetChequesBtn.Text = "Get reconciled cheques but not redeemed";
            this.GetChequesBtn.UseVisualStyleBackColor = true;
            this.GetChequesBtn.Click += new System.EventHandler(this.GetChequesBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tenant";
            // 
            // tenantTextBox
            // 
            this.tenantTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tenantTextBox.Location = new System.Drawing.Point(80, 104);
            this.tenantTextBox.Name = "tenantTextBox";
            this.tenantTextBox.Size = new System.Drawing.Size(100, 26);
            this.tenantTextBox.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 268);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1196, 327);
            this.dataGridView1.TabIndex = 3;
            // 
            // chequeIdTextBox
            // 
            this.chequeIdTextBox.Enabled = false;
            this.chequeIdTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chequeIdTextBox.Location = new System.Drawing.Point(80, 29);
            this.chequeIdTextBox.Name = "chequeIdTextBox";
            this.chequeIdTextBox.Size = new System.Drawing.Size(100, 26);
            this.chequeIdTextBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(18, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Cheque Id";
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(186, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 26);
            this.button1.TabIndex = 6;
            this.button1.Text = "redeem one Cheque and recalculate total";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Controls.Add(this.tenantsTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.logTextBox);
            this.groupBox1.Location = new System.Drawing.Point(528, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(653, 233);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "correct data";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(16, 186);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(268, 26);
            this.button4.TabIndex = 9;
            this.button4.Text = "Recalculate All Billto futurecheques";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.button3.Location = new System.Drawing.Point(16, 154);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(268, 26);
            this.button3.TabIndex = 10;
            this.button3.Text = "Recalculate  GLAccountMoreData Futurecheques";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.fixTotalsBtn_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button2.Location = new System.Drawing.Point(16, 79);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(268, 26);
            this.button2.TabIndex = 9;
            this.button2.Text = "Fix cheques";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.chequesUpdateAll_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(443, 17);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(138, 20);
            this.progressBar1.TabIndex = 8;
            // 
            // tenantsTextBox
            // 
            this.tenantsTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.tenantsTextBox.Location = new System.Drawing.Point(16, 44);
            this.tenantsTextBox.Name = "tenantsTextBox";
            this.tenantsTextBox.Size = new System.Drawing.Size(268, 20);
            this.tenantsTextBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Tenants (seperated by comma)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(318, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "log";
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(321, 44);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(260, 168);
            this.logTextBox.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // countLbl
            // 
            this.countLbl.AutoSize = true;
            this.countLbl.Location = new System.Drawing.Point(18, 252);
            this.countLbl.Name = "countLbl";
            this.countLbl.Size = new System.Drawing.Size(0, 13);
            this.countLbl.TabIndex = 8;
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button5.Location = new System.Drawing.Point(16, 111);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(268, 26);
            this.button5.TabIndex = 11;
            this.button5.Text = "Fix defered cheques";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.defered);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(254, 123);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(268, 26);
            this.button6.TabIndex = 9;
            this.button6.Text = "Get defered reconciled cheques but not redeemed";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.GetDefferedChequesBtn_Click);
            // 
            // RedeemedCheques
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1196, 595);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.countLbl);
            this.Controls.Add(this.groupBox1);
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
            this.Load += new System.EventHandler(this.RedeemedCheques_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox tenantsTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label countLbl;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
    }
}