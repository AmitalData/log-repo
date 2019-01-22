 
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
   public partial class DeclarationMamanSpecialActionRepository:IRepository<DeclarationMamanSpecialAction>
   {
        
		public List<DeclarationMamanSpecialAction> GetMulti(EntityKeyFields entityKeys)
        {

            DeclarationMamanSpecialActionKeys mamanSpecialActionKeys = entityKeys as DeclarationMamanSpecialActionKeys;

            return (from a in context.DeclarationMamanSpecialActions
                    where a.DeclarationId == mamanSpecialActionKeys.DeclarationId
                    select a).ToList();
        }

        public List<DeclarationMamanSpecialAction> GetDeclarationMamanSpecialActionByDeclarationId(string declarationId, int tenant)
        {
            return (from a in context.DeclarationMamanSpecialActions
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).ToList();
        }

    }

}
   