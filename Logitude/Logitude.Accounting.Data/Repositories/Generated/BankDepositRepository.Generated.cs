 
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
   public partial class BankDepositRepository:IRepository<BankDeposit>
   {
   
        private IAccountingContext currentContext;
        public BankDepositRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public BankDepositRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BankDeposit GetSingle(string id, int tenant)
        {
            return (from a in context.BankDeposits
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BankDeposit> GetAll(int tenant)
        {
            return from a in context.BankDeposits  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BankDeposit GetSingle(EntityKeyFields entityKeys)
        {
            BankDepositKeys keys = entityKeys as BankDepositKeys;
            return (from a in context.BankDeposits
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BankDeposit entity)
        {
            onAdd();
            context.BankDeposits.Add(entity);
        }

        public void Remove(BankDeposit entity)
        {
            context.BankDeposits.Attach(entity);
            context.BankDeposits.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BankDeposit entity)
        {
            onUpdate();
            context.BankDeposits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BankDeposit> All()
        {
            return context.BankDeposits.ToList();
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
	 