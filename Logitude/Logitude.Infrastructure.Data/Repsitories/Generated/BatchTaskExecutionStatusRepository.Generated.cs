 
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
   public partial class BatchTaskExecutionStatusRepository:IRepository<BatchTaskExecutionStatus>
   {
   
        private IInfrastructureContext currentContext;
        public BatchTaskExecutionStatusRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public BatchTaskExecutionStatusRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  BatchTaskExecutionStatus GetSingle(string code)
        {
            return (from a in context.BatchTaskExecutionStatus
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<BatchTaskExecutionStatus> GetAll()
        {
            return from a in context.BatchTaskExecutionStatus  
                   select a;
        }
				 
        public BatchTaskExecutionStatus GetSingle(EntityKeyFields entityKeys)
        {
            BatchTaskExecutionStatusKeys keys = entityKeys as BatchTaskExecutionStatusKeys;
            return (from a in context.BatchTaskExecutionStatus
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BatchTaskExecutionStatus entity)
        {
            onAdd();
            context.BatchTaskExecutionStatus.Add(entity);
        }

        public void Remove(BatchTaskExecutionStatus entity)
        {
            context.BatchTaskExecutionStatus.Attach(entity);
            context.BatchTaskExecutionStatus.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BatchTaskExecutionStatus entity)
        {
            onUpdate();
            context.BatchTaskExecutionStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BatchTaskExecutionStatus> All()
        {
            return context.BatchTaskExecutionStatus.ToList();
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
	 