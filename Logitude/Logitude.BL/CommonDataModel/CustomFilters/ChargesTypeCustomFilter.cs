using Logitude.BL.Security;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.CustomFilters
{
    public class ChargesTypeCustomFilter
    {
        public int Tenant { get; set; }
        public ChargesTypeCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<ChargesType> GetFilteredQuery(QueryOperations operations, IQueryable<ChargesType> queryableData)
        {
            
            bool IsInterestFeatureVlid= SecurityUtility.CheckFeature("InterestReport", "Module", this.Tenant);
            if (!IsInterestFeatureVlid)
            {
                queryableData=queryableData.Where(s => s.Code != "INT");
            }
            
            return queryableData;
        }
    }
}
