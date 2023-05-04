 
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
   public partial class AccountingCompanyTypeRepository:IRepository<AccountingCompanyType>
   {
   
        private IAccountingContext currentContext;
        public AccountingCompanyTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public AccountingCompanyTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  AccountingCompanyType GetSingle(string id, int tenant)
        {
            return (from a in context.AccountingCompanyTypes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AccountingCompanyType> GetAll(int tenant)
        {
            return from a in context.AccountingCompanyTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public AccountingCompanyType GetSingle(EntityKeyFields entityKeys)
        {
            AccountingCompanyTypeKeys keys = entityKeys as AccountingCompanyTypeKeys;
            return (from a in context.AccountingCompanyTypes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AccountingCompanyType entity)
        {
            onAdd();
            context.AccountingCompanyTypes.Add(entity);
        }

        public void Remove(AccountingCompanyType entity)
        {
            context.AccountingCompanyTypes.Attach(entity);
            context.AccountingCompanyTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AccountingCompanyType entity)
        {
            onUpdate();
            context.AccountingCompanyTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingCompanyType> All()
        {
            return context.AccountingCompanyTypes.ToList();
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
	 