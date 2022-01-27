
namespace External_API_Load_Testing
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
            this.NumberOfShipments = new System.Windows.Forms.TextBox();
            this.JsonText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CreatedShipment = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PeriodMin = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtServerUrl = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtCredentialsPrimary = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.create = new System.Windows.Forms.Button();
            this.createErrorMsg = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // NumberOfShipments
            // 
            this.NumberOfShipments.Location = new System.Drawing.Point(33, 242);
            this.NumberOfShipments.Name = "NumberOfShipments";
            this.NumberOfShipments.Size = new System.Drawing.Size(238, 27);
            this.NumberOfShipments.TabIndex = 0;
            this.NumberOfShipments.Text = "1";
            // 
            // JsonText
            // 
            this.JsonText.Location = new System.Drawing.Point(33, 323);
            this.JsonText.Multiline = true;
            this.JsonText.Name = "JsonText";
            this.JsonText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.JsonText.Size = new System.Drawing.Size(731, 200);
            this.JsonText.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label1.Location = new System.Drawing.Point(29, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 22);
            this.label1.TabIndex = 3;
            this.label1.Text = "Number of shipments";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 9F);
            this.label2.Location = new System.Drawing.Point(356, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 22);
            this.label2.TabIndex = 4;
            this.label2.Text = "Period (sec)";
            // 
            // CreatedShipment
            // 
            this.CreatedShipment.Location = new System.Drawing.Point(33, 607);
            this.CreatedShipment.Multiline = true;
            this.CreatedShipment.Name = "CreatedShipment";
            this.CreatedShipment.Size = new System.Drawing.Size(731, 200);
            this.CreatedShipment.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 584);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 19);
            this.label4.TabIndex = 7;
            this.label4.Text = "Created shipment";
            // 
            // PeriodMin
            // 
            this.PeriodMin.Location = new System.Drawing.Point(360, 242);
            this.PeriodMin.Name = "PeriodMin";
            this.PeriodMin.Size = new System.Drawing.Size(238, 27);
            this.PeriodMin.TabIndex = 8;
            this.PeriodMin.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(39, 16);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 25);
            this.label5.TabIndex = 9;
            this.label5.Text = "Server Url:";
            // 
            // txtServerUrl
            // 
            this.txtServerUrl.Location = new System.Drawing.Point(152, 16);
            this.txtServerUrl.Margin = new System.Windows.Forms.Padding(4);
            this.txtServerUrl.Name = "txtServerUrl";
            this.txtServerUrl.Size = new System.Drawing.Size(328, 27);
            this.txtServerUrl.TabIndex = 10;
            this.txtServerUrl.Text = "https://system.logitudeworld.com/api/";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(514, 10);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(154, 38);
            this.btnConnect.TabIndex = 25;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtCredentialsPrimary);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(33, 75);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(495, 102);
            this.groupBox2.TabIndex = 26;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Credentials";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(9, 50);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 22);
            this.label9.TabIndex = 23;
            this.label9.Text = "Primary Key:";
            // 
            // txtCredentialsPrimary
            // 
            this.txtCredentialsPrimary.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtCredentialsPrimary.Location = new System.Drawing.Point(124, 50);
            this.txtCredentialsPrimary.Margin = new System.Windows.Forms.Padding(4);
            this.txtCredentialsPrimary.Name = "txtCredentialsPrimary";
            this.txtCredentialsPrimary.Size = new System.Drawing.Size(328, 26);
            this.txtCredentialsPrimary.TabIndex = 21;
            this.txtCredentialsPrimary.Text = "e5ce73ab-e497-4177-9b32-dd556e9be5c9\r\n";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(510, 51);
            this.lblMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 20);
            this.lblMessage.TabIndex = 27;
            // 
            // create
            // 
            this.create.Enabled = false;
            this.create.Location = new System.Drawing.Point(610, 530);
            this.create.Margin = new System.Windows.Forms.Padding(4);
            this.create.Name = "create";
            this.create.Size = new System.Drawing.Size(154, 38);
            this.create.TabIndex = 28;
            this.create.Text = "Create";
            this.create.UseVisualStyleBackColor = true;
            this.create.Click += new System.EventHandler(this.create_Click);
            // 
            // createErrorMsg
            // 
            this.createErrorMsg.AutoSize = true;
            this.createErrorMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createErrorMsg.Location = new System.Drawing.Point(29, 526);
            this.createErrorMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.createErrorMsg.Name = "createErrorMsg";
            this.createErrorMsg.Size = new System.Drawing.Size(0, 20);
            this.createErrorMsg.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 301);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "Shipment Json";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(800, 840);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.createErrorMsg);
            this.Controls.Add(this.create);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtServerUrl);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PeriodMin);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.CreatedShipment);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.JsonText);
            this.Controls.Add(this.NumberOfShipments);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NumberOfShipments;
        private System.Windows.Forms.TextBox JsonText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CreatedShipment;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PeriodMin;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtServerUrl;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCredentialsPrimary;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button create;
        private System.Windows.Forms.Label createErrorMsg;
        private System.Windows.Forms.Label label3;
    }
}

