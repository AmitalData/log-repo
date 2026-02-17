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

using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using WebFreight.Web.DataContracts;

using WebFreight.Web.AccountingModel.DomainServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{
    public partial class PaymentChequeViewsController : ApiController
    {
        public HttpResponseMessage GetPymentChequesSummary()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                AccountingDomainService domain = new AccountingDomainService();
                PaymentChequeSummary myResult = domain.GetPaymentChequeSummary(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        //public HttpResponseMessage GetCheckIfPaymentChequeExists(string BankAccountId, string ChequeNumber)
        //{
        //    try
        //    {
        //        try
        //        {
        //            string token = HttpContext.Current.Request.Headers["Token"];
        //            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
        //            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
        //            IAccountingContext MyContext = AccountingContext.GetContext(authToken.Tenant);
        //            PaymentChequeQueryService paymentChequeQueryService = new PaymentChequeQueryService(MyContext);
        //            if (ChequeNumber == "undefined") ChequeNumber = null;
        //           bool exist = paymentChequeQueryService.CheckIfPaymentChequeExists(BankAccountId,ChequeNumber, authToken.Tenant);

        //            return Request.CreateResponse(HttpStatusCode.OK, exist);
        //        }
        //        catch (Exception ex)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
        //    }
        //}

    }
}