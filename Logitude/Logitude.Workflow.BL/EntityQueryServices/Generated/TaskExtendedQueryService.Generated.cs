 
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
   public partial class TaskExtendedQueryService: EntityQueryService<TaskExtended,TaskExtendedKeys,TaskExtendedPM,object,TaskExtendedKeys>
   {
   
        TaskExtendedRepository repository;
		IWorkflowContext  context;
        public TaskExtendedQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new TaskExtendedRepository(context);
            Repository = repository;
            mapping = new TaskExtendedDataMapping();
        }

        public TaskExtendedQueryService(TaskExtendedRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TaskExtendedDataMapping();
        }

        public TaskExtendedQueryService(IWorkflowContext context)
        {
            this.repository = new TaskExtendedRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TaskExtendedDataMapping();
        }
		 
		public  TaskExtendedPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TaskExtendedKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TaskExtended entityPOCO)
        {
            TaskExtendedKeys entityKeys = new TaskExtendedKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 