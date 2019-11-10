 
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
   public partial class DeclarationPaymentProtestRepository:IRepository<DeclarationPaymentProtest>
   {
        
		public List<DeclarationPaymentProtest> GetMulti(EntityKeyFields entityKeys)
        {

            DeclarationPaymentKeys keys = entityKeys as DeclarationPaymentKeys;
            return (from a in context.DeclarationPaymentProtests
                    where a.DeclarationId == keys.DeclarationId
                    select a).ToList();
        }

        public bool AnyDeclarationPaymentProtest(string declarationId, int tenant)
        {
            return this.GetAll(tenant).Any(r => r.DeclarationId == declarationId);
        }
    }

}
   