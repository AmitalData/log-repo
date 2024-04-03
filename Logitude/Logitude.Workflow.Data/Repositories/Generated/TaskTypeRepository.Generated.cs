 
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
   public partial class TaskTypeRepository:IRepository<TaskType>
   {
   
        private IWorkflowContext currentContext;
        public TaskTypeRepository(int tenant)
        {
            currentContext = WorkflowContext.GetContext(tenant);
        }

        public TaskTypeRepository(IWorkflowContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaskType GetSingle(string id, int tenant)
        {
            return (from a in context.TaskTypes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TaskType> GetAll(int tenant)
        {
            return from a in context.TaskTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TaskType GetSingle(EntityKeyFields entityKeys)
        {
            TaskTypeKeys keys = entityKeys as TaskTypeKeys;
            return (from a in context.TaskTypes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaskType entity)
        {
            onAdd();
            context.TaskTypes.Add(entity);
        }

        public void Remove(TaskType entity)
        {
            context.TaskTypes.Attach(entity);
            context.TaskTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaskType entity)
        {
            onUpdate();
            context.TaskTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaskType> All()
        {
            return context.TaskTypes.ToList();
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
	 