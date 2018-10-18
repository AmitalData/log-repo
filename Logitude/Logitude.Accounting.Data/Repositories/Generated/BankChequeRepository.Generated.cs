 
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
   public partial class BankChequeRepository:IRepository<BankCheque>
   {
   
        private IAccountingContext currentContext;
        public BankChequeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public BankChequeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BankCheque GetSingle(string id, int tenant)
        {
            return (from a in context.BankCheques
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BankCheque> GetAll(int tenant)
        {
            return from a in context.BankCheques  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BankCheque GetSingle(EntityKeyFields entityKeys)
        {
            BankChequeKeys keys = entityKeys as BankChequeKeys;
            return (from a in context.BankCheques
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 
        public void Add(BankCheque entity)
        {
            context.BankCheques.Add(entity);
        }

        public void Remove(BankCheque entity)
        {
            context.BankCheques.Attach(entity);
            context.BankCheques.Remove(entity);
        }

        public void Update(BankCheque entity)
        {
            context.BankCheques.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BankCheque> All()
        {
            return context.BankCheques.ToList();
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
	 