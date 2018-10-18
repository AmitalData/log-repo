 
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
   public partial class DeclarationPaymentMethodRepository:IRepository<DeclarationPaymentMethod>
   {
        
		public List<DeclarationPaymentMethod> GetMulti(EntityKeyFields entityKeys)
        {

            DeclarationPaymentKeys keys = entityKeys as DeclarationPaymentKeys;
            return (from a in context.DeclarationPaymentMethods
                    where a.DeclarationId == keys.DeclarationId
                    select a).ToList();
        }

   }

}
   