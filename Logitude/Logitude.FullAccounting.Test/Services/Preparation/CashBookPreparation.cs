using Logitude.FullAccounting.Test.Models;
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
    public class CashBookPreparation
    {
        const string CashNIS = "CashNIS1";
        const string ChequesNIS = "CashBook1";
        public void Prepare()
        {
            FullAccountingData.CashBookCash1 = GetByCode(CashNIS);
            FullAccountingData.CashBookCheques1 = GetByCode(ChequesNIS);
        }
        private string GetByCode(string code)
        {
            var id = GetIdByLocalName(code);
            if (string.IsNullOrEmpty(id))
            {
                return Create(code);
            }
            return id;
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

        public string Create(string name = null)
        {
            var cashBook = CreateInstance(name);
            var response = APICaller.CallPost<CashBookPM>(cashBook, Urls.CashBooksController, UserTenant.Token);
            return response.Data?.Id;
        }
        private CashBookPM CreateInstance(string name)
        {
            switch (name)
            {
                case CashNIS:
                    return CreateCashNISInstance(name);
                case ChequesNIS:
                    return CreateChequesNISInstance(name);
                default:
                    return CreateAny();
            }
        }

        

        private CashBookPM CreateCashNISInstance(string name)
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
                EnglishName = name,
                BranchId = FullAccountingData.BZUBranchID,
                TotalAmount = 0,
                LocalName = name,
                Tenant = UserTenant.Tenant,
                SearchFields = name

            };
        }
        private CashBookPM CreateChequesNISInstance(string name)
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
                EnglishName = name,
                BranchId = FullAccountingData.BZUBranchID,
                LocalName = name,
                TotalAmount = 0,
                Tenant = UserTenant.Tenant,
                SearchFields = name

            };
        }
        private CashBookPM CreateAny()
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
                BranchId = new BranchPreparation().Create(),
                LocalName = name,
                TotalAmount = 500,
                Tenant = UserTenant.Tenant,
                SearchFields = name

            };
        }
    }
}
