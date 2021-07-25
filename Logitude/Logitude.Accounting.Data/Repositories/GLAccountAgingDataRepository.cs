 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class GLAccountAgingDataRepository:IRepository<GLAccountAgingData>
   {
        
		public List<GLAccountAgingData> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public IQueryable<GLAccountAgingData> GetByIds(int tenant , IQueryable<string> idS)
        {

            var q=(from agingDataRow in GetAll(tenant)
             join accountId in idS on agingDataRow.AccountId equals accountId
             select agingDataRow
                 );
            return q;//.ToList();
        }
    }

}
   