using Logitude.BL.CommonDataModel;
using Logitude.BL.CommonDataModel.DataContracts;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.CommonDataModel.Extended
{
    public class CargoTrackingTenantMilestoneDefinitionExtendedController : ApiController
    {
        const string CreatedStatusCode = "C";

        public HttpResponseMessage GetAll()
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                CargoTenantMilestoneDefinitionQuery cargoTrackingMilestoneQuery = new CargoTenantMilestoneDefinitionQuery(authToken.Tenant);
                List<CargoTenantMilestoneDefinitionPM> result = cargoTrackingMilestoneQuery.GetAll(authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage Put(List<CargoTenantMilestoneDefinitionPM> cargoTenantMilestoneDefinitionPMs)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string logKey = PerformanceLogger.LogCurrentTime();
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                    foreach (var entityPM in cargoTenantMilestoneDefinitionPMs)
                    {
                        SecurityUtility.AuthenticationOnEntityTenant("CargoTenantMilestoneDefinition", entityPM.Tenant, authToken.Tenant);
                    }

                    ICommonDataContext commonDataContext = CommonDataContext.GetContext(authToken.Tenant);
                    CargoTenantMilestoneDefinitionService cargoTenantMilestoneDefinitionService = new CargoTenantMilestoneDefinitionService(commonDataContext, authToken.Tenant);
                    cargoTenantMilestoneDefinitionService.UpdateCargoTenantMilestoneDefinitionsPM(cargoTenantMilestoneDefinitionPMs, authToken.Tenant);
                    CreateUpdateShipmentsBatchTask(authToken.Tenant);

                    scope.Complete();
                    PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                    return Request.CreateResponse(HttpStatusCode.OK, cargoTenantMilestoneDefinitionPMs);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private void CreateUpdateShipmentsBatchTask(int tenant)
        {
            UpdateShipmetsBatchArgs args = new UpdateShipmetsBatchArgs() { Tenant = tenant };
            
            string xmlParameters = SerializeParamers(args);

            var updateLastUpdateDateForShipmentsInTenantTask = CreateBatchTaskInstance(args, xmlParameters);

            IInfrastructureContext MyContext = InfrastructureContext.GetContext(tenant);
            BatchTaskExecutionUpdateService bteUpdateService = new BatchTaskExecutionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            bteUpdateService.Update(updateLastUpdateDateForShipmentsInTenantTask, true);
            SendTaskToQueue(tenant, updateLastUpdateDateForShipmentsInTenantTask.Id);
            
        }

        private void SendTaskToQueue(int tenant, string id)
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("batchtaskexecutionqueue", 0);
            queueservice.Send(new Dictionary<string, string>()
                {
                    { "BatchTaskExecutionId", id },
                    { "Tenant", tenant.ToString() }
                }, tenant);
        }

        private BatchTaskExecutionPM CreateBatchTaskInstance(UpdateShipmetsBatchArgs args, string xmlParameters)
        {
            return new BatchTaskExecutionPM()
            {
                Subject = $"Update all shipments and orders in the tenant {args.Tenant} for Cargo Incremental service",
                Tenant = args.Tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ClassName = "Logitude.CargoTracking.BL.CoreBL.Batch.BatchUpdateShipmentsForCargoIncremental,Logitude.CargoTracking.BL",
                CreateDate = DateTime.Now,
                PrametersXml = xmlParameters,
                StatusCode = CreatedStatusCode
            };
        }

        private string SerializeParamers(UpdateShipmetsBatchArgs args)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(typeof(UpdateShipmetsBatchArgs));
            serializer.Serialize(stringwriter, args);
            return stringwriter.ToString();
        }
    }
}