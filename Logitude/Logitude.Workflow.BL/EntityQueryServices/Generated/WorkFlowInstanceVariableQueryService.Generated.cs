 
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
   public partial class WorkFlowInstanceVariableQueryService: EntityQueryService<WorkFlowInstanceVariable,WorkFlowInstanceVariableKeys,WorkFlowInstanceVariablePM,object,WorkFlowInstanceVariableKeys>
   {
   
        WorkFlowInstanceVariableRepository repository;
		IWorkflowContext  context;
        public WorkFlowInstanceVariableQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new WorkFlowInstanceVariableRepository(context);
            Repository = repository;
            mapping = new WorkFlowInstanceVariableDataMapping();
        }

        public WorkFlowInstanceVariableQueryService(WorkFlowInstanceVariableRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WorkFlowInstanceVariableDataMapping();
        }

        public WorkFlowInstanceVariableQueryService(IWorkflowContext context)
        {
            this.repository = new WorkFlowInstanceVariableRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WorkFlowInstanceVariableDataMapping();
        }
		 
		public  WorkFlowInstanceVariablePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WorkFlowInstanceVariableKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WorkFlowInstanceVariable entityPOCO)
        {
            WorkFlowInstanceVariableKeys entityKeys = new WorkFlowInstanceVariableKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 