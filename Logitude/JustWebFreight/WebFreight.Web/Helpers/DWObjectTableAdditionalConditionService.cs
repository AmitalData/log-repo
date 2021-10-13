using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class DWObjectTableAdditionalConditionService
    {

        private DWObjectTablePM dWObjectTablePM;
        public DWObjectTableAdditionalConditionService(DWObjectTablePM dWObjectTablePM)
        {
            this.dWObjectTablePM = dWObjectTablePM;
        }


        public string Get(bool isDWQueryUsedAdditionalFact)
        {
            if (string.IsNullOrEmpty(dWObjectTablePM.AdditionalConditions) && string.IsNullOrEmpty(dWObjectTablePM.AdditionalFactCode)) return null;
            if (!string.IsNullOrEmpty(dWObjectTablePM.AdditionalConditions)) return dWObjectTablePM.AdditionalConditions;
            if (!isDWQueryUsedAdditionalFact) return null;
            return GetDWObjectTableByCode(dWObjectTablePM.AdditionalFactCode, dWObjectTablePM.Tenant)?.AdditionalConditions;
        }


        private DWObjectTablePM GetDWObjectTableByCode(string code, int tenant)
        {
            DWObjectTableQuery dWObjectTableQuery = new DWObjectTableQuery(tenant);
            return dWObjectTableQuery.GetSinglePM(code, tenant);
        }


    }
}