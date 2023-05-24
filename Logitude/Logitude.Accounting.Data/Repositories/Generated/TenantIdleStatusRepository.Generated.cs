 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class TenantIdleStatusRepository:IRepository<TenantIdleStatus>
   {
   
        private IAccountingContext currentContext;
        public TenantIdleStatusRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public TenantIdleStatusRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  TenantIdleStatus GetSingle(string id, int tenant)
        {
            return (from a in context.TenantIdleStatuses
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TenantIdleStatus> GetAll(int tenant)
        {
            return from a in context.TenantIdleStatuses  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TenantIdleStatus GetSingle(EntityKeyFields entityKeys)
        {
            TenantIdleStatusKeys keys = entityKeys as TenantIdleStatusKeys;
            return (from a in context.TenantIdleStatuses
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TenantIdleStatus entity)
        {
            onAdd();
            context.TenantIdleStatuses.Add(entity);
        }

        public void Remove(TenantIdleStatus entity)
        {
            context.TenantIdleStatuses.Attach(entity);
            context.TenantIdleStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TenantIdleStatus entity)
        {
            onUpdate();
            context.TenantIdleStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TenantIdleStatus> All()
        {
            return context.TenantIdleStatuses.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 