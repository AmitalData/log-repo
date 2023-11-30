 
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
   public partial class OperatorCategoryQueryService: EntityQueryService<OperatorCategory,OperatorCategoryKeys,OperatorCategoryPM,object,OperatorCategoryKeys>
   {
   
        OperatorCategoryRepository repository;
		IWorkflowContext  context;
        public OperatorCategoryQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new OperatorCategoryRepository(context);
            Repository = repository;
            mapping = new OperatorCategoryDataMapping();
        }

        public OperatorCategoryQueryService(OperatorCategoryRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OperatorCategoryDataMapping();
        }

        public OperatorCategoryQueryService(IWorkflowContext context)
        {
            this.repository = new OperatorCategoryRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OperatorCategoryDataMapping();
        }
		 
		public  OperatorCategoryPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OperatorCategoryKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OperatorCategory entityPOCO)
        {
            OperatorCategoryKeys entityKeys = new OperatorCategoryKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 