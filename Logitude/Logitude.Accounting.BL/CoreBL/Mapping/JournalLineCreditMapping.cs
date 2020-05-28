using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logitude.Accounting.BL.Validators;

namespace Logitude.Accounting.BL.CoreBL.Mapping
{
    public class JournalLineCreditMapping : JournalLineMappingBase
    {
        private IAccountingSettingResolver _myIAccountingSettingResolver;

        public JournalLineCreditMapping(JournalLinePM journalLine, JournalPM journalPM, IGLAccountDataProvider myGLAccountPMProvider, IAccountingSettingResolver myIAccountingSettingResolver)
            : base(journalLine, journalPM, myGLAccountPMProvider, myIAccountingSettingResolver)
        {
            this._myIAccountingSettingResolver = myIAccountingSettingResolver;
        }

        protected override void MapIt()
        {



            MyLedgerTransaction.AccountId = _JournalLine.CreditAccountId;
            MyLedgerTransaction.LocalAmountDebit = 0;
            MyLedgerTransaction.LocalAmountCredit = (decimal)_JournalLine.LocalAmount;
            MyLedgerTransaction.CurrencyId = _JournalLine.CurrencyId;
            MyLedgerTransaction.ForeignAmountDebit = 0;
            MyLedgerTransaction.ForeignAmountCredit = (decimal)_JournalLine.ForeignAmount;
            //if (_JournalLine.ExchangeRate != null)
            //{
            //    MyLedgerTransaction.ExchangeRate = (decimal)_JournalLine.ExchangeRate;
            //}
            //else
            {

                decimal LocalAmountCredit = MyLedgerTransaction.LocalAmountCredit;
                decimal ForeignAmountCredit = MyLedgerTransaction.ForeignAmountCredit;
                if (LocalAmountCredit == 0)
                {
                    MyLedgerTransaction.ExchangeRate = 0;
                }
                else if (ForeignAmountCredit == 0)//Journalvalidation not null !!
                {

                }
                else
                {
                    MyLedgerTransaction.ExchangeRate = LocalAmountCredit / ForeignAmountCredit;
                }


            }





            //GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(_JournalLine.Tenant);
            GLAccountPM parentAccount  ///gLAccountQueryService.GetSingle(_JournalLine.CreditAccountId, false, false);
            = GetGLAccountPM(_JournalLine.CreditAccountId, _JournalLine.Tenant);
            if (parentAccount == null)
            {
                throw new Exception("CreditAccountId is not valid");
            }
            if ((!string.IsNullOrWhiteSpace(parentAccount.AccountTypeCode)) &&
                (parentAccount.AccountTypeCode != ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString())
        //AccountingSettingResolver.ResolveCard1AccountType())
        )
            {

                if (parentAccount.ControlAccountId != _JournalLine.CreditControlAccountId)
                {
                    throw new Exception("(parentAccount.ControlAccountId != _JournalLine.CreditControlAccountId)");
                }
                MyLedgerTransaction.ControlAccountId = _JournalLine.CreditControlAccountId;
                //if (String.IsNullOrWhiteSpace(MyLedgerTransaction.ControlAccountId))
                //{
                //    MyLedgerTransaction.ControlAccountId = parentAccount.ControlAccountId;
                //}
                if (String.IsNullOrWhiteSpace(MyLedgerTransaction.ControlAccountId))
                {
                    throw new Exception("AccountType!=Card , But MyLedgerTransaction.ControlAccountId==null");
                }
                //if (MyLedgerTransaction.ControlAccountId != _JournalLine.CreditControlAccountId)
                //{
                //    throw new Exception("(MyLedgerTransaction.ControlAccountId != _JournalLine.CreditControlAccountId)");
                //}
            }
            else
            {
                MyLedgerTransaction.ControlAccountId = null;
            }

            if (parentAccount.ReconcileMethodCode ==
                //AccountingSettingResolver.ResolveLocalCurrency0ReconcileMethodCode()
                ((int)Logitude.Accounting.Def.EntityPMs.ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString()
                ) //Local Currency
            {

                MyLedgerTransaction.OpenAmount = (decimal)_JournalLine.LocalAmount * -1;
                MyLedgerTransaction.OpenAmountCurrencyId = ResolveAccountingCurrencyId(_JournalPM.Tenant);
                ;// localAccountingCurrencyId;
            }
            else
            {
                MyLedgerTransaction.OpenAmount = (decimal)_JournalLine.ForeignAmount * -1;
                MyLedgerTransaction.OpenAmountCurrencyId = _JournalLine.CurrencyId;

            }
            MapExternalOpenAmount();

            MyLedgerTransaction.OppositeAccountId = _JournalLine.DebitAccountId;





        }

        
        public virtual string ResolveAccountingCurrencyId(int Tenant)
        {
            return _myIAccountingSettingResolver//(new AccountingSettingResolver())
                .ResolveAccountingCurrencyId(Tenant);
        }


        protected override void AddGLAccountTotalByMounth(GLAccountTotalByMonthPM currGLAccountTotalByMounth, LedgerTransactionPM currLedgerTransaction)
        {
            currGLAccountTotalByMounth.ForeignAmountCredit += currLedgerTransaction.ForeignAmountCredit;
            currGLAccountTotalByMounth.LocalAmountCredit += currLedgerTransaction.LocalAmountCredit ;
        }
        
        
        public override JournalLineMappingBase.MappingTypeEnum MyMappingTypeEnum
        {
            get { return MappingTypeEnum.Credit; }
        }


        public virtual GLAccountPM GetGLAccountPM(string AccountId, int Tenant)
        {
            return _GLAccountPMProvider.GetGLAccount(AccountId, Tenant);
        }
    }
}
