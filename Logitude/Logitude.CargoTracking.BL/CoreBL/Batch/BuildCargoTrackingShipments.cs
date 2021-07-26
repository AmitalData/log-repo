  
 
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
            TenantRepository tenantRepository = new TenantRepository(0);
            CargoTrackingArguments = GetCargoTrackingArgs();
            try
            {
                UpdateIsIncrementalRunning(tenantRepository,true);
                ServiceHelper.CheckAndUpdateWaterMark(destinationConnectionString,sourceConnectionString);
                AddAllTablesToThread(CargoTrackingTableList.GetCargoTrackingTableList());
                UpdateIsIncrementalRunning(tenantRepository,false);
            }

            catch (Exception e)
            {
                UpdateIsIncrementalRunning(tenantRepository,false);
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
            string[] GlobalConnectionArray = dbConnectionFrom.Split(',');
            string[] CargoTrackingConnectionArray = dbConnectionTo.Split(',');
            string[] MainConnectionArray = GetMainDBConnectionString(ServiceHelper.BuildConnectionString(ServiceHelper.GetConnectionStringArguments(GlobalConnectionArray))).Split(',');
            ConnectionStringArguments sourceConnectionStringArguments = ServiceHelper.GetConnectionStringArguments(MainConnectionArray);
            ConnectionStringArguments destinationConnectionStringArguments = ServiceHelper.GetConnectionStringArguments(CargoTrackingConnectionArray);
            sourceConnectionString = ServiceHelper.BuildConnectionString(sourceConnectionStringArguments);
            destinationConnectionString = ServiceHelper.BuildConnectionString(destinationConnectionStringArguments);
        }
 

        public string GetMainDBConnectionString(string connectionString)
        {
            string result = null;
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("select DBConnection,SecondaryAzureDBConnection from dbo.GlobalDBs where Id =0;", connection);
            try
            {
                connection.Open();
                result=ExecuteGetMainDBConnectionStringCommand(command);
            }
            catch (Exception ex)
            {
                connection.Close();
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        private string ExecuteGetMainDBConnectionStringCommand(SqlCommand command)
        {
            string result = null;
            using (SqlDataReader reader = command.ExecuteReader())
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
