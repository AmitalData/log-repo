 
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
   public partial class CargoTrackingCardRepository:IRepository<CargoTrackingCard>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingCardRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingCardRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingCard GetSingle(string id, int tenant)
        {
            return (from a in context.CargoTrackingCards
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingCard> GetAll(int tenant)
        {
            return from a in context.CargoTrackingCards  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CargoTrackingCard GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingCardKeys keys = entityKeys as CargoTrackingCardKeys;
            return (from a in context.CargoTrackingCards
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingCard entity)
        {
            onAdd();
            context.CargoTrackingCards.Add(entity);
        }

        public void Remove(CargoTrackingCard entity)
        {
            context.CargoTrackingCards.Attach(entity);
            context.CargoTrackingCards.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingCard entity)
        {
            onUpdate();
            context.CargoTrackingCards.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingCard> All()
        {
            return context.CargoTrackingCards.ToList();
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
	 