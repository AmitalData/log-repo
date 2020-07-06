using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WarehouseDataViews
{
    public partial class Form1 : Form
    {


        // string dbSourceConnection = "LogitudeMain-Test2,sa,Saas256,logitudetestdb.westeurope.cloudapp.azure.com";
        // string dbDestinationConnection = "DWPrivate,sa,Saas256,logitudetestdb.westeurope.cloudapp.azure.com";
        //  private string dbSourceConnection = "Logitude2-5_Main,sa,Saas256,.";
        //  private string dbDestinationConnection = "2019R1_Global,sa,Saas256,.";
        //"UnicargoDW,UnicargoDBUser,Y&P95et1,logitude-ep.database.windows.net";
        // logitudedw-shared,logitudeep,!LO852456,logitude-ep.database.windows.net


        //Locally PrivateDB
        //private int? tenant = 1;
        //string dbSourceConnection = "2020R3_Main,sa,Saas256,.";
        //string dbDestinationConnection = "2020R3_Global,sa,Saas256,.";


        //Pre Private DB
        private int? tenant = 570;
        string dbSourceConnection = "LogitudeMain_PreR3,logitudemanager,!LO009008,logitudetest.database.windows.net";
        string dbDestinationConnection = "UnicargoDW,logitudeep,!LO852456,logitude-ep.database.windows.net";


        //Online PrivateDB
        //private int? tenant = 570;
        //string dbSourceConnection = "LogitudeMain,logitudemanager,!LO852456,ebup282itq.database.windows.net";
        //string dbDestinationConnection = "T570Unicargo,Admin1423,London2015!London2015!,logitudedw1.database.windows.net";

        public Form1()
        {
            InitializeComponent();
            this.SourceConnectionTextBox.Text = dbSourceConnection;
            this.DestinationConnectionTextBox.Text = dbDestinationConnection;
            this.TenantTextBox.Text = tenant.ToString() ;
        }

        private void CreateViewsButton_Click(object sender, EventArgs e)
        {
            CreateDataWarehouseViews();
        }
        private void CreateDataWarehouseViews()
        {
            try
            {
                ResultLabel.Text = "";
                if (!string.IsNullOrEmpty(dbSourceConnection) && !string.IsNullOrEmpty(dbDestinationConnection) && tenant !=null)
                {
                    WarehouseViewsService warehouseViewsService = new WarehouseViewsService((int)tenant);
                    string sourceConnectionString = warehouseViewsService.BuildConnectionString(dbSourceConnection);
                    string destinationConnectionString = warehouseViewsService.BuildConnectionString(dbDestinationConnection);
                    warehouseViewsService.CreateAllDimensionViews(sourceConnectionString, destinationConnectionString);
                    warehouseViewsService.CreateFactShipmentView(sourceConnectionString, destinationConnectionString);
                    SetResultLable(true);
                }
                else MessageBox.Show("Connection Problem");
            }
            catch (Exception ex) 
            {
                DisplayExceptionMessage(ex);
            }
        }

       
        private void DeleteViewsButton_Click(object sender, EventArgs e)
        {

            DeleteDataWarehouseViews();
        }
        private void DeleteDataWarehouseViews()
        {
            try
            {
                ResultLabel.Text = "";
                if (!string.IsNullOrEmpty(dbSourceConnection) && !string.IsNullOrEmpty(dbDestinationConnection) && tenant!=null)
                {
                    WarehouseViewsService warehouseViewsService = new WarehouseViewsService((int)tenant);
                    string sourceConnectionString = warehouseViewsService.BuildConnectionString(dbSourceConnection);
                    string destinationConnectionString = warehouseViewsService.BuildConnectionString(dbDestinationConnection);
                    warehouseViewsService.DeleteDimensionViews(sourceConnectionString, destinationConnectionString);
                    warehouseViewsService.DropView("factShipments", destinationConnectionString);
                    SetResultLable(true);
                }
                else MessageBox.Show("Connection Problem");
            }

            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }

        }
      

        private void DisplayExceptionMessage(Exception ex)
        {
            SetResultLable(false);
            string message = ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : "");
            if (message.Length > 1500) message = message.Substring(0, 1500);
            MessageBox.Show(message);
        }
        private void SetResultLable(bool isSuccess)
        {
            ResultLabel.Text = isSuccess ? "Done" : "Fail";
            ResultLabel.ForeColor = isSuccess ? Color.Green : Color.Red;
        }
        private void SourceConnectionTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox sourceConnectionTextBox = sender as TextBox;
            this.dbSourceConnection = sourceConnectionTextBox.Text;

        }
        private void DestinationConnectionTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox destinationConnectionTextBox = sender as TextBox;
            this.dbDestinationConnection = destinationConnectionTextBox.Text;
        }

        private void TenantTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox destinationConnectionTextBox = sender as TextBox;
            if (!string.IsNullOrEmpty(destinationConnectionTextBox.Text))
            {
                this.tenant = Int32.Parse(destinationConnectionTextBox.Text);
            }
        }
    }
}
