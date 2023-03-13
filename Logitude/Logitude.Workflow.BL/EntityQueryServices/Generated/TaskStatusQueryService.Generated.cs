 
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
   public partial class TaskStatusQueryService: EntityQueryService<TaskStatus,TaskStatusKeys,TaskStatusPM,object,TaskStatusKeys>
   {
   
        TaskStatusRepository repository;
		IWorkflowContext  context;
        public TaskStatusQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new TaskStatusRepository(context);
            Repository = repository;
            mapping = new TaskStatusDataMapping();
        }

        public TaskStatusQueryService(TaskStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TaskStatusDataMapping();
        }

        public TaskStatusQueryService(IWorkflowContext context)
        {
            this.repository = new TaskStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TaskStatusDataMapping();
        }
		 
		public  TaskStatusPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TaskStatusKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TaskStatus entityPOCO)
        {
            TaskStatusKeys entityKeys = new TaskStatusKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 