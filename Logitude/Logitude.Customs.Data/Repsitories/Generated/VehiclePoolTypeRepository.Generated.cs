 
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
   public partial class VehiclePoolTypeRepository:IRepository<VehiclePoolType>
   {
   
        private ICustomContext currentContext;
        public VehiclePoolTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VehiclePoolTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VehiclePoolType GetSingle(string code)
        {
            return (from a in context.VehiclePoolTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VehiclePoolType> GetAll()
        {
            return from a in context.VehiclePoolTypes  
                   select a;
        }
				 
        public VehiclePoolType GetSingle(EntityKeyFields entityKeys)
        {
            VehiclePoolTypeKeys keys = entityKeys as VehiclePoolTypeKeys;
            return (from a in context.VehiclePoolTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VehiclePoolType entity)
        {
            onAdd();
            context.VehiclePoolTypes.Add(entity);
        }

        public void Remove(VehiclePoolType entity)
        {
            context.VehiclePoolTypes.Attach(entity);
            context.VehiclePoolTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VehiclePoolType entity)
        {
            onUpdate();
            context.VehiclePoolTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VehiclePoolType> All()
        {
            return context.VehiclePoolTypes.ToList();
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
	 