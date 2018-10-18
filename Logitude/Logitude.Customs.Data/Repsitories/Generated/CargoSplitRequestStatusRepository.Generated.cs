 
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
   public partial class CargoSplitRequestStatusRepository:IRepository<CargoSplitRequestStatus>
   {
   
        private ICustomContext currentContext;
        public CargoSplitRequestStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CargoSplitRequestStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CargoSplitRequestStatus GetSingle(string code)
        {
            return (from a in context.CargoSplitRequestStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CargoSplitRequestStatus> GetAll()
        {
            return from a in context.CargoSplitRequestStatuses  
                   select a;
        }
				 
        public CargoSplitRequestStatus GetSingle(EntityKeyFields entityKeys)
        {
            CargoSplitRequestStatusKeys keys = entityKeys as CargoSplitRequestStatusKeys;
            return (from a in context.CargoSplitRequestStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CargoSplitRequestStatus entity)
        {
            onAdd();
            context.CargoSplitRequestStatuses.Add(entity);
        }

        public void Remove(CargoSplitRequestStatus entity)
        {
            context.CargoSplitRequestStatuses.Attach(entity);
            context.CargoSplitRequestStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CargoSplitRequestStatus entity)
        {
            onUpdate();
            context.CargoSplitRequestStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CargoSplitRequestStatus> All()
        {
            return context.CargoSplitRequestStatuses.ToList();
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
	 