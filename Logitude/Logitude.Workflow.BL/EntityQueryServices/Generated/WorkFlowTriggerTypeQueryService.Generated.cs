 
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
   public partial class WorkFlowTriggerTypeQueryService: EntityQueryService<WorkFlowTriggerType,WorkFlowTriggerTypeKeys,WorkFlowTriggerTypePM,object,WorkFlowTriggerTypeKeys>
   {
   
        WorkFlowTriggerTypeRepository repository;
		IWorkflowContext  context;
        public WorkFlowTriggerTypeQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new WorkFlowTriggerTypeRepository(context);
            Repository = repository;
            mapping = new WorkFlowTriggerTypeDataMapping();
        }

        public WorkFlowTriggerTypeQueryService(WorkFlowTriggerTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WorkFlowTriggerTypeDataMapping();
        }

        public WorkFlowTriggerTypeQueryService(IWorkflowContext context)
        {
            this.repository = new WorkFlowTriggerTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WorkFlowTriggerTypeDataMapping();
        }
		 
		public  WorkFlowTriggerTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WorkFlowTriggerTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WorkFlowTriggerType entityPOCO)
        {
            WorkFlowTriggerTypeKeys entityKeys = new WorkFlowTriggerTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 