 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class SLAEscalationRecepientRepository:IRepository<SLAEscalationRecepient>
   {
        
		public List<SLAEscalationRecepient> GetMulti(EntityKeyFields entityKeys)
        {
            SLAEscalationKeys myEntityKeys = entityKeys as SLAEscalationKeys;
            return (from a in context.SLAEscalationRecepients where a.SLAEscalationId == myEntityKeys.Id select a).ToList();
        }


        public List<SLAEscalationRecepient> GetSLAEscalationRecepientByEscalationId(string id, int tenant)
        {
            return (from a in context.SLAEscalationRecepients
                    where a.SLAEscalationId == id && a.Tenant == tenant
                    select a).ToList();
        }
   }

}
   