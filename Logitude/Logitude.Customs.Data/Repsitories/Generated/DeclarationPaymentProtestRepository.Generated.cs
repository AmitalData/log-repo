 
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
   public partial class DeclarationPaymentProtestRepository:IRepository<DeclarationPaymentProtest>
   {
   
        private ICustomContext currentContext;
        public DeclarationPaymentProtestRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationPaymentProtestRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationPaymentProtest GetSingle(string declarationid, int line, int tenant)
        {
            return (from a in context.DeclarationPaymentProtests
                    where a.DeclarationId == declarationid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationPaymentProtest> GetAll(int tenant)
        {
            return from a in context.DeclarationPaymentProtests  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationPaymentProtest GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationPaymentProtestKeys keys = entityKeys as DeclarationPaymentProtestKeys;
            return (from a in context.DeclarationPaymentProtests
                    where a.DeclarationId == keys.DeclarationId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationPaymentProtest entity)
        {
            onAdd();
            context.DeclarationPaymentProtests.Add(entity);
        }

        public void Remove(DeclarationPaymentProtest entity)
        {
            context.DeclarationPaymentProtests.Attach(entity);
            context.DeclarationPaymentProtests.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationPaymentProtest entity)
        {
            onUpdate();
            context.DeclarationPaymentProtests.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationPaymentProtest> All()
        {
            return context.DeclarationPaymentProtests.ToList();
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
	 