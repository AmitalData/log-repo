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
using System.IO;
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
using Logitude.SystemLogs;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class ARPaymentController : ApiController
    {


        public HttpResponseMessage GetSingleARPayment(string id, string number)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

            try
            {

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
                Result.PaymentInvoices = null;
                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
              {
                //ExceptionHandler.HandleException
                //ExceptionHandler.HandleException(ex, DateTime.Now, authToken.Tenant , authToken.Email , "", "AuthenticationController : PostLoginData", null);

                //var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                string exceptionMessage = ex.Message+ Environment.NewLine+ex.StackTrace;
                if (ex.InnerException != null)
                {
                    exceptionMessage += Environment.NewLine + "inner1: " + ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace;
                }
                    
                    
                return Request.CreateResponse(HttpStatusCode.InternalServerError, exceptionMessage);//(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
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
                        int tenant = authToken.Tenant;

                        SecurityUtility.AuthenticateAPICall(authToken.Tenant);

                        if (entity != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<ARPayment>(LogitudeXmlSerializer.SerializeObjectToXmlString(entity));
                        }

                        IInvoiceContext MyContext = InvoiceContext.GetContext(tenant);
                        ARPaymentQueryService mappingService = new ARPaymentQueryService(tenant);
                        
                        ARPaymentPM entityPM = mappingService.ARPaymentDataMappingAndValidatin(entity, tenant);
                        entityPM.UpdatedByUserId = entityPM.CreatedByUserId;
                        entityPM.Tenant = tenant;
                        entityPM.SetApproved = true;
                        entityPM.IsExternalEntity = true;
                        mappingService.CheckARPaymentNumber(entityPM.PaymentNo,entityPM.Id, entityPM.Tenant);
                       
                        ARPaymentService service = new ARPaymentService(MyContext, tenant);
                        entityPM = mappingService.SetARPaymentPMFields(entityPM);
                        entityPM = mappingService.MapAPPaymentChequeFieldsToARPayment(entity, entityPM);
                        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                        {
                            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(tenant);
                            var partner = computingPartnerQuery.GetSinglePMByCodeAndCheckTenantZero(entity.ComputingPartnerCode, tenant);
                            entityPM.CreatedByPartner = (partner != null ? partner.Name : null);

                        }
                        service.Create(entityPM);
                      
                       

                        entity = mappingService.ARPaymentDataMapping(entityPM, tenant);

                        APIHelper.AddCommunicationLog("D", oldEntity, entity, "ARPayment", entityPM.Id, "ARPayment API", tenant);

                        scope.Complete();
                        if (entityPM.AccountingPaymentMethodCode == "CA")
                        {
                            entity.ARPaymentCheques = null;
                        }
                        entity.PaymentInvoices = null;
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
                        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                        {
                            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(tenant);
                            var partner = computingPartnerQuery.GetSinglePMByCodeAndCheckTenantZero(entity.ComputingPartnerCode, tenant);
                            entityPM.CreatedByPartner = (partner != null ? partner.Name : null);

                        }
                        ARPaymentService service = new ARPaymentService(MyContext, tenant);
                        service.Update(entityPM, true);

                        APIHelper.AddCommunicationLog("D", oldEntity, entity, "ARPayment", entityPM.Id, "ARPayment API", authToken.Tenant);

                        scope.Complete();

                        entity.PaymentInvoices = null;
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