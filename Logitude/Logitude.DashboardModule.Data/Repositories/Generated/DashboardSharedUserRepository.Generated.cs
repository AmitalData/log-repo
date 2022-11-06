 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.DashboardModule.Data.Repositories
{
   public partial class DashboardSharedUserRepository:IRepository<DashboardSharedUser>
   {
   
        private IDashboardContext currentContext;
        public DashboardSharedUserRepository(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public DashboardSharedUserRepository(IDashboardContext context)
        {
            currentContext = context;
        }

		 
		
		public  DashboardSharedUser GetSingle(string id, int tenant)
        {
            return (from a in context.DashboardSharedUsers
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DashboardSharedUser> GetAll(int tenant)
        {
            return from a in context.DashboardSharedUsers  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DashboardSharedUser GetSingle(EntityKeyFields entityKeys)
        {
            DashboardSharedUserKeys keys = entityKeys as DashboardSharedUserKeys;
            return (from a in context.DashboardSharedUsers
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DashboardSharedUser entity)
        {
            onAdd();
            context.DashboardSharedUsers.Add(entity);
        }

        public void Remove(DashboardSharedUser entity)
        {
            context.DashboardSharedUsers.Attach(entity);
            context.DashboardSharedUsers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DashboardSharedUser entity)
        {
            onUpdate();
            context.DashboardSharedUsers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DashboardSharedUser> All()
        {
            return context.DashboardSharedUsers.ToList();
        }

        private IDashboardContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 