using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityAMs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Security;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.CodePropertiesMapping;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Newtonsoft.Json;
using Logitude.Server.Tools.QueueService;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.App_Code
{
    public class ImporterShipmentsCancleController : ApiController
    {
         
        public HttpResponseMessage Put(ShipmentAM Shipment)
        {
            try
            {
                //var temp = id.Split(',');
                string ShipmentNumber = Shipment.CustomerShipmentNumber;//temp[0];
                int Tenant = Shipment.ImporterTenant;//int.Parse(temp[1]);
                SecurityUtility.AuthenticationOnTenant(Tenant);
                ShipmentQuery shipmentQuery = new ShipmentQuery(Tenant);
                var ImporterShipment = shipmentQuery.GetSingleShipmentPMByNumber(ShipmentNumber, Tenant);

                bool IsNewLog = false;
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(ImporterShipment.Tenant);
                APILogsService apiLogsService = new APILogsService(webFreightContext, ImporterShipment.Tenant);
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(ImporterShipment.Tenant);
                #region APILogs
                var aPILogsRepository = new APILogsRepository(webFreightContext);
                APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, ImporterShipment.Tenant);
                APILogsPM LogPM;
                if (Log == null)
                {
                    IsNewLog = true;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", ImporterShipment.Tenant, true);
                    LogPM = new APILogsPM()
                    {
                        Id = IdCounter.GetNumber("APILogs", ImporterShipment.Tenant),
                        CorrelationId = CorrelationId,
                        CreateDate = DateTime.Now,
                        CreateDateUTC = DateTime.UtcNow,
                        Direction = "I",
                        EntityId = ImporterShipment.Id,
                        LastUpdateDate = DateTime.Now,
                        LastUpdateDateUTC = DateTime.UtcNow,
                        NumberOfRetries = 1,
                        ObjectTableId = Objecttable.Id,
                        ExpirationDate = DateTime.Now.AddDays(90),
                        Refrence = ImporterShipment.ShipmentNumber,
                        Status = "I",
                        Tenant = ImporterShipment.Tenant
                    };
                }
                else
                {
                    IsNewLog = false;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", ImporterShipment.Tenant, true);
                    LogPM = new APILogsPM()
                    {
                        Id = Log.Id,
                        CorrelationId = Log.CorrelationId,
                        CreateDate = Log.CreateDate,
                        CreateDateUTC = Log.CreateDateUTC,
                        Direction = Log.Direction,
                        EntityId = Log.EntityId,
                        LastUpdateDate = Log.LastUpdateDate,
                        LastUpdateDateUTC = Log.LastUpdateDateUTC,
                        NumberOfRetries = Log.NumberOfRetries++,
                        ObjectTableId = Log.ObjectTableId,
                        ExpirationDate = Log.ExpirationDate,
                        Refrence = Log.Refrence,
                        Status = "I",
                        Tenant = Log.Tenant,

                    };
                }
                #endregion
                LogPM.Subject = "Cancle Shipment in Importer Tenant";
                if (IsNewLog)
                {
                    apiLogsService.Create(LogPM);
                }
                try
                {
                    var msg = "Start cancelling Shipment At Importer Tenant " + DateTime.Now;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, LogPM.Status, 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(ImporterShipment), null, null, "");

                    if (ImporterShipment != null)
                    {
                        IShipmentsContext objectContext = ShipmentsContext.GetContext(Tenant);
                        string systemEmail = "system@tenant" + Tenant + ".com";
                        ImporterShipment.DontAddToImportersQueue = true;
                        ImporterShipment.IsCancelled = true;
                        ImporterShipment.ForwarderShipmentNumber = null;
                        ImporterShipment.ComputedForwarderShipmentNumber = ImporterShipment.Id;
                        ShipmentService shipmentService = new ShipmentService(objectContext, ImporterShipment, systemEmail);
                        shipmentService.Update();
                        HandelCancelledShipmentDocuments(ImporterShipment.Id, ImporterShipment.Tenant);
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "Ok");
                }
                catch (Exception ex)
                {
                    var apiException = new APIException()
                    {
                        ErrorType = ex.GetType().Name,
                        ErrorMessage = ex.Message
                    };
                    string errorMessage = ex.Message + Environment.NewLine;

                    if (ex.InnerException != null)
                    {

                        errorMessage = errorMessage + " (" + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) + ")" + Environment.NewLine;

                    }

                    errorMessage = errorMessage + ex.StackTrace + Environment.NewLine;
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Cancle Shipment At Importer Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

                    return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }    
           
        }
         

        private void HandelCancelledShipmentDocuments(string EntityId, int Tenant)
        {
           
            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(Tenant);
            var DocumentFilingPMs = documentsFilingQuery.GetDocumentsFilingPMsByEntityId(EntityId, "I", Tenant);
            ICommonDataContext objectContext = CommonDataContext.GetContext(Tenant); 
            DocumentsFilingService documentsFilingService = new DocumentsFilingService(objectContext, Tenant);
            foreach (var item in DocumentFilingPMs)
            {
                item.DontAddToQueue = true;
                item.IsDeleted = true;
                //item.ForwarderDocumentId = null;
                documentsFilingService.Update(item, null);
            }
        }
         
    }
}