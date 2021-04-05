
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class GLAccountAgingDataQueryService : EntityQueryService<GLAccountAgingData, GLAccountAgingDataKeys, GLAccountAgingDataPM, object, GLAccountAgingDataKeys>
    {
        public List<GLAccountAgingDataPM> Get(int tenant,IQueryable<GLAccount> _qAllAccAging4AccountTypeCode_CustomerOrVendor)
        {
            var q = (from acc in _qAllAccAging4AccountTypeCode_CustomerOrVendor
                     join agging in this.repository.GetAll(tenant)
                     on acc.Id equals agging.AccountId into agingGroup
                     from agging in agingGroup//.DefaultIfEmpty()
                     select new GLAccountAgingDataPM()
                     {
                         AccountId = acc.Id,
                         Tenant = acc.Tenant,
                         ///ExistInDB= agging != null,
                         //Period0 = agging == null ? null : agging.Period0,
                         //Period1 = agging == null ? null : agging.Period1,
                         //Period2 = agging == null ? null : agging.Period2,
                         //Period3 = agging == null ? null : agging.Period3,
                         //Period4 = agging == null ? null : agging.Period4,
                         //Period5 = agging == null ? null : agging.Period5,
                         //TotalOpenTransactions = agging == null ? 0 : agging.TotalOpenTransactions,
                         PeriodPast= agging.PeriodPast,
                         Period0 = agging.Period0,
                         Period1 = agging.Period1,
                         Period2 = agging.Period2,
                         Period3 = agging.Period3,
                         Period4 = agging.Period4,
                         Period5 = agging.Period5,
                         PeriodFuture= agging.PeriodFuture,
                         TotalOpenTransactions = agging.TotalOpenTransactions,
                     }
             );
            return q.ToList();
                
        }
    }
}
