 
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
   public partial class AccountingEntityRepository:IRepository<AccountingEntity>
   {
   
        private IAccountingContext currentContext;
        public AccountingEntityRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public AccountingEntityRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  AccountingEntity GetSingle(string code)
        {
            return (from a in context.AccountingEntities
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AccountingEntity> GetAll()
        {
            return from a in context.AccountingEntities  
                   select a;
        }
				 
        public AccountingEntity GetSingle(EntityKeyFields entityKeys)
        {
            AccountingEntityKeys keys = entityKeys as AccountingEntityKeys;
            return (from a in context.AccountingEntities
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AccountingEntity entity)
        {
            onAdd();
            context.AccountingEntities.Add(entity);
        }

        public void Remove(AccountingEntity entity)
        {
            context.AccountingEntities.Attach(entity);
            context.AccountingEntities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AccountingEntity entity)
        {
            onUpdate();
            context.AccountingEntities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AccountingEntity> All()
        {
            return context.AccountingEntities.ToList();
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
	 