using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmitalCustomsWindowsService.Tester.SU
{
    public partial class SignUpForm : Form
    {
        public SignUpForm()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_CopmanyTextBox.Text ))
            {
                MessageBox.Show("_CopmanyTextBox.Text is null");
                return ;
            }

            if (string.IsNullOrWhiteSpace(_EmailtextBox.Text))
            {
                MessageBox.Show("_EmailtextBox.Text is null");
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }


        public string Email
        {
            get
            {
                return _EmailtextBox.Text;
            }
        }

        public string Company
        {
            get
            {
                return _CopmanyTextBox.Text;
            }
        }
    }
}
