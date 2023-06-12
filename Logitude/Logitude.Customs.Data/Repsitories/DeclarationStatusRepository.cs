 
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
   public partial class DeclarationStatusRepository:IRepository<DeclarationStatus>
   {
        public List<DeclarationStatus> GetByDeclarationIdAndTenant(int tenant, string declarationId)
        {
            return (from a in context.DeclarationStatuses
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).ToList();
        }
        public List<string> GetIdsByDeclarationIdAndTenant(int tenant, string declarationId)
        {
            return (from a in context.DeclarationStatuses
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a.DeclarationId).ToList();
        }
        public void DeleteByIdAndTenant(string decId,int tenant)
        {
            (context as DbContextBase)
                .DeleteWhere<DeclarationStatus>(rec => rec.DeclarationId == decId && rec.Tenant == tenant);
        }

        public List<DeclarationStatus> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

   }

}
   