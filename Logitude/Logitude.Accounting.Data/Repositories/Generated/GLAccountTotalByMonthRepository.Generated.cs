 
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
   public partial class GLAccountTotalByMonthRepository:IRepository<GLAccountTotalByMonth>
   {
   
        private IAccountingContext currentContext;
        public GLAccountTotalByMonthRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public GLAccountTotalByMonthRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  GLAccountTotalByMonth GetSingle(string accountid, string datetypecode, int year, int month, string currencyid, int tenant)
        {
            return (from a in context.GLAccountTotalByMonths
                    where a.AccountId == accountid && a.DateTypeCode == datetypecode && a.Year == year && a.Month == month && a.CurrencyId == currencyid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<GLAccountTotalByMonth> GetAll(int tenant)
        {
            return from a in context.GLAccountTotalByMonths  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public GLAccountTotalByMonth GetSingle(EntityKeyFields entityKeys)
        {
            GLAccountTotalByMonthKeys keys = entityKeys as GLAccountTotalByMonthKeys;
            return (from a in context.GLAccountTotalByMonths
                    where a.AccountId == keys.AccountId && a.DateTypeCode == keys.DateTypeCode && a.Year == keys.Year && a.Month == keys.Month && a.CurrencyId == keys.CurrencyId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GLAccountTotalByMonth entity)
        {
            onAdd();
            context.GLAccountTotalByMonths.Add(entity);
        }

        public void Remove(GLAccountTotalByMonth entity)
        {
            context.GLAccountTotalByMonths.Attach(entity);
            context.GLAccountTotalByMonths.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GLAccountTotalByMonth entity)
        {
            onUpdate();
            context.GLAccountTotalByMonths.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GLAccountTotalByMonth> All()
        {
            return context.GLAccountTotalByMonths.ToList();
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
	 