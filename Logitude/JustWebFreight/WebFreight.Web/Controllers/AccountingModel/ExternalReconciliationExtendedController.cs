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
using Logitude.Accounting.BL.CoreBL.ExternalReconcile.Utils;

namespace WebFreight.Web.Controllers.AccountingModel
{
    public class ExternalReconciliationExtendedController : ApiController
    {
        public ExternalReconciliationExtendedController()
        {

        }
        public class CreateJournalReconcileAdjustBankFeeM
        {
            public List<string> LedgerTransactionIds { get; set; }
            public List<string> ReconcileExternalPageLineIdList { get; set; }
            
        }

        public HttpResponseMessage PostCreateJournalReconcileAdjustBankFee(
       //     List<string> ledgerTransactionIds,
       ///*List<*/string/*>*/ reconcileExternalPageLineId /*List*/,
       CreateJournalReconcileAdjustBankFeeM createJournalReconcileAdjustBankFeeM,
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
                    var externalReconcileDataProvider = new ExternalReconcileDataProvider(accountingContext);

                    var myExternalReconcileTypeService = new ExternalReconcileTypeService();
                    myExternalReconcileTypeService.MustInit(externalReconcileDataProvider);
                    ExternalReconcileType externalReconcileType= myExternalReconcileTypeService.GetExternalReconcileTypeFrom(tenant, createJournalReconcileAdjustBankFeeM.ReconcileExternalPageLineIdList,
                        createJournalReconcileAdjustBankFeeM.LedgerTransactionIds);

                    switch (externalReconcileType)
                    {
                        case ExternalReconcileType.MoveBankCheckFromTransferExternalReconcile:
                            var externalReconcileMoveBankCheckFromTransfer2GLAccountService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
                            externalReconcileMoveBankCheckFromTransfer2GLAccountService.MustInit(externalReconcileDataProvider);
                            externalReconcileMoveBankCheckFromTransfer2GLAccountService.OnAdjustMustInit(AdjustAccountId, Remarks);
                            externalReconcileMoveBankCheckFromTransfer2GLAccountService.CreateJournalWithExtReconcile(
                                tenant,
                                createJournalReconcileAdjustBankFeeM.LedgerTransactionIds/*.First()*/,
                                createJournalReconcileAdjustBankFeeM.ReconcileExternalPageLineIdList.First()
                                );
                            TheNewJournal = externalReconcileMoveBankCheckFromTransfer2GLAccountService.TheJournalPM;

                            break;
                        case ExternalReconcileType.AdjustExternalReconcile:
                            var externalReconcileAdjustBankFeesService = new ExternalReconcileAdjustBankFeesService();
                            externalReconcileAdjustBankFeesService.MustInit(externalReconcileDataProvider);
                            externalReconcileAdjustBankFeesService
                                .CreateJournalWithExtReconcile(tenant, createJournalReconcileAdjustBankFeeM.ReconcileExternalPageLineIdList, AdjustAccountId, Remarks, AccountDate, createJournalReconcileAdjustBankFeeM.LedgerTransactionIds);

                            TheNewJournal = externalReconcileAdjustBankFeesService.TheNewJournal;

                            break;
                    }



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