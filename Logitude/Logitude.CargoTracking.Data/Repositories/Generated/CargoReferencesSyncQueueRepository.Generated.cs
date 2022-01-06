 
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
   public partial class CargoReferencesSyncQueueRepository:IRepository<CargoReferencesSyncQueue>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoReferencesSyncQueueRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoReferencesSyncQueueRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoReferencesSyncQueue GetSingle(string id, int tenant)
        {
            return (from a in context.CargoReferencesSyncQueues
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoReferencesSyncQueue> GetAll(int tenant)
        {
            return from a in context.CargoReferencesSyncQueues  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CargoReferencesSyncQueue GetSingle(EntityKeyFields entityKeys)
        {
            CargoReferencesSyncQueueKeys keys = entityKeys as CargoReferencesSyncQueueKeys;
            return (from a in context.CargoReferencesSyncQueues
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoReferencesSyncQueue entity)
        {
            onAdd();
            context.CargoReferencesSyncQueues.Add(entity);
        }

        public void Remove(CargoReferencesSyncQueue entity)
        {
            context.CargoReferencesSyncQueues.Attach(entity);
            context.CargoReferencesSyncQueues.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoReferencesSyncQueue entity)
        {
            onUpdate();
            context.CargoReferencesSyncQueues.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoReferencesSyncQueue> All()
        {
            return context.CargoReferencesSyncQueues.ToList();
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
	 