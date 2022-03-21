using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using Logitude.Base.Models.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Base.Models.BillingsPreparation;
using Logitude.FullAccounting.Test.Models.Codes;

namespace Logitude.FullAccounting.Test.Services.Preparation
{

    public class ChargeTypePreparation
    {
        public void Prepare()
        {
            FullAccountingData.ItemsChargeTypeId = GetItems();
        }

        private string GetItems()
        {
            return GetIdByCode(ChargeTypeCodes.Items) ?? Create(CreateITMSInstance());
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
        private string Create(ChargesTypePM chargesTypePM)
        {
            var response = APICaller.CallPost<ChargesTypePM>(chargesTypePM, Urls.ChargesTypes, UserTenant.Token);
            return response.Data?.Id;
        }

        private ChargesTypePM CreateITMSInstance()
        {
            new AccountPreparation().PrepareAccounts();
            return new ChargesTypePM()
            {
                Code = ChargeTypeCodes.Items,
                EnglishName = "Items Charge",
                LocalName = "Items Charge",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{ChargeTypeCodes.Items},Items Charge",
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
