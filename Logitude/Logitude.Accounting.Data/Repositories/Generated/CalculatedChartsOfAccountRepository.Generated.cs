 
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
   public partial class CalculatedChartsOfAccountRepository:IRepository<CalculatedChartsOfAccount>
   {
   
        private IAccountingContext currentContext;
        public CalculatedChartsOfAccountRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public CalculatedChartsOfAccountRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CalculatedChartsOfAccount GetSingle(string id, int tenant)
        {
            return (from a in context.CalculatedChartsOfAccounts
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CalculatedChartsOfAccount> GetAll(int tenant)
        {
            return from a in context.CalculatedChartsOfAccounts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CalculatedChartsOfAccount GetSingle(EntityKeyFields entityKeys)
        {
            CalculatedChartsOfAccountKeys keys = entityKeys as CalculatedChartsOfAccountKeys;
            return (from a in context.CalculatedChartsOfAccounts
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CalculatedChartsOfAccount entity)
        {
            onAdd();
            context.CalculatedChartsOfAccounts.Add(entity);
        }

        public void Remove(CalculatedChartsOfAccount entity)
        {
            context.CalculatedChartsOfAccounts.Attach(entity);
            context.CalculatedChartsOfAccounts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CalculatedChartsOfAccount entity)
        {
            onUpdate();
            context.CalculatedChartsOfAccounts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CalculatedChartsOfAccount> All()
        {
            return context.CalculatedChartsOfAccounts.ToList();
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
	 