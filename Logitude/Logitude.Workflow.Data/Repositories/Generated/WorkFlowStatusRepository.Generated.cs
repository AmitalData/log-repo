 
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
   public partial class WorkFlowStatusRepository:IRepository<WorkFlowStatus>
   {
   
        private IWorkflowContext currentContext;
        public WorkFlowStatusRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public WorkFlowStatusRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  WorkFlowStatus GetSingle(string code)
        {
            return (from a in context.WorkFlowStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<WorkFlowStatus> GetAll()
        {
            return from a in context.WorkFlowStatuses  
                   select a;
        }
				 
        public WorkFlowStatus GetSingle(EntityKeyFields entityKeys)
        {
            WorkFlowStatusKeys keys = entityKeys as WorkFlowStatusKeys;
            return (from a in context.WorkFlowStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WorkFlowStatus entity)
        {
            onAdd();
            context.WorkFlowStatuses.Add(entity);
        }

        public void Remove(WorkFlowStatus entity)
        {
            context.WorkFlowStatuses.Attach(entity);
            context.WorkFlowStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WorkFlowStatus entity)
        {
            onUpdate();
            context.WorkFlowStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WorkFlowStatus> All()
        {
            return context.WorkFlowStatuses.ToList();
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
	 