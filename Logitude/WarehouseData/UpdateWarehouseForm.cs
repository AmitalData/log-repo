using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WarehouseData;
using WarehouseData.Helper;

namespace WarehouseData
{
    public partial class UpdateWarehouseForm : Form
    {
        string dbSourceConnection = "Logitude2-5_Main,sa,Saas256,.";//"LogitudeMain-PreR2,logitudemanager,!LO009008,logitudetest.database.windows.net";//"LogitudeMain-Test2,sa,Saas256,logitudetest.cloudapp.net";
        string dbDestinationConnection = "Logitude2-5_Global,sa,Saas256,.";

        public UpdateWarehouseForm()
        {
            InitializeComponent();

            this.SourceConnectionlTextBox.Text = dbSourceConnection;
            this.DestinationConnectionlTextBox.Text = dbDestinationConnection;
        }

    

        
        private void UpdateWarehouseDataButton_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(() => Start());
            thread.IsBackground = true;
            thread.Start();
        }

        bool IsUpdateDataRunning = false;
        private void Start()
        {
            try
            {


                if (!string.IsNullOrEmpty(dbSourceConnection) && !string.IsNullOrEmpty(dbDestinationConnection))
                {
                    if (!IsUpdateDataRunning)
                    {
                        IsUpdateDataRunning = true;


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

                            List<TableClass> tableNameLists = warehouseHelper.FillTable();
                            warehouseHelper.BuildWarehouseObjectField(tableNameLists, sourceConnectionString);

                            #region Update DW Table

                            foreach (TableClass table in tableNameLists)
                            {
                                if (table.DispayInScreen)
                                {
                                    SetControlPropertyValue("ForeColor", Color.Black, table.DBTableName);
                                    SetControlPropertyValue("Text", "", table.DBTableName);

                                    SetControlPropertyValue("ForeColor", Color.Black, table.DBTableName, "Dim");
                                    SetControlPropertyValue("Text", "", table.DBTableName, "Dim");
                                    SetControlPropertyValue("ForeColor", Color.Black, table.DBTableName, "Fact");
                                    SetControlPropertyValue("Text", "", table.DBTableName, "Fact");

                                }
                            }


                            foreach (TableClass table in tableNameLists)
                            {

                                Stopwatch stopWatchDWTable = null;
                                if (table.DispayInScreen)
                                {
                                    stopWatchDWTable = new Stopwatch();
                                    stopWatchDWTable.Start();
                                    SetControlPropertyValue("Text", "Updating...", table.DBTableName);
                                }

                                stepName = table.DBTableName;
                                if (table.DBTableName != "WaterMarks")
                                {
                                    warehouseHelper.UpdateDWDataBase(table, sourceConnectionString, destinationConnectionString);
                
                                    if (table.DispayInScreen)
                                    {

                                        string message = !table.IsUpdated ? "  No update available" : ("  Updated (" + table.UpdatedCount.ToString() + "Records )");
                                        stopWatchDWTable.Stop();
                                        TimeSpan stopWatchDWTableTs = stopWatchDWTable.Elapsed;
                                        SetControlPropertyValue("ForeColor", !table.IsUpdated ? Color.Green : Color.Red, table.DBTableName);
                                        SetControlPropertyValue("Text", "Done in ( " + stopWatchDWTableTs.ToString(@"hh\:mm\:ss") + " ) " + message, table.DBTableName);


                                    }
                                }
                            }

                            #endregion

                            warehouseHelper.RunSqlFunctions(destinationConnectionString);


                            #region Update Dimensions Table
                            foreach (TableClass table in tableNameLists.Where(d => d.HasDimensionTable).ToList())
                            {
                                Stopwatch stopWatchDimensionsTable = null;
                                stepName = table.IncrementalScriptName;
                                stopWatchDimensionsTable = new Stopwatch();
                                stopWatchDimensionsTable.Start();
                                SetControlPropertyValue("Text", "Updating ...", table.DBTableName, "Dim");
                                SetControlPropertyValue("ForeColor", Color.Black, table.DBTableName, "Dim");

                                warehouseHelper.BuildAndExecuteDataWarehouseScript("IncrementalWarehouse", destinationConnectionString , table);

                                stopWatchDimensionsTable.Stop();
                                TimeSpan stopWatchDimensionsTableTs = stopWatchDimensionsTable.Elapsed;
                                SetControlPropertyValue("Text", "Done in ( " + stopWatchDimensionsTableTs.ToString(@"hh\:mm\:ss") + " )", table.DBTableName, "Dim");
                                SetControlPropertyValue("ForeColor", Color.Green, table.DBTableName, "Dim");

                            }
                            #endregion

                            #region Update Fact Table


                            foreach (TableClass table in tableNameLists.Where(d => d.HasFactTable).ToList())
                            {

                                Stopwatch stopWatchDFactTable = null;
                                if (table.TableName == "Shipment")
                                {
                                    stopWatchDFactTable = new Stopwatch();
                                    stopWatchDFactTable.Start();
                                    SetControlPropertyValue("ForeColor", Color.Black, table.DBTableName, "Fact");
                                    SetControlPropertyValue("Text", "Updating...", table.DBTableName, "Fact");
                                }

                                warehouseHelper.RemoveDataFromFactShipment(table, destinationConnectionString);
                                warehouseHelper.BuildAndExecuteDataWarehouseScript( "IncrementalWarehouse", destinationConnectionString, table);


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

                            IsUpdateDataRunning = false;
                        }
                        catch (Exception ex)
                        {
                            IsUpdateDataRunning = false;
                            MessageBox.Show(ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : ""), stepName);
                        }
                    }
                }
                else MessageBox.Show("Connection Problem");
            }

            catch (Exception ex)
            {
                IsUpdateDataRunning = false;
                MessageBox.Show(ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : ""));

            }



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
                    if (typeTable == "Dim") oControl = DimPartnersLabel;
                    else oControl = DWPartnersLabel;

                    break;

                case "ShipmentMasterDatas":
                    oControl = DWShipmentMasterDatasLable;
                    break;


                case "Users":
                    if (typeTable == "Dim") oControl = DimUsersLabel;
                    else oControl = DWUserLabel;
                    break;

                case "Ports":
                    oControl = DWPortsLabel;
                    if (typeTable == "Dim") oControl = DimPortsLabel;
                    break;

                case "Tenants":
                    if (typeTable == "Dim") oControl = DimTenantLabel;
                    else oControl = DWTenantLabel;
          

                    break;
     
                case "Incoterms":
                    if (typeTable == "Dim") oControl = DimIncotermLabel;
                    else oControl = DWIcontermLabel;


                    break;

                case "Currencies":
                    if (typeTable == "Dim") oControl = DimCurrencyLabel;
                    else oControl = DWCurrencyLabel;


                    break;

                case "Departments":
                    
                    if (typeTable == "Dim") oControl = DimDepartmentLabel;
                    else oControl = DWDepartmentLabel;

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

        private void SourceConnectionlTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (textbox != null)
            {
                this.dbSourceConnection = textbox.Text;
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
    }
}
