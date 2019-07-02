using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.Server.Tools;
//using Logitude.Server.Tools.SignalRHubs;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.LogitudeCacheManager;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.App_Code.LogBoxSignTool
{
    public class LogBoxSignatureClientController : ApiController
    {
        public HttpResponseMessage PutSignRequestReceived(DocumentsFilingPM entityPM)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                if (entityPM.CancellSignRequest == true)
                {
                    ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                    DocumentsFilingService service = new DocumentsFilingService(MyContext, entityPM.Tenant);
                    entityPM.CancellSignRequest = false;
                    entityPM.DontAddToQueue = true;
                    service.Update(entityPM, false);
                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }
                var CachedData = LogitudeCacheManager.ServerCache.GetFromCache("ClientAppStatus_" + authToken.Email);// HttpContext.Current.Cache["ClientAppStatus_" + authToken.Email]
                if (CachedData != null)
                {
                    var data = JsonConvert.DeserializeObject<StatusData>(CachedData); //(StatusData)(HttpContext.Current.Cache["ClientAppStatus_" + authToken.Email]);
                                                                                      //var temp = JsonConvert.DeserializeObject<StatusData>(data);
                    if (entityPM.CancellSignRequest == true)
                    {
                        ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                        DocumentsFilingService service = new DocumentsFilingService(MyContext, entityPM.Tenant);
                        entityPM.DontAddToQueue = true;
                        service.Update(entityPM, false);
                        return Request.CreateResponse(HttpStatusCode.OK, "");
                    }
                    if (data != null && data.IsActive == true && data.IsLogged == true && data.IsValidCert == true)
                    {
                        ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                        DocumentsFilingService service = new DocumentsFilingService(MyContext, entityPM.Tenant);
                        entityPM.DontAddToQueue = true;
                        service.Update(entityPM, false);
                        if (entityPM.CancellSignRequest == true)
                        {
                            //ICommonDataContext MyContext = CommonDataContext.GetContext(entityPM.Tenant);
                            //DocumentsFilingService service = new DocumentsFilingService(MyContext, entityPM.Tenant);
                            //service.Update(entityPM, false);
                            return Request.CreateResponse(HttpStatusCode.OK, "");
                        }


                        //SignalRHubMessageSender.SendSignalRMessage("SignRequestReceived", "LogBox", authToken.Email);



                        return Request.CreateResponse(HttpStatusCode.OK, "");
                    }
                    else
                    {
                        if (data == null || !data.IsLogged)
                        {
                            var myError = new MyErrorClass();
                            myError.HasError = true;
                            myError.ErrorsArray = new List<string>();
                            myError.ErrorsArray.Add("Your user is not  logged to the client app");
                            return Request.CreateResponse(HttpStatusCode.OK, myError);
                        }
                        else if (!data.IsActive)
                        {
                            var myError = new MyErrorClass();
                            myError.HasError = true;
                            myError.ErrorsArray = new List<string>();
                            myError.ErrorsArray.Add("Your client app is not activated , please check the app status");
                            return Request.CreateResponse(HttpStatusCode.OK, myError);
                        }
                        else
                        {
                            var myError = new MyErrorClass();
                            myError.HasError = true;
                            myError.ErrorsArray = new List<string>();
                            myError.ErrorsArray.Add("Your card is disconnected !");
                            return Request.CreateResponse(HttpStatusCode.OK, myError);
                        }

                    }
                }
                else
                {
                    var myError = new MyErrorClass();
                    myError.HasError = true;
                    myError.ErrorsArray = new List<string>();
                    myError.ErrorsArray.Add("Your client app is not activated , please check the app status");
                    return Request.CreateResponse(HttpStatusCode.OK, myError);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "LogBoxSignatureClient Request", null);
                var myError = new MyErrorClass();
                myError.HasError = true;
                myError.ErrorsArray = new List<string>();
                myError.ErrorsArray.Add(ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
                return Request.CreateResponse(HttpStatusCode.OK, myError);
                //return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetMultiSignRequestReceived([FromUri] List<string> Ids)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                var CachedData = LogitudeCacheManager.ServerCache.GetFromCache("ClientAppStatus_" + authToken.Email);// HttpContext.Current.Cache["ClientAppStatus_" + authToken.Email]
                if (CachedData != null)
                {
                    var data = JsonConvert.DeserializeObject<StatusData>(CachedData); //(StatusData)(HttpContext.Current.Cache["ClientAppStatus_" + authToken.Email]);


                    if (data != null && data.IsActive == true && data.IsLogged == true && data.IsValidCert == true)
                    {
                        foreach (var item in Ids)
                        {
                            ICommonDataContext MyContext = CommonDataContext.GetContext(authToken.Tenant);
                            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(authToken.Tenant);
                            DocumentsFilingPM entityPM = documentsFilingQuery.GetSinglePM(item, authToken.Tenant);
                            DocumentsFilingService service = new DocumentsFilingService(MyContext, entityPM.Tenant);
                            entityPM.DontAddToQueue = true;
                            entityPM.SignRequestByUserEmail = authToken.Email;
                            service.Update(entityPM, false);

                        }
                        return Request.CreateResponse(HttpStatusCode.OK,"");
                    }
                    else
                    {
                        if (data == null || !data.IsLogged)
                        {
                            var myError = new MyErrorClass();
                            myError.HasError = true;
                            myError.ErrorsArray = new List<string>();
                            myError.ErrorsArray.Add("Your user is not  logged to the client app");
                            return Request.CreateResponse(HttpStatusCode.OK, myError);
                        }
                        else if (!data.IsActive)
                        {
                            var myError = new MyErrorClass();
                            myError.HasError = true;
                            myError.ErrorsArray = new List<string>();
                            myError.ErrorsArray.Add("Your client app is not activated , please check the app status");
                            return Request.CreateResponse(HttpStatusCode.OK, myError);
                        }
                        else
                        {
                            var myError = new MyErrorClass();
                            myError.HasError = true;
                            myError.ErrorsArray = new List<string>();
                            myError.ErrorsArray.Add("Your card is disconnected !");
                            return Request.CreateResponse(HttpStatusCode.OK, myError);
                        }

                    }
                }
                else
                {
                    var myError = new MyErrorClass();
                    myError.HasError = true;
                    myError.ErrorsArray = new List<string>();
                    myError.ErrorsArray.Add("Your client app is not activated , please check the app status");
                    return Request.CreateResponse(HttpStatusCode.OK, myError);
                }
                 
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "LogBoxSignatureClient Request", null);
                var myError = new MyErrorClass();
                myError.HasError = true;
                myError.ErrorsArray = new List<string>();
                myError.ErrorsArray.Add(ex.Message + (ex.InnerException != null ? ex.InnerException.Message : ""));
                return Request.CreateResponse(HttpStatusCode.OK, myError);
            }
        }

        public HttpResponseMessage GetDocumentDataBySignRequestUserEmail(string Email, int myTenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int Tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(Tenant);

                SecurityUtility.CheckContactFeature("DocumentsFiling", "READ", Tenant);
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(Tenant);
                DocumentsFilingPM documentsFilingPM = documentsFilingQuery.GetSinglePMBySignRequestEmail(Email, Tenant);
                if (documentsFilingPM != null)
                {
                    documentsFilingPM.OrigionalDocumentId = documentsFilingPM.DocumentId;
                    //documentsFilingPM.DocumentId = null;
                    var commoncontext = CommonDataContext.GetContext(documentsFilingPM.Tenant);
                    documentsFilingPM.DontAddToQueue = true;

                    if (documentsFilingPM.SignDueDate != null && DateTime.Now > documentsFilingPM.SignDueDate)
                    {
                        documentsFilingPM.SignRequestByUserEmail = null;
                    }
                    DocumentsFilingService Service = new DocumentsFilingService(commoncontext, documentsFilingPM.Tenant);
                    Service.Update(documentsFilingPM, false);
                }

                //if (documentsFilingPM.CancellSignRequest)
                //{
                //    ICommonDataContext MyContext = CommonDataContext.GetContext(documentsFilingPM.Tenant);
                //    DocumentsFilingService service = new DocumentsFilingService(MyContext, documentsFilingPM.Tenant);
                //    documentsFilingPM.CancellSignRequest = false;
                //    service.Update(documentsFilingPM, false);
                //}
                DocumentData data = new DocumentData();
                if (documentsFilingPM != null)
                {
                    Uploader up = new Uploader();
                    var _DatainByte = up.DownloadFile(documentsFilingPM.DocumentId, documentsFilingPM.FileExtension, "", Tenant);
                    data.BinarryFile = _DatainByte;
                    data.Extention = documentsFilingPM.FileExtension;
                    data.DocumentId = documentsFilingPM.DocumentId;
                    data.DocumentFilingId = documentsFilingPM.Id;
                    data.EntityId = documentsFilingPM.EntityId;
                    data.ObjectTableId = documentsFilingPM.ObjectTableId;
                    data.FileName = documentsFilingPM.FileName;
                }

                return Request.CreateResponse(HttpStatusCode.OK, data);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage Post(FileInformation FileInfo)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int Tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(Tenant);
                DocumentFileUploadHelper documentFileUploadHelper = new DocumentFileUploadHelper();
                bool isDigitallySigned = false;
                string signersList = "";
                ICommonDataContext commoncontext = CommonDataContext.GetContext(Tenant);
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(Tenant);
                ShipmentRepository ShipmentRepo = new ShipmentRepository(Tenant);
                DocumentsFilingPM extDocPM = documentsFilingQuery.GetSinglePM(FileInfo.DocumentsFilingId, Tenant);
                if (!string.IsNullOrEmpty(extDocPM.SignRequestByUserEmail))
                {
                    if (FileInfo.FileSize == 0)
                    {
                        //DocumentRepository rep = new DocumentRepository(commoncontext);
                        //var doc = rep.GetSingleDocument(Tenant, extDocPM.DocumentId);
                        ////doc.HasFile = false;
                        //rep.Update(doc);
                        //rep.SubmitChanges();
                        DocumentsFilingService Service = new DocumentsFilingService(commoncontext, Tenant);
                        extDocPM.SignRequestByUserEmail = null;
                        extDocPM.DontAddToQueue = true;
                        Service.Update(extDocPM, false);
                        return Request.CreateResponse(HttpStatusCode.OK, extDocPM.DocumentId);
                    }
                    if (FileInfo.BufferNumber == 0)
                    {
                        DocumentsFilingService Service = new DocumentsFilingService(commoncontext, Tenant);
                        extDocPM.DocumentId = null;
                        extDocPM.DontAddToQueue = true;
                        Service.Update(extDocPM, false);
                    }

                    var response = documentFileUploadHelper.UploadSignDocumentFileData(FileInfo.buffer, FileInfo.FileSize, FileInfo.SentSize, FileInfo.BlockIdsList, FileInfo.BufferNumber, Tenant, FileInfo.FileName, FileInfo.DocumentsFilingId, ref isDigitallySigned, ref signersList);
                    if (!response.HasError)
                    {
                        if (FileInfo.FileSize == FileInfo.SentSize)
                        {
                            commoncontext = CommonDataContext.GetContext(Tenant);
                            documentsFilingQuery = new DocumentsFilingQuery(Tenant);
                            DocumentsFilingService Service = new DocumentsFilingService(commoncontext, Tenant);
                            extDocPM = documentsFilingQuery.GetSinglePM(FileInfo.DocumentsFilingId, Tenant);
                            extDocPM.IsDigitallySigned = isDigitallySigned;
                            extDocPM.IsDigitalSignRequired = false;
                            extDocPM.SignersList = signersList;
                            extDocPM.SignRequestByUserEmail = null;
                            //extDocPM.DontAddToQueue = true;
                            
                            if (extDocPM.IsSharedWithForwarder == true)
                            {
                                var DocsEntity = ShipmentRepo.GetSingleShipment(extDocPM.EntityId, extDocPM.Tenant);
                                if (!string.IsNullOrEmpty(DocsEntity.ForwarderShipmentNumber))
                                {
                                    extDocPM.DontAddToQueue = false;
                                }
                                else
                                {
                                    extDocPM.DontAddToQueue = true;
                                }
                                //sextDocPM.IsSharedWithForwarder = true;
                                //extDocPM.ForwarderDocumentId = null;
                            }
                            else
                            {
                                extDocPM.DontAddToQueue = true;
                            }
                            if (extDocPM.IsSharedWithCustomer == true)
                            {
                                extDocPM.DontAddToQueue = false;
                                //sextDocPM.IsSharedWithForwarder = true;
                                //extDocPM.ForwarderDocumentId = null;
                            }
                            Service.Update(extDocPM, false);

                            UserRepository userRep = new UserRepository(Tenant);
                            User loggedUser = userRep.GetSingleUserByEmail(authToken.Email, Tenant);
                            //SignalRHubMessageSender.SendSignalRMessage("DocumentSigned", "User" + loggedUser.Id + Tenant, FileInfo.DocumentsFilingId);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, response.Result);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, response.ErrorMessage);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "");
                }

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostStatusData(StatusData statusData, string Email)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int Tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(Tenant);
                var serializedObj = JsonConvert.SerializeObject(statusData);
                LogitudeCacheManager.ServerCache.AddToCache("ClientAppStatus_" + authToken.Email, serializedObj);
                //HttpContext.Current.Cache["ClientAppStatus_" + authToken.Email] = statusData;
                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetServerStatus(int Tenant)
        {
            return Request.CreateResponse(HttpStatusCode.OK, Tenant);
        }

        public HttpResponseMessage GetIfThereIsSignRequestByUserEmail(string Email, int myTenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int Tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(Tenant);

                SecurityUtility.CheckContactFeature("DocumentsFiling", "READ", Tenant);
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(Tenant);
                DocumentsFilingPM documentsFilingPM = documentsFilingQuery.GetSinglePMBySignRequestEmail(Email, Tenant);
                if (documentsFilingPM != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, documentsFilingPM.Id);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "false");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage GetSignAppLastVersion(int Tenant)
        {
            return Request.CreateResponse(HttpStatusCode.OK, LogitudeSettings.SignAppVersion);
        }
    }

}

public class MyErrorClass
{
    public bool HasError { get; set; }
    public List<string> ErrorsArray { get; set; }
}

public class DocumentData
{
    public byte[] BinarryFile { get; set; }
    public string DocumentId { get; set; }
    public string Extention { get; set; }
    public string DocumentFilingId { get; set; }
    public string EntityId { get; set; }
    public string ObjectTableId { get; set; }
    public string FileName { get; set; }
}

public class StatusData
{
    public int Tenant { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastStatusDate { get; set; }
    public bool IsLogged { get; set; }
    public string LoggedByUserEmail { get; set; }
    public bool IsValidCert { get; set; }
}
