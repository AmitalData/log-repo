
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using WarehouseData.Helper;

namespace WarehouseData
{
    public partial class BuildWarehouseForm : Form
    {
        string dbSourceConnection = "2021R1_Main,sa,Saas256,.";//"LogitudeMain-PreR2,logitudemanager,!LO009008,logitudetest.database.windows.net";//"LogitudeMain-Test2,sa,Saas256,logitudetest.cloudapp.net";

        string dbDestinationConnection = "2021R1_Global,sa,Saas256,.";


        public BuildWarehouseForm()
        {
            InitializeComponent();
            this.SourceConnectionlTextBox.Text = dbSourceConnection;
            this.DestinationConnectionlTextBox.Text = dbDestinationConnection;

        }


        delegate void SetControlValueCallback(string propName, object propValue, TableClass table = null, string typeTable = null);
        private void SetControlPropertyValue(string propName, object propValue, TableClass table = null, string typeTable = "DW")
        {

            Control oControl = null;
            if (table != null)
            {
                string lableName = (table.HasFactTable ? table.DWObjectTableCode.Replace("_", "") : (typeTable + table.DBTableName)) + "Label";
                oControl = this.Controls.OfType<Control>().Where(l => l.Name.ToLower().Contains((lableName).ToLower())).FirstOrDefault();


            }
            else oControl = BuildWarehouseData;
            if (oControl != null)
            {

                if (oControl.InvokeRequired)
                {
                    SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                    oControl.Invoke(d, new object[] { propName, propValue, table, typeTable });
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

        bool IsBuildDataRunning = false;
        private void BuildDataFirstTime_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => BuildData());
            thread.IsBackground = true;
            thread.Start();

        }

        private void BuildData()
        {
            try
            {
                if (!string.IsNullOrEmpty(dbSourceConnection) && !string.IsNullOrEmpty(dbDestinationConnection))
                {
                    if (!IsBuildDataRunning)
                    {
                        IsBuildDataRunning = true;

                        string[] sourceConnectionArray = dbSourceConnection.Split(',');
                        string[] destinationConnectionArray = dbDestinationConnection.Split(',');

                        if (sourceConnectionArray.Length != 4 || destinationConnectionArray.Length != 4)
                        {
                            MessageBox.Show("connection not valid");
                            return;
                        }

                        MainDataWarehouseService mainDataWarehouseService = new MainDataWarehouseService();
                        string sourceConnectionString = mainDataWarehouseService.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
                        string destinationConnectionString = mainDataWarehouseService.BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);
                        string stepName = "";
                        try
                        {
                            SetControlPropertyValue("ForeColor", Color.Black);
                            SetControlPropertyValue("Text", "Starting...");

                            Stopwatch stopWatch = new Stopwatch();
                            stopWatch.Start();

                            mainDataWarehouseService.CreateWaterMarksTable("WaterMarks", sourceConnectionString);

                            List<TableClass> tableNameLists = mainDataWarehouseService.BulidDataWarehouseTableLists(sourceConnectionString);

                            foreach (TableClass table in tableNameLists)
                            {
                                if (table.DispayInScreen)
                                {
                                    SetControlPropertyValue("ForeColor", Color.Black, table);
                                    SetControlPropertyValue("Text", "", table);

                                    SetControlPropertyValue("ForeColor", Color.Black, table, "DIM");
                                    SetControlPropertyValue("Text", "", table, "DIM");
                                    SetControlPropertyValue("ForeColor", Color.Black, table, "Fact");
                                    SetControlPropertyValue("Text", "", table, "Fact");

                                }
                            }


                            #region Create and Build  DW Table

                            TableClass waterMark = tableNameLists.Where(d => d.TableName == "WaterMark").FirstOrDefault();
                           Parallel.ForEach(tableNameLists.Where(d => !d.HasFactTable && d.TableName != waterMark.TableName).ToList(), (table) =>
                           {
                               stepName = table.DBTableName;


                               Stopwatch stopWatchDWTable = null;
                               if (table.DispayInScreen)
                               {
                                   stopWatchDWTable = new Stopwatch();
                                   stopWatchDWTable.Start();
                                   SetControlPropertyValue("Text", "Copying...", table);
                               }


                               mainDataWarehouseService.BuildDWDataBase(sourceConnectionString, destinationConnectionString, table, TotalCountLabel);

                               if (table.DispayInScreen)
                               {
                                   stopWatchDWTable.Stop();
                                   TimeSpan stopWatchDWTableTs = stopWatchDWTable.Elapsed;
                                   SetControlPropertyValue("ForeColor", Color.Green, table);
                                   SetControlPropertyValue("Text", "Done in ( " + stopWatchDWTableTs.ToString(@"hh\:mm\:ss") + " )", table);
                               }


                           });


                            mainDataWarehouseService.BuildDWDataBase(sourceConnectionString, destinationConnectionString, waterMark, TotalCountLabel);

                            #endregion


                            #region Create and Build Dimensions Table


                            stepName = "BuildDateDimensionsTable";
                            mainDataWarehouseService.ExecuteFixedDimensionScripts(destinationConnectionString);

                            //mainDataWarehouseService.ExecuteScript("BuildWarehouse", "BuildDateDimensionsTable", destinationConnectionString);
                            //mainDataWarehouseService.ExecuteScript("BuildWarehouse", "BuildInvoiceFiltersDimensionsTable", destinationConnectionString);

                            stepName = "RunOtherScripte";
                            mainDataWarehouseService.RunAdditionalScripte(destinationConnectionString, tableNameLists);


                            Parallel.ForEach(tableNameLists.Where(d => d.HasDimensionTable).ToList(), (table) =>
                            {
                                Stopwatch stopWatchDimensionsTable = null;

                                if (table.DispayInScreen)
                                {
                                    stopWatchDimensionsTable = new Stopwatch();
                                    stopWatchDimensionsTable.Start();
                                    SetControlPropertyValue("Text", "Building ...", table, "DIM");
                                    SetControlPropertyValue("ForeColor", Color.Black, table, "DIM");
                                }

                                stepName = table.BuildScriptName;

                                mainDataWarehouseService.BuildDimensionTable(destinationConnectionString, table);

                                if (table.DispayInScreen)
                                {
                                    stopWatchDimensionsTable.Stop();
                                    TimeSpan stopWatchDimensionsTableTs = stopWatchDimensionsTable.Elapsed;
                                    SetControlPropertyValue("Text", "Done in ( " + stopWatchDimensionsTableTs.ToString(@"hh\:mm\:ss") + " )", table, "DIM");
                                    SetControlPropertyValue("ForeColor", Color.Green, table, "DIM");

                                }

                            });
                            #endregion

                            #region  Create and Build Fact Table
                            Parallel.ForEach(tableNameLists.Where(d => d.HasFactTable).ToList(), (table) =>
                            {
                                stepName = table.BuildScriptName;

                                Stopwatch stopWatchDFactTable = new Stopwatch();
                                stopWatchDFactTable.Start();

                                SetControlPropertyValue("ForeColor", Color.Black, table, "Fact");
                                SetControlPropertyValue("Text", "Building...", table, "Fact");

                                mainDataWarehouseService.BuildFactTable(destinationConnectionString, table);

                                stopWatchDFactTable.Stop();
                                TimeSpan stopWatchDFactTableTs = stopWatchDFactTable.Elapsed;
                                SetControlPropertyValue("ForeColor", Color.Green, table, "Fact");
                                SetControlPropertyValue("Text", "Done in ( " + stopWatchDFactTableTs.ToString(@"hh\:mm\:ss") + " )", table, "Fact");

                            });

                            #endregion



                            mainDataWarehouseService.FinishBuildingDataWarehouse(sourceConnectionString ,destinationConnectionString, tableNameLists);
                            stopWatch.Stop();
                            TimeSpan ts = stopWatch.Elapsed;
                            SetControlPropertyValue("Text", "Done in ( " + ts.ToString(@"hh\:mm\:ss") + " )");
                            SetControlPropertyValue("ForeColor", Color.Green);

                            #region Statistic Data



                            foreach (TableClass table in tableNameLists.Where(d => d.DispayInScreen && !d.HasFactTable))
                            {
                                stepName = "DW table count";
                                GetCount(table, "DW", destinationConnectionString);
                            }
                            foreach (TableClass table in tableNameLists.Where(d => d.HasDimensionTable).ToList())
                            {
                                stepName = "DIM table count";
                                GetCount(table, "DIM", destinationConnectionString);
                            }

                            foreach (TableClass table in tableNameLists.Where(d => d.HasFactTable).ToList())
                            {
                                stepName = table.DWObjectTableCode + " table count";
                                GetCount(table, "Fact", destinationConnectionString);
                            }




                            #endregion


                            IsBuildDataRunning = false;

                        }
                        catch (Exception ex)
                        {
                            IsBuildDataRunning = false;

                            string message = ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                            if (message.Length > 1500) message = message.Substring(0, 1500);


                            MessageBox.Show(message, stepName);
                        }
                    }

                }
                else MessageBox.Show("Connection Problem");
            }

            catch (Exception ex)
            {
                IsBuildDataRunning = false;
                string message = ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                if (message.Length > 1500) message = message.Substring(0, 1500);
                MessageBox.Show(message);

            }




        }

        private void DestinationConnectionlTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (textbox != null)
            {
                this.dbDestinationConnection = textbox.Text;
            }
        }

        private void SourceConnectionlTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (textbox != null)
            {
                this.dbSourceConnection = textbox.Text;
            }
        }

        private void GetCount(TableClass table, string typeTable, string connectionString)
        {


            string name = "";
            if (typeTable == "DW") name = table.Dw_TableName;
            else name = table.DWObjectTableCode;

            using (SqlConnection sourceConnection =
                       new SqlConnection(connectionString))
            {
                sourceConnection.Open();
                SqlCommand commandRowCount = new SqlCommand(
                "SELECT COUNT(*) FROM " +
                "dbo." + name + ";",
                sourceConnection);

                try
                {
                    long countStart = System.Convert.ToInt32(
                        commandRowCount.ExecuteScalar());
                    DisplayCountValueToScreen(table, typeTable, countStart);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void DisplayCountValueToScreen(TableClass table, string typeTable, long count)
        {

            if (table.HasFactTable)
            {

            }
            string lableName = (table.HasFactTable ? table.DWObjectTableCode.Replace("_", "") : (typeTable + table.DBTableName)) + "Label";
            var labelEntity = this.Controls.OfType<Label>().Where(l => l.Name.ToLower().Contains((lableName).ToLower())).FirstOrDefault();
            if (labelEntity != null)
            {
                SetControlPropertyValue("Text", labelEntity.Text + "     (" + count + ")", table, typeTable);
            }
        }
         
    }

}
