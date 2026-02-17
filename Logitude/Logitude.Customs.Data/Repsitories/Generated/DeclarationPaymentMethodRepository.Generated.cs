 
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
   public partial class DeclarationPaymentMethodRepository:IRepository<DeclarationPaymentMethod>
   {
   
        private ICustomContext currentContext;
        public DeclarationPaymentMethodRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationPaymentMethodRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationPaymentMethod GetSingle(string declarationid, int line, int tenant)
        {
            return (from a in context.DeclarationPaymentMethods
                    where a.DeclarationId == declarationid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationPaymentMethod> GetAll(int tenant)
        {
            return from a in context.DeclarationPaymentMethods  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationPaymentMethod GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationPaymentMethodKeys keys = entityKeys as DeclarationPaymentMethodKeys;
            return (from a in context.DeclarationPaymentMethods
                    where a.DeclarationId == keys.DeclarationId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationPaymentMethod entity)
        {
            onAdd();
            context.DeclarationPaymentMethods.Add(entity);
        }

        public void Remove(DeclarationPaymentMethod entity)
        {
            context.DeclarationPaymentMethods.Attach(entity);
            context.DeclarationPaymentMethods.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationPaymentMethod entity)
        {
            onUpdate();
            context.DeclarationPaymentMethods.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationPaymentMethod> All()
        {
            return context.DeclarationPaymentMethods.ToList();
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
	 