 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class AdditionalCurrencyRateRepository:IRepository<AdditionalCurrencyRate>
   {
   
        private IAccountingContext currentContext;
        public AdditionalCurrencyRateRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public AdditionalCurrencyRateRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  AdditionalCurrencyRate GetSingle(string id, int tenant)
        {
            return (from a in context.AdditionalCurrencyRates
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AdditionalCurrencyRate> GetAll(int tenant)
        {
            return from a in context.AdditionalCurrencyRates  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public AdditionalCurrencyRate GetSingle(EntityKeyFields entityKeys)
        {
            AdditionalCurrencyRateKeys keys = entityKeys as AdditionalCurrencyRateKeys;
            return (from a in context.AdditionalCurrencyRates
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AdditionalCurrencyRate entity)
        {
            onAdd();
            context.AdditionalCurrencyRates.Add(entity);
        }

        public void Remove(AdditionalCurrencyRate entity)
        {
            context.AdditionalCurrencyRates.Attach(entity);
            context.AdditionalCurrencyRates.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AdditionalCurrencyRate entity)
        {
            onUpdate();
            context.AdditionalCurrencyRates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AdditionalCurrencyRate> All()
        {
            return context.AdditionalCurrencyRates.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 