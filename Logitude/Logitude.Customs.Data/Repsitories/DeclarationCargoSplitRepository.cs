 
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
   public partial class DeclarationCargoSplitRepository:IRepository<DeclarationCargoSplit>
   {
        
		public List<DeclarationCargoSplit> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<DeclarationCargoSplit> GetDeclarationCargoSplitsList(string declarationId, int tenant)
        {
            return (from a in context.DeclarationCargoSplits
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).ToList();
        }

        public string GetIdByDeclarationCargoSplitRequestNumber(string declarationCargoSplitRequestNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(declarationCargoSplitRequestNumber)) return "";
            return
                  (
                  from rec in context.DeclarationCargoSplits
                  where rec.RequestNumber == declarationCargoSplitRequestNumber && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }
    }

}
   