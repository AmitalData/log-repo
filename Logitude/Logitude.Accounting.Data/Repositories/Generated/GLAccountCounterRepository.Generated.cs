 
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
   public partial class GLAccountCounterRepository:IRepository<GLAccountCounter>
   {
   
        private IAccountingContext currentContext;
        public GLAccountCounterRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountCounterRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccountCounter GetSingle(string id, int tenant)
        {
            return (from a in context.GLAccountCounters
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccountCounter> GetAll(int tenant)
        {
            return from a in context.GLAccountCounters  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GLAccountCounter GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountCounterKeys keys = entityKeys as GLAccountCounterKeys;
            return (from a in context.GLAccountCounters
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccountCounter entity)
        {
            onAdd();
            context.GLAccountCounters.Add(entity);
        }

        public void Remove(GLAccountCounter entity)
        {
            context.GLAccountCounters.Attach(entity);
            context.GLAccountCounters.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccountCounter entity)
        {
            onUpdate();
            context.GLAccountCounters.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccountCounter> All()
        {
            return context.GLAccountCounters.ToList();
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
	 