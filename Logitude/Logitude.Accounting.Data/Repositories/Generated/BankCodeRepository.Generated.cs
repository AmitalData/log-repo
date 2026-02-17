 
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
   public partial class BankCodeRepository:IRepository<BankCode>
   {
   
        private IAccountingContext currentContext;
        public BankCodeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public BankCodeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BankCode GetSingle(string id, int tenant)
        {
            return (from a in context.BankCodes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BankCode> GetAll(int tenant)
        {
            return from a in context.BankCodes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BankCode GetSingle(EntityKeyFields entityKeys)
        {
            BankCodeKeys keys = entityKeys as BankCodeKeys;
            return (from a in context.BankCodes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BankCode entity)
        {
            onAdd();
            context.BankCodes.Add(entity);
        }

        public void Remove(BankCode entity)
        {
            context.BankCodes.Attach(entity);
            context.BankCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BankCode entity)
        {
            onUpdate();
            context.BankCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BankCode> All()
        {
            return context.BankCodes.ToList();
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
	 