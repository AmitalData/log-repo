 
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
   public partial class CashBookLineRepository:IRepository<CashBookLine>
   {
   
        private IAccountingContext currentContext;
        public CashBookLineRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public CashBookLineRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CashBookLine GetSingle(string cashbookid, string arpchequeid, int tenant)
        {
            return (from a in context.CashBookLines
                    where a.CashBookId == cashbookid && a.ARPChequeId == arpchequeid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CashBookLine> GetAll(int tenant)
        {
            return from a in context.CashBookLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CashBookLine GetSingle(EntityKeyFields entityKeys)
        {
            CashBookLineKeys keys = entityKeys as CashBookLineKeys;
            return (from a in context.CashBookLines
                    where a.CashBookId == keys.CashBookId && a.ARPChequeId == keys.ARPChequeId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CashBookLine entity)
        {
            onAdd();
            context.CashBookLines.Add(entity);
        }

        public void Remove(CashBookLine entity)
        {
            context.CashBookLines.Attach(entity);
            context.CashBookLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CashBookLine entity)
        {
            onUpdate();
            context.CashBookLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CashBookLine> All()
        {
            return context.CashBookLines.ToList();
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
	 