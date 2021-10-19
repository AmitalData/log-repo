 
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

        public List< CustomsAutonomyKeyword> GetByKeywordtypeCodeList(string keywordtypeCode, int tenant)
        {
            return
            this
                .GetAll(tenant)
                .Where(r => r.KeywordtypeCode == keywordtypeCode).ToList()
                ;
        }

        public List<string> GetByKeywordtypeCodeStringList(string keywordtypeCode, int tenant)
        {
                var Keywordtype = (from a in context.CustomsAutonomyKeywords
                                    where a.Tenant == tenant
                                    where a.KeywordtypeCode == keywordtypeCode
                                   select a.KeywordsList
                                                        );

                return Keywordtype.ToList();
        }

        public CustomsAutonomyKeyword  GetByKeywordtypeCode(string keywordtypeCode, int tenant)
        {
            return
            this
                .GetAll(tenant)
                .FirstOrDefault(r => r.KeywordtypeCode == keywordtypeCode)
                ;
        }
        public CustomsAutonomyKeyword GetBykeywordList(string keywordsList, int tenant)
        {
            return 
            this.GetAll(tenant).FirstOrDefault(r => r.KeywordsList == keywordsList)
                ;
        }
    }

}
   