 
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
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class DeficitConnFileParagraphTypeRepository:IRepository<DeficitConnFileParagraphType>
   {
        
		public List<DeficitConnFileParagraphType> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<DeficitConnFileParagraphType> GetDeficitConnectedFileParagraphTypesByDeclarationId(string declarationId, string deficitId, int tenant)
        {
            return (from a in context.DeficitConnFileParagraphTypes.Include("ParagraphType")
                    where a.DeclarationId == declarationId && a.DeficitId == deficitId && a.Tenant == tenant
                    select a).ToList();
        }

   }

}
   