 
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
   public partial class GLAccountRepository:IRepository<GLAccount>
   {
   
        private IAccountingContext currentContext;
        public GLAccountRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccount GetSingle(string id, int tenant)
        {
            return (from a in context.GLAccounts
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccount> GetAll(int tenant)
        {
            return from a in context.GLAccounts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GLAccount GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountKeys keys = entityKeys as GLAccountKeys;
            return (from a in context.GLAccounts
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccount entity)
        {
            onAdd();
            context.GLAccounts.Add(entity);
        }

        public void Remove(GLAccount entity)
        {
            context.GLAccounts.Attach(entity);
            context.GLAccounts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccount entity)
        {
            onUpdate();
            context.GLAccounts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccount> All()
        {
            return context.GLAccounts.ToList();
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
	 