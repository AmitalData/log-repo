 
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
   public partial class CouriersVatRepository:IRepository<CouriersVat>
   {
        
		public List<CouriersVat> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public CouriersVat GetByVatNumber(string code, string entityId)
        {

            var vat = (from a in context.CouriersVats
                    where a.VatNumber == code 
                       && a.Id != entityId
                       select a).FirstOrDefault();

            return vat;
        }

    }

}
   