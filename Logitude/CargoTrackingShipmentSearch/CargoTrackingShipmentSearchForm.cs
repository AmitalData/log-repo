using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CargoTrackingShipmentSearch
{
    public partial class CargoTrackingShipmentSearchForm : Form
    {
        public CargoTrackingShipmentSearchForm()
        {
            InitializeComponent();
        }

        private void CargoTrackingShipmentSearchForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            this.dataGridView1.Columns.Add("33","33");
            this.dataGridView1.Rows.Add("1", "XX");

        }
    }
}
