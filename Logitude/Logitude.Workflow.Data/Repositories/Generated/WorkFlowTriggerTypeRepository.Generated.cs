 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Workflow.Data.Repositories
{
   public partial class WorkFlowTriggerTypeRepository:IRepository<WorkFlowTriggerType>
   {
   
        private IWorkflowContext currentContext;
        public WorkFlowTriggerTypeRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public WorkFlowTriggerTypeRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  WorkFlowTriggerType GetSingle(string code)
        {
            return (from a in context.WorkFlowTriggerTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<WorkFlowTriggerType> GetAll()
        {
            return from a in context.WorkFlowTriggerTypes  
                   select a;
        }
				 
        public WorkFlowTriggerType GetSingle(EntityKeyFields entityKeys)
        {
            WorkFlowTriggerTypeKeys keys = entityKeys as WorkFlowTriggerTypeKeys;
            return (from a in context.WorkFlowTriggerTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WorkFlowTriggerType entity)
        {
            onAdd();
            context.WorkFlowTriggerTypes.Add(entity);
        }

        public void Remove(WorkFlowTriggerType entity)
        {
            context.WorkFlowTriggerTypes.Attach(entity);
            context.WorkFlowTriggerTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WorkFlowTriggerType entity)
        {
            onUpdate();
            context.WorkFlowTriggerTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WorkFlowTriggerType> All()
        {
            return context.WorkFlowTriggerTypes.ToList();
        }

        private IWorkflowContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 