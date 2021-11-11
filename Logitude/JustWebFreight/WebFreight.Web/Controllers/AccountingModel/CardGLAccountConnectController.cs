using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using Logitude.Accounting.BL.Utils;
using System.Net;
using WebFreight.Web.Helpers;
using System.Text.RegularExpressions;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.CoreBL.Batch;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using WebFreight.Web.Security;
using Simplog.Data.CommonDataModel.Repositories;

namespace WebFreight.Web.Controllers.AccountingModel
{
    //[RoutePrefix("api/CardGLAccountConnect")]
    public class CardGLAccountConnectController : ApiController
    {
        public CardGLAccountConnectController()
        {

        }


        public HttpResponseMessage GetCardGLAccountConnect(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.AuthenticationOnTenant(tenant);
                bool batchIt = true;
                if (batchIt)
                {

                    var accountingContext = AccountingContext.GetContext(tenant);

                    var myBatchCardGLAccountConnectTask = new BatchCardGLAccountConnectTask(null);
                    string subj = $"Card GLAccount Connect";
                    var batchTaskId = myBatchCardGLAccountConnectTask.CreateQBatchTaskExecution<CardGLAccountConnectArg>(
                        new CardGLAccountConnectArg()
                        {
                            Tenant = tenant,
                        }, tenant, subj, false);


                    var res1 = new { Success = true, Message = $"Send to Batch Task {batchTaskId}" };
                    return Request.CreateResponse(HttpStatusCode.Accepted, res1);
                }
                else
                {
                    CardGLAccountConnectBatch cardGLAccountConnectBatch = new CardGLAccountConnectBatch();
                    CardGLAccountConnectArg cardGLAccountConnectArg = new CardGLAccountConnectArg()
                    {
                        Tenant = tenant,
                    };
                    cardGLAccountConnectBatch.RunCardGLAccountConnect(cardGLAccountConnectArg);
                    string responseText = cardGLAccountConnectBatch.ResponseText();
                    HttpStatusCode StatusCode = cardGLAccountConnectBatch.StatusCode();
                    var res1 = new { Success = true, Message = responseText };

                    return Request.CreateResponse(StatusCode, res1);

                }
                }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


    }
}