using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseData.Helper;
using WarehouseData.Service;

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

                    MainDataWarehouseService mainDataWarehouseService = new MainDataWarehouseService();

                    string sourceConnectionString = mainDataWarehouseService.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
                    try
                    {

                        SetControlPropertyValue(label, "Text", type == "Build" ? "Building data..." : "Updating data...");
                        SetControlPropertyValue(label, "ForeColor", Color.Black);
                        SetControlPropertyValue(PrivateDblabel, "Text", "");
                        SetControlPropertyValue(PrivateDblabel, "ForeColor", Color.Black);


                        Stopwatch stopWatch = new Stopwatch();
                        stopWatch.Start();
                        string allMessage = "";

                        if (type == "Build") mainDataWarehouseService.CreateWaterMarksTable("PrivateWaterMarks", sourceConnectionString, true);

                        var dWHSettingsTable = mainDataWarehouseService.privateTenantDataWarehouse.GetPrivateTenant(sourceConnectionString);

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

                            SetControlPropertyValue(PrivateDblabel, "Text", allMessage);

                            Stopwatch stopWatchPrivateDB = new Stopwatch();
                            stopWatchPrivateDB.Start();

                            string destinationConnectionString = mainDataWarehouseService.BuildConnectionString(catalog, userName, password, server);
                            List<int> relatedTenants = mainDataWarehouseService.privateTenantDataWarehouse.GetPrivateRelatedTenants(sourceConnectionString, tenant);

                            if (!relatedTenants.Contains(tenant)) relatedTenants.Add(tenant);

                            string tenants = mainDataWarehouseService.privateTenantDataWarehouse.ConvertIntgerListToString(relatedTenants);

                            if (type == "Build") mainDataWarehouseService.BuildDataWarehouse(sourceConnectionString, destinationConnectionString, tenant, tenants);
                            else mainDataWarehouseService.UpdateDataWarehouse(sourceConnectionString, destinationConnectionString, tenant, tenants);

                            stopWatchPrivateDB.Stop();
                            TimeSpan tsPrivateDB = stopWatchPrivateDB.Elapsed;
                            string replaceMessage = "Updated Private Tenant (" + tenant + ")" + "    Children Tenants" + tenants.Replace(", " + tenant.ToString(), "").Replace(tenant.ToString() + ",", "") + "   Done in ( " + tsPrivateDB.ToString(@"hh\:mm\:ss") + " )";
                            if (type == "Build")
                            {
                                string count = mainDataWarehouseService.GetRecordDataCountByTableName("Fact_Shipments", destinationConnectionString).ToString();
                                replaceMessage = "Private Tenant (" + tenant + ")" + "    Children Tenants" + tenants.Replace(", " + tenant.ToString(), "").Replace(tenant.ToString() + ",", "") + "    Fact Count (" + count + ")  Done in ( " + tsPrivateDB.ToString(@"hh\:mm\:ss") + " )";
                            }

                            allMessage = allMessage.Replace(message, replaceMessage);
                            SetControlPropertyValue(PrivateDblabel, "Text", allMessage);

                        }
                        SetControlPropertyValue(PrivateDblabel, "ForeColor", Color.Green);
                        IsBuildDataRunning = false;

                        stopWatch.Stop();
                        TimeSpan ts = stopWatch.Elapsed;

                        SetControlPropertyValue(label, "Text", "Done in ( " + ts.ToString(@"hh\:mm\:ss") + " )");
                        SetControlPropertyValue(label, "ForeColor", Color.Green);
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


        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        public void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { oControl, propName, propValue });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if (p.Name.ToUpper() == propName.ToUpper())
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }





    }
}
