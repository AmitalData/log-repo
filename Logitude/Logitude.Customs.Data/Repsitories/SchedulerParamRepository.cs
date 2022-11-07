 
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
   public partial class SchedulerParamRepository:IRepository<SchedulerParam>
   {
        
		public List<SchedulerParam> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public List<SchedulerParam> GetAllByProcedureCode(int tenant,string schedulerProcedureCode)
        {
            return  (from SchedulerParam in context.SchedulerParams
                     where SchedulerParam.SchedulerProcedureCode == schedulerProcedureCode && SchedulerParam.Tenant==tenant
                     select SchedulerParam).ToList();
        }

    }

}
   