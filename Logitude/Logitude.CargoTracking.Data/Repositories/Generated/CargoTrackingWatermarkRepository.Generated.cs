 
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
   public partial class CargoTrackingWatermarkRepository:IRepository<CargoTrackingWatermark>
   {
   
        private ICargoTrackingContext currentContext;
        public CargoTrackingWatermarkRepository(int tenant)
        {
            currentContext = CargoTrackingContext.GetContext(tenant);
        }

        public CargoTrackingWatermarkRepository(ICargoTrackingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoTrackingWatermark GetSingle(string tablename)
        {
            return (from a in context.CargoTrackingWatermarks
                    where a.TableName == tablename 
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoTrackingWatermark> GetAll()
        {
            return from a in context.CargoTrackingWatermarks  
                   select a;
        }
				 
        public CargoTrackingWatermark GetSingle(EntityKeyFields entityKeys)
        {
            CargoTrackingWatermarkKeys keys = entityKeys as CargoTrackingWatermarkKeys;
            return (from a in context.CargoTrackingWatermarks
                    where a.TableName == keys.TableName
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoTrackingWatermark entity)
        {
            onAdd();
            context.CargoTrackingWatermarks.Add(entity);
        }

        public void Remove(CargoTrackingWatermark entity)
        {
            context.CargoTrackingWatermarks.Attach(entity);
            context.CargoTrackingWatermarks.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoTrackingWatermark entity)
        {
            onUpdate();
            context.CargoTrackingWatermarks.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoTrackingWatermark> All()
        {
            return context.CargoTrackingWatermarks.ToList();
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
	 