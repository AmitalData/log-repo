 
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
   public partial class CustomsHouseTypeTenantRepository:IRepository<CustomsHouseTypeTenant>
   {
   
        private ICustomContext currentContext;
        public CustomsHouseTypeTenantRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsHouseTypeTenantRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsHouseTypeTenant GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsHouseTypeTenants
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsHouseTypeTenant> GetAll(int tenant)
        {
            return from a in context.CustomsHouseTypeTenants  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsHouseTypeTenant GetSingle(EntityKeyFields entityKeys)
        {
            CustomsHouseTypeTenantKeys keys = entityKeys as CustomsHouseTypeTenantKeys;
            return (from a in context.CustomsHouseTypeTenants
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsHouseTypeTenant entity)
        {
            onAdd();
            context.CustomsHouseTypeTenants.Add(entity);
        }

        public void Remove(CustomsHouseTypeTenant entity)
        {
            context.CustomsHouseTypeTenants.Attach(entity);
            context.CustomsHouseTypeTenants.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsHouseTypeTenant entity)
        {
            onUpdate();
            context.CustomsHouseTypeTenants.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsHouseTypeTenant> All()
        {
            return context.CustomsHouseTypeTenants.ToList();
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
	 