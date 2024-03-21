 
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
   public partial class CB_CustomsItemDetailsHistoryQueryService: EntityQueryService<CB_CustomsItemDetailsHistory,CB_CustomsItemDetailsHistoryKeys,CB_CustomsItemDetailsHistoryPM,CB_CustomsItemPM,CB_CustomsItemKeys>
   {
   
        CB_CustomsItemDetailsHistoryRepository repository;
		ICustomContext  context;
        public CB_CustomsItemDetailsHistoryQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_CustomsItemDetailsHistoryRepository(context);
            Repository = repository;
            mapping = new CB_CustomsItemDetailsHistoryDataMapping();
        }

        public CB_CustomsItemDetailsHistoryQueryService(CB_CustomsItemDetailsHistoryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_CustomsItemDetailsHistoryDataMapping();
        }

        public CB_CustomsItemDetailsHistoryQueryService(ICustomContext context)
        {
            this.repository = new CB_CustomsItemDetailsHistoryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_CustomsItemDetailsHistoryDataMapping();
        }
		 
		public  CB_CustomsItemDetailsHistoryPM GetSingle(string cb_id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_CustomsItemDetailsHistoryKeys(){ CB_ID = cb_id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_CustomsItemDetailsHistory entityPOCO)
        {
            CB_CustomsItemDetailsHistoryKeys entityKeys = new CB_CustomsItemDetailsHistoryKeys() { CB_ID = entityPOCO.CB_ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 