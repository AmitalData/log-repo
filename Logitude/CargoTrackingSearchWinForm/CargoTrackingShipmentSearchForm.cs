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

namespace CargoTrackingSearchWinForm
{
    public partial class CargoTrackingShipmentSearchForm : Form
    {
        private string LocalConectionstring = "CargoTracking,sa,Saas256,.";
        private string TestConectionstring = "Main-Test,sa,Saas256,amitaltestdb.westeurope.cloudapp.azure.com";
        private string CloudConectionstring = "Main,sa,Saas256,amitaldata.cloudapp.net";
        private string CurrentConectionstring;
        public CargoTrackingShipmentSearchForm()
        {
            CurrentConectionstring = LocalConectionstring;
            InitializeComponent();
        }

        private void CargoTrackingShipmentSearchForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();
        }
 
        private void LoadData( )
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                string sqlQueryStr = "SELECT top 10 * FROM ( SELECT  Id, Tenant, SearchFields, ShipmentId, ShipmentDate, ROW_NUMBER() OVER(PARTITION BY ShipmentId ORDER BY ID DESC) rn FROM CargoTrackingShipmentSearches WHERE SearchFields = '" + textBox1.Text + "' ) a WHERE rn = 1 Order by ShipmentDate DESC";
 
                SqlConnection conn = new SqlConnection();
                try
                {
                    string[] ConnectionSplitted = CurrentConectionstring.Split(',');
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                    builder.DataSource = ConnectionSplitted[3];
                    builder.InitialCatalog = ConnectionSplitted[0];
                    builder.IntegratedSecurity = false;
                    builder.PersistSecurityInfo = true;
                    builder.UserID = ConnectionSplitted[1];
                    builder.Password = ConnectionSplitted[2];
                    builder.MultipleActiveResultSets = true;

                    conn.ConnectionString = builder.ConnectionString;

                    // Connect
                    conn.Open();

                    using (SqlDataAdapter myAdapter = new SqlDataAdapter(sqlQueryStr, conn))
                    {
                        // Use DataAdapter to fill DataTable
                        DataTable myTable = new DataTable();
                        myAdapter.Fill(myTable);

                        // Render data onto the screen
                        dataGridView1.DataSource = myTable;
                        dataGridView1.Columns.Remove("rn");
                    }
                }
                catch (SqlException e)
                {
                    MessageBox.Show("Error accessing database: { 0}", e.Message);
                }
                finally
                {
                    conn.Close();
                }

            }
            else
            {
                MessageBox.Show("Type Text To Start Search ...");

            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            CurrentConectionstring = LocalConectionstring;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            CurrentConectionstring = TestConectionstring;

        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            CurrentConectionstring = CloudConectionstring;

        }
    }
}
