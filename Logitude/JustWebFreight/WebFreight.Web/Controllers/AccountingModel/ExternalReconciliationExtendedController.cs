using Logitude.Accounting.Data;
using WebFreight.Web.Security;
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
using Logitude.Accounting.BL.CoreBL;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.BL.CoreBL.ExternalReconcile;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Def.EntityPMs;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class ExternalReconciliationExtendedController : ApiController
    {
        public ExternalReconciliationExtendedController()
        {

        }

        public HttpResponseMessage PostCreateJournalReconcileAdjustBankFee(
       List<string> reconcileExternalPageLineIdList,
       string TheAccountId,
       string AdjustAccountId,
       DateTime AccountDate,
       string Remarks

       )
        {
            try
            {
                JournalPM TheNewJournal=null;
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                using (var scope = TransactionFactory.GetTransaction())
                {
                    int tenant = authToken.Tenant;

                    var accountingContext = AccountingContext.GetContext(authToken.Tenant);
                    var externalReconcileAdjustBankFeesService = new ExternalReconcileAdjustBankFeesService();
                    var externalReconcileDataProvider = new ExternalReconcileDataProvider(accountingContext);
                    externalReconcileAdjustBankFeesService.MustInit(externalReconcileDataProvider);
                    externalReconcileAdjustBankFeesService
                        .CreateJournalWithExtReconcile(tenant, reconcileExternalPageLineIdList, AdjustAccountId, Remarks, AccountDate);

                    TheNewJournal = externalReconcileAdjustBankFeesService.TheNewJournal;
                    var JournalUP = new JournalUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                    JournalUP.Update(TheNewJournal, true);

                    scope.Complete();

                }
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, TheNewJournal);


                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }
    }
}