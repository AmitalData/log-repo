 
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
   public partial class CustomsItemRepository:IRepository<CustomsItem>
   {
        
		public List<CustomsItem> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public CustomsItem GetCustomsItemByClassificationCode(string classificationCode)
        {
            return (from a in context.CustomsItems
                    where a.FullClassification== classificationCode
                    select a).FirstOrDefault();
        }

        public List<CustomsItem> GetAllCustomsItemByClassificationCode(string classificationCode)
        {
            return (from a in context.CustomsItems
                    where a.FullClassification == classificationCode
                    select a).ToList();
        }

    }

}
   