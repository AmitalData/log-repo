 
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
   public partial class ChartOfAccountRepository:IRepository<ChartOfAccount>
   {
   
        private IAccountingContext currentContext;
        public ChartOfAccountRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ChartOfAccountRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ChartOfAccount GetSingle(string id, int tenant)
        {
            return (from a in context.ChartOfAccounts
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ChartOfAccount> GetAll(int tenant)
        {
            return from a in context.ChartOfAccounts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ChartOfAccount GetSingle(EntityKeyFields entityKeys)
        {
            ChartOfAccountKeys keys = entityKeys as ChartOfAccountKeys;
            return (from a in context.ChartOfAccounts
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ChartOfAccount entity)
        {
            onAdd();
            context.ChartOfAccounts.Add(entity);
        }

        public void Remove(ChartOfAccount entity)
        {
            context.ChartOfAccounts.Attach(entity);
            context.ChartOfAccounts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ChartOfAccount entity)
        {
            onUpdate();
            context.ChartOfAccounts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ChartOfAccount> All()
        {
            return context.ChartOfAccounts.ToList();
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
	 