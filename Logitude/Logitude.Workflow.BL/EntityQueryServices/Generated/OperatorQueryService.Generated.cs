 
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
   public partial class OperatorQueryService: EntityQueryService<Operator,OperatorKeys,OperatorPM,object,OperatorKeys>
   {
   
        OperatorRepository repository;
		IWorkflowContext  context;
        public OperatorQueryService(int tenant)
        {
		    context = WorkflowContext.GetContext(tenant);
            MainContext = context;
            repository = new OperatorRepository(context);
            Repository = repository;
            mapping = new OperatorDataMapping();
        }

        public OperatorQueryService(OperatorRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OperatorDataMapping();
        }

        public OperatorQueryService(IWorkflowContext context)
        {
            this.repository = new OperatorRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OperatorDataMapping();
        }
		 
		public  OperatorPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OperatorKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Operator entityPOCO)
        {
            OperatorKeys entityKeys = new OperatorKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 