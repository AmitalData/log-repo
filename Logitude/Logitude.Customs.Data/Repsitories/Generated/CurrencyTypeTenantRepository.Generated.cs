 
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
   public partial class CurrencyTypeTenantRepository:IRepository<CurrencyTypeTenant>
   {
   
        private ICustomContext currentContext;
        public CurrencyTypeTenantRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CurrencyTypeTenantRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CurrencyTypeTenant GetSingle(string id, int tenant)
        {
            return (from a in context.CurrencyTypeTenants
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CurrencyTypeTenant> GetAll(int tenant)
        {
            return from a in context.CurrencyTypeTenants  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CurrencyTypeTenant GetSingle(EntityKeyFields entityKeys)
        {
            CurrencyTypeTenantKeys keys = entityKeys as CurrencyTypeTenantKeys;
            return (from a in context.CurrencyTypeTenants
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CurrencyTypeTenant entity)
        {
            onAdd();
            context.CurrencyTypeTenants.Add(entity);
        }

        public void Remove(CurrencyTypeTenant entity)
        {
            context.CurrencyTypeTenants.Attach(entity);
            context.CurrencyTypeTenants.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CurrencyTypeTenant entity)
        {
            onUpdate();
            context.CurrencyTypeTenants.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CurrencyTypeTenant> All()
        {
            return context.CurrencyTypeTenants.ToList();
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
	 