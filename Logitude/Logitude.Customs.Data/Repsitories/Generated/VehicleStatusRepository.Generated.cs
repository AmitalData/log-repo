 
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
   public partial class VehicleStatusRepository:IRepository<VehicleStatus>
   {
   
        private ICustomContext currentContext;
        public VehicleStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VehicleStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VehicleStatus GetSingle(string code)
        {
            return (from a in context.VehicleStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VehicleStatus> GetAll()
        {
            return from a in context.VehicleStatuses  
                   select a;
        }
				 
        public VehicleStatus GetSingle(EntityKeyFields entityKeys)
        {
            VehicleStatusKeys keys = entityKeys as VehicleStatusKeys;
            return (from a in context.VehicleStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VehicleStatus entity)
        {
            onAdd();
            context.VehicleStatuses.Add(entity);
        }

        public void Remove(VehicleStatus entity)
        {
            context.VehicleStatuses.Attach(entity);
            context.VehicleStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VehicleStatus entity)
        {
            onUpdate();
            context.VehicleStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VehicleStatus> All()
        {
            return context.VehicleStatuses.ToList();
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
	 