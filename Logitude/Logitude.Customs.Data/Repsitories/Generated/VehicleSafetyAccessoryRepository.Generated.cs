 
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
   public partial class VehicleSafetyAccessoryRepository:IRepository<VehicleSafetyAccessory>
   {
   
        private ICustomContext currentContext;
        public VehicleSafetyAccessoryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VehicleSafetyAccessoryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VehicleSafetyAccessory GetSingle(string vehicleid, int linenumber, int tenant)
        {
            return (from a in context.VehicleSafetyAccessories
                    where a.VehicleId == vehicleid && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<VehicleSafetyAccessory> GetAll(int tenant)
        {
            return from a in context.VehicleSafetyAccessories  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public VehicleSafetyAccessory GetSingle(EntityKeyFields entityKeys)
        {
            VehicleSafetyAccessoryKeys keys = entityKeys as VehicleSafetyAccessoryKeys;
            return (from a in context.VehicleSafetyAccessories
                    where a.VehicleId == keys.VehicleId && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VehicleSafetyAccessory entity)
        {
            onAdd();
            context.VehicleSafetyAccessories.Add(entity);
        }

        public void Remove(VehicleSafetyAccessory entity)
        {
            context.VehicleSafetyAccessories.Attach(entity);
            context.VehicleSafetyAccessories.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VehicleSafetyAccessory entity)
        {
            onUpdate();
            context.VehicleSafetyAccessories.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VehicleSafetyAccessory> All()
        {
            return context.VehicleSafetyAccessories.ToList();
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
	 