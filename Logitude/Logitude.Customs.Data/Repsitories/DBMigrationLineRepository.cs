 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class DBMigrationLineRepository:IRepository<DBMigrationLine>
   {
        
		public List<DBMigrationLine> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public DBMigrationLine GetLastExec(string DBMigrationId)
        {
            var q =
            this
             .GetAll()
                .Where(r => r.DBMigrationId == DBMigrationId)
            .OrderByDescending(r => r.CounterKey);

            var res=q.FirstOrDefault();
            return res;

        }
    }

}
   