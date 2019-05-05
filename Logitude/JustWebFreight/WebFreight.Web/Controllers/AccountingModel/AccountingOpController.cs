
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class AccountingOpController : ApiController
    {
        public AccountingOpController()
        {

        }
        public HttpResponseMessage GetGenerate1000(string email)
        {
            try
            {
                
                using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(1)))
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Journal", "UPDATE", authToken.Tenant);



                    //entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    //service.Update(entityPM, true);
                    string message = "NO Vendor Cards Having Deduction";
                    var accountingContext = AccountingContext.GetContext(authToken.Tenant);
                    ISystem1000Service System1000Service = new System1000Service();
                    var flatFiles = System1000Service.GetSystem1000FlatFile(accountingContext, authToken.Tenant);
                    if (flatFiles.Count > 0)
                    {
                        System1000Service.EmailIt(email, flatFiles, authToken.Tenant);
                        message = "System 1000 Flat File have sent ";
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, new { Message = message });
                }
            }




            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}