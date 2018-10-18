 
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
   public partial class DeclarationQueryService: EntityQueryService<Declaration,DeclarationKeys,DeclarationPM,object,DeclarationKeys>
   {
   
        DeclarationRepository repository;
		ICustomContext  context;
        public DeclarationQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationRepository(context);
            Repository = repository;
            mapping = new DeclarationDataMapping();
        }

        public DeclarationQueryService(DeclarationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationDataMapping();
        }

        public DeclarationQueryService(ICustomContext context)
        {
            this.repository = new DeclarationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationDataMapping();
        }
		 
		public  DeclarationPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Declaration entityPOCO)
        {
            DeclarationKeys entityKeys = new DeclarationKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 