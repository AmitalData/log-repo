 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class PaymentOrderConnectionTableRepository:IRepository<PaymentOrderConnectionTable>
   {
   
        private ICustomContext currentContext;
        public PaymentOrderConnectionTableRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PaymentOrderConnectionTableRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentOrderConnectionTable GetSingle(string paymentorderid, string connectedentityid, int tenant)
        {
            return (from a in context.PaymentOrderConnectionTables
                    where a.PaymentOrderId == paymentorderid && a.ConnectedEntityId == connectedentityid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentOrderConnectionTable> GetAll(int tenant)
        {
            return from a in context.PaymentOrderConnectionTables  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public PaymentOrderConnectionTable GetSingle(EntityKeyFields entityKeys)
        {
            PaymentOrderConnectionTableKeys keys = entityKeys as PaymentOrderConnectionTableKeys;
            return (from a in context.PaymentOrderConnectionTables
                    where a.PaymentOrderId == keys.PaymentOrderId && a.ConnectedEntityId == keys.ConnectedEntityId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentOrderConnectionTable entity)
        {
            onAdd();
            context.PaymentOrderConnectionTables.Add(entity);
        }

        public void Remove(PaymentOrderConnectionTable entity)
        {
            context.PaymentOrderConnectionTables.Attach(entity);
            context.PaymentOrderConnectionTables.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentOrderConnectionTable entity)
        {
            onUpdate();
            context.PaymentOrderConnectionTables.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentOrderConnectionTable> All()
        {
            return context.PaymentOrderConnectionTables.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 