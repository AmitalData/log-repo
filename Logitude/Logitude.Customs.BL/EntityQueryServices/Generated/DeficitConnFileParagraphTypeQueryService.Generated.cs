 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class DeficitConnFileParagraphTypeQueryService: EntityQueryService<DeficitConnFileParagraphType,DeficitConnFileParagraphTypeKeys,DeficitConnFileParagraphTypePM,object,DeficitConnFileParagraphTypeKeys>
   {
   
        DeficitConnFileParagraphTypeRepository repository;
		ICustomContext  context;
        public DeficitConnFileParagraphTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeficitConnFileParagraphTypeRepository(context);
            Repository = repository;
            mapping = new DeficitConnFileParagraphTypeDataMapping();
        }

        public DeficitConnFileParagraphTypeQueryService(DeficitConnFileParagraphTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeficitConnFileParagraphTypeDataMapping();
        }

        public DeficitConnFileParagraphTypeQueryService(ICustomContext context)
        {
            this.repository = new DeficitConnFileParagraphTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeficitConnFileParagraphTypeDataMapping();
        }
		 
		public  DeficitConnFileParagraphTypePM GetSingle(string deficitid, string declarationid, string paragraphtypecode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeficitConnFileParagraphTypeKeys(){ DeficitId = deficitid, DeclarationId = declarationid, ParagraphTypeCode = paragraphtypecode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeficitConnFileParagraphType entityPOCO)
        {
            DeficitConnFileParagraphTypeKeys entityKeys = new DeficitConnFileParagraphTypeKeys() { DeficitId = entityPOCO.DeficitId, DeclarationId = entityPOCO.DeclarationId, ParagraphTypeCode = entityPOCO.ParagraphTypeCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 