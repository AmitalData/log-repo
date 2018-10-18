using Logitude.CRM.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.Data.CustomFilters
{
    public class CRMFilterSettingCustomFilters
    {
        public static IQueryable<CRMFilterSetting> GetFilteredQuery(QueryOperations operations, IQueryable<CRMFilterSetting> queryableData, int tenant)
        {
            return queryableData;
        }
    }
}
