 
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
   public partial class CargoTrackingHeaderEntityTypeRepository:IRepository<CargoTrackingHeaderEntityType>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingHeaderEntityTypeRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingHeaderEntityTypeRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingHeaderEntityType GetSingle(string code)
        {
            return (from a in context.CargoTrackingHeaderEntityTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingHeaderEntityType> GetAll()
        {
            return from a in context.CargoTrackingHeaderEntityTypes  
                   select a;
        }
				 
        public CargoTrackingHeaderEntityType GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingHeaderEntityTypeKeys keys = entityKeys as CargoTrackingHeaderEntityTypeKeys;
            return (from a in context.CargoTrackingHeaderEntityTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingHeaderEntityType entity)
        {
            onAdd();
            context.CargoTrackingHeaderEntityTypes.Add(entity);
        }

        public void Remove(CargoTrackingHeaderEntityType entity)
        {
            context.CargoTrackingHeaderEntityTypes.Attach(entity);
            context.CargoTrackingHeaderEntityTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingHeaderEntityType entity)
        {
            onUpdate();
            context.CargoTrackingHeaderEntityTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingHeaderEntityType> All()
        {
            return context.CargoTrackingHeaderEntityTypes.ToList();
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
	 