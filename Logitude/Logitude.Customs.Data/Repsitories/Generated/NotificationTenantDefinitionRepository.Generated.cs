 
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
   public partial class NotificationTenantDefinitionRepository:IRepository<NotificationTenantDefinition>
   {
   
        private ICustomContext currentContext;
        public NotificationTenantDefinitionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public NotificationTenantDefinitionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  NotificationTenantDefinition GetSingle(string id, int tenant)
        {
            return (from a in context.NotificationTenantDefinition
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<NotificationTenantDefinition> GetAll(int tenant)
        {
            return from a in context.NotificationTenantDefinition  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public NotificationTenantDefinition GetSingle(EntityKeyFields entityKeys)
        {
            NotificationTenantDefinitionKeys keys = entityKeys as NotificationTenantDefinitionKeys;
            return (from a in context.NotificationTenantDefinition
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(NotificationTenantDefinition entity)
        {
            onAdd();
            context.NotificationTenantDefinition.Add(entity);
        }

        public void Remove(NotificationTenantDefinition entity)
        {
            context.NotificationTenantDefinition.Attach(entity);
            context.NotificationTenantDefinition.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(NotificationTenantDefinition entity)
        {
            onUpdate();
            context.NotificationTenantDefinition.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<NotificationTenantDefinition> All()
        {
            return context.NotificationTenantDefinition.ToList();
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
	 