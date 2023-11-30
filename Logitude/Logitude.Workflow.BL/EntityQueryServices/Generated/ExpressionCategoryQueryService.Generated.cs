 
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
   public partial class ExpressionCategoryQueryService: EntityQueryService<ExpressionCategory,ExpressionCategoryKeys,ExpressionCategoryPM,object,ExpressionCategoryKeys>
   {
   
        ExpressionCategoryRepository repository;
		IWorkflowContext  context;
        public ExpressionCategoryQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new ExpressionCategoryRepository(context);
            Repository = repository;
            mapping = new ExpressionCategoryDataMapping();
        }

        public ExpressionCategoryQueryService(ExpressionCategoryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExpressionCategoryDataMapping();
        }

        public ExpressionCategoryQueryService(IWorkflowContext context)
        {
            this.repository = new ExpressionCategoryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExpressionCategoryDataMapping();
        }
		 
		public  ExpressionCategoryPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExpressionCategoryKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExpressionCategory entityPOCO)
        {
            ExpressionCategoryKeys entityKeys = new ExpressionCategoryKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 