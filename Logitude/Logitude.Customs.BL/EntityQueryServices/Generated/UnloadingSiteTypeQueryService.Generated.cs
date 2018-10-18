 
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
   public partial class UnloadingSiteTypeQueryService: EntityQueryService<UnloadingSiteType,UnloadingSiteTypeKeys,UnloadingSiteTypePM,object,UnloadingSiteTypeKeys>
   {
   
        UnloadingSiteTypeRepository repository;
		ICustomContext  context;
        public UnloadingSiteTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new UnloadingSiteTypeRepository(context);
            Repository = repository;
            mapping = new UnloadingSiteTypeDataMapping();
        }

        public UnloadingSiteTypeQueryService(UnloadingSiteTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new UnloadingSiteTypeDataMapping();
        }

        public UnloadingSiteTypeQueryService(ICustomContext context)
        {
            this.repository = new UnloadingSiteTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new UnloadingSiteTypeDataMapping();
        }
		 
		public  UnloadingSiteTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new UnloadingSiteTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(UnloadingSiteType entityPOCO)
        {
            UnloadingSiteTypeKeys entityKeys = new UnloadingSiteTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 