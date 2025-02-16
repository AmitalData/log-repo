 
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
   public partial class Aur_PaymentItemRepository:IRepository<Aur_PaymentItem>
   {
   
        private IAccountingContext currentContext;
        public Aur_PaymentItemRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public Aur_PaymentItemRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Aur_PaymentItem GetSingle(string paymentid, int line, int tenant)
        {
            return (from a in context.Aur_PaymentItems
                    where a.PaymentId == paymentid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Aur_PaymentItem> GetAll(int tenant)
        {
            return from a in context.Aur_PaymentItems  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Aur_PaymentItem GetSingle(EntityKeyFields entityKeys)
        {
            Aur_PaymentItemKeys keys = entityKeys as Aur_PaymentItemKeys;
            return (from a in context.Aur_PaymentItems
                    where a.PaymentId == keys.PaymentId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Aur_PaymentItem entity)
        {
            onAdd();
            context.Aur_PaymentItems.Add(entity);
        }

        public void Remove(Aur_PaymentItem entity)
        {
            context.Aur_PaymentItems.Attach(entity);
            context.Aur_PaymentItems.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Aur_PaymentItem entity)
        {
            onUpdate();
            context.Aur_PaymentItems.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Aur_PaymentItem> All()
        {
            return context.Aur_PaymentItems.ToList();
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
	 