 
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
   public partial class PaymentOrderLineRepository:IRepository<PaymentOrderLine>
   {
        
		public List<PaymentOrderLine> GetMulti(EntityKeyFields entityKeys)
        {
            PaymentOrderKeys keys=entityKeys as PaymentOrderKeys;
            return (from a in context.PaymentOrderLines
                    where a.PaymentOrderId == keys.Id
                    select a).ToList();
        }

   }

}
   