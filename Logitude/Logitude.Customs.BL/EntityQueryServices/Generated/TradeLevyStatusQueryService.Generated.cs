 
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
   public partial class TradeLevyStatusQueryService: EntityQueryService<TradeLevyStatus,TradeLevyStatusKeys,TradeLevyStatusPM,object,TradeLevyStatusKeys>
   {
   
        TradeLevyStatusRepository repository;
		ICustomContext  context;
        public TradeLevyStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new TradeLevyStatusRepository(context);
            Repository = repository;
            mapping = new TradeLevyStatusDataMapping();
        }

        public TradeLevyStatusQueryService(TradeLevyStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TradeLevyStatusDataMapping();
        }

        public TradeLevyStatusQueryService(ICustomContext context)
        {
            this.repository = new TradeLevyStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TradeLevyStatusDataMapping();
        }
		 
		public  TradeLevyStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TradeLevyStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TradeLevyStatus entityPOCO)
        {
            TradeLevyStatusKeys entityKeys = new TradeLevyStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 