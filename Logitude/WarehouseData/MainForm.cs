using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseData
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void BuildWarehouseDataButton_Click(object sender, EventArgs e)
        {
            BuildWarehouseForm m = new BuildWarehouseForm();
            m.Show();
        }

        private void UpdateWarehouseDataButton_Click(object sender, EventArgs e)
        {
            UpdateWarehouseForm m = new UpdateWarehouseForm();
            m.Show();
        }

        private void PrivateDBButton_Click(object sender, EventArgs e)
        {
            PrivateDBForm m = new PrivateDBForm();
            m.Show();
        }
    }
}
