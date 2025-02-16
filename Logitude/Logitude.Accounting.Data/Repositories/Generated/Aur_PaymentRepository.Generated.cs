 
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
   public partial class Aur_PaymentRepository:IRepository<Aur_Payment>
   {
   
        private IAccountingContext currentContext;
        public Aur_PaymentRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public Aur_PaymentRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Aur_Payment GetSingle(string id, int tenant)
        {
            return (from a in context.Aur_Payments
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Aur_Payment> GetAll(int tenant)
        {
            return from a in context.Aur_Payments  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Aur_Payment GetSingle(EntityKeyFields entityKeys)
        {
            Aur_PaymentKeys keys = entityKeys as Aur_PaymentKeys;
            return (from a in context.Aur_Payments
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Aur_Payment entity)
        {
            onAdd();
            context.Aur_Payments.Add(entity);
        }

        public void Remove(Aur_Payment entity)
        {
            context.Aur_Payments.Attach(entity);
            context.Aur_Payments.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Aur_Payment entity)
        {
            onUpdate();
            context.Aur_Payments.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Aur_Payment> All()
        {
            return context.Aur_Payments.ToList();
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
	 