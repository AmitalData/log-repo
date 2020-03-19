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
            this.label6 = new System.Windows.Forms.Label();
            this.tabAP = new System.Windows.Forms.TabPage();
            this.txtAccountingPartnerData = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabUserIDNumberRequest = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtForwarderShipmentNumber = new System.Windows.Forms.TextBox();
            this.txtCustomerAddress = new System.Windows.Forms.TextBox();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.txtDeclarationNumber = new System.Windows.Forms.TextBox();
            this.txtQuantity = new System.Windows.Forms.TextBox();
            this.txtMaster = new System.Windows.Forms.TextBox();
            this.txtGoodsDescritpion = new System.Windows.Forms.TextBox();
            this.txtHawb = new System.Windows.Forms.TextBox();
            this.txtShipmentValueInNIS = new System.Windows.Forms.TextBox();
            this.txtSenderDetails = new System.Windows.Forms.TextBox();
            this.txtWeight = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabDec.SuspendLayout();
            this.tabAP.SuspendLayout();
            this.tabUserIDNumberRequest.SuspendLayout();
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
            this.tabControl1.Controls.Add(this.tabUserIDNumberRequest);
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
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Declaration Xml Data";
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
            // txtAccountingPartnerData
            // 
            this.txtAccountingPartnerData.Location = new System.Drawing.Point(131, 6);
            this.txtAccountingPartnerData.Multiline = true;
            this.txtAccountingPartnerData.Name = "txtAccountingPartnerData";
            this.txtAccountingPartnerData.Size = new System.Drawing.Size(621, 300);
            this.txtAccountingPartnerData.TabIndex = 1;
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
            // tabUserIDNumberRequest
            // 
            this.tabUserIDNumberRequest.BackColor = System.Drawing.Color.Gainsboro;
            this.tabUserIDNumberRequest.Controls.Add(this.txtWeight);
            this.tabUserIDNumberRequest.Controls.Add(this.txtShipmentValueInNIS);
            this.tabUserIDNumberRequest.Controls.Add(this.txtSenderDetails);
            this.tabUserIDNumberRequest.Controls.Add(this.txtQuantity);
            this.tabUserIDNumberRequest.Controls.Add(this.txtMaster);
            this.tabUserIDNumberRequest.Controls.Add(this.txtGoodsDescritpion);
            this.tabUserIDNumberRequest.Controls.Add(this.txtHawb);
            this.tabUserIDNumberRequest.Controls.Add(this.txtDeclarationNumber);
            this.tabUserIDNumberRequest.Controls.Add(this.txtCustomerName);
            this.tabUserIDNumberRequest.Controls.Add(this.txtCustomerAddress);
            this.tabUserIDNumberRequest.Controls.Add(this.txtForwarderShipmentNumber);
            this.tabUserIDNumberRequest.Controls.Add(this.label18);
            this.tabUserIDNumberRequest.Controls.Add(this.label19);
            this.tabUserIDNumberRequest.Controls.Add(this.label14);
            this.tabUserIDNumberRequest.Controls.Add(this.label15);
            this.tabUserIDNumberRequest.Controls.Add(this.label16);
            this.tabUserIDNumberRequest.Controls.Add(this.label11);
            this.tabUserIDNumberRequest.Controls.Add(this.label12);
            this.tabUserIDNumberRequest.Controls.Add(this.label13);
            this.tabUserIDNumberRequest.Controls.Add(this.label10);
            this.tabUserIDNumberRequest.Controls.Add(this.label9);
            this.tabUserIDNumberRequest.Controls.Add(this.label8);
            this.tabUserIDNumberRequest.Location = new System.Drawing.Point(4, 22);
            this.tabUserIDNumberRequest.Name = "tabUserIDNumberRequest";
            this.tabUserIDNumberRequest.Padding = new System.Windows.Forms.Padding(3);
            this.tabUserIDNumberRequest.Size = new System.Drawing.Size(758, 312);
            this.tabUserIDNumberRequest.TabIndex = 2;
            this.tabUserIDNumberRequest.Text = "UserIDNumberRequest";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(24, 27);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(135, 13);
            this.label8.TabIndex = 2;
            this.label8.Text = "ForwarderShipmentNumber";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(24, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "CustomerAddress";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(24, 77);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "CustomerName";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(24, 153);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(91, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "GoodsDescritpion";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(24, 129);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(35, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Hawb";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(24, 103);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(98, 13);
            this.label13.TabIndex = 5;
            this.label13.Text = "DeclarationNumber";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(24, 231);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(73, 13);
            this.label14.TabIndex = 10;
            this.label14.Text = "SenderDetails";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(24, 205);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(46, 13);
            this.label15.TabIndex = 9;
            this.label15.Text = "Quantity";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(24, 179);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(39, 13);
            this.label16.TabIndex = 8;
            this.label16.Text = "Master";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(24, 281);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(41, 13);
            this.label18.TabIndex = 12;
            this.label18.Text = "Weight";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(24, 255);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(105, 13);
            this.label19.TabIndex = 11;
            this.label19.Text = "ShipmentValueInNIS";
            // 
            // txtForwarderShipmentNumber
            // 
            this.txtForwarderShipmentNumber.Location = new System.Drawing.Point(174, 24);
            this.txtForwarderShipmentNumber.Name = "txtForwarderShipmentNumber";
            this.txtForwarderShipmentNumber.Size = new System.Drawing.Size(181, 20);
            this.txtForwarderShipmentNumber.TabIndex = 13;
            // 
            // txtCustomerAddress
            // 
            this.txtCustomerAddress.Location = new System.Drawing.Point(174, 48);
            this.txtCustomerAddress.Name = "txtCustomerAddress";
            this.txtCustomerAddress.Size = new System.Drawing.Size(181, 20);
            this.txtCustomerAddress.TabIndex = 14;
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Location = new System.Drawing.Point(174, 74);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(181, 20);
            this.txtCustomerName.TabIndex = 15;
            // 
            // txtDeclarationNumber
            // 
            this.txtDeclarationNumber.Location = new System.Drawing.Point(174, 100);
            this.txtDeclarationNumber.Name = "txtDeclarationNumber";
            this.txtDeclarationNumber.Size = new System.Drawing.Size(181, 20);
            this.txtDeclarationNumber.TabIndex = 16;
            // 
            // txtQuantity
            // 
            this.txtQuantity.Location = new System.Drawing.Point(174, 202);
            this.txtQuantity.Name = "txtQuantity";
            this.txtQuantity.Size = new System.Drawing.Size(181, 20);
            this.txtQuantity.TabIndex = 20;
            // 
            // txtMaster
            // 
            this.txtMaster.Location = new System.Drawing.Point(174, 176);
            this.txtMaster.Name = "txtMaster";
            this.txtMaster.Size = new System.Drawing.Size(181, 20);
            this.txtMaster.TabIndex = 19;
            // 
            // txtGoodsDescritpion
            // 
            this.txtGoodsDescritpion.Location = new System.Drawing.Point(174, 150);
            this.txtGoodsDescritpion.Name = "txtGoodsDescritpion";
            this.txtGoodsDescritpion.Size = new System.Drawing.Size(181, 20);
            this.txtGoodsDescritpion.TabIndex = 18;
            // 
            // txtHawb
            // 
            this.txtHawb.Location = new System.Drawing.Point(174, 126);
            this.txtHawb.Name = "txtHawb";
            this.txtHawb.Size = new System.Drawing.Size(181, 20);
            this.txtHawb.TabIndex = 17;
            // 
            // txtShipmentValueInNIS
            // 
            this.txtShipmentValueInNIS.Location = new System.Drawing.Point(174, 252);
            this.txtShipmentValueInNIS.Name = "txtShipmentValueInNIS";
            this.txtShipmentValueInNIS.Size = new System.Drawing.Size(181, 20);
            this.txtShipmentValueInNIS.TabIndex = 22;
            // 
            // txtSenderDetails
            // 
            this.txtSenderDetails.Location = new System.Drawing.Point(174, 228);
            this.txtSenderDetails.Name = "txtSenderDetails";
            this.txtSenderDetails.Size = new System.Drawing.Size(181, 20);
            this.txtSenderDetails.TabIndex = 21;
            // 
            // txtWeight
            // 
            this.txtWeight.Location = new System.Drawing.Point(174, 278);
            this.txtWeight.Name = "txtWeight";
            this.txtWeight.Size = new System.Drawing.Size(181, 20);
            this.txtWeight.TabIndex = 23;
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
            this.tabUserIDNumberRequest.ResumeLayout(false);
            this.tabUserIDNumberRequest.PerformLayout();
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
        private System.Windows.Forms.TabPage tabUserIDNumberRequest;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtWeight;
        private System.Windows.Forms.TextBox txtShipmentValueInNIS;
        private System.Windows.Forms.TextBox txtSenderDetails;
        private System.Windows.Forms.TextBox txtQuantity;
        private System.Windows.Forms.TextBox txtMaster;
        private System.Windows.Forms.TextBox txtGoodsDescritpion;
        private System.Windows.Forms.TextBox txtHawb;
        private System.Windows.Forms.TextBox txtDeclarationNumber;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.TextBox txtCustomerAddress;
        private System.Windows.Forms.TextBox txtForwarderShipmentNumber;
    }
}

