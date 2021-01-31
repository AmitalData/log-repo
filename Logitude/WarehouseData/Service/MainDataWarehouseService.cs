using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarehouseData.Service;
using WarehouseDataViews.Service;

namespace WarehouseData.Helper
{
    public class MainDataWarehouseService : GeneralDataWarehouseService
    {
        public PrivateTenantDataWarehouse privateTenantDataWarehouse;
        ObjectFieldDataWarehouseService objectFieldDataWarehouseService;
        FinalDataWarehouseService finalDataWarehouseService;
        CustomFieldWarehouseService customFieldWarehouseService;
        DimensionWarehouseService dimensionWarehouseService;
        FactWarehouseService factWarehouseService;
        WaterMarkDataWarehouseService waterMarkDataWarehouseService;
        DWDataWarehouseService dWDataWarehouseService;

        public MainDataWarehouseService(string applicationName = "WarehouseData", string applicationMode = "Debug") :base(applicationName, applicationMode)
        {

            InitializeDataWarehouseServices();
        }

        private void InitializeDataWarehouseServices()
        {
            finalDataWarehouseService = new FinalDataWarehouseService();
            customFieldWarehouseService = new CustomFieldWarehouseService();
            dimensionWarehouseService = new DimensionWarehouseService(ApplicationName, ApplicationMode);
            factWarehouseService = new FactWarehouseService(ApplicationName, ApplicationMode);
            objectFieldDataWarehouseService = new ObjectFieldDataWarehouseService();
            waterMarkDataWarehouseService = new WaterMarkDataWarehouseService();
            privateTenantDataWarehouse = new PrivateTenantDataWarehouse();
            dWDataWarehouseService = new DWDataWarehouseService();
        }


        public List<TableClass> BulidDataWarehouseTableLists (string connectionString)
        {
            List<TableClass> dataWarehouseTables = FillDataWarehouseTable();
            dataWarehouseTables = objectFieldDataWarehouseService.SetCustomObjectFieldMetaData(dataWarehouseTables, connectionString);
            dataWarehouseTables = objectFieldDataWarehouseService.BuildWarehouseObjectFieldOnTables(dataWarehouseTables, connectionString);
            dataWarehouseTables = objectFieldDataWarehouseService.BuildDWObjectFieldDB(dataWarehouseTables, connectionString);

            var dwObjectTables = GetDataTableFromSql(connectionString, "select IndexesXml,Code from DWObjectTables where IndexesXml is not null and IndexesXml !=''");
            foreach (DataRow row in dwObjectTables.AsEnumerable())
            {
                string tableCode = row["Code"] != null ? row["Code"].ToString() : "";
                string indexesXml = row["IndexesXml"] != null ? row["IndexesXml"].ToString() : "";
                var table = dataWarehouseTables.Where(d => d.DWObjectTableCode == tableCode).FirstOrDefault();
                if (table != null) table.Indexes = GetDWObjectFieldIndexes(indexesXml);

            }

            dWDataWarehouseService.CustomObjectFieldTableLists = dataWarehouseTables.Where(d => !string.IsNullOrEmpty(d.CustomFieldObjectTableName)).GroupBy(d => d.CustomFieldObjectTableName).Select(d => d.First().CustomFieldObjectTableName).ToList();
           
            return dataWarehouseTables;
        }

        #region DW Table

        public void BuildDWDataBase(string sourceConnectionString,string destinationConnectionString , TableClass table , Control controlLable = null) { 
        
            dWDataWarehouseService.InitializationDWTable(table, sourceConnectionString, destinationConnectionString);
            dWDataWarehouseService.CreateIndex(table, table.KeyName, destinationConnectionString);

            if (table.TableName != "WaterMark")
            {
                dWDataWarehouseService.CreateIndex(table, "AutomaticLastUpdateDate", destinationConnectionString);
                if (!string.IsNullOrEmpty(table.AdditionalIndexes)) dWDataWarehouseService.CreateAdditionalIndexes(table, destinationConnectionString);
                if (table.HasConstraint) dWDataWarehouseService.AddConstraint(table, destinationConnectionString);
                if (table.HasNotSpecifiedValue) dWDataWarehouseService.InSertNotSpecifiedValueToDW(table, destinationConnectionString);
            }
            else dWDataWarehouseService.CreateIndex(table, "LastUpdateDate", destinationConnectionString);


            dWDataWarehouseService.CopyDataBase(controlLable, table, sourceConnectionString, destinationConnectionString);
            dWDataWarehouseService.UpdateAutomaticLastUpdate(table, sourceConnectionString, destinationConnectionString);





        }

        public void UpdateDWDataBase(TableClass table, string sourceConnectionString, string destinationConnectionString, int? privateTenant = null, string relatedTenants = null)
        {
            dWDataWarehouseService.UpdateDWDataBase( new BuildDWArgs() {table = table, SourceConnectionString = sourceConnectionString , DestinationConnectionString = destinationConnectionString,PrivateTenant =privateTenant, RelatedTenants = relatedTenants });
        }

        #endregion


        #region  Dimension Table

        public void BuildDimensionTable(string connectionString, TableClass table)
        {
            dimensionWarehouseService.BuildDimensionTable(connectionString, table);
        }

        public void UpdateDimensionTable(string connectionString, TableClass table)
        {
            dimensionWarehouseService.UpdateDimensionTable(connectionString, table);
        }

        #endregion

        #region Fact Table
        public void BuildFactTable(string connectionString, TableClass table)
        {
            factWarehouseService.BuildFactTable(connectionString, table);
        }
        public void UpdateFactTable(string connectionString, TableClass table)
        {
            factWarehouseService.UpdateFactTable(connectionString, table);


        }

        #endregion


        public void FinishBuildingDataWarehouse(string connectionString,List<TableClass> tableLists)
        {
        
            finalDataWarehouseService.FinishBuildingDataWarehouse(connectionString, tableLists);


        }

        public void RunAdditionalScripte(string connectionString, List<TableClass> tableLists, bool isIncrement = false)
        {

            customFieldWarehouseService.BuildCustomObjectFieldsTable(connectionString, tableLists);
            RunSqlFunctions(connectionString);
        }


        #region Service Method
        public void BuildDataWarehouse(string sourceConnectionString, string destinationConnectionString, int? privateTenant = null, string relatedTenants = null)
        {
            bool isPrivateDB = privateTenant != null ? true : false;

            if (!isPrivateDB) CreateWaterMarksTable("WaterMarks", sourceConnectionString);

            List<TableClass> tableNameLists = BulidDataWarehouseTableLists(sourceConnectionString);

            foreach (TableClass table in tableNameLists.Where(d=>!d.HasFactTable))
            {
                if (table.TableName != "WaterMark")
                {
                    this.dWDataWarehouseService.InitializationDWTable(table, sourceConnectionString, destinationConnectionString);
                    this.dWDataWarehouseService.CreateIndex(table, table.KeyName, destinationConnectionString);
                    this.dWDataWarehouseService.CreateIndex(table, "AutomaticLastUpdateDate", destinationConnectionString);
                    if (!string.IsNullOrEmpty(table.AdditionalIndexes)) dWDataWarehouseService.CreateAdditionalIndexes(table, destinationConnectionString);

                    if (table.HasConstraint) this.dWDataWarehouseService.AddConstraint(table, destinationConnectionString);
                    if (table.HasNotSpecifiedValue) this.dWDataWarehouseService.InSertNotSpecifiedValueToDW(table, destinationConnectionString, privateTenant);
                }
                else
                {
                    if (isPrivateDB) CreateWaterMarksTable("dw_WaterMarks", destinationConnectionString);
                    else
                    {
                        this.dWDataWarehouseService.InitializationDWTable(table, sourceConnectionString, destinationConnectionString);
                        this.dWDataWarehouseService.CreateIndex(table, table.KeyName, destinationConnectionString);
                        this.dWDataWarehouseService.CreateIndex(table, "LastUpdateDate", destinationConnectionString);
                    }
                }


                this.dWDataWarehouseService.CopyDataBase(null, table, sourceConnectionString, destinationConnectionString, privateTenant, relatedTenants);
                this.dWDataWarehouseService.UpdateAutomaticLastUpdate(table, sourceConnectionString, destinationConnectionString, privateTenant);
            }

            ExecuteScript("BuildWarehouse", "BuildDateDimensionsTable", destinationConnectionString);

            RunAdditionalScripte(destinationConnectionString, tableNameLists);


            foreach (TableClass table in tableNameLists.Where(d => d.HasDimensionTable).ToList())
            {
                this.BuildDimensionTable(destinationConnectionString, table);
            }

            foreach (TableClass table in tableNameLists.Where(d => d.HasFactTable).ToList())
            {
                this.BuildFactTable(destinationConnectionString, table);
            }


            FinishBuildingDataWarehouse(destinationConnectionString,tableNameLists);
        }

        public void UpdateDataWarehouse(string sourceConnectionString, string destinationConnectionString, int? privateTenant = null, string relatedTenants = null)
        {
            List<TableClass> tableNameLists = this.BulidDataWarehouseTableLists(sourceConnectionString);
            foreach (TableClass table in tableNameLists.Where(d => !d.HasFactTable))
            {
                if (table.DBTableName != "WaterMarks")
                {
                    UpdateDWDataBase(table, sourceConnectionString, destinationConnectionString, privateTenant, relatedTenants);

                }
            }

            RunAdditionalScripte(destinationConnectionString, tableNameLists, true);

            #region Update Dimensions Table
            foreach (TableClass table in tableNameLists.Where(d => d.HasDimensionTable).ToList())
            {
                this.UpdateDimensionTable(destinationConnectionString, table);
            }
            #endregion

            #region Update Fact Table

            foreach (TableClass table in tableNameLists.Where(d => d.HasFactTable).ToList())
            {
                this.UpdateFactTable(destinationConnectionString, table);
            }

            #endregion

        }

        public void BuildOrUpdatePrivateDataWarehouse(string dbsourceConnection, string dbDestinationConnection, string type)
        {
            if (!string.IsNullOrEmpty(dbsourceConnection) && !string.IsNullOrEmpty(dbDestinationConnection))
            {
                string[] sourceConnectionArray = dbsourceConnection.Split(',');
                string[] destinationConnectionArray = dbDestinationConnection.Split(',');

                if (sourceConnectionArray.Length != 4 || destinationConnectionArray.Length != 4)
                {
                    if (ApplicationName != "Service") MessageBox.Show("connection not valid");
                    return;
                }

                string sourceConnectionString = BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);


                var dWHSettingsTable = privateTenantDataWarehouse.GetPrivateTenant(sourceConnectionString);
                PrivateDataWarehouseViewService privateDataWarehouseViewService = null ;
                if (type == "Build")
                {
                    CreateWaterMarksTable("PrivateWaterMarks", sourceConnectionString, true);
                    privateDataWarehouseViewService = new PrivateDataWarehouseViewService(sourceConnectionString);
                }
                FeatureDataWarehouseService featureDataWarehouseService = new FeatureDataWarehouseService(sourceConnectionString.Replace("Main" ,"Global"), sourceConnectionString);

                foreach (DataRow row in dWHSettingsTable.Rows)
                {
                    int tenant = Int32.Parse(row["Tenant"].ToString());
                    string catalog = row["Catalog"].ToString();
                    string userName = row["UserName"].ToString();
                    string password = row["Password"].ToString();
                    string server = row["Server"].ToString();
                    string privateUserName = row["PrivateUserName"].ToString();
                    bool isParentTenant = bool.Parse(row["IsParentTenant"].ToString());

                    if (featureDataWarehouseService.CheckFeature("PrivateDB", tenant))
                    {
                        string destinationConnectionString = BuildConnectionString(catalog, userName, password, server);
                        List<int> relatedTenants = privateTenantDataWarehouse.GetPrivateRelatedTenants(sourceConnectionString, tenant);
                        if (!relatedTenants.Contains(tenant)) relatedTenants.Add(tenant);
                        string tenants = privateTenantDataWarehouse.ConvertIntgerListToString(relatedTenants);
                        if (type == "Build")
                        {
                            BuildDataWarehouse(sourceConnectionString, destinationConnectionString, tenant, tenants);
                            privateDataWarehouseViewService.GeneratePrivateViews(new PrivateViewArgs() { ConnectionString = destinationConnectionString, UserName = privateUserName, Tenant = tenant, Catalog = catalog, ApplyGrantOnViews = !string.IsNullOrEmpty(privateUserName) ? true : false , IsParentTenant = isParentTenant });

                        }
                        else UpdateDataWarehouse(sourceConnectionString, destinationConnectionString, tenant, tenants);
                    }
                }
            }
            else if (ApplicationName != "Service") MessageBox.Show("Connection Problem");
   
        }
         #endregion

    }
}
