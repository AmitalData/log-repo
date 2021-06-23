
namespace Cloud_Rest_Client
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
            this.components = new System.ComponentModel.Container();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.xmlBrowser1 = new XmlRender.XmlBrowser();
            this.btnCopyResponseBody = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCallApi = new System.Windows.Forms.Button();
            this.txtServerUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.txtReponseCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbxLatestStatusOnly = new System.Windows.Forms.CheckBox();
            this.txtHouseNumber = new System.Windows.Forms.TextBox();
            this.lblParameter = new System.Windows.Forms.Label();
            this.cmbOperations = new System.Windows.Forms.ComboBox();
            this.cmbAPI = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // xmlBrowser1
            // 
            this.xmlBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xmlBrowser1.Location = new System.Drawing.Point(17, 486);
            this.xmlBrowser1.Margin = new System.Windows.Forms.Padding(4);
            this.xmlBrowser1.MinimumSize = new System.Drawing.Size(27, 25);
            this.xmlBrowser1.Name = "xmlBrowser1";
            this.xmlBrowser1.Size = new System.Drawing.Size(1040, 400);
            this.xmlBrowser1.TabIndex = 76;
            this.xmlBrowser1.XmlDocument = null;
            this.xmlBrowser1.XmlDocumentTransformType = XmlRender.XmlBrowser.XslTransformType.XSLT10Basic;
            this.xmlBrowser1.XmlText = "";
            // 
            // btnCopyResponseBody
            // 
            this.btnCopyResponseBody.Location = new System.Drawing.Point(1065, 486);
            this.btnCopyResponseBody.Margin = new System.Windows.Forms.Padding(4);
            this.btnCopyResponseBody.Name = "btnCopyResponseBody";
            this.btnCopyResponseBody.Size = new System.Drawing.Size(141, 28);
            this.btnCopyResponseBody.TabIndex = 75;
            this.btnCopyResponseBody.Text = "Copy to Clipboard";
            this.btnCopyResponseBody.UseVisualStyleBackColor = true;
            this.btnCopyResponseBody.Click += new System.EventHandler(this.btnCopyResponseBody_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(651, 16);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Padding = new System.Windows.Forms.Padding(13, 12, 13, 12);
            this.lblMessage.Size = new System.Drawing.Size(543, 100);
            this.lblMessage.TabIndex = 68;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 455);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(140, 20);
            this.label7.TabIndex = 67;
            this.label7.Text = "Response Body";
            // 
            // btnCallApi
            // 
            this.btnCallApi.Enabled = false;
            this.btnCallApi.Location = new System.Drawing.Point(505, 290);
            this.btnCallApi.Margin = new System.Windows.Forms.Padding(4);
            this.btnCallApi.Name = "btnCallApi";
            this.btnCallApi.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnCallApi.Size = new System.Drawing.Size(137, 53);
            this.btnCallApi.TabIndex = 63;
            this.btnCallApi.Text = "Send";
            this.btnCallApi.UseVisualStyleBackColor = true;
            this.btnCallApi.Click += new System.EventHandler(this.btnCallApi_Click);
            // 
            // txtServerUrl
            // 
            this.txtServerUrl.Location = new System.Drawing.Point(180, 25);
            this.txtServerUrl.Margin = new System.Windows.Forms.Padding(4);
            this.txtServerUrl.Name = "txtServerUrl";
            this.txtServerUrl.Size = new System.Drawing.Size(292, 22);
            this.txtServerUrl.TabIndex = 62;
            this.txtServerUrl.Text = "https://test.logitudeworld.com/test/api/";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(15, 28);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 20);
            this.label3.TabIndex = 61;
            this.label3.Text = "Server Url";
            // 
            // txtEmail
            // 
            this.txtEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtEmail.Location = new System.Drawing.Point(111, 42);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(335, 23);
            this.txtEmail.TabIndex = 21;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPassword);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtEmail);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(16, 70);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(476, 126);
            this.groupBox2.TabIndex = 69;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Credentials";
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtPassword.Location = new System.Drawing.Point(111, 81);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(335, 23);
            this.txtPassword.TabIndex = 25;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(8, 81);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 18);
            this.label8.TabIndex = 24;
            this.label8.Text = "Password:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 42);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 18);
            this.label9.TabIndex = 23;
            this.label9.Text = "Email:";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(505, 21);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(137, 28);
            this.btnConnect.TabIndex = 71;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txtReponseCode
            // 
            this.txtReponseCode.Location = new System.Drawing.Point(203, 420);
            this.txtReponseCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtReponseCode.Name = "txtReponseCode";
            this.txtReponseCode.ReadOnly = true;
            this.txtReponseCode.Size = new System.Drawing.Size(303, 22);
            this.txtReponseCode.TabIndex = 66;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 420);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 20);
            this.label6.TabIndex = 65;
            this.label6.Text = "Response Code";
            // 
            // cbxLatestStatusOnly
            // 
            this.cbxLatestStatusOnly.AutoSize = true;
            this.cbxLatestStatusOnly.Location = new System.Drawing.Point(180, 322);
            this.cbxLatestStatusOnly.Margin = new System.Windows.Forms.Padding(4);
            this.cbxLatestStatusOnly.Name = "cbxLatestStatusOnly";
            this.cbxLatestStatusOnly.Size = new System.Drawing.Size(150, 21);
            this.cbxLatestStatusOnly.TabIndex = 102;
            this.cbxLatestStatusOnly.Text = " Latest Status Only";
            this.cbxLatestStatusOnly.UseVisualStyleBackColor = true;
            // 
            // txtHouseNumber
            // 
            this.txtHouseNumber.Location = new System.Drawing.Point(180, 290);
            this.txtHouseNumber.Margin = new System.Windows.Forms.Padding(4);
            this.txtHouseNumber.Name = "txtHouseNumber";
            this.txtHouseNumber.Size = new System.Drawing.Size(284, 22);
            this.txtHouseNumber.TabIndex = 101;
            // 
            // lblParameter
            // 
            this.lblParameter.AutoSize = true;
            this.lblParameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParameter.Location = new System.Drawing.Point(15, 294);
            this.lblParameter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParameter.Name = "lblParameter";
            this.lblParameter.Size = new System.Drawing.Size(63, 20);
            this.lblParameter.TabIndex = 100;
            this.lblParameter.Text = "House";
            // 
            // cmbOperations
            // 
            this.cmbOperations.Enabled = false;
            this.cmbOperations.FormattingEnabled = true;
            this.cmbOperations.Items.AddRange(new object[] {
            "GET"});
            this.cmbOperations.Location = new System.Drawing.Point(180, 236);
            this.cmbOperations.Margin = new System.Windows.Forms.Padding(4);
            this.cmbOperations.Name = "cmbOperations";
            this.cmbOperations.Size = new System.Drawing.Size(292, 24);
            this.cmbOperations.TabIndex = 99;
            // 
            // cmbAPI
            // 
            this.cmbAPI.FormattingEnabled = true;
            this.cmbAPI.Items.AddRange(new object[] {
            "ShipmentStatuses"});
            this.cmbAPI.Location = new System.Drawing.Point(180, 203);
            this.cmbAPI.Margin = new System.Windows.Forms.Padding(4);
            this.cmbAPI.Name = "cmbAPI";
            this.cmbAPI.Size = new System.Drawing.Size(292, 24);
            this.cmbAPI.TabIndex = 98;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(15, 236);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 20);
            this.label4.TabIndex = 97;
            this.label4.Text = "Operation";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(15, 208);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 20);
            this.label1.TabIndex = 96;
            this.label1.Text = "API";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1233, 940);
            this.Controls.Add(this.cbxLatestStatusOnly);
            this.Controls.Add(this.txtHouseNumber);
            this.Controls.Add(this.lblParameter);
            this.Controls.Add(this.cmbOperations);
            this.Controls.Add(this.cmbAPI);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.xmlBrowser1);
            this.Controls.Add(this.btnCopyResponseBody);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnCallApi);
            this.Controls.Add(this.txtServerUrl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtReponseCode);
            this.Controls.Add(this.label6);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Cloud Rest Client";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private XmlRender.XmlBrowser xmlBrowser1;
        private System.Windows.Forms.Button btnCopyResponseBody;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCallApi;
        private System.Windows.Forms.TextBox txtServerUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtReponseCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox cbxLatestStatusOnly;
        private System.Windows.Forms.TextBox txtHouseNumber;
        private System.Windows.Forms.Label lblParameter;
        private System.Windows.Forms.ComboBox cmbOperations;
        private System.Windows.Forms.ComboBox cmbAPI;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
    }
}

