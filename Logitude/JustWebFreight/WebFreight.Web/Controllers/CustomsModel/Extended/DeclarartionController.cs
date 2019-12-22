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
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;

using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using WebFreight.Web.CustomModel;
using Logitude.Customs.BL.EntityUpdateServices;
using Unifreight.Data.AmitalModel;
using WebFreight.Web.WebServices;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Logitude.CustomsMessaging.Helpers;
using Logitude.Server.Tools.Models;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.CustomsMessaging.RequestServices;
using System.Xml;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    public class DeclarartionController : ApiController
    {




        public HttpResponseMessage GetSingleDeclarationByCustomFileNo(string customFileNo)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                DeclarationRepository declarationRep = new DeclarationRepository(customContext);
                string id = declarationRep.GetIdByCustomFileNo(customFileNo, tenant);

                DeclarationQueryService declarationQuery = new DeclarationQueryService(customContext);
                DeclarationListQueryService listService = new DeclarationListQueryService(customContext);
                DeclarationList declaration = listService.GetSingle(id);

                return Request.CreateResponse(HttpStatusCode.OK, declaration);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSingleDeclarationByNumber(string declarationByNumber
            , int tenant
            )
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                DeclarationRepository declarationRep = new DeclarationRepository(customContext);
                string id = declarationRep.GetIdByDeclarationNumber(declarationByNumber, tenant);

                DeclarationQueryService declarationQuery = new DeclarationQueryService(customContext);
                DeclarationListQueryService listService = new DeclarationListQueryService(customContext);
                DeclarationList declaration = listService.GetSingle(id);

                return Request.CreateResponse(HttpStatusCode.OK, declaration);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetConsignmentListPMByCustomFileNo(string customFileNo)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                DeclarationRepository declarationRep = new DeclarationRepository(customContext);
                string DeclarationId = declarationRep.GetIdByCustomFileNo(customFileNo, tenant);
                if (string.IsNullOrWhiteSpace(DeclarationId))
                {
                    //return null;
                    return Request.CreateResponse(HttpStatusCode.OK, DeclarationId);
                }

                DeclarationQueryService declarationQuery = new DeclarationQueryService(customContext);
                List<ConsignmentPM> myConsignmentPM = declarationQuery.GetConsignmentListPMByDeclarationId(DeclarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myConsignmentPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        public HttpResponseMessage GetDeclarationPendingListPMByDeclarationId(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                DeclarationRepository declarationRep = new DeclarationRepository(customContext);
                 if (string.IsNullOrWhiteSpace(declarationId))
                {
                    //return null;
                    return Request.CreateResponse(HttpStatusCode.OK, declarationId);
                }

                DeclarationQueryService declarationQuery = new DeclarationQueryService(customContext);
                List<DeclarationPendingPM> myDeclarationPendingPM = declarationQuery.GetDeclarationPendingListPMByDeclarationId(declarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myDeclarationPendingPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetCurrenciesCodesForDeclaration(string declarationId, int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                var list = GetCurrenciesCodesForDeclarationBL(declarationId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        List<SupplierInvoiceCurrency> GetCurrenciesCodesForDeclarationBL(string declarationId, int tenant)
        {


            var customContext = CustomContext.GetContext(tenant);


            var declarationQuery = new DeclarationQueryService(customContext);
            List<SupplierInvoicePM> invoices = declarationQuery.GetInvoicesByDeclaration(declarationId, tenant);
            List<SupplierInvoiceCurrency> result = (from a in invoices
                                                    group a by new { a.ExchangeRate, a.InvoiceCurrencyTypeCode } into gr
                                                    select new SupplierInvoiceCurrency()
                                                    {
                                                        Id = Guid.NewGuid().ToString(),
                                                        ExchangeRate = gr.Key.ExchangeRate,
                                                        InvoiceCurrencyId = gr.Key.InvoiceCurrencyTypeCode,

                                                    }).ToList();
            return result;
        }

        public HttpResponseMessage GetSingleDeclarationPMByCargoIdentifiers(string cargoTypeCode, string manifestNumber, string secondCargoID, int tenant)
        {

            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                var customContext = CustomContext.GetContext(tenant);
                var declarationQuery = new DeclarationQueryService(customContext);
                var declarationPM = declarationQuery.GetDeclarationPMByCargoIdentifiers(cargoTypeCode, manifestNumber, secondCargoID, tenant);
                List<DeclarationPM> myList = null;
                if (declarationPM == null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, myList);

                }
                myList = new List<DeclarationPM>();
                myList.Add(declarationPM);
                ;

                return Request.CreateResponse(HttpStatusCode.OK, myList);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage PutCopyDeclaration(string fromDeclarationId, string toDeclarationId, int tenant)
        {

            try
            {
                //string token = HttpContext.Current.Request.Headers["Token"];
                //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                //string loggedUserEmail = authToken.Email;
                //int tenant = authToken.Tenant;
                //SecurityUtility.AuthenticationOnTenant(tenant);
                var customContext = CustomContext.GetContext(tenant);

                DeclarationUpdateService service = new DeclarationUpdateService(customContext, new Dictionary<string, IContext>(), tenant);
                service.CopyDeclaration(fromDeclarationId, toDeclarationId, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSupplierInvoiceItemCount(string declarationId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                var customContext = CustomContext.GetContext(tenant);
                SupplierInvoiceItemQueryService queryService = new SupplierInvoiceItemQueryService(customContext);
                SupplierInvoiceQueryService invoiceService = new SupplierInvoiceQueryService(customContext);
                DeclarationQueryService declarationQuery = new DeclarationQueryService(customContext);
                DeclarationPM declaration = declarationQuery.GetSingle(declarationId, false, false);
                declaration.SupplierInvoices = invoiceService.GetSupplierInvoicesForDeclaration(declarationId, tenant, false);
                bool moreThan1000 = false;
                foreach (SupplierInvoicePM item in declaration.SupplierInvoices)
                {
                    int count = queryService.GetSupplierInvoiceItemsCount(declarationId, item.InvoiceCounterKey, tenant);
                    if (count > 1000)
                    {
                        moreThan1000 = true;
                        break;
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, moreThan1000);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDeclarationByCustomFileNoAndCCU(string customFileNo)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);
                DeclarationList declaration = new DeclarationList();
                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);

                DeclarationRepository declarationRep = new DeclarationRepository(customContext);
                string id = declarationRep.GetIdByCustomFileNo(customFileNo, tenant);

                // first check if Declaration exist 
                if (!string.IsNullOrWhiteSpace(id))
                {
                    DeclarationQueryService declarationQuery = new DeclarationQueryService(customContext);
                    DeclarationListQueryService listService = new DeclarationListQueryService(customContext);
                    declaration = listService.GetSingle(id);
                }
                else // if not, Check if there is a CCU file
                {
                    AmitalContext amitalContext = AmitalContext.GetContext(tenant);
                    var myCCUFILEMQueryService = new Unifreight.BL.EntityQueryServices.CCUFILEMQueryService(amitalContext);
                    long lCUSTOMFILENO;
                    if (long.TryParse(customFileNo, out lCUSTOMFILENO))
                    {
                        int? FILENO = myCCUFILEMQueryService.GetFILENOByCUSTOMFILENO(lCUSTOMFILENO);
                        if (FILENO.HasValue)
                        {
                            declaration.Id = FILENO.ToString();
                        }
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, declaration);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetLast2750ResponseDataAsFileStream(string customFileNo, int tenant)//AMI-63411 - ממשק מסמכים למערכת  - יצירת WS לקבלת תשובת של המכס על שליחת DECLARATION
        {
            //http://localhost:9996/api/Declarartion/GetLast2750ResponseDataAsFileStream?customFileNo=51321159
            string responseDataDocumentId = "NaN";
            string declarationVersionId = "NaN";
            string Status = "Error";
            HttpResponseMessage httpResponse = null;
            try
            {

                ICustomContext customContext = CustomContext.GetContext(tenant);

                DeclarationRepository declarationRep = new DeclarationRepository(customContext);
                var decPoco = declarationRep.GetByCustomFileNo(customFileNo, tenant);

                // first check if Declaration exist 
                if (decPoco == null)
                {
                    throw new Exception("Declaration !exist ");
                }

                declarationVersionId = decPoco.VersionId;
                if (String.IsNullOrWhiteSpace(declarationVersionId))
                {
                    throw new Exception("declarationVersionId !exist ");
                }
                var crsAnalyzeStatus = "30";
                string interfaceTypeCode = "2750";
                var crsRepo = new CustomsRequestsSheetRepository(customContext);
                var crsPoco = crsRepo.GetLastCRSByCustomfileStatusInterface(decPoco.CustomFileNo, crsAnalyzeStatus, interfaceTypeCode, tenant);
                if (crsPoco == null)
                {
                    throw new Exception("CustomsRequestsSheet !exist ");
                }
                var stepRepo = new CommunicationLogStepRepository(tenant);
                int stepReceivedCustomResponseCorrelation = 20;
                var stepPoco = stepRepo.CommunicationLogStep(crsPoco.RequestComminicationId, stepReceivedCustomResponseCorrelation, tenant);
                responseDataDocumentId = stepPoco.DocumentId;
                if (String.IsNullOrWhiteSpace(responseDataDocumentId))
                {
                    throw new Exception("ResponseDataDocumentId !exist ");
                }


                string blobId = tenant + "_" + responseDataDocumentId;
                httpResponse = Uploader.GetFileStream(blobId);
                Status = "OK";
            }
            catch (Exception ee)
            {

                XElement myXml =
new XElement("FileStreamError",
    new XElement("Error", ee.ToString()

        )
    );

                var data = System.Text.UTF8Encoding.UTF8.GetBytes(myXml.ToString());

                httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

                httpResponse.Content = new StreamContent(new MemoryStream(data));
                httpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                httpResponse.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                httpResponse.Content.Headers.ContentDisposition.FileName = responseDataDocumentId + ".xml";
            }
            finally
            {
                var fileName = httpResponse.Content.Headers.ContentDisposition.FileName;
                var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                var Extension = Path.GetExtension(fileName);
                var newFileName = $"DocumentId={responseDataDocumentId};DeclarationVersionId={declarationVersionId};Status={Status}" + Extension;
                httpResponse.Content.Headers.ContentDisposition.FileName = newFileName;

            }
            return httpResponse;


        }





        public HttpResponseMessage GetLast2755ResponseDataAsFileStream(string customFileNo, int tenant)//AMI-66312 - שליחת מסר תשובה של מסר הגשה במקום של טיוטה אחרונה
        {
            //http://192.116.221.103:572/NextProd572/api/Declarartion/GetLast2755ResponseDataAsFileStream?customFileNo=51340159&tenant=1
            string responseDataDocumentId = "NaN";
            string declarationVersionId = "NaN";
            string Status = "Error";
            HttpResponseMessage httpResponse = null;
            try
            {

                ICustomContext customContext = CustomContext.GetContext(tenant);

                DeclarationRepository declarationRep = new DeclarationRepository(customContext);
                var decPoco = declarationRep.GetByCustomFileNo(customFileNo, tenant);

                // first check if Declaration exist 
                if (decPoco == null)
                {
                    throw new BusinessErrorException("Declaration !exist ");
                }

                declarationVersionId = decPoco.VersionId;
                if (String.IsNullOrWhiteSpace(declarationVersionId))
                {
                    throw new BusinessErrorException("declarationVersionId !exist ");
                }
                var crsAnalyzeStatus = "30";
                string interfaceTypeCode = "2755";
                var crsRepo = new CustomsRequestsSheetRepository(customContext);
                var crsPoco = crsRepo.GetLastCRSByCustomfileStatusInterfaceFirstOrDefault(decPoco.CustomFileNo, crsAnalyzeStatus, interfaceTypeCode, tenant);
                if (crsPoco == null)
                {
                    throw new BusinessErrorException(/*"CustomsRequestsSheet !exist "*/ $"Message '{interfaceTypeCode}' didn't send yet  to customs!!!.");
                }

                var stepRepo = new CommunicationLogStepRepository(tenant);
                int stepReceivedCustomResponseCorrelation = 20;
                var stepPoco = stepRepo.CommunicationLogStep(crsPoco.RequestComminicationId, stepReceivedCustomResponseCorrelation, tenant);
                responseDataDocumentId = stepPoco.DocumentId;
                if (String.IsNullOrWhiteSpace(responseDataDocumentId))
                {
                    throw new BusinessErrorException("ResponseDataDocumentId !exist ");
                }


                string blobId = tenant + "_" + responseDataDocumentId;
                httpResponse = Uploader.GetFileStream(blobId);
                Status = "OK";
            }
            catch (BusinessErrorException businessErrorException)
            {
                XElement myXml =
new XElement("FileStreamError",
    new XElement("Error", businessErrorException.Message
        ));
                httpResponse = GetResponse(responseDataDocumentId, myXml);
            }
            catch (Exception ee)
            {

                XElement myXml =
new XElement("FileStreamError",
    new XElement("Error", ee.ToString()

        )
    );
                httpResponse = GetResponse(responseDataDocumentId, myXml);
            }
            finally
            {
                var fileName = httpResponse.Content.Headers.ContentDisposition.FileName;
                var FileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                var Extension = Path.GetExtension(fileName);
                var newFileName = $"DocumentId={responseDataDocumentId};DeclarationVersionId={declarationVersionId};Status={Status}" + Extension;
                httpResponse.Content.Headers.ContentDisposition.FileName = newFileName;

            }
            return httpResponse;


        }

        private static HttpResponseMessage GetResponse(string responseDataDocumentId, XElement myXml)
        {
            HttpResponseMessage httpResponse;
            var data = System.Text.UTF8Encoding.UTF8.GetBytes(myXml.ToString());

            httpResponse = new HttpResponseMessage(HttpStatusCode.OK);

            httpResponse.Content = new StreamContent(new MemoryStream(data));
            httpResponse.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            httpResponse.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            httpResponse.Content.Headers.ContentDisposition.FileName = responseDataDocumentId + ".xml";
            return httpResponse;
        }


        public HttpResponseMessage PostSendCollateral8212(SendCollateralsRequestParams requestParamsData)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(tenant);

                ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
                var messagingService = new DCAInUCB8212_MsgMessagingService();
                var sts = messagingService.CreateCRS(tenant, null, requestParamsData.Collaterals);

                return Request.CreateResponse(HttpStatusCode.OK, sts);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage GetDeclarationAmendmentsById(string id)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            int tenant = authToken.Tenant;
            string loggedUserEmail = authToken.Email;
            SecurityUtility.AuthenticationOnTenant(tenant);
            DeclarationList declaration = new DeclarationList();
            ICustomContext customContext = CustomContext.GetContext(authToken.Tenant);
            try {
                DeclarationQueryService declarationQuery = new DeclarationQueryService(customContext);

                var declarations=  declarationQuery.GetDeclarationAmendmentsById(tenant , id);
            return Request.CreateResponse(HttpStatusCode.OK, declarations); 
        }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


}


    }
    }