 
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
   public partial class EntityTypeLookupQueryService: EntityQueryService<EntityTypeLookup,EntityTypeLookupKeys,EntityTypeLookupPM,object,EntityTypeLookupKeys>
   {
   
        EntityTypeLookupRepository repository;
		ICustomContext  context;
        public EntityTypeLookupQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new EntityTypeLookupRepository(context);
            Repository = repository;
            mapping = new EntityTypeLookupDataMapping();
        }

        public EntityTypeLookupQueryService(EntityTypeLookupRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new EntityTypeLookupDataMapping();
        }

        public EntityTypeLookupQueryService(ICustomContext context)
        {
            this.repository = new EntityTypeLookupRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new EntityTypeLookupDataMapping();
        }
		 
		public  EntityTypeLookupPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new EntityTypeLookupKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(EntityTypeLookup entityPOCO)
        {
            EntityTypeLookupKeys entityKeys = new EntityTypeLookupKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 