 
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
   public partial class LogisticActionRequestTypeQueryService: EntityQueryService<LogisticActionRequestType,LogisticActionRequestTypeKeys,LogisticActionRequestTypePM,object,LogisticActionRequestTypeKeys>
   {
   
        LogisticActionRequestTypeRepository repository;
		ICustomContext  context;
        public LogisticActionRequestTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new LogisticActionRequestTypeRepository(context);
            Repository = repository;
            mapping = new LogisticActionRequestTypeDataMapping();
        }

        public LogisticActionRequestTypeQueryService(LogisticActionRequestTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new LogisticActionRequestTypeDataMapping();
        }

        public LogisticActionRequestTypeQueryService(ICustomContext context)
        {
            this.repository = new LogisticActionRequestTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new LogisticActionRequestTypeDataMapping();
        }
		 
		public  LogisticActionRequestTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new LogisticActionRequestTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(LogisticActionRequestType entityPOCO)
        {
            LogisticActionRequestTypeKeys entityKeys = new LogisticActionRequestTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 