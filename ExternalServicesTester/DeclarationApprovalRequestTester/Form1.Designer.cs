namespace DeclarationApprovalRequestTester
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtTenant = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtShipmentNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDeclarationXmlData = new System.Windows.Forms.TextBox();
            this.btnSendRequest = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtGetToken = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabDec = new System.Windows.Forms.TabPage();
            this.tabAP = new System.Windows.Forms.TabPage();
            this.btnClose = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtAccountingPartnerData = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabDec.SuspendLayout();
            this.tabAP.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(445, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tenant #";
            // 
            // txtTenant
            // 
            this.txtTenant.Location = new System.Drawing.Point(607, 28);
            this.txtTenant.Name = "txtTenant";
            this.txtTenant.Size = new System.Drawing.Size(181, 20);
            this.txtTenant.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(141, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Forwarder Shipment Number";
            // 
            // txtShipmentNumber
            // 
            this.txtShipmentNumber.Location = new System.Drawing.Point(158, 6);
            this.txtShipmentNumber.Name = "txtShipmentNumber";
            this.txtShipmentNumber.Size = new System.Drawing.Size(181, 20);
            this.txtShipmentNumber.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-117, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Declaration Xml Data";
            // 
            // txtDeclarationXmlData
            // 
            this.txtDeclarationXmlData.Location = new System.Drawing.Point(158, 30);
            this.txtDeclarationXmlData.Multiline = true;
            this.txtDeclarationXmlData.Name = "txtDeclarationXmlData";
            this.txtDeclarationXmlData.Size = new System.Drawing.Size(597, 276);
            this.txtDeclarationXmlData.TabIndex = 5;
            // 
            // btnSendRequest
            // 
            this.btnSendRequest.Location = new System.Drawing.Point(683, 450);
            this.btnSendRequest.Name = "btnSendRequest";
            this.btnSendRequest.Size = new System.Drawing.Size(103, 23);
            this.btnSendRequest.TabIndex = 6;
            this.btnSendRequest.Text = "Send Request";
            this.btnSendRequest.UseVisualStyleBackColor = true;
            this.btnSendRequest.Click += new System.EventHandler(this.btnSendRequest_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtGetToken);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(22, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 92);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login Information";
            // 
            // txtGetToken
            // 
            this.txtGetToken.Location = new System.Drawing.Point(325, 20);
            this.txtGetToken.Name = "txtGetToken";
            this.txtGetToken.Size = new System.Drawing.Size(69, 53);
            this.txtGetToken.TabIndex = 4;
            this.txtGetToken.Text = "Get Token";
            this.txtGetToken.UseVisualStyleBackColor = true;
            this.txtGetToken.Click += new System.EventHandler(this.txtGetToken_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(74, 53);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(248, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Password";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(74, 20);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(248, 20);
            this.txtUserName.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(60, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "User Name";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabDec);
            this.tabControl1.Controls.Add(this.tabAP);
            this.tabControl1.Location = new System.Drawing.Point(22, 110);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(766, 338);
            this.tabControl1.TabIndex = 9;
            // 
            // tabDec
            // 
            this.tabDec.BackColor = System.Drawing.Color.Gainsboro;
            this.tabDec.Controls.Add(this.label6);
            this.tabDec.Controls.Add(this.txtDeclarationXmlData);
            this.tabDec.Controls.Add(this.label3);
            this.tabDec.Controls.Add(this.txtShipmentNumber);
            this.tabDec.Controls.Add(this.label2);
            this.tabDec.Location = new System.Drawing.Point(4, 22);
            this.tabDec.Name = "tabDec";
            this.tabDec.Padding = new System.Windows.Forms.Padding(3);
            this.tabDec.Size = new System.Drawing.Size(758, 312);
            this.tabDec.TabIndex = 0;
            this.tabDec.Text = "Declaration Approval";
            // 
            // tabAP
            // 
            this.tabAP.BackColor = System.Drawing.Color.Gainsboro;
            this.tabAP.Controls.Add(this.txtAccountingPartnerData);
            this.tabAP.Controls.Add(this.label7);
            this.tabAP.Location = new System.Drawing.Point(4, 22);
            this.tabAP.Name = "tabAP";
            this.tabAP.Padding = new System.Windows.Forms.Padding(3);
            this.tabAP.Size = new System.Drawing.Size(758, 312);
            this.tabAP.TabIndex = 1;
            this.tabAP.Text = "Accounting Partner";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(601, 450);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Declaration Xml Data";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Accounting Partner Xml";
            // 
            // txtAccountingPartnerData
            // 
            this.txtAccountingPartnerData.Location = new System.Drawing.Point(131, 6);
            this.txtAccountingPartnerData.Multiline = true;
            this.txtAccountingPartnerData.Name = "txtAccountingPartnerData";
            this.txtAccountingPartnerData.Size = new System.Drawing.Size(621, 300);
            this.txtAccountingPartnerData.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSendRequest);
            this.Controls.Add(this.txtTenant);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabDec.ResumeLayout(false);
            this.tabDec.PerformLayout();
            this.tabAP.ResumeLayout(false);
            this.tabAP.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTenant;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtShipmentNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDeclarationXmlData;
        private System.Windows.Forms.Button btnSendRequest;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button txtGetToken;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabDec;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabAP;
        private System.Windows.Forms.TextBox txtAccountingPartnerData;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnClose;
    }
}

