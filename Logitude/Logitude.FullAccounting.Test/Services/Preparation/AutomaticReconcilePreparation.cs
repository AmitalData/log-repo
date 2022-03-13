using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Base.Models.Api;
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
    public class AutomaticReconcilePreparation
    {
        public void Prepare()
        {
            FullAccountingData.AutomaticReconcile_A = GetAMethod();
            //add more
        }

        private string GetAMethod()
        {
            return GetIdByCode(AutomaticReconcileMethodCodes.OpenAmount) ?? Create(CreateOpenAmountInstance());
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
        private string Create(AutomaticReconcileMethodPM automaticReconcileMethod)
        {
            var response = APICaller.CallPost<AutomaticReconcileMethodPM>(automaticReconcileMethod, Urls.AutomaticReconcileMethods, UserTenant.Token);
            return response.Data?.Id;
        }
        
        private AutomaticReconcileMethodPM CreateOpenAmountInstance()
        {
            return new AutomaticReconcileMethodPM()
            {
                Code = AutomaticReconcileMethodCodes.OpenAmount,
                Name = "Open Amount",
                LocalName = "Open Amount",
                Tenant = UserTenant.Tenant,
                SearchFields = $"{AutomaticReconcileMethodCodes.OpenAmount},Open Amount",
                AutomaticReconcile1 = (int)AutomaticReconcileEnum.OpenAmount + "",

            };
        }
    }
}
