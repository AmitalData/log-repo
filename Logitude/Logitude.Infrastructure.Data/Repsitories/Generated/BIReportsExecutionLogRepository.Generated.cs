 
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
   public partial class BIReportsExecutionLogRepository:IRepository<BIReportsExecutionLog>
   {
   
        private IInfrastructureContext currentContext;
        public BIReportsExecutionLogRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public BIReportsExecutionLogRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  BIReportsExecutionLog GetSingle(string id, int tenant)
        {
            return (from a in context.BIReportsExecutionLogs
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BIReportsExecutionLog> GetAll(int tenant)
        {
            return from a in context.BIReportsExecutionLogs  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BIReportsExecutionLog GetSingle(EntityKeyFields entityKeys)
        {
            BIReportsExecutionLogKeys keys = entityKeys as BIReportsExecutionLogKeys;
            return (from a in context.BIReportsExecutionLogs
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BIReportsExecutionLog entity)
        {
            onAdd();
            context.BIReportsExecutionLogs.Add(entity);
        }

        public void Remove(BIReportsExecutionLog entity)
        {
            context.BIReportsExecutionLogs.Attach(entity);
            context.BIReportsExecutionLogs.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BIReportsExecutionLog entity)
        {
            onUpdate();
            context.BIReportsExecutionLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BIReportsExecutionLog> All()
        {
            return context.BIReportsExecutionLogs.ToList();
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
	 