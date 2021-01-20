 
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
   public partial class LoadingSiteTypeQueryService: EntityQueryService<LoadingSiteType,LoadingSiteTypeKeys,LoadingSiteTypePM,object,LoadingSiteTypeKeys>
   {
   
        LoadingSiteTypeRepository repository;
		ICustomContext  context;
        public LoadingSiteTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new LoadingSiteTypeRepository(context);
            Repository = repository;
            mapping = new LoadingSiteTypeDataMapping();
        }

        public LoadingSiteTypeQueryService(LoadingSiteTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new LoadingSiteTypeDataMapping();
        }

        public LoadingSiteTypeQueryService(ICustomContext context)
        {
            this.repository = new LoadingSiteTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new LoadingSiteTypeDataMapping();
        }
		 
		public  LoadingSiteTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new LoadingSiteTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(LoadingSiteType entityPOCO)
        {
            LoadingSiteTypeKeys entityKeys = new LoadingSiteTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 