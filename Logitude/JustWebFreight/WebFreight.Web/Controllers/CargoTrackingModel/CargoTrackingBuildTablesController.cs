
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using Logitude.CargoTracking.BL.CoreBL.Batch;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.AccountingModel
{


    public class CargoTrackingBuildTablesController : ApiController
    {
        CargoTrackingMainService cargoTrackingMainService = new CargoTrackingMainService(); //Test
        string sourceConnectionString = string.Empty;//Test
        string destinationConnectionString = string.Empty;//Test
        public HttpResponseMessage PostCargoTrackingBuilder(CargoTrackingArgs Args)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                int tenant = authToken.Tenant;
                //BuildConnectionString();
                //TestCargoTrackingBatch(Args, tenant);
                CreateBatchTaskExecution(Args, "Build Cargo Tracking Shipments", "Logitude.CargoTracking.BL.CoreBL.Batch.BuildCargoTrackingShipments,Logitude.Accounting.BL", tenant);
                return Request.CreateResponse(HttpStatusCode.OK, Args);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public void TestCargoTrackingBatch(CargoTrackingArgs CargoTrackingArguments,int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            UpdateIsIncrementalRunning(tenantRepository, true);
            cargoTrackingMainService.CheckAndUpdateWaterMark(destinationConnectionString, sourceConnectionString);
            AddAllTablesToThread(cargoTrackingMainService.FillCargoTableList(), CargoTrackingArguments);
            UpdateIsIncrementalRunning(tenantRepository, false);
        }

        private string CreateBatchTaskExecution(CargoTrackingArgs Args, string Subject, string ClassName, int tenant)
        {
            // 1- create BTE record
            BatchTaskExecutionPM taskExe;

            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(CargoTrackingArgs));
            serializer.Serialize(stringwriter, Args);
            string xmlParameters = stringwriter.ToString();


            taskExe = new BatchTaskExecutionPM()
            {
                Subject = Subject,
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = ClassName,
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = "C",

            };


            IInfrastructureContext MyContext = InfrastructureContext.GetContext(tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            bteUpdateService.Update(taskExe, true);

            // 2- Send to queue
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", taskExe.Id },
                    { "Tenant",  tenant.ToString() }
                }, tenant);


            return taskExe.Id;
        }

        //Test  ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void UpdateIsIncrementalRunning(TenantRepository tenantRepository, bool IsRunning)
        {
            Tenant tenant = tenantRepository.GetSingleTenant(0);
            tenant.IsIncrementalBuildRunning = IsRunning;
            tenantRepository.Update(tenant);
            tenantRepository.SubmitChanges();
        }

 
        private void UpdateCargoDataBase(CargoTable table, CargoTrackingArgs Args)
        {
            CargoTrackingArguments CargoTrackingArgs = new CargoTrackingArguments
            {
                FromDate = Args.FromDate,
                Tenant = Args.Tenant,
                ToDate = Args.ToDate,
            };
            cargoTrackingMainService.UpdateCTDataBase(new CargoArgs() { Table = table, SourceConnectionString = sourceConnectionString, DestinationConnectionString = destinationConnectionString }, 1000, null, CargoTrackingArgs);
        }


        private void AddAllTablesToThread(List<CargoTable> CargoTableLists, CargoTrackingArgs CargoTrackingArguments)
        {
            foreach (CargoTable table in CargoTableLists)
            {

                UpdateCargoDataBase(table, CargoTrackingArguments);

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

        private string GetUpdateDataBaseCondition(CargoArgs buildCargoArgs)
        {

            string condition = " where AutomaticLastUpdateDate > ( select LastUpdateDate from WaterMarks where TableName = " + "'" + buildCargoArgs.Table.CT_TableName + "')";
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




}