 
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
   public partial class DeclarationReferantDataRepository:IRepository<DeclarationReferantData>
   {
        
		public List<DeclarationReferantData> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }


        public DeclarationReferantData GetDeclarationReferandDateByDeclarationIdToDisplay(string declarationId, int tenant)
        {
            return (from a in context.DeclarationReferantDatas
                    where (a.DeclarationId == declarationId || a.DeclarationIdToDisplay == declarationId) && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
    }

}
   