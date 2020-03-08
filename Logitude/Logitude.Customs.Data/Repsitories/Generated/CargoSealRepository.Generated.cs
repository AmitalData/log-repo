 
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
   public partial class CargoSealRepository:IRepository<CargoSeal>
   {
   
        private ICustomContext currentContext;
        public CargoSealRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CargoSealRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoSeal GetSingle(string id, int tenant)
        {
            return (from a in context.CargoSeals
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoSeal> GetAll(int tenant)
        {
            return from a in context.CargoSeals  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CargoSeal GetSingle(EntityKeyFields entityKeys)
        {
            CargoSealKeys keys = entityKeys as CargoSealKeys;
            return (from a in context.CargoSeals
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoSeal entity)
        {
            onAdd();
            context.CargoSeals.Add(entity);
        }

        public void Remove(CargoSeal entity)
        {
            context.CargoSeals.Attach(entity);
            context.CargoSeals.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoSeal entity)
        {
            onUpdate();
            context.CargoSeals.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoSeal> All()
        {
            return context.CargoSeals.ToList();
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
	 