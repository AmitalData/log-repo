using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.Accounting.BL.Utils;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    //[RoutePrefix("api/Redempt_a_Cheque")]
    public class Redempt_a_ChequeController : ApiController
    {
        public Redempt_a_ChequeController()
        {

        }


        public HttpResponseMessage GetRedempt_a_Cheque(int tenant, string aRChequeId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                Redempt_a_ChequeArg args = null;
                string message = "";
                bool isSuccess = CreateArgs(tenant, aRChequeId,  ref args, message);
                if (!isSuccess)
                {
                    throw new Exception(message);
                }


                    Redempt_a_ChequeBatch Redempt_a_ChequeBatch = new Redempt_a_ChequeBatch();
                    Redempt_a_ChequeArg Redempt_a_ChequeArg = new Redempt_a_ChequeArg()
                    {
                        Tenant = tenant,
                        ARChequeId = args.ARChequeId,
                    };
                    Redempt_a_ChequeBatch.RunRedempt_a_Cheque(Redempt_a_ChequeArg);
                    string responseText = Redempt_a_ChequeBatch.ResponseText();
                    HttpStatusCode StatusCode = Redempt_a_ChequeBatch.StatusCode();
                    var res1 = new { Success = true, Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private bool CreateArgs(int tenant, string aRChequeId, ref Redempt_a_ChequeArg args, string message)
        {
            bool isSuccess = false;
            if (String.IsNullOrWhiteSpace(aRChequeId))
            {
                message = "ARChequeId is a must";
                return isSuccess;
            }


            IAccountingContext accContext = AccountingContext.GetContext(tenant);
            ARPaymentChequeQueryService aPaymentChequeQueryService = new ARPaymentChequeQueryService(accContext);




            var chq = aPaymentChequeQueryService.GetSingle(aRChequeId, false, false);
            if (chq == null)
            {
                message = "Cheque {aRChequeId} not found";
                return isSuccess;
            }


            args = new Redempt_a_ChequeArg()
            {
                Tenant = tenant,
                ARChequeId = aRChequeId,
            };
            isSuccess = true;
            return isSuccess;
        }


    }
}