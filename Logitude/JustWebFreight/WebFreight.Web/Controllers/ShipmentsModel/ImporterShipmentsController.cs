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
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using System.Transactions;
using Simplog.Data.Helpers;

namespace WebFreight.Web.Controllers.ShipmentsModel
{
    public class ImporterShipmentsController : ApiController
    {

        public bool GetIfShipmentExists(int importertenant, int Tenant, string shipmentnumber)// NotFinished
        {
            //SecurityUtility.AuthenticationOnTenant(importertenant);
            //SecurityUtility.CheckContactFeature("Shipment", "READ", importertenant);
            ShipmentQuery shipmentQuery = new ShipmentQuery(importertenant);
            var Temp = shipmentQuery.GetSingleShipmentPMByForwarderNumber(shipmentnumber, importertenant, Tenant);
            if (Temp != null)//&& !Temp.IsCancelled
            {
                return true;
            }
            else
            {
                return false;
            }
            //  if (Temp.CustomerShipmentNumber != null)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        if (Temp.ForwarderShipmentNumber != null)
            //        {
            //            shipmentQuery = new ShipmentQuery(importertenant);
            //            var Ship = shipmentQuery.GetSingleShipmentPMByForwarderNumber(Temp.ForwarderShipmentNumber, importertenant);
            //            if (Ship != null)
            //            {
            //                return true;
            //            }
            //        }
            //        return false;
            //    }
            //}
            //else
            //{
            //    return false;
            //}

        }


        public HttpResponseMessage Post(ShipmentAM Shipment)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(Shipment.ImporterTenant);
                //SecurityUtility.CheckContactFeature("Shipment", "NEW", Shipment.ImporterTenant);
                ShipmentQuery shipmentQuery = new ShipmentQuery(Shipment.ImporterTenant);

                bool IsNewLog = false;
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(Shipment.ImporterTenant);
                APILogsService apiLogsService = new APILogsService(webFreightContext, Shipment.ImporterTenant);
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(Shipment.ImporterTenant);
                HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(Shipment.ImporterTenant);
                HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(hybridPartnerRepository);
                HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePMByPartnerTenant(Shipment.Tenant);

                var aPILogsRepository = new APILogsRepository(webFreightContext);
                APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, Shipment.ImporterTenant);
                APILogsPM LogPM;
                if (Log == null)
                {
                    IsNewLog = true;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", Shipment.ImporterTenant, true);
                    LogPM = new APILogsPM()
                    {
                        Id = IdCounter.GetNumber("APILogs", Shipment.ImporterTenant),
                        CorrelationId = CorrelationId,
                        CreateDate = DateTime.Now,
                        CreateDateUTC = DateTime.UtcNow,
                        Direction = "I",
                        EntityId = Shipment.Id,
                        LastUpdateDate = DateTime.Now,
                        LastUpdateDateUTC = DateTime.UtcNow,
                        NumberOfRetries = 1,
                        ObjectTableId = Objecttable.Id,
                        ExpirationDate = DateTime.Now.AddDays(90),
                        Refrence = Shipment.ForwarderShipmentNumber,
                        Status = "I",
                        Tenant = Shipment.ImporterTenant
                    };
                }
                else
                {
                    IsNewLog = false;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", Shipment.ImporterTenant, true);
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
                if (Partner != null)
                {
                    LogPM.PartnerName = Partner.Name + "(" + Partner.Id + ")";
                }
                LogPM.Subject = "Inserting Shipment To Importer Tenant";
                if (IsNewLog)
                {
                    apiLogsService.Create(LogPM);
                }
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start Inserting Shipment To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(Shipment), null, null, "");
                var IsNew = false;
                try
                {
                    var msg = "Start Inserting Shipment To Importer Tenant " + DateTime.Now;
                    //APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, LogPM.Status, 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(Shipment), null, null, "");
                    // Insert Importer Shipment
                    var ImporterShipment = new ShipmentPM() { NewConcurrencyGUID = Guid.NewGuid().ToString() };//shipmentQuery.GetSinglePMWithoutComposition(Shipment.Id, Shipment.Tenant);
                    if (!string.IsNullOrEmpty(Shipment.CustomerShipmentNumber))
                    {
                        ImporterShipment = shipmentQuery.GetSingleShipmentPMByNumber(Shipment.CustomerShipmentNumber, Shipment.ImporterTenant);
                    }
                    if (ImporterShipment == null)
                    {
                        ImporterShipment = new ShipmentPM() { NewConcurrencyGUID = Guid.NewGuid().ToString() };
                        IsNew = true;
                    }
                    APIException Result = MapEntityAMToEntityPM(Shipment, ImporterShipment);

                    if (Result == null)
                    {
                        IShipmentsContext objectContext = ShipmentsContext.GetContext(ImporterShipment.Tenant);
                        if (!IsNew)
                        {
                            ShipmentPackageQuery shipPackageQuery = new ShipmentPackageQuery(new ShipmentPackageRepository(objectContext));
                            List<ShipmentPackagePM> shipmentPackages = shipPackageQuery.GetShipmentPackages(ImporterShipment.Id, ImporterShipment.ShipmentNumber, ImporterShipment.Tenant);
                            foreach (ShipmentPackagePM package in shipmentPackages)
                            {
                                package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                                ImporterShipment.ShipmentPackages.Add(package);
                            }
                        }
                        string systemEmail = "system@tenant" + ImporterShipment.Tenant + ".com";
                        ShipmentService shipmentService = new ShipmentService(objectContext, ImporterShipment, systemEmail);
                        ImporterShipment.DontAddToImportersQueue = true;
                        shipmentService.SetChangeSet(ImporterShipment.ShipmentPackages, new List<ShipmentOrderPackagePM>(), new List<ShipmentPickUpPM>(), new List<ShipmentDeliveryPM>(), new List<ShipmentReceivablePM>(), new List<ShipmentPayablePM>(), new List<ShipmentFollowUpPM>(), new List<ShipmentAWBPrintOnlyPM>(), new List<ConsoleShipmentPM>(), new List<ShipmentCarrierStatusPM>(), new List<AWBOCIPM>(), new List<ShipmentCommodityPM>(), new List<ShipmentAssemblyPM>(), new List<ShipmentStoragePricingPM>(), new List<ShipmentProductItemPM>());
                        if (!string.IsNullOrEmpty(ImporterShipment.CustomerShipmentNumber))
                        {
                            shipmentService.Update();
                            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(ImporterShipment.Tenant);
                            var DocumentFilingPMsIds = documentsFilingQuery.GetSharedWithAgentDocumentsFilingPMsIdsByEntityId(ImporterShipment.Id, "I", ImporterShipment.Tenant);
                            if (DocumentFilingPMsIds != null && DocumentFilingPMsIds.Count > 0)
                            {
                                foreach (var DocumentFilingPMId in DocumentFilingPMsIds)
                                {
                                    DocumentsFilingPM DocumentFilingPM = documentsFilingQuery.GetSinglePM(DocumentFilingPMId, ImporterShipment.Tenant);
                                    IQueueService queueservice = new DbQueueService();
                                    queueservice.InitializeQueue("ForwardersShipmentDocumentsQueue", 0);
                                    queueservice.Send(new Dictionary<string, string>() { { "ShipmentId", ImporterShipment.Id }, { "DocumentFilingId", DocumentFilingPMId }, { "Tenant", ImporterShipment.Tenant.ToString() } }, ImporterShipment.Tenant);
                                }
                            }
                        }
                        else
                        {
                            shipmentService.Create();
                        }

                        var ResponseData = JsonConvert.SerializeObject(ImporterShipment.Id);
                        var Donemsg = "Shipment Added To Importer Tenant Successfully " + DateTime.Now;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, ImporterShipment.Id, null, "");
                        return Request.CreateResponse(HttpStatusCode.OK, new List<string>() { ImporterShipment.Id, ImporterShipment.ShipmentNumber });
                    }
                    else
                    {

                        var Failmsg = "Inserting Shipment Faild " + DateTime.Now;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(Result), null, "");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, Result);
                    }
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
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Update Shipment At Importer Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

                    return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage Put(ShipmentAM Shipment)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(Shipment.ImporterTenant);
                //SecurityUtility.CheckContactFeature("Shipment", "UPDATE", Shipment.ImporterTenant);
                ShipmentQuery shipmentQuery = new ShipmentQuery(Shipment.ImporterTenant);

                bool IsNewLog = false;
                string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                IWebFreightContext webFreightContext = WebFreightContext.GetContext(Shipment.ImporterTenant);
                APILogsService apiLogsService = new APILogsService(webFreightContext, Shipment.ImporterTenant);
                ObjectTableRepository objectTabelRepository = new ObjectTableRepository(Shipment.ImporterTenant);
                HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(Shipment.ImporterTenant);
                HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(hybridPartnerRepository);
                HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePMByPartnerTenant(Shipment.Tenant);

                var aPILogsRepository = new APILogsRepository(webFreightContext);
                APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, Shipment.ImporterTenant);
                APILogsPM LogPM;
                if (Log == null)
                {
                    IsNewLog = true;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", Shipment.ImporterTenant, true);
                    LogPM = new APILogsPM()
                    {
                        Id = IdCounter.GetNumber("APILogs", Shipment.ImporterTenant),
                        CorrelationId = CorrelationId,
                        CreateDate = DateTime.Now,
                        CreateDateUTC = DateTime.UtcNow,
                        Direction = "I",
                        EntityId = Shipment.Id,
                        LastUpdateDate = DateTime.Now,
                        LastUpdateDateUTC = DateTime.UtcNow,
                        NumberOfRetries = 1,
                        ObjectTableId = Objecttable.Id,
                        ExpirationDate = DateTime.Now.AddDays(90),
                        Refrence = Shipment.ForwarderShipmentNumber,
                        Status = "I",
                        Tenant = Shipment.ImporterTenant
                    };
                }
                else
                {
                    IsNewLog = false;
                    var Objecttable = objectTabelRepository.GetObjectTableByName("Shipment", Shipment.ImporterTenant, true);
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
                if (Partner != null)
                {
                    LogPM.PartnerName = Partner.Name;
                }
                LogPM.Subject = "Update Shipment To Importer Tenant";
                if (IsNewLog)
                {
                    apiLogsService.Create(LogPM);
                }
                APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "I", 1, DateTime.Now, DateTime.UtcNow, "Start updating Shipment To Importer Tenant " + DateTime.Now, LogitudeXmlSerializer.SerializeObjectToXmlString(Shipment), null, null, "");

                try
                {
                    var msg = "Start updating Shipment At Importer Tenant " + DateTime.Now;
                    //APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, LogPM.Status, 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(Shipment), null, null, "");
                    ShipmentPM ImporterShipment = null;
                    var IsNew = false;
                    if (!string.IsNullOrEmpty(Shipment.CustomerShipmentNumber))
                    {
                        ImporterShipment = shipmentQuery.GetSingleShipmentPMByNumber(Shipment.CustomerShipmentNumber, Shipment.ImporterTenant);
                        if (ImporterShipment == null)
                        {
                            ImporterShipment = shipmentQuery.GetSingleShipmentPMByForwarderNumber(Shipment.ForwarderShipmentNumber, Shipment.ImporterTenant, Shipment.Tenant);
                        }
                    }
                    else
                    {
                        ImporterShipment = shipmentQuery.GetSingleShipmentPMByForwarderNumber(Shipment.ForwarderShipmentNumber, Shipment.ImporterTenant, Shipment.Tenant);
                    }

                    if (ImporterShipment == null)
                    {
                        ImporterShipment = new ShipmentPM() { NewConcurrencyGUID = Guid.NewGuid().ToString() };
                        IsNew = true;
                    }
                    APIException Result = MapEntityAMToEntityPM(Shipment, ImporterShipment);

                    if (Result == null)
                    {
                        IShipmentsContext objectContext = ShipmentsContext.GetContext(Shipment.ImporterTenant);
                        if (!IsNew)
                        {
                            ShipmentPackageQuery shipPackageQuery = new ShipmentPackageQuery(new ShipmentPackageRepository(objectContext));
                            List<ShipmentPackagePM> shipmentPackages = shipPackageQuery.GetShipmentPackages(ImporterShipment.Id, ImporterShipment.ShipmentNumber, ImporterShipment.Tenant);
                            foreach (ShipmentPackagePM package in shipmentPackages)
                            {
                                package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                                ImporterShipment.ShipmentPackages.Add(package);
                            }
                        }

                        string systemEmail = "system@tenant" + Shipment.ImporterTenant + ".com";
                        ShipmentService shipmentService = new ShipmentService(objectContext, ImporterShipment, systemEmail);
                        ImporterShipment.DontAddToImportersQueue = true;
                        shipmentService.SetChangeSet(ImporterShipment.ShipmentPackages, new List<ShipmentOrderPackagePM>(), new List<ShipmentPickUpPM>(), new List<ShipmentDeliveryPM>(), new List<ShipmentReceivablePM>(), new List<ShipmentPayablePM>(), new List<ShipmentFollowUpPM>(), new List<ShipmentAWBPrintOnlyPM>(), new List<ConsoleShipmentPM>(), new List<ShipmentCarrierStatusPM>(), new List<AWBOCIPM>(), new List<ShipmentCommodityPM>(), new List<ShipmentAssemblyPM>(), new List<ShipmentStoragePricingPM>(), new List<ShipmentProductItemPM>());
                        shipmentService.Update();
                        var Donemsg = "Shipment Updated Successfully " + DateTime.Now;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, Donemsg, null, ImporterShipment.Id, null, "");
                        return Request.CreateResponse(HttpStatusCode.OK, new List<string>() { ImporterShipment.Id, ImporterShipment.ShipmentNumber });
                    }
                    else
                    {
                        var Failmsg = "Updating Shipment Faild " + DateTime.Now;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, Failmsg, null, LogitudeXmlSerializer.SerializeObjectToXmlString(Result), null, "");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, Result);
                    }
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
                    APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "F", 1, DateTime.Now, DateTime.UtcNow, "Update Shipment At Importer Tenant Faild " + DateTime.Now, null, null, errorMessage, (errorMessage.Length >= 250 ? errorMessage.Substring(0, 249) : errorMessage));

                    return Request.CreateResponse(HttpStatusCode.BadRequest, apiException);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }


        public HttpResponseMessage Delete(string id)
        {
            try
            {
                var temp = id.Split(',');
                string ShipmentNumber = temp[0];
                int Tenant = int.Parse(temp[1]);
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
                documentsFilingService.Update(item, null);
            }
        }


        private APIException MapEntityAMToEntityPM(ShipmentAM entityAM, ShipmentPM entityPM)
        {
            APIException Responce = new APIException();
            //TenantQuery myTenantQuery = new TenantQuery();
            TenantPM currentTenant = TenantQuery.GetSingleTenantPM(entityAM.ImporterTenant, false);
            ICommonDataContext commoncontext = CommonDataContext.GetContext(entityAM.ImporterTenant);
            HybridPartnerRepository hybridPartnerRepository = new HybridPartnerRepository(commoncontext);
            HybridPartnerQuery HybridPartnerQuerey = new HybridPartnerQuery(hybridPartnerRepository);
            HybridPartnerPM Partner = HybridPartnerQuerey.GetSinglePMByPartnerTenant(entityAM.Tenant);
            TenantManagmentPrivateLabelsPM privatelabel = null;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagmentPrivateLabelsQuery query = new TenantManagmentPrivateLabelsQuery(0);
                privatelabel = query.GetSinglePM(currentTenant.PrivateLabelId);

            }


            entityPM.Tenant = entityAM.ImporterTenant;
            entityPM.GrossWeightUnitCode = "KG";
            entityPM.DimensionsUnitCode = "Cm";
            entityPM.ChargeableWeightUnitCode = "KG";
            entityPM.VolumeUnitCode = "CBF";
            if (!string.IsNullOrEmpty(entityAM.ForwardingPartnerTenant))
            {
                int ForwardingPartnerTenant = int.Parse(entityAM.ForwardingPartnerTenant);

                HybridPartnerPM ForwardingPartner = HybridPartnerQuerey.GetSinglePMByPartnerTenant(ForwardingPartnerTenant);
                if (ForwardingPartner != null)
                {
                    entityPM.ForwardingPartnerId = ForwardingPartner.Id;
                }
            }
            if (!string.IsNullOrEmpty(currentTenant.CustomerId))
            {
                entityPM.ConsigneeId = currentTenant.CustomerId;
                entityPM.CustomerId = currentTenant.CustomerId;
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "Tenant CustomerId field is required.";
                return Responce;
            }
            if (entityAM.ForwarderShipmentNumber != null)
            {

                entityPM.ForwarderShipmentNumber = entityAM.ForwarderShipmentNumber;

            }
            if (entityAM.CustomerShipmentNumber != null)
            {

                entityPM.CustomerShipmentNumber = entityAM.CustomerShipmentNumber;

            }

            //else
            //{
            //    Responce.ErrorType = "Validation Error";
            //    Responce.ErrorMessage = "ForwarderShipmentNumber field is required.";
            //    return Responce;
            //}
            if (entityAM.Incoterm != null)
            {
                var IncotermId = IncotermCodePropertiesMapping.GetIncotermIdFromIncotermProperties(entityAM.ImporterTenant, entityAM.Incoterm);
                if (!string.IsNullOrEmpty(IncotermId))
                {
                    entityPM.IncotermId = IncotermId;
                }
                else
                {
                    Responce.ErrorType = "Validation Error";
                    Responce.ErrorMessage = "IncotermId field doesn't exist in the database, insert this entity before using it.";
                    return Responce;
                }

            }
            if (entityAM.FromPort != null)
            {
                var FromportId = PortCodePropertiesMapping.GetPortIdFromPortProperties(entityAM.ImporterTenant, entityAM.FromPort);
                if (!string.IsNullOrEmpty(FromportId))
                {
                    entityPM.FromPortId = FromportId;
                    entityPM.MainCarriageFromPortId = FromportId;
                }
                else
                {
                    Responce.ErrorType = "Validation Error";
                    Responce.ErrorMessage = "FromPortId field doesn't exist in the database, insert this entity before using it.";
                    return Responce;
                }

            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "FromPortId field is required.";
                return Responce;
            }

            if (!string.IsNullOrEmpty(entityAM.FreightPrepaidCollectId))
            {
                entityPM.FreightPrepaidCollectId = entityAM.FreightPrepaidCollectId;
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "FreightPrepaidCollectId field is required.";
                return Responce;
            }

            if (!string.IsNullOrEmpty(entityAM.OtherPrepaidCollectId))
            {
                entityPM.OtherPrepaidCollectId = entityAM.OtherPrepaidCollectId;
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "OtherPrepaidCollectId field is required.";
                return Responce;
            }


            if (entityAM.ToPort != null)
            {
                var ToportId = PortCodePropertiesMapping.GetPortIdFromPortProperties(entityAM.ImporterTenant, entityAM.ToPort);
                if (!string.IsNullOrEmpty(ToportId))
                {
                    entityPM.ToPortId = ToportId;
                    entityPM.MainCarriageToPortId = ToportId;
                }
                else
                {
                    Responce.ErrorType = "Validation Error";
                    Responce.ErrorMessage = "ToportId field doesn't exist in the database, insert this entity before using it.";
                    return Responce;
                }
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "ToportId field is required.";
                return Responce;
            }
            if (!string.IsNullOrEmpty(entityAM.TransportModeId))
            {
                entityPM.TransportModeId = entityAM.TransportModeId;
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "TransportModeId field is required.";
                return Responce;
            }
            if (!string.IsNullOrEmpty(entityAM.DirectionId))
            {
                entityPM.DirectionId = entityAM.DirectionId;
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "DirectionId field is required.";
                return Responce;
            }
            if (!string.IsNullOrEmpty(entityAM.ShipmentLevelCode))
            {
                entityPM.ShipmentLevelCode = entityAM.ShipmentLevelCode;
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "ShipmentLevelCode field is required.";
                return Responce;
            }
            //if (entityAM.Branch != null)
            //{
            //    var BranchId = BranchCodePropertiesMapping.GetBranchIdFromBranchProperties(entityAM.ImporterTenant, entityAM.Branch);
            //    if (!string.IsNullOrEmpty(BranchId))
            //    {
            //        entityPM.BranchId = BranchId;
            //    }
            //    else
            //    {
            //        Responce.ErrorType = "Validation Error";
            //        Responce.ErrorMessage = "BranchId field doesn't exist in the database, insert this entity before using it.";
            //        return Responce;
            //    }

            //}
            //else
            //{
            //    Responce.ErrorType = "Validation Error";
            //    Responce.ErrorMessage = "BranchId field is required.";
            //    return Responce;
            //}
            //if (entityAM.Department != null)
            //{
            //    var DepartmentId = DepartmentCodePropertiesMapping.GetDepartmenthIdFromDepartmentProperties(entityAM.ImporterTenant, entityAM.Department);
            //    if (!string.IsNullOrEmpty(DepartmentId))
            //    {
            //        entityPM.DepartmentId = DepartmentId;
            //    }
            //    else
            //    {
            //        Responce.ErrorType = "Validation Error";
            //        Responce.ErrorMessage = "DepartmentId field doesn't exist in the database, insert this entity before using it.";
            //        return Responce;
            //    }

            //}
            //else
            //{
            //    Responce.ErrorType = "Validation Error";
            //    Responce.ErrorMessage = "DepartmentId field is required.";
            //    return Responce;
            //}
            if (entityAM.Branch != null)
            {
                BranchRepository Repo = new BranchRepository(entityAM.ImporterTenant);
                var Branch = Repo.GetBranches(entityAM.ImporterTenant).FirstOrDefault(); //BranchCodePropertiesMapping.GetBranchIdFromBranchProperties(entityAM.ImporterTenant, entityAM.Branch);
                if (Branch != null)
                {
                    entityPM.BranchId = Branch.Id;
                }
                else
                {
                    Responce.ErrorType = "Validation Error";
                    Responce.ErrorMessage = "BranchId field doesn't exist in the database, insert this entity before using it.";
                    return Responce;
                }

            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "BranchId field is required.";
                return Responce;
            }
            if (entityAM.Department != null)
            {
                DepartmentRepository Repo = new DepartmentRepository(entityAM.ImporterTenant);
                var Department = Repo.GetDepartments(entityAM.ImporterTenant).FirstOrDefault(); //BranchCodePropertiesMapping.GetBranchIdFromBranchProperties(entityAM.ImporterTenant, entityAM.Branch);
                if (Department != null)
                {
                    entityPM.DepartmentId = Department.Id;
                }
                //var DepartmentId = DepartmentCodePropertiesMapping.GetDepartmenthIdFromDepartmentProperties(entityAM.ImporterTenant, entityAM.Department);
                //if (!string.IsNullOrEmpty(DepartmentId))
                //{
                //    entityPM.DepartmentId = DepartmentId;
                //}
                else
                {
                    Responce.ErrorType = "Validation Error";
                    Responce.ErrorMessage = "DepartmentId field doesn't exist in the database, insert this entity before using it.";
                    return Responce;
                }

            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "DepartmentId field is required.";
                return Responce;
            }

            if (!string.IsNullOrEmpty(entityAM.Shipper.Code) || !string.IsNullOrEmpty(entityAM.Shipper.Id) || !string.IsNullOrEmpty(entityAM.Shipper.ExternalCode))
            {
                var ShipperId = CardCodePropertiesMapping.GetCardIdFromCardProperties(entityAM.ImporterTenant, entityAM.Shipper);
                if (!string.IsNullOrEmpty(ShipperId))
                {
                    entityPM.ShipperId = ShipperId;
                }
            }
            //    else
            //    {
            //        Responce.ErrorType = "Validation Error";
            //        Responce.ErrorMessage = "ShipperId field doesn't exist in the database, insert this entity before using it.";
            //        return Responce;
            //    }

            //}
            //else
            //{
            //    if (entityAM.DirectionId != "I")
            //    {
            //        Responce.ErrorType = "Validation Error";
            //        Responce.ErrorMessage = "ShipperId field is required.";
            //        return Responce;
            //    }
            //}


            //if (!string.IsNullOrEmpty(entityAM.ConsigneeProperties.Code) || !string.IsNullOrEmpty(entityAM.ConsigneeProperties.Id) || !string.IsNullOrEmpty(entityAM.ConsigneeProperties.ExternalCode))
            //{
            //    var ConsigneeId = CardCodePropertiesMapping.GetCardIdFromCardProperties(entityAM.ImporterTenant, entityAM.ConsigneeProperties);
            //    if (!string.IsNullOrEmpty(ConsigneeId))
            //    {
            //        entityPM.ConsigneeId = ConsigneeId;
            //    }
            //    else
            //    {
            //        Responce.ErrorType = "Validation Error";
            //        Responce.ErrorMessage = "ConsigneeId field doesn't exist in the database, insert this entity before using it.";
            //        return Responce;
            //    }
            //}
            //else
            //{
            //    if (entityAM.DirectionId != "E")
            //    {
            //        Responce.ErrorType = "Validation Error";
            //        Responce.ErrorMessage = "ConsigneeId field is required.";
            //        return Responce;
            //    }
            //}

            entityPM.PrivateLabelMainCarriageETA = entityAM.MainCarriageETA;
            entityPM.PrivateLabelMainCarriageETD = entityAM.MainCarriageETD;
            entityPM.PrivateLabelMainCarriageATA = entityAM.MainCarriageATA;
            entityPM.PrivateLabelMainCarriageATD = entityAM.MainCarriageATD; 
            entityPM.MainCarriageETA = entityAM.MainCarriageETA;
            entityPM.MainCarriageATA = entityAM.MainCarriageATA;
            entityPM.MainCarriageATD = entityAM.MainCarriageATD;
            entityPM.MainCarriageETD = entityAM.MainCarriageETD;
            entityPM.OnCarriageATA = entityAM.OnCarriageATA;
            entityPM.OnCarriageATD = entityAM.OnCarriageATD;
            entityPM.PreCarriageATA = entityAM.PreCarriageATA;
            entityPM.PreCarriageATD = entityAM.PreCarriageATD;

            entityPM.DimensionsUnitCode = entityAM.DimensionsUnitCode;
            entityPM.GrossWeightUnitCode = entityAM.GrossWeightUnitCode;
            entityPM.ChargeableWeightUnitCode = entityAM.ChargeableWeightUnitCode;
            entityPM.VolumeUnitCode = entityAM.VolumeUnitCode;

            if (!string.IsNullOrEmpty(entityAM.DeclarationXMLData))
            {
                entityPM.DeclarationXMLData = entityAM.DeclarationXMLData;
            }
            if (!string.IsNullOrEmpty(entityAM.VersionApproved))
            {
                entityPM.VersionApproved = entityAM.VersionApproved;
            }
            if (entityAM.ApproveDateTime != null)
            {
                entityPM.ApproveDateTime = entityAM.ApproveDateTime;
            }
            if (entityAM.IsOperationalClosed == false && entityAM.CustomsClearanceDate != null && entityAM.StatusCode.ToLower() == "ccd")
            {
                entityPM.IsShipmentComputedFieldChange = true;
                entityPM.IsRequestedDocuments = false;
                entityPM.RequestedDocumentsCount = 0;
                entityPM.MissingDocumentsCount = 0;
                entityPM.IsMissingDocument = false;


            }

            entityPM.IsImporterApprovalRequired = entityAM.IsImporterApprovalRequired;

            if (currentTenant.AutoArchiveOnInvoice == true && entityAM.OriginalStatusCode == "INPR" && entityAM.CustomsClearanceDate != null && entityPM.IsOperationalClosed == false)
            {
                entityPM.IsOperationalClosed = true;
            }
            if (entityPM.CustomsClearanceDate == null)
            {
                entityPM.ExceptionDate = entityAM.ExceptionDate;
                entityPM.ExceptionDescription = entityAM.ExceptionDescription;
                entityPM.HasException = entityAM.HasException;
            }
            if (entityPM.CustomsClearanceDate == null && entityAM.CustomsClearanceDate != null && entityAM.HasException == true)
            {

                entityPM.HasException = false;
                entityPM.ExceptionDate = null;
                entityPM.ExceptionDescription = null;
                entityPM.ExceptionResolvedDescription = "Customs Clearance";

            }
            if (entityAM.CustomsClearanceDate != null && entityAM.IsImporterApprovalRequired && entityPM.ApproveDateTime == null && string.IsNullOrEmpty(entityPM.ApprovedBy))
            {
                entityPM.ApprovedBy = "System";
                entityPM.ApproveDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.VersionApproved = entityAM.VersionApproved;
            }

            entityPM.CustomsClearanceDate = entityAM.CustomsClearanceDate;
            if (Partner != null)
            {
                entityPM.ForwarderPartnerId = Partner.Id;
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "ForwarderPartnerId field is required.";
                return Responce;
            }
            //entityPM.IsOperationalClosed = entityAM.IsOperationalClosed;
            if (entityAM.StatusDate != null || entityAM.OriginalStatusDate != null)
            {
                //EntityStatus tempstatus = null;
                //if (!string.IsNullOrEmpty(entityPM.StatusId))
                //{
                //    tempstatus = EntityStatusRepository.GetSingleEntityStatus(entityPM.StatusId, entityPM.Tenant, true);
                //    if (tempstatus != null && tempstatus.Code.ToLower() != "ccd" && (entityAM.StatusCode.ToLower() == "opop" || entityAM.StatusCode.ToLower() == "ccd"))
                //    {
                if ((Partner != null && Partner.ReceiveAllStatuses == true) || (privatelabel != null && privatelabel.ReceiveAllStatuses == true))
                {
                    entityPM.StatusDate = entityAM.OriginalStatusDate;
                }
                else
                {
                    entityPM.StatusDate = entityAM.StatusDate;
                }

                //    }
                //}
                //else if (entityAM.StatusCode.ToLower() == "opop" || entityAM.StatusCode.ToLower() == "ccd")
                //{
                //    entityPM.StatusDate = entityAM.StatusDate;
                //}
            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "StatusDate field is required.";
                return Responce;
            }

            if (!string.IsNullOrEmpty(entityAM.StatusCode) || !string.IsNullOrEmpty(entityAM.OriginalStatusCode))
            {
                if ((Partner != null && Partner.ReceiveAllStatuses == true) || (privatelabel != null && privatelabel.ReceiveAllStatuses == true))
                {

                    EntityStatus status = EntityStatusRepository.GetSingleEntityStatusByCode(entityAM.OriginalStatusCode, entityPM.Tenant, true);
                    if (status != null)
                    {
                        entityPM.StatusId = status.Id;
                    }
                    else
                    {
                        Responce.ErrorType = "Validation Error";
                        Responce.ErrorMessage = "StatusId field doesn't exist in the database, insert this entity before using it.";
                        return Responce;
                    }

                }
                else
                {
                    EntityStatus tempstatus = null;
                    if (!string.IsNullOrEmpty(entityPM.StatusId))
                    {
                        tempstatus = EntityStatusRepository.GetSingleEntityStatus(entityPM.StatusId, entityPM.Tenant, true);
                    }
                    if (tempstatus != null && tempstatus.Code.ToLower() == "ccd")
                    {
                        entityPM.StatusId = tempstatus.Id;
                    }
                    else
                    {
                        EntityStatus status = EntityStatusRepository.GetSingleEntityStatusByCode(entityAM.StatusCode, entityPM.Tenant, true);
                        if (status != null)
                        {
                            entityPM.StatusId = status.Id;
                        }
                        else
                        {
                            Responce.ErrorType = "Validation Error";
                            Responce.ErrorMessage = "StatusId field doesn't exist in the database, insert this entity before using it.";
                            return Responce;
                        }
                    }
                }

            }
            else
            {
                Responce.ErrorType = "Validation Error";
                Responce.ErrorMessage = "StatusId field is required.";
                return Responce;
            }


            //if (!string.IsNullOrEmpty(entityAM.StatusId))
            //{
            //    entityPM.StatusId = entityAM.StatusId;
            //}
            //else
            //{
            //    Responce.ErrorType = "Validation Error";
            //    Responce.ErrorMessage = "StatusId field is required.";
            //    return Responce;
            //}
            if (!string.IsNullOrEmpty(entityAM.ShipmentTypeId))
            {
                entityPM.ShipmentTypeId = entityAM.ShipmentTypeId;
            }
            if (!string.IsNullOrEmpty(entityAM.House))
            {
                entityPM.House = entityAM.House;
            }
            if (!string.IsNullOrEmpty(entityAM.DescriptionOfGoods))
            {
                entityPM.DescriptionOfGoods = entityAM.DescriptionOfGoods;
            }
            if (!string.IsNullOrEmpty(entityAM.ShipmentLevelCode))
            {
                entityPM.ShipmentLevelCode = entityAM.ShipmentLevelCode;
            }
            if (!string.IsNullOrEmpty(entityAM.ConsigneeReference1))
            {
                entityPM.ConsigneeReference1 = entityAM.ConsigneeReference1;
            }
            if (!string.IsNullOrEmpty(entityAM.CustomerReference1))
            {
                entityPM.CustomerReference1 = entityAM.CustomerReference1;
            }
            if (!string.IsNullOrEmpty(entityAM.ConsigneeReference2))
            {
                entityPM.ConsigneeReference2 = entityAM.ConsigneeReference2;
            }
            if (!string.IsNullOrEmpty(entityAM.CustomerReference2) && string.IsNullOrEmpty(entityPM.CustomerReference2))
            {
                entityPM.CustomerReference2 = entityAM.CustomerReference2;
            }
            entityPM.GrossWeight = entityAM.Weight;
            entityPM.NumberOfContainers = entityAM.Quantity;
            entityPM.NumberOfPackages = entityAM.Quantity;
            entityPM.PackagesQuantity = entityAM.Quantity;
            if (!string.IsNullOrEmpty(entityAM.Master))
            {
                entityPM.Master = entityAM.Master;
            }
            if (!string.IsNullOrEmpty(entityAM.ShipmentCustomerTypeCode))
            {
                entityPM.ShipmentCustomerTypeCode = entityAM.ShipmentCustomerTypeCode;
            }

            entityPM.IsCancelled = entityAM.IsCancelled;

            if (!string.IsNullOrEmpty(entityAM.ShipperName))
            {
                entityPM.ShipperName = entityAM.ShipperName;
            }
            if (!string.IsNullOrEmpty(entityAM.CarrierTransportDocumentNumber))
            {
                entityPM.CarrierTransportDocumentNumber = entityAM.CarrierTransportDocumentNumber;
            }
            //if (!string.IsNullOrEmpty(entityAM.ShipperProperties.Code) || !string.IsNullOrEmpty(entityAM.ShipperProperties.Id) || !string.IsNullOrEmpty(entityAM.ShipperProperties.ExternalCode))
            //{
            //    var CustomerId = CardCodePropertiesMapping.GetCardIdFromCardProperties(entityAM.ImporterTenant, entityAM.CustomerProperties);
            //    if (!string.IsNullOrEmpty(CustomerId))
            //    {
            //        entityPM.CustomerId = CustomerId;
            //    }
            //    else
            //    {
            //        Responce.ErrorType = "Validation Error";
            //        Responce.ErrorMessage = "CustomerId field doesn't exist in the database, insert this entity before using it.";
            //        return Responce;
            //    }

            //}
            //else
            //{
            //    Responce.ErrorType = "Validation Error";
            //    Responce.ErrorMessage = "CustomerId field is required.";
            //    return Responce;
            //}
            if (!string.IsNullOrEmpty(entityAM.PreCarriageFromPort.Code) || !string.IsNullOrEmpty(entityAM.PreCarriageFromPort.Id) || !string.IsNullOrEmpty(entityAM.PreCarriageFromPort.ExternalCode))
            {
                var PreCarriageFromPortId = PortCodePropertiesMapping.GetPortIdFromPortProperties(entityAM.ImporterTenant, entityAM.PreCarriageFromPort);
                if (!string.IsNullOrEmpty(PreCarriageFromPortId))
                {
                    entityPM.PreCarriageFromPortId = PreCarriageFromPortId;
                }
                else
                {
                    Responce.ErrorType = "Validation Error";
                    Responce.ErrorMessage = "PreCarriageFromPortId field doesn't exist in the database, insert this entity before using it.";
                    return Responce;
                }
            }
            //else
            //{
            //    Responce.ErrorType = "Validation Error";
            //    Responce.ErrorMessage = "PreCarriageFromPortId field is required.";
            //    return Responce;
            //}
            if (!string.IsNullOrEmpty(entityAM.PreCarriageToPort.Code) || !string.IsNullOrEmpty(entityAM.PreCarriageToPort.Id) || !string.IsNullOrEmpty(entityAM.PreCarriageToPort.ExternalCode))
            {
                var PreCarriageToPortId = PortCodePropertiesMapping.GetPortIdFromPortProperties(entityAM.ImporterTenant, entityAM.PreCarriageToPort);
                if (!string.IsNullOrEmpty(PreCarriageToPortId))
                {
                    entityPM.PreCarriageToPortId = PreCarriageToPortId;
                }
                else
                {
                    Responce.ErrorType = "Validation Error";
                    Responce.ErrorMessage = "PreCarriageToPortId field doesn't exist in the database, insert this entity before using it.";
                    return Responce;
                }
            }
            //else
            //{
            //    Responce.ErrorType = "Validation Error";
            //    Responce.ErrorMessage = "PreCarriageToPortId field is required.";
            //    return Responce;
            //}
            if (!string.IsNullOrEmpty(entityAM.OnCarriageToPort.Code) || !string.IsNullOrEmpty(entityAM.OnCarriageToPort.Id) || !string.IsNullOrEmpty(entityAM.OnCarriageToPort.ExternalCode))
            {
                var OnCarriageToPortId = PortCodePropertiesMapping.GetPortIdFromPortProperties(entityAM.ImporterTenant, entityAM.OnCarriageToPort);
                if (!string.IsNullOrEmpty(OnCarriageToPortId))
                {
                    entityPM.OnCarriageToPortId = OnCarriageToPortId;
                }
                else
                {
                    Responce.ErrorType = "Validation Error";
                    Responce.ErrorMessage = "OnCarriageToPortId field doesn't exist in the database, insert this entity before using it.";
                    return Responce;
                }
            }
            //else
            //{
            //    Responce.ErrorType = "Validation Error";
            //    Responce.ErrorMessage = "OnCarriageToPortId field is required.";
            //    return Responce;
            //}
            if (entityAM.ShipmentPackagesAM != null)
            {
                if (entityPM.ShipmentPackages == null)
                {
                    entityPM.ShipmentPackages = new List<ShipmentPackagePM>();
                }

                foreach (ShipmentPackageAM package in entityAM.ShipmentPackagesAM)
                {
                    var MyPackage = new ShipmentPackagePM();
                    MyPackage.Tenant = entityAM.ImporterTenant;
                    MyPackage.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    if (!string.IsNullOrEmpty(package.PackageTypeCode))
                    {
                        //package.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                        PackageTypeRepository packageTypeRep = new PackageTypeRepository(commoncontext);
                        PackageType type = packageTypeRep.GetSinglePackageTypeByCode(package.PackageTypeCode, entityPM.Tenant, true);

                        if (type != null)
                        {
                            MyPackage.PackageTypeId = type.Id;
                        }
                        else
                        {
                            Responce.ErrorType = "Validation Error";
                            Responce.ErrorMessage = "PackageTypeCode field doesn't exist in the database,Upsert this entity before using it.";
                            return Responce;
                        }
                    }
                    else
                    {
                        Responce.ErrorType = "Validation Error";
                        Responce.ErrorMessage = "PackageTypeCode field is required.";
                        return Responce;
                    }
                    if (!string.IsNullOrEmpty(package.ContainerNumber))
                    {
                        MyPackage.ContainerNumber = package.ContainerNumber;
                    }
                    //else
                    //{
                    //    Responce.ErrorType = "Validation Error";
                    //    Responce.ErrorMessage = "ContainerNumber field is required.";
                    //    return Responce;
                    //}
                    if (!string.IsNullOrEmpty(package.ShipperSeal))
                    {
                        MyPackage.ShipperSeal = package.ShipperSeal;
                    }
                    //else
                    //{
                    //    Responce.ErrorType = "Validation Error";
                    //    Responce.ErrorMessage = "ShipperSeal field is required.";
                    //    return Responce;
                    //}
                    MyPackage.Quantity = package.Quantity;
                    MyPackage.Weight = package.Weight;
                    MyPackage.Volume = package.Volume;
                    entityPM.ShipmentPackages.Add(MyPackage);
                }

            }
            return null;
        }

    }
}