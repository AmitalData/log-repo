 
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
   public partial class CargoDisconnectQueueRepository:IRepository<CargoDisconnectQueue>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoDisconnectQueueRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoDisconnectQueueRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoDisconnectQueue GetSingle(int id, int tenant)
        {
            return (from a in context.CargoDisconnectQueues
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoDisconnectQueue> GetAll(int tenant)
        {
            return from a in context.CargoDisconnectQueues  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CargoDisconnectQueue GetSingle(EntityKeyFields entityKeys)
        {
            CargoDisconnectQueueKeys keys = entityKeys as CargoDisconnectQueueKeys;
            return (from a in context.CargoDisconnectQueues
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoDisconnectQueue entity)
        {
            onAdd();
            context.CargoDisconnectQueues.Add(entity);
        }

        public void Remove(CargoDisconnectQueue entity)
        {
            context.CargoDisconnectQueues.Attach(entity);
            context.CargoDisconnectQueues.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoDisconnectQueue entity)
        {
            onUpdate();
            context.CargoDisconnectQueues.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoDisconnectQueue> All()
        {
            return context.CargoDisconnectQueues.ToList();
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
	 