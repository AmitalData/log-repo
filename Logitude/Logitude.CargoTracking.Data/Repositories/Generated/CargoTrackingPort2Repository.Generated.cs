 
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
   public partial class CargoTrackingPort2Repository:IRepository<CargoTrackingPort2>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingPort2Repository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingPort2Repository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingPort2 GetSingle(string id, int tenant)
        {
            return (from a in context.CargoTrackingPort2s
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingPort2> GetAll(int tenant)
        {
            return from a in context.CargoTrackingPort2s  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CargoTrackingPort2 GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingPort2Keys keys = entityKeys as CargoTrackingPort2Keys;
            return (from a in context.CargoTrackingPort2s
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingPort2 entity)
        {
            onAdd();
            context.CargoTrackingPort2s.Add(entity);
        }

        public void Remove(CargoTrackingPort2 entity)
        {
            context.CargoTrackingPort2s.Attach(entity);
            context.CargoTrackingPort2s.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingPort2 entity)
        {
            onUpdate();
            context.CargoTrackingPort2s.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingPort2> All()
        {
            return context.CargoTrackingPort2s.ToList();
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
	 