 
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
   public partial class VehicleOwnerRepository:IRepository<VehicleOwner>
   {
   
        private ICustomContext currentContext;
        public VehicleOwnerRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VehicleOwnerRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VehicleOwner GetSingle(string vehicleid, int linenumber, int tenant)
        {
            return (from a in context.VehicleOwners
                    where a.VehicleId == vehicleid && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<VehicleOwner> GetAll(int tenant)
        {
            return from a in context.VehicleOwners  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public VehicleOwner GetSingle(EntityKeyFields entityKeys)
        {
            VehicleOwnerKeys keys = entityKeys as VehicleOwnerKeys;
            return (from a in context.VehicleOwners
                    where a.VehicleId == keys.VehicleId && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VehicleOwner entity)
        {
            onAdd();
            context.VehicleOwners.Add(entity);
        }

        public void Remove(VehicleOwner entity)
        {
            context.VehicleOwners.Attach(entity);
            context.VehicleOwners.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VehicleOwner entity)
        {
            onUpdate();
            context.VehicleOwners.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VehicleOwner> All()
        {
            return context.VehicleOwners.ToList();
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
	 