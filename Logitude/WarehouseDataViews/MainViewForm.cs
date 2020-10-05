using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseDataViews
{
    public partial class MainViewForm : Form
    {
        public MainViewForm()
        {
            InitializeComponent();
        }

        private void BuildViewsButton_Click(object sender, EventArgs e)
        {
            Form1 m = new Form1();
            m.Show();
        }

        private void CompareViewButton_Click(object sender, EventArgs e)
        {
            CompareViewForm m = new CompareViewForm();
            m.Show();
        }
    }
}
