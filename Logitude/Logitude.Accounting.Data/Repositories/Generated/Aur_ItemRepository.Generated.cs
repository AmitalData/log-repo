 
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
   public partial class Aur_ItemRepository:IRepository<Aur_Item>
   {
   
        private IAccountingContext currentContext;
        public Aur_ItemRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public Aur_ItemRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  Aur_Item GetSingle(string paymentid, int line, int tenant)
        {
            return (from a in context.Aur_Items
                    where a.PaymentId == paymentid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Aur_Item> GetAll(int tenant)
        {
            return from a in context.Aur_Items  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Aur_Item GetSingle(EntityKeyFields entityKeys)
        {
            Aur_ItemKeys keys = entityKeys as Aur_ItemKeys;
            return (from a in context.Aur_Items
                    where a.PaymentId == keys.PaymentId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Aur_Item entity)
        {
            onAdd();
            context.Aur_Items.Add(entity);
        }

        public void Remove(Aur_Item entity)
        {
            context.Aur_Items.Attach(entity);
            context.Aur_Items.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Aur_Item entity)
        {
            onUpdate();
            context.Aur_Items.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Aur_Item> All()
        {
            return context.Aur_Items.ToList();
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
	 