 
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
   public partial class WorkFlowStatusQueryService: EntityQueryService<WorkFlowStatus,WorkFlowStatusKeys,WorkFlowStatusPM,object,WorkFlowStatusKeys>
   {
   
        WorkFlowStatusRepository repository;
		IWorkflowContext  context;
        public WorkFlowStatusQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new WorkFlowStatusRepository(context);
            Repository = repository;
            mapping = new WorkFlowStatusDataMapping();
        }

        public WorkFlowStatusQueryService(WorkFlowStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WorkFlowStatusDataMapping();
        }

        public WorkFlowStatusQueryService(IWorkflowContext context)
        {
            this.repository = new WorkFlowStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WorkFlowStatusDataMapping();
        }
		 
		public  WorkFlowStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WorkFlowStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WorkFlowStatus entityPOCO)
        {
            WorkFlowStatusKeys entityKeys = new WorkFlowStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 