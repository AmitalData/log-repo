 
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
   public partial class ExternalFieldMappingRepository:IRepository<ExternalFieldMapping>
   {
        
		public List<ExternalFieldMapping> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }
        public ExternalFieldMapping GetSingleByStatusFieldTypeAndStatusCode(string statusFieldType,string statusCode,int tenant)
        {
            var q = from a in context.ExternalFieldMappings
                    where
                    a.Tenant == tenant &&
                    a.StatusFieldType == statusFieldType &&
                    a.StatusCode == statusCode
                    select a;
            return q.FirstOrDefault();
        }
        public ExternalFieldMapping GetSingleByField(string field, int tenant)
        {
            var q = from a in context.ExternalFieldMappings
                    where
                    a.Tenant == tenant &&
                    a.Field == field 
                    select a;
            return q.FirstOrDefault();
        }

    }

}
   