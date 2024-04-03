 
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
   public partial class GLAccountCurrencyRepository:IRepository<GLAccountCurrency>
   {
   
        private IAccountingContext currentContext;
        public GLAccountCurrencyRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountCurrencyRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccountCurrency GetSingle(string id, int tenant)
        {
            return (from a in context.GLAccountCurrencies
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccountCurrency> GetAll(int tenant)
        {
            return from a in context.GLAccountCurrencies  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GLAccountCurrency GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountCurrencyKeys keys = entityKeys as GLAccountCurrencyKeys;
            return (from a in context.GLAccountCurrencies
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccountCurrency entity)
        {
            onAdd();
            context.GLAccountCurrencies.Add(entity);
        }

        public void Remove(GLAccountCurrency entity)
        {
            context.GLAccountCurrencies.Attach(entity);
            context.GLAccountCurrencies.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccountCurrency entity)
        {
            onUpdate();
            context.GLAccountCurrencies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccountCurrency> All()
        {
            return context.GLAccountCurrencies.ToList();
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
	 