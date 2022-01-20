 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CargoTracking.Data.Repositories
{
   public partial class CargoDisconnectQueueRepository:IRepository<CargoDisconnectQueue>
   {
        
		public List<CargoDisconnectQueue> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

   }

}
   