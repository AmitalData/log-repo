 
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
   public partial class BankDepositLineRepository:IRepository<BankDepositLine>
   {
   
        private IAccountingContext currentContext;
        public BankDepositLineRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public BankDepositLineRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BankDepositLine GetSingle(string depositid, int line, int tenant)
        {
            return (from a in context.BankDepositLines
                    where a.DepositId == depositid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BankDepositLine> GetAll(int tenant)
        {
            return from a in context.BankDepositLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BankDepositLine GetSingle(EntityKeyFields entityKeys)
        {
            BankDepositLineKeys keys = entityKeys as BankDepositLineKeys;
            return (from a in context.BankDepositLines
                    where a.DepositId == keys.DepositId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BankDepositLine entity)
        {
            onAdd();
            context.BankDepositLines.Add(entity);
        }

        public void Remove(BankDepositLine entity)
        {
            context.BankDepositLines.Attach(entity);
            context.BankDepositLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BankDepositLine entity)
        {
            onUpdate();
            context.BankDepositLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BankDepositLine> All()
        {
            return context.BankDepositLines.ToList();
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
	 