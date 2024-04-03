 
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
   public partial class CargoTypeRepository:IRepository<CargoType>
   {
   
        private ICustomContext currentContext;
        public CargoTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CargoTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoType GetSingle(string code)
        {
            return (from a in context.CargoTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoType> GetAll()
        {
            return from a in context.CargoTypes  
                   select a;
        }
				 
        public CargoType GetSingle(EntityKeyFields entityKeys)
        {
            CargoTypeKeys keys = entityKeys as CargoTypeKeys;
            return (from a in context.CargoTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoType entity)
        {
            onAdd();
            context.CargoTypes.Add(entity);
        }

        public void Remove(CargoType entity)
        {
            context.CargoTypes.Attach(entity);
            context.CargoTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoType entity)
        {
            onUpdate();
            context.CargoTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoType> All()
        {
            return context.CargoTypes.ToList();
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
	 