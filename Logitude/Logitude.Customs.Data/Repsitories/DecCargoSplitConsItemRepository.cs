 
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
   public partial class DecCargoSplitConsItemRepository:IRepository<DecCargoSplitConsItem>
   {
        
		public List<DecCargoSplitConsItem> GetMulti(EntityKeyFields entityKeys)
        {
            DecCargoSplitConKeys decCargoSplitConKeys = entityKeys as DecCargoSplitConKeys;

            return (from a in context.DecCargoSplitConsItems
                    where a.DeclarationCargoSplitId == decCargoSplitConKeys.DeclarationCargoSplitId
                    && a.DecCargoSplitConsLineNo == decCargoSplitConKeys.LineNumber
            select a).ToList();
        }

        public void FastDeleteMulti(DecCargoSplitConKeys entityKeyFields)
        {
            (context as DbContextBase)
                .DeleteWhere<DecCargoSplitConsItem>(rec => rec.DeclarationCargoSplitId == entityKeyFields.DeclarationCargoSplitId && rec.DecCargoSplitConsLineNo == entityKeyFields.LineNumber);
        }
    }

}
   