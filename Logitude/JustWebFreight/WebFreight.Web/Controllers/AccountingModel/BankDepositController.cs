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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using WebFreight.Web.AccountingModel.Reports.BankDeposit;
using System.Transactions;

namespace WebFreight.Web.App_Code.AngularJS_App_Code.Generated
{


    public partial class BankDepositController : ApiController
    {
        public HttpResponseMessage PostReturnCheque(string bankDepositId, string arpChequeId, string returnType, string notes)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    int tenant = authToken.Tenant;
                    var accountingContext = AccountingContext.GetContext(tenant);

                    BankDepositQueryService query = new BankDepositQueryService(accountingContext);
                    query.ReturnCheque(bankDepositId, arpChequeId, returnType, notes, tenant);


                    scope.Complete();

                    return Request.CreateResponse(HttpStatusCode.OK, "OK");
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        public HttpResponseMessage PostCancelDeposit(string bankDepositId)
        {

            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    if (authToken == null)
                        SecurityExceptionsThrower.ThrowNotAuthorized();
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    int tenant = authToken.Tenant;
                    var accountingContext = AccountingContext.GetContext(tenant);
                    BankDepositQueryService query = new BankDepositQueryService(accountingContext);
                    query.CancelDeposit(bankDepositId, tenant);

                    scope.Complete();

                    return Request.CreateResponse(HttpStatusCode.OK, "OK");
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
        public HttpResponseMessage GetSingleWithoutLines(string id)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Cashbook", "READ", authToken.Tenant);

                BankDepositPM bankDepositPM = GetBankDepositWithoutLines(id, authToken.Tenant);

                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, bankDepositPM);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

        private BankDepositPM GetBankDepositWithoutLines(string id, int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            BankDepositQueryService query = new BankDepositQueryService(MyContext);
            query.InitializeSettings();
            BankDepositPM bankDepositPM = query.GetSingle(id, false, false);
            return bankDepositPM;
        }
    }
}
	 