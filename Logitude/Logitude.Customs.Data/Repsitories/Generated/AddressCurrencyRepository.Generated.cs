 
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
   public partial class AddressCurrencyRepository:IRepository<AddressCurrency>
   {
   
        private ICustomContext currentContext;
        public AddressCurrencyRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AddressCurrencyRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AddressCurrency GetSingle(string addressid, string currency, int tenant)
        {
            return (from a in context.AddressCurrencies
                    where a.AddressId == addressid && a.Currency == currency && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AddressCurrency> GetAll(int tenant)
        {
            return from a in context.AddressCurrencies  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public AddressCurrency GetSingle(EntityKeyFields entityKeys)
        {
            AddressCurrencyKeys keys = entityKeys as AddressCurrencyKeys;
            return (from a in context.AddressCurrencies
                    where a.AddressId == keys.AddressId && a.Currency == keys.Currency
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AddressCurrency entity)
        {
            onAdd();
            context.AddressCurrencies.Add(entity);
        }

        public void Remove(AddressCurrency entity)
        {
            context.AddressCurrencies.Attach(entity);
            context.AddressCurrencies.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AddressCurrency entity)
        {
            onUpdate();
            context.AddressCurrencies.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AddressCurrency> All()
        {
            return context.AddressCurrencies.ToList();
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
	 