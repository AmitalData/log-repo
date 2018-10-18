 
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
   public partial class DeclarationErrorMappingQueryService: EntityQueryService<DeclarationErrorMapping,DeclarationErrorMappingKeys,DeclarationErrorMappingPM,object,DeclarationErrorMappingKeys>
   {
   
        DeclarationErrorMappingRepository repository;
		ICustomContext  context;
        public DeclarationErrorMappingQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationErrorMappingRepository(context);
            Repository = repository;
            mapping = new DeclarationErrorMappingDataMapping();
        }

        public DeclarationErrorMappingQueryService(DeclarationErrorMappingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationErrorMappingDataMapping();
        }

        public DeclarationErrorMappingQueryService(ICustomContext context)
        {
            this.repository = new DeclarationErrorMappingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationErrorMappingDataMapping();
        }
		 
		public  DeclarationErrorMappingPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationErrorMappingKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationErrorMapping entityPOCO)
        {
            DeclarationErrorMappingKeys entityKeys = new DeclarationErrorMappingKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 