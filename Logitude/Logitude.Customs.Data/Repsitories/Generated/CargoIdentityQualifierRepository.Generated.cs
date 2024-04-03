 
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
   public partial class CargoIdentityQualifierRepository:IRepository<CargoIdentityQualifier>
   {
   
        private ICustomContext currentContext;
        public CargoIdentityQualifierRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CargoIdentityQualifierRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoIdentityQualifier GetSingle(string code)
        {
            return (from a in context.CargoIdentityQualifiers
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoIdentityQualifier> GetAll()
        {
            return from a in context.CargoIdentityQualifiers  
                   select a;
        }
				 
        public CargoIdentityQualifier GetSingle(EntityKeyFields entityKeys)
        {
            CargoIdentityQualifierKeys keys = entityKeys as CargoIdentityQualifierKeys;
            return (from a in context.CargoIdentityQualifiers
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoIdentityQualifier entity)
        {
            onAdd();
            context.CargoIdentityQualifiers.Add(entity);
        }

        public void Remove(CargoIdentityQualifier entity)
        {
            context.CargoIdentityQualifiers.Attach(entity);
            context.CargoIdentityQualifiers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoIdentityQualifier entity)
        {
            onUpdate();
            context.CargoIdentityQualifiers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoIdentityQualifier> All()
        {
            return context.CargoIdentityQualifiers.ToList();
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
	 