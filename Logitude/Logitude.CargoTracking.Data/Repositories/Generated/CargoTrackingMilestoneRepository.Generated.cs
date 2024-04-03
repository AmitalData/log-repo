 
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
   public partial class CargoTrackingMilestoneRepository:IRepository<CargoTrackingMilestone>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingMilestoneRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingMilestoneRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingMilestone GetSingle(string code)
        {
            return (from a in context.CargoTrackingMilestones
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingMilestone> GetAll()
        {
            return from a in context.CargoTrackingMilestones  
                   select a;
        }
				 
        public CargoTrackingMilestone GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingMilestoneKeys keys = entityKeys as CargoTrackingMilestoneKeys;
            return (from a in context.CargoTrackingMilestones
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingMilestone entity)
        {
            onAdd();
            context.CargoTrackingMilestones.Add(entity);
        }

        public void Remove(CargoTrackingMilestone entity)
        {
            context.CargoTrackingMilestones.Attach(entity);
            context.CargoTrackingMilestones.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingMilestone entity)
        {
            onUpdate();
            context.CargoTrackingMilestones.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingMilestone> All()
        {
            return context.CargoTrackingMilestones.ToList();
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
	 