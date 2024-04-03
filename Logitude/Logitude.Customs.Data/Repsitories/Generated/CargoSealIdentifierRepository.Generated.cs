 
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
   public partial class CargoSealIdentifierRepository:IRepository<CargoSealIdentifier>
   {
   
        private ICustomContext currentContext;
        public CargoSealIdentifierRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CargoSealIdentifierRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoSealIdentifier GetSingle(string id, int tenant)
        {
            return (from a in context.CargoSealIdentifiers
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoSealIdentifier> GetAll(int tenant)
        {
            return from a in context.CargoSealIdentifiers  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CargoSealIdentifier GetSingle(EntityKeyFields entityKeys)
        {
            CargoSealIdentifierKeys keys = entityKeys as CargoSealIdentifierKeys;
            return (from a in context.CargoSealIdentifiers
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoSealIdentifier entity)
        {
            onAdd();
            context.CargoSealIdentifiers.Add(entity);
        }

        public void Remove(CargoSealIdentifier entity)
        {
            context.CargoSealIdentifiers.Attach(entity);
            context.CargoSealIdentifiers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoSealIdentifier entity)
        {
            onUpdate();
            context.CargoSealIdentifiers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoSealIdentifier> All()
        {
            return context.CargoSealIdentifiers.ToList();
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
	 