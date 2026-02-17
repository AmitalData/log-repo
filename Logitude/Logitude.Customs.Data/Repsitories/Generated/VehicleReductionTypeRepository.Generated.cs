 
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
   public partial class VehicleReductionTypeRepository:IRepository<VehicleReductionType>
   {
   
        private ICustomContext currentContext;
        public VehicleReductionTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VehicleReductionTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VehicleReductionType GetSingle(string code)
        {
            return (from a in context.VehicleReductionTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VehicleReductionType> GetAll()
        {
            return from a in context.VehicleReductionTypes  
                   select a;
        }
				 
        public VehicleReductionType GetSingle(EntityKeyFields entityKeys)
        {
            VehicleReductionTypeKeys keys = entityKeys as VehicleReductionTypeKeys;
            return (from a in context.VehicleReductionTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VehicleReductionType entity)
        {
            onAdd();
            context.VehicleReductionTypes.Add(entity);
        }

        public void Remove(VehicleReductionType entity)
        {
            context.VehicleReductionTypes.Attach(entity);
            context.VehicleReductionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VehicleReductionType entity)
        {
            onUpdate();
            context.VehicleReductionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VehicleReductionType> All()
        {
            return context.VehicleReductionTypes.ToList();
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
	 