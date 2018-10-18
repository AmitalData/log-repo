 
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
   public partial class ClaimImporterDeclarsPage3Repository:IRepository<ClaimImporterDeclarsPage3>
   {
        
		public List<ClaimImporterDeclarsPage3> GetMulti(EntityKeyFields entityKeys)
        {
            ClaimKeys claimKeys = entityKeys as ClaimKeys;

            return (from a in context.ClaimImporterDeclarsPage3s
                    where a.ClaimId == claimKeys.Id
                    select a).ToList();
        }

   }

}
   