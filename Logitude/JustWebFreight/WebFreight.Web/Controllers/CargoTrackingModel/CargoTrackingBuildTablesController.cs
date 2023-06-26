
using Logitude.BL.CommonDataModel.Helpers;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
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

        public HttpResponseMessage PostCargoTrackingBuilder(Logitude.CargoTracking.BL.CoreBL.Batch.CargoTrackingXMLParameters Args)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                if(Args.Tenant.HasValue)
                    SecurityUtility.AuthenticationOnTenant(Args.Tenant.Value);

                int tenant = authToken.Tenant;
                CreateBatchTaskExecution(Args, BatchTaskNames.BuildCargoTrackingShipments, "Logitude.CargoTracking.BL.CoreBL.Batch.BuildCargoTrackingShipments,Logitude.CargoTracking.BL", tenant);
                return Request.CreateResponse(HttpStatusCode.OK, Args);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

 
        private string CreateBatchTaskExecution(Logitude.CargoTracking.BL.CoreBL.Batch.CargoTrackingXMLParameters Args, string Subject, string ClassName, int tenant)
        {
            // 1- create BTE record
            BatchTaskExecutionPM taskExe;
            AddDateRange(Args);
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(Logitude.CargoTracking.BL.CoreBL.Batch.CargoTrackingXMLParameters));
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

        private void AddDateRange(CargoTrackingXMLParameters Args)
        {
            TenantManagementPM tenantManagementPM = new TenantManagementQuery(Args.Tenant.Value).GetSinglePM(Args.Tenant.Value);
            Args.ToDate = DateTime.Now;

            Args.FromDate = tenantManagementPM.ActivatePrivateSite && tenantManagementPM.PermissionBuildMonths.HasValue ?
                DateTime.Now.AddMonths(Convert.ToInt32(tenantManagementPM.PermissionBuildMonths.Value) * -1) :
                DateTime.Now.AddMonths(-6);
        }
    }




}