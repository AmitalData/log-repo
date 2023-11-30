 
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
   public partial class WorkFlowInstanceQueryService: EntityQueryService<WorkFlowInstance,WorkFlowInstanceKeys,WorkFlowInstancePM,object,WorkFlowInstanceKeys>
   {
   
        WorkFlowInstanceRepository repository;
		IWorkflowContext  context;
        public WorkFlowInstanceQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new WorkFlowInstanceRepository(context);
            Repository = repository;
            mapping = new WorkFlowInstanceDataMapping();
        }

        public WorkFlowInstanceQueryService(WorkFlowInstanceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WorkFlowInstanceDataMapping();
        }

        public WorkFlowInstanceQueryService(IWorkflowContext context)
        {
            this.repository = new WorkFlowInstanceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WorkFlowInstanceDataMapping();
        }
		 
		public  WorkFlowInstancePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WorkFlowInstanceKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WorkFlowInstance entityPOCO)
        {
            WorkFlowInstanceKeys entityKeys = new WorkFlowInstanceKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 