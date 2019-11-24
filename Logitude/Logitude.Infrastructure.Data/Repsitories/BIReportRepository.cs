 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class BIReportRepository:IRepository<BIReport>
   {
        
		public List<BIReport> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public bool DoesReportExist(string name,string folderId, int tenant)
        {
            return (from a in context.BIReports
                    where a.Name == name.ToLower() && a.BIReportFolderId == folderId && a.Tenant == tenant
                    select a).Any();
        }


    }

}
   