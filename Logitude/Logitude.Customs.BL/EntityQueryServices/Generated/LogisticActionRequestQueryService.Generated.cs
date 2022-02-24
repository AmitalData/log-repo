 
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
   public partial class LogisticActionRequestQueryService: EntityQueryService<LogisticActionRequest,LogisticActionRequestKeys,LogisticActionRequestPM,object,LogisticActionRequestKeys>
   {
   
        LogisticActionRequestRepository repository;
		ICustomContext  context;
        public LogisticActionRequestQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new LogisticActionRequestRepository(context);
            Repository = repository;
            mapping = new LogisticActionRequestDataMapping();
        }

        public LogisticActionRequestQueryService(LogisticActionRequestRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new LogisticActionRequestDataMapping();
        }

        public LogisticActionRequestQueryService(ICustomContext context)
        {
            this.repository = new LogisticActionRequestRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new LogisticActionRequestDataMapping();
        }
		 
		public  LogisticActionRequestPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new LogisticActionRequestKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(LogisticActionRequest entityPOCO)
        {
            LogisticActionRequestKeys entityKeys = new LogisticActionRequestKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 