using Logitude.BL.CommonDataModel.CodePropertiesMapping;
using Logitude.BL.CommonDataModel.EntityAMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class ImporterShipmentDocumentsCancleController : ApiController
    {
         
        public HttpResponseMessage Put(DocumentsFilingAM EntityAM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                string DocId = EntityAM.CustomerDocumentId;
                int Tenant = EntityAM.ImporterTenant;
                SecurityUtility.AuthenticationOnTenant(Tenant);

                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(Tenant);
                DocumentsFilingPM DocumentFilingPM = documentsFilingQuery.GetSinglePM(DocId, Tenant);

                if (DocumentFilingPM != null)
                {
                    bool IsNewLog = false;
                    string CorrelationId = HttpContext.Current.Request.Headers["CorrelationId"];
                    IWebFreightContext webFreightContext = WebFreightContext.GetContext(DocumentFilingPM.Tenant);
                    APILogsService apiLogsService = new APILogsService(webFreightContext, DocumentFilingPM.Tenant);
                    ObjectTableRepository objectTabelRepository = new ObjectTableRepository(DocumentFilingPM.Tenant);
                    #region APILogs
                    var aPILogsRepository = new APILogsRepository(webFreightContext);
                    APILogs Log = aPILogsRepository.GetSingleAPILogsByCorrelationId(CorrelationId, DocumentFilingPM.Tenant);
                    APILogsPM LogPM;
                    if (Log == null)
                    {
                        IsNewLog = true;
                        var Objecttable = objectTabelRepository.GetObjectTableByName("DocumentsFiling", DocumentFilingPM.Tenant, true);
                        LogPM = new APILogsPM()
                        {
                            Id = IdCounter.GetNumber("APILogs", DocumentFilingPM.Tenant),
                            CorrelationId = CorrelationId,
                            CreateDate = DateTime.Now,
                            CreateDateUTC = DateTime.UtcNow,
                            Direction = "I",
                            EntityId = DocumentFilingPM.Id,
                            LastUpdateDate = DateTime.Now,
                            LastUpdateDateUTC = DateTime.UtcNow,
                            NumberOfRetries = 1,
                            ObjectTableId = Objecttable.Id,
                            ExpirationDate = DateTime.Now.AddDays(90),
                            Refrence = DocumentFilingPM.Code,
                            Status = "I",
                            Tenant = DocumentFilingPM.Tenant
                        };
                    }
                    else
                    {
                        IsNewLog = false;
                        var Objecttable = objectTabelRepository.GetObjectTableByName("DocumentsFiling", DocumentFilingPM.Tenant, true);
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
                    LogPM.Subject = "Delete Document in Importer Tenant";
                    if (IsNewLog)
                    {
                        apiLogsService.Create(LogPM);
                    }
                    try
                    {
                        var msg = "Start deleting document At Importer Tenant " + DateTime.Now;
                        APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, LogPM.Status, 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(DocumentFilingPM), null, null, "");

                        if (DocumentFilingPM != null)
                        {
                            DocumentFilingPM.IsDeleted = true;
                            DocumentFilingPM.DontAddToQueue = true;
                            ContactRepository contactRepository = new ContactRepository(DocumentFilingPM.Tenant);
                            var User = contactRepository.GetSingleContactByEmail("system@tenant" + DocumentFilingPM.Tenant + ".com", DocumentFilingPM.Tenant, true);
                            ICommonDataContext objectContext = CommonDataContext.GetContext(DocumentFilingPM.Tenant);
                            DocumentsFilingService documentsFilingService = new DocumentsFilingService(objectContext, DocumentFilingPM.Tenant);
                            documentsFilingService.Update(DocumentFilingPM, null, User.Id);
                            msg = "Document deleted successfully " + DateTime.Now;
                            APILogsUtility.UpdateAPILogStatus(LogPM.Id, LogPM.Tenant, "D", 1, DateTime.Now, DateTime.UtcNow, msg, LogitudeXmlSerializer.SerializeObjectToXmlString(DocumentFilingPM), DocumentFilingPM.Id, null, "");

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
                return Request.CreateResponse(HttpStatusCode.OK, "Ok");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }    
            
        } 

    }
}