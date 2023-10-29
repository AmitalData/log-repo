
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.CoreBL
{
    class GLAccountChequesTotalCalculator
    {
        int tenant;
        DateTime endOfTodayDate;
        GLAccountPM _mainCardGLA = new GLAccountPM();
        List<GLAccountMoreDataPM> _glaccountMoreDatas = new List<GLAccountMoreDataPM>();
        GLAccountMoreDataPM _mainGlaccountMoreData = new GLAccountMoreDataPM();
        List<string> _splittedGlaccountIds = new List<string>();

        public GLAccountChequesTotalCalculator(int tenant)
        {
            this.tenant = tenant;
            endOfTodayDate = TenantServerConfigration.GetEndOfTodayDate(tenant);
        }

        public void RecalculateChequesTotalForBillToAccount(string billToAccountId)
        {
            _mainCardGLA = GetCardGLAccount(billToAccountId, tenant);
            _glaccountMoreDatas = GetGLAccountMoreDataConnectedToBillToAccount(tenant);
            ResetChequesTotals(_glaccountMoreDatas);
            LedgerTransactionQueryService transactionsQuery = new LedgerTransactionQueryService(tenant);
            foreach (var glMoreData in _glaccountMoreDatas)
            {
                var totalOpenChequesInLocalCur = transactionsQuery.GetTotalOpenChequesLocalAmount(glMoreData.AccountId) ?? 0;
                var totFutureOpenChequesInLocalCur = transactionsQuery.GetTotFutureOpenChequesLocalAmount(glMoreData.AccountId, endOfTodayDate) ?? 0;
                glMoreData.TotalOpenChequesInLocalCur = totalOpenChequesInLocalCur;
                glMoreData.TotFutureOpenChequesInLocalCur = totFutureOpenChequesInLocalCur;
                SubmiGLAccountMoreData(tenant, glMoreData);
            }
        }

        private void ResetChequesTotals(List<GLAccountMoreDataPM> glaccountMoreDatas)
        {

            _mainGlaccountMoreData.TotFutureOpenChequesInLocalCur = 0;
            _mainGlaccountMoreData.TotalOpenChequesInLocalCur = 0;
            for (int i = 0; i < glaccountMoreDatas.Count; i++)
            {
                glaccountMoreDatas[i].TotFutureOpenChequesInLocalCur = 0;
                glaccountMoreDatas[i].TotalOpenChequesInLocalCur = 0;
            }
        }
        private List<GLAccountMoreDataPM> GetGLAccountMoreDataConnectedToBillToAccount(int tenant)
        {

            _mainGlaccountMoreData = GetGLAccountMoreDataPM(tenant, _mainCardGLA.Id);
            _glaccountMoreDatas.Add(_mainGlaccountMoreData);
            foreach (var splittedGLAId in _splittedGlaccountIds)
            {
                _glaccountMoreDatas.Add(GetGLAccountMoreDataPM(tenant, splittedGLAId));
            }
            return _glaccountMoreDatas;
        }

        private GLAccountPM GetCardGLAccount(string billToId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(billToId, tenant);
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, tenant);

                var gLAccountCurrencyQueryService = new GLAccountCurrencyQueryService(MyContext);
                var glAccountCurrencies = gLAccountCurrencyQueryService.GetRelatedCurrenciesAccounts(tenant, glaAccount.Id).ToList();
                if (glAccountCurrencies.Count > 0)
                {
                    _splittedGlaccountIds = glAccountCurrencies;
                }

            }
            return glaAccount;
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

    }
}
