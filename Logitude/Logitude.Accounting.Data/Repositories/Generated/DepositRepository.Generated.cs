 
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
   public partial class DepositRepository:IRepository<Deposit>
   {
   
        private IAccountingContext currentContext;
        public DepositRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public DepositRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Deposit GetSingle(string id, int tenant)
        {
            return (from a in context.Deposits
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Deposit> GetAll(int tenant)
        {
            return from a in context.Deposits  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Deposit GetSingle(EntityKeyFields entityKeys)
        {
            DepositKeys keys = entityKeys as DepositKeys;
            return (from a in context.Deposits
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 
        public void Add(Deposit entity)
        {
            context.Deposits.Add(entity);
        }

        public void Remove(Deposit entity)
        {
            context.Deposits.Attach(entity);
            context.Deposits.Remove(entity);
        }

        public void Update(Deposit entity)
        {
            context.Deposits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Deposit> All()
        {
            return context.Deposits.ToList();
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
	 