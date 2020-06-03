namespace Logitude.ShipmentHeaders
{
    partial class ShipmentHeaders
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SourceConnectionlTextBox = new System.Windows.Forms.TextBox();
            this.Checking = new System.Windows.Forms.Label();
            this.Bulding = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.RecordsUpdated = new System.Windows.Forms.Label();
            this.RecordsNumbers = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.button1.Font = new System.Drawing.Font("Constantia", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(129, 114);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(216, 64);
            this.button1.TabIndex = 0;
            this.button1.Text = "Update ShipmentHeaders";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(44)))), ((int)(((byte)(115)))));
            this.label1.Location = new System.Drawing.Point(27, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(162, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Connection Strings";
            // 
            // SourceConnectionlTextBox
            // 
            this.SourceConnectionlTextBox.Location = new System.Drawing.Point(232, 20);
            this.SourceConnectionlTextBox.Name = "SourceConnectionlTextBox";
            this.SourceConnectionlTextBox.Size = new System.Drawing.Size(221, 20);
            this.SourceConnectionlTextBox.TabIndex = 3;
            this.SourceConnectionlTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Checking
            // 
            this.Checking.AutoSize = true;
            this.Checking.Location = new System.Drawing.Point(378, 124);
            this.Checking.Name = "Checking";
            this.Checking.Size = new System.Drawing.Size(16, 13);
            this.Checking.TabIndex = 4;
            this.Checking.Text = "...";
            // 
            // Bulding
            // 
            this.Bulding.AutoSize = true;
            this.Bulding.Location = new System.Drawing.Point(378, 154);
            this.Bulding.Name = "Bulding";
            this.Bulding.Size = new System.Drawing.Size(16, 13);
            this.Bulding.TabIndex = 5;
            this.Bulding.Text = "...";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            // 
            // RecordsUpdated
            // 
            this.RecordsUpdated.AutoSize = true;
            this.RecordsUpdated.Location = new System.Drawing.Point(76, 71);
            this.RecordsUpdated.Name = "RecordsUpdated";
            this.RecordsUpdated.Size = new System.Drawing.Size(16, 13);
            this.RecordsUpdated.TabIndex = 6;
            this.RecordsUpdated.Text = "...";
            // 
            // RecordsNumbers
            // 
            this.RecordsNumbers.AutoSize = true;
            this.RecordsNumbers.Location = new System.Drawing.Point(188, 71);
            this.RecordsNumbers.Name = "RecordsNumbers";
            this.RecordsNumbers.Size = new System.Drawing.Size(16, 13);
            this.RecordsNumbers.TabIndex = 8;
            this.RecordsNumbers.Text = "...";
            this.RecordsNumbers.Click += new System.EventHandler(this.label3_Click);
            // 
            // ShipmentHeaders
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(510, 243);
            this.Controls.Add(this.RecordsNumbers);
            this.Controls.Add(this.RecordsUpdated);
            this.Controls.Add(this.Bulding);
            this.Controls.Add(this.Checking);
            this.Controls.Add(this.SourceConnectionlTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ShipmentHeaders";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShipmentHeaders";
            this.Load += new System.EventHandler(this.ShipmentHeaders_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SourceConnectionlTextBox;
        private System.Windows.Forms.Label Checking;
        private System.Windows.Forms.Label Bulding;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label RecordsUpdated;
        private System.Windows.Forms.Label RecordsNumbers;
    }
}

