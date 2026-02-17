namespace WarehouseData
{
    partial class PrivateDBForm
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
            this.SourceConnectionlTextBox = new System.Windows.Forms.TextBox();
            this.SourceConnectionlabel = new System.Windows.Forms.Label();
            this.BuildWarehouseBotton = new System.Windows.Forms.Button();
            this.label = new System.Windows.Forms.Label();
            this.PrivateDblabel = new System.Windows.Forms.Label();
            this.DestinationConnectiontextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BuildWarehouseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SourceConnectionlTextBox
            // 
            this.SourceConnectionlTextBox.Location = new System.Drawing.Point(216, 14);
            this.SourceConnectionlTextBox.Name = "SourceConnectionlTextBox";
            this.SourceConnectionlTextBox.Size = new System.Drawing.Size(484, 20);
            this.SourceConnectionlTextBox.TabIndex = 145;
            this.SourceConnectionlTextBox.TextChanged += new System.EventHandler(this.SourceConnectionlTextBox_TextChanged);
            // 
            // SourceConnectionlabel
            // 
            this.SourceConnectionlabel.AutoSize = true;
            this.SourceConnectionlabel.BackColor = System.Drawing.SystemColors.Control;
            this.SourceConnectionlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SourceConnectionlabel.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.SourceConnectionlabel.Location = new System.Drawing.Point(19, 15);
            this.SourceConnectionlabel.Name = "SourceConnectionlabel";
            this.SourceConnectionlabel.Size = new System.Drawing.Size(145, 20);
            this.SourceConnectionlabel.TabIndex = 143;
            this.SourceConnectionlabel.Text = "Source Connection";
            // 
            // BuildWarehouseBotton
            // 
            this.BuildWarehouseBotton.Location = new System.Drawing.Point(216, 66);
            this.BuildWarehouseBotton.Name = "BuildWarehouseBotton";
            this.BuildWarehouseBotton.Size = new System.Drawing.Size(484, 23);
            this.BuildWarehouseBotton.TabIndex = 142;
            this.BuildWarehouseBotton.Text = "Build warehouse data on private db";
            this.BuildWarehouseBotton.UseVisualStyleBackColor = true;
            this.BuildWarehouseBotton.Click += new System.EventHandler(this.BuildDataFirstTime_Click);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.BackColor = System.Drawing.SystemColors.Control;
            this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label.Location = new System.Drawing.Point(19, 69);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(0, 20);
            this.label.TabIndex = 146;
            // 
            // PrivateDblabel
            // 
            this.PrivateDblabel.AutoSize = true;
            this.PrivateDblabel.BackColor = System.Drawing.SystemColors.Control;
            this.PrivateDblabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PrivateDblabel.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.PrivateDblabel.Location = new System.Drawing.Point(212, 145);
            this.PrivateDblabel.Name = "PrivateDblabel";
            this.PrivateDblabel.Size = new System.Drawing.Size(0, 20);
            this.PrivateDblabel.TabIndex = 147;
            // 
            // DestinationConnectiontextBox
            // 
            this.DestinationConnectiontextBox.Location = new System.Drawing.Point(216, 40);
            this.DestinationConnectiontextBox.Name = "DestinationConnectiontextBox";
            this.DestinationConnectiontextBox.Size = new System.Drawing.Size(484, 20);
            this.DestinationConnectiontextBox.TabIndex = 148;
            this.DestinationConnectiontextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.label1.Location = new System.Drawing.Point(19, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 20);
            this.label1.TabIndex = 149;
            this.label1.Text = "Destination Connection";
            // 
            // BuildWarehouseButton
            // 
            this.BuildWarehouseButton.Location = new System.Drawing.Point(216, 95);
            this.BuildWarehouseButton.Name = "BuildWarehouseButton";
            this.BuildWarehouseButton.Size = new System.Drawing.Size(484, 23);
            this.BuildWarehouseButton.TabIndex = 150;
            this.BuildWarehouseButton.Text = "Update warehouse data on private db";
            this.BuildWarehouseButton.UseVisualStyleBackColor = true;
            this.BuildWarehouseButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // PrivateDBForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 424);
            this.Controls.Add(this.BuildWarehouseButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DestinationConnectiontextBox);
            this.Controls.Add(this.PrivateDblabel);
            this.Controls.Add(this.label);
            this.Controls.Add(this.SourceConnectionlTextBox);
            this.Controls.Add(this.SourceConnectionlabel);
            this.Controls.Add(this.BuildWarehouseBotton);
            this.Name = "PrivateDBForm";
            this.Text = "Private DB";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox SourceConnectionlTextBox;
        private System.Windows.Forms.Label SourceConnectionlabel;
        private System.Windows.Forms.Button BuildWarehouseBotton;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.Label PrivateDblabel;
        private System.Windows.Forms.TextBox DestinationConnectiontextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BuildWarehouseButton;
    }
}