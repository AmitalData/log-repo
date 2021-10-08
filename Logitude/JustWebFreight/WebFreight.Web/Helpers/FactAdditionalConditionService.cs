using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class FactAdditionalConditionService
    {

        private string factCondition = string.Empty;
        private DWObjectTablePM dWObjectTablePM;
        public FactAdditionalConditionService(DWObjectTablePM dWObjectTablePM , string factCondition)
        {
            this.dWObjectTablePM = dWObjectTablePM;
            this.factCondition = factCondition;
        }


        public string Build()
        {
            string additionalCondition = GetAdditionalCondition(dWObjectTablePM);
            if (string.IsNullOrEmpty(additionalCondition)) return null;
            return (!string.IsNullOrEmpty(factCondition) ? " and " : " ") + additionalCondition;
        }



        private string GetAdditionalCondition(DWObjectTablePM dWObjectTablePM)
        {
            if (string.IsNullOrEmpty(dWObjectTablePM.AdditionalConditions) && string.IsNullOrEmpty(dWObjectTablePM.AdditionalFactCode)) return null;
            if (!string.IsNullOrEmpty(dWObjectTablePM.AdditionalConditions)) return dWObjectTablePM.AdditionalConditions;
            return GetDWObjectTableByCode(dWObjectTablePM.AdditionalFactCode, dWObjectTablePM.Tenant)?.AdditionalConditions;
        }



        private DWObjectTablePM GetDWObjectTableByCode(string code, int tenant)
        {
            DWObjectTableQuery dWObjectTableQuery = new DWObjectTableQuery(tenant);
            return dWObjectTableQuery.GetSinglePM(code, tenant);
        }


    }
}