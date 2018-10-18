 
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
   public partial class SubCountryRepository:IRepository<SubCountry>
   {
   
        private ICustomContext currentContext;
        public SubCountryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SubCountryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SubCountry GetSingle(string code)
        {
            return (from a in context.SubCountries
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SubCountry> GetAll()
        {
            return from a in context.SubCountries  
                   select a;
        }
				 
        public SubCountry GetSingle(EntityKeyFields entityKeys)
        {
            SubCountryKeys keys = entityKeys as SubCountryKeys;
            return (from a in context.SubCountries
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SubCountry entity)
        {
            onAdd();
            context.SubCountries.Add(entity);
        }

        public void Remove(SubCountry entity)
        {
            context.SubCountries.Attach(entity);
            context.SubCountries.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SubCountry entity)
        {
            onUpdate();
            context.SubCountries.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SubCountry> All()
        {
            return context.SubCountries.ToList();
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
	 