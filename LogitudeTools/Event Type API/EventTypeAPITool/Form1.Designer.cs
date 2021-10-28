namespace CloudRestClientTool
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtRequestContentType = new System.Windows.Forms.TextBox();
            this.rdbXml = new System.Windows.Forms.RadioButton();
            this.btnCopyResponseBody = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.rdbJson = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.ClearButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCredentialsPrimary = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtReponseCode = new System.Windows.Forms.TextBox();
            this.btnCallApi = new System.Windows.Forms.Button();
            this.txtServerUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.xmlBrowser1 = new XmlRender.XmlBrowser();
            this.APILabel = new System.Windows.Forms.Label();
            this.operationCombo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ObjectTableTextBox = new System.Windows.Forms.TextBox();
            this.lblParameter = new System.Windows.Forms.Label();
            this.ConnectedToStatus = new System.Windows.Forms.CheckBox();
            this.connectToTest = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(20, 48);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 20);
            this.label1.TabIndex = 81;
            this.label1.Text = "API";
            // 
            // txtRequestContentType
            // 
            this.txtRequestContentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRequestContentType.Location = new System.Drawing.Point(159, 31);
            this.txtRequestContentType.Margin = new System.Windows.Forms.Padding(4);
            this.txtRequestContentType.Name = "txtRequestContentType";
            this.txtRequestContentType.ReadOnly = true;
            this.txtRequestContentType.Size = new System.Drawing.Size(152, 23);
            this.txtRequestContentType.TabIndex = 3;
            this.txtRequestContentType.Text = "application/xml";
            // 
            // rdbXml
            // 
            this.rdbXml.AutoSize = true;
            this.rdbXml.Checked = true;
            this.rdbXml.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbXml.Location = new System.Drawing.Point(29, 63);
            this.rdbXml.Margin = new System.Windows.Forms.Padding(4);
            this.rdbXml.Name = "rdbXml";
            this.rdbXml.Size = new System.Drawing.Size(56, 24);
            this.rdbXml.TabIndex = 2;
            this.rdbXml.TabStop = true;
            this.rdbXml.Text = "xml";
            this.rdbXml.UseVisualStyleBackColor = true;
            // 
            // btnCopyResponseBody
            // 
            this.btnCopyResponseBody.Location = new System.Drawing.Point(1070, 416);
            this.btnCopyResponseBody.Margin = new System.Windows.Forms.Padding(4);
            this.btnCopyResponseBody.Name = "btnCopyResponseBody";
            this.btnCopyResponseBody.Size = new System.Drawing.Size(141, 46);
            this.btnCopyResponseBody.TabIndex = 80;
            this.btnCopyResponseBody.Text = "Copy to Clipboard";
            this.btnCopyResponseBody.UseVisualStyleBackColor = true;
            this.btnCopyResponseBody.Click += new System.EventHandler(this.btnCopyResponseBody_Click);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(515, 14);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(137, 28);
            this.btnConnect.TabIndex = 76;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // rdbJson
            // 
            this.rdbJson.AutoSize = true;
            this.rdbJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbJson.Location = new System.Drawing.Point(112, 63);
            this.rdbJson.Margin = new System.Windows.Forms.Padding(4);
            this.rdbJson.Name = "rdbJson";
            this.rdbJson.Size = new System.Drawing.Size(61, 24);
            this.rdbJson.TabIndex = 1;
            this.rdbJson.Text = "json";
            this.rdbJson.UseVisualStyleBackColor = true;
            this.rdbJson.CheckedChanged += new System.EventHandler(this.rdbJson_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(24, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "content-type\t";
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(1070, 545);
            this.ClearButton.Margin = new System.Windows.Forms.Padding(4);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(141, 44);
            this.ClearButton.TabIndex = 78;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtCredentialsPrimary);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(16, 220);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(440, 97);
            this.groupBox2.TabIndex = 75;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Credentials";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(8, 42);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 18);
            this.label9.TabIndex = 23;
            this.label9.Text = "Primary Key:";
            // 
            // txtCredentialsPrimary
            // 
            this.txtCredentialsPrimary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtCredentialsPrimary.Location = new System.Drawing.Point(108, 42);
            this.txtCredentialsPrimary.Margin = new System.Windows.Forms.Padding(4);
            this.txtCredentialsPrimary.Name = "txtCredentialsPrimary";
            this.txtCredentialsPrimary.Size = new System.Drawing.Size(292, 23);
            this.txtCredentialsPrimary.TabIndex = 21;
            this.txtCredentialsPrimary.Text = "8099fa61-fe6f-4a04-815e-66f4c2d5834a";
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(512, 50);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(318, 84);
            this.lblMessage.TabIndex = 74;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(26, 392);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(140, 20);
            this.label7.TabIndex = 73;
            this.label7.Text = "Response Body";
            // 
            // txtReponseCode
            // 
            this.txtReponseCode.Location = new System.Drawing.Point(183, 345);
            this.txtReponseCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtReponseCode.Name = "txtReponseCode";
            this.txtReponseCode.ReadOnly = true;
            this.txtReponseCode.Size = new System.Drawing.Size(303, 22);
            this.txtReponseCode.TabIndex = 72;
            // 
            // btnCallApi
            // 
            this.btnCallApi.Enabled = false;
            this.btnCallApi.Location = new System.Drawing.Point(1070, 481);
            this.btnCallApi.Margin = new System.Windows.Forms.Padding(4);
            this.btnCallApi.Name = "btnCallApi";
            this.btnCallApi.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnCallApi.Size = new System.Drawing.Size(141, 46);
            this.btnCallApi.TabIndex = 69;
            this.btnCallApi.Text = "Send";
            this.btnCallApi.UseVisualStyleBackColor = true;
            this.btnCallApi.Click += new System.EventHandler(this.btnCallApi_Click);
            // 
            // txtServerUrl
            // 
            this.txtServerUrl.Location = new System.Drawing.Point(183, 14);
            this.txtServerUrl.Margin = new System.Windows.Forms.Padding(4);
            this.txtServerUrl.Name = "txtServerUrl";
            this.txtServerUrl.Size = new System.Drawing.Size(292, 22);
            this.txtServerUrl.TabIndex = 68;
            this.txtServerUrl.Text = "http://test.logitudeworld.com/test/api/";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(20, 17);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 20);
            this.label3.TabIndex = 67;
            this.label3.Text = "Server Url:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(20, 345);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(141, 20);
            this.label6.TabIndex = 71;
            this.label6.Text = "Response Code";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRequestContentType);
            this.groupBox1.Controls.Add(this.rdbXml);
            this.groupBox1.Controls.Add(this.rdbJson);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(475, 220);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(348, 97);
            this.groupBox1.TabIndex = 66;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Header";
            // 
            // xmlBrowser1
            // 
            this.xmlBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xmlBrowser1.Location = new System.Drawing.Point(24, 416);
            this.xmlBrowser1.Margin = new System.Windows.Forms.Padding(4);
            this.xmlBrowser1.MinimumSize = new System.Drawing.Size(27, 25);
            this.xmlBrowser1.Name = "xmlBrowser1";
            this.xmlBrowser1.Size = new System.Drawing.Size(1022, 286);
            this.xmlBrowser1.TabIndex = 87;
            this.xmlBrowser1.XmlDocument = null;
            this.xmlBrowser1.XmlDocumentTransformType = XmlRender.XmlBrowser.XslTransformType.XSLT10Basic;
            this.xmlBrowser1.XmlText = "";
            // 
            // APILabel
            // 
            this.APILabel.AutoSize = true;
            this.APILabel.Location = new System.Drawing.Point(199, 50);
            this.APILabel.Name = "APILabel";
            this.APILabel.Size = new System.Drawing.Size(87, 17);
            this.APILabel.TabIndex = 88;
            this.APILabel.Text = "Event Types";
            // 
            // operationCombo
            // 
            this.operationCombo.Enabled = false;
            this.operationCombo.FormattingEnabled = true;
            this.operationCombo.Location = new System.Drawing.Point(203, 85);
            this.operationCombo.Margin = new System.Windows.Forms.Padding(4);
            this.operationCombo.Name = "operationCombo";
            this.operationCombo.Size = new System.Drawing.Size(180, 24);
            this.operationCombo.TabIndex = 90;
            this.operationCombo.SelectedIndexChanged += new System.EventHandler(this.operationCombo_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(15, 85);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 20);
            this.label4.TabIndex = 89;
            this.label4.Text = "Operation";
            // 
            // ObjectTableTextBox
            // 
            this.ObjectTableTextBox.Location = new System.Drawing.Point(203, 119);
            this.ObjectTableTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ObjectTableTextBox.Name = "ObjectTableTextBox";
            this.ObjectTableTextBox.Size = new System.Drawing.Size(180, 22);
            this.ObjectTableTextBox.TabIndex = 92;
            // 
            // lblParameter
            // 
            this.lblParameter.AutoSize = true;
            this.lblParameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParameter.Location = new System.Drawing.Point(13, 121);
            this.lblParameter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParameter.Name = "lblParameter";
            this.lblParameter.Size = new System.Drawing.Size(116, 20);
            this.lblParameter.TabIndex = 91;
            this.lblParameter.Text = "Object Table";
            // 
            // ConnectedToStatus
            // 
            this.ConnectedToStatus.AutoSize = true;
            this.ConnectedToStatus.Location = new System.Drawing.Point(202, 162);
            this.ConnectedToStatus.Name = "ConnectedToStatus";
            this.ConnectedToStatus.Size = new System.Drawing.Size(158, 21);
            this.ConnectedToStatus.TabIndex = 93;
            this.ConnectedToStatus.Text = "Events with statuses";
            this.ConnectedToStatus.UseVisualStyleBackColor = true;
            // 
            // connectToTest
            // 
            this.connectToTest.Location = new System.Drawing.Point(686, 14);
            this.connectToTest.Margin = new System.Windows.Forms.Padding(4);
            this.connectToTest.Name = "connectToTest";
            this.connectToTest.Size = new System.Drawing.Size(137, 31);
            this.connectToTest.TabIndex = 94;
            this.connectToTest.Text = "Connect To Test";
            this.connectToTest.UseVisualStyleBackColor = true;
            this.connectToTest.Click += new System.EventHandler(this.connectToTest_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1235, 757);
            this.Controls.Add(this.connectToTest);
            this.Controls.Add(this.ConnectedToStatus);
            this.Controls.Add(this.ObjectTableTextBox);
            this.Controls.Add(this.lblParameter);
            this.Controls.Add(this.operationCombo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.APILabel);
            this.Controls.Add(this.xmlBrowser1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCopyResponseBody);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtReponseCode);
            this.Controls.Add(this.btnCallApi);
            this.Controls.Add(this.txtServerUrl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRequestContentType;
        private System.Windows.Forms.RadioButton rdbXml;
        private System.Windows.Forms.Button btnCopyResponseBody;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.RadioButton rdbJson;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCredentialsPrimary;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtReponseCode;
        private System.Windows.Forms.Button btnCallApi;
        private System.Windows.Forms.TextBox txtServerUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private XmlRender.XmlBrowser xmlBrowser1;
        private System.Windows.Forms.Label APILabel;
        private System.Windows.Forms.ComboBox operationCombo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ObjectTableTextBox;
        private System.Windows.Forms.Label lblParameter;
        private System.Windows.Forms.CheckBox ConnectedToStatus;
        private System.Windows.Forms.Button connectToTest;
    }
}

