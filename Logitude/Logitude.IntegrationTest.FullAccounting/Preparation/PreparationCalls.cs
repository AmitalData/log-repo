using Logitude.Accounting.Data.EntityLists;
using Logitude.IntegrationTest.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.FullAccounting.Preparation
{
    public class PreparationCalls
    {
        public static async Task<bool> PrepareVariables()
        {
            bool ready = true;
            string chartOfAccountId = await SetChartOfAccountId();
            return ready;
        }
        
        public static async Task<ChartOfAccountList> GetSingleChartOfAccount()
        {
            string urlparameters = QueryFiltersPreparation.GetUrlParameters(PreparationVariables.ChartOfAccountCode);
            HttpResponseMessage response = await RestClientService.GetAsync("chartofaccountviews" + urlparameters);
            string result = RestClientService.ParseResponseAndReturnSingleResult(response);
            ChartOfAccountList chartOfAccount = JsonConvert.DeserializeObject<ChartOfAccountList>(result);
            return chartOfAccount;
        }

        private static async Task<string> SetChartOfAccountId()
        {
            ChartOfAccountList chartOfAccount = await GetSingleChartOfAccount();
            PreparationVariables.ChartOfAccountId = chartOfAccount.Id;
            return PreparationVariables.ChartOfAccountId;
        }
    }
}
