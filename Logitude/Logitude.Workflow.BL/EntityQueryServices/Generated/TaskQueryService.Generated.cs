 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
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
   public partial class TaskQueryService: EntityQueryService<Task,TaskKeys,TaskPM,object,TaskKeys>
   {
   
        TaskRepository repository;
		IWorkflowContext  context;
        public TaskQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new TaskRepository(context);
            Repository = repository;
            mapping = new TaskDataMapping();
        }

        public TaskQueryService(TaskRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TaskDataMapping();
        }

        public TaskQueryService(IWorkflowContext context)
        {
            this.repository = new TaskRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TaskDataMapping();
        }
		 
		public  TaskPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TaskKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Task entityPOCO)
        {
            TaskKeys entityKeys = new TaskKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 