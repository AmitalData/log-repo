using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseData.Helper;

namespace WarehouseData
{
    public partial class PrivateDBForm : Form
    {
        string dbSourceConnection = "Logitude2-5_Main,sa,Saas256,.";
        string dbDestinationConnection = "Logitude2-5_Global,sa,Saas256,.";
        long timeOut = 10000000000000000;
        public PrivateDBForm()
        {

            InitializeComponent();
            this.SourceConnectionlTextBox.Text = dbSourceConnection;
            this.DestinationConnectiontextBox.Text = dbDestinationConnection;
            

        }


        private void BuildDataFirstTime_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Start("Build"));
            thread.IsBackground = true;
            thread.Start();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Start("Update"));
            thread.IsBackground = true;
            thread.Start();
        }

        bool IsBuildDataRunning = false;
     
        private void Start(string type)
        {


            if (!string.IsNullOrEmpty(dbSourceConnection) && !string.IsNullOrEmpty(dbDestinationConnection))
            {
                string[] sourceConnectionArray = dbSourceConnection.Split(',');
                string[] destinationConnectionArray = dbDestinationConnection.Split(',');

                if (sourceConnectionArray.Length != 4 || destinationConnectionArray.Length != 4)
                {
                    MessageBox.Show("connection not valid");
                    return;
                }

                if (!IsBuildDataRunning)
                {
                    IsBuildDataRunning = true;

                    WarehouseHelper warehouseHelper = new WarehouseHelper();
                    string sourceConnectionString = warehouseHelper.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
                    try
                    {

                        warehouseHelper.SetControlPropertyValue(label, "Text", type == "Build" ? "Building data..." : "Updating data...");
                        warehouseHelper.SetControlPropertyValue(label, "ForeColor", Color.Black);

                        warehouseHelper.SetControlPropertyValue(PrivateDblabel, "Text", "");
                        warehouseHelper.SetControlPropertyValue(PrivateDblabel, "ForeColor", Color.Black);


                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();
                        string allMessage = "";

                        if (type == "Build") warehouseHelper.CreatePrivateWaterMarksTable(sourceConnectionString);

                        var dWHSettingsTable = warehouseHelper.GetPrivateTenant(sourceConnectionString);

                        string userName = destinationConnectionArray[1];
                        string password = destinationConnectionArray[2];
                        string server = destinationConnectionArray[3];

                        foreach (DataRow row in dWHSettingsTable.Rows)
                        {
                            int tenant = Int32.Parse(row["Tenant"].ToString());
                            string catalog = row["Catalog"].ToString();
                            string message = "Start " + (type == "Build" ? "building" : "updating") + " data on private tenant (" + tenant + ")";

                            if (string.IsNullOrEmpty(allMessage)) allMessage = message + System.Environment.NewLine;
                            else allMessage += (message + System.Environment.NewLine);

                            warehouseHelper.SetControlPropertyValue(PrivateDblabel, "Text", allMessage);

                            Stopwatch stopWatchPrivateDB = new Stopwatch();
                            stopWatchPrivateDB.Start();

                            string destinationConnectionString = warehouseHelper.BuildConnectionString(catalog, userName, password, server);
                            List<int> relatedTenants = warehouseHelper.GetPrivateRelatedTenants(sourceConnectionString, tenant);

                            if (!relatedTenants.Contains(tenant)) relatedTenants.Add(tenant);

                            string tenants = warehouseHelper.ConvertIntgerListToString(relatedTenants);

                            if (type == "Build") warehouseHelper.BuildDataBase(sourceConnectionString, destinationConnectionString, tenant, tenants);
                            else warehouseHelper.UpdateWarehouseData(sourceConnectionString, destinationConnectionString, tenant, tenants);

                            stopWatchPrivateDB.Stop();
                            TimeSpan tsPrivateDB = stopWatchPrivateDB.Elapsed;
                            string replaceMessage = "Updated Private Tenant (" + tenant + ")" + "    Children Tenants" + tenants.Replace(", " + tenant.ToString(), "").Replace(tenant.ToString() + ",", "") + "   Done in ( " + tsPrivateDB.ToString(@"hh\:mm\:ss") + " )";
                            if (type == "Build")
                            {
                                string count = warehouseHelper.GetCount("Fact_Shipments", destinationConnectionString).ToString();
                                replaceMessage = "Private Tenant (" + tenant + ")" + "    Children Tenants" + tenants.Replace(", " + tenant.ToString(), "").Replace(tenant.ToString() + ",", "") + "    Fact Count (" + count + ")  Done in ( " + tsPrivateDB.ToString(@"hh\:mm\:ss") + " )";
                            }

                            allMessage = allMessage.Replace(message, replaceMessage);
                            warehouseHelper.SetControlPropertyValue(PrivateDblabel, "Text", allMessage);

                        }
                        warehouseHelper.SetControlPropertyValue(PrivateDblabel, "ForeColor", Color.Green);
                        IsBuildDataRunning = false;

                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;

                        warehouseHelper.SetControlPropertyValue(label, "Text", "Done in ( " + ts.ToString(@"hh\:mm\:ss") + " )");
                        warehouseHelper.SetControlPropertyValue(label, "ForeColor", Color.Green);
                    }
                    catch (Exception ex)
                    {
                        IsBuildDataRunning = false;
                        MessageBox.Show(ex.Message, ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : ""));

                    }
                }
            }
            else MessageBox.Show("Connection Problem");
        }

        private void SourceConnectionlTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (textbox != null)
            {
                this.dbSourceConnection = textbox.Text;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (textbox != null)
            {
                this.dbDestinationConnection = textbox.Text;
            }


        }



 


    }
}
