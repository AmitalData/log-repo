 
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
   public partial class BatchTaskExecutionStatusQueryService: EntityQueryService<BatchTaskExecutionStatus,BatchTaskExecutionStatusKeys,BatchTaskExecutionStatusPM,object,BatchTaskExecutionStatusKeys>
   {
   
        BatchTaskExecutionStatusRepository repository;
		IInfrastructureContext  context;
        public BatchTaskExecutionStatusQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new BatchTaskExecutionStatusRepository(context);
            Repository = repository;
            mapping = new BatchTaskExecutionStatusDataMapping();
        }

        public BatchTaskExecutionStatusQueryService(BatchTaskExecutionStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new BatchTaskExecutionStatusDataMapping();
        }

        public BatchTaskExecutionStatusQueryService(IInfrastructureContext context)
        {
            this.repository = new BatchTaskExecutionStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new BatchTaskExecutionStatusDataMapping();
        }
		 
		public  BatchTaskExecutionStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new BatchTaskExecutionStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(BatchTaskExecutionStatus entityPOCO)
        {
            BatchTaskExecutionStatusKeys entityKeys = new BatchTaskExecutionStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 