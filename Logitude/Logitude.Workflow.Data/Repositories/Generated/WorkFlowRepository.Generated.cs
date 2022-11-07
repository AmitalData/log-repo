 
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
   public partial class WorkFlowRepository:IRepository<WorkFlow>
   {
   
        private IWorkflowContext currentContext;
        public WorkFlowRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public WorkFlowRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  WorkFlow GetSingle(string id, int tenant)
        {
            return (from a in context.WorkFlows
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<WorkFlow> GetAll(int tenant)
        {
            return from a in context.WorkFlows  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public WorkFlow GetSingle(EntityKeyFields entityKeys)
        {
            WorkFlowKeys keys = entityKeys as WorkFlowKeys;
            return (from a in context.WorkFlows
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WorkFlow entity)
        {
            onAdd();
            context.WorkFlows.Add(entity);
        }

        public void Remove(WorkFlow entity)
        {
            context.WorkFlows.Attach(entity);
            context.WorkFlows.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WorkFlow entity)
        {
            onUpdate();
            context.WorkFlows.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WorkFlow> All()
        {
            return context.WorkFlows.ToList();
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
	 