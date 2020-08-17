namespace Logitude.Update
{
    partial class CopyData
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
            this.txtStartCopy = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.txtTargetTenant = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.txtFromPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtToPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCopyUserContactsToCrm = new System.Windows.Forms.Button();
            this.btnCopyChargesTypesToCloud = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnCopyDocumentTypes = new System.Windows.Forms.Button();
            this.txtDestinationConnStr = new System.Windows.Forms.TextBox();
            this.lblDestConn = new System.Windows.Forms.Label();
            this.txtSourceConnStr = new System.Windows.Forms.TextBox();
            this.lblSourceConnection = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtStartCopy
            // 
            this.txtStartCopy.Location = new System.Drawing.Point(30, 46);
            this.txtStartCopy.Name = "txtStartCopy";
            this.txtStartCopy.Size = new System.Drawing.Size(101, 89);
            this.txtStartCopy.TabIndex = 0;
            this.txtStartCopy.Text = "Start Copy";
            this.txtStartCopy.UseVisualStyleBackColor = true;
            this.txtStartCopy.Click += new System.EventHandler(this.txtStartCopy_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 237);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Completed Successfully ...";
            this.label1.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(30, 164);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(215, 27);
            this.button1.TabIndex = 2;
            this.button1.Text = "Copy Date To Tenant: ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtTargetTenant
            // 
            this.txtTargetTenant.Location = new System.Drawing.Point(274, 164);
            this.txtTargetTenant.Name = "txtTargetTenant";
            this.txtTargetTenant.Size = new System.Drawing.Size(246, 20);
            this.txtTargetTenant.TabIndex = 3;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(134, 46);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(166, 89);
            this.button2.TabIndex = 4;
            this.button2.Text = "Copy Statuses";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(306, 46);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(214, 89);
            this.button3.TabIndex = 5;
            this.button3.Text = "Cope Document Metadata Types";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(527, 46);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(83, 89);
            this.button4.TabIndex = 6;
            this.button4.Text = "Copy Ports";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // txtFromPort
            // 
            this.txtFromPort.Location = new System.Drawing.Point(690, 64);
            this.txtFromPort.Name = "txtFromPort";
            this.txtFromPort.Size = new System.Drawing.Size(60, 20);
            this.txtFromPort.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(617, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "From Tenant";
            // 
            // txtToPort
            // 
            this.txtToPort.Location = new System.Drawing.Point(690, 100);
            this.txtToPort.Name = "txtToPort";
            this.txtToPort.Size = new System.Drawing.Size(60, 20);
            this.txtToPort.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(617, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "To Tenant";
            // 
            // btnCopyUserContactsToCrm
            // 
            this.btnCopyUserContactsToCrm.Location = new System.Drawing.Point(690, 187);
            this.btnCopyUserContactsToCrm.Name = "btnCopyUserContactsToCrm";
            this.btnCopyUserContactsToCrm.Size = new System.Drawing.Size(165, 73);
            this.btnCopyUserContactsToCrm.TabIndex = 11;
            this.btnCopyUserContactsToCrm.Text = "Copy CRM Contacts";
            this.btnCopyUserContactsToCrm.UseVisualStyleBackColor = true;
            this.btnCopyUserContactsToCrm.Click += new System.EventHandler(this.btnCopyUserContactsToCrm_Click);
            // 
            // btnCopyChargesTypesToCloud
            // 
            this.btnCopyChargesTypesToCloud.Location = new System.Drawing.Point(274, 208);
            this.btnCopyChargesTypesToCloud.Name = "btnCopyChargesTypesToCloud";
            this.btnCopyChargesTypesToCloud.Size = new System.Drawing.Size(246, 42);
            this.btnCopyChargesTypesToCloud.TabIndex = 12;
            this.btnCopyChargesTypesToCloud.Text = "Copy Charges Types";
            this.btnCopyChargesTypesToCloud.UseVisualStyleBackColor = true;
            this.btnCopyChargesTypesToCloud.Click += new System.EventHandler(this.btnCopyChargesTypesToCloud_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 300);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1126, 297);
            this.tabControl1.TabIndex = 18;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnCopyDocumentTypes);
            this.tabPage1.Controls.Add(this.txtDestinationConnStr);
            this.tabPage1.Controls.Add(this.lblDestConn);
            this.tabPage1.Controls.Add(this.txtSourceConnStr);
            this.tabPage1.Controls.Add(this.lblSourceConnection);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1118, 271);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnCopyDocumentTypes
            // 
            this.btnCopyDocumentTypes.Location = new System.Drawing.Point(18, 124);
            this.btnCopyDocumentTypes.Name = "btnCopyDocumentTypes";
            this.btnCopyDocumentTypes.Size = new System.Drawing.Size(136, 32);
            this.btnCopyDocumentTypes.TabIndex = 22;
            this.btnCopyDocumentTypes.Text = "Copy Document Types";
            this.btnCopyDocumentTypes.UseVisualStyleBackColor = true;
            this.btnCopyDocumentTypes.Click += new System.EventHandler(this.btnCopyDocumentTypes_Click_1);
            // 
            // txtDestinationConnStr
            // 
            this.txtDestinationConnStr.Location = new System.Drawing.Point(177, 70);
            this.txtDestinationConnStr.Name = "txtDestinationConnStr";
            this.txtDestinationConnStr.Size = new System.Drawing.Size(481, 20);
            this.txtDestinationConnStr.TabIndex = 21;
            this.txtDestinationConnStr.Text = "Main,sa,Saas256,amitaldata.cloudapp.net";
            // 
            // lblDestConn
            // 
            this.lblDestConn.AutoSize = true;
            this.lblDestConn.Location = new System.Drawing.Point(15, 73);
            this.lblDestConn.Name = "lblDestConn";
            this.lblDestConn.Size = new System.Drawing.Size(129, 13);
            this.lblDestConn.TabIndex = 20;
            this.lblDestConn.Text = "Source Connection string:";
            // 
            // txtSourceConnStr
            // 
            this.txtSourceConnStr.Location = new System.Drawing.Point(177, 31);
            this.txtSourceConnStr.Name = "txtSourceConnStr";
            this.txtSourceConnStr.Size = new System.Drawing.Size(481, 20);
            this.txtSourceConnStr.TabIndex = 19;
            this.txtSourceConnStr.Text = "logbox-main,logboxadmin,London2015!London2015!,logboxdbs.database.windows.net";
            // 
            // lblSourceConnection
            // 
            this.lblSourceConnection.AutoSize = true;
            this.lblSourceConnection.Location = new System.Drawing.Point(15, 34);
            this.lblSourceConnection.Name = "lblSourceConnection";
            this.lblSourceConnection.Size = new System.Drawing.Size(129, 13);
            this.lblSourceConnection.TabIndex = 18;
            this.lblSourceConnection.Text = "Source Connection string:";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1118, 271);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // CopyData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 598);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCopyChargesTypesToCloud);
            this.Controls.Add(this.btnCopyUserContactsToCrm);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtToPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFromPort);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtTargetTenant);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtStartCopy);
            this.Name = "CopyData";
            this.Text = "CopyData";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button txtStartCopy;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtTargetTenant;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox txtFromPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtToPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCopyUserContactsToCrm;
        private System.Windows.Forms.Button btnCopyChargesTypesToCloud;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnCopyDocumentTypes;
        private System.Windows.Forms.TextBox txtDestinationConnStr;
        private System.Windows.Forms.Label lblDestConn;
        private System.Windows.Forms.TextBox txtSourceConnStr;
        private System.Windows.Forms.Label lblSourceConnection;
        private System.Windows.Forms.TabPage tabPage2;
    }
}