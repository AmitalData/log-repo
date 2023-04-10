 
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
   public partial class DashboardsUserSettingRepository:IRepository<DashboardsUserSetting>
   {
   
        private IDashboardContext currentContext;
        public DashboardsUserSettingRepository(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public DashboardsUserSettingRepository(IDashboardContext context)
        {
            currentContext = context;
        }

		 
		
		public  DashboardsUserSetting GetSingle(string id, int tenant)
        {
            return (from a in context.DashboardsUserSettings
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DashboardsUserSetting> GetAll(int tenant)
        {
            return from a in context.DashboardsUserSettings  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DashboardsUserSetting GetSingle(EntityKeyFields entityKeys)
        {
            DashboardsUserSettingKeys keys = entityKeys as DashboardsUserSettingKeys;
            return (from a in context.DashboardsUserSettings
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DashboardsUserSetting entity)
        {
            onAdd();
            context.DashboardsUserSettings.Add(entity);
        }

        public void Remove(DashboardsUserSetting entity)
        {
            context.DashboardsUserSettings.Attach(entity);
            context.DashboardsUserSettings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DashboardsUserSetting entity)
        {
            onUpdate();
            context.DashboardsUserSettings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DashboardsUserSetting> All()
        {
            return context.DashboardsUserSettings.ToList();
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
	 