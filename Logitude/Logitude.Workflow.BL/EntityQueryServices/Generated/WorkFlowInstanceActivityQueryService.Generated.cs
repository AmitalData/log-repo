 
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
   public partial class WorkFlowInstanceActivityQueryService: EntityQueryService<WorkFlowInstanceActivity,WorkFlowInstanceActivityKeys,WorkFlowInstanceActivityPM,object,WorkFlowInstanceActivityKeys>
   {
   
        WorkFlowInstanceActivityRepository repository;
		IWorkflowContext  context;
        public WorkFlowInstanceActivityQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new WorkFlowInstanceActivityRepository(context);
            Repository = repository;
            mapping = new WorkFlowInstanceActivityDataMapping();
        }

        public WorkFlowInstanceActivityQueryService(WorkFlowInstanceActivityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WorkFlowInstanceActivityDataMapping();
        }

        public WorkFlowInstanceActivityQueryService(IWorkflowContext context)
        {
            this.repository = new WorkFlowInstanceActivityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WorkFlowInstanceActivityDataMapping();
        }
		 
		public  WorkFlowInstanceActivityPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WorkFlowInstanceActivityKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WorkFlowInstanceActivity entityPOCO)
        {
            WorkFlowInstanceActivityKeys entityKeys = new WorkFlowInstanceActivityKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 