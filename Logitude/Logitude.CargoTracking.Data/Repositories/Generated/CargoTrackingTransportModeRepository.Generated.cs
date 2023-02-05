 
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
   public partial class CargoTrackingTransportModeRepository:IRepository<CargoTrackingTransportMode>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingTransportModeRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingTransportModeRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingTransportMode GetSingle(string id)
        {
            return (from a in context.CargoTrackingTransportModes
                    where a.Id == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingTransportMode> GetAll()
        {
            return from a in context.CargoTrackingTransportModes  
                   select a;
        }
				 
        public CargoTrackingTransportMode GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingTransportModeKeys keys = entityKeys as CargoTrackingTransportModeKeys;
            return (from a in context.CargoTrackingTransportModes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingTransportMode entity)
        {
            onAdd();
            context.CargoTrackingTransportModes.Add(entity);
        }

        public void Remove(CargoTrackingTransportMode entity)
        {
            context.CargoTrackingTransportModes.Attach(entity);
            context.CargoTrackingTransportModes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingTransportMode entity)
        {
            onUpdate();
            context.CargoTrackingTransportModes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingTransportMode> All()
        {
            return context.CargoTrackingTransportModes.ToList();
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
	 