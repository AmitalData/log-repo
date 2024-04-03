 
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
   public partial class InternationalSiteTenantRepository:IRepository<InternationalSiteTenant>
   {
   
        private ICustomContext currentContext;
        public InternationalSiteTenantRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public InternationalSiteTenantRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  InternationalSiteTenant GetSingle(string id, int tenant)
        {
            return (from a in context.InternationalSiteTenants
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<InternationalSiteTenant> GetAll(int tenant)
        {
            return from a in context.InternationalSiteTenants  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public InternationalSiteTenant GetSingle(EntityKeyFields entityKeys)
        {
            InternationalSiteTenantKeys keys = entityKeys as InternationalSiteTenantKeys;
            return (from a in context.InternationalSiteTenants
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InternationalSiteTenant entity)
        {
            onAdd();
            context.InternationalSiteTenants.Add(entity);
        }

        public void Remove(InternationalSiteTenant entity)
        {
            context.InternationalSiteTenants.Attach(entity);
            context.InternationalSiteTenants.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InternationalSiteTenant entity)
        {
            onUpdate();
            context.InternationalSiteTenants.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InternationalSiteTenant> All()
        {
            return context.InternationalSiteTenants.ToList();
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
	 