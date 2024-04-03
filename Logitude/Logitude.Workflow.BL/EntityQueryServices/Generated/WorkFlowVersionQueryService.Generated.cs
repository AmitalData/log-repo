 
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
   public partial class WorkFlowVersionQueryService: EntityQueryService<WorkFlowVersion,WorkFlowVersionKeys,WorkFlowVersionPM,object,WorkFlowVersionKeys>
   {
   
        WorkFlowVersionRepository repository;
		IWorkflowContext  context;
        public WorkFlowVersionQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new WorkFlowVersionRepository(context);
            Repository = repository;
            mapping = new WorkFlowVersionDataMapping();
        }

        public WorkFlowVersionQueryService(WorkFlowVersionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WorkFlowVersionDataMapping();
        }

        public WorkFlowVersionQueryService(IWorkflowContext context)
        {
            this.repository = new WorkFlowVersionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WorkFlowVersionDataMapping();
        }
		 
		public  WorkFlowVersionPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WorkFlowVersionKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WorkFlowVersion entityPOCO)
        {
            WorkFlowVersionKeys entityKeys = new WorkFlowVersionKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 