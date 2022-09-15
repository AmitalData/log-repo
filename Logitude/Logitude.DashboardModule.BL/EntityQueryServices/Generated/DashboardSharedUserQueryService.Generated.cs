 
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
   public partial class DashboardSharedUserQueryService: EntityQueryService<DashboardSharedUser,DashboardSharedUserKeys,DashboardSharedUserPM,DashboardPM,DashboardKeys>
   {
   
        DashboardSharedUserRepository repository;
		IDashboardContext  context;
        public DashboardSharedUserQueryService(int tenant)
        {
		    context = DashboardContext.GetContext(tenant);
            MainContext = context;
            repository = new DashboardSharedUserRepository(context);
            Repository = repository;
            mapping = new DashboardSharedUserDataMapping();
        }

        public DashboardSharedUserQueryService(DashboardSharedUserRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DashboardSharedUserDataMapping();
        }

        public DashboardSharedUserQueryService(IDashboardContext context)
        {
            this.repository = new DashboardSharedUserRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DashboardSharedUserDataMapping();
        }
		 
		public  DashboardSharedUserPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DashboardSharedUserKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DashboardSharedUser entityPOCO)
        {
            DashboardSharedUserKeys entityKeys = new DashboardSharedUserKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 