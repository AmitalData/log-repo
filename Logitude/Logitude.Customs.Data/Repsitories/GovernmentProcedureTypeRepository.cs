 
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
   public partial class GovernmentProcedureTypeRepository:IRepository<GovernmentProcedureType>
   {
        
		public List<GovernmentProcedureType> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
		public GovernmentProcedureType GetSingleGovernmentProcedureType(EntityKeyFields entityKeys)
		{
			GovernmentProcedureTypeKeys keys = entityKeys as GovernmentProcedureTypeKeys;
			return (from a in context.GovernmentProcedureTypes
					where a.Code == keys.Code
					select a).FirstOrDefault();
		}

	}

}
   