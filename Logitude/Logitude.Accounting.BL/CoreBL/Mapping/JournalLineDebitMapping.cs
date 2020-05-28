using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logitude.Accounting.BL.Validators;

namespace Logitude.Accounting.BL.CoreBL.Mapping
{
    public class JournalLineDebitMapping : JournalLineMappingBase
    {
        private IAccountingSettingResolver _myIAccountingSettingResolver;
        private bool _VatExtract;

        internal JournalLineDebitMapping(JournalLinePM journalLine, JournalPM journalPM, bool vatExtract, IGLAccountDataProvider myGLAccountPMProvider, IAccountingSettingResolver myIAccountingSettingResolver)
            : base(journalLine, journalPM, myGLAccountPMProvider, myIAccountingSettingResolver)
        {
            _myIAccountingSettingResolver = myIAccountingSettingResolver;
            this._VatExtract = vatExtract;
        }
        protected override void MapIt()
        {

            MyLedgerTransaction.AccountId = _JournalLine.DebitAccountId;


            decimal myVat = //(new AccountingSettingResolver()).
                ResolveVat(_JournalLine.Tenant, _JournalLine.DocumentDate);// Convert.ToDecimal(1.18);
            decimal localAmountWithoutVat = (decimal)_JournalLine.LocalAmount / myVat;
            decimal foreignAmountWithoutVat = (decimal)_JournalLine.ForeignAmount / myVat;

            if (_VatExtract)
            {


                MyLedgerTransaction.LocalAmountDebit = localAmountWithoutVat;
                MyLedgerTransaction.ForeignAmountDebit = foreignAmountWithoutVat;

            }
            else
            {
                MyLedgerTransaction.LocalAmountDebit = (decimal)_JournalLine.LocalAmount;
                MyLedgerTransaction.ForeignAmountDebit = (decimal)_JournalLine.ForeignAmount;


            }

            MyLedgerTransaction.ForeignAmountCredit = 0;
            MyLedgerTransaction.LocalAmountCredit = 0;
            MyLedgerTransaction.CurrencyId = _JournalLine.CurrencyId;


            if (_JournalLine.ExchangeRate != null)
            {
                MyLedgerTransaction.ExchangeRate = (decimal)_JournalLine.ExchangeRate;
            }
            else
            {
                decimal LocalAmountDebit = MyLedgerTransaction.LocalAmountDebit;
                decimal ForeignAmountDebit = MyLedgerTransaction.ForeignAmountDebit ;
                MyLedgerTransaction.ExchangeRate = LocalAmountDebit / ForeignAmountDebit;
            }




            if (!string.IsNullOrWhiteSpace(_JournalLine.DebitAccountId))
            {

                GLAccountPM parent = GetGLAccountPM(_JournalLine.DebitAccountId,_JournalLine.Tenant);

                if ((!string.IsNullOrWhiteSpace(parent.AccountTypeCode)) && (parent.AccountTypeCode != 
                    //AccountingSettingResolver.ResolveCard1AccountType() 
                    ((int)GLAccountTypePM.GLAccountTypeEnum.Card).ToString()
                    /*"1" */))
                {

                    if (parent.ControlAccountId != _JournalLine.DebitControlAccountId)
                    {
                        throw new Exception("(parentAccount.ControlAccountId != _JournalLine.CreditControlAccountId)");
                    }
                    MyLedgerTransaction.ControlAccountId = _JournalLine.DebitControlAccountId;
                    //if (String.IsNullOrWhiteSpace(MyLedgerTransaction.ControlAccountId))
                    //{
                    //    MyLedgerTransaction.ControlAccountId = parent.ControlAccountId;
                    //}
                    if (String.IsNullOrWhiteSpace(MyLedgerTransaction.ControlAccountId))
                    {
                        throw new Exception("AccountType!=Card , But MyLedgerTransaction.ControlAccountId==null");
                    }
                    //if (MyLedgerTransaction.ControlAccountId != _JournalLine.DebitControlAccountId)
                    //{
                    //    throw new Exception("(MyLedgerTransaction.ControlAccountId != _JournalLine.DebitControlAccountId)");
                    //}
                }
                else
                {
                    MyLedgerTransaction.ControlAccountId = null;
                }

                if (_VatExtract)
                {

                    if (parent.ReconcileMethodCode == 
                        //AccountingSettingResolver.ResolveLocalCurrency0ReconcileMethodCode() 
                        ((int)Logitude.Accounting.Def.EntityPMs.ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString()
                        /* "0" */ ) //Local Currency
                    {

                        MyLedgerTransaction.OpenAmount =  localAmountWithoutVat;
                        MyLedgerTransaction.OpenAmountCurrencyId = ResolveAccountingCurrencyId(_JournalPM.Tenant);// localAccountingCurrencyId;
                    }
                    else
                    {
                        MyLedgerTransaction.OpenAmount = foreignAmountWithoutVat;
                        MyLedgerTransaction.OpenAmountCurrencyId = _JournalLine.CurrencyId;
                    }
                }
                else
                {

                    if (parent.ReconcileMethodCode == 
                        //AccountingSettingResolver.ResolveLocalCurrency0ReconcileMethodCode() 
                        ((int)Logitude.Accounting.Def.EntityPMs.ReconcileMethodPM.ReconcileMethodEnum.LocalCurrency).ToString()
                        /* "0" */ ) //Local Currency
                    {
                        MyLedgerTransaction.OpenAmount =  (decimal)_JournalLine.LocalAmount;
                        MyLedgerTransaction.OpenAmountCurrencyId = ResolveAccountingCurrencyId(_JournalPM.Tenant);// localAccountingCurrencyId;
                    }
                    else
                    {
                        MyLedgerTransaction.OpenAmount = (decimal)_JournalLine.ForeignAmount;
                        MyLedgerTransaction.OpenAmountCurrencyId = _JournalLine.CurrencyId;
                    }
                    MapExternalOpenAmount();
                }
            }

            MyLedgerTransaction.OppositeAccountId = _JournalLine.CreditAccountId;
            


        }

        public virtual decimal ResolveVat(int Tenant, DateTime DocumentDate)
        {
            //(new AccountingSettingResolver()).
            return //(new AccountingSettingResolver())
            _myIAccountingSettingResolver
                .ResolveVat(Tenant, DocumentDate);// Convert.ToDecimal(1.18);
        }
        public virtual string ResolveAccountingCurrencyId(int Tenant)
        {
            return
                _myIAccountingSettingResolver//(new AccountingSettingResolver())
                .ResolveAccountingCurrencyId(Tenant);
        }
        protected override void AddGLAccountTotalByMounth(GLAccountTotalByMonthPM currGLAccountTotalByMounth, LedgerTransactionPM currLedgerTransaction)
        {
            currGLAccountTotalByMounth.ForeignAmountDebit += currLedgerTransaction.ForeignAmountDebit;
            currGLAccountTotalByMounth.LocalAmountDebit += currLedgerTransaction.LocalAmountDebit ;
        }

        public override JournalLineMappingBase.MappingTypeEnum MyMappingTypeEnum
        {
            get { return MappingTypeEnum.Debit; }
        }
        public virtual GLAccountPM GetGLAccountPM(string AccountId, int Tenant)
        {
            return _GLAccountPMProvider.GetGLAccount(AccountId, Tenant);
        }
    }
}
