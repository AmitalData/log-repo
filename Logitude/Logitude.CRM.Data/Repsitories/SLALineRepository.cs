
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
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class SLALineRepository:IRepository<SLALine>
   {

       public List<SLALine> GetMulti(EntityKeyFields entityKeys)
       {
           SLAHeaderKeys myEntityKeys = entityKeys as SLAHeaderKeys;
           return (from a in context.SLALines where a.SLAHeaderId == myEntityKeys.Id select a).ToList();
       }

       public SLALine GetSLALineBySeverityId(int tenant, string severityId, string slaID)
       {
            if (!string.IsNullOrEmpty(slaID))
            {
                return (context.SLALines.Where(d => d.Tenant == tenant && d.SeverityId == severityId && d.SLAHeaderId == slaID)).FirstOrDefault();
            }
            else
            {
                return (context.SLALines.Where(d => d.Tenant == tenant && d.SeverityId == severityId)).FirstOrDefault();
            }
        }
   }
}
   