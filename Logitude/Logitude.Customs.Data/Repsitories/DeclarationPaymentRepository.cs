 
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
   public partial class DeclarationPaymentRepository:IRepository<DeclarationPayment>
   {
        
		public List<DeclarationPayment> GetMulti(EntityKeyFields entityKeys)
        {

            DeclarationKeys declarationKeys = entityKeys as DeclarationKeys;

            return (from a in context.DeclarationPayments
                    where a.DeclarationId == declarationKeys.Id
                    select a).ToList();
        }

        public int GetAutomaticPayment(string declarationid)
        {
            var q = this.context.DeclarationPayments
                .Where(r => r.DeclarationId == declarationid)
                .Select(r => r.AutomaticPayment);

            return q.FirstOrDefault();
        }
    }

}
   