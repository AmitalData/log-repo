 
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
   public partial class CargoStatusRepository:IRepository<CargoStatus>
   {
   
        private ICustomContext currentContext;
        public CargoStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CargoStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoStatus GetSingle(string code)
        {
            return (from a in context.CargoStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoStatus> GetAll()
        {
            return from a in context.CargoStatuses  
                   select a;
        }
				 
        public CargoStatus GetSingle(EntityKeyFields entityKeys)
        {
            CargoStatusKeys keys = entityKeys as CargoStatusKeys;
            return (from a in context.CargoStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoStatus entity)
        {
            onAdd();
            context.CargoStatuses.Add(entity);
        }

        public void Remove(CargoStatus entity)
        {
            context.CargoStatuses.Attach(entity);
            context.CargoStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoStatus entity)
        {
            onUpdate();
            context.CargoStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoStatus> All()
        {
            return context.CargoStatuses.ToList();
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
	 