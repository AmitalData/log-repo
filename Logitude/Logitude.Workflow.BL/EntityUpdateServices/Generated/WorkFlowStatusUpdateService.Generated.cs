 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.EntityPMs;
using Logitude.Workflow.BL.EntityDataMappings;
using Logitude.Workflow.Data.Repositories;
using Logitude.Workflow.Data.EntityKeys;
using Logitude.Workflow.Data;

namespace Logitude.Workflow.BL.EntityUpdateServices
{ 
   public partial class WorkFlowStatusUpdateService:EntityUpdateService<WorkFlowStatus,WorkFlowStatusPM,EntityPM>
   {
   
        WorkFlowStatusRepository entityRepository;
        public WorkFlowStatusUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IWorkflowContext  context = mainContext as WorkflowContext;
            context = context ??mainContext as IWorkflowContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new WorkFlowStatusDataMapping();
            Repository = new WorkFlowStatusRepository(context);
        }

       
        private IWorkflowContext currentContext;
        public WorkFlowStatusUpdateService(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public WorkFlowStatusUpdateService(IWorkflowContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(WorkFlowStatusPM entityPM)
        {
            WorkFlowStatusKeys entityKeys = new WorkFlowStatusKeys() { Code = entityPM.Code };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(WorkFlowStatusPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(WorkFlowStatusPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 