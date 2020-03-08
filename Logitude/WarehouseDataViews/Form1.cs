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
        string dbSourceConnection = "Logitude2-5_Main,sa,Saas256,.";
        string dbDestinationConnection = "Logitude2-5_Global,sa,Saas256,.";

        //string dbSourceConnection = "LogitudeMain,logitudemanager,!LO852456,ebup282itq.database.windows.net";
        //string dbDestinationConnection = "UnicargoDW, UnicargoDBUser,Y&P95et1,logitude-ep.database.windows.net";

        public Form1()
        {
            InitializeComponent();
            this.SourceConnectionTextBox.Text = dbSourceConnection;
            this.DestinationConnectionTextBox.Text = dbDestinationConnection;
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
                if (!string.IsNullOrEmpty(dbSourceConnection) && !string.IsNullOrEmpty(dbDestinationConnection))
                {
                    WarehouseViewsService warehouseViewsService = new WarehouseViewsService();
                    string sourceConnectionString = warehouseViewsService.BuildConnectionString(dbSourceConnection);
                    string destinationConnectionString = warehouseViewsService.BuildConnectionString(dbDestinationConnection);
                    CreateDimensionViews(sourceConnectionString, destinationConnectionString);
                    CreateFactViews(warehouseViewsService, destinationConnectionString);
                    SetResultLable(true);
                }
                else MessageBox.Show("Connection Problem");
            }
            catch (Exception ex) 
            {
                DisplayExceptionMessage(ex);
            }
        }
        private void CreateDimensionViews(string sourceConnectionString, string destinationConnectionString)
        {
            WarehouseViewsService warehouseViewsService = new WarehouseViewsService();
            DataTable dimensionDWobjectFieldOnFact = warehouseViewsService.GetDimensionDWobjectFieldOnFactShipment(sourceConnectionString);
            foreach (DataRow row in dimensionDWobjectFieldOnFact.Rows)
            {
                string fieldCode=  row["Code"].ToString();
                string dimensionTableCode = row["DimensionTableCode"].ToString();
                if (!string.IsNullOrEmpty(fieldCode) && fieldCode!="[Partner Tenant]" &&  !string.IsNullOrEmpty(dimensionTableCode))
                {
                    if (dimensionTableCode != "DIM_Dates")
                    {
                        warehouseViewsService.DropView(fieldCode, destinationConnectionString);
                        warehouseViewsService.CreateView(fieldCode, dimensionTableCode, destinationConnectionString);
                    }
                    // warehouseViewsService.GrantView(fieldCode, destinationConnectionString);
                }
            }
        }
        private  void CreateFactViews(WarehouseViewsService warehouseViewsService, string destinationConnectionString)
        {
            warehouseViewsService.DropView("Shipment", destinationConnectionString);
            warehouseViewsService.CreateView("Shipment", "Fact_Shipments", destinationConnectionString);
            //  warehouseViewsService.GrantView("Shipment", destinationConnectionString);
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
                if (!string.IsNullOrEmpty(dbSourceConnection) && !string.IsNullOrEmpty(dbDestinationConnection))
                {
                    WarehouseViewsService warehouseViewsService = new WarehouseViewsService();
                    string sourceConnectionString = warehouseViewsService.BuildConnectionString(dbSourceConnection);
                    string destinationConnectionString = warehouseViewsService.BuildConnectionString(dbDestinationConnection);
                    DeleteDimensionViews(sourceConnectionString, destinationConnectionString);
                    warehouseViewsService.DropView("Shipment", destinationConnectionString);
                    SetResultLable(true);
                }
                else MessageBox.Show("Connection Problem");
            }

            catch (Exception ex)
            {
                DisplayExceptionMessage(ex);
            }

        }
        private  void DeleteDimensionViews( string sourceConnectionString, string destinationConnectionString)
        {
            WarehouseViewsService warehouseViewsService = new WarehouseViewsService();
            DataTable dimensionDWobjectFieldOnFact = warehouseViewsService.GetDimensionDWobjectFieldOnFactShipment(sourceConnectionString);
            foreach (DataRow row in dimensionDWobjectFieldOnFact.Rows)
            {
                string fieldCode  = row["Code"].ToString();
                if (!string.IsNullOrEmpty(fieldCode)) warehouseViewsService.DropView(fieldCode, destinationConnectionString);
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

    }
}
