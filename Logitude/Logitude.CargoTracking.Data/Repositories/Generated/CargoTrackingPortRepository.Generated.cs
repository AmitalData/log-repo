 
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
   public partial class CargoTrackingPortRepository:IRepository<CargoTrackingPort>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingPortRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingPortRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingPort GetSingle(string id, int tenant)
        {
            return (from a in context.CargoTrackingPorts
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingPort> GetAll(int tenant)
        {
            return from a in context.CargoTrackingPorts  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CargoTrackingPort GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingPortKeys keys = entityKeys as CargoTrackingPortKeys;
            return (from a in context.CargoTrackingPorts
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingPort entity)
        {
            onAdd();
            context.CargoTrackingPorts.Add(entity);
        }

        public void Remove(CargoTrackingPort entity)
        {
            context.CargoTrackingPorts.Attach(entity);
            context.CargoTrackingPorts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingPort entity)
        {
            onUpdate();
            context.CargoTrackingPorts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingPort> All()
        {
            return context.CargoTrackingPorts.ToList();
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
	 