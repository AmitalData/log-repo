 
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
   public partial class CB_PropertiesDetailsHistoryQueryService: EntityQueryService<CB_PropertiesDetailsHistory,CB_PropertiesDetailsHistoryKeys,CB_PropertiesDetailsHistoryPM,CB_CustomsItemPM,CB_CustomsItemKeys>
   {
   
        CB_PropertiesDetailsHistoryRepository repository;
		ICustomContext  context;
        public CB_PropertiesDetailsHistoryQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_PropertiesDetailsHistoryRepository(context);
            Repository = repository;
            mapping = new CB_PropertiesDetailsHistoryDataMapping();
        }

        public CB_PropertiesDetailsHistoryQueryService(CB_PropertiesDetailsHistoryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_PropertiesDetailsHistoryDataMapping();
        }

        public CB_PropertiesDetailsHistoryQueryService(ICustomContext context)
        {
            this.repository = new CB_PropertiesDetailsHistoryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_PropertiesDetailsHistoryDataMapping();
        }
		 
		public  CB_PropertiesDetailsHistoryPM GetSingle(int id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_PropertiesDetailsHistoryKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_PropertiesDetailsHistory entityPOCO)
        {
            CB_PropertiesDetailsHistoryKeys entityKeys = new CB_PropertiesDetailsHistoryKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 