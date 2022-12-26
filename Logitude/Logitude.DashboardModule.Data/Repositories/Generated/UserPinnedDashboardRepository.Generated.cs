 
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
   public partial class UserPinnedDashboardRepository:IRepository<UserPinnedDashboard>
   {
   
        private IDashboardContext currentContext;
        public UserPinnedDashboardRepository(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public UserPinnedDashboardRepository(IDashboardContext context)
        {
            currentContext = context;
        }

		 
		
		public  UserPinnedDashboard GetSingle(string id, int tenant)
        {
            return (from a in context.UserPinnedDashboards
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<UserPinnedDashboard> GetAll(int tenant)
        {
            return from a in context.UserPinnedDashboards  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public UserPinnedDashboard GetSingle(EntityKeyFields entityKeys)
        {
            UserPinnedDashboardKeys keys = entityKeys as UserPinnedDashboardKeys;
            return (from a in context.UserPinnedDashboards
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(UserPinnedDashboard entity)
        {
            onAdd();
            context.UserPinnedDashboards.Add(entity);
        }

        public void Remove(UserPinnedDashboard entity)
        {
            context.UserPinnedDashboards.Attach(entity);
            context.UserPinnedDashboards.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(UserPinnedDashboard entity)
        {
            onUpdate();
            context.UserPinnedDashboards.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<UserPinnedDashboard> All()
        {
            return context.UserPinnedDashboards.ToList();
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
	 