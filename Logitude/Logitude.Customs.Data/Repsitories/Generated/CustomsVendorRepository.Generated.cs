 
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
   public partial class CustomsVendorRepository:IRepository<CustomsVendor>
   {
   
        private ICustomContext currentContext;
        public CustomsVendorRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsVendorRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsVendor GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsVendors
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsVendor> GetAll(int tenant)
        {
            return from a in context.CustomsVendors  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsVendor GetSingle(EntityKeyFields entityKeys)
        {
            CustomsVendorKeys keys = entityKeys as CustomsVendorKeys;
            return (from a in context.CustomsVendors
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsVendor entity)
        {
            onAdd();
            context.CustomsVendors.Add(entity);
        }

        public void Remove(CustomsVendor entity)
        {
            context.CustomsVendors.Attach(entity);
            context.CustomsVendors.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsVendor entity)
        {
            onUpdate();
            context.CustomsVendors.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsVendor> All()
        {
            return context.CustomsVendors.ToList();
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
	 