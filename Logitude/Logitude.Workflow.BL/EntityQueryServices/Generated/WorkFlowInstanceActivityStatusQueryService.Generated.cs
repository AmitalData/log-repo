 
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
   public partial class WorkFlowInstanceActivityStatusQueryService: EntityQueryService<WorkFlowInstanceActivityStatus,WorkFlowInstanceActivityStatusKeys,WorkFlowInstanceActivityStatusPM,object,WorkFlowInstanceActivityStatusKeys>
   {
   
        WorkFlowInstanceActivityStatusRepository repository;
		IWorkflowContext  context;
        public WorkFlowInstanceActivityStatusQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new WorkFlowInstanceActivityStatusRepository(context);
            Repository = repository;
            mapping = new WorkFlowInstanceActivityStatusDataMapping();
        }

        public WorkFlowInstanceActivityStatusQueryService(WorkFlowInstanceActivityStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WorkFlowInstanceActivityStatusDataMapping();
        }

        public WorkFlowInstanceActivityStatusQueryService(IWorkflowContext context)
        {
            this.repository = new WorkFlowInstanceActivityStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WorkFlowInstanceActivityStatusDataMapping();
        }
		 
		public  WorkFlowInstanceActivityStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WorkFlowInstanceActivityStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WorkFlowInstanceActivityStatus entityPOCO)
        {
            WorkFlowInstanceActivityStatusKeys entityKeys = new WorkFlowInstanceActivityStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 