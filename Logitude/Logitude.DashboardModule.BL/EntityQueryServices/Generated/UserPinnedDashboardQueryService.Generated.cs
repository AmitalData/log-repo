 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.BL.EntityDataMappings;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.DashboardModule.Data.EntityKeys;
using Logitude.DashboardModule.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.DashboardModule.BL.EntityQueryServices
{ 
   public partial class UserPinnedDashboardQueryService: EntityQueryService<UserPinnedDashboard,UserPinnedDashboardKeys,UserPinnedDashboardPM,object,UserPinnedDashboardKeys>
   {
   
        UserPinnedDashboardRepository repository;
		IDashboardContext  context;
        public UserPinnedDashboardQueryService(int tenant)
        {
		    context = DashboardContext.GetContext(tenant);
            MainContext = context;
            repository = new UserPinnedDashboardRepository(context);
            Repository = repository;
            mapping = new UserPinnedDashboardDataMapping();
        }

        public UserPinnedDashboardQueryService(UserPinnedDashboardRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new UserPinnedDashboardDataMapping();
        }

        public UserPinnedDashboardQueryService(IDashboardContext context)
        {
            this.repository = new UserPinnedDashboardRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new UserPinnedDashboardDataMapping();
        }
		 
		public  UserPinnedDashboardPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new UserPinnedDashboardKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(UserPinnedDashboard entityPOCO)
        {
            UserPinnedDashboardKeys entityKeys = new UserPinnedDashboardKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 