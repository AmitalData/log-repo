 
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
   public partial class VehicleTypeRepository:IRepository<VehicleType>
   {
   
        private ICustomContext currentContext;
        public VehicleTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VehicleTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VehicleType GetSingle(string code)
        {
            return (from a in context.VehicleTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VehicleType> GetAll()
        {
            return from a in context.VehicleTypes  
                   select a;
        }
				 
        public VehicleType GetSingle(EntityKeyFields entityKeys)
        {
            VehicleTypeKeys keys = entityKeys as VehicleTypeKeys;
            return (from a in context.VehicleTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VehicleType entity)
        {
            onAdd();
            context.VehicleTypes.Add(entity);
        }

        public void Remove(VehicleType entity)
        {
            context.VehicleTypes.Attach(entity);
            context.VehicleTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VehicleType entity)
        {
            onUpdate();
            context.VehicleTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VehicleType> All()
        {
            return context.VehicleTypes.ToList();
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
	 