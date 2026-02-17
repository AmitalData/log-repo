 
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
   public partial class PaymentOrderLineRepository:IRepository<PaymentOrderLine>
   {
   
        private ICustomContext currentContext;
        public PaymentOrderLineRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PaymentOrderLineRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentOrderLine GetSingle(string paymentorderid, string paragraphtypecode, int tenant)
        {
            return (from a in context.PaymentOrderLines
                    where a.PaymentOrderId == paymentorderid && a.ParagraphTypeCode == paragraphtypecode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentOrderLine> GetAll(int tenant)
        {
            return from a in context.PaymentOrderLines  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public PaymentOrderLine GetSingle(EntityKeyFields entityKeys)
        {
            PaymentOrderLineKeys keys = entityKeys as PaymentOrderLineKeys;
            return (from a in context.PaymentOrderLines
                    where a.PaymentOrderId == keys.PaymentOrderId && a.ParagraphTypeCode == keys.ParagraphTypeCode
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentOrderLine entity)
        {
            onAdd();
            context.PaymentOrderLines.Add(entity);
        }

        public void Remove(PaymentOrderLine entity)
        {
            context.PaymentOrderLines.Attach(entity);
            context.PaymentOrderLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentOrderLine entity)
        {
            onUpdate();
            context.PaymentOrderLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentOrderLine> All()
        {
            return context.PaymentOrderLines.ToList();
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
	 