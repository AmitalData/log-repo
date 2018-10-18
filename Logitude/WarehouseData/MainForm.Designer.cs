namespace WarehouseData
{
    partial class MainForm
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
            this.BuildWarehouseDataButton = new System.Windows.Forms.Button();
            this.UpdateWarehouseDataButton = new System.Windows.Forms.Button();
            this.PrivateDBButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BuildWarehouseDataButton
            // 
            this.BuildWarehouseDataButton.Location = new System.Drawing.Point(49, 38);
            this.BuildWarehouseDataButton.Name = "BuildWarehouseDataButton";
            this.BuildWarehouseDataButton.Size = new System.Drawing.Size(148, 28);
            this.BuildWarehouseDataButton.TabIndex = 0;
            this.BuildWarehouseDataButton.Text = "Build Warehouse Data";
            this.BuildWarehouseDataButton.UseVisualStyleBackColor = true;
            this.BuildWarehouseDataButton.Click += new System.EventHandler(this.BuildWarehouseDataButton_Click);
            // 
            // UpdateWarehouseDataButton
            // 
            this.UpdateWarehouseDataButton.Location = new System.Drawing.Point(49, 86);
            this.UpdateWarehouseDataButton.Name = "UpdateWarehouseDataButton";
            this.UpdateWarehouseDataButton.Size = new System.Drawing.Size(148, 28);
            this.UpdateWarehouseDataButton.TabIndex = 1;
            this.UpdateWarehouseDataButton.Text = "Update Warehouse Data";
            this.UpdateWarehouseDataButton.UseVisualStyleBackColor = true;
            this.UpdateWarehouseDataButton.Click += new System.EventHandler(this.UpdateWarehouseDataButton_Click);
            // 
            // PrivateDBButton
            // 
            this.PrivateDBButton.Location = new System.Drawing.Point(49, 132);
            this.PrivateDBButton.Name = "PrivateDBButton";
            this.PrivateDBButton.Size = new System.Drawing.Size(148, 23);
            this.PrivateDBButton.TabIndex = 2;
            this.PrivateDBButton.Text = "Private DB";
            this.PrivateDBButton.UseVisualStyleBackColor = true;
            this.PrivateDBButton.Click += new System.EventHandler(this.PrivateDBButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 304);
            this.Controls.Add(this.PrivateDBButton);
            this.Controls.Add(this.UpdateWarehouseDataButton);
            this.Controls.Add(this.BuildWarehouseDataButton);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BuildWarehouseDataButton;
        private System.Windows.Forms.Button UpdateWarehouseDataButton;
        private System.Windows.Forms.Button PrivateDBButton;
    }
}