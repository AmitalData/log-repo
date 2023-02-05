 
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
   public partial class CargoTrackingShipmentSearchRepository:IRepository<CargoTrackingShipmentSearch>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingShipmentSearchRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingShipmentSearchRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingShipmentSearch GetSingle(int id, int tenant)
        {
            return (from a in context.CargoTrackingShipmentSearches
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingShipmentSearch> GetAll(int tenant)
        {
            return from a in context.CargoTrackingShipmentSearches  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CargoTrackingShipmentSearch GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingShipmentSearchKeys keys = entityKeys as CargoTrackingShipmentSearchKeys;
            return (from a in context.CargoTrackingShipmentSearches
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingShipmentSearch entity)
        {
            onAdd();
            context.CargoTrackingShipmentSearches.Add(entity);
        }

        public void Remove(CargoTrackingShipmentSearch entity)
        {
            context.CargoTrackingShipmentSearches.Attach(entity);
            context.CargoTrackingShipmentSearches.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingShipmentSearch entity)
        {
            onUpdate();
            context.CargoTrackingShipmentSearches.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingShipmentSearch> All()
        {
            return context.CargoTrackingShipmentSearches.ToList();
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
	 