using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.BillingsPreparation;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class CashBookPreparation
    {

        public void Prepare()
        {
            FullAccountingData.CashBookCash1 = GetCashNIS();
            FullAccountingData.CashBookCheques1 = GetChequesNIS();
        }

        private string GetCashNIS()
        {
            return GetIdByLocalName(CashBookCodes.CashNIS) ?? Create(CreateCashNISInstance());
        }

        private string GetChequesNIS()
        {
            return GetIdByLocalName(CashBookCodes.ChequesNIS) ?? Create(CreateChequesNISInstance());
        }
        public string GetNewCashNIS()
        {
            return  Create(CreateAnyCashNIS());
        }


        private string GetIdByLocalName(string name)
        {
            var filter = GetFilterByLocalName(name);
            var response = APICaller.CallGetByFilters<List<CashBookPM>>(Urls.CashBookViewsByFilters, UserTenant.Token, filter);
            return response.Data?.FirstOrDefault()?.Id;
        }

        private ApiQueryFilters GetFilterByLocalName(string name)
        {
            return new ApiQueryFiltersBuilder()
                .WithDefualtValues()
                .Filter1Name("LocalName")
                .Filter1Value(name)
                .Build();
        }

        public string Create(CashBookPM cashBook)
        {
            var response = APICaller.CallPost<CashBookPM>(cashBook, Urls.CashBooksController, UserTenant.Token);
            return response.Data?.Id;
        }
        private CashBookPM CreateCashNISInstance()
        {
            return new CashBookPM()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                CurrencyId = BillingData.CurrencyNISId,
                AccountId = new AccountPreparation().Create(),
                CashBookTypeCode = (int)CashBookTypeCodeEnum.Cash + "",
                EnglishName = CashBookCodes.CashNIS,
                BranchId = FullAccountingData.BZUBranchID,
                TotalAmount = 0,
                LocalName = CashBookCodes.CashNIS,
                Tenant = UserTenant.Tenant,
                SearchFields = CashBookCodes.CashNIS

            };
        }
        private CashBookPM CreateChequesNISInstance()
        {
            return new CashBookPM()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                CurrencyId = BillingData.CurrencyNISId,
                AccountId = new AccountPreparation().Create(),
                CashBookTypeCode = (int)CashBookTypeCodeEnum.Cheques + "",
                EnglishName = CashBookCodes.ChequesNIS,
                BranchId = FullAccountingData.BZUBranchID,
                LocalName = CashBookCodes.ChequesNIS,
                TotalAmount = 0,
                Tenant = UserTenant.Tenant,
                SearchFields = CashBookCodes.ChequesNIS

            };
        }
        private CashBookPM CreateAnyCashNIS()
        {
            var name = "CB" + DateTime.Now.Ticks;
            return new CashBookPM()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                CurrencyId = BillingData.CurrencyNISId,
                AccountId = new AccountPreparation().Create(),
                CashBookTypeCode = (int)CashBookTypeCodeEnum.Cash + "",
                EnglishName = name,
                BranchId = new BranchPreparation().GetNewBranch(),
                LocalName = name,
                TotalAmount = 500,
                Tenant = UserTenant.Tenant,
                SearchFields = name

            };
        }
    }
}
