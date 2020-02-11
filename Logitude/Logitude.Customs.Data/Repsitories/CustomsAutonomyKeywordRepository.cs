 
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
   public partial class CustomsAutonomyKeywordRepository:IRepository<CustomsAutonomyKeyword>
   {
        
		public List<CustomsAutonomyKeyword> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public CustomsAutonomyKeyword GetByKeywordtypeCode(string keywordtypeCode, int tenant)
        {
            return
            this
                .GetAll(tenant)
                .FirstOrDefault(r => r.KeywordtypeCode == keywordtypeCode)
                ;
        }
    }

}
   