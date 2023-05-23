 
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
   public partial class CargoTrackingShipmentComputedRepository:IRepository<CargoTrackingShipmentComputed>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingShipmentComputedRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingShipmentComputedRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingShipmentComputed GetSingle(string id, int tenant)
        {
            return (from a in context.CargoTrackingShipmentComputeds
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingShipmentComputed> GetAll(int tenant)
        {
            return from a in context.CargoTrackingShipmentComputeds  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CargoTrackingShipmentComputed GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingShipmentComputedKeys keys = entityKeys as CargoTrackingShipmentComputedKeys;
            return (from a in context.CargoTrackingShipmentComputeds
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingShipmentComputed entity)
        {
            onAdd();
            context.CargoTrackingShipmentComputeds.Add(entity);
        }

        public void Remove(CargoTrackingShipmentComputed entity)
        {
            context.CargoTrackingShipmentComputeds.Attach(entity);
            context.CargoTrackingShipmentComputeds.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingShipmentComputed entity)
        {
            onUpdate();
            context.CargoTrackingShipmentComputeds.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingShipmentComputed> All()
        {
            return context.CargoTrackingShipmentComputeds.ToList();
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
	 