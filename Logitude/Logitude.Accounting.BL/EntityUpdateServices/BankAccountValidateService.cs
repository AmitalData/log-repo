using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.BL.Security;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.Resolvers;
using Microsoft.Practices.Unity;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public class BankAccountValidateService: EntityValidateService<BankAccountPM>, IBankAccountValidateService
    {
        private IAccountingContext _MainContext;
        public BankAccountValidateService(IAccountingContext mainContext)
        {
            _MainContext = mainContext;
        }

        public override void Validate(BankAccountPM entityPM)
        {
            bool useLocal = true;
            ContactPM currenctUser = GetLoggedContact(entityPM.Tenant);
            if (currenctUser != null)
                useLocal = !currenctUser.DontShowLocal;


            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                CheckBankAccountExists(entityPM);
                if (ErrorsList.Count == 0)
                {
                    CheckGLAccountAlreadyConnectedToBankAccountOnInsert(entityPM, useLocal);
                }
                else
                {
                    return;
                }
                if (ErrorsList.Count == 0)
                {
                    CheckDeferredGLAccountAlreadyConnectedToBankAccountOnInsert(entityPM);
                }
                else
                {
                    return;
                }
            }

            if (entityPM.ChangeSetOp == ChangeSetOperation.Update)
            {
                BankAccount poco = GetSingleBankAccount(entityPM.Id, entityPM.Tenant);

                CheckGLAccountAlreadyConnectedToBankAccountOnUpdate(entityPM, poco, useLocal);
                if (ErrorsList.Count == 0)
                {
                    CheckDeferredGLAccountAlreadyConnectedToBankAccountOnUpdate(entityPM, poco);
                }
                else
                {
                    return;
                }
                if (ErrorsList.Count == 0)
                {
                    CheckGLAccountHasTransactionsOnUpdate(entityPM, poco, useLocal);
                }
                else
                {
                    return;
                }
                if (ErrorsList.Count == 0)
                {
                    CheckDefferedGLAccountTransactionsOnUpdate(entityPM, poco, useLocal);
                }
                else
                {
                    return;
                }
            }
           
        }

        public virtual void CheckBankAccountExists(BankAccountPM entityPM)
        {
            BankAccountList account = GetUniqueAccount(entityPM.AccountNumber, entityPM.BranchNumber, entityPM.BankId, entityPM.Tenant);
            ValidationResult result;
            if (account != null)
            {
                result = new ValidationResult("Bank account exist");
                AddValidationError(result.ErrorMessage);
                //throw new ApplicationException(result.ErrorMessage);
            }
        }

        public virtual void CheckGLAccountAlreadyConnectedToBankAccountOnInsert(BankAccountPM entityPM,bool useLocal)
        {
            ValidationResult result;
            if (!String.IsNullOrEmpty(entityPM.GLAccountId))
            {
                BankAccountList res = GetByGLAccount(entityPM.GLAccountId, entityPM.Tenant);
                if (res != null)
                {
                    result = new ValidationResult(GetMessageTranslation("Accounting.General.O.GLAccountAlreadyConnectedToBankAccount", entityPM.Tenant, useLocal));
                    AddValidationError(result.ErrorMessage);
                    // throw new ApplicationException(result.ErrorMessage);
                }
            }
        }

        public virtual void CheckDeferredGLAccountAlreadyConnectedToBankAccountOnInsert(BankAccountPM entityPM)
        {
            ValidationResult result;
            if (!String.IsNullOrEmpty(entityPM.DeferredGLAccountId))
            {
                BankAccountList res = GetByDeferedGLAccount(entityPM.DeferredGLAccountId, entityPM.Tenant);

                if (res != null)
                {
                    result = new ValidationResult("The Deferred GLAccount is already connected to a Bank Account");
                    AddValidationError(result.ErrorMessage);
                    //throw new ApplicationException(result.ErrorMessage);
                }
            }
        }

        public virtual void CheckGLAccountAlreadyConnectedToBankAccountOnUpdate(BankAccountPM entityPM,BankAccount poco, bool useLocal) {
            
            ValidationResult result;
            if (!String.IsNullOrEmpty(entityPM.GLAccountId) && entityPM.GLAccountId != poco.GLAccountId)
            {
                BankAccountList res = GetByGLAccount(entityPM.GLAccountId, entityPM.Tenant);
                if (res != null)
                {
                    result = new ValidationResult(GetMessageTranslation("Accounting.General.O.GLAccountAlreadyConnectedToBankAccount", entityPM.Tenant, useLocal));
                    AddValidationError(result.ErrorMessage);
                    //throw new ApplicationException(result.ErrorMessage);
                }
            }
        }

        public virtual void CheckDeferredGLAccountAlreadyConnectedToBankAccountOnUpdate(BankAccountPM entityPM, BankAccount poco)
        {
            ValidationResult result;
            if (!String.IsNullOrEmpty(entityPM.DeferredGLAccountId) && entityPM.DeferredGLAccountId != poco.DeferredGLAccountId)
            {
                BankAccountList res = GetByDeferedGLAccount(entityPM.DeferredGLAccountId, entityPM.Tenant);

                if (res != null)
                {
                    result = new ValidationResult("The Deferred GLAccount is already connected to a Bank Account");
                    AddValidationError(result.ErrorMessage);
                    //throw new ApplicationException(result.ErrorMessage);
                }
            }
        }

        public virtual void CheckGLAccountHasTransactionsOnUpdate(BankAccountPM entityPM,BankAccount entityPOCO, bool useLocal)
        {
            if (entityPM.GLAccountId != entityPOCO.GLAccountId)
            {
                // GLAccount changed
                // check glaccount transactions
                List<LedgerTransaction> transactions = GetLedgerTransactions(entityPOCO.GLAccountId, entityPOCO.Tenant);
                if (transactions.Count > 0)
                {
                    string message = GetMessageTranslation("Accounting.O.ThereRTransactions4GLAccountCantUpdated", entityPM.Tenant, useLocal);
                    AddValidationError(message);
                    //throw new ApplicationException(message);
                }
            }
        }

        public virtual void CheckDefferedGLAccountTransactionsOnUpdate(BankAccountPM entityPM, BankAccount entityPOCO, bool useLocal)
        {
            if (entityPM.DeferredGLAccountId != entityPOCO.DeferredGLAccountId)
            {
                // Deferred GLAccount changed
                // check glaccount transactions
                List<LedgerTransaction> transactions = GetLedgerTransactions(entityPOCO.DeferredGLAccountId, entityPOCO.Tenant);
                if (transactions.Count > 0)
                {
                    string message = GetMessageTranslation("Accounting.O.ThereRTransactions4GLAccountCantUpdated", entityPM.Tenant, useLocal);
                    AddValidationError(message);
                    //throw new ApplicationException(message);
                }
            }
        }

        public virtual Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }


        public virtual ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


        public virtual string GetMessageTranslation(string code, int tenant, bool useLocal)
        {
            return TranslateTextsClassUtilResolver.Translate(code, tenant, useLocal);//TranslateTextsClass.Translate(code, tenant, useLocal);
        }

        public virtual BankAccountList GetUniqueAccount(string accountNumber, string branchNumber, string bankId, int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            BankAccountListQueryService query = new BankAccountListQueryService(MyContext);
            BankAccountList account = query.GetUniqueAccount(accountNumber, branchNumber, bankId, tenant);
            return account;
        }

        public virtual BankAccountList GetByGLAccount(string gLAccountId,int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            BankAccountListQueryService query = new BankAccountListQueryService(MyContext);
            BankAccountList account = query.GetByGLAccount(gLAccountId);
            return account;
        }

        public virtual BankAccountList GetByDeferedGLAccount(string deferedGLAccountId, int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            BankAccountListQueryService query = new BankAccountListQueryService(MyContext);
            BankAccountList account = query.GetByDeferedGLAccount(deferedGLAccountId);
            return account;
        }

        public virtual BankAccount GetSingleBankAccount(string id,int tenant)
        {
            BankAccountRepository repo = new BankAccountRepository(tenant);
            BankAccount poco = repo.GetSingle(id, tenant);
            return poco;
        }

        public virtual List<LedgerTransaction> GetLedgerTransactions(string glAccountId, int tenant)
        {
            LedgerTransactionRepository transactionsRepo = new LedgerTransactionRepository(tenant);
            List<LedgerTransaction> transactions = transactionsRepo.GetByAccountId(glAccountId, tenant);
            return transactions;
        }
    }

    public interface IBankAccountValidateService
    {
        void Validate(BankAccountPM entityPM);
        ContactPM GetLoggedContact(int tenant);
        string GetMessageTranslation(string code, int tenant, bool useLocal);
        BankAccountList GetUniqueAccount(string accountNumber, string branchNumber, string bankId, int tenant);
        BankAccountList GetByGLAccount(string glAccountId, int tenant);
        BankAccountList GetByDeferedGLAccount(string deferredGLAccountId, int tenant);
        BankAccount GetSingleBankAccount(string id, int tenant);
        void CheckBankAccountExists(BankAccountPM entityPM);
        void CheckGLAccountAlreadyConnectedToBankAccountOnInsert(BankAccountPM entityPM, bool useLocal);
        void CheckDeferredGLAccountAlreadyConnectedToBankAccountOnInsert(BankAccountPM entityPM);
        void CheckGLAccountAlreadyConnectedToBankAccountOnUpdate(BankAccountPM entityPM, BankAccount poco, bool useLocal);
        void CheckDeferredGLAccountAlreadyConnectedToBankAccountOnUpdate(BankAccountPM entityPM, BankAccount poco);

    }

}
