 
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
   public partial class WorkFlowInstanceActivityRepository:IRepository<WorkFlowInstanceActivity>
   {
   
        private IWorkflowContext currentContext;
        public WorkFlowInstanceActivityRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public WorkFlowInstanceActivityRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  WorkFlowInstanceActivity GetSingle(string id, int tenant)
        {
            return (from a in context.WorkFlowInstanceActivities
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<WorkFlowInstanceActivity> GetAll(int tenant)
        {
            return from a in context.WorkFlowInstanceActivities  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public WorkFlowInstanceActivity GetSingle(EntityKeyFields entityKeys)
        {
            WorkFlowInstanceActivityKeys keys = entityKeys as WorkFlowInstanceActivityKeys;
            return (from a in context.WorkFlowInstanceActivities
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WorkFlowInstanceActivity entity)
        {
            onAdd();
            context.WorkFlowInstanceActivities.Add(entity);
        }

        public void Remove(WorkFlowInstanceActivity entity)
        {
            context.WorkFlowInstanceActivities.Attach(entity);
            context.WorkFlowInstanceActivities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WorkFlowInstanceActivity entity)
        {
            onUpdate();
            context.WorkFlowInstanceActivities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WorkFlowInstanceActivity> All()
        {
            return context.WorkFlowInstanceActivities.ToList();
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
	 