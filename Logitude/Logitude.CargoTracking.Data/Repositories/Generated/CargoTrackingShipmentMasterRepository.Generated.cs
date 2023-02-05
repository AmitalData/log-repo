 
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
   public partial class CargoTrackingShipmentMasterRepository:IRepository<CargoTrackingShipmentMaster>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingShipmentMasterRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingShipmentMasterRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingShipmentMaster GetSingle(string id, int tenant)
        {
            return (from a in context.CargoTrackingShipmentMasters
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingShipmentMaster> GetAll(int tenant)
        {
            return from a in context.CargoTrackingShipmentMasters  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CargoTrackingShipmentMaster GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingShipmentMasterKeys keys = entityKeys as CargoTrackingShipmentMasterKeys;
            return (from a in context.CargoTrackingShipmentMasters
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingShipmentMaster entity)
        {
            onAdd();
            context.CargoTrackingShipmentMasters.Add(entity);
        }

        public void Remove(CargoTrackingShipmentMaster entity)
        {
            context.CargoTrackingShipmentMasters.Attach(entity);
            context.CargoTrackingShipmentMasters.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingShipmentMaster entity)
        {
            onUpdate();
            context.CargoTrackingShipmentMasters.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingShipmentMaster> All()
        {
            return context.CargoTrackingShipmentMasters.ToList();
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
	 