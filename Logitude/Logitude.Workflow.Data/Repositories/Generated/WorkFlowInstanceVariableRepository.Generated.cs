 
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
   public partial class WorkFlowInstanceVariableRepository:IRepository<WorkFlowInstanceVariable>
   {
   
        private IWorkflowContext currentContext;
        public WorkFlowInstanceVariableRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public WorkFlowInstanceVariableRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  WorkFlowInstanceVariable GetSingle(string id, int tenant)
        {
            return (from a in context.WorkFlowInstanceVariables
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<WorkFlowInstanceVariable> GetAll(int tenant)
        {
            return from a in context.WorkFlowInstanceVariables  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public WorkFlowInstanceVariable GetSingle(EntityKeyFields entityKeys)
        {
            WorkFlowInstanceVariableKeys keys = entityKeys as WorkFlowInstanceVariableKeys;
            return (from a in context.WorkFlowInstanceVariables
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WorkFlowInstanceVariable entity)
        {
            onAdd();
            context.WorkFlowInstanceVariables.Add(entity);
        }

        public void Remove(WorkFlowInstanceVariable entity)
        {
            context.WorkFlowInstanceVariables.Attach(entity);
            context.WorkFlowInstanceVariables.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WorkFlowInstanceVariable entity)
        {
            onUpdate();
            context.WorkFlowInstanceVariables.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WorkFlowInstanceVariable> All()
        {
            return context.WorkFlowInstanceVariables.ToList();
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
	 