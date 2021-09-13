using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.Api;
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
    public class AutomaticReconcilePreparation
    {
        const string OpenAmount = "A";
        public void Prepare()
        {
            FullAccountingData.AutomaticReconcile_A = GetByCode(OpenAmount);
            //add more
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
            var response = APICaller.CallGetByFilters<List<AutomaticReconcileMethodPM>>(Urls.AutomaticReconcileMethodViewsByFilters, UserTenant.Token, filter);
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
            var AutomaticReconcileMethod = CreateInstance(code);
            var response = APICaller.CallPost<AutomaticReconcileMethodPM>(AutomaticReconcileMethod, Urls.AutomaticReconcileMethods, UserTenant.Token);
            return response.Data?.Id;
        }
        private AutomaticReconcileMethodPM CreateInstance(string code)
        {
            switch (code)
            {
                case OpenAmount:
                    return CreateOpenAmountInstance(code);
                default:
                    return null;
            }
        }
        private AutomaticReconcileMethodPM CreateOpenAmountInstance(string code)
        {
            return new AutomaticReconcileMethodPM()
            {
                Code = code,
                Name = "Open Amount",
                LocalName = "Open Amount",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{code},Open Amount",
                AutomaticReconcile1 = (int)AutomaticReconcileEnum.OpenAmount + "",

            };
        }
    }
}
