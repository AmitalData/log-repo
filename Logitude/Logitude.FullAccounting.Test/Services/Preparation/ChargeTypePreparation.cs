using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Test.Base.Models.BillingsPreparation;

namespace Logitude.FullAccounting.Test.Services.Preparation
{

    public class ChargeTypePreparation
    {
        const string Items = "ITMS";
        public void Prepare()
        {
            FullAccountingData.ItemsChargeTypeId = GetByCode(Items);
        }
        private string GetByCode(string code)
        {
            var id = GetIdByCode(code);
            if (string.IsNullOrEmpty(id))
            {
                return Create(code);
            }
            return id;
        }
        private string GetIdByCode(string code)
        {
            var filter = GetFilterByCode(code);
            var response = APICaller.CallGetByFilters<List<ChargesTypePM>>(Urls.ChargeTypeViewsGetByFilters, UserTenant.Token, filter);
            return response.Data?.FirstOrDefault()?.Id;
        }
        private ApiQueryFilters GetFilterByCode(string code)
        {
            return new ApiQueryFiltersBuilder()
                .WithDefualtValues()
                .Filter1Name("Code")
                .Filter1Value(code)
                .Build();
        }
        private string Create(string code)
        {
            var chargesTypePM = CreateInstance(code);
            var response = APICaller.CallPost<ChargesTypePM>(chargesTypePM, Urls.ChargesTypes, UserTenant.Token);
            return response.Data?.Id;
        }
        private ChargesTypePM CreateInstance(string code)
        {
            switch (code)
            {
                case Items:
                    return CreateITMSInstance(code);
                default:
                    return null;
            }
        }
        private ChargesTypePM CreateITMSInstance(string code)
        {
            new AccountPreparation().PrepareAccounts();
            return new ChargesTypePM()
            {
                Code = code,
                EnglishName = "Items Charge",
                LocalName = "Items Charge",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code},Items Charge",
                ChargesGroupId = FullAccountingData.ChargesGroup1ID,
                ChargesGroupCode = "G1",
                MeasurementCode = "GRWT",
                MeasurementId = BillingData.MeasurementGRWTId,
                ReceivableCreditGLAccountId = FullAccountingData.GLAccountRevenuesId,
                PayableDebitGLAcountId = FullAccountingData.GLAccountExpensesId
            };
        }
        

    }
}
