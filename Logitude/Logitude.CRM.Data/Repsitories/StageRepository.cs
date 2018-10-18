 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class StageRepository:IRepository<Stage>
   {
        
		public List<Stage> GetMulti(EntityKeyFields entityKeys)
        {            
			throw new NotImplementedException();
        }

        public Stage GetStageByCode(string code, int tenant)
        {
            return (from a in context.Stages where a.Code == code && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<Stage> GetStagesByTenant(int tenant)
        {
            IQueryable<Stage> stages = from a in context.Stages
                                               where a.Tenant == tenant
                                               select a;
            return stages;
        }

   }

}
   