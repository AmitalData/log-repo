using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class JournalLineOnUpdate
    {
        private IAccountingContext _MainContext;

        public JournalLineOnUpdate(IAccountingContext mainContext)
        {
            this._MainContext = mainContext;
        }



        public virtual void OnUpdate(JournalLinePM journalLinePM, JournalPM journalPM)
        {




            //journalLinePM.ChangeSetOp = ChangeSetOperation.Insert;
            journalLinePM.EnsureAllDecimalPrecisionIfChangeChangeUpdate();
            journalLinePM.JournalId = journalPM.Id;
            journalLinePM.Tenant = journalPM.Tenant;

            journalLinePM.AccountingDate = journalLinePM.AccountingDate.Date; //Eyal:Time No Meaning (create+update Have  Time have Meaning )

            ///move up b4 CurrencyId is using 
            if (!String.IsNullOrWhiteSpace(journalLinePM.CurrencyCode) && String.IsNullOrWhiteSpace(journalLinePM.CurrencyId))
            {
                CurrencyPM currency =
                    GetSingleCurrencyByCode(journalLinePM.Tenant, journalLinePM.CurrencyCode);
                if (currency != null)
                {
                    journalLinePM.CurrencyId = currency.Id;
                    journalLinePM.CurrencyName = currency.EnglishName;
                }
            }



            var creditIVerifyGLAccountManager = GetIVerifyGLAccountManager();
            creditIVerifyGLAccountManager.Verify(this._MainContext, journalLinePM.Tenant, journalLinePM.CreditAccountId, journalLinePM.CreditAccountNumber, journalLinePM.CurrencyId);
            bool haveChange = false;
            haveChange = (journalLinePM.CreditAccountId != creditIVerifyGLAccountManager.AccountId ||
                journalLinePM.CreditControlAccountId != creditIVerifyGLAccountManager.ControlAccountId);
            journalLinePM.CreditAccountId = creditIVerifyGLAccountManager.AccountId;
            journalLinePM.CreditControlAccountId = creditIVerifyGLAccountManager.ControlAccountId;


            FullAccountingSettingPM accountingSettings = getFullAccountingSettings(journalPM.Tenant);

            var debitIVerifyGLAccountManager = GetIVerifyGLAccountManager();
            debitIVerifyGLAccountManager.Verify(
                this._MainContext,
                journalLinePM.Tenant,
                journalLinePM.DebitAccountId,
                journalLinePM.DebitAccountNumber,
                journalLinePM.CurrencyId
                );
            if (!haveChange)
            {
                haveChange = (journalLinePM.DebitAccountId != debitIVerifyGLAccountManager.AccountId ||
                    journalLinePM.DebitControlAccountId != debitIVerifyGLAccountManager.ControlAccountId);
            }

            if (
                //from mumps >>> CHANGE DEBIT
                IsFromMumps(journalLinePM)
                ||
                // Regular Journal do not  CHANGE DEBIT if credit equal VATOutputGLAccountId
                CreditAccountIsNotVATOutputGLAccountId(journalLinePM, accountingSettings)
                )
            {
                journalLinePM.DebitAccountId = debitIVerifyGLAccountManager.AccountId;
                journalLinePM.DebitControlAccountId = debitIVerifyGLAccountManager.ControlAccountId;
            }




            if (haveChange && journalLinePM.ChangeSetOp == ChangeSetOperation.None)
            {
                journalLinePM.ChangeSetOp = ChangeSetOperation.Update;
            }



            //if (journalLinePM.Line == 0) renumber = true;


            if (!String.IsNullOrWhiteSpace(journalLinePM.ActionTypeCode) &&
                String.IsNullOrWhiteSpace(journalLinePM.ActionCode))
            {

                JournalActionTypeList action =
                    JournalActionTypeListGetByCode(journalLinePM);
                if (action != null)
                {
                    journalLinePM.ActionCode = action.Id;
                    journalLinePM.ActionName = action.EnglishName;
                }
            }


            //if (!item.ExchangeRate.HasValue && item.LocalAmount.HasValue && item.LocalAmount.Value != 0m && item.ForeignAmount.HasValue && item.ForeignAmount.Value != 0m)

            if (!journalLinePM.ExchangeRate.HasValue)
            {
                double local = (double)journalLinePM.LocalAmount;
                double foreign = (double)journalLinePM.ForeignAmount;
                double rate = 0;
                if (local != 0 && foreign != 0)
                {
                    rate = local / foreign;
                }
                journalLinePM.ExchangeRate = Math.Round((decimal)rate, 5);
            }
        }

        private static bool CreditAccountIsNotVATOutputGLAccountId(JournalLinePM journalLinePM, FullAccountingSettingPM accountingSettings)
        {
            return accountingSettings.VATOutputGLAccountId != journalLinePM.CreditAccountId;
        }

        private static bool IsFromMumps(JournalLinePM journalLinePM)
        {
            return journalLinePM.DebitAccountId == "dmy";
        }


        public virtual FullAccountingSettingPM getFullAccountingSettings(int tenant)
        {
            bool fromCache = true;
            FullAccountingSettingPM accountingSettings=null;
            if (fromCache)
            {
                accountingSettings = FullAccountingSettingQueryService.Get(tenant);
                return accountingSettings;
            }


            
            FullAccountingSettingQueryService query = new FullAccountingSettingQueryService(tenant);
            accountingSettings = query.GetSingleFullAccountingSetting(tenant);
            return accountingSettings;
        }



        public virtual JournalActionTypeList JournalActionTypeListGetByCode(JournalLinePM item)
        {

            var _IJournalActionTypeListQueryService =
                new JournalActionTypeListQueryService(this._MainContext as IAccountingContext);

            JournalActionTypeList action = _IJournalActionTypeListQueryService
                .GetByCode(item.ActionTypeCode, item.Tenant);
            return action;
        }
        public virtual CurrencyPM GetSingleCurrencyByCode(int tenant, string CurrencyCode)
        {
            var queryService = ContainerAccessor.Container.ResolveSafe<ICurrencyQuery>() ??
                new CurrencyQuery(tenant);
            string key = $"GetSingleCurrencyByCode({tenant},{CurrencyCode})";
            return CacheManager.GetOrInsertNewObject<CurrencyPM>(key, () =>
            {
                return queryService.GetSingleCurrencyByCode(CurrencyCode, tenant);
            });
            
            
        }



        private static string GetControlAccountId(List<GLAccountPM> GLAccountsList, string accId)
        {
            if (!String.IsNullOrWhiteSpace(accId))
            {
                var GLAccount = GLAccountsList.FirstOrDefault(rec => rec.Id == accId);
                if (GLAccount != null)
                {
                    if ((!string.IsNullOrWhiteSpace(GLAccount.AccountTypeCode)) &&
                (GLAccount.AccountTypeCode != ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString())

        )
                    {
                        return GLAccount.ControlAccountId;
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            return null;
        }
        public virtual IVerifyGLAccountManager GetIVerifyGLAccountManager()
        {
            return new VerifyGLAccountManager() as IVerifyGLAccountManager;
        }
    }
    public interface IVerifyGLAccountManager
    {
        void Verify(IAccountingContext mainContext, int tenant, string accountId, string accountNumber, string CurrencyId);

        string AccountId { get; }
        string ControlAccountId { get; }
    }
    public class VerifyGLAccountManager : IVerifyGLAccountManager
    {
        private int _tenant;
        private string _accountId;
        private string _accountNumber;
        private IAccountingContext _MainContext;



        public void Verify(IAccountingContext mainContext, int tenant, string accountId, string accountNumber, string CurrencyId)
        {
            this._MainContext = mainContext;
            this._tenant = tenant;
            this._accountId = accountId;
            this._accountNumber = accountNumber;

            GLAccountPM myGLAccountPM = null;
            if (!String.IsNullOrWhiteSpace(_accountId))
            {
                myGLAccountPM = CheckAccountId(_tenant, _accountId);

            }
            if (myGLAccountPM == null && !String.IsNullOrWhiteSpace(_accountNumber))
            {
                myGLAccountPM = GetAccountIdByAccountNumber(_tenant, _accountNumber);
            }
            var why = JournalValidator.K_SuppressCheckGLAccountIsMultiCurrencyWI40640;//Task 40640: טיפול בסרביס לפקודת יומן - במקרה של כרטיס מפוצל לרשום על הפיצול
            if (why == JournalValidator.K_SuppressCheckGLAccountIsMultiCurrencyWI40640)//Task 40640: טיפול בסרביס לפקודת יומן - במקרה של כרטיס מפוצל לרשום על הפיצול
            {
                if (myGLAccountPM != null)
                {
                    if (myGLAccountPM.IsMultiCurrency.GetValueOrDefault())//Task 40640: טיפול בסרביס לפקודת יומן -במקרה של כרטיס מפוצל לרשום על הפיצול
                    {
                        //SuppressCheckGLAccountIsMultiCurrencyWI40640

                        //Task 40640: טיפול בסרביס לפקודת יומן -במקרה של כרטיס מפוצל לרשום על הפיצול
                        var myGLAccountCurrencyList = GetRelatedCurrenciesAccount(myGLAccountPM.Id, tenant);
                        var myGLAccountCurrency = myGLAccountCurrencyList.FirstOrDefault(r => r.CurrencyId == CurrencyId);
                        if (myGLAccountCurrency != null)
                        {
                            _accountId = myGLAccountCurrency.GLAccountId;
                            myGLAccountPM = CheckAccountId(_tenant, _accountId);
                        }
                    }
                }
            }
            if (myGLAccountPM != null)
            {
                this.AccountId = myGLAccountPM.Id;
                if (myGLAccountPM.AccountTypeCode != "1")
                {
                    this.ControlAccountId = myGLAccountPM.ControlAccountId;
                    if (string.IsNullOrWhiteSpace(this.ControlAccountId))
                    {
                        throw new Exception("GLAccount is not Card but ControlAccountId  is null ??");
                    }
                }
            }
        }



        public virtual List<GLAccountCurrencyPM> GetRelatedCurrenciesAccount(string CustomerGLAccountId, int tenant)
        {
            var a = new GLAccountCurrencyQueryService(this._MainContext as IAccountingContext);
            return a.GetRelatedCurrenciesAccount(tenant, CustomerGLAccountId);
        }


        GLAccountPM CheckAccountId(int tenant, string accountId)
        {
            var pm = GetSingleGLAccount(accountId);
            if (pm == null)
            {
                return null;
            }
            if (pm.Tenant != tenant)
            {
                return null;
            }
            if (pm.Inactive.GetValueOrDefault())
            {
                return null;
            }
            return pm;
        }



        GLAccountPM GetAccountIdByAccountNumber(int tenant, string accountNumber)
        {
            var pm = GetByInternalNumberGLAccount(tenant, accountNumber);
            if (pm == null)
            {
                return null;
            }
            return CheckAccountId(tenant, pm.Id);
        }

        public virtual GLAccountPM GetSingleGLAccount(string accountId)
        {
            var glQS = new GLAccountQueryService(this._MainContext);
            glQS.SetSuppressFetchOpenReconcilation(true);
            
            var pm = glQS.GetSingle(accountId, false, true);
            return pm;
        }
        public virtual GLAccountPM GetByInternalNumberGLAccount(int tenant, string accountNumber)
        {

            string key = $"GetByInternalNumberGLAccount({tenant},{accountNumber})";
            return CacheManager.GetOrInsertNewObject<GLAccountPM>(key, () =>
            {
                var glQS = new GLAccountQueryService(this._MainContext);
                glQS.SetSuppressFetchOpenReconcilation(true);
                var pm = glQS.GetByInternalNumber(accountNumber, tenant)/*.SingleOrDefault()*/;
                return pm;
            });
            
        }


        public string AccountId { get; private set; }
        public string ControlAccountId { get; private set; }




    }
    public interface IJournalLineOnUpdate
    { }
}
