 
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
   public partial class CargoTrackingIncrementalStatRepository:IRepository<CargoTrackingIncrementalStat>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingIncrementalStatRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingIncrementalStatRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingIncrementalStat GetSingle(int id)
        {
            return (from a in context.CargoTrackingIncrementalStats
                    where a.Id == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingIncrementalStat> GetAll()
        {
            return from a in context.CargoTrackingIncrementalStats  
                   select a;
        }
				 
        public CargoTrackingIncrementalStat GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingIncrementalStatKeys keys = entityKeys as CargoTrackingIncrementalStatKeys;
            return (from a in context.CargoTrackingIncrementalStats
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingIncrementalStat entity)
        {
            onAdd();
            context.CargoTrackingIncrementalStats.Add(entity);
        }

        public void Remove(CargoTrackingIncrementalStat entity)
        {
            context.CargoTrackingIncrementalStats.Attach(entity);
            context.CargoTrackingIncrementalStats.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingIncrementalStat entity)
        {
            onUpdate();
            context.CargoTrackingIncrementalStats.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingIncrementalStat> All()
        {
            return context.CargoTrackingIncrementalStats.ToList();
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
	 