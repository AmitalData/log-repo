 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.BL.EntityDataMappings;
using Logitude.Workflow.Data.Repositories;
using Logitude.Workflow.Data.EntityKeys;
using Logitude.Workflow.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Workflow.BL.EntityQueryServices
{ 
   public partial class TaskTypeQueryService: EntityQueryService<TaskType,TaskTypeKeys,TaskTypePM,object,TaskTypeKeys>
   {
   
        TaskTypeRepository repository;
		IWorkflowContext  context;
        public TaskTypeQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new TaskTypeRepository(context);
            Repository = repository;
            mapping = new TaskTypeDataMapping();
        }

        public TaskTypeQueryService(TaskTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TaskTypeDataMapping();
        }

        public TaskTypeQueryService(IWorkflowContext context)
        {
            this.repository = new TaskTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TaskTypeDataMapping();
        }
		 
		public  TaskTypePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TaskTypeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TaskType entityPOCO)
        {
            TaskTypeKeys entityKeys = new TaskTypeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 