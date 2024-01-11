 
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
   public partial class VehicleManufacturerRepository:IRepository<VehicleManufacturer>
   {
   
        private ICustomContext currentContext;
        public VehicleManufacturerRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VehicleManufacturerRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VehicleManufacturer GetSingle(string code)
        {
            return (from a in context.VehicleManufacturers
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VehicleManufacturer> GetAll()
        {
            return from a in context.VehicleManufacturers  
                   select a;
        }
				 
        public VehicleManufacturer GetSingle(EntityKeyFields entityKeys)
        {
            VehicleManufacturerKeys keys = entityKeys as VehicleManufacturerKeys;
            return (from a in context.VehicleManufacturers
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VehicleManufacturer entity)
        {
            onAdd();
            context.VehicleManufacturers.Add(entity);
        }

        public void Remove(VehicleManufacturer entity)
        {
            context.VehicleManufacturers.Attach(entity);
            context.VehicleManufacturers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VehicleManufacturer entity)
        {
            onUpdate();
            context.VehicleManufacturers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VehicleManufacturer> All()
        {
            return context.VehicleManufacturers.ToList();
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
	 