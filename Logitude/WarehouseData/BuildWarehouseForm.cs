
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
        string dbSourceConnection = "Logitude2-5_Main,sa,Saas256,.";//"LogitudeMain-PreR2,logitudemanager,!LO009008,logitudetest.database.windows.net";//"LogitudeMain-Test2,sa,Saas256,logitudetest.cloudapp.net";

        string dbDestinationConnection = "Logitude2-5_Global,sa,Saas256,.";


        public BuildWarehouseForm()
        {
            InitializeComponent();
            this.SourceConnectionlTextBox.Text = dbSourceConnection;
            this.DestinationConnectionlTextBox.Text = dbDestinationConnection;

        }


        delegate void SetControlValueCallback(string propName, object propValue, string tableName = null, string typeTable = null);
        private void SetControlPropertyValue(string propName, object propValue, string tableName = null, string typeTable = null)
        {
            Control oControl = null;
            switch (tableName)
            {
                case "Shipments":
                    if (typeTable == "Fact") oControl = FactShipmentsLabel;
                    else oControl = DWShipmentLable;

                    break;
                case "Cards":
                    if (typeTable == "DIM") oControl = DimPartnersLabel;
                    else oControl = DWPartnersLabel;

                    break;

                case "ShipmentMasterDatas":
                    oControl = DWShipmentMasterDatasLable;
                    break;


                case "Users":
                    if (typeTable == "DIM") oControl = DimUsersLabel;
                    else oControl = DWUserLabel;
                    break;


                case "Contacts":
                    oControl = DWContactsLabel;
                    break;

                case "Customers":
                    oControl = DWCustomersLabel;
                    break;
                case "Ports":
                    oControl = DWPortsLabel;
                    if (typeTable == "DIM") oControl = DimPortsLabel;
                    break;

                case "Tenants":
                    if (typeTable == "DIM") oControl = DimTenantLabel;
                    else oControl = DWTenantLabel;


                    break;

                case "Incoterms":
                    if (typeTable == "DIM") oControl = DimIncotermLabel;
                    else oControl = DWIcontermLabel;


                    break;

                case "Currencies":
                    if (typeTable == "DIM") oControl = DimCurrencyLabel;
                    else oControl = DWCurrencyLabel;


                    break;

                case "Departments":

                    if (typeTable == "DIM") oControl = DimDepartmentLabel;
                    else oControl = DWDepartmentLabel;

                    break;
                case "ShipmentComputedFields":

                    oControl = DWShipmentComputedFieldLabel;
                    break;

                default:
                    oControl = BuildWarehouseData;
                    break;
            }

            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] { propName, propValue, tableName, typeTable });
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

        bool  IsBuildDataRunning =false;
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
         
                        WarehouseHelper warehouseHelper = new WarehouseHelper();
                        string sourceConnectionString = warehouseHelper.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
                        string destinationConnectionString = warehouseHelper.BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);

                        string stepName = "";
                        try
                        {
                            SetControlPropertyValue("ForeColor", Color.Black);
                            SetControlPropertyValue("Text", "Starting...");

                            Stopwatch stopWatch = new Stopwatch();
                            stopWatch.Start();

                            warehouseHelper.CreateWaterMarksTable("WaterMarks",sourceConnectionString);

                            List<TableClass> tableNameLists = warehouseHelper.FillTable();

                            warehouseHelper.BuildWarehouseObjectField( tableNameLists, sourceConnectionString);
                            warehouseHelper.BuildDWObjectFieldDB(tableNameLists, sourceConnectionString);
                            
                            foreach (TableClass table in tableNameLists)
                            {
                                if (table.DispayInScreen)
                                {
                                    SetControlPropertyValue("ForeColor", Color.Black, table.DBTableName);
                                    SetControlPropertyValue("Text", "", table.DBTableName);

                                    SetControlPropertyValue("ForeColor", Color.Black, table.DBTableName, "DIM");
                                    SetControlPropertyValue("Text", "", table.DBTableName, "DIM");
                                    SetControlPropertyValue("ForeColor", Color.Black, table.DBTableName, "Fact");
                                    SetControlPropertyValue("Text", "", table.DBTableName, "Fact");

                                }
                            }


                            #region Create and Build  DW Table

                            foreach (TableClass table in tableNameLists)
                            {
                                    stepName = table.DBTableName;

                                    Stopwatch stopWatchDWTable = null;
                                    if (table.DispayInScreen)
                                    {
                                        stopWatchDWTable = new Stopwatch();
                                        stopWatchDWTable.Start();
                                        SetControlPropertyValue("Text", "Copying...", table.DBTableName);
                                    }

                                warehouseHelper.InitializationDWTable(table, sourceConnectionString, destinationConnectionString);
                                warehouseHelper.CreateIndex(table, table.KeyName, destinationConnectionString);

                                if (table.TableName != "WaterMark")
                                {
                                    warehouseHelper.CreateIndex(table, "AutomaticLastUpdateDate", destinationConnectionString);
                                    if (table.HasConstraint) warehouseHelper.AddConstraint(table, destinationConnectionString);
                                    if (table.HasNotSpecifiedValue) warehouseHelper.InSertNotSpecifiedValueToDW(table, destinationConnectionString);
                                }

                                else warehouseHelper.CreateIndex(table, "LastUpdateDate", destinationConnectionString);
                                warehouseHelper.CopyDataBase(TotalCountLable,table, sourceConnectionString, destinationConnectionString);

                                warehouseHelper.UpdateAutomaticLastUpdate(table, sourceConnectionString, destinationConnectionString);
                                if (table.DispayInScreen)
                                {
                                    stopWatchDWTable.Stop();
                                    TimeSpan stopWatchDWTableTs = stopWatchDWTable.Elapsed;
                                    SetControlPropertyValue("ForeColor", Color.Green, table.DBTableName);
                                    SetControlPropertyValue("Text", "Done in ( " + stopWatchDWTableTs.ToString(@"hh\:mm\:ss") + " )", table.DBTableName);
                                }


                            }

                            #endregion


                            #region Create and Build Dimensions Table


                              stepName = "BuildDateDimensionsTable";
                            warehouseHelper.ExecuteScript("BuildWarehouse", destinationConnectionString, "BuildDateDimensionsTable");
                            stepName = "RunOtherScripte";
                            warehouseHelper.RunOtherScripte(destinationConnectionString);
               



                            foreach (TableClass table in tableNameLists.Where(d => d.HasDimensionTable).ToList())
                            {
                                    Stopwatch stopWatchDimensionsTable = null;

                                    if (table.DispayInScreen)
                                    {
                                        stopWatchDimensionsTable = new Stopwatch();
                                        stopWatchDimensionsTable.Start();
                                        SetControlPropertyValue("Text", "Building ...", table.DBTableName, "DIM");
                                        SetControlPropertyValue("ForeColor", Color.Black, table.DBTableName, "DIM");
                                    }

                                    stepName = table.BuildScriptName;
                                   
                                    warehouseHelper.BuildAndExecuteDataWarehouseScript("BuildWarehouse", destinationConnectionString , table);


                                    if (table.DispayInScreen)
                                    {
                                        stopWatchDimensionsTable.Stop();
                                        TimeSpan stopWatchDimensionsTableTs = stopWatchDimensionsTable.Elapsed;
                                        SetControlPropertyValue("Text", "Done in ( " + stopWatchDimensionsTableTs.ToString(@"hh\:mm\:ss") + " )", table.DBTableName, "DIM");
                                        SetControlPropertyValue("ForeColor", Color.Green, table.DBTableName, "DIM");

                                    }
                                
                            }
                            #endregion

                            #region  Create and Build Fact Table
                            foreach (TableClass table in tableNameLists.Where(d => d.HasFactTable).ToList())
                            {
                               
                                    stepName = table.BuildScriptName;

                                    Stopwatch stopWatchDFactTable = new Stopwatch();
                                    stopWatchDFactTable.Start();
                                    if (table.TableName == "Shipment")
                                    {
                                        SetControlPropertyValue("ForeColor", Color.Black, table.DBTableName, "Fact");
                                        SetControlPropertyValue("Text", "Building...", table.DBTableName, "Fact");
                                    }
                                    warehouseHelper.BuildAndExecuteDataWarehouseScript("BuildWarehouse", destinationConnectionString , table);
                          
                                    if (table.TableName == "Shipment")
                                    {
                                        stopWatchDFactTable.Stop();
                                        TimeSpan stopWatchDFactTableTs = stopWatchDFactTable.Elapsed;
                                        SetControlPropertyValue("ForeColor", Color.Green, table.DBTableName, "Fact");
                                        SetControlPropertyValue("Text", "Done in ( " + stopWatchDFactTableTs.ToString(@"hh\:mm\:ss") + " )", table.DBTableName, "Fact");

                                    }
                                
                            }

                            #endregion

                            stopWatch.Stop();
                            TimeSpan ts = stopWatch.Elapsed;
                            SetControlPropertyValue("Text", "Done in ( " + ts.ToString(@"hh\:mm\:ss") + " )");
                            SetControlPropertyValue("ForeColor", Color.Green);

                            #region Statistic Data



                            foreach (TableClass table in tableNameLists.Where(d => d.DispayInScreen))
                            {
                                stepName = "DW table count";
                                GetCount(table, "DW", destinationConnectionString);
                            }
                            foreach (TableClass table in tableNameLists.Where(d => d.HasDimensionTable).ToList())
                            {
                                stepName = "DIM table count";
                                GetCount(table, "DIM", destinationConnectionString);
                            }

                            stepName = "Fact table count";
                            GetCount(tableNameLists.Where(d => d.DBTableName == "Shipments").FirstOrDefault(), "Fact", destinationConnectionString);

                            #endregion


                            IsBuildDataRunning = false;

                        }
                        catch (Exception ex)
                        {
                            IsBuildDataRunning = false;

                            string message = ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : "");
                            if (message.Length > 1500)  message = message.Substring(0, 1500);


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

        private void GetCount(TableClass table , string typeTable , string connectionString)
        {
            string name = "";
            if (typeTable == "DW") name = table.Dw_TableName;
            else if (typeTable == "DIM")
            {
                if (table.DBTableName == "Cards") name = "DIM_Partners";
                else if (table.DBTableName == "ShipmentLevels") name = "DIM_Levels";
                else if (table.DBTableName == "ShipmentTypes") name = "DIM_Types";
                else if (table.DBTableName == "EntityStatus") name = "DIM_ShipmentStatuses";
                
                else name = ("DIM_" + table.DBTableName);
            }
            else name = "Fact_" + table.DBTableName;

   
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

                    switch (table.DBTableName)
                    {
                        case "Shipments":
                            if (typeTable == "Fact") SetControlPropertyValue("Text", FactShipmentsLabel.Text + "     (" + countStart + ")", table.DBTableName, "Fact");
                            else  SetControlPropertyValue("Text", DWShipmentLable.Text + "     (" + countStart + ")", table.DBTableName);  ;

                            break;
                        case "Cards":
          
                            if (typeTable == "DIM") SetControlPropertyValue("Text", DimPartnersLabel.Text + "     (" + countStart + ")", table.DBTableName, "DIM");
                            else  SetControlPropertyValue("Text", DWPartnersLabel.Text + "     (" + countStart + ")", table.DBTableName) ;

                            break;

                        case "ShipmentMasterDatas":
                  
                            SetControlPropertyValue("Text", DWShipmentMasterDatasLable.Text + "     (" + countStart + ")", table.DBTableName);
                            break;


                        case "Users":
                  
                            if (typeTable == "DIM") SetControlPropertyValue("Text", DimUsersLabel.Text + "     (" + countStart + ")", table.DBTableName, "DIM");
                            else SetControlPropertyValue("Text", DWUserLabel.Text + "     (" + countStart + ")", table.DBTableName);

                            break;


                        case "Contacts":
           
                            SetControlPropertyValue("Text", DWContactsLabel.Text + "     (" + countStart + ")", table.DBTableName);
                            break;

                        case "Customers":
                     
                            SetControlPropertyValue("Text", DWCustomersLabel.Text + "     (" + countStart + ")", table.DBTableName);
                            break;
                        case "Ports":

                            if (typeTable == "DIM") SetControlPropertyValue("Text", DimPortsLabel.Text + "     (" + countStart + ")", table.DBTableName, "DIM");
                            else SetControlPropertyValue("Text", DWPortsLabel.Text + "     (" + countStart + ")", table.DBTableName);

                            break;

                        case "Tenants":

                            if (typeTable == "DIM") SetControlPropertyValue("Text", DimTenantLabel.Text + "     (" + countStart + ")", table.DBTableName, "DIM");
                            else SetControlPropertyValue("Text", DWTenantLabel.Text + "     (" + countStart + ")", table.DBTableName);

                            break;
                        case "Departments":

                            if (typeTable == "DIM") SetControlPropertyValue("Text", DimDepartmentLabel.Text + "     (" + countStart + ")", table.DBTableName, "DIM");
                            else SetControlPropertyValue("Text", DWDepartmentLabel.Text + "     (" + countStart + ")", table.DBTableName);

                            break;
                        case "Incoterms":

                            if (typeTable == "DIM") SetControlPropertyValue("Text", DimIncotermLabel.Text + "     (" + countStart + ")", table.DBTableName, "DIM");
                            else SetControlPropertyValue("Text", DWIcontermLabel.Text + "     (" + countStart + ")", table.DBTableName);

                            break;

                        case "Currencies":

                            if (typeTable == "DIM") SetControlPropertyValue("Text", DimCurrencyLabel.Text + "     (" + countStart + ")", table.DBTableName, "DIM");
                            else SetControlPropertyValue("Text", DWCurrencyLabel.Text + "     (" + countStart + ")", table.DBTableName);

                            break;
                
                        case "ShipmentComputedFields":
                            SetControlPropertyValue("Text", DWShipmentComputedFieldLabel.Text + "     (" + countStart + ")", table.DBTableName);
               
                            break;


                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }




            }

        }

    }




}
