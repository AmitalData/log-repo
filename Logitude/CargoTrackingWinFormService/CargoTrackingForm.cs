using CargoTrackingWinService;
using CargoTrackingWinService.Helper;
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.CargoTrackingSetLogic;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.CargoTracking.BL.CoreBL.Batch;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CargoTrackingWinFormService.Forms
{
    public partial class CargoTrackingForm : Form
    {
 

        private string FromLocalConectionstring = "2022R1_Main,sa,Saas256,.";
        
        private string FromTestConectionstring = "Main-Test,sa,Saas256,testaccsqldb.israelcentral.cloudapp.azure.com";
        private string FromCloudConectionstring = "Main,CloudApp,London2020!,amitaldata.cloudapp.net";
        private string ToLocalConectionstring = "CargoTracking,sa,Saas256,.";
        private string ToTestConectionstring = "CargoTracking,sa,Saas256,testaccsqldb.israelcentral.cloudapp.azure.com";
        private string ToCloudConectionstring = "CargoTracking,amitaladmin,London2015!London2015!,amital.database.windows.net";
        private List<string> ErrorsValidatons = new List<string>();
        string SelectedTable = null;
        List<CargoTrackingTable> CargoTableLists;
        private int[] ScreensHight;
        private int[] ScreensWidth;
        private int[] ScreensTotalIncreasing;
        private bool[] ScreensFirstInits;
        private Point[] ScreensLocation=null;
        private FormStartPosition[] ScreensStartPosition;
        private string dbSourceConnection  ; 
        private string dbDestinationConnection;
        private int? IncrementalNumbersSelecting = 100;
        private int IncrementalsResultCount= 0;
        private int IncrementalsErrosResultCount = 0;
        private CargoTrackingMainService cargoTrackingService;
        private int NumberOfCoulmnUpdated = 0;
        private int NumberOfCoulmnUpdated2 = 0;
        private int Table_X = 0;
        private int Table_Y = 1;
        private int TableCellMrginHight = 10;
        private int TotalIncreasing = 0;
        private bool FirstInit = true;
        private int NumberOfBulkPerTime = 1000;
        private int TabsNumber = 6;
        private int SleepTime = 0;
        private bool ButtonWindowsServiceIsForStop=false;
        public int TotalRecordedUpdated = 0;
        public int TotalTimeUpdated = 0;
        public CargoTrackingForm()
        {
            InitializeComponent();
            label20.Hide();
            numericUpDown1.Hide();
            tabControl1.Selecting += new TabControlCancelEventHandler(tabControl1_Selecting);
            ScreensHight = Enumerable.Repeat(this.Height, TabsNumber).ToArray();
            ScreensWidth = Enumerable.Repeat(this.Width, TabsNumber).ToArray();
            ScreensTotalIncreasing = Enumerable.Repeat(this.TotalIncreasing, TabsNumber).ToArray();
            ScreensFirstInits = Enumerable.Repeat(this.FirstInit, TabsNumber).ToArray();
            ScreensStartPosition = Enumerable.Repeat(this.StartPosition, TabsNumber).ToArray();
            dbSourceConnection = FromLocalConectionstring;
            dbDestinationConnection = ToLocalConectionstring;
            cargoTrackingService = new CargoTrackingMainService();
            this.SourceConnectionlTextBox.Text = dbSourceConnection;
            this.DestinationConnectionlTextBox.Text = dbDestinationConnection;
            this.textBox1.Text = dbSourceConnection;
            this.textBox2.Text = dbDestinationConnection;
            this.IncrementalConnectionStrings.Text = dbDestinationConnection;
            this.IncrementalErrorscConnections.Text = dbDestinationConnection;
            this.WinServiceFromConnections.Text = dbSourceConnection;
            this.WinServiceToConnections.Text = dbDestinationConnection;
            this.SleepSecounds.Value = SleepTime;
            this.MappingFromConnections.Text = dbDestinationConnection;
            this.numericUpDown1.Value =  3;
            this.checkBox2.Checked = true;
            //this.textBox3.Text = "Shipments";
            this.BuildFrom.Value = BuildTo.Value.AddMonths(-6);
            this.checkBox1.Checked = false;
            this.checkBox1.Enabled = false;
            MappingFromConnections.Enabled = false;
            this.numericUpDown2.Value = 1;
            this.numericUpDown2.Enabled = false;
            this.checkBox3.Checked = true;
            this.checkBox3.Enabled = true;
            CargoTableLists = CargoTrackingTableList.GetCargoTrackingTableList();
            string [] DBTabkeNames = CargoTableLists.Select(s=>s.DBTableName).ToArray();
            this.comboBox2.Items.AddRange(DBTabkeNames);
            this.comboBox2.SelectedIndex = DBTabkeNames.Count() - 1;
        }


        private void button1_Click(object sender, EventArgs e)
        {
             
           // this.EnableDisabledAllData(false);
            if (FirstInit)
            {
                this.Height += 40;
                FirstInit = false;
            }
            
 
            BuildData();

        }
    
        private void BuildData(bool IsFromBuild=false) {
            if (ScreensLocation == null)
            {
                ScreensLocation = Enumerable.Repeat(this.Location, TabsNumber).ToArray();
            }
            this.Location = new Point(this.Location.X, 20);

            BuildConnectionStrings(IsFromBuild);
            this.SetFormHight(IsFromBuild);
 
            UpdateCargoTables(IsFromBuild);
            this.ScreensHight[tabControl1.SelectedIndex]= this.Height;
            this.ScreensTotalIncreasing[tabControl1.SelectedIndex] = this.TotalIncreasing;
            this.ScreensFirstInits[tabControl1.SelectedIndex] = this.FirstInit;
            this.ScreensStartPosition[tabControl1.SelectedIndex] = this.StartPosition;
            this.ScreensLocation[tabControl1.SelectedIndex] = this.Location;
        }


        private void SetFormHight(bool IsFromBuild)
        {
 
            this.Height = this.Height - this.TotalIncreasing;
            tabControl1.Height = this.Height - this.TotalIncreasing;
            if (IsFromBuild) {
                tableLayoutPanel2.Controls.Clear();
                tableLayoutPanel2.RowStyles.Clear();
                tableLayoutPanel2.Size = new Size(tableLayoutPanel2.Size.Width, tableLayoutPanel2.Size.Height - this.TotalIncreasing);
                tableLayoutPanel2.RowCount = 0;
            }
            else
            {
                tableLayoutPanel1.Controls.Clear();
                tableLayoutPanel1.RowStyles.Clear();
                tableLayoutPanel1.Size = new Size(tableLayoutPanel1.Size.Width, tableLayoutPanel1.Size.Height - this.TotalIncreasing);
                tableLayoutPanel1.RowCount = 0;
            }
            
            this.TotalIncreasing = 0;
            this.Table_Y = 0;
            this.Table_X = 0;
            
            AddLabelToGrid("Table Name", 1, 0 , 0 ,null,IsFromBuild);
            AddLabelToGrid("# Of Updated Records", 1, 0, 0, null, IsFromBuild);
            AddLabelToGrid("Statues", 1, 0, 0, null, IsFromBuild);
            this.Table_X = 0;
            this.Table_Y = 1;


        }

      
        private void BuildConnectionStrings(bool IsFromBuild)
        {


            this.dbSourceConnection = this.SourceConnectionlTextBox.Text;
            this.dbDestinationConnection = this.DestinationConnectionlTextBox.Text;

            if (IsFromBuild)
            {
                this.dbSourceConnection = this.textBox1.Text;
                this.dbDestinationConnection = this.textBox2.Text;
            }
 
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

            ConnectionStringArguments sourceConnectionStringArguments = GetConnectionStringArguments(sourceConnectionArray);
            ConnectionStringArguments destinationConnectionStringArguments = GetConnectionStringArguments(destinationConnectionArray);
 

            dbSourceConnection = ServiceHelper.BuildConnectionString(sourceConnectionStringArguments);
            dbDestinationConnection = ServiceHelper.BuildConnectionString(destinationConnectionStringArguments);
 
        }

        private ConnectionStringArguments GetConnectionStringArguments(string[] connectionArray)
        {
            ConnectionStringArguments connectionStringArguments = new ConnectionStringArguments()
            {
                Catalog = connectionArray[0],
                UserName = connectionArray[1],
                Password = connectionArray[2],
                Server = connectionArray[3],
            };

            return connectionStringArguments;
        }



        private void UpdateCargoDataBase(CargoTrackingTable table , bool IsFromBuild )
        {
            CargoTrackingArguments CargoTrackingArguments = null;
            if (IsFromBuild)
            {
                int? SelectedTenant = null;
                if (!checkBox3.Checked)
                {
                    SelectedTenant = (int)numericUpDown2.Value;
                }
                CargoTrackingArguments = new CargoTrackingArguments()
                {

                    FromDate = checkBox1.Checked ? new DateTime(1900, 1, 1) : BuildFrom.Value,
                    ToDate = checkBox1.Checked ? new DateTime(2500, 1, 1) : BuildTo.Value,
                    Tenant = SelectedTenant,
                    ThreadNumber = (int)numericUpDown1.Value,
                    FormTableName = checkBox2.Checked ? null: SelectedTable,
                };
            }


            CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs = new CargoTrackingUpdateDataBaseArgs() {
                BuildCargoArgs = new Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses.CargoTrackingArgs() { Table = table, SourceConnectionString = dbSourceConnection, DestinationConnectionString = dbDestinationConnection },
                NumberOfBulkPerTime = NumberOfBulkPerTime,
                IsUpdateFromBuild = IsFromBuild,
                CargoTrackingArguments = CargoTrackingArguments,
                IsUpdateAfterFinished = null,
            };
            RecordUpdated Numbers = cargoTrackingService.UpdateCargoTrackingDataBase(cargoTrackingDataBaseArgs);
            NumberOfCoulmnUpdated = Numbers.NumberOfRecordUpdated;
            NumberOfCoulmnUpdated2 = Numbers.NumberOfRecordUpdated2;
            TotalRecordedUpdated += (TotalRecordedUpdated+ NumberOfCoulmnUpdated2);
         }



        private void AddLabelToTable( TableLayoutPanel tableLayoutPanel, string Dw_TableName, int x, int y,int AccessLevel=0, CargoTrackingTable table=null)
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




        private void AddLabelToGrid(string Dw_TableName,int X ,int Y,int AccessLevel=0, CargoTrackingTable table=null,bool IsFromBuild =false)
        {
            this.Height = this.Height + TableCellMrginHight;
            tabControl1.Height = this.Height + TableCellMrginHight;
            this.TotalIncreasing += TableCellMrginHight;

            if (IsFromBuild)
            {
                tableLayoutPanel2.Size = new Size(tableLayoutPanel2.Size.Width, tableLayoutPanel2.Size.Height + TableCellMrginHight);
                tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 0.5f));
                AddLabelToTable(tableLayoutPanel2, Dw_TableName, Table_X, Table_Y, AccessLevel, table);
            }
            else
            {
                tableLayoutPanel1.Size = new Size(tableLayoutPanel1.Size.Width, tableLayoutPanel1.Size.Height + TableCellMrginHight);
                tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 0.5f));
                AddLabelToTable(tableLayoutPanel1, Dw_TableName, Table_X, Table_Y, AccessLevel, table);

            }
            
            this.Table_X += X;
            this.Table_Y += Y;
        }




        private void UpdateCargoTables(bool IsFromBuild)
        {
            CargoTableLists = CargoTrackingTableList.GetCargoTrackingTableList();

            if (!checkBox2.Checked)
            {
                SelectedTable = (string)comboBox2.SelectedItem;
                CargoTableLists = CargoTableLists.Where(s => s.DBTableName == SelectedTable).ToList();
            }

            if (CargoTableLists==null || CargoTableLists.Count ==0)
            {
                MessageBox.Show("Please add a valid table name or selecet all tables check box.");

            }

            foreach (CargoTrackingTable table in CargoTableLists)
            {
                table.Labels = new List<object>();
                string TableNameLabe = table.DBTableName.Length <23 ? table.DBTableName : table.DBTableName.Substring(0,17)+" ...";
                AddLabelToGrid(TableNameLabe, 1, 0, 1, table, IsFromBuild);
                AddLabelToGrid( "In Progress...", 1, 0, 2, table, IsFromBuild);
                AddLabelToGrid( "Remaining ...", 0, 1, 3, table, IsFromBuild);
                this.Table_X = 0;
            }
            InitFirstChecking();
            Thread thread = new Thread(() => {  AddAllTablesToThread(CargoTableLists, IsFromBuild); });
            thread.IsBackground = true;
            thread.Start();
          
           // this.EnableDisabledAllData(true);

        }

        private void AddAllTablesToThread(List<CargoTrackingTable> CargoTableLists , bool IsFromBuild)
        {
            foreach (CargoTrackingTable table in CargoTableLists)
            {

                try
                {
                    SetLabelValueAndUpdateTable(table, IsFromBuild);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message + Environment.NewLine + e.StackTrace);
                    //Application.Exit();
                    break;
                }
                

            }

        }


        private void EnableDisabledAllData(bool IsEnabled)
        {
            this.radioButton12.Enabled = IsEnabled;
            this.radioButton13.Enabled = IsEnabled;
            this.radioButton14.Enabled = IsEnabled;
            this.radioButton15.Enabled = IsEnabled;
            this.radioButton16.Enabled = IsEnabled;
            this.radioButton17.Enabled = IsEnabled;
            this.radioButton18.Enabled = IsEnabled;
            this.radioButton19.Enabled = IsEnabled;

            this.radioButton11.Enabled = IsEnabled;
            this.radioButton10.Enabled = IsEnabled;
            this.radioButton9.Enabled = IsEnabled;
            this.radioButton8.Enabled = IsEnabled;
            this.radioButton7.Enabled = IsEnabled;
            this.radioButton6.Enabled = IsEnabled;
            this.radioButton5.Enabled = IsEnabled;
            this.radioButton4.Enabled = IsEnabled;
            this.radioButton3.Enabled = IsEnabled;
            this.radioButton2.Enabled = IsEnabled;
            this.radioButton1.Enabled = IsEnabled;

            this.button2.Enabled = IsEnabled;
       
            this.button1.Enabled = IsEnabled;

            this.numericUpDown1.Enabled = IsEnabled;
            this.checkBox1.Enabled = IsEnabled;
            this.checkBox2.Enabled = IsEnabled;
            this.checkBox3.Enabled = IsEnabled;

            if (IsEnabled==true)
            {
                if (checkBox3.Checked)
                {
                    this.numericUpDown2.Enabled = !IsEnabled;
                }
                else
                {
                    this.numericUpDown2.Enabled = IsEnabled;
                }

                if (checkBox2.Checked)
                {
                    //this.textBox3.Enabled = !IsEnabled;
                    //comboBox2.Enabled = !IsEnabled;

                }
                else
                {
                    //this.textBox3.Enabled = IsEnabled;
                    //comboBox2.Enabled =  IsEnabled;

                }

                if (checkBox1.Checked)
                {
                    this.BuildFrom.Enabled = !IsEnabled;
                    //this.BuildTo.Enabled = !IsEnabled;
                }
                else
                {
                    this.BuildFrom.Enabled = IsEnabled;
                    //this.BuildTo.Enabled = IsEnabled;

                }
            }
            else
            {
                this.numericUpDown2.Enabled = IsEnabled;
               // this.textBox3.Enabled = IsEnabled;
               // comboBox2.Enabled =  IsEnabled;

                this.BuildFrom.Enabled = IsEnabled;
               // this.BuildTo.Enabled = IsEnabled;
            }
        }

        private void SetLabelValueAndUpdateTable(CargoTrackingTable table, bool IsFromBuild)
        {

            SetControlPropertyValue((Label)table.Labels[2], "Text", "Updating...");
            SetControlPropertyValue((Label)table.Labels[2], "ForeColor", Color.Black);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            globalStopwatch = stopWatch;
            generalLabel = (Label)table.Labels[2];
            timer1.Enabled = true;
            timer1.Start();
            UpdateCargoDataBase(table, IsFromBuild);
 
            SetControlPropertyValue((Label)table.Labels[1], "Font", new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold));
            if (NumberOfCoulmnUpdated2 == 0)
                SetControlPropertyValue((Label)table.Labels[1], "Text", "( " + String.Format("{0:n0}", NumberOfCoulmnUpdated) + " )");
            else
                SetControlPropertyValue((Label)table.Labels[1], "Text", "( " + String.Format("{0:n0}", NumberOfCoulmnUpdated) + " ) " + "( "+ String.Format("{0:n0}", NumberOfCoulmnUpdated2) + " ) ");
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
                ServiceHelper.CheckAndUpdateWaterMark(dbDestinationConnection,dbSourceConnection);
            if (name == "UpdateCargoTables")
            {

                UpdateCargoTables(false);
 
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
            this.SourceConnectionlTextBox.Text = FromLocalConectionstring;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            SourceConnectionlTextBox.Enabled = false;
            this.SourceConnectionlTextBox.Text = FromTestConectionstring;
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
            this.DestinationConnectionlTextBox.Text = ToLocalConectionstring;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            DestinationConnectionlTextBox.Enabled = false;
            this.DestinationConnectionlTextBox.Text = ToTestConectionstring;
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
            this.SourceConnectionlTextBox.Text = FromCloudConectionstring;
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            DestinationConnectionlTextBox.Enabled = false;
            this.DestinationConnectionlTextBox.Text = ToCloudConectionstring;
        }

        private void radioButton13_CheckedChanged(object sender, EventArgs e)
        {
            DestinationConnectionlTextBox.Enabled = false;
            this.DestinationConnectionlTextBox.Text = ToLocalConectionstring;
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            SourceConnectionlTextBox.Enabled = false;
            this.SourceConnectionlTextBox.Text = ToLocalConectionstring;
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (textbox != null)
            {
                this.dbSourceConnection = textbox.Text;
            }

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (textbox != null)
            {
                this.dbDestinationConnection = textbox.Text;
            }

        }

        private void radioButton17_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            this.textBox1.Text = FromLocalConectionstring;
        }

        private void radioButton19_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            this.textBox2.Text = ToLocalConectionstring;
        }

        private void radioButton16_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            this.textBox1.Text = FromTestConectionstring;
        }

        private void radioButton18_CheckedChanged(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            this.textBox2.Text = ToTestConectionstring;
        }

        private void radioButton14_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            this.textBox1.Text = FromCloudConectionstring;
        }

        private void radioButton12_CheckedChanged_1(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            this.textBox2.Text = ToCloudConectionstring;
        }

        private void radioButton15_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
        }

        private void radioButton13_CheckedChanged_1(object sender, EventArgs e)
        {
            textBox2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ErrorsValidatons = new List<string>();

            CheckBuildDateIsValid();
            if (ErrorsValidatons.Count == 0)
            {
                cargoTrackingService = new CargoTrackingMainService();
                if (FirstInit)
                {
                    this.Height += 40;
                    FirstInit = false;
                }

                BuildData(true);
                
            }
            else
            {
                MessageBox.Show(string.Join(",", ErrorsValidatons));
            }
         
        }

        private void CheckBuildDateIsValid()
        {
            if (!checkBox1.Checked && (BuildTo.Value == null || BuildFrom.Value == null))
            {
                this.ErrorsValidatons.Add("Add Valid Dates");

            }
            else
            {
                var dateSpan = BuildTo.Value - BuildFrom.Value;
                if (dateSpan.TotalDays < 0)
                {
                    this.ErrorsValidatons.Add("'From date' must be less than 'to date'");
                }
            }

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            this.Height = this.ScreensHight[tabControl1.SelectedIndex];
            this.Width = this.ScreensWidth[tabControl1.SelectedIndex];
            this.StartPosition = this.ScreensStartPosition[tabControl1.SelectedIndex];
            this.TotalIncreasing = this.ScreensTotalIncreasing[tabControl1.SelectedIndex];
            if (ScreensLocation!=null)
            {
                this.Location = this.ScreensLocation[tabControl1.SelectedIndex];
            }
            this.FirstInit = this.ScreensFirstInits[tabControl1.SelectedIndex];

            if (tabControl1.SelectedIndex == 2 || tabControl1.SelectedIndex == 4)
            {
                this.IncrementalEndDate.Value = DateTime.Today.AddDays(1);
                this.IncrementalStartDate.Value = IncrementalEndDate.Value.AddMonths(-1);
                if (tabControl1.SelectedIndex == 2)
                {
                    this.IncResults.Text = "(" + IncrementalsResultCount + ")";
                }
                else
                {
                    this.IncResults.Text = "(" + IncrementalsErrosResultCount + ")";
                }

                if (this.FirstInit)
                {
                    int IncrementalWidth = 560;
                    int IncrementalHieght = 400;
                    this.Width += IncrementalWidth;
                    this.Height += IncrementalHieght;
                    if (tabControl1.SelectedIndex == 2)
                    {

                        dataGridView1.Width += IncrementalWidth;
                        dataGridView1.Height += IncrementalHieght;
                    }
                    else
                    {

                        dataGridView2.Width += IncrementalWidth;
                        dataGridView2.Height += IncrementalHieght;
                    }
                   
                    tabControl1.Height += IncrementalHieght;
                    tabControl1.Width += IncrementalWidth;
                    if (ScreensLocation == null)
                    {
                        ScreensLocation = Enumerable.Repeat(this.Location, TabsNumber).ToArray();
                    }
                    this.Location = new Point(0 , 20);
                    this.StartPosition = FormStartPosition.Manual;
                    this.FirstInit = false;
                }

                this.ScreensFirstInits[tabControl1.SelectedIndex] = this.FirstInit;
                this.ScreensWidth[tabControl1.SelectedIndex] = this.Width;
                this.ScreensStartPosition[tabControl1.SelectedIndex] = this.StartPosition;
                this.ScreensLocation[tabControl1.SelectedIndex] = this.Location;
                this.ScreensHight[tabControl1.SelectedIndex] = this.Height;

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.LoadIncrementalsData("*");
        }


        private void radioButton25_CheckedChanged(object sender, EventArgs e)
        {
            IncrementalConnectionStrings.Enabled = false;
            this.IncrementalConnectionStrings.Text = ToLocalConectionstring;

        }

        private void radioButton24_CheckedChanged(object sender, EventArgs e)
        {
            IncrementalConnectionStrings.Enabled = false;
            this.IncrementalConnectionStrings.Text = ToTestConectionstring;
        }

        private void radioButton22_CheckedChanged(object sender, EventArgs e)
        {
            IncrementalConnectionStrings.Enabled = false;
            this.IncrementalConnectionStrings.Text = ToCloudConectionstring;
        }

        private void radioButton23_CheckedChanged(object sender, EventArgs e)
        {
            IncrementalConnectionStrings.Enabled = true;
        }

        private void LoadIncrementalsData(string Fields)
        {
            
           
            if (!string.IsNullOrEmpty(IncrementalConnectionStrings.Text))
            {
                string sqlQueryStr = null;

                if (this.IncrementalNumbersSelecting !=null)
                {
                    sqlQueryStr = "SELECT top " + this.IncrementalNumbersSelecting + " "+ Fields + "  From CargoTrackingIncrementalStats ";
                }
                else
                {
                    sqlQueryStr = "SELECT * From CargoTrackingIncrementalStats ";
                }
                if (this.IncrementalStartDate.Value!=null && this.IncrementalEndDate.Value!=null)
                {
                    sqlQueryStr += "Where StartDate >= '" + this.IncrementalStartDate.Value.Date + "' and EndDate <= '" + this.IncrementalEndDate.Value.Date + "'";
                }
                else if (this.IncrementalStartDate.Value != null)
                {
                    sqlQueryStr += "Where StartDate >= '" + this.IncrementalStartDate.Value.Date + "'";
                }
                else if (this.IncrementalEndDate.Value != null)
                {
                    sqlQueryStr += "Where EndDate <= '" + this.IncrementalEndDate.Value.Date + "'";
                }

                SqlConnection conn = new SqlConnection();
                try
                {
                    string[] ConnectionSplitted = null;
                    if (Fields=="*")
                    {
                        ConnectionSplitted = IncrementalConnectionStrings.Text.Split(',');
                    }
                    else
                    {
                        ConnectionSplitted = IncrementalErrorscConnections.Text.Split(',');
                    }
                    
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
                        
                        if (Fields=="*")
                        {
                            dataGridView1.DataSource = myTable;
                            for (int i=0;i < dataGridView1.Columns.Count; i++)
                            {
                                if (dataGridView1.Columns[i].HeaderText =="ErrorLog")
                                {
                                    dataGridView1.Columns[i].Width = 400;
                                    break;

                                }

                            }
                            IncrementalsResultCount = myTable.Rows.Count;
                            this.IncResults.Text = "(" + IncrementalsResultCount + ")";

                        }
                        else
                        {
                            dataGridView2.DataSource = myTable;
                            for (int i = 0; i < dataGridView2.Columns.Count; i++)
                            {
                                if (dataGridView2.Columns[i].HeaderText == "ErrorLog")
                                {
                                    dataGridView2.Columns[i].Width = 715;
                                    break;

                                }

                            }
                            IncrementalsErrosResultCount = myTable.Rows.Count;
                            this.IncResults.Text = "(" + IncrementalsErrosResultCount + ")";

                        }
                        


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
                MessageBox.Show("Add Valid Connection strings ...");

            }

        }


        private void LoadWaterMarksData()
        {


            if (!string.IsNullOrEmpty(MappingFromConnections.Text))
            {
                string sqlQueryStr = "SELECT * From CargoTrackingWatermarks ";



                SqlConnection conn = new SqlConnection();
                try
                {
                    string[] ConnectionSplitted = null;
                
                    ConnectionSplitted = MappingFromConnections.Text.Split(',');
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
                        dataGridView3.DataSource = myTable;
                        for (int i = 0; i < dataGridView3.Columns.Count; i++)
                        {
                            if (dataGridView3.Columns[i].HeaderText == "TableName")
                            {
                                dataGridView3.Columns[i].Width =200;
                                break;

                            }

                        }
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
                MessageBox.Show("Add Valid Connection strings ...");

            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void IncTop100_CheckedChanged(object sender, EventArgs e)
        {
            this.IncrementalNumbersSelecting = 100;

        }

        private void IncTop500_CheckedChanged(object sender, EventArgs e)
        {
            this.IncrementalNumbersSelecting = 500;
        }

        private void IncTop1000_CheckedChanged(object sender, EventArgs e)
        {
            this.IncrementalNumbersSelecting = 1000;
        }

        private void IncTop5000_CheckedChanged(object sender, EventArgs e)
        {
            this.IncrementalNumbersSelecting = 5000;
        }

        private void radioButton20_CheckedChanged(object sender, EventArgs e)
        {
            this.IncrementalNumbersSelecting = null;
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void radioButton32_CheckedChanged(object sender, EventArgs e)
        {
            IncrementalErrorscConnections.Enabled = false;
            this.IncrementalErrorscConnections.Text = ToLocalConectionstring;

        }

        private void radioButton31_CheckedChanged(object sender, EventArgs e)
        {
            IncrementalErrorscConnections.Enabled = false;
            this.IncrementalErrorscConnections.Text = ToTestConectionstring;
        }

        private void radioButton29_CheckedChanged(object sender, EventArgs e)
        {
            IncrementalErrorscConnections.Enabled = false;
            this.IncrementalErrorscConnections.Text = ToCloudConectionstring;

        }

        private void radioButton30_CheckedChanged(object sender, EventArgs e)
        {
            IncrementalErrorscConnections.Enabled = true;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.LoadIncrementalsData("Id,StartDate,EndDate,ErrorLog");
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton34_CheckedChanged(object sender, EventArgs e)
        {
            WinServiceFromConnections.Enabled = false;
            this.WinServiceFromConnections.Text = FromLocalConectionstring;
        }

        private void radioButton33_CheckedChanged(object sender, EventArgs e)
        {
            WinServiceFromConnections.Enabled = false;
            this.WinServiceFromConnections.Text = FromTestConectionstring;
        }

        private void radioButton27_CheckedChanged(object sender, EventArgs e)
        {
            WinServiceFromConnections.Enabled = false;
            this.WinServiceFromConnections.Text = FromCloudConectionstring;
        }

        private void radioButton28_CheckedChanged(object sender, EventArgs e)
        {
            WinServiceFromConnections.Enabled = true;
        }

        private void radioButton36_CheckedChanged(object sender, EventArgs e)
        {
            WinServiceToConnections.Enabled = false;
            this.WinServiceToConnections.Text = ToLocalConectionstring;
        }

        private void radioButton35_CheckedChanged(object sender, EventArgs e)
        {
            WinServiceToConnections.Enabled = false;
            this.WinServiceToConnections.Text = ToTestConectionstring;
        }

        private void radioButton21_CheckedChanged(object sender, EventArgs e)
        {
            WinServiceToConnections.Enabled = false;
            this.WinServiceToConnections.Text = ToCloudConectionstring;
        }

        private void radioButton26_CheckedChanged(object sender, EventArgs e)
        {
            WinServiceToConnections.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!ButtonWindowsServiceIsForStop)
            {
                this.button4.Text = "Stop Windows Service";
                button4.BackColor = System.Drawing.Color.LightGray;
                button4.FlatStyle = FlatStyle.Flat;
                button4.FlatAppearance.BorderColor = Color.Red;
                button4.FlatAppearance.BorderSize = 1;
                ButtonWindowsServiceIsForStop = true;
                ApplicationInfo.Mode = "Debug";
                Service1 myService = new Service1();
                this.SleepTime = ((int)this.SleepSecounds.Value) * 1000;
                myService.OnDebug(this.WinServiceFromConnections.Text, this.WinServiceToConnections.Text, SleepTime);
            }
            else
            {
                System.Windows.Forms.Application.ExitThread();
            }
           
           
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void MappingFromConnections_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton42_CheckedChanged(object sender, EventArgs e)
        {
            MappingFromConnections.Enabled = false;
            this.MappingFromConnections.Text = ToLocalConectionstring;

        }

        private void radioButton41_CheckedChanged(object sender, EventArgs e)
        {
            MappingFromConnections.Enabled = false;
            this.MappingFromConnections.Text = ToTestConectionstring;
        }

        private void radioButton39_CheckedChanged(object sender, EventArgs e)
        {
            MappingFromConnections.Enabled = false;
            this.MappingFromConnections.Text = ToCloudConectionstring;
        }

        private void radioButton40_CheckedChanged(object sender, EventArgs e)
        {
            MappingFromConnections.Enabled = true;
        }
 

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

     

 
 

        private string GetTextMapping(string TableNmae , string FieldName)
        {
            string Note = "Mapping From => "+Environment.NewLine+"Table Name: "+ TableNmae+ Environment.NewLine+"Field Name: "+ FieldName;
            return Note;
           
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                BuildFrom.Enabled = false;
                //BuildTo.Enabled = false;
            }
            else
            {
                BuildFrom.Enabled = true;
                //BuildTo.Enabled = true;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                numericUpDown2.Enabled = false;
             }
            else
            {
                numericUpDown2.Enabled = true;
             }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                //textBox3.Enabled = false;
                comboBox2.Enabled = false;
             }
            else
            {
                //textBox3.Enabled = true;
                comboBox2.Enabled = true;
            }
        }
 

        private void BuildTo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            this.LoadWaterMarksData();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            
            this.BuildConnectionStrings(false);
            string[] destinationConnectionArray = MappingFromConnections.Text.Split(',');
            if (destinationConnectionArray.Length != 4 || destinationConnectionArray.Length != 4)
            {
                MessageBox.Show("Connection String not valid");
                return;
            }
            ConnectionStringArguments destinationConnectionStringArguments = GetConnectionStringArguments(destinationConnectionArray);
            string ConnectionString = ServiceHelper.BuildConnectionString(destinationConnectionStringArguments);
            ServiceHelper.DeleteWatermarks(ConnectionString);
            this.LoadWaterMarksData();
        }
    }
}
