 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class AuditLogRepository:IRepository<AuditLog>
   {
   
        private IInfrastructureContext currentContext;
        public AuditLogRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public AuditLogRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  AuditLog GetSingle(string id, int tenant)
        {
            return (from a in context.AuditLogs
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AuditLog> GetAll(int tenant)
        {
            return from a in context.AuditLogs  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public AuditLog GetSingle(EntityKeyFields entityKeys)
        {
            AuditLogKeys keys = entityKeys as AuditLogKeys;
            return (from a in context.AuditLogs
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AuditLog entity)
        {
            onAdd();
            context.AuditLogs.Add(entity);
        }

        public void Remove(AuditLog entity)
        {
            context.AuditLogs.Attach(entity);
            context.AuditLogs.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AuditLog entity)
        {
            onUpdate();
            context.AuditLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AuditLog> All()
        {
            return context.AuditLogs.ToList();
        }

        private IInfrastructureContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 