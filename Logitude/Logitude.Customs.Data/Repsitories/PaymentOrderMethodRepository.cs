 
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
   public partial class PaymentOrderMethodRepository:IRepository<PaymentOrderMethod>
   {
        
		public List<PaymentOrderMethod> GetMulti(EntityKeyFields entityKeys)
        {

            PaymentOrderKeys keys = entityKeys as PaymentOrderKeys;
            return (from a in context.PaymentOrderMethods
                    where a.PaymentOrderId == keys.Id
                    select a).ToList();
        }

   }

}
   