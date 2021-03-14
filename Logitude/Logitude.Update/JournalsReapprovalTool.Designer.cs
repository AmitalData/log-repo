
namespace Logitude.Update
{
    partial class JournalsReapprovalTool
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
            this.filtersGroup = new System.Windows.Forms.GroupBox();
            this.StatusTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tenantTextBox = new System.Windows.Forms.TextBox();
            this.tenantLbl = new System.Windows.Forms.Label();
            this.GetJournalsBtn = new System.Windows.Forms.Button();
            this.externalSystemCheckBox = new System.Windows.Forms.CheckBox();
            this.extSystemLbl = new System.Windows.Forms.Label();
            this.JournalNumberTextBox = new System.Windows.Forms.TextBox();
            this.JournalNumberLbl = new System.Windows.Forms.Label();
            this.journalIdTextBox = new System.Windows.Forms.TextBox();
            this.JournalIdLbl = new System.Windows.Forms.Label();
            this.toDatePicker = new System.Windows.Forms.DateTimePicker();
            this.toDateLbl = new System.Windows.Forms.Label();
            this.fromDatePicker = new System.Windows.Forms.DateTimePicker();
            this.fromDateLbl = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.resultGridView = new System.Windows.Forms.DataGridView();
            this.countLbl = new System.Windows.Forms.Label();
            this.approveBtn = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressLabel = new System.Windows.Forms.Label();
            this.filtersGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // filtersGroup
            // 
            this.filtersGroup.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.filtersGroup.Controls.Add(this.StatusTextBox);
            this.filtersGroup.Controls.Add(this.label2);
            this.filtersGroup.Controls.Add(this.tenantTextBox);
            this.filtersGroup.Controls.Add(this.tenantLbl);
            this.filtersGroup.Controls.Add(this.GetJournalsBtn);
            this.filtersGroup.Controls.Add(this.externalSystemCheckBox);
            this.filtersGroup.Controls.Add(this.extSystemLbl);
            this.filtersGroup.Controls.Add(this.JournalNumberTextBox);
            this.filtersGroup.Controls.Add(this.JournalNumberLbl);
            this.filtersGroup.Controls.Add(this.journalIdTextBox);
            this.filtersGroup.Controls.Add(this.JournalIdLbl);
            this.filtersGroup.Controls.Add(this.toDatePicker);
            this.filtersGroup.Controls.Add(this.toDateLbl);
            this.filtersGroup.Controls.Add(this.fromDatePicker);
            this.filtersGroup.Controls.Add(this.fromDateLbl);
            this.filtersGroup.Location = new System.Drawing.Point(15, 12);
            this.filtersGroup.Name = "filtersGroup";
            this.filtersGroup.Size = new System.Drawing.Size(322, 272);
            this.filtersGroup.TabIndex = 0;
            this.filtersGroup.TabStop = false;
            this.filtersGroup.Text = "Filters";
            // 
            // StatusTextBox
            // 
            this.StatusTextBox.Location = new System.Drawing.Point(128, 155);
            this.StatusTextBox.Name = "StatusTextBox";
            this.StatusTextBox.Size = new System.Drawing.Size(164, 22);
            this.StatusTextBox.TabIndex = 13;
            this.StatusTextBox.Text = "4";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 158);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Status Code";
            // 
            // tenantTextBox
            // 
            this.tenantTextBox.Location = new System.Drawing.Point(128, 26);
            this.tenantTextBox.Name = "tenantTextBox";
            this.tenantTextBox.Size = new System.Drawing.Size(164, 22);
            this.tenantTextBox.TabIndex = 11;
            // 
            // tenantLbl
            // 
            this.tenantLbl.AutoSize = true;
            this.tenantLbl.Location = new System.Drawing.Point(15, 29);
            this.tenantLbl.Name = "tenantLbl";
            this.tenantLbl.Size = new System.Drawing.Size(50, 16);
            this.tenantLbl.TabIndex = 10;
            this.tenantLbl.Text = "Tenant";
            // 
            // GetJournalsBtn
            // 
            this.GetJournalsBtn.Location = new System.Drawing.Point(127, 232);
            this.GetJournalsBtn.Name = "GetJournalsBtn";
            this.GetJournalsBtn.Size = new System.Drawing.Size(164, 34);
            this.GetJournalsBtn.TabIndex = 2;
            this.GetJournalsBtn.Text = "Get Unapproved Journals";
            this.GetJournalsBtn.UseVisualStyleBackColor = true;
            this.GetJournalsBtn.Click += new System.EventHandler(this.GetJournalsBtn_Click);
            // 
            // externalSystemCheckBox
            // 
            this.externalSystemCheckBox.AutoSize = true;
            this.externalSystemCheckBox.Checked = true;
            this.externalSystemCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.externalSystemCheckBox.Location = new System.Drawing.Point(127, 183);
            this.externalSystemCheckBox.Name = "externalSystemCheckBox";
            this.externalSystemCheckBox.Size = new System.Drawing.Size(15, 14);
            this.externalSystemCheckBox.TabIndex = 9;
            this.externalSystemCheckBox.UseVisualStyleBackColor = true;
            // 
            // extSystemLbl
            // 
            this.extSystemLbl.AutoSize = true;
            this.extSystemLbl.Location = new System.Drawing.Point(14, 183);
            this.extSystemLbl.Name = "extSystemLbl";
            this.extSystemLbl.Size = new System.Drawing.Size(104, 16);
            this.extSystemLbl.TabIndex = 8;
            this.extSystemLbl.Text = "External System";
            // 
            // JournalNumberTextBox
            // 
            this.JournalNumberTextBox.Location = new System.Drawing.Point(128, 130);
            this.JournalNumberTextBox.Name = "JournalNumberTextBox";
            this.JournalNumberTextBox.Size = new System.Drawing.Size(164, 22);
            this.JournalNumberTextBox.TabIndex = 7;
            // 
            // JournalNumberLbl
            // 
            this.JournalNumberLbl.AutoSize = true;
            this.JournalNumberLbl.Location = new System.Drawing.Point(15, 133);
            this.JournalNumberLbl.Name = "JournalNumberLbl";
            this.JournalNumberLbl.Size = new System.Drawing.Size(103, 16);
            this.JournalNumberLbl.TabIndex = 6;
            this.JournalNumberLbl.Text = "Journal Number";
            // 
            // journalIdTextBox
            // 
            this.journalIdTextBox.Location = new System.Drawing.Point(128, 104);
            this.journalIdTextBox.Name = "journalIdTextBox";
            this.journalIdTextBox.Size = new System.Drawing.Size(164, 22);
            this.journalIdTextBox.TabIndex = 5;
            // 
            // JournalIdLbl
            // 
            this.JournalIdLbl.AutoSize = true;
            this.JournalIdLbl.Location = new System.Drawing.Point(15, 107);
            this.JournalIdLbl.Name = "JournalIdLbl";
            this.JournalIdLbl.Size = new System.Drawing.Size(66, 16);
            this.JournalIdLbl.TabIndex = 4;
            this.JournalIdLbl.Text = "Journal Id";
            // 
            // toDatePicker
            // 
            this.toDatePicker.Checked = false;
            this.toDatePicker.CustomFormat = "dd-MM-yyyy, hh:mm";
            this.toDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.toDatePicker.Location = new System.Drawing.Point(128, 78);
            this.toDatePicker.Name = "toDatePicker";
            this.toDatePicker.ShowCheckBox = true;
            this.toDatePicker.Size = new System.Drawing.Size(164, 22);
            this.toDatePicker.TabIndex = 3;
            // 
            // toDateLbl
            // 
            this.toDateLbl.AutoSize = true;
            this.toDateLbl.Location = new System.Drawing.Point(15, 84);
            this.toDateLbl.Name = "toDateLbl";
            this.toDateLbl.Size = new System.Drawing.Size(57, 16);
            this.toDateLbl.TabIndex = 2;
            this.toDateLbl.Text = "To Date";
            // 
            // fromDatePicker
            // 
            this.fromDatePicker.Checked = false;
            this.fromDatePicker.CustomFormat = "dd-MM-yyyy, hh:mm";
            this.fromDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.fromDatePicker.Location = new System.Drawing.Point(128, 52);
            this.fromDatePicker.Name = "fromDatePicker";
            this.fromDatePicker.ShowCheckBox = true;
            this.fromDatePicker.Size = new System.Drawing.Size(164, 22);
            this.fromDatePicker.TabIndex = 1;
            // 
            // fromDateLbl
            // 
            this.fromDateLbl.AutoSize = true;
            this.fromDateLbl.Location = new System.Drawing.Point(15, 58);
            this.fromDateLbl.Name = "fromDateLbl";
            this.fromDateLbl.Size = new System.Drawing.Size(71, 16);
            this.fromDateLbl.TabIndex = 0;
            this.fromDateLbl.Text = "From Date";
            // 
            // resultGridView
            // 
            this.resultGridView.AllowUserToAddRows = false;
            this.resultGridView.AllowUserToDeleteRows = false;
            this.resultGridView.AllowUserToOrderColumns = true;
            this.resultGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.resultGridView.Location = new System.Drawing.Point(0, 290);
            this.resultGridView.Name = "resultGridView";
            this.resultGridView.ReadOnly = true;
            this.resultGridView.Size = new System.Drawing.Size(923, 270);
            this.resultGridView.TabIndex = 1;
            // 
            // countLbl
            // 
            this.countLbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.countLbl.Location = new System.Drawing.Point(829, 274);
            this.countLbl.Name = "countLbl";
            this.countLbl.Size = new System.Drawing.Size(82, 13);
            this.countLbl.TabIndex = 9;
            this.countLbl.Text = "...";
            this.countLbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // approveBtn
            // 
            this.approveBtn.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.approveBtn.Location = new System.Drawing.Point(556, 30);
            this.approveBtn.Name = "approveBtn";
            this.approveBtn.Size = new System.Drawing.Size(181, 34);
            this.approveBtn.TabIndex = 10;
            this.approveBtn.Text = "Get And Reapprove Journals";
            this.approveBtn.UseVisualStyleBackColor = true;
            this.approveBtn.Click += new System.EventHandler(this.approveButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.progressBar1.Location = new System.Drawing.Point(556, 70);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(181, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // progressLabel
            // 
            this.progressLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.progressLabel.Location = new System.Drawing.Point(553, 96);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(184, 28);
            this.progressLabel.TabIndex = 12;
            this.progressLabel.Text = "...";
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // JournalsReapprovalTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(923, 560);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.approveBtn);
            this.Controls.Add(this.countLbl);
            this.Controls.Add(this.resultGridView);
            this.Controls.Add(this.filtersGroup);
            this.MaximizeBox = false;
            this.Name = "JournalsReapprovalTool";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Journals Reapproval Tool";
            this.Load += new System.EventHandler(this.JournalsReapprovalTool_Load);
            this.filtersGroup.ResumeLayout(false);
            this.filtersGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox filtersGroup;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.DateTimePicker toDatePicker;
        private System.Windows.Forms.Label toDateLbl;
        private System.Windows.Forms.DateTimePicker fromDatePicker;
        private System.Windows.Forms.Label fromDateLbl;
        private System.Windows.Forms.TextBox journalIdTextBox;
        private System.Windows.Forms.Label JournalIdLbl;
        private System.Windows.Forms.Button GetJournalsBtn;
        private System.Windows.Forms.CheckBox externalSystemCheckBox;
        private System.Windows.Forms.Label extSystemLbl;
        private System.Windows.Forms.TextBox JournalNumberTextBox;
        private System.Windows.Forms.Label JournalNumberLbl;
        private System.Windows.Forms.DataGridView resultGridView;
        private System.Windows.Forms.Label countLbl;
        private System.Windows.Forms.TextBox tenantTextBox;
        private System.Windows.Forms.Label tenantLbl;
        private System.Windows.Forms.Button approveBtn;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.TextBox StatusTextBox;
        private System.Windows.Forms.Label label2;
    }
}