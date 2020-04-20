 
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
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CustomsRequiredFieldRepository:IRepository<CustomsRequiredField>
   {
        
		public List<CustomsRequiredField> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<CustomsRequiredField> GetCustomRequiredFieldsByObjectTable(string ObjectTableId, int Tenant)
        {
            List<CustomsRequiredField> requiredFields;

            requiredFields = (from a in context.CustomsRequiredFields
                              where a.ObjectTableId == ObjectTableId && a.Tenant == Tenant
                              select a).ToList();
            return requiredFields;
        }

        public CustomsRequiredField GetCustomRequiredFieldsByObjectFieldCode(string ObjectFieldCode, int Tenant)
        {
            CustomsRequiredField requiredFields;

            requiredFields = (from a in context.CustomsRequiredFields
                              where a.ObjectfieldCode == ObjectFieldCode && a.Tenant == Tenant
                              select a).FirstOrDefault();
            return requiredFields;
        }

    }

}
   