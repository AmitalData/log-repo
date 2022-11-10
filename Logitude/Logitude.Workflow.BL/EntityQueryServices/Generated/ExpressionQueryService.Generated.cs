 
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
   public partial class ExpressionQueryService: EntityQueryService<Expression,ExpressionKeys,ExpressionPM,object,ExpressionKeys>
   {
   
        ExpressionRepository repository;
		IWorkflowContext  context;
        public ExpressionQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new ExpressionRepository(context);
            Repository = repository;
            mapping = new ExpressionDataMapping();
        }

        public ExpressionQueryService(ExpressionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExpressionDataMapping();
        }

        public ExpressionQueryService(IWorkflowContext context)
        {
            this.repository = new ExpressionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExpressionDataMapping();
        }
		 
		public  ExpressionPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExpressionKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Expression entityPOCO)
        {
            ExpressionKeys entityKeys = new ExpressionKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 