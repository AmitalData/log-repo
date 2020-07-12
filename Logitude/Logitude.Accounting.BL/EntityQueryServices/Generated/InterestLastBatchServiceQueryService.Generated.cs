 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class InterestLastBatchServiceQueryService: EntityQueryService<InterestLastBatchService,InterestLastBatchServiceKeys,InterestLastBatchServicePM,object,InterestLastBatchServiceKeys>
   {
   
        InterestLastBatchServiceRepository repository;
		IAccountingContext  context;
        public InterestLastBatchServiceQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new InterestLastBatchServiceRepository(context);
            Repository = repository;
            mapping = new InterestLastBatchServiceDataMapping();
        }

        public InterestLastBatchServiceQueryService(InterestLastBatchServiceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InterestLastBatchServiceDataMapping();
        }

        public InterestLastBatchServiceQueryService(IAccountingContext context)
        {
            this.repository = new InterestLastBatchServiceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InterestLastBatchServiceDataMapping();
        }
		 
		public  InterestLastBatchServicePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InterestLastBatchServiceKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InterestLastBatchService entityPOCO)
        {
            InterestLastBatchServiceKeys entityKeys = new InterestLastBatchServiceKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 