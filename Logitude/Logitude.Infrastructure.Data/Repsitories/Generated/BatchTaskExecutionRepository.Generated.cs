 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class BatchTaskExecutionRepository:IRepository<BatchTaskExecution>
   {
   
        private IInfrastructureContext currentContext;
        public BatchTaskExecutionRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public BatchTaskExecutionRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  BatchTaskExecution GetSingle(string id, int tenant)
        {
            return (from a in context.BatchTaskExecutions
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BatchTaskExecution> GetAll(int tenant)
        {
            return from a in context.BatchTaskExecutions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BatchTaskExecution GetSingle(EntityKeyFields entityKeys)
        {
            BatchTaskExecutionKeys keys = entityKeys as BatchTaskExecutionKeys;
            return (from a in context.BatchTaskExecutions
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BatchTaskExecution entity)
        {
            onAdd();
            context.BatchTaskExecutions.Add(entity);
        }

        public void Remove(BatchTaskExecution entity)
        {
            context.BatchTaskExecutions.Attach(entity);
            context.BatchTaskExecutions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BatchTaskExecution entity)
        {
            onUpdate();
            context.BatchTaskExecutions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BatchTaskExecution> All()
        {
            return context.BatchTaskExecutions.ToList();
        }

        private IInfrastructureContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 