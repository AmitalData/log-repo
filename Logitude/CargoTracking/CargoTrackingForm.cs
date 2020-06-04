using CargoTracking.CargoTracking.BL.HelperClasses;
using CargoTracking.CargoTracking.BL.Services;
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

namespace CargoTracking.Forms
{
    public partial class CargoTrackingForm : Form
    {
        private string dbSourceConnection = "Logitude2-5_Main,sa,Saas256,.";//"LogitudeMain-PreR2,logitudemanager,!LO009008,logitudetest.database.windows.net";//"LogitudeMain-Test2,sa,Saas256,logitudetest.cloudapp.net";
        //string dbDestinationConnection = "Logitude2-5_Main,sa,Saas256,.";//"Logitude2-5_Global,sa,Saas256,.";
        private CargoTrackingService shipmentHeaderService;
        private int NumberOfCoulmnUpdated = 0;
        private int Table_X = 0;
        private int Table_Y = 1;
        private int TableCellMrginHight = 10;
        private int TotalIncreasing = 0;
        private bool FirstInit = true;
        private ManualResetEvent syncEvent;
        //bool FirstChecking = true;
        public CargoTrackingForm()
        {
            InitializeComponent();
            shipmentHeaderService = new CargoTrackingService();
            this.SourceConnectionlTextBox.Text = dbSourceConnection;
            syncEvent = new ManualResetEvent(false);
            //    this.BuildConnectionStrings();
        }
        private List<CargoTable> FillCargoTableList()
        {
            List<CargoTable> CargoTableLists = new List<CargoTable>();
            CargoTableLists.Add(new CargoTable() { TableName = "Port", FieldsDBName = "Id,Code,EnglishName,CountryId,AutomaticLastUpdateDate", KeyName = "Id", DBTableName = "Ports", Dw_TableName = "CargoTrackingPorts" });
            CargoTableLists.Add(new CargoTable() { TableName = "Card", FieldsDBName = "Id,Code,EnglishName,LocalName,AutomaticLastUpdateDate", KeyName = "Id", DBTableName = "Cards", Dw_TableName = "CargoTrackingCards" });
            CargoTableLists.Add(new CargoTable() { TableName = "Shipment", FieldsDBName = "Id,Tenant,CustomerId,TransportModeId,MasterShipmentDataId,House,FromPortId,ToPortId,ShipmentNumber,ShipperId,ConsigneeId,GrossWeight,Volume,CustomConnectToShipment,FirstPickupETA,AutomaticLastUpdateDate", KeyName = "Id", DBTableName = "Shipments", Dw_TableName = "CargoTrackingShipments" });
        
            return CargoTableLists;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (FirstInit)
            {
                this.Height += 40;
                FirstInit = false;
            }
            

            //Thread thread = new Thread(delegate () { BuildData(); });
            //thread.IsBackground = true;
            //thread.Start();
            BuildData();

        }
    
        private void BuildData() {
            BuildConnectionStrings();
            this.SetFormHight();

            //Thread thread = new Thread(() => { BuildModule("CheckAndUpdateWaterMark", Checking, "Checking WaterMark ..."); syncEvent.Set();});
            //thread.IsBackground = true;
            //thread.Start();


            UpdateCargoTables();
        }


        private void SetFormHight()
        {
 
            this.Height = this.Height - this.TotalIncreasing;
            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.Size = new Size(tableLayoutPanel1.Size.Width, tableLayoutPanel1.Size.Height - this.TotalIncreasing);
            this.TotalIncreasing = 0;
            this.Table_Y = 0;
            this.Table_X = 0;
            tableLayoutPanel1.RowCount = 0;
            AddLabelToGrid("Table Name", 1, 0);
            AddLabelToGrid("Row Updated #", 1, 0);
            AddLabelToGrid("Statues", 1, 0);
            this.Table_X = 0;
            this.Table_Y = 1;


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

            //dbSourceConnection  string[] destinationConnectionArray = dbSourceConnection.Split(',');
            this.dbSourceConnection = this.SourceConnectionlTextBox.Text;

            //if (!dbSourceConnection.Contains("Data Source"))
            //{
                string[] sourceConnectionArray = dbSourceConnection.Split(',');
                if (sourceConnectionArray.Length != 4 || sourceConnectionArray.Length != 4)
                {
                    MessageBox.Show("connection not valid");
                    return;
                }

                dbSourceConnection = shipmentHeaderService.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
            //}
            //   dbSourceConnection = shipmentHeaderService.BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);

        }
        private void UpdateCargoDataBase(CargoTable table)
        {
            NumberOfCoulmnUpdated=shipmentHeaderService.UpdateDWDataBase(new CargoArgs() { Table = table, SourceConnectionString = dbSourceConnection, DestinationConnectionString = dbSourceConnection });
        }



        private void AddLabelToTable( TableLayoutPanel tableLayoutPanel, string Dw_TableName, int x, int y,int AccessLevel=0, CargoTable table=null)
        {

            // AccessLevel = 0 For Main Coulmn ("Table Name" , "Row Updated #" ,"Statues" ) 
            // AccessLevel = 1 For Record in Coulmn ("Table Name") 
            // AccessLevel = 2 For Record in Coulmn ("Row Updated #" ) 
            // AccessLevel = 3 For Record in Coulmn ("Statues" ) 


            Label Label = new Label
            {
                Name = "Label" + Dw_TableName,
                AutoSize = true,
                Text = Dw_TableName,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                TextAlign = ContentAlignment.MiddleCenter,
            };

            if (table!=null)
            {
                table.Labels.Add(Label);
            }
/// هتا
             SetLabelStyleOnGRID(Label, AccessLevel);
             tableLayoutPanel.Controls.Add(Label, x, y);
   
                 
        }

        private void SetLabelStyleOnGRID(Label Label , int AccessLevel)
        {
            switch (AccessLevel)
            {
                case 0:
                    {
                        SetControlPropertyValue(Label, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
                        SetControlPropertyValue(Label, "ForeColor", Color.Black);
                        break;
                    }
                case 1:
                    {
                        SetControlPropertyValue(Label, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
                        SetControlPropertyValue(Label, "ForeColor", Color.Blue);
                        break;
                    }
                case 2:
                    {
                        SetControlPropertyValue(Label, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
                        SetControlPropertyValue(Label, "ForeColor", Color.Red);

                        break;
                    }
                case 3:
                    {
                        SetControlPropertyValue(Label, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
                        SetControlPropertyValue(Label, "ForeColor", Color.Gray);

                        break;
                    }
            }
        }




        private void AddLabelToGrid(string Dw_TableName,int X ,int Y,int AccessLevel=0, CargoTable table=null)
        {
            this.Height = this.Height + TableCellMrginHight;
            this.TotalIncreasing += TableCellMrginHight;
            //tableLayoutPanel1.RowCount++;
            tableLayoutPanel1.Size = new Size(tableLayoutPanel1.Size.Width, tableLayoutPanel1.Size.Height + TableCellMrginHight);
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 0.5f));
            AddLabelToTable( tableLayoutPanel1,  Dw_TableName, Table_X, Table_Y, AccessLevel, table);
            this.Table_X += X;
            this.Table_Y += Y;
        }




        private void UpdateCargoTables()
        {

            List<CargoTable> CargoTableLists = FillCargoTableList();
            foreach (CargoTable table in CargoTableLists)
            {
                table.Labels = new List<Label>();
                AddLabelToGrid(table.Dw_TableName, 1, 0, 1, table);
                AddLabelToGrid( "In Progress...", 1, 0, 2, table);
                AddLabelToGrid( "Remaining ...", 0, 1, 3, table);
                this.Table_X = 0;
            }
            InitFirstChecking();
            Thread thread = new Thread(() => {  AddAllTablesToThread(CargoTableLists); });
            thread.IsBackground = true;
            thread.Start();
         
        }

        private void AddAllTablesToThread(List<CargoTable> CargoTableLists)
        {
            foreach (CargoTable table in CargoTableLists)
            {

                SetLabelValueAndUpdateTable(table);

            }
        }


        private void SetLabelValueAndUpdateTable(CargoTable table)
        {

            SetControlPropertyValue(table.Labels[2], "Text", "Updating...");
            SetControlPropertyValue(table.Labels[2], "ForeColor", Color.Black);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            globalStopwatch = stopWatch;
            generalLabel = table.Labels[2];
            timer1.Enabled = true;
            timer1.Start();
            UpdateCargoDataBase(table);

            //SetControlPropertyValue(table.Labels[1], "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
            //SetControlPropertyValue(table.Labels[1], "Text", "Records Updated: ");
            //SetControlPropertyValue(table.Labels[1], "ForeColor", Color.Red);
            SetControlPropertyValue(table.Labels[1], "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
            SetControlPropertyValue(table.Labels[1], "Text", "( " + NumberOfCoulmnUpdated + " )");
            SetControlPropertyValue(table.Labels[1], "ForeColor", Color.Red);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            globalStopwatch = null;
            generalLabel = null;
            timer1.Start();
            SetControlPropertyValue(table.Labels[2], "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
            SetControlPropertyValue(table.Labels[2], "ForeColor", Color.Green); // timer
            SetControlPropertyValue(table.Labels[2], "Text", "Done in " + ts.ToString(@"hh\:mm\:ss"));
        }

        private void InitFirstChecking()
        {
            //if ( FirstChecking == true)
            //{
                BuildModule("CheckAndUpdateWaterMark", Checking, "Checking WaterMark ...");
            //    FirstChecking = false;
            //}
        }

        Stopwatch globalStopwatch;
        Label generalLabel;
        private void BuildModule(string name, Label lable,string TextStarting= "Updating...", string TextEnding= "Done in ")
        {
            SetControlPropertyValue(lable, "Text", TextStarting);
            SetControlPropertyValue(lable, "ForeColor", Color.Black); // timer


            if (name == "CheckAndUpdateWaterMark")
            {
                SetControlPropertyValue(CheckLabel, "Text", "Check WaterMarks: ");
                SetControlPropertyValue(CheckLabel, "ForeColor", Color.BlueViolet);
                SetControlPropertyValue(CheckLabel, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));

            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // for timer
            //if (generalLabel != null) SetControlPropertyValue(generalLabel, "Text", TextStarting);
                                 


            globalStopwatch = stopWatch;
            generalLabel = lable;
            timer1.Enabled = true;
            timer1.Start();

       

            if (name == "CheckAndUpdateWaterMark")
                CheckAndUpdateWaterMark();
            if (name == "UpdateCargoTables")
            {

                UpdateCargoTables();


                //SetControlPropertyValue(RecordsUpdated, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
                //SetControlPropertyValue(RecordsUpdated, "Text", "Records Updated: ");
                //SetControlPropertyValue(RecordsUpdated, "ForeColor", Color.Red);
                //SetControlPropertyValue(RecordsNumbers, "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
                //SetControlPropertyValue(RecordsNumbers, "Text", "( "+NumberOfCoulmnUpdated+" )");
                //SetControlPropertyValue(RecordsNumbers, "ForeColor", Color.Black);
            }
               

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            globalStopwatch = null;
            generalLabel = null;
            timer1.Start();
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
 
        private void timer1_Tick_2(object sender, EventArgs e)
        {

            if (globalStopwatch != null && generalLabel != null)
            {
                TimeSpan elapsedTime = globalStopwatch.Elapsed;

                SetControlPropertyValue(generalLabel, "Text", "Updating " + elapsedTime.ToString(@"hh\:mm\:ss"));
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
         
        }

        private void Statues_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            SourceConnectionlTextBox.Enabled = false;
            this.SourceConnectionlTextBox.Text = "Logitude2-5_Main,sa,Saas256,.";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            SourceConnectionlTextBox.Enabled = false;
            this.SourceConnectionlTextBox.Text = "LogitudeMain-Test2,sa,Saas256,logitudetest.cloudapp.net";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            SourceConnectionlTextBox.Enabled = true;

        }
    }
}
