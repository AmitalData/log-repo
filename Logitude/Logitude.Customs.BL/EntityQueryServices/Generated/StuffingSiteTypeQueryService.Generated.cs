 
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
   public partial class StuffingSiteTypeQueryService: EntityQueryService<StuffingSiteType,StuffingSiteTypeKeys,StuffingSiteTypePM,object,StuffingSiteTypeKeys>
   {
   
        StuffingSiteTypeRepository repository;
		ICustomContext  context;
        public StuffingSiteTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new StuffingSiteTypeRepository(context);
            Repository = repository;
            mapping = new StuffingSiteTypeDataMapping();
        }

        public StuffingSiteTypeQueryService(StuffingSiteTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new StuffingSiteTypeDataMapping();
        }

        public StuffingSiteTypeQueryService(ICustomContext context)
        {
            this.repository = new StuffingSiteTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new StuffingSiteTypeDataMapping();
        }
		 
		public  StuffingSiteTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new StuffingSiteTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(StuffingSiteType entityPOCO)
        {
            StuffingSiteTypeKeys entityKeys = new StuffingSiteTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 