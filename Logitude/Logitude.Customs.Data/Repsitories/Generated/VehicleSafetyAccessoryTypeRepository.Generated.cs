 
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
   public partial class VehicleSafetyAccessoryTypeRepository:IRepository<VehicleSafetyAccessoryType>
   {
   
        private ICustomContext currentContext;
        public VehicleSafetyAccessoryTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VehicleSafetyAccessoryTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VehicleSafetyAccessoryType GetSingle(string code)
        {
            return (from a in context.VehicleSafetyAccessoryTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VehicleSafetyAccessoryType> GetAll()
        {
            return from a in context.VehicleSafetyAccessoryTypes  
                   select a;
        }
				 
        public VehicleSafetyAccessoryType GetSingle(EntityKeyFields entityKeys)
        {
            VehicleSafetyAccessoryTypeKeys keys = entityKeys as VehicleSafetyAccessoryTypeKeys;
            return (from a in context.VehicleSafetyAccessoryTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VehicleSafetyAccessoryType entity)
        {
            onAdd();
            context.VehicleSafetyAccessoryTypes.Add(entity);
        }

        public void Remove(VehicleSafetyAccessoryType entity)
        {
            context.VehicleSafetyAccessoryTypes.Attach(entity);
            context.VehicleSafetyAccessoryTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VehicleSafetyAccessoryType entity)
        {
            onUpdate();
            context.VehicleSafetyAccessoryTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VehicleSafetyAccessoryType> All()
        {
            return context.VehicleSafetyAccessoryTypes.ToList();
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
	 