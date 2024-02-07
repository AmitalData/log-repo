 
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
   public partial class RequestToAdvanceAQueueQueryService: EntityQueryService<RequestToAdvanceAQueue,RequestToAdvanceAQueueKeys,RequestToAdvanceAQueuePM,object,RequestToAdvanceAQueueKeys>
   {
   
        RequestToAdvanceAQueueRepository repository;
		ICustomContext  context;
        public RequestToAdvanceAQueueQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new RequestToAdvanceAQueueRepository(context);
            Repository = repository;
            mapping = new RequestToAdvanceAQueueDataMapping();
        }

        public RequestToAdvanceAQueueQueryService(RequestToAdvanceAQueueRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RequestToAdvanceAQueueDataMapping();
        }

        public RequestToAdvanceAQueueQueryService(ICustomContext context)
        {
            this.repository = new RequestToAdvanceAQueueRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RequestToAdvanceAQueueDataMapping();
        }
		 
		public  RequestToAdvanceAQueuePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RequestToAdvanceAQueueKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(RequestToAdvanceAQueue entityPOCO)
        {
            RequestToAdvanceAQueueKeys entityKeys = new RequestToAdvanceAQueueKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 