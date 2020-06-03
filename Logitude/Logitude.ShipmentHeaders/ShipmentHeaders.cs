using Logitude.ShipmentHeaders.Logitude.ShipmentHeaders.BL.HelperClasses;
using Logitude.ShipmentHeaders.Logitude.ShipmentHeaders.BL.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logitude.ShipmentHeaders
{
    public partial class ShipmentHeaders : Form
    {
        string dbSourceConnection = "Logitude2-5_Main,sa,Saas256,.";//"LogitudeMain-PreR2,logitudemanager,!LO009008,logitudetest.database.windows.net";//"LogitudeMain-Test2,sa,Saas256,logitudetest.cloudapp.net";
        //string dbDestinationConnection = "Logitude2-5_Main,sa,Saas256,.";//"Logitude2-5_Global,sa,Saas256,.";
        ShipmentHeaderService shipmentHeaderService;
        int NumberOfCoulmnUpdated = 0;

        public ShipmentHeaders()
        {
            InitializeComponent();
            shipmentHeaderService = new ShipmentHeaderService();
            this.SourceConnectionlTextBox.Text = dbSourceConnection;
            this.BuildConnectionStrings();
        }

        private void ShipmentHeaders_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => BuildData());
            thread.IsBackground = true;
            thread.Start();

         
        }
        private void button2_Click(object sender, EventArgs e)
        {
           
        }
        private void BuildData() {
            BuildModule("CheckAndUpdateWaterMark", Checking, "Checking WaterMark ...");
            BuildModule("UpdateCargoTables", Bulding);
        }

        private void CheckAndUpdateWaterMark()
        {

            List<CargoTable> CargoTableLists = FillCargoTableList();
            foreach (CargoTable table in CargoTableLists)
            {
                if (table.DBTableName != "WaterMarks")
                {
                    using (SqlConnection DestinationConnection =
                         new SqlConnection(dbSourceConnection))
                    {
                        DestinationConnection.Open();

                        SqlCommand commandSourceData = new SqlCommand(
                       "SELECT  TableName" +
                       " FROM dbo.WaterMarks WHERE TableName = '" + table .Dw_TableName+ "'", DestinationConnection);

                        SqlDataReader reader = commandSourceData.ExecuteReader();
                        if (!reader.HasRows)
                        {
                            string lastUpdateDate = shipmentHeaderService.GetAutomaticLastUpdateDate(table.DBTableName, dbSourceConnection);
                            if (string.IsNullOrEmpty(lastUpdateDate)) lastUpdateDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff tt");
                            shipmentHeaderService.AddWaterMarksRecord(table, lastUpdateDate, dbSourceConnection);

                        }
                        else
                        {
                            DestinationConnection.Close();

                        }
                    }
                }
            }

        }

        private void BuildConnectionStrings()
        {
            string[] sourceConnectionArray = dbSourceConnection.Split(',');
            //dbSourceConnection  string[] destinationConnectionArray = dbSourceConnection.Split(',');

            if (sourceConnectionArray.Length != 4 || sourceConnectionArray.Length != 4)
            {
                MessageBox.Show("connection not valid");
                return;
            }

             dbSourceConnection = shipmentHeaderService.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
         //   dbSourceConnection = shipmentHeaderService.BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);

        }
        private void UpdateCargoDataBase(CargoTable table)
        {
            NumberOfCoulmnUpdated=shipmentHeaderService.UpdateDWDataBase(new CargoArgs() { Table = table, SourceConnectionString = dbSourceConnection, DestinationConnectionString = dbSourceConnection });
        }


        private void UpdateCargoTables()
        {
            List<CargoTable> CargoTableLists = FillCargoTableList();
            foreach (CargoTable table in CargoTableLists)
            {
                UpdateCargoDataBase(table);
            }
        }

        private List<CargoTable>   FillCargoTableList()
        {
            List<CargoTable> CargoTableLists = new List<CargoTable>();
            CargoTableLists.Add(new CargoTable() { TableName = "Shipment", FieldsDBName = "Id,Tenant,CustomerId,TransportModeId,MasterShipmentDataId,House,FromPortId,ToPortId,ShipmentNumber,ShipperId,ConsigneeId,GrossWeight,Volume,CustomConnectToShipment,FirstPickupETA,AutomaticLastUpdateDate", KeyName = "Id", DBTableName = "Shipments" ,Dw_TableName= "ShipmentHeaders" });
            //ShipmentHeaders
            //CargoTableLists.Add(new CargoTable() { TableName = "ShipmentHeader", DBTableName = "ShipmentHeaders" });
            //WaterMark
            //CargoTableLists.Add(new CargoTable() { TableName = "WaterMark", DBTableName = "WaterMarks", KeyName = "TableName", FieldsDBName = "TableName,LastUpdateDate" });

            return CargoTableLists;

        }


        Stopwatch globalStopwatch;
        Label generalLabel;
        private void BuildModule(string name, Label lable,string TextStarting= "Updating...", string TextEnding= "Done in ")
        {
            SetControlPropertyValue(lable, "Text", TextStarting);
            SetControlPropertyValue(lable, "ForeColor", Color.Black); // timer
            if (name == "UpdateCargoTables")
            {
                SetControlPropertyValue(RecordsUpdated, "Text", "");
                SetControlPropertyValue(RecordsUpdated, "ForeColor", Color.Black);
                SetControlPropertyValue(RecordsNumbers, "Text", "");
                SetControlPropertyValue(RecordsNumbers, "ForeColor", Color.Black);
            }
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // for timer
            if (generalLabel != null) SetControlPropertyValue(generalLabel, "Text", TextStarting);
            globalStopwatch = stopWatch;
            generalLabel = lable;
            timer1.Enabled = true;
            timer1.Start();



            globalStopwatch = null;
            generalLabel = null;
            timer1.Start();

            if (name == "CheckAndUpdateWaterMark")
                CheckAndUpdateWaterMark();
            if (name == "UpdateCargoTables")
            {

                UpdateCargoTables();


                SetControlPropertyValue(RecordsUpdated, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
                SetControlPropertyValue(RecordsUpdated, "Text", "Records Updated: ");
                SetControlPropertyValue(RecordsUpdated, "ForeColor", Color.Red);
                SetControlPropertyValue(RecordsNumbers, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
                SetControlPropertyValue(RecordsNumbers, "Text", "( "+NumberOfCoulmnUpdated+" )");
                SetControlPropertyValue(RecordsNumbers, "ForeColor", Color.Black);
            }
               

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            SetControlPropertyValue(lable, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
            SetControlPropertyValue(lable, "ForeColor", Color.Green); // timer
            SetControlPropertyValue(lable, "Text", TextEnding + ts.ToString(@"hh\:mm\:ss"));

        }

        delegate void SetControlValueCallback(Control oControl, string propName, object propValue);
        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (textbox != null)
            {
                this.dbSourceConnection = textbox.Text;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (textbox != null)
            {
               // this.dbDestinationConnection = textbox.Text;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (globalStopwatch != null && generalLabel != null)
            {
                TimeSpan elapsedTime = globalStopwatch.Elapsed;

                SetControlPropertyValue(generalLabel, "Text", "Updating " + elapsedTime.ToString(@"hh\:mm\:ss"));
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
