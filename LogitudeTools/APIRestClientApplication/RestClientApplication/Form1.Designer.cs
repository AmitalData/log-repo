namespace RestClientApplication
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
            this.txtRequestBody = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRequestContentType = new System.Windows.Forms.TextBox();
            this.rdbXml = new System.Windows.Forms.RadioButton();
            this.rdbJson = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtServerUrl = new System.Windows.Forms.TextBox();
            this.btnCallApi = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtReponseCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.txtCredentialsPrimary = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnCopyToClipboard = new System.Windows.Forms.Button();
            this.btnCopyResponseBody = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.xmlBrowser1 = new XmlRender.XmlBrowser();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.apiCombo = new System.Windows.Forms.ComboBox();
            this.operationCombo = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ActionLabel = new System.Windows.Forms.Label();
            this.actionCombo = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.txtParameter = new System.Windows.Forms.TextBox();
            this.lblParameter = new System.Windows.Forms.Label();
            this.txtParameter2 = new System.Windows.Forms.TextBox();
            this.lblParameter2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.CopyResponse = new System.Windows.Forms.Button();
            this.Post1000ARInvoice = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.count1000 = new System.Windows.Forms.Label();
            this.postCountTxt = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button4 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtRequestBody
            // 
            this.txtRequestBody.AcceptsReturn = true;
            this.txtRequestBody.Location = new System.Drawing.Point(19, 309);
            this.txtRequestBody.Multiline = true;
            this.txtRequestBody.Name = "txtRequestBody";
            this.txtRequestBody.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRequestBody.Size = new System.Drawing.Size(767, 155);
            this.txtRequestBody.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtRequestContentType);
            this.groupBox1.Controls.Add(this.rdbXml);
            this.groupBox1.Controls.Add(this.rdbJson);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(362, 204);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 79);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Header";
            // 
            // txtRequestContentType
            // 
            this.txtRequestContentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRequestContentType.Location = new System.Drawing.Point(119, 25);
            this.txtRequestContentType.Name = "txtRequestContentType";
            this.txtRequestContentType.ReadOnly = true;
            this.txtRequestContentType.Size = new System.Drawing.Size(115, 20);
            this.txtRequestContentType.TabIndex = 3;
            this.txtRequestContentType.Text = "application/xml";
            // 
            // rdbXml
            // 
            this.rdbXml.AutoSize = true;
            this.rdbXml.Checked = true;
            this.rdbXml.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbXml.Location = new System.Drawing.Point(22, 51);
            this.rdbXml.Name = "rdbXml";
            this.rdbXml.Size = new System.Drawing.Size(46, 20);
            this.rdbXml.TabIndex = 2;
            this.rdbXml.TabStop = true;
            this.rdbXml.Text = "xml";
            this.rdbXml.UseVisualStyleBackColor = true;
            // 
            // rdbJson
            // 
            this.rdbJson.AutoSize = true;
            this.rdbJson.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdbJson.Location = new System.Drawing.Point(84, 51);
            this.rdbJson.Name = "rdbJson";
            this.rdbJson.Size = new System.Drawing.Size(51, 20);
            this.rdbJson.TabIndex = 1;
            this.rdbJson.Text = "json";
            this.rdbJson.UseVisualStyleBackColor = true;
            this.rdbJson.CheckedChanged += new System.EventHandler(this.rdbJson_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "content-type\t";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Server Url:";
            // 
            // txtServerUrl
            // 
            this.txtServerUrl.Location = new System.Drawing.Point(134, 17);
            this.txtServerUrl.Name = "txtServerUrl";
            this.txtServerUrl.Size = new System.Drawing.Size(220, 20);
            this.txtServerUrl.TabIndex = 5;
            this.txtServerUrl.Text = "http://localhost:9996/api/";
            // 
            // btnCallApi
            // 
            this.btnCallApi.Enabled = false;
            this.btnCallApi.Location = new System.Drawing.Point(807, 338);
            this.btnCallApi.Name = "btnCallApi";
            this.btnCallApi.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnCallApi.Size = new System.Drawing.Size(103, 37);
            this.btnCallApi.TabIndex = 6;
            this.btnCallApi.Text = "Send";
            this.btnCallApi.UseVisualStyleBackColor = true;
            this.btnCallApi.Click += new System.EventHandler(this.btnCallApi_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(15, 286);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(109, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "Request Body";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(17, 486);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "Response Code";
            // 
            // txtReponseCode
            // 
            this.txtReponseCode.Location = new System.Drawing.Point(160, 486);
            this.txtReponseCode.Name = "txtReponseCode";
            this.txtReponseCode.ReadOnly = true;
            this.txtReponseCode.Size = new System.Drawing.Size(228, 20);
            this.txtReponseCode.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(17, 515);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(121, 17);
            this.label7.TabIndex = 14;
            this.label7.Text = "Response Body";
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(693, 20);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(131, 39);
            this.lblMessage.TabIndex = 16;
            // 
            // txtCredentialsPrimary
            // 
            this.txtCredentialsPrimary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtCredentialsPrimary.Location = new System.Drawing.Point(83, 34);
            this.txtCredentialsPrimary.Name = "txtCredentialsPrimary";
            this.txtCredentialsPrimary.Size = new System.Drawing.Size(220, 20);
            this.txtCredentialsPrimary.TabIndex = 21;
            this.txtCredentialsPrimary.Text = "8234e5d4-e6c5-47c8-8c2d-67875e5a3edf";
            this.txtCredentialsPrimary.TextChanged += new System.EventHandler(this.txtCredentialsPrimary_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtCredentialsPrimary);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(18, 204);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(330, 79);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Credentials";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 15);
            this.label9.TabIndex = 23;
            this.label9.Text = "Primary Key:";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(378, 14);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(103, 23);
            this.btnConnect.TabIndex = 24;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnCopyToClipboard
            // 
            this.btnCopyToClipboard.Location = new System.Drawing.Point(807, 309);
            this.btnCopyToClipboard.Name = "btnCopyToClipboard";
            this.btnCopyToClipboard.Size = new System.Drawing.Size(103, 23);
            this.btnCopyToClipboard.TabIndex = 25;
            this.btnCopyToClipboard.Text = "Copy to Clipboard";
            this.btnCopyToClipboard.UseVisualStyleBackColor = true;
            this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
            // 
            // btnCopyResponseBody
            // 
            this.btnCopyResponseBody.Location = new System.Drawing.Point(807, 540);
            this.btnCopyResponseBody.Name = "btnCopyResponseBody";
            this.btnCopyResponseBody.Size = new System.Drawing.Size(106, 23);
            this.btnCopyResponseBody.TabIndex = 26;
            this.btnCopyResponseBody.Text = "Copy to Clipboard";
            this.btnCopyResponseBody.UseVisualStyleBackColor = true;
            this.btnCopyResponseBody.Click += new System.EventHandler(this.btnCopyResponseBody_Click);
            // 
            // xmlBrowser1
            // 
            this.xmlBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.xmlBrowser1.Location = new System.Drawing.Point(21, 540);
            this.xmlBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.xmlBrowser1.Name = "xmlBrowser1";
            this.xmlBrowser1.Size = new System.Drawing.Size(765, 183);
            this.xmlBrowser1.TabIndex = 29;
            this.xmlBrowser1.XmlDocument = null;
            this.xmlBrowser1.XmlDocumentTransformType = XmlRender.XmlBrowser.XslTransformType.XSLT10Basic;
            this.xmlBrowser1.XmlText = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(15, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 17);
            this.label1.TabIndex = 30;
            this.label1.Text = "API";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(10, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 17);
            this.label4.TabIndex = 31;
            this.label4.Text = "Operation";
            // 
            // apiCombo
            // 
            this.apiCombo.FormattingEnabled = true;
            this.apiCombo.Location = new System.Drawing.Point(134, 43);
            this.apiCombo.Name = "apiCombo";
            this.apiCombo.Size = new System.Drawing.Size(136, 21);
            this.apiCombo.TabIndex = 38;
            this.apiCombo.SelectedIndexChanged += new System.EventHandler(this.apiCombo_SelectedIndexChanged);
            // 
            // operationCombo
            // 
            this.operationCombo.FormattingEnabled = true;
            this.operationCombo.Location = new System.Drawing.Point(134, 70);
            this.operationCombo.Name = "operationCombo";
            this.operationCombo.Size = new System.Drawing.Size(136, 21);
            this.operationCombo.TabIndex = 39;
            this.operationCombo.SelectedIndexChanged += new System.EventHandler(this.operationCombo_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.label8.Location = new System.Drawing.Point(279, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(392, 13);
            this.label8.TabIndex = 40;
            this.label8.Text = "If you selected Quote API, please select an action and also select PUT operation!" +
    "";
            // 
            // ActionLabel
            // 
            this.ActionLabel.AutoSize = true;
            this.ActionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionLabel.Location = new System.Drawing.Point(12, 107);
            this.ActionLabel.Name = "ActionLabel";
            this.ActionLabel.Size = new System.Drawing.Size(53, 17);
            this.ActionLabel.TabIndex = 41;
            this.ActionLabel.Text = "Action";
            this.ActionLabel.Visible = false;
            // 
            // actionCombo
            // 
            this.actionCombo.FormattingEnabled = true;
            this.actionCombo.Location = new System.Drawing.Point(134, 105);
            this.actionCombo.Name = "actionCombo";
            this.actionCombo.Size = new System.Drawing.Size(136, 21);
            this.actionCombo.TabIndex = 42;
            this.actionCombo.Visible = false;
            this.actionCombo.SelectedIndexChanged += new System.EventHandler(this.actionCombo_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // txtParameter
            // 
            this.txtParameter.Location = new System.Drawing.Point(134, 140);
            this.txtParameter.Name = "txtParameter";
            this.txtParameter.Size = new System.Drawing.Size(159, 20);
            this.txtParameter.TabIndex = 45;
            this.txtParameter.Visible = false;
            // 
            // lblParameter
            // 
            this.lblParameter.AutoSize = true;
            this.lblParameter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParameter.Location = new System.Drawing.Point(15, 141);
            this.lblParameter.Name = "lblParameter";
            this.lblParameter.Size = new System.Drawing.Size(88, 17);
            this.lblParameter.TabIndex = 44;
            this.lblParameter.Text = "Parameter:";
            this.lblParameter.Visible = false;
            // 
            // txtParameter2
            // 
            this.txtParameter2.Location = new System.Drawing.Point(472, 142);
            this.txtParameter2.Name = "txtParameter2";
            this.txtParameter2.Size = new System.Drawing.Size(142, 20);
            this.txtParameter2.TabIndex = 47;
            this.txtParameter2.Visible = false;
            // 
            // lblParameter2
            // 
            this.lblParameter2.AutoSize = true;
            this.lblParameter2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblParameter2.Location = new System.Drawing.Point(371, 143);
            this.lblParameter2.Name = "lblParameter2";
            this.lblParameter2.Size = new System.Drawing.Size(88, 17);
            this.lblParameter2.TabIndex = 46;
            this.lblParameter2.Text = "Parameter:";
            this.lblParameter2.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(134, 164);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(117, 20);
            this.textBox1.TabIndex = 48;
            this.textBox1.Visible = false;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(406, 164);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(102, 20);
            this.textBox2.TabIndex = 49;
            this.textBox2.Visible = false;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(18, 167);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(26, 17);
            this.label10.TabIndex = 50;
            this.label10.Text = "Id:";
            this.label10.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(258, 2);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(127, 17);
            this.label11.TabIndex = 51;
            this.label11.Text = "Display Number:";
            this.label11.Visible = false;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(655, 164);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(110, 20);
            this.textBox3.TabIndex = 52;
            this.textBox3.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(522, 165);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(129, 17);
            this.label12.TabIndex = 53;
            this.label12.Text = "Internal Number:";
            this.label12.Visible = false;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(134, 141);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(159, 20);
            this.textBox4.TabIndex = 54;
            this.textBox4.Visible = false;
            // 
            // CopyResponse
            // 
            this.CopyResponse.Location = new System.Drawing.Point(38, 83);
            this.CopyResponse.Name = "CopyResponse";
            this.CopyResponse.Size = new System.Drawing.Size(120, 23);
            this.CopyResponse.TabIndex = 55;
            this.CopyResponse.Text = "Copy Response";
            this.CopyResponse.UseVisualStyleBackColor = true;
            this.CopyResponse.Visible = false;
            this.CopyResponse.Click += new System.EventHandler(this.CopyResponse1000Btn_Click);
            // 
            // Post1000ARInvoice
            // 
            this.Post1000ARInvoice.Location = new System.Drawing.Point(61, 19);
            this.Post1000ARInvoice.Name = "Post1000ARInvoice";
            this.Post1000ARInvoice.Size = new System.Drawing.Size(70, 26);
            this.Post1000ARInvoice.TabIndex = 55;
            this.Post1000ARInvoice.Text = "Post ×";
            this.Post1000ARInvoice.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Post1000ARInvoice.UseVisualStyleBackColor = true;
            this.Post1000ARInvoice.Click += new System.EventHandler(this.Post1000ARInvoice_ClickAsync);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(487, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 23);
            this.button1.TabIndex = 24;
            this.button1.Text = "Connect 2 test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnConnect_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(807, 381);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(103, 23);
            this.button2.TabIndex = 25;
            this.button2.Text = "Clear";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(807, 410);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 23);
            this.button3.TabIndex = 25;
            this.button3.Text = "Paste";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // count1000
            // 
            this.count1000.AutoSize = true;
            this.count1000.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.count1000.Location = new System.Drawing.Point(14, 60);
            this.count1000.MinimumSize = new System.Drawing.Size(20, 0);
            this.count1000.Name = "count1000";
            this.count1000.Size = new System.Drawing.Size(20, 17);
            this.count1000.TabIndex = 56;
            this.count1000.Text = "...";
            // 
            // postCountTxt
            // 
            this.postCountTxt.Location = new System.Drawing.Point(137, 25);
            this.postCountTxt.Name = "postCountTxt";
            this.postCountTxt.Size = new System.Drawing.Size(36, 20);
            this.postCountTxt.TabIndex = 57;
            this.postCountTxt.Text = "100";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button4);
            this.groupBox3.Controls.Add(this.Post1000ARInvoice);
            this.groupBox3.Controls.Add(this.CopyResponse);
            this.groupBox3.Controls.Add(this.postCountTxt);
            this.groupBox3.Controls.Add(this.count1000);
            this.groupBox3.Location = new System.Drawing.Point(710, 191);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 112);
            this.groupBox3.TabIndex = 58;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Multi Post";
            this.groupBox3.Visible = false;
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(17, 19);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(38, 26);
            this.button4.TabIndex = 55;
            this.button4.Text = "Get";
            this.button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.GetARInvoiceBtn_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.textBox6);
            this.panel1.Controls.Add(this.textBox5);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Location = new System.Drawing.Point(21, 165);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(596, 39);
            this.panel1.TabIndex = 59;
            this.panel1.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(-3, 14);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(97, 17);
            this.label14.TabIndex = 56;
            this.label14.Text = "External No:";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(113, 12);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(159, 20);
            this.textBox6.TabIndex = 55;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(451, 13);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(142, 20);
            this.textBox5.TabIndex = 52;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(309, 13);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(129, 17);
            this.label13.TabIndex = 53;
            this.label13.Text = "External System:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(642, 142);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(88, 17);
            this.label16.TabIndex = 61;
            this.label16.Text = "Parameter:";
            this.label16.Visible = false;
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(734, 140);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(142, 20);
            this.textBox7.TabIndex = 62;
            this.textBox7.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 742);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.txtParameter2);
            this.Controls.Add(this.lblParameter2);
            this.Controls.Add(this.txtParameter);
            this.Controls.Add(this.lblParameter);
            this.Controls.Add(this.actionCombo);
            this.Controls.Add(this.ActionLabel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.operationCombo);
            this.Controls.Add(this.apiCombo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.xmlBrowser1);
            this.Controls.Add(this.btnCopyResponseBody);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnCopyToClipboard);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtReponseCode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCallApi);
            this.Controls.Add(this.txtServerUrl);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtRequestBody);
            this.Name = "Form1";
            this.Text = "Logitude Rest Client";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtRequestBody;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtServerUrl;
        private System.Windows.Forms.Button btnCallApi;
        private System.Windows.Forms.TextBox txtRequestContentType;
        private System.Windows.Forms.RadioButton rdbXml;
        private System.Windows.Forms.RadioButton rdbJson;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtReponseCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.TextBox txtCredentialsPrimary;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnCopyToClipboard;
        private System.Windows.Forms.Button btnCopyResponseBody;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private XmlRender.XmlBrowser xmlBrowser1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox apiCombo;
        private System.Windows.Forms.ComboBox operationCombo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label ActionLabel;
        private System.Windows.Forms.ComboBox actionCombo;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox txtParameter;
        private System.Windows.Forms.Label lblParameter;
		private System.Windows.Forms.TextBox txtParameter2;
		private System.Windows.Forms.Label lblParameter2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Button CopyResponse;
        private System.Windows.Forms.Button Post1000ARInvoice;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label count1000;
        private System.Windows.Forms.TextBox postCountTxt;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBox7;
    }
}

