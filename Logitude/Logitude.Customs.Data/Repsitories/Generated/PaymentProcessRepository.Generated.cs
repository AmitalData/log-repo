 
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
   public partial class PaymentProcessRepository:IRepository<PaymentProcess>
   {
   
        private ICustomContext currentContext;
        public PaymentProcessRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PaymentProcessRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PaymentProcess GetSingle(string code)
        {
            return (from a in context.PaymentProcesses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PaymentProcess> GetAll()
        {
            return from a in context.PaymentProcesses  
                   select a;
        }
				 
        public PaymentProcess GetSingle(EntityKeyFields entityKeys)
        {
            PaymentProcessKeys keys = entityKeys as PaymentProcessKeys;
            return (from a in context.PaymentProcesses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PaymentProcess entity)
        {
            onAdd();
            context.PaymentProcesses.Add(entity);
        }

        public void Remove(PaymentProcess entity)
        {
            context.PaymentProcesses.Attach(entity);
            context.PaymentProcesses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PaymentProcess entity)
        {
            onUpdate();
            context.PaymentProcesses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PaymentProcess> All()
        {
            return context.PaymentProcesses.ToList();
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
	 