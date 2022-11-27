 
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
   public partial class WorkFlowVersionStatusRepository:IRepository<WorkFlowVersionStatus>
   {
   
        private IWorkflowContext currentContext;
        public WorkFlowVersionStatusRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public WorkFlowVersionStatusRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  WorkFlowVersionStatus GetSingle(string code)
        {
            return (from a in context.WorkFlowVersionStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<WorkFlowVersionStatus> GetAll()
        {
            return from a in context.WorkFlowVersionStatuses  
                   select a;
        }
				 
        public WorkFlowVersionStatus GetSingle(EntityKeyFields entityKeys)
        {
            WorkFlowVersionStatusKeys keys = entityKeys as WorkFlowVersionStatusKeys;
            return (from a in context.WorkFlowVersionStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WorkFlowVersionStatus entity)
        {
            onAdd();
            context.WorkFlowVersionStatuses.Add(entity);
        }

        public void Remove(WorkFlowVersionStatus entity)
        {
            context.WorkFlowVersionStatuses.Attach(entity);
            context.WorkFlowVersionStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WorkFlowVersionStatus entity)
        {
            onUpdate();
            context.WorkFlowVersionStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WorkFlowVersionStatus> All()
        {
            return context.WorkFlowVersionStatuses.ToList();
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
	 