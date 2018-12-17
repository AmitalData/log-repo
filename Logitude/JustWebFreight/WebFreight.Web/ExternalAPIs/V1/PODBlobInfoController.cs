using Logitude.BL.CommonDataModel.APIDataContract;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class PODBlobInfoController : ApiController
    {
        public HttpResponseMessage Post(PODBlobInfo blobInfo)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (blobInfo.BlobChunk.Length > 100000) throw new ApplicationException("Blob chunk must not be larger than 100 KB");

                    #region Authentication

                    string token = HttpContext.Current.Request.Headers["Token"];

                    if (string.IsNullOrEmpty(blobInfo.SecurityKey) && string.IsNullOrEmpty(token)) throw new AutenticationException("Sorry! this user is not authorized!");

                    bool isUsedToken = false;
                    string shipmentId = string.Empty;
                    ShipmentQuery shipmentQuery = new ShipmentQuery(blobInfo.Tenant);
                    if (!string.IsNullOrEmpty(blobInfo.SecurityKey))
                    {
                        shipmentId = shipmentQuery.GetShipmentIdBySecurityKeyAndShipmentNumber(blobInfo.ShipmentNumber, blobInfo.SecurityKey, blobInfo.Tenant);
                    }
                    else if (!string.IsNullOrEmpty(token))
                    {
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        if (authToken == null) throw new AutenticationException("Sorry! this user is not authorized!");
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        shipmentId = shipmentQuery.GetShipmentIdByShipmentNumber(blobInfo.ShipmentNumber, blobInfo.Tenant);
                        isUsedToken = true;
                    }

                    if (string.IsNullOrEmpty(shipmentId))
                    {
                        bool isExist = shipmentQuery.CheckIfShipmentExistByShipmentNumber(blobInfo.ShipmentNumber, blobInfo.Tenant);
                        if (!isExist) throw new ApplicationException("התיק לא אותר");
                        else
                        {
                            if (isUsedToken) throw new AutenticationException("Sorry! this user is not authorized!");
                            else throw new ApplicationException("זיהוי משלוח לא תקין- אנא פנה לסוכן מכס");
                        }

                    }

                    #endregion

                    #region UploadDocument

                    DocumentFileUploadHelper documentFileUploadHelper = new DocumentFileUploadHelper();
                    Response response = documentFileUploadHelper.UploadDocumentFileData(blobInfo.BlobChunk, blobInfo.BlobSize, blobInfo.TotalSentChunksSize, blobInfo.BlobChunkIdsList.ToArray(), blobInfo.BlobChunkNumber, blobInfo.Tenant, blobInfo.FileName, blobInfo.DocumentId);
                    DocumentsFilingPM extDocPM = null;
                    if (!response.HasError)
                    {
                        if (string.IsNullOrEmpty(blobInfo.DocumentId))
                        {
                            blobInfo.DocumentId = response.Result;
                        }

                        if (blobInfo.BlobSize == blobInfo.TotalSentChunksSize)
                        {

                            if (HttpContext.Current != null && HttpContext.Current.User.Identity != null && !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                            {
                                HttpContext.Current.User = Thread.CurrentPrincipal =new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity(""), new string[0]);
                            }


                            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(blobInfo.Tenant);
                            ContactRepository contactRepository = new ContactRepository(blobInfo.Tenant);
                            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(blobInfo.Tenant);

                            string objectTableId = objectTabelRepository.GetObjectTableIdByName("Shipment");
                            string userId = contactRepository.GetConactIdByemail("system@tenant" + blobInfo.Tenant.ToString() + ".com", blobInfo.Tenant);
                            string  documentTypeId =  documentTypeRepository.GetDocumentTypeIdByCode("POD", blobInfo.Tenant);

                            if (string.IsNullOrEmpty(documentTypeId)) throw new ApplicationException("סוג מסמך POD לא נמצא");
                    
                            extDocPM = new DocumentsFilingPM()
                            {
                                Id = IdCounter.GetNumber("DocumentsFiling", blobInfo.Tenant).ToString(),
                                DirectionCode = "I",
                                Tenant = blobInfo.Tenant,
                                DocumentId = blobInfo.DocumentId,
                                EntityId = shipmentId,
                                DocumentTypeId = documentTypeId,
                                ObjectTableId = objectTableId,
                                CreatedByUserId = userId,  //System@logitudeworld.com
                                CreateDate = DateTime.Now,
                                OwnerId = userId,
                                UpdatedByUserId = userId,
                                ExternalEntityName = "Shipment",
                                EntityReference = blobInfo.ShipmentNumber,
                                FileExtension = blobInfo.Extension,
                                FileSize = blobInfo.BlobSize,
                                Folder = "docsin",
                                HasFile = true,
                                FileName = blobInfo.FileName.Split('.')[0],
                                Received = true,
                                ReceivedDate = DateTime.Now,
                                ReceivedByUserId = userId,

                                Description = "POD" + " for file " + blobInfo.ShipmentNumber,
                                IsFromUnifreightPodMobile = true,
                            };
                            ICommonDataContext objectContext = CommonDataContext.GetContext(blobInfo.Tenant);
                            DocumentsFilingService documentsService = new DocumentsFilingService(objectContext, blobInfo.Tenant);

                            documentsService.Create(extDocPM, null, null, true);

                        }

                        if (extDocPM != null) return Request.CreateResponse(HttpStatusCode.OK, extDocPM);
                        else return Request.CreateResponse(HttpStatusCode.OK, blobInfo );
                    }
                    else
                    {
                        throw new ApplicationException(response.ErrorMessage);
                    }

                    #endregion 

                }
                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", blobInfo, apiExceptionResult.Exception, "BODBlobInfo", null, "BODBlobInfo API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", blobInfo, apiExceptionResult.Exception, "BODBlobInfo", null, "BODBlobInfo API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

    }
}