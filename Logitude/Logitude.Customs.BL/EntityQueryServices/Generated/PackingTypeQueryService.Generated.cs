 
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
   public partial class PackingTypeQueryService: EntityQueryService<PackingType,PackingTypeKeys,PackingTypePM,object,PackingTypeKeys>
   {
   
        PackingTypeRepository repository;
		ICustomContext  context;
        public PackingTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new PackingTypeRepository(context);
            Repository = repository;
            mapping = new PackingTypeDataMapping();
        }

        public PackingTypeQueryService(PackingTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PackingTypeDataMapping();
        }

        public PackingTypeQueryService(ICustomContext context)
        {
            this.repository = new PackingTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PackingTypeDataMapping();
        }
		 
		public  PackingTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PackingTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PackingType entityPOCO)
        {
            PackingTypeKeys entityKeys = new PackingTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 