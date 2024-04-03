 
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
   public partial class UIMessageTenantRepository:IRepository<UIMessageTenant>
   {
   
        private ICustomContext currentContext;
        public UIMessageTenantRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public UIMessageTenantRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  UIMessageTenant GetSingle(string id, int tenant)
        {
            return (from a in context.UIMessageTenants
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<UIMessageTenant> GetAll(int tenant)
        {
            return from a in context.UIMessageTenants  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public UIMessageTenant GetSingle(EntityKeyFields entityKeys)
        {
            UIMessageTenantKeys keys = entityKeys as UIMessageTenantKeys;
            return (from a in context.UIMessageTenants
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(UIMessageTenant entity)
        {
            onAdd();
            context.UIMessageTenants.Add(entity);
        }

        public void Remove(UIMessageTenant entity)
        {
            context.UIMessageTenants.Attach(entity);
            context.UIMessageTenants.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(UIMessageTenant entity)
        {
            onUpdate();
            context.UIMessageTenants.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<UIMessageTenant> All()
        {
            return context.UIMessageTenants.ToList();
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
	 