 
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
   public partial class WorkFlowInstanceRepository:IRepository<WorkFlowInstance>
   {
   
        private IWorkflowContext currentContext;
        public WorkFlowInstanceRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public WorkFlowInstanceRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  WorkFlowInstance GetSingle(string id, int tenant)
        {
            return (from a in context.WorkFlowInstances
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<WorkFlowInstance> GetAll(int tenant)
        {
            return from a in context.WorkFlowInstances  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public WorkFlowInstance GetSingle(EntityKeyFields entityKeys)
        {
            WorkFlowInstanceKeys keys = entityKeys as WorkFlowInstanceKeys;
            return (from a in context.WorkFlowInstances
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WorkFlowInstance entity)
        {
            onAdd();
            context.WorkFlowInstances.Add(entity);
        }

        public void Remove(WorkFlowInstance entity)
        {
            context.WorkFlowInstances.Attach(entity);
            context.WorkFlowInstances.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WorkFlowInstance entity)
        {
            onUpdate();
            context.WorkFlowInstances.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WorkFlowInstance> All()
        {
            return context.WorkFlowInstances.ToList();
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
	 