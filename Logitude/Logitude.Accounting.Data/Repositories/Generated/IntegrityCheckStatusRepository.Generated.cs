 
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
   public partial class IntegrityCheckStatusRepository:IRepository<IntegrityCheckStatus>
   {
   
        private IAccountingContext currentContext;
        public IntegrityCheckStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public IntegrityCheckStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  IntegrityCheckStatus GetSingle(string code)
        {
            return (from a in context.IntegrityCheckStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<IntegrityCheckStatus> GetAll()
        {
            return from a in context.IntegrityCheckStatuses  
                   select a;
        }
				 
        public IntegrityCheckStatus GetSingle(EntityKeyFields entityKeys)
        {
            IntegrityCheckStatusKeys keys = entityKeys as IntegrityCheckStatusKeys;
            return (from a in context.IntegrityCheckStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(IntegrityCheckStatus entity)
        {
            onAdd();
            context.IntegrityCheckStatuses.Add(entity);
        }

        public void Remove(IntegrityCheckStatus entity)
        {
            context.IntegrityCheckStatuses.Attach(entity);
            context.IntegrityCheckStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(IntegrityCheckStatus entity)
        {
            onUpdate();
            context.IntegrityCheckStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<IntegrityCheckStatus> All()
        {
            return context.IntegrityCheckStatuses.ToList();
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
	 