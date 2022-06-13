 
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
   public partial class AvailableStatusFieldRepository:IRepository<AvailableStatusField>
   {
        
		public List<AvailableStatusField> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public string GetAvailableFieldByStatusFieldType(int tenant,string statusFieldType)
        {
            return (from a in context.AvailableStatusFields
                               where a.Tenant == tenant && a.StatusFieldType == statusFieldType && a.IsAvailable ==true
                               select a.FieldCode).FirstOrDefault();
            
        }

   }

}
   