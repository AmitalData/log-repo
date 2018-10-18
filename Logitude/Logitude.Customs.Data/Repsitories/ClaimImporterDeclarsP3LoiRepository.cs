 
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
   public partial class ClaimImporterDeclarsP3LoiRepository:IRepository<ClaimImporterDeclarsP3Loi>
   {
        
		public List<ClaimImporterDeclarsP3Loi> GetMulti(EntityKeyFields entityKeys)
        {
            ClaimImporterDeclarsPage3Keys claimImporterDeclarsPage3Keys = entityKeys as ClaimImporterDeclarsPage3Keys;

            return (from a in context.ClaimImporterDeclarsP3Lois
                    where a.ClaimId == claimImporterDeclarsPage3Keys.ClaimId && a.CounterKey == claimImporterDeclarsPage3Keys.LineNo
                    select a).ToList();
        }

   }

}
   