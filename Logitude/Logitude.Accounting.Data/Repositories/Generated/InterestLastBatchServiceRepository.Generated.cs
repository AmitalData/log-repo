 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class InterestLastBatchServiceRepository:IRepository<InterestLastBatchService>
   {
   
        private IAccountingContext currentContext;
        public InterestLastBatchServiceRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public InterestLastBatchServiceRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterestLastBatchService GetSingle(string id, int tenant)
        {
            return (from a in context.InterestLastBatchServices
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<InterestLastBatchService> GetAll(int tenant)
        {
            return from a in context.InterestLastBatchServices  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public InterestLastBatchService GetSingle(EntityKeyFields entityKeys)
        {
            InterestLastBatchServiceKeys keys = entityKeys as InterestLastBatchServiceKeys;
            return (from a in context.InterestLastBatchServices
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterestLastBatchService entity)
        {
            onAdd();
            context.InterestLastBatchServices.Add(entity);
        }

        public void Remove(InterestLastBatchService entity)
        {
            context.InterestLastBatchServices.Attach(entity);
            context.InterestLastBatchServices.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterestLastBatchService entity)
        {
            onUpdate();
            context.InterestLastBatchServices.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterestLastBatchService> All()
        {
            return context.InterestLastBatchServices.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 