using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Interfaces;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Transactions;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.BL;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Unifreight.BL.EntityQueryServices;
using Unifreight.Data.AmitalModel;
using Logitude.CustomsMessaging.MessagingServices;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    public class CustDocRelatedDocsWebServiceController : ApiController
    {
        public HttpResponseMessage GetDocumentsFilingsForRelatedDocuments(string entityId, string childEntityId, string objectTableId, string directionCode, string referenceNumber, string filterVlaue, string declarationType,string ExportFile)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                if (childEntityId == "null")
                {
                    childEntityId = null;
                }

                if (referenceNumber == "null")
                {
                    referenceNumber = null;
                }

                ICustomContext MyContext = CustomContext.GetContext(authToken.Tenant);
                List<DocumentsFilingPM> documentFilings = null;
                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(authToken.Tenant);
                CFICONNQueryService queryService = null;
                if (declarationType != "E" && CustomsSettingQueryService.GetSettingByTenant(authToken.Tenant).IsConnectedToUniFreight)
                {
                    queryService = new CFICONNQueryService(AmitalContext.GetContext(authToken.Tenant));
                }
                

                //externalEntityReferences.Add("1091");
                //externalEntityReferences.Add("1088");
                //externalEntityReferences.Add("1057");
               
                    if (filterVlaue == "customs" )
                    {

                        if (referenceNumber != null)
                        {
                            documentFilings = documentsFilingQuery.GetDocumentsFilingsForRelatedDocuments(entityId, childEntityId, objectTableId, directionCode, referenceNumber, null, authToken.Tenant);
                        }
                        else
                        {
                            documentFilings = documentsFilingQuery.GetDocumentsFilingsByIdForRelatedDocuments(entityId, childEntityId, objectTableId, directionCode, authToken.Tenant, null);
                        }
                    }
                    else if (filterVlaue == "forwarding")
                    {
                    List<string> externalEntityReferences;
                    if (declarationType=="E")
                    {
                        externalEntityReferences = new List<string> { referenceNumber };

                    }
                    else
                    {
                      externalEntityReferences = queryService.GetImportFilesByCustomFile(Convert.ToInt64(referenceNumber));

                    }

                    documentFilings = documentsFilingQuery.GetDocumentsFilingsByRferenceForRelatedDocuments(authToken.Tenant, externalEntityReferences);

                    }

                    else if (filterVlaue == "all")
                    {
                        List<string> externalEntityReferences;

                     
                         if (declarationType != "E")
                         {
                             externalEntityReferences = queryService.GetImportFilesByCustomFile(Convert.ToInt64(referenceNumber));
                         }
                         else {
                              externalEntityReferences = new List<string> { ExportFile };
                         }
                  
                        if (referenceNumber != null)
                        {
                            documentFilings = documentsFilingQuery.GetDocumentsFilingsForRelatedDocuments(entityId, childEntityId, objectTableId, directionCode, referenceNumber, externalEntityReferences, authToken.Tenant, declarationType);
                        }
                        else
                        {
                            documentFilings = documentsFilingQuery.GetDocumentsFilingsByIdForRelatedDocuments(entityId, childEntityId, objectTableId, directionCode, authToken.Tenant, externalEntityReferences);
                        }
                    }

                    else
                    {
                    if (referenceNumber != null)
                    {
                        documentFilings = documentsFilingQuery.GetDocumentsFilingsForRelatedDocuments(entityId, childEntityId, objectTableId, directionCode, referenceNumber, null, authToken.Tenant);
                    }
                    else
                    {
                        documentFilings =  documentsFilingQuery.GetDocumentsFilingsByIdForRelatedDocuments(entityId, childEntityId, objectTableId, directionCode, authToken.Tenant, null);
                    }
                   }
                    
               



                return Request.CreateResponse(HttpStatusCode.OK, documentFilings);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingleDocumentsFilingPM(string id, bool checkOcr)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(authToken.Tenant);
                DocumentsFilingPM newExtDoc = documentsFilingQuery.GetSinglePM(id, authToken.Tenant, checkOcr);
                return Request.CreateResponse(HttpStatusCode.OK, newExtDoc);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        [HttpGet]
        public HttpResponseMessage UpsertSupplierInvioceByOcr(string declarationId, string documentsFilingId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggingUserId = AuthenticationUtil.ResolveUserId(tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);

               

                var messagingService = new DCAInUCBUpsertSupplierInvioceByOcr_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, loggingUserId, declarationId, documentsFilingId);

                return Request.CreateResponse(HttpStatusCode.OK, sts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

    }
}