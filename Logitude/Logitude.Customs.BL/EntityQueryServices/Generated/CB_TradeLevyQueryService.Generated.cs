 
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
   public partial class CB_TradeLevyQueryService: EntityQueryService<CB_TradeLevy,CB_TradeLevyKeys,CB_TradeLevyPM,object,CB_TradeLevyKeys>
   {
   
        CB_TradeLevyRepository repository;
		ICustomContext  context;
        public CB_TradeLevyQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CB_TradeLevyRepository(context);
            Repository = repository;
            mapping = new CB_TradeLevyDataMapping();
        }

        public CB_TradeLevyQueryService(CB_TradeLevyRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CB_TradeLevyDataMapping();
        }

        public CB_TradeLevyQueryService(ICustomContext context)
        {
            this.repository = new CB_TradeLevyRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CB_TradeLevyDataMapping();
        }
		 
		public  CB_TradeLevyPM GetSingle(int id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CB_TradeLevyKeys(){ ID = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CB_TradeLevy entityPOCO)
        {
            CB_TradeLevyKeys entityKeys = new CB_TradeLevyKeys() { ID = entityPOCO.ID,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 