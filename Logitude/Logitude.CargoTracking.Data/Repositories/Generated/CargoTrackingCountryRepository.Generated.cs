 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CargoTracking.Data.Repositories
{
   public partial class CargoTrackingCountryRepository:IRepository<CargoTrackingCountry>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingCountryRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingCountryRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingCountry GetSingle(string id, int tenant)
        {
            return (from a in context.CargoTrackingCountries
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingCountry> GetAll(int tenant)
        {
            return from a in context.CargoTrackingCountries  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CargoTrackingCountry GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingCountryKeys keys = entityKeys as CargoTrackingCountryKeys;
            return (from a in context.CargoTrackingCountries
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingCountry entity)
        {
            onAdd();
            context.CargoTrackingCountries.Add(entity);
        }

        public void Remove(CargoTrackingCountry entity)
        {
            context.CargoTrackingCountries.Attach(entity);
            context.CargoTrackingCountries.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingCountry entity)
        {
            onUpdate();
            context.CargoTrackingCountries.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingCountry> All()
        {
            return context.CargoTrackingCountries.ToList();
        }

        private ICargoTrackingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 