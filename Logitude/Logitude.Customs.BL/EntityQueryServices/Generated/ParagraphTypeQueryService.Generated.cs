 
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
   public partial class ParagraphTypeQueryService: EntityQueryService<ParagraphType,ParagraphTypeKeys,ParagraphTypePM,object,ParagraphTypeKeys>
   {
   
        ParagraphTypeRepository repository;
		ICustomContext  context;
        public ParagraphTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ParagraphTypeRepository(context);
            Repository = repository;
            mapping = new ParagraphTypeDataMapping();
        }

        public ParagraphTypeQueryService(ParagraphTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ParagraphTypeDataMapping();
        }

        public ParagraphTypeQueryService(ICustomContext context)
        {
            this.repository = new ParagraphTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ParagraphTypeDataMapping();
        }
		 
		public  ParagraphTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ParagraphTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ParagraphType entityPOCO)
        {
            ParagraphTypeKeys entityKeys = new ParagraphTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 