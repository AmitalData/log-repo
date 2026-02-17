 
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
   public partial class EntitlementTypeQueryService: EntityQueryService<EntitlementType,EntitlementTypeKeys,EntitlementTypePM,object,EntitlementTypeKeys>
   {
   
        EntitlementTypeRepository repository;
		ICustomContext  context;
        public EntitlementTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new EntitlementTypeRepository(context);
            Repository = repository;
            mapping = new EntitlementTypeDataMapping();
        }

        public EntitlementTypeQueryService(EntitlementTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new EntitlementTypeDataMapping();
        }

        public EntitlementTypeQueryService(ICustomContext context)
        {
            this.repository = new EntitlementTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new EntitlementTypeDataMapping();
        }
		 
		public  EntitlementTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new EntitlementTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(EntitlementType entityPOCO)
        {
            EntitlementTypeKeys entityKeys = new EntitlementTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 