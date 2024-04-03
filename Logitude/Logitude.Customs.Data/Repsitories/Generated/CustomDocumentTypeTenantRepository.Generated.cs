 
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
   public partial class CustomDocumentTypeTenantRepository:IRepository<CustomDocumentTypeTenant>
   {
   
        private ICustomContext currentContext;
        public CustomDocumentTypeTenantRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomDocumentTypeTenantRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomDocumentTypeTenant GetSingle(string id, int tenant)
        {
            return (from a in context.CustomDocumentTypeTenants
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomDocumentTypeTenant> GetAll(int tenant)
        {
            return from a in context.CustomDocumentTypeTenants  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomDocumentTypeTenant GetSingle(EntityKeyFields entityKeys)
        {
            CustomDocumentTypeTenantKeys keys = entityKeys as CustomDocumentTypeTenantKeys;
            return (from a in context.CustomDocumentTypeTenants
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomDocumentTypeTenant entity)
        {
            onAdd();
            context.CustomDocumentTypeTenants.Add(entity);
        }

        public void Remove(CustomDocumentTypeTenant entity)
        {
            context.CustomDocumentTypeTenants.Attach(entity);
            context.CustomDocumentTypeTenants.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomDocumentTypeTenant entity)
        {
            onUpdate();
            context.CustomDocumentTypeTenants.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomDocumentTypeTenant> All()
        {
            return context.CustomDocumentTypeTenants.ToList();
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
	 