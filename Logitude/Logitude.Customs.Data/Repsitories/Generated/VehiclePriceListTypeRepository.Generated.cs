 
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
   public partial class VehiclePriceListTypeRepository:IRepository<VehiclePriceListType>
   {
   
        private ICustomContext currentContext;
        public VehiclePriceListTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VehiclePriceListTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VehiclePriceListType GetSingle(string code)
        {
            return (from a in context.VehiclePriceListType
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VehiclePriceListType> GetAll()
        {
            return from a in context.VehiclePriceListType  
                   select a;
        }
				 
        public VehiclePriceListType GetSingle(EntityKeyFields entityKeys)
        {
            VehiclePriceListTypeKeys keys = entityKeys as VehiclePriceListTypeKeys;
            return (from a in context.VehiclePriceListType
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VehiclePriceListType entity)
        {
            onAdd();
            context.VehiclePriceListType.Add(entity);
        }

        public void Remove(VehiclePriceListType entity)
        {
            context.VehiclePriceListType.Attach(entity);
            context.VehiclePriceListType.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VehiclePriceListType entity)
        {
            onUpdate();
            context.VehiclePriceListType.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VehiclePriceListType> All()
        {
            return context.VehiclePriceListType.ToList();
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
	 