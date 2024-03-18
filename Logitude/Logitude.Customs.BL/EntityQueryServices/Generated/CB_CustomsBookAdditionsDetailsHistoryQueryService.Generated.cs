 
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
   public partial class CB_CustomsBookAdditionsDetailsHistoryQueryService: EntityQueryService<CB_CustomsBookAdditionsDetailsHistory,CB_CustomsBookAdditionsDetailsHistoryKeys,CB_CustomsBookAdditionsDetailsHistoryPM,object,CB_CustomsBookAdditionsDetailsHistoryKeys>
   {
   
        CB_CustomsBookAdditionsDetailsHistoryRepository repository;
		ICustomContext  context;
        public CB_CustomsBookAdditionsDetailsHistoryQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_CustomsBookAdditionsDetailsHistoryRepository(context);
            Repository = repository;
            mapping = new CB_CustomsBookAdditionsDetailsHistoryDataMapping();
        }

        public CB_CustomsBookAdditionsDetailsHistoryQueryService(CB_CustomsBookAdditionsDetailsHistoryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_CustomsBookAdditionsDetailsHistoryDataMapping();
        }

        public CB_CustomsBookAdditionsDetailsHistoryQueryService(ICustomContext context)
        {
            this.repository = new CB_CustomsBookAdditionsDetailsHistoryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_CustomsBookAdditionsDetailsHistoryDataMapping();
        }
		 
		public  CB_CustomsBookAdditionsDetailsHistoryPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_CustomsBookAdditionsDetailsHistoryKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_CustomsBookAdditionsDetailsHistory entityPOCO)
        {
            CB_CustomsBookAdditionsDetailsHistoryKeys entityKeys = new CB_CustomsBookAdditionsDetailsHistoryKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 