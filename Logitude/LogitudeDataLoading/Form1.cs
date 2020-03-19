using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogitudeDataLoading
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                int tenant;
                int.TryParse(textBox1.Text, out tenant);

                DataLoading loading = new DataLoading();
               string status = loading.LoadCustomersFromAfile(tenant);
               if (status == "success")
               {
                   label2.Text = "Loading Customers from a file completed successfully.";
               }
               else
               {
                   label2.Text = "Loading Customer from a file failed check the errorlog file.";
               }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                int tenant;
                int.TryParse(textBox1.Text, out tenant);

                DataLoading loading = new DataLoading();
                string status = loading.LoadAgentsFromAFile(tenant);

                if (status == "success")
                {
                    label2.Text = "Loading Agents from a file completed successfully.";
                }
                else
                {
                    label2.Text = "Loading Agents from a file failed check the errorlog file.";
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                int tenant;
                int.TryParse(textBox1.Text, out tenant);

                DataLoading loading = new DataLoading();
                string status = loading.LoadTruckersFromAFile(tenant);

                if (status == "success")
                {
                    label2.Text = "Loading Truckers from a file completed successfully.";
                }
                else
                {
                    label2.Text = "Loading Truckers from a file failed check the errorlog file.";
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                int tenant;
                int.TryParse(textBox1.Text, out tenant);

                DataLoading loading = new DataLoading();
                string status = loading.LoadWarehousesFromAFile(tenant);

                if (status == "success")
                {
                    label2.Text = "Loading Warehouse from a file completed successfully.";
                }
                else
                {
                    label2.Text = "Loading Warehouse from a file failed check the errorlog file.";
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox1.Text))
            {
                int tenant;
                int.TryParse(textBox1.Text, out tenant);

                DataLoading loading = new DataLoading();
                string status = loading.LoadVendorsFromAfile(tenant);
                if (status == "success")
                {
                    label2.Text = "Loading Vendors from a file completed successfully.";
                }
                else
                {
                    label2.Text = "Loading Vendors from a file failed check the errorlog file.";
                }

            }
        }
       
    }
}
