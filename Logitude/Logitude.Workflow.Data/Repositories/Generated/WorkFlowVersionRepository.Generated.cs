 
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
   public partial class WorkFlowVersionRepository:IRepository<WorkFlowVersion>
   {
   
        private IWorkflowContext currentContext;
        public WorkFlowVersionRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public WorkFlowVersionRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  WorkFlowVersion GetSingle(string id, int tenant)
        {
            return (from a in context.WorkFlowVersions
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<WorkFlowVersion> GetAll(int tenant)
        {
            return from a in context.WorkFlowVersions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public WorkFlowVersion GetSingle(EntityKeyFields entityKeys)
        {
            WorkFlowVersionKeys keys = entityKeys as WorkFlowVersionKeys;
            return (from a in context.WorkFlowVersions
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(WorkFlowVersion entity)
        {
            onAdd();
            context.WorkFlowVersions.Add(entity);
        }

        public void Remove(WorkFlowVersion entity)
        {
            context.WorkFlowVersions.Attach(entity);
            context.WorkFlowVersions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(WorkFlowVersion entity)
        {
            onUpdate();
            context.WorkFlowVersions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WorkFlowVersion> All()
        {
            return context.WorkFlowVersions.ToList();
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
	 