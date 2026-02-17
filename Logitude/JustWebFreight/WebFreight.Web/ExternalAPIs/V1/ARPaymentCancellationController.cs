using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
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
    public class ARPaymentCancellationController : ApiController
    {

        ARPaymentCancellation ARPaymentCancellation = null;
        UserPM user;
        ARPaymentPM paymentPM;
        public HttpResponseMessage Put(ARPaymentCancellation entity)
        {
             paymentPM = null;
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                    ARPaymentCancellation = entity;

                    ARPaymentQueryService Service = new ARPaymentQueryService(tenant);
                    ServiceResponse response = new ServiceResponse();
                    var Result = new ARPayment();

                    CheckPaymentNumber(entity.PaymentNo, tenant);
                   
                    CheckPaymentStatus(paymentPM);
                    SetPaymentVoided(paymentPM);
                    UpdateARPayment(paymentPM);


                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, "Payment has been voided");
                }
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                APIHelper.AddCommunicationLog("F", paymentPM, apiExceptionResult.Exception, "ARPayment", null, "ARPayment API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public void UpdateARPayment(ARPaymentPM paymentPM)
        {
            IInvoiceContext MyContext = InvoiceContext.GetContext(paymentPM.Tenant);
            ARPaymentService service = new ARPaymentService(MyContext, paymentPM.Tenant);
            service.Update(paymentPM);
        }


        public void SetPaymentVoided(ARPaymentPM paymentPM)
        {
            paymentPM.SetVoided = true;
            GetUser(paymentPM.Tenant);
            if(user != null)
            {
                paymentPM.UpdatedByUserId = user.Id;
            }
           
            paymentPM.InternalNotes = ARPaymentCancellation.Remark;
            paymentPM.IsExternalEntity = true;
        }

        public ARPaymentPM GetARPaymentPMByNumber(string PaymentNo, int tenant)
        {
            ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
            ARPaymentPM paymentPM = paymentQuery.GetSinglePaymentByPaymentNumber_00(PaymentNo, tenant);
            return paymentPM;
        }

        public void CheckPaymentStatus(ARPaymentPM paymentPM)
        {
            if(paymentPM.StatusCode != "AD")
            {
                throw new Exception("Payment Status <> Approved - Cant Void");
            }
        }

        public void GetUser(int tenant)
        {
            UserQuery userQuery = new UserQuery(tenant);
            if (!string.IsNullOrEmpty(ARPaymentCancellation.CancelledbyUser))
            {
                user = userQuery.GetSinglePMByCode(ARPaymentCancellation.CancelledbyUser, tenant);
                if (user == null)
                {
                    throw new Exception("User with code " + ARPaymentCancellation.CancelledbyUser + " doesn't exist");
                }
            }

            else
            {
                throw new Exception("CancelledbyUser is required");
            }
        }

        public void CheckPaymentNumber(string paymentNo, int tenant)
        {
            if (!string.IsNullOrEmpty(paymentNo))
            {
                paymentPM = GetARPaymentPMByNumber(paymentNo, tenant);
                if (paymentPM == null)
                {
                    throw new Exception("Payment with number  " + paymentNo + " does not exist");
                }

            }

            else
            {
                throw new Exception("ARPayment number is required");
            }
        }

        


    }
}