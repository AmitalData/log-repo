 
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
   public partial class CargoSealIdentifierRepository:IRepository<CargoSealIdentifier>
   {
        
		public List<CargoSealIdentifier> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<CargoSealIdentifier> GetDeclarationCargoSealIdentifierList(string declarationId, int tenant)
        {
            return (from a in context.CargoSealIdentifiers
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).ToList();
        }

    }

}
   