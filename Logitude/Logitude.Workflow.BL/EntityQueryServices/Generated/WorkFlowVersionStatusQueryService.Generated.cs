 
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
   public partial class WorkFlowVersionStatusQueryService: EntityQueryService<WorkFlowVersionStatus,WorkFlowVersionStatusKeys,WorkFlowVersionStatusPM,object,WorkFlowVersionStatusKeys>
   {
   
        WorkFlowVersionStatusRepository repository;
		IWorkflowContext  context;
        public WorkFlowVersionStatusQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new WorkFlowVersionStatusRepository(context);
            Repository = repository;
            mapping = new WorkFlowVersionStatusDataMapping();
        }

        public WorkFlowVersionStatusQueryService(WorkFlowVersionStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WorkFlowVersionStatusDataMapping();
        }

        public WorkFlowVersionStatusQueryService(IWorkflowContext context)
        {
            this.repository = new WorkFlowVersionStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WorkFlowVersionStatusDataMapping();
        }
		 
		public  WorkFlowVersionStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WorkFlowVersionStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WorkFlowVersionStatus entityPOCO)
        {
            WorkFlowVersionStatusKeys entityKeys = new WorkFlowVersionStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 