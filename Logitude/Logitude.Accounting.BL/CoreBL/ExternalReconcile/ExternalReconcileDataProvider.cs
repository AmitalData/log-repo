using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile
{
    public class ExternalReconcileDataProvider : IExternalReconcileDataProvider
    {
        private IAccountingContext _AccountingContext;
        private IWebFreightContext _IWebFreightContext;
        public ExternalReconcileDataProvider(IAccountingContext accountingContext)
        {
            _AccountingContext = accountingContext;

        }

        private User GetLoggedUser(int tenant)
        {
            string email = AuthenticationUtil.GetLoggedUserEmail(tenant);
            UserRepository userRepository = new UserRepository(tenant);
            var loggedUser = userRepository.GetSingleUserByEmail(email, tenant, true);
            return loggedUser;
        }

        public virtual List<LedgerTransactionPM> GetLedgerTransactionToReconcile(List<string> theReconcileAgainstLTranIdList, int tenant)
        {
            var qs = new LedgerTransactionQueryService(_AccountingContext);

            var myOldTransToReconcile = qs.GetLedgerTransactionPMsByIdList(theReconcileAgainstLTranIdList, tenant);
            return myOldTransToReconcile;

        }

        public virtual BankAccountPM GetBankAccountFromTransferAccount(string myLedgerTransactionTransferInCreditAccountId, int tenant)
        {
            var bankAccountQS = new BankAccountQueryService(this._AccountingContext);
            var bankAccountPM = bankAccountQS.GetBankAccountByTransferGLAcccountId(myLedgerTransactionTransferInCreditAccountId, tenant);
            if (bankAccountPM == null)
            {
                throw new Exception("could not found bank from myLedgerTransactionTransferInCredit.id ");
            }

            return bankAccountPM;
        }


        public virtual BankAccountPM GetBankAccountFromReconcileExternalPageLineId(string reconcileExternalPageLineId, int tenant)
        {
            var bankPageLineQS = new ReconcileExternalPageLineRepository(this._AccountingContext);
            var reconcileExternalPageLinePM = bankPageLineQS.GetSingle(reconcileExternalPageLineId, tenant);
            if (reconcileExternalPageLinePM == null)
            {
                throw new Exception("bankPageLine is null");
            }
            var bankPageQS = new ReconcileExternalPageRepository(this._AccountingContext);
            var page = bankPageQS.GetSingle(reconcileExternalPageLinePM.ReconcileExternalPageId, tenant);
            var bankAccountQS = new BankAccountQueryService(this._AccountingContext);
            var bankAccount = bankAccountQS.GetSingle(page.EntityId, false, false);
            return bankAccount;

        }
        public virtual ReconcileExternalPageLinePM GetReconcileExternalPageLinePM(int tenant, string reconcileExternalPageLineId)
        {
            var qs = new ReconcileExternalPageLineQueryService(this._AccountingContext);
            var PM = qs.GetSingle(reconcileExternalPageLineId, false, false);
            return PM;
        }
        public virtual string ResolveUserId(int tenant)
        {
            return AuthenticationUtil.ResolveUserId(tenant); ;
        }

        public virtual DateTime GetCurrentDateTime(int tenant)
        {
            return TenantServerConfigration.GetCurrentDateTime(tenant);
        }
        public virtual List<LedgerTransactionPM> GetLedgerTransactionList(List<string> theReconcileAgainstLTranIdList, int tenant)
        {
            var qs = new LedgerTransactionQueryService(_AccountingContext);

            var myOldTransToReconcile = qs.GetLedgerTransactionPMsByIdList(theReconcileAgainstLTranIdList, tenant);
            return myOldTransToReconcile;
        }

        public virtual List<ReconcileExternalPageLineList> GetReconcileExternalPageLineList(int tenant, List<string> reconcileExternalPageLineIdList)
        {


            var qs = new ReconcileExternalPageLineListQueryService(_AccountingContext);
            IQueryable<ReconcileExternalPageLineList> query2 = qs.GetIqueryableList(_AccountingContext.ReconcileExternalPageLines.Where(r=>r.Tenant== tenant));
            
            var list = query2.Where(r => reconcileExternalPageLineIdList.Contains(r.Id)).ToList();
            return list;
        }
        public virtual List<ReconcileExternalPageList> GetReconcileExternalPageList(int tenant, List<string> reconcileExternalPageIdList)
        {
            var qs = new ReconcileExternalPageListQueryService(_AccountingContext);

            IQueryable<ReconcileExternalPageList> q = qs.GetIqueryableList(_AccountingContext.ReconcileExternalPages.Where(r => r.Tenant == tenant));

            var list = q.Where(r => reconcileExternalPageIdList.Contains(r.Id)).ToList();
            return list;
        }
        
        public virtual LastRate GetLastRateByValueDate(int tenant, string foreignCurrencyId, string baseCurrencyId, DateTime valueDate)
        {
            if (_IWebFreightContext == null)
            {
                _IWebFreightContext = WebFreightContext.GetContext(tenant);
            }

            var ratesTablesRepository = new RatesTableRepository(_IWebFreightContext);
            var ratesTableQuery = new RatesTableQuery(ratesTablesRepository);

            LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, foreignCurrencyId, baseCurrencyId, valueDate);
            return lastRate;
        }
        public virtual string GetaccountingCurrencyId(int tenant)
        {
            var tenantQuery = new TenantQuery(tenant);
            var tPM = tenantQuery.GetSinglePM(tenant);
            string accountingCurrencyId = tPM.CurrencyId;
            return accountingCurrencyId;
        }

        public virtual List<GLAccountList> GetListOfGLAccountList(int tenant, List<string> listOfAccId)
        {
            var qs = new GLAccountListQueryService(_AccountingContext);
            var q = qs.GetIqueryableList(_AccountingContext.GLAccounts.Where(r => r.Tenant == tenant),GetLoggedUser(tenant));
            var listOfGLAccountList =q.Where(r => listOfAccId.Contains(r.Id)).ToList();

            return listOfGLAccountList;
        }
    }
}
