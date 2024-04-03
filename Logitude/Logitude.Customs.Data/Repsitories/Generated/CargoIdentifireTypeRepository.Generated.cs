 
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
   public partial class CargoIdentifireTypeRepository:IRepository<CargoIdentifireType>
   {
   
        private ICustomContext currentContext;
        public CargoIdentifireTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CargoIdentifireTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoIdentifireType GetSingle(string code)
        {
            return (from a in context.CargoIdentifireTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoIdentifireType> GetAll()
        {
            return from a in context.CargoIdentifireTypes  
                   select a;
        }
				 
        public CargoIdentifireType GetSingle(EntityKeyFields entityKeys)
        {
            CargoIdentifireTypeKeys keys = entityKeys as CargoIdentifireTypeKeys;
            return (from a in context.CargoIdentifireTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoIdentifireType entity)
        {
            onAdd();
            context.CargoIdentifireTypes.Add(entity);
        }

        public void Remove(CargoIdentifireType entity)
        {
            context.CargoIdentifireTypes.Attach(entity);
            context.CargoIdentifireTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoIdentifireType entity)
        {
            onUpdate();
            context.CargoIdentifireTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoIdentifireType> All()
        {
            return context.CargoIdentifireTypes.ToList();
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
	 