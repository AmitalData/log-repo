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


        public string Get( )
        {
            if (string.IsNullOrEmpty(dWObjectTablePM.AdditionalConditions) && string.IsNullOrEmpty(dWObjectTablePM.ParentFactCode)) return null;
            if (!string.IsNullOrEmpty(dWObjectTablePM.AdditionalConditions)) return dWObjectTablePM.AdditionalConditions;
            return GetDWObjectTableByCode(dWObjectTablePM.ParentFactCode, dWObjectTablePM.Tenant)?.AdditionalConditions;
        }


        private DWObjectTablePM GetDWObjectTableByCode(string code, int tenant)
        {
            DWObjectTableQuery dWObjectTableQuery = new DWObjectTableQuery(tenant);
            return dWObjectTableQuery.GetSinglePM(code, tenant);
        }


    }
}