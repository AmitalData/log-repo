using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.DataContract;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.Utils
{
    public class FutureOpenChequesBatch
    {

        private string _ResponseText;
        private HttpStatusCode _StatusCode;

        public FutureOpenChequesBatch()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }

        public void SetTotalFutureOpenChequesInLocalCurrency()
        {
            List<GlobalTenant> globalTenants;
            using (var scope = TransactionFactory.GetNewTransaction())
            {
                globalTenants = GlobalTenantRepository.GetGlobalTenants();
                scope.Complete();
            }

            using (var scope = TransactionFactory.GetNewTransaction())
            {
                if (globalTenants != null)
                {
                    GlobalTenant tenantZero = globalTenants.Where(d => d.Id == 0).FirstOrDefault();
                    List<GlobalTenant> upgradableTenants = (from a in globalTenants
                                                            where a.IsActive == true
                                                            select a).ToList();
                    if (upgradableTenants.Count > 0)
                    {
                        foreach (GlobalTenant tenant in upgradableTenants)
                        {
                            try
                            {

                                FullAccountingSettingQueryService fullAccountingSettingQueryService = new FullAccountingSettingQueryService(tenant.Id);
                                FullAccountingSettingPM fullAccountingSettingPM = fullAccountingSettingQueryService.GetSingleFullAccountingSetting(tenant.Id);
                                if (fullAccountingSettingPM != null && fullAccountingSettingPM.AccountingActivated)
                                {
                                    List<ARPaymentChequePM> aRPaymentCheques = null;
                                    ARPaymentChequeQueryService queryService = new ARPaymentChequeQueryService(tenant.Id);
                                    aRPaymentCheques = queryService.GetOpenARPaymentCheques(tenant.Id);
                                    //ARPaymentRepository repo = new ARPaymentRepository(tenant.Id);
                                    //CardRepository cardRepo = new CardRepository(tenant.Id);
                                    GLAccountMoreDataQueryService moreDataQueryService = new GLAccountMoreDataQueryService(tenant.Id);
                                    //List<string> paymentIds = new List<string>();
                                    //paymentIds = aRPaymentCheques.Where(d=> d.PaymentId != null).Select(d => d.PaymentId).ToList();
                                    //List<string> cardIds = repo.GetCardIdsFromPayments(paymentIds, tenant.Id);
                                    //List<string> glAccountIds = cardRepo.GetGLAccountIdssByCardIds(cardIds, tenant.Id);
                                    //List<GLAccountMoreDataPM> gLAccountMoreDataPMs = moreDataQueryService.GetByGLAccountsIdList(glAccountIds, tenant.Id);
                                    IAccountingContext MyContext = AccountingContext.GetContext(tenant.Id);
                                    GLAccountMoreDataUpdateService updateService = new GLAccountMoreDataUpdateService(MyContext, new Dictionary<string, IContext>(), tenant.Id);
                                    List<ARPaymentChequeFutureData> data = moreDataQueryService.GetARPaymentChequeFutureData(tenant.Id);
                                    LedgerTransactionListQueryService ledgerQuery = new LedgerTransactionListQueryService(MyContext);
                                    List<string> glAccountIds = new List<string>();
                                    foreach (ARPaymentChequeFutureData item in data)
                                    {

                                        if (glAccountIds.Contains(item.GLAccountId))
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            glAccountIds.Add(item.GLAccountId);
                                            GLAccountMoreDataPM moreDataPM = moreDataQueryService.GetSingle(item.GLAccountId, false, false);
                                            if (moreDataPM != null)
                                            {
                                                moreDataPM.TotFutureOpenChequesInLocalCur = 0;
                                                if (item.PaymentId != null)
                                                {
                                                    moreDataPM.TotalOpenChequesInLocalCur = 0;
                                                    List<string> paymentIds = data.Where(d => d.GLAccountId == item.GLAccountId).Select(d => d.PaymentId).ToList();

                                                    List<ARPaymentChequePM> aRPaymentChequePMs = (from a in aRPaymentCheques
                                                                                                  join j in MyContext.Journals 
                                                                                                  on a.PaymentId equals j.AccountingEntityId
                                                                                                  join transaction in MyContext.LedgerTransactions 
                                                                                                  on j.Id equals transaction.JournalId
                                                                                                  where paymentIds.Contains(a.PaymentId) && transaction.Reference2 == a.ChequeNumber
                                                                                                  select a).Distinct().ToList();


                                                    foreach (ARPaymentChequePM paymentCheque in aRPaymentChequePMs)
                                                    {
                                                        if (moreDataPM.TotalOpenChequesInLocalCur == null) moreDataPM.TotalOpenChequesInLocalCur = 0;
                                                        if (moreDataPM.TotFutureOpenChequesInLocalCur == null) moreDataPM.TotFutureOpenChequesInLocalCur = 0;
                                                        if (paymentCheque.StatusCode != ARPaymentChequeStatusValues.Redeemed && paymentCheque.StatusCode != ARPaymentChequeStatusValues.ReturnedToCustomer)
                                                        {
                                                            if (paymentCheque.ValueDate > TenantServerConfigration.GetCurrentDateTime(tenant.Id))
                                                            {
                                                                moreDataPM.TotFutureOpenChequesInLocalCur += paymentCheque.LocalAmount;
                                                            }
                                                            else
                                                            {
                                                                moreDataPM.TotalOpenChequesInLocalCur += paymentCheque.LocalAmount;
                                                            }
                                                        }
                                                    }
                                                }
                                                var externalTransactions = ledgerQuery.GetExternalTransactionsForAccount(item.GLAccountId, tenant.Id).ToList();
                                                var externalTransactionsTotal = externalTransactions.Sum(d => d.LocalAmountCredit);
                                                moreDataPM.TotFutureOpenChequesInLocalCur += externalTransactionsTotal;
                                                moreDataPM.ChangeSetOp = ChangeSetOperation.Update;
                                                updateService.Update(moreDataPM, true);
                                            }
                                        }


                                    }


                                }

                            }

                            catch (Exception ex)
                            {

                            }

                        }



                    }
                }

                scope.Complete();
            }

        }



    }
}

