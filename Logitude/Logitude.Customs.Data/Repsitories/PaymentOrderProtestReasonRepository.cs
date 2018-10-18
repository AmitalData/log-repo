 
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
   public partial class PaymentOrderProtestReasonRepository:IRepository<PaymentOrderProtestReason>
   {
        
		public List<PaymentOrderProtestReason> GetMulti(EntityKeyFields entityKeys)
        {
            PaymentOrderKeys keys = entityKeys as PaymentOrderKeys;
            return (from a in context.PaymentOrderProtestReasons
                    where a.PaymentOrderId == keys.Id
                    select a).ToList();
        }

   }

}
   