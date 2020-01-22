namespace Logitude.Update.PatchDistribution
{
    partial class PatchDistributionForm
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.textBoxApproveRemarks = new System.Windows.Forms.TextBox();
            this.buttonApproveLastFailure = new System.Windows.Forms.Button();
            this.buttonUpdateDB = new System.Windows.Forms.Button();
            this._TBTenant = new System.Windows.Forms.TextBox();
            this.textBoxLogger = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(701, 8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(65, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "LogSQL";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(611, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tenant";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textBoxApproveRemarks);
            this.splitContainer1.Panel1.Controls.Add(this.buttonApproveLastFailure);
            this.splitContainer1.Panel1.Controls.Add(this.buttonUpdateDB);
            this.splitContainer1.Panel1.Controls.Add(this.checkBox1);
            this.splitContainer1.Panel1.Controls.Add(this._TBTenant);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBoxLogger);
            this.splitContainer1.Size = new System.Drawing.Size(800, 426);
            this.splitContainer1.SplitterDistance = 68;
            this.splitContainer1.TabIndex = 4;
            // 
            // textBoxApproveRemarks
            // 
            this.textBoxApproveRemarks.Location = new System.Drawing.Point(302, 5);
            this.textBoxApproveRemarks.Multiline = true;
            this.textBoxApproveRemarks.Name = "textBoxApproveRemarks";
            this.textBoxApproveRemarks.Size = new System.Drawing.Size(173, 60);
            this.textBoxApproveRemarks.TabIndex = 10;
            // 
            // buttonApproveLastFailure
            // 
            this.buttonApproveLastFailure.Location = new System.Drawing.Point(481, 3);
            this.buttonApproveLastFailure.Name = "buttonApproveLastFailure";
            this.buttonApproveLastFailure.Size = new System.Drawing.Size(75, 62);
            this.buttonApproveLastFailure.TabIndex = 9;
            this.buttonApproveLastFailure.Text = "Approve \r\nLast Failure";
            this.buttonApproveLastFailure.UseVisualStyleBackColor = true;
            this.buttonApproveLastFailure.Click += new System.EventHandler(this.buttonApproveLastFailure_Click);
            // 
            // buttonUpdateDB
            // 
            this.buttonUpdateDB.Location = new System.Drawing.Point(3, 3);
            this.buttonUpdateDB.Name = "buttonUpdateDB";
            this.buttonUpdateDB.Size = new System.Drawing.Size(75, 62);
            this.buttonUpdateDB.TabIndex = 8;
            this.buttonUpdateDB.Text = "Update\r\nDataBase";
            this.buttonUpdateDB.UseVisualStyleBackColor = true;
            this.buttonUpdateDB.Click += new System.EventHandler(this.buttonUpdateDB_Click);
            // 
            // _TBTenant
            // 
            this._TBTenant.Location = new System.Drawing.Point(658, 5);
            this._TBTenant.Name = "_TBTenant";
            this._TBTenant.Size = new System.Drawing.Size(37, 20);
            this._TBTenant.TabIndex = 1;
            this._TBTenant.Text = "1";
            // 
            // textBoxLogger
            // 
            this.textBoxLogger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxLogger.Location = new System.Drawing.Point(0, 0);
            this.textBoxLogger.Multiline = true;
            this.textBoxLogger.Name = "textBoxLogger";
            this.textBoxLogger.Size = new System.Drawing.Size(800, 354);
            this.textBoxLogger.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.startToolStripMenuItem.Text = "Start";
            // 
            // PatchDistributionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "PatchDistributionForm";
            this.Text = "Update-Database";
            this.Load += new System.EventHandler(this.PatchDistributionForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox _TBTenant;
        private System.Windows.Forms.TextBox textBoxLogger;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.Button buttonUpdateDB;
        private System.Windows.Forms.Button buttonApproveLastFailure;
        private System.Windows.Forms.TextBox textBoxApproveRemarks;
    }
}