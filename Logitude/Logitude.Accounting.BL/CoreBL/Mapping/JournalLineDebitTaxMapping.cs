using Logitude.Accounting.BL.Validators;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Mapping
{
    public class JournalLineDebitTaxMapping : JournalLineMappingBase
    {

        public JournalLineDebitTaxMapping(JournalLinePM journalLine, JournalPM journalPM, IGLAccountDataProvider myGLAccountPMProvider)
            : base(journalLine, journalPM, myGLAccountPMProvider)
        {

        }
        protected override void MapIt()
        {



            MyLedgerTransaction.ControlAccountId = null;
            MyLedgerTransaction.AccountId = //(new AccountingSettingResolver()).
                ResolveVATOutputGLAccountId(_JournalPM.Tenant); //Tax Account

            decimal myVat = //(new AccountingSettingResolver()).
                ResolveVat(_JournalLine.Tenant, _JournalLine.DocumentDate); //Convert.ToDecimal(1.18);
            decimal localAmount = (decimal)_JournalLine.LocalAmount / myVat;
            decimal foreignAmount = (decimal)_JournalLine.ForeignAmount / myVat;
            MyLedgerTransaction.LocalAmountDebit = (decimal)_JournalLine.LocalAmount - 1 * localAmount;
            MyLedgerTransaction.LocalAmountCredit = 0;
            MyLedgerTransaction.CurrencyId = _JournalLine.CurrencyId;

            MyLedgerTransaction.OppositeAccountId = _JournalLine.CreditAccountId;
            MyLedgerTransaction.ForeignAmountDebit = (decimal)_JournalLine.ForeignAmount - 1 * foreignAmount;
            MyLedgerTransaction.ForeignAmountCredit = 0;
            if (_JournalLine.ExchangeRate != null)
            {
                MyLedgerTransaction.ExchangeRate = (decimal)_JournalLine.ExchangeRate;
            }
            else
            {

                decimal LocalAmountDebit = MyLedgerTransaction.LocalAmountDebit ;
                decimal ForeignAmountDebit = MyLedgerTransaction.ForeignAmountDebit ;
                MyLedgerTransaction.ExchangeRate = LocalAmountDebit / ForeignAmountDebit;
            }
            

            decimal localVat = (decimal)_JournalLine.LocalAmount - 1 * localAmount;
            MyLedgerTransaction.OpenAmount =  localVat;

            

                

            MyLedgerTransaction.OpenAmountCurrencyId = //(new AccountingSettingResolver()).
                ResolveAccountingCurrencyId(_JournalPM.Tenant);// localAccountingCurrencyId;

            MyLedgerTransaction.OppositeAccountId = _JournalLine.CreditAccountId;



        }

        private string ResolveAccountingCurrencyId(int Tenant)
        {
           return (new AccountingSettingResolver()).
            ResolveAccountingCurrencyId(Tenant);// localAccountingCurrencyId;
        }

        public virtual decimal ResolveVat(int Tenant, DateTime DocumentDate)
        {
            return(new AccountingSettingResolver()).
            ResolveVat(Tenant, DocumentDate); //Convert.ToDecimal(1.18);
        }

        public virtual string ResolveVATOutputGLAccountId(int Tenant)
        {
            return (new AccountingSettingResolver()).
            ResolveVATOutputGLAccountId(Tenant); //Tax Account

        }

        protected override void AddGLAccountTotalByMounth(GLAccountTotalByMonthPM currGLAccountTotalByMounth, LedgerTransactionPM MyLedgerTransaction)
        {

            currGLAccountTotalByMounth.ForeignAmountDebit += MyLedgerTransaction.ForeignAmountDebit; ;
            currGLAccountTotalByMounth.LocalAmountDebit += MyLedgerTransaction.LocalAmountDebit ;
        }


        public virtual GLAccountPM GetGLAccountPM(string AccountId, int Tenant)
        {
            return _GLAccountPMProvider.GetGLAccount(AccountId, Tenant);
        }


        public override JournalLineMappingBase.MappingTypeEnum MyMappingTypeEnum
        {
            get { return MappingTypeEnum.DebitTax; }
        }
    }
}
