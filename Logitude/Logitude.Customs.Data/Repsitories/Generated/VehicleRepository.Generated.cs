 
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
   public partial class VehicleRepository:IRepository<Vehicle>
   {
   
        private ICustomContext currentContext;
        public VehicleRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VehicleRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Vehicle GetSingle(string id, int tenant)
        {
            return (from a in context.Vehicles
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Vehicle> GetAll(int tenant)
        {
            return from a in context.Vehicles  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Vehicle GetSingle(EntityKeyFields entityKeys)
        {
            VehicleKeys keys = entityKeys as VehicleKeys;
            return (from a in context.Vehicles
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Vehicle entity)
        {
            onAdd();
            context.Vehicles.Add(entity);
        }

        public void Remove(Vehicle entity)
        {
            context.Vehicles.Attach(entity);
            context.Vehicles.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Vehicle entity)
        {
            onUpdate();
            context.Vehicles.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Vehicle> All()
        {
            return context.Vehicles.ToList();
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
	 