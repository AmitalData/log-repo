 
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
   public partial class GLAccountCounterRepository:IRepository<GLAccountCounter>
   {

		public List<GLAccountCounter> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public GLAccountCounter GetByPrefix(string prefix, int tenant)
        {
            GLAccountCounter entity;

            entity = (from a in context.GLAccountCounters
                      where a.Prefix == prefix && a.Tenant == tenant
                      select a).FirstOrDefault();

            return entity;
        }


    }

}
   