using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    class GLAccountChequesTotalCalculator
    {
        int tenant;
        DateTime endOfTodayDate;
        GLAccountPM _mainCardGLA = new GLAccountPM();
        List<GLAccountMoreDataPM> _glaccountMoreDatas =new List<GLAccountMoreDataPM>();
        GLAccountMoreDataPM _mainGlaccountMoreData = new GLAccountMoreDataPM();
        List<GLAccountPM> _splittedGlaccounts = new List<GLAccountPM>();

        public GLAccountChequesTotalCalculator(int tenant)
        {
            this.tenant = tenant;
            endOfTodayDate = TenantServerConfigration.GetEndOfTodayDate(tenant);
        }

        public void RecalculateChequesTotalForBillToAccount(string billToAccountId)
        {
            List<ARPayment> payments = GetBillToPayments(tenant, billToAccountId);
            var currenciesIds = payments.Select(x => x.PaymentCurrencyId).Distinct().ToList();
            _mainCardGLA = GetCardGLAccount(billToAccountId, tenant);
            IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
            if (_mainCardGLA.IsMultiCurrency == true) {
                foreach(var currencyId in currenciesIds) {
                    string splitByCurrencyAccountId = GetAccountIdForGLAccountCurrency(_mainCardGLA, currencyId);
                    var splittedGLA = glAccountQuery.GetSingleGLAccountPM(splitByCurrencyAccountId, tenant);
                    _splittedGlaccounts.Add(splittedGLA);
                }
            }
            _glaccountMoreDatas = GetGLAccountMoreDataConnectedToBillToAccount(tenant);
            List<ARPaymentChequePM> cheques = GetChequesOfPaymentBillToAccount(payments, tenant, billToAccountId);        
            ResetChequesTotals(_glaccountMoreDatas);

            foreach (ARPaymentChequePM cheque in cheques) {
                 if (_mainCardGLA.IsMultiCurrency == true)
                {
                    GLAccountMoreDataPM splittedGlaccount = getsplittedGlaccount(cheque.CurrencyId);
                    if(splittedGlaccount  !=  null)
                         AddChequeAmountToTotal(splittedGlaccount, cheque);
                    else
                        AddChequeAmountToTotal(_mainGlaccountMoreData, cheque);
                }
                else
                {
                    AddChequeAmountToTotal(_mainGlaccountMoreData, cheque);
                }

            }
                    

        }
        private GLAccountMoreDataPM getsplittedGlaccount(string currencyId)
        {
            foreach(var glaccountMoreData in _glaccountMoreDatas)
            {
                var splittedGlA = _splittedGlaccounts.Find(s => s.Id == glaccountMoreData.AccountId);
                if (splittedGlA.CurrencyId == currencyId)
                    return glaccountMoreData;
            }
            return null;
        }

        private void ResetChequesTotals(List<GLAccountMoreDataPM> glaccountMoreDatas)
        {
            for (int i = 0; i < glaccountMoreDatas.Count; i++)
            {
                glaccountMoreDatas[i].TotFutureOpenChequesInLocalCur = 0;
                glaccountMoreDatas[i].TotalOpenChequesInLocalCur = 0;

            }
        }

        private void AddChequeAmountToTotal(GLAccountMoreDataPM glaccountMoreData, ARPaymentChequePM cheque)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            LedgerTransactionListQueryService ledgerQuery = new LedgerTransactionListQueryService(MyContext);
            if (cheque.StatusCode != ARPaymentChequeStatusValues.Redeemed && cheque.StatusCode != ARPaymentChequeStatusValues.ReturnedToCustomer)
            {
                if (cheque.ValueDate > endOfTodayDate)
                    glaccountMoreData.TotFutureOpenChequesInLocalCur += cheque.LocalAmount;
                else
                    glaccountMoreData.TotalOpenChequesInLocalCur += cheque.LocalAmount;
            }
            var externalTransactions = ledgerQuery.GetExternalTransactionsForAccount(glaccountMoreData.AccountId, tenant).ToList();
            var externalTransactionsTotal = externalTransactions.Sum(d => d.LocalAmountCredit);
            glaccountMoreData.TotFutureOpenChequesInLocalCur += externalTransactionsTotal;
            SubmiGLAccountMoreData(tenant, glaccountMoreData);
        }

        private List<GLAccountMoreDataPM> GetGLAccountMoreDataConnectedToBillToAccount(int tenant)
        {

            _mainGlaccountMoreData = GetGLAccountMoreDataPM(tenant, _mainCardGLA.Id);
            foreach (var splittedGLA in _splittedGlaccounts) {
                _glaccountMoreDatas.Add(GetGLAccountMoreDataPM(tenant, splittedGLA.Id));
            }
            return _glaccountMoreDatas;
        }

        private List<ARPaymentChequePM> GetChequesOfPaymentBillToAccount(List<ARPayment> payments, int tenant, string billToAccountId)
        {
            var paymentIds = payments.Select(d => d.Id).ToList();

            List<ARPaymentChequePM> cheques = GetChequesOfPayments(tenant, paymentIds);
            return cheques;
        }

        private GLAccountPM GetCardGLAccount(string billToId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(billToId, tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, tenant);
            }
            return glaAccount;
        }

        private GLAccountPM GetGLAccount(string billToId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(billToId, tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.Id, tenant);

                if (glaAccount != null && glaAccount.IsMultiCurrency.Value)
                {
                    string splitByCurrencyAccountId = GetAccountIdForGLAccountCurrency(glaAccount, glaAccount.CurrencyId);//payment CurrencyId
                    glaAccount = glAccountQuery.GetSingleGLAccountPM(splitByCurrencyAccountId, tenant);
                }
                else return glaAccount;
            }


            return glaAccount;
        }
        private string GetAccountIdForGLAccountCurrency(GLAccountPM gLAccount, string paymentCurrencyId)
        {
            GLAccountCurrencyRepository glAccountCurrencyRepository = new GLAccountCurrencyRepository(gLAccount.Tenant);
            GLAccountCurrency gLAccountCurrency = glAccountCurrencyRepository.GetEntityByCurrencyAndGLAccountId(gLAccount.Id, paymentCurrencyId, gLAccount.Tenant);
            if (gLAccountCurrency != null)
            {
                return gLAccountCurrency.GLAccountId;
            }
            else return gLAccount.Id;

        }


        private void SubmiGLAccountMoreData(int tenant, GLAccountMoreDataPM glaccountMoreDataPM)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            GLAccountMoreDataUpdateService updateService = new GLAccountMoreDataUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);

            glaccountMoreDataPM.ChangeSetOp = ChangeSetOperation.Update;
            updateService.Update(glaccountMoreDataPM, true);
        }

        private GLAccountMoreDataPM GetGLAccountMoreDataPM(int tenant, string GLAccountId)
        {
            GLAccountMoreDataQueryService moreDataQueryService = new GLAccountMoreDataQueryService(tenant);
            GLAccountMoreDataPM moreDataPM = moreDataQueryService.GetSingle(GLAccountId, false, false);
            return moreDataPM;
        }

        private string GetBillToGLAccountId(int tenant, string billTo)
        {
            CardRepository cardRepo = new CardRepository(tenant);
            string GLAccountId = cardRepo.GetGLAccountIdByCardId(billTo, tenant);
            return GLAccountId;
        }

        private List<ARPaymentChequePM> GetChequesOfPayments(int tenant, List<string> paymentIds)
        {
            ARPaymentChequeQueryService queryService = new ARPaymentChequeQueryService(tenant);
            List<ARPaymentChequePM> aRPaymentChequePMs = queryService.GetARPaymentChequesByPaymentIds(paymentIds, tenant);
            return aRPaymentChequePMs;
        }

        private List<ARPayment> GetBillToPayments(int tenant, string paymentBillToId)
        {
            ARPaymentRepository repo = new ARPaymentRepository(tenant);
            List<ARPayment> payments = repo.GetARPaymentsByBillTo(paymentBillToId, tenant);
            return payments;
        }

    }
}
