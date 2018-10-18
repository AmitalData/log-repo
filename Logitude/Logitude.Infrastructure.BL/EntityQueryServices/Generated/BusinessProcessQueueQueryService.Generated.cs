 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityDataMappings;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Infrastructure.BL.EntityQueryServices
{ 
   public partial class BusinessProcessQueueQueryService: EntityQueryService<BusinessProcessQueue,BusinessProcessQueueKeys,BusinessProcessQueuePM,object,BusinessProcessQueueKeys>
   {
   
        BusinessProcessQueueRepository repository;
		IInfrastructureContext  context;
        public BusinessProcessQueueQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new BusinessProcessQueueRepository(context);
            Repository = repository;
            mapping = new BusinessProcessQueueDataMapping();
        }

        public BusinessProcessQueueQueryService(BusinessProcessQueueRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BusinessProcessQueueDataMapping();
        }

        public BusinessProcessQueueQueryService(IInfrastructureContext context)
        {
            this.repository = new BusinessProcessQueueRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BusinessProcessQueueDataMapping();
        }
		 
		public  BusinessProcessQueuePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BusinessProcessQueueKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BusinessProcessQueue entityPOCO)
        {
            BusinessProcessQueueKeys entityKeys = new BusinessProcessQueueKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 