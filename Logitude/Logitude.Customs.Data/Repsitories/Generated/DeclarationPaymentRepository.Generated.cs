 
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
   public partial class DeclarationPaymentRepository:IRepository<DeclarationPayment>
   {
   
        private ICustomContext currentContext;
        public DeclarationPaymentRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationPaymentRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationPayment GetSingle(string declarationid, int tenant)
        {
            return (from a in context.DeclarationPayments
                    where a.DeclarationId == declarationid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationPayment> GetAll(int tenant)
        {
            return from a in context.DeclarationPayments  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationPayment GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationPaymentKeys keys = entityKeys as DeclarationPaymentKeys;
            return (from a in context.DeclarationPayments
                    where a.DeclarationId == keys.DeclarationId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationPayment entity)
        {
            onAdd();
            context.DeclarationPayments.Add(entity);
        }

        public void Remove(DeclarationPayment entity)
        {
            context.DeclarationPayments.Attach(entity);
            context.DeclarationPayments.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationPayment entity)
        {
            onUpdate();
            context.DeclarationPayments.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationPayment> All()
        {
            return context.DeclarationPayments.ToList();
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
	 