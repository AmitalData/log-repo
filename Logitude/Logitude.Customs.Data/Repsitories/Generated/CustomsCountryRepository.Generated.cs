 
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
   public partial class CustomsCountryRepository:IRepository<CustomsCountry>
   {
   
        private ICustomContext currentContext;
        public CustomsCountryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsCountryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsCountry GetSingle(string code)
        {
            return (from a in context.CustomsCountries
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsCountry> GetAll()
        {
            return from a in context.CustomsCountries  
                   select a;
        }
				 
        public CustomsCountry GetSingle(EntityKeyFields entityKeys)
        {
            CustomsCountryKeys keys = entityKeys as CustomsCountryKeys;
            return (from a in context.CustomsCountries
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsCountry entity)
        {
            onAdd();
            context.CustomsCountries.Add(entity);
        }

        public void Remove(CustomsCountry entity)
        {
            context.CustomsCountries.Attach(entity);
            context.CustomsCountries.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsCountry entity)
        {
            onUpdate();
            context.CustomsCountries.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsCountry> All()
        {
            return context.CustomsCountries.ToList();
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
	 