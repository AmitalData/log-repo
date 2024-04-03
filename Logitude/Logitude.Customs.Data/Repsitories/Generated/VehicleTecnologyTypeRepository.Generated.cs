 
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
   public partial class VehicleTecnologyTypeRepository:IRepository<VehicleTecnologyType>
   {
   
        private ICustomContext currentContext;
        public VehicleTecnologyTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VehicleTecnologyTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VehicleTecnologyType GetSingle(string code)
        {
            return (from a in context.VehicleTecnologyTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VehicleTecnologyType> GetAll()
        {
            return from a in context.VehicleTecnologyTypes  
                   select a;
        }
				 
        public VehicleTecnologyType GetSingle(EntityKeyFields entityKeys)
        {
            VehicleTecnologyTypeKeys keys = entityKeys as VehicleTecnologyTypeKeys;
            return (from a in context.VehicleTecnologyTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VehicleTecnologyType entity)
        {
            onAdd();
            context.VehicleTecnologyTypes.Add(entity);
        }

        public void Remove(VehicleTecnologyType entity)
        {
            context.VehicleTecnologyTypes.Attach(entity);
            context.VehicleTecnologyTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VehicleTecnologyType entity)
        {
            onUpdate();
            context.VehicleTecnologyTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VehicleTecnologyType> All()
        {
            return context.VehicleTecnologyTypes.ToList();
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
	 