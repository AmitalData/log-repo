 
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
   public partial class FuelTypeRepository:IRepository<FuelType>
   {
   
        private ICustomContext currentContext;
        public FuelTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public FuelTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  FuelType GetSingle(string code)
        {
            return (from a in context.FuelTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<FuelType> GetAll()
        {
            return from a in context.FuelTypes  
                   select a;
        }
				 
        public FuelType GetSingle(EntityKeyFields entityKeys)
        {
            FuelTypeKeys keys = entityKeys as FuelTypeKeys;
            return (from a in context.FuelTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FuelType entity)
        {
            onAdd();
            context.FuelTypes.Add(entity);
        }

        public void Remove(FuelType entity)
        {
            context.FuelTypes.Attach(entity);
            context.FuelTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FuelType entity)
        {
            onUpdate();
            context.FuelTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FuelType> All()
        {
            return context.FuelTypes.ToList();
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
	 