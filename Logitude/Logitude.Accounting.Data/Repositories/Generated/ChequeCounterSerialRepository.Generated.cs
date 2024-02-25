 
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
   public partial class ChequeCounterSerialRepository:IRepository<ChequeCounterSerial>
   {
   
        private IAccountingContext currentContext;
        public ChequeCounterSerialRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ChequeCounterSerialRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ChequeCounterSerial GetSingle(string id, int tenant)
        {
            return (from a in context.ChequeCounterSerials
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ChequeCounterSerial> GetAll(int tenant)
        {
            return from a in context.ChequeCounterSerials  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ChequeCounterSerial GetSingle(EntityKeyFields entityKeys)
        {
            ChequeCounterSerialKeys keys = entityKeys as ChequeCounterSerialKeys;
            return (from a in context.ChequeCounterSerials
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ChequeCounterSerial entity)
        {
            onAdd();
            context.ChequeCounterSerials.Add(entity);
        }

        public void Remove(ChequeCounterSerial entity)
        {
            context.ChequeCounterSerials.Attach(entity);
            context.ChequeCounterSerials.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ChequeCounterSerial entity)
        {
            onUpdate();
            context.ChequeCounterSerials.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ChequeCounterSerial> All()
        {
            return context.ChequeCounterSerials.ToList();
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
	 