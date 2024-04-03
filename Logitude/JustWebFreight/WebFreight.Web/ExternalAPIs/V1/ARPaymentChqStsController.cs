using Logitude.BL.InvoiceModel.APIDataContract;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Security;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class ARPaymentChqStsController : ApiController
    {


        public HttpResponseMessage GetSingleARPaymentChqSts(string number)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

            try
            {

                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("ARPayment", authToken.Tenant);

                ARPaymentQueryService Service = new ARPaymentQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                ARPayment aRPayment = new ARPayment();
                if (!string.IsNullOrEmpty(number))
                {
                    aRPayment = Service.GetARPaymentByNumber(number, tenant);
                }

                ARPaymentChqSts aRPaymentChqStsResult_onlyPay = new ARPaymentChqSts()
                {
                    PaymentId = aRPayment != null ? aRPayment.Id : "",
                    PaymentChequeId = "",
                    ChequeNumber = "",
                    BankAccount = "",
                    StatusCode = ""
                };

                List<ARPaymentChqSts> Result = new List<ARPaymentChqSts>();

                if (aRPayment != null && !String.IsNullOrEmpty(aRPayment.Id))
                {
                    Logitude.Accounting.BL.EntityQueryServices.ARPaymentChequeQueryService aRPaymentChequeQueryService = new Logitude.Accounting.BL.EntityQueryServices.ARPaymentChequeQueryService(tenant);

                    List<ARPaymentChequePM> aRPaymentCheques = aRPaymentChequeQueryService.GetARPaymentChequesBySinglePaymentId(aRPayment.Id, tenant);


                    ARPaymentChqSts aRPaymentChqStsResult = new ARPaymentChqSts();

                    if (aRPaymentCheques == null || aRPaymentCheques.Count == 0)
                    {
                        Result.Add(aRPaymentChqStsResult_onlyPay);
                    }
                    else
                    {
                        foreach (ARPaymentChequePM aRPaymentCheque in aRPaymentCheques)
                        {
                            ARPaymentChqSts aRPaymentChqSts = new ARPaymentChqSts()
                            {
                                PaymentId = aRPaymentCheque.PaymentId,
                                PaymentChequeId = aRPaymentCheque.Id,
                                ChequeNumber = aRPaymentCheque.ChequeNumber,
                                BankAccount = aRPaymentCheque.BankAccount,
                                StatusCode = aRPaymentCheque.StatusCode,
                            };
                            Result.Add(aRPaymentChqSts);
                        }
                        
                    }
                }
                else
                {
                    Result.Add(aRPaymentChqStsResult_onlyPay);
                }


                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                string exceptionMessage = ex.Message + Environment.NewLine + ex.StackTrace;
                if (ex.InnerException != null)
                {
                    exceptionMessage += Environment.NewLine + "inner1: " + ex.InnerException.Message + Environment.NewLine + ex.InnerException.StackTrace;
                }


                return Request.CreateResponse(HttpStatusCode.InternalServerError, exceptionMessage);
            }
        }

    }
}