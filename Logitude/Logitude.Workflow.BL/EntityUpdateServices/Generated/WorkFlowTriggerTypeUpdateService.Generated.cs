 
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
   public partial class WorkFlowTriggerTypeUpdateService:EntityUpdateService<WorkFlowTriggerType,WorkFlowTriggerTypePM,EntityPM>
   {
   
        WorkFlowTriggerTypeRepository entityRepository;
        public WorkFlowTriggerTypeUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IWorkflowContext  context = mainContext as WorkflowContext;
            context = context ??mainContext as IWorkflowContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new WorkFlowTriggerTypeDataMapping();
            Repository = new WorkFlowTriggerTypeRepository(context);
        }

       
        private IWorkflowContext currentContext;
        public WorkFlowTriggerTypeUpdateService(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public WorkFlowTriggerTypeUpdateService(IWorkflowContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(WorkFlowTriggerTypePM entityPM)
        {
            WorkFlowTriggerTypeKeys entityKeys = new WorkFlowTriggerTypeKeys() { Code = entityPM.Code };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(WorkFlowTriggerTypePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(WorkFlowTriggerTypePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 