 
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
   public partial class DecCargoSplitCargoIdentifierRepository:IRepository<DecCargoSplitCargoIdentifier>
   {
        
		public List<DecCargoSplitCargoIdentifier> GetMulti(EntityKeyFields entityKeys)
        {

            DeclarationCargoSplitKeys declarationCargoSplitKeys = entityKeys as DeclarationCargoSplitKeys;

            return (from a in context.DecCargoSplitCargoIdentifiers
                    where a.DeclarationCargoSplitId == declarationCargoSplitKeys.Id
                    select a).ToList();
        }

        public void FastDeleteMulti(DeclarationCargoSplitKeys entityKeyFields)
        {
            (context as DbContextBase)
                .DeleteWhere<DecCargoSplitCargoIdentifier>(rec => rec.DeclarationCargoSplitId == entityKeyFields.Id);
        }

    }

}
   