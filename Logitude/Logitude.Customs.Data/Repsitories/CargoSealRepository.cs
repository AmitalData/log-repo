 
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
   public partial class CargoSealRepository:IRepository<CargoSeal>
   {
        
		public List<CargoSeal> GetMulti(EntityKeyFields entityKeys)
        {
            CargoSealIdentifierKeys cargoSealIdentifierKeys = entityKeys as CargoSealIdentifierKeys;

            return (from a in context.CargoSeals
                    where a.CargoSealIdentifierId == cargoSealIdentifierKeys.Id
                    select a).ToList();
        }

   }

}
   