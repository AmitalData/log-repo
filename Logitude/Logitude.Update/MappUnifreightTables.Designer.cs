
namespace Logitude.Update
{
    partial class MappUnifreightTables
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
            this.label2 = new System.Windows.Forms.Label();
            this.TableName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.KeyTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(487, 335);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(124, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "שם טבלה";
            // 
            // TableName
            // 
            this.TableName.Location = new System.Drawing.Point(246, 83);
            this.TableName.Name = "TableName";
            this.TableName.Size = new System.Drawing.Size(145, 20);
            this.TableName.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(124, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "Key";
            // 
            // KeyTextBox
            // 
            this.KeyTextBox.Location = new System.Drawing.Point(246, 136);
            this.KeyTextBox.Name = "KeyTextBox";
            this.KeyTextBox.Size = new System.Drawing.Size(145, 20);
            this.KeyTextBox.TabIndex = 4;
            // 
            // MappUnifreightTables
            // 
            this.ClientSize = new System.Drawing.Size(646, 414);
            this.Controls.Add(this.KeyTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.TableName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Name = "MappUnifreightTables";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TableName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox KeyTextBox;
    }
}