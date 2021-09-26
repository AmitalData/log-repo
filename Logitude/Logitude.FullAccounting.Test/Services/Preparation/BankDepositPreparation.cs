using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class BankDepositPreparation
    {

        public void Prepare()
        {
            FullAccountingData.BankDeposit1 = CraeteCash().Id;
        }
        public void ChequePrepare()
        {
            var cashBookId = new CashBookPreparation().GetNewCashNIS();
            var chequeBankDepositLine = Getline(cashBookId);
            var chequeBankDeposiInstance = CraeteChequeInstance(cashBookId, new List<BankDepositLinePM>() { chequeBankDepositLine });
            var chequeBankDeposi = Create(chequeBankDeposiInstance);
            FullAccountingData.BankDepositChequeId = chequeBankDeposi.Id;
            FullAccountingData.ChequeBankDepositARPaymentChequeId = chequeBankDepositLine.ARPChequeId;
        }


        private BankDepositPM CraeteCash()
        {
            var cash = CraeteCashInstance();
            return Create(cash);
        }
        private BankDepositPM CraeteCashInstance()
        {
            return new BankDepositPM()
            {
                Tenant = UserTenant.Tenant,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                AccountingDate = DateTime.Now,
                DepositDate = DateTime.Now,
                DepositCurrencyId = BillingData.CurrencyNISId,
                DepositCurrencyCode = CurrencyCodes.NIS,
                DepositBankAccountId = FullAccountingData.BankAccountId,
                IsCashDeposit = true,
                ForeignAmount = 100,
                LocalDepositAmount = 100,
                CashBookId = new CashBookPreparation().GetNewCashNIS(),

            };
        }
        private BankDepositLinePM Getline(string cashBookId)
        {
            var filter = CreateBankDepositLineApiFilter(FullAccountingData.CashBookCheques1, false);
            var lines = APICaller.CallGetByFilters<List<CashBookLineList>>(Urls.CashBookLineViewsByFilters, UserTenant.Token, filter).Data;
            return MapToBankDepositLine(cashBookId,lines.Last());
        }

        private BankDepositLinePM MapToBankDepositLine(string cashBookId, CashBookLineList line)
        {
            return new BankDepositLinePM() 
            {
                ARPaymentChequeId = line.ARPaymentId,
                ARPaymentId = line.ARPaymentId,
                ARPaymentNumber = line.ARPaymentNumber,
                AccountNumber = line.AccountNumber,
                Bank = line.Bank,
                Branch = line.Branch,
                ChequeNumber = line.ChequeNumber,
                CompositId = cashBookId+","+ line.ARPaymentId,
                Currency = line.Currency,
                ForeignAmount = line.ForeignAmount,
                Line = 1,
                LocalAmount = line.LocalAmount,
                Tenant = UserTenant.Tenant
            };
        }

        private ApiQueryFilters CreateBankDepositLineApiFilter(string cashBookId, bool isDeposited)
        {
            return new ApiQueryFilters()
            {
                GetAll = true,
                Filter1Name = "CashBookId",
                Filter1Value = cashBookId,
                Filter2Name = "IsDeposited",
                Filter2Value = isDeposited.ToString().ToLower()

            };
        }
        public BankDepositPM Create(BankDepositPM cash)
        {
            var response = APICaller.CallPost<BankDepositPM>(cash, Urls.BankDepositsController, UserTenant.Token);
            return response.Data;
        }
        private BankDepositPM CraeteChequeInstance(string cashBookId, List<BankDepositLinePM> bankDepositLinePMs)
        {
            return new BankDepositPM()
            {
                Tenant = UserTenant.Tenant,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                AccountingDate = DateTime.Now,
                DepositDate = DateTime.Now,
                DepositCurrencyId = BillingData.CurrencyNISId,
                DepositBankAccountId = FullAccountingData.BankAccountId,
                IsCashDeposit = false,
                ForeignAmount = bankDepositLinePMs.Sum(e=>e.ForeignAmount),
                LocalDepositAmount = bankDepositLinePMs.Sum(e => e.LocalAmount),
                CashBookId = cashBookId,
                BankDepositLines = bankDepositLinePMs

            };
        }


    }
}
