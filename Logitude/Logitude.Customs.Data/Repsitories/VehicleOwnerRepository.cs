 
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
   public partial class VehicleOwnerRepository:IRepository<VehicleOwner>
   {
        
		public List<VehicleOwner> GetMulti(EntityKeyFields entityKeys)
        {

            VehicleKeys vehicleKeys = entityKeys as VehicleKeys;

            return (from a in context.VehicleOwners
                    where a.VehicleId == vehicleKeys.Id
                    select a).ToList();
        }

   }

}
   