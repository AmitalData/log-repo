 
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
   public partial class DeclarationTaxRepository:IRepository<DeclarationTax>
   {
   
        private ICustomContext currentContext;
        public DeclarationTaxRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationTaxRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationTax GetSingle(string declarationid, string taxtypecode, int tenant)
        {
            return (from a in context.DeclarationTaxes
                    where a.DeclarationId == declarationid && a.TaxTypeCode == taxtypecode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationTax> GetAll(int tenant)
        {
            return from a in context.DeclarationTaxes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationTax GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationTaxKeys keys = entityKeys as DeclarationTaxKeys;
            return (from a in context.DeclarationTaxes
                    where a.DeclarationId == keys.DeclarationId && a.TaxTypeCode == keys.TaxTypeCode
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationTax entity)
        {
            onAdd();
            context.DeclarationTaxes.Add(entity);
        }

        public void Remove(DeclarationTax entity)
        {
            context.DeclarationTaxes.Attach(entity);
            context.DeclarationTaxes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationTax entity)
        {
            onUpdate();
            context.DeclarationTaxes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationTax> All()
        {
            return context.DeclarationTaxes.ToList();
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
	 