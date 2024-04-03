 
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
   public partial class VendorCurrencyRepository:IRepository<VendorCurrency>
   {
   
        private ICustomContext currentContext;
        public VendorCurrencyRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VendorCurrencyRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VendorCurrency GetSingle(string vendorid, string currency, int tenant)
        {
            return (from a in context.VendorCurrencies
                    where a.VendorId == vendorid && a.Currency == currency && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<VendorCurrency> GetAll(int tenant)
        {
            return from a in context.VendorCurrencies  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public VendorCurrency GetSingle(EntityKeyFields entityKeys)
        {
            VendorCurrencyKeys keys = entityKeys as VendorCurrencyKeys;
            return (from a in context.VendorCurrencies
                    where a.VendorId == keys.VendorId && a.Currency == keys.Currency
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VendorCurrency entity)
        {
            onAdd();
            context.VendorCurrencies.Add(entity);
        }

        public void Remove(VendorCurrency entity)
        {
            context.VendorCurrencies.Attach(entity);
            context.VendorCurrencies.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VendorCurrency entity)
        {
            onUpdate();
            context.VendorCurrencies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VendorCurrency> All()
        {
            return context.VendorCurrencies.ToList();
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
	 