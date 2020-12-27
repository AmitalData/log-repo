  
 
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.CargoTracking.BL.CoreBL.Batch
{
    public class BuildCargoTrackingShipments : BatchTaskExecutionsService
    {
        string sourceConnectionString = string.Empty;
        string destinationConnectionString = string.Empty;
        CargoTrackingMainService cargoTrackingMainService;
        CargoTrackingArgs CargoTrackingArguments;

        public BuildCargoTrackingShipments(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
            cargoTrackingMainService = new CargoTrackingMainService();
            BuildConnectionString();
        }

        public override void RunCode()
        {
            CargoTrackingArguments = GetCargoTrackingArgs();
            try
            {
                TenantRepository tenantRepository = new TenantRepository(0);
                UpdateIsIncrementalRunning(tenantRepository,true);
                ServiceHelper.CheckAndUpdateWaterMark(destinationConnectionString,sourceConnectionString);
                AddAllTablesToThread(CargoTrackingTableList.FillCargoTableList());
                UpdateIsIncrementalRunning(tenantRepository,false);
            }

            catch (Exception e)
            {
                throw new Exception(e.Message+"\n"+e.StackTrace);
            }

        }

        private void UpdateIsIncrementalRunning(TenantRepository tenantRepository,bool IsRunning)
        {
            Tenant tenant = tenantRepository.GetSingleTenant(0);
            tenant.IsIncrementalBuildRunning = IsRunning;
            tenantRepository.Update(tenant);
            tenantRepository.SubmitChanges();
        }


        private CargoTrackingArgs GetCargoTrackingArgs()
        {
            string xmlParameters = BatchTaskExecution.PrametersXml;
            System.IO.StringReader stringReader = new System.IO.StringReader(xmlParameters);
            XmlSerializer serializer = new XmlSerializer(typeof(CargoTrackingArgs));
            CargoTrackingArgs  Args = serializer.Deserialize(stringReader) as CargoTrackingArgs;
            return Args;
        }

        private void UpdateCargoDataBase(CargoTrackingTable table)
        {
            CargoTrackingArguments CargoTrackingArgs = new CargoTrackingArguments
            {
                FromDate = CargoTrackingArguments.FromDate,
                Tenant = CargoTrackingArguments.Tenant,
                ToDate = CargoTrackingArguments.ToDate,
                ThreadNumber = 50,
                FormTableName = null,
            };
            CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs = new CargoTrackingUpdateDataBaseArgs()
            {
                BuildCargoArgs = new CargoTrackingServices.HelperClasses.CargoTrackingArgs() { Table = table, SourceConnectionString = sourceConnectionString, DestinationConnectionString = destinationConnectionString },
                NumberOfBulkPerTime = 1000,
                IsUpdateAfterFinished = null,
                CargoTrackingArguments = CargoTrackingArgs,
                IsUpdateFromBuild = true,
            };
            cargoTrackingMainService.UpdateCargoTrackingDataBase(cargoTrackingDataBaseArgs);
        }


        private void AddAllTablesToThread(List<CargoTrackingTable> CargoTableLists)
        {
            foreach (CargoTrackingTable table in CargoTableLists)
            {

                UpdateCargoDataBase(table);

            }
        }

        private void BuildConnectionString()
        {
            string dbConnectionFrom = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString;
            string dbConnectionTo = ConfigurationManager.ConnectionStrings["CargoTrackingStr"].ConnectionString;
            string[] sourceConnectionArray = dbConnectionFrom.Split(',');
            string[] destinationConnectionArray = dbConnectionTo.Split(',');
            string[] SourceMainDB = GetMainDBConnectionString(BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3])).Split(',');
            sourceConnectionString = BuildConnectionString(SourceMainDB[0], SourceMainDB[1], SourceMainDB[2], SourceMainDB[3]);
            destinationConnectionString = BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);
        }

        private string GetUpdateDataBaseCondition(CargoTrackingServices.HelperClasses.CargoTrackingArgs buildCargoArgs)
        {

            string condition = " where AutomaticLastUpdateDate > ( select LastUpdateDate from WaterMarks where TableName = " + "'" + buildCargoArgs.Table.CargoTracking_TableName + "')";
            return condition;
        }

        private string BuildConnectionString(string catalog, string userName, string password, string server)
        {
            string result = "Data Source=" + server + ";Initial Catalog=" + catalog + ";Integrated Security=False;Persist Security Info=True;User ID=" + userName + ";Password= " + password + ";MultipleActiveResultSets=True;Connect Timeout=60";
            return result;
        }

        public string GetMainDBConnectionString(string connectionString)
        {
            string result = null;
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand com = new SqlCommand("select DBConnection,SecondaryAzureDBConnection from dbo.GlobalDBs where Id =0;", con);
            try
            {
                con.Open();

                using (SqlDataReader reader = com.ExecuteReader())
                {
                    reader.Read();

                    var dbConnectionString = reader["DBConnection"];
                    if (dbConnectionString != null && !string.IsNullOrEmpty(dbConnectionString.ToString()))
                    {
                        result = dbConnectionString.ToString();
                    }
                    else
                    {
                        dbConnectionString = reader["SecondaryAzureDBConnection"];
                        if (dbConnectionString != null && !string.IsNullOrEmpty(dbConnectionString.ToString())) result = dbConnectionString.ToString();
                    }

                }
            }
            finally
            {
                con.Close();
            }

            return result;
        }
    }

    

    public class CargoTrackingArgs
    {
        public DateTime? FromDate;
        public DateTime? ToDate;
        public int? Tenant;

    }
}
