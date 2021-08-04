 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.ShipmentOrderLib.Data.EntityPOCOs;
using Logitude.ShipmentOrderLib.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.ShipmentOrderLib.Data.Repositories
{
   public partial class ShipmentOrderRepository:IRepository<ShipmentOrder>
   {
        
		public List<ShipmentOrder> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

   }

}
   