 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CarEngineVolumeRepository:IRepository<CarEngineVolume>
   {
   
        private ICustomContext currentContext;
        public CarEngineVolumeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CarEngineVolumeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CarEngineVolume GetSingle(string code)
        {
            return (from a in context.CarEngineVolumes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CarEngineVolume> GetAll()
        {
            return from a in context.CarEngineVolumes  
                   select a;
        }
				 
        public CarEngineVolume GetSingle(EntityKeyFields entityKeys)
        {
            CarEngineVolumeKeys keys = entityKeys as CarEngineVolumeKeys;
            return (from a in context.CarEngineVolumes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CarEngineVolume entity)
        {
            onAdd();
            context.CarEngineVolumes.Add(entity);
        }

        public void Remove(CarEngineVolume entity)
        {
            context.CarEngineVolumes.Attach(entity);
            context.CarEngineVolumes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CarEngineVolume entity)
        {
            onUpdate();
            context.CarEngineVolumes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CarEngineVolume> All()
        {
            return context.CarEngineVolumes.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 