 
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
   public partial class LogisticActionResponseRequestSQueryService: EntityQueryService<LogisticActionResponseRequestS,LogisticActionResponseRequestSKeys,LogisticActionResponseRequestSPM,object,LogisticActionResponseRequestSKeys>
   {
   
        LogisticActionResponseRequestSRepository repository;
		ICustomContext  context;
        public LogisticActionResponseRequestSQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new LogisticActionResponseRequestSRepository(context);
            Repository = repository;
            mapping = new LogisticActionResponseRequestSDataMapping();
        }

        public LogisticActionResponseRequestSQueryService(LogisticActionResponseRequestSRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new LogisticActionResponseRequestSDataMapping();
        }

        public LogisticActionResponseRequestSQueryService(ICustomContext context)
        {
            this.repository = new LogisticActionResponseRequestSRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new LogisticActionResponseRequestSDataMapping();
        }
		 
		public  LogisticActionResponseRequestSPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new LogisticActionResponseRequestSKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(LogisticActionResponseRequestS entityPOCO)
        {
            LogisticActionResponseRequestSKeys entityKeys = new LogisticActionResponseRequestSKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 