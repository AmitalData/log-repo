 
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
   public partial class CargoTrackingShipmentRepository:IRepository<CargoTrackingShipment>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingShipmentRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingShipmentRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingShipment GetSingle(int id, int tenant)
        {
            return (from a in context.CargoTrackingShipments
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingShipment> GetAll(int tenant)
        {
            return from a in context.CargoTrackingShipments  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CargoTrackingShipment GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingShipmentKeys keys = entityKeys as CargoTrackingShipmentKeys;
            return (from a in context.CargoTrackingShipments
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingShipment entity)
        {
            onAdd();
            context.CargoTrackingShipments.Add(entity);
        }

        public void Remove(CargoTrackingShipment entity)
        {
            context.CargoTrackingShipments.Attach(entity);
            context.CargoTrackingShipments.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingShipment entity)
        {
            onUpdate();
            context.CargoTrackingShipments.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingShipment> All()
        {
            return context.CargoTrackingShipments.ToList();
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
	 