 
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
   public partial class CashBookRepository:IRepository<CashBook>
   {
   
        private IAccountingContext currentContext;
        public CashBookRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public CashBookRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CashBook GetSingle(string id, int tenant)
        {
            return (from a in context.CashBooks
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CashBook> GetAll(int tenant)
        {
            return from a in context.CashBooks  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CashBook GetSingle(EntityKeyFields entityKeys)
        {
            CashBookKeys keys = entityKeys as CashBookKeys;
            return (from a in context.CashBooks
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CashBook entity)
        {
            onAdd();
            context.CashBooks.Add(entity);
        }

        public void Remove(CashBook entity)
        {
            context.CashBooks.Attach(entity);
            context.CashBooks.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CashBook entity)
        {
            onUpdate();
            context.CashBooks.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CashBook> All()
        {
            return context.CashBooks.ToList();
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
	 