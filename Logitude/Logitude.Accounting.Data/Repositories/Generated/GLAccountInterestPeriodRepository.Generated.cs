 
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
   public partial class GLAccountInterestPeriodRepository:IRepository<GLAccountInterestPeriod>
   {
   
        private IAccountingContext currentContext;
        public GLAccountInterestPeriodRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountInterestPeriodRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccountInterestPeriod GetSingle(int linenumber, string glaccountid, int tenant)
        {
            return (from a in context.GLAccountInterestPeriods
                    where a.LineNumber == linenumber && a.GLAccountId == glaccountid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccountInterestPeriod> GetAll(int tenant)
        {
            return from a in context.GLAccountInterestPeriods  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GLAccountInterestPeriod GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountInterestPeriodKeys keys = entityKeys as GLAccountInterestPeriodKeys;
            return (from a in context.GLAccountInterestPeriods
                    where a.LineNumber == keys.LineNumber && a.GLAccountId == keys.GLAccountId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccountInterestPeriod entity)
        {
            onAdd();
            context.GLAccountInterestPeriods.Add(entity);
        }

        public void Remove(GLAccountInterestPeriod entity)
        {
            context.GLAccountInterestPeriods.Attach(entity);
            context.GLAccountInterestPeriods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccountInterestPeriod entity)
        {
            onUpdate();
            context.GLAccountInterestPeriods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccountInterestPeriod> All()
        {
            return context.GLAccountInterestPeriods.ToList();
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
	 