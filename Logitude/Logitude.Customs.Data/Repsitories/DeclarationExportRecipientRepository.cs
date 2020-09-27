 
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
   public partial class DeclarationExportRecipientRepository:IRepository<DeclarationExportRecipient>
   {
        
		public List<DeclarationExportRecipient> GetMulti(EntityKeyFields entityKeys)
        {

            DeclarationKeys declarationKeys = entityKeys as DeclarationKeys;

            return (from a in context.DeclarationExportRecipients
                    where a.DeclarationId == declarationKeys.Id
                    select a).ToList();
        }

        public int? GetMaxCounterKey(string declarationId, int tenant)
        {
            var x = context.DeclarationExportRecipients.Where(a => a.DeclarationId == declarationId);
            return (from a in context.DeclarationExportRecipients
                    where a.DeclarationId == declarationId && a.Tenant == tenant
                    select a).Max(d => (int?)d.LineNumber) ?? 0;
        }
    }

}
   