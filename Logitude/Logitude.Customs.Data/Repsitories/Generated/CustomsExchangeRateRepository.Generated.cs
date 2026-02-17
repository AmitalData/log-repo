 
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
   public partial class CustomsExchangeRateRepository:IRepository<CustomsExchangeRate>
   {
   
        private ICustomContext currentContext;
        public CustomsExchangeRateRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsExchangeRateRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsExchangeRate GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsExchangeRates
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsExchangeRate> GetAll(int tenant)
        {
            return from a in context.CustomsExchangeRates  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsExchangeRate GetSingle(EntityKeyFields entityKeys)
        {
            CustomsExchangeRateKeys keys = entityKeys as CustomsExchangeRateKeys;
            return (from a in context.CustomsExchangeRates
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsExchangeRate entity)
        {
            onAdd();
            context.CustomsExchangeRates.Add(entity);
        }

        public void Remove(CustomsExchangeRate entity)
        {
            context.CustomsExchangeRates.Attach(entity);
            context.CustomsExchangeRates.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsExchangeRate entity)
        {
            onUpdate();
            context.CustomsExchangeRates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsExchangeRate> All()
        {
            return context.CustomsExchangeRates.ToList();
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
	 