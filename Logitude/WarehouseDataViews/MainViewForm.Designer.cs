namespace WarehouseDataViews
{
    partial class MainViewForm
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
            this.BuildViewsButton = new System.Windows.Forms.Button();
            this.CompareViewButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BuildViewsButton
            // 
            this.BuildViewsButton.Location = new System.Drawing.Point(224, 86);
            this.BuildViewsButton.Name = "BuildViewsButton";
            this.BuildViewsButton.Size = new System.Drawing.Size(307, 23);
            this.BuildViewsButton.TabIndex = 0;
            this.BuildViewsButton.Text = "Build Views";
            this.BuildViewsButton.UseVisualStyleBackColor = true;
            this.BuildViewsButton.Click += new System.EventHandler(this.BuildViewsButton_Click);
            // 
            // CompareViewButton
            // 
            this.CompareViewButton.Location = new System.Drawing.Point(224, 134);
            this.CompareViewButton.Name = "CompareViewButton";
            this.CompareViewButton.Size = new System.Drawing.Size(307, 23);
            this.CompareViewButton.TabIndex = 1;
            this.CompareViewButton.Text = "Compare Views";
            this.CompareViewButton.UseVisualStyleBackColor = true;
            this.CompareViewButton.Click += new System.EventHandler(this.CompareViewButton_Click);
            // 
            // MainViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CompareViewButton);
            this.Controls.Add(this.BuildViewsButton);
            this.Name = "MainViewForm";
            this.Text = "Warehouse Views";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BuildViewsButton;
        private System.Windows.Forms.Button CompareViewButton;
    }
}