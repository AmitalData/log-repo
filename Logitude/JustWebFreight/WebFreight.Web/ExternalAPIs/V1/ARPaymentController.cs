using Logitude.BL.InvoiceModel.APIDataContract;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityLists;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
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
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class ARPaymentController : ApiController
    {


        public HttpResponseMessage GetSingleARPayment(string id, string number)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);


                ARPaymentQueryService Service = new ARPaymentQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = new ARPayment();
                if (!string.IsNullOrEmpty(id))
                {
                    Result = Service.GetARPaymentById(id, tenant);
                }
                else if (!string.IsNullOrEmpty(number))
                {
                    Result = Service.GetARPaymentByNumber(number, tenant);
                }

                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(ARPayment entity)
        {
            ARPayment oldEntity = entity;

            if (ModelState.IsValid)
            {
                try
                {

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        int tenant = entity.Tenant;

                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);

                        if (entity != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<ARPayment>(LogitudeXmlSerializer.SerializeObjectToXmlString(entity));
                        }

                        IInvoiceContext MyContext = InvoiceContext.GetContext(entity.Tenant);
                        ARPaymentQueryService mappingService = new ARPaymentQueryService(entity.Tenant);
                        
                        ARPaymentPM entityPM = mappingService.ARPaymentDataMappingAndValidatin(entity, entity.Tenant);
                        entityPM.UpdatedByUserId = entity.CreatedByUser.Id;
                        entityPM.Tenant = entity.Tenant;
                        entityPM.SetApproved = true;
                        entityPM.IsExternalEntity = true;
                        mappingService.CheckARPaymentNumber(entityPM.PaymentNo,entityPM.Id, entityPM.Tenant);
                       
                        ARPaymentService service = new ARPaymentService(MyContext, entity.Tenant);
                        entityPM = mappingService.SetARPaymentPMFields(entityPM);
                        service.Create(entityPM);

                       

                        entity = mappingService.ARPaymentDataMapping(entityPM, entity.Tenant);
                        APIHelper.AddCommunicationLog("D", oldEntity, entity, "ARPayment", entityPM.Id, "ARPayment API", entity.Tenant);

                        scope.Complete();


                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "ARPayment", null, "ARPayment API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }

            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "ARPayment", null, "ARPayment API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(ARPayment entity)
        {
            ARPayment oldEntity = entity;

            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {

                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        int tenant = authToken.Tenant;
                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                        if (entity != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<ARPayment>(LogitudeXmlSerializer.SerializeObjectToXmlString(entity));
                        }

                        IInvoiceContext MyContext = InvoiceContext.GetContext(tenant);
                        ARPaymentQueryService mappingService = new ARPaymentQueryService(tenant);
                        ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
                        ARPaymentList payment = paymentQuery.GetPaymentByPaymentNumber(entity.PaymentNo, tenant);
                        if(payment == null)
                        {
                            throw new ApplicationException("ARPayment with number " + entity.PaymentNo + " doesn't exist");
                        }
                        entity.Id = payment.Id;
                       ARPaymentPM entityPM = mappingService.ARPaymentDataMappingAndValidatin(entity, tenant);
                        entityPM.IsExternalEntity = true;
                        mappingService.CheckARPaymentNumber(entityPM.PaymentNo, entityPM.Id, entityPM.Tenant);

                        ARPaymentService service = new ARPaymentService(MyContext, tenant);
                        service.Update(entityPM, true);

                        APIHelper.AddCommunicationLog("D", oldEntity, entity, "ARPayment", entityPM.Id, "ARPayment API", authToken.Tenant);

                        scope.Complete();


                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "ARPayment", null, "ARPayment API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "ARPayment", null, "ARPayment API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
       
    }
}