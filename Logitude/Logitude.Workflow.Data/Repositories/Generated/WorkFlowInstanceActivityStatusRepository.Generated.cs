 
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
   public partial class WorkFlowInstanceActivityStatusRepository:IRepository<WorkFlowInstanceActivityStatus>
   {
   
        private IWorkflowContext currentContext;
        public WorkFlowInstanceActivityStatusRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public WorkFlowInstanceActivityStatusRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  WorkFlowInstanceActivityStatus GetSingle(string code)
        {
            return (from a in context.WorkFlowInstanceActivityStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<WorkFlowInstanceActivityStatus> GetAll()
        {
            return from a in context.WorkFlowInstanceActivityStatuses  
                   select a;
        }
				 
        public WorkFlowInstanceActivityStatus GetSingle(EntityKeyFields entityKeys)
        {
            WorkFlowInstanceActivityStatusKeys keys = entityKeys as WorkFlowInstanceActivityStatusKeys;
            return (from a in context.WorkFlowInstanceActivityStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WorkFlowInstanceActivityStatus entity)
        {
            onAdd();
            context.WorkFlowInstanceActivityStatuses.Add(entity);
        }

        public void Remove(WorkFlowInstanceActivityStatus entity)
        {
            context.WorkFlowInstanceActivityStatuses.Attach(entity);
            context.WorkFlowInstanceActivityStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WorkFlowInstanceActivityStatus entity)
        {
            onUpdate();
            context.WorkFlowInstanceActivityStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WorkFlowInstanceActivityStatus> All()
        {
            return context.WorkFlowInstanceActivityStatuses.ToList();
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
	 