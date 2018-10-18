 
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
   public partial class DecCargoSplitConsPackDetRepository:IRepository<DecCargoSplitConsPackDet>
   {
        
		public List<DecCargoSplitConsPackDet> GetMulti(EntityKeyFields entityKeys)
        {

            DecCargoSplitConsItemKeys decCargoSplitConsItemKeys = entityKeys as DecCargoSplitConsItemKeys;

            return (from a in context.DecCargoSplitConsPackDets
                    where a.DeclarationCargoSplitId == decCargoSplitConsItemKeys.DeclarationCargoSplitId
                    && a.DecCargoSplitConsLineNo == decCargoSplitConsItemKeys.DecCargoSplitConsLineNo
                    && a.DecCargoSplitConsItemLine == decCargoSplitConsItemKeys.ItemLine
                    select a).ToList();
        }

        public void FastDeleteMulti(DecCargoSplitConsItemKeys entityKeyFields)
        {
            (context as DbContextBase)
                .DeleteWhere<DecCargoSplitConsPackDet>(rec => rec.DeclarationCargoSplitId == entityKeyFields.DeclarationCargoSplitId && rec.DecCargoSplitConsLineNo == entityKeyFields.DecCargoSplitConsLineNo
                    && rec.DecCargoSplitConsItemLine == entityKeyFields.ItemLine);
        }
    }

}
   