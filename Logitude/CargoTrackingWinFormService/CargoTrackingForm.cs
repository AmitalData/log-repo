using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
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

namespace CargoTrackingWinFormService.Forms
{
    public partial class CargoTrackingForm : Form
    {
        private string LocalConectionstring = "Logitude2-5_Main,sa,Saas256,.";
        private string TestConectionstring =  "LogitudeMain-Test2,sa,Saas256,logitudetestdb.westeurope.cloudapp.azure.com";
        private string CloudConectionstring = "Main,sa,Saas256,amitaldata.cloudapp.net";
        private string CargoTrackingConectionstring = "CargoTracking,sa,Saas256,.";
        private string dbSourceConnection  ; 
        private string dbDestinationConnection;
        private CargoTrackingMainService cargoTrackingService;
        private int NumberOfCoulmnUpdated = 0;
        private int Table_X = 0;
        private int Table_Y = 1;
        private int TableCellMrginHight = 10;
        private int TotalIncreasing = 0;
        private bool FirstInit = true;
        private int NumberOfBulkPerTime = 1000;
        public CargoTrackingForm()
        {
            InitializeComponent();
            dbSourceConnection = LocalConectionstring;
            dbDestinationConnection = CargoTrackingConectionstring;
            cargoTrackingService = new CargoTrackingMainService();
            this.SourceConnectionlTextBox.Text = dbSourceConnection;
            this.DestinationConnectionlTextBox.Text = dbDestinationConnection;
         }
  

        private void button1_Click(object sender, EventArgs e)
        {
            if (FirstInit)
            {
                this.Height += 40;
                FirstInit = false;
            }
            
 
            BuildData();

        }
    
        private void BuildData() {
            BuildConnectionStrings();
            this.SetFormHight();
 
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
            AddLabelToGrid("# Of Updated Records", 1, 0);
            AddLabelToGrid("Statues", 1, 0);
            this.Table_X = 0;
            this.Table_Y = 1;


        }

      
        private void BuildConnectionStrings()
        {

            this.dbSourceConnection = this.SourceConnectionlTextBox.Text;
            this.dbDestinationConnection = this.DestinationConnectionlTextBox.Text;
 
            string[] sourceConnectionArray = dbSourceConnection.Split(',');
                string[] destinationConnectionArray = dbDestinationConnection.Split(',');

               if (sourceConnectionArray.Length != 4 || sourceConnectionArray.Length != 4)
                {
                    MessageBox.Show("Source connection not valid");
                    return;
                }

            if (destinationConnectionArray.Length != 4 || destinationConnectionArray.Length != 4)
            {
                MessageBox.Show("Destination connection not valid");
                return;
            }

            dbSourceConnection = cargoTrackingService.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
            dbDestinationConnection = cargoTrackingService.BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);
 
        }
        private void UpdateCargoDataBase(CargoTable table)
        {
            CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs = new CargoTrackingUpdateDataBaseArgs() {
                buildCargoArgs = new CargoArgs() { Table = table, SourceConnectionString = dbSourceConnection, DestinationConnectionString = dbDestinationConnection },
                NumberOfBulkPerTime = NumberOfBulkPerTime,
                IsUpdateFromBuild = false,
                CargoTrackingArguments = null,
                IsUpdateAfterFinished = null,
            };
          NumberOfCoulmnUpdated = cargoTrackingService.UpdateCargoTrackingDataBase(cargoTrackingDataBaseArgs).NumberOfRecordUpdated;
  
        }



        private void AddLabelToTable( TableLayoutPanel tableLayoutPanel, string Dw_TableName, int x, int y,int AccessLevel=0, CargoTable table=null)
        {
 

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
            tableLayoutPanel1.Size = new Size(tableLayoutPanel1.Size.Width, tableLayoutPanel1.Size.Height + TableCellMrginHight);
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 0.5f));
            AddLabelToTable( tableLayoutPanel1,  Dw_TableName, Table_X, Table_Y, AccessLevel, table);
            this.Table_X += X;
            this.Table_Y += Y;
        }




        private void UpdateCargoTables()
        {
            
            List<CargoTable> CargoTableLists = cargoTrackingService.FillCargoTableList();
            foreach (CargoTable table in CargoTableLists)
            {
                table.Labels = new List<object>();
                string TableNameLabe = table.CT_TableName.Length <23 ? table.CT_TableName : table.CT_TableName.Substring(0,17)+" ...";
                AddLabelToGrid(TableNameLabe, 1, 0, 1, table);
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

            SetControlPropertyValue((Label)table.Labels[2], "Text", "Updating...");
            SetControlPropertyValue((Label)table.Labels[2], "ForeColor", Color.Black);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            globalStopwatch = stopWatch;
            generalLabel = (Label)table.Labels[2];
            timer1.Enabled = true;
            timer1.Start();
            UpdateCargoDataBase(table);
 
            SetControlPropertyValue((Label)table.Labels[1], "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
            SetControlPropertyValue((Label)table.Labels[1], "Text", "( " + NumberOfCoulmnUpdated + " )");
            SetControlPropertyValue((Label)table.Labels[1], "ForeColor", Color.Red);

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            globalStopwatch = null;
            generalLabel = null;
            timer1.Start();
            SetControlPropertyValue((Label)table.Labels[2], "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
            SetControlPropertyValue((Label)table.Labels[2], "ForeColor", Color.Green); // timer
            SetControlPropertyValue((Label)table.Labels[2], "Text", "Done in " + ts.ToString(@"hh\:mm\:ss"));
        }

        private void InitFirstChecking()
        {
             
                BuildModule("CheckAndUpdateWaterMark", Checking, "Checking WaterMark ...");
          
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
 

            globalStopwatch = stopWatch;
            generalLabel = lable;
            timer1.Enabled = true;
            timer1.Start();

       

            if (name == "CheckAndUpdateWaterMark")
                cargoTrackingService.CheckAndUpdateWaterMark(dbDestinationConnection,dbSourceConnection);
            if (name == "UpdateCargoTables")
            {

                UpdateCargoTables();
 
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
            this.SourceConnectionlTextBox.Text = LocalConectionstring;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            SourceConnectionlTextBox.Enabled = false;
            this.SourceConnectionlTextBox.Text = TestConectionstring;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            SourceConnectionlTextBox.Enabled = true;

        }

        private void DestinationConnectionlTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (textbox != null)
            {
                this.dbDestinationConnection = textbox.Text;
            }

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            DestinationConnectionlTextBox.Enabled = false;
            this.DestinationConnectionlTextBox.Text = LocalConectionstring;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            DestinationConnectionlTextBox.Enabled = false;
            this.DestinationConnectionlTextBox.Text = TestConectionstring;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            DestinationConnectionlTextBox.Enabled = true;
        }

        private void CargoTrackingForm_Load(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            this.NumberOfBulkPerTime = 1000;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            this.NumberOfBulkPerTime = 5000;
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            this.NumberOfBulkPerTime = 10000;
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            SourceConnectionlTextBox.Enabled = false;
            this.SourceConnectionlTextBox.Text = CloudConectionstring;
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            DestinationConnectionlTextBox.Enabled = false;
            this.DestinationConnectionlTextBox.Text = CloudConectionstring;
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            DestinationConnectionlTextBox.Enabled = false;
            this.DestinationConnectionlTextBox.Text = CargoTrackingConectionstring;
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            SourceConnectionlTextBox.Enabled = false;
            this.SourceConnectionlTextBox.Text = CargoTrackingConectionstring;
        }
    }
}
