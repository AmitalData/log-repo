namespace AmitalCustomsWindowsService.Tester.CustomMessage
{
    partial class LoadTesterForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.listViewDec = new System.Windows.Forms.ListView();
            this.comboBoxTotalRequest = new System.Windows.Forms.ComboBox();
            this.buttonBuildAndListen = new System.Windows.Forms.Button();
            this._textBoxPersonalId = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._radioButtonPersonal = new System.Windows.Forms.RadioButton();
            this._radioButtonCompany = new System.Windows.Forms.RadioButton();
            this._radioButtonNoSign = new System.Windows.Forms.RadioButton();
            this._buttonLoadDoc50 = new System.Windows.Forms.Button();
            this._listViewDocs = new System.Windows.Forms.ListView();
            this._StressWebcheckBox = new System.Windows.Forms.CheckBox();
            this.checkBoxExchangeRate = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load 50";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listViewDec
            // 
            this.listViewDec.Location = new System.Drawing.Point(12, 29);
            this.listViewDec.Name = "listViewDec";
            this.listViewDec.Size = new System.Drawing.Size(542, 97);
            this.listViewDec.TabIndex = 1;
            this.listViewDec.UseCompatibleStateImageBehavior = false;
            this.listViewDec.View = System.Windows.Forms.View.List;
            // 
            // comboBoxTotalRequest
            // 
            this.comboBoxTotalRequest.FormattingEnabled = true;
            this.comboBoxTotalRequest.Items.AddRange(new object[] {
            "10",
            "50",
            "100",
            "150",
            "300",
            "500",
            "1000",
            "1500",
            "2000"});
            this.comboBoxTotalRequest.Location = new System.Drawing.Point(81, 2);
            this.comboBoxTotalRequest.Name = "comboBoxTotalRequest";
            this.comboBoxTotalRequest.Size = new System.Drawing.Size(121, 21);
            this.comboBoxTotalRequest.TabIndex = 2;
            this.comboBoxTotalRequest.Text = "2000";
            // 
            // buttonBuildAndListen
            // 
            this.buttonBuildAndListen.Location = new System.Drawing.Point(414, 330);
            this.buttonBuildAndListen.Name = "buttonBuildAndListen";
            this.buttonBuildAndListen.Size = new System.Drawing.Size(140, 23);
            this.buttonBuildAndListen.TabIndex = 3;
            this.buttonBuildAndListen.Text = "Build And Listen";
            this.buttonBuildAndListen.UseVisualStyleBackColor = true;
            this.buttonBuildAndListen.Click += new System.EventHandler(this.buttonBuildAndListen_Click);
            // 
            // _textBoxPersonalId
            // 
            this._textBoxPersonalId.Location = new System.Drawing.Point(6, 80);
            this._textBoxPersonalId.Name = "_textBoxPersonalId";
            this._textBoxPersonalId.Size = new System.Drawing.Size(117, 20);
            this._textBoxPersonalId.TabIndex = 4;
            this._textBoxPersonalId.Text = "049028392";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._radioButtonPersonal);
            this.groupBox1.Controls.Add(this._textBoxPersonalId);
            this.groupBox1.Controls.Add(this._radioButtonCompany);
            this.groupBox1.Controls.Add(this._radioButtonNoSign);
            this.groupBox1.Location = new System.Drawing.Point(12, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(211, 117);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sign Method";
            // 
            // _radioButtonPersonal
            // 
            this._radioButtonPersonal.AutoSize = true;
            this._radioButtonPersonal.Location = new System.Drawing.Point(129, 83);
            this._radioButtonPersonal.Name = "_radioButtonPersonal";
            this._radioButtonPersonal.Size = new System.Drawing.Size(66, 17);
            this._radioButtonPersonal.TabIndex = 2;
            this._radioButtonPersonal.Text = "Personal";
            this._radioButtonPersonal.UseVisualStyleBackColor = true;
            this._radioButtonPersonal.CheckedChanged += new System.EventHandler(this._radioButtonPersonal_CheckedChanged);
            // 
            // _radioButtonCompany
            // 
            this._radioButtonCompany.AutoSize = true;
            this._radioButtonCompany.Location = new System.Drawing.Point(6, 54);
            this._radioButtonCompany.Name = "_radioButtonCompany";
            this._radioButtonCompany.Size = new System.Drawing.Size(69, 17);
            this._radioButtonCompany.TabIndex = 1;
            this._radioButtonCompany.Text = "Company";
            this._radioButtonCompany.UseVisualStyleBackColor = true;
            this._radioButtonCompany.CheckedChanged += new System.EventHandler(this._radioButtonCompany_CheckedChanged);
            // 
            // _radioButtonNoSign
            // 
            this._radioButtonNoSign.AutoSize = true;
            this._radioButtonNoSign.Checked = true;
            this._radioButtonNoSign.Location = new System.Drawing.Point(6, 31);
            this._radioButtonNoSign.Name = "_radioButtonNoSign";
            this._radioButtonNoSign.Size = new System.Drawing.Size(60, 17);
            this._radioButtonNoSign.TabIndex = 0;
            this._radioButtonNoSign.TabStop = true;
            this._radioButtonNoSign.Text = "NoSign";
            this._radioButtonNoSign.UseVisualStyleBackColor = true;
            // 
            // _buttonLoadDoc50
            // 
            this._buttonLoadDoc50.Location = new System.Drawing.Point(229, 132);
            this._buttonLoadDoc50.Name = "_buttonLoadDoc50";
            this._buttonLoadDoc50.Size = new System.Drawing.Size(123, 23);
            this._buttonLoadDoc50.TabIndex = 5;
            this._buttonLoadDoc50.Text = "Load Doc50";
            this._buttonLoadDoc50.UseVisualStyleBackColor = true;
            this._buttonLoadDoc50.Click += new System.EventHandler(this._buttonLoadDoc50_Click);
            // 
            // _listViewDocs
            // 
            this._listViewDocs.Location = new System.Drawing.Point(229, 163);
            this._listViewDocs.Name = "_listViewDocs";
            this._listViewDocs.Size = new System.Drawing.Size(325, 121);
            this._listViewDocs.TabIndex = 6;
            this._listViewDocs.UseCompatibleStateImageBehavior = false;
            this._listViewDocs.View = System.Windows.Forms.View.List;
            this._listViewDocs.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            // 
            // _StressWebcheckBox
            // 
            this._StressWebcheckBox.AutoSize = true;
            this._StressWebcheckBox.Enabled = false;
            this._StressWebcheckBox.Location = new System.Drawing.Point(229, 6);
            this._StressWebcheckBox.Name = "_StressWebcheckBox";
            this._StressWebcheckBox.Size = new System.Drawing.Size(78, 17);
            this._StressWebcheckBox.TabIndex = 7;
            this._StressWebcheckBox.Text = "StressWeb";
            this._StressWebcheckBox.UseVisualStyleBackColor = true;
            this._StressWebcheckBox.CheckedChanged += new System.EventHandler(this._StressWebcheckBox_CheckedChanged);
            // 
            // checkBoxExchangeRate
            // 
            this.checkBoxExchangeRate.AutoSize = true;
            this.checkBoxExchangeRate.Location = new System.Drawing.Point(458, 290);
            this.checkBoxExchangeRate.Name = "checkBoxExchangeRate";
            this.checkBoxExchangeRate.Size = new System.Drawing.Size(97, 17);
            this.checkBoxExchangeRate.TabIndex = 8;
            this.checkBoxExchangeRate.Text = "ExchangeRate";
            this.checkBoxExchangeRate.UseVisualStyleBackColor = true;
            this.checkBoxExchangeRate.CheckedChanged += new System.EventHandler(this.checkBoxExchangeRate_CheckedChanged);
            // 
            // LoadTesterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 365);
            this.Controls.Add(this.checkBoxExchangeRate);
            this.Controls.Add(this._StressWebcheckBox);
            this.Controls.Add(this._listViewDocs);
            this.Controls.Add(this._buttonLoadDoc50);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonBuildAndListen);
            this.Controls.Add(this.comboBoxTotalRequest);
            this.Controls.Add(this.listViewDec);
            this.Controls.Add(this.button1);
            this.Name = "LoadTesterForm";
            this.Text = "LoadTesterForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listViewDec;
        private System.Windows.Forms.ComboBox comboBoxTotalRequest;
        private System.Windows.Forms.Button buttonBuildAndListen;
        private System.Windows.Forms.TextBox _textBoxPersonalId;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton _radioButtonPersonal;
        private System.Windows.Forms.RadioButton _radioButtonCompany;
        private System.Windows.Forms.RadioButton _radioButtonNoSign;
        private System.Windows.Forms.Button _buttonLoadDoc50;
        private System.Windows.Forms.ListView _listViewDocs;
        private System.Windows.Forms.CheckBox _StressWebcheckBox;
        private System.Windows.Forms.CheckBox checkBoxExchangeRate;

        
    }
}