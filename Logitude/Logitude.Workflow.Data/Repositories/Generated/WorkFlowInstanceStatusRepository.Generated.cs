 
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
   public partial class WorkFlowInstanceStatusRepository:IRepository<WorkFlowInstanceStatus>
   {
   
        private IWorkflowContext currentContext;
        public WorkFlowInstanceStatusRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public WorkFlowInstanceStatusRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  WorkFlowInstanceStatus GetSingle(string code)
        {
            return (from a in context.WorkFlowInstanceStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<WorkFlowInstanceStatus> GetAll()
        {
            return from a in context.WorkFlowInstanceStatuses  
                   select a;
        }
				 
        public WorkFlowInstanceStatus GetSingle(EntityKeyFields entityKeys)
        {
            WorkFlowInstanceStatusKeys keys = entityKeys as WorkFlowInstanceStatusKeys;
            return (from a in context.WorkFlowInstanceStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WorkFlowInstanceStatus entity)
        {
            onAdd();
            context.WorkFlowInstanceStatuses.Add(entity);
        }

        public void Remove(WorkFlowInstanceStatus entity)
        {
            context.WorkFlowInstanceStatuses.Attach(entity);
            context.WorkFlowInstanceStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WorkFlowInstanceStatus entity)
        {
            onUpdate();
            context.WorkFlowInstanceStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WorkFlowInstanceStatus> All()
        {
            return context.WorkFlowInstanceStatuses.ToList();
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
	 