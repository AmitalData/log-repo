 
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
   public partial class BIReportsExecutionLogQueryService: EntityQueryService<BIReportsExecutionLog,BIReportsExecutionLogKeys,BIReportsExecutionLogPM,object,BIReportsExecutionLogKeys>
   {
   
        BIReportsExecutionLogRepository repository;
		IInfrastructureContext  context;
        public BIReportsExecutionLogQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new BIReportsExecutionLogRepository(context);
            Repository = repository;
            mapping = new BIReportsExecutionLogDataMapping();
        }

        public BIReportsExecutionLogQueryService(BIReportsExecutionLogRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BIReportsExecutionLogDataMapping();
        }

        public BIReportsExecutionLogQueryService(IInfrastructureContext context)
        {
            this.repository = new BIReportsExecutionLogRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BIReportsExecutionLogDataMapping();
        }
		 
		public  BIReportsExecutionLogPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BIReportsExecutionLogKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BIReportsExecutionLog entityPOCO)
        {
            BIReportsExecutionLogKeys entityKeys = new BIReportsExecutionLogKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 