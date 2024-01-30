using Logitude.BL.Security;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.MessagingServices;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.WebServices
{
    
    public class CertificateOfOriginController : ApiController
    {

        // POST api/<controller>    

        public HttpResponseMessage PostCertificateOfOriginRequest(CertificateOfOriginRequestRequestParams requestParams)
        {
            try
            {
				INF_MSG_GenericResponseData responseData = null;

                var service = new DCAInGetPC_MSG2280_2281_CertificateOfOriginRequestMessagingService();
                responseData = service.Send(requestParams);

                return Request.CreateResponse(HttpStatusCode.OK, responseData);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

		public HttpResponseMessage GetCertificateOfOriginByID(string declarationId ,int tenant)
		{
			try
			{
				CertificateOfOriginQueryService certificateOfOriginQueryService =  new CertificateOfOriginQueryService(tenant);
                 var certificateOfOrigins = certificateOfOriginQueryService.GetCertificateOfOriginsByDeclarationId(declarationId, tenant);

				return Request.CreateResponse(HttpStatusCode.OK, certificateOfOrigins);
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}
		}

        public HttpResponseMessage Delete(string certificateOfOriginId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;



                        ICustomContext MyContext = CustomContext.GetContext(tenant);

                        CertificateOfOriginQueryService query = new CertificateOfOriginQueryService(MyContext);
                        CertificateOfOriginPM entityPM = query.GetSingle(certificateOfOriginId, false, false);
                        CertificateOfOriginUpdateService service = new CertificateOfOriginUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;

                        #region composition handling
                        //certificateOfOrigin invoices: 
                        CertificateOfOriginInvoiceQueryService CertificateOfOriginInvoiceQueryService = new CertificateOfOriginInvoiceQueryService(MyContext);

                        List<CertificateOfOriginInvoicePM> CertificateOfOriginInvoicesChangeset = CertificateOfOriginInvoiceQueryService.GetCertificateOfOriginInvoicesByCertificateId(entityPM.Id, tenant);//ChangeSet.GetAssociatedChanges(entityPM, d => d.CertificateOfOriginInvoices).Cast<CertificateOfOriginInvoicePM>().ToList();
                        foreach (CertificateOfOriginInvoicePM Invoice in CertificateOfOriginInvoicesChangeset)
                        {
                            CertificateOfOriginInvoicePM deletedInvoice = new CertificateOfOriginInvoicePM()
                            {

                                CertificateOfOriginId = Invoice.CertificateOfOriginId,
                                InvoicesIdUry = Invoice.InvoicesIdUry,
                                Id = Invoice.Id,
                                Tenant = Invoice.Tenant,
                                ChangeSetOp = ChangeSetOperation.Delete,
                            };
                            entityPM.DeletedCertificateOriginInvoiceItems.Add(deletedInvoice);
                        }

                        //certificateOfOrigin items: 
                        CertificateOfOriginItemQueryService CertificateOfOriginItemQueryService = new CertificateOfOriginItemQueryService(MyContext);

                        List<CertificateOfOriginItemPM> CertificateOfOriginItemsChangeset = CertificateOfOriginItemQueryService.GetCertificateOfOriginItemsByCertificateId(entityPM.Id, tenant);//ChangeSet.GetAssociatedChanges(entityPM, d => d.CertificateOfOriginItems).Cast<CertificateOfOriginItemPM>().ToList();
                        foreach (CertificateOfOriginItemPM item in CertificateOfOriginItemsChangeset)
                        {
                            CertificateOfOriginItemPM deletedItem = new CertificateOfOriginItemPM()
                            {
                               
                                CertificateOfOriginId = item.CertificateOfOriginId,
                                ItemSerial = item.ItemSerial,
                                Id = item.Id,
                                Tenant = item.Tenant,
                                ChangeSetOp = ChangeSetOperation.Delete,
                            };
                            entityPM.DeletedCertificateOriginItemItems.Add(deletedItem);
                        }

                       
                          
                        #endregion

                        service.Update(entityPM, true);
                        

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }
		
	}
}