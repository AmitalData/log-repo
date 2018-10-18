 
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
   public partial class DeclarationConsAcceptanceRepository:IRepository<DeclarationConsAcceptance>
   {
        
		public List<DeclarationConsAcceptance> GetMulti(EntityKeyFields entityKeys)
        {
            DeclarationKeys declarationKeys = entityKeys as DeclarationKeys;

            return (from a in context.DeclarationConsAcceptances
                    where a.DeclarationId == declarationKeys.Id
                    select a).ToList();
        }

        public int? GetMaxCounterKey(string declarationId, int tenant)
        {
            return (from a in context.DeclarationConsAcceptances
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).Max(d => (int?)d.LineNumber) ?? 0;
        }

    }

}
   