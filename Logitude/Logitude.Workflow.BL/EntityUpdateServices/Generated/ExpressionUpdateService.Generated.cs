 
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
   public partial class ExpressionUpdateService:EntityUpdateService<Expression,ExpressionPM,EntityPM>
   {
   
        ExpressionRepository entityRepository;
        public ExpressionUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IWorkflowContext  context = mainContext as WorkflowContext;
            context = context ??mainContext as IWorkflowContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new ExpressionDataMapping();
            Repository = new ExpressionRepository(context);
        }

       
        private IWorkflowContext currentContext;
        public ExpressionUpdateService(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public ExpressionUpdateService(IWorkflowContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(ExpressionPM entityPM)
        {
            ExpressionKeys entityKeys = new ExpressionKeys() { Code = entityPM.Code };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(ExpressionPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(ExpressionPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 