 
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
   public partial class PermissionLevelQueryService: EntityQueryService<PermissionLevel,PermissionLevelKeys,PermissionLevelPM,object,PermissionLevelKeys>
   {
   
        PermissionLevelRepository repository;
		IDashboardContext  context;
        public PermissionLevelQueryService(int tenant)
        {
		    context = DashboardContext.GetContext(tenant);
            MainContext = context;
            repository = new PermissionLevelRepository(context);
            Repository = repository;
            mapping = new PermissionLevelDataMapping();
        }

        public PermissionLevelQueryService(PermissionLevelRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PermissionLevelDataMapping();
        }

        public PermissionLevelQueryService(IDashboardContext context)
        {
            this.repository = new PermissionLevelRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PermissionLevelDataMapping();
        }
		 
		public  PermissionLevelPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PermissionLevelKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PermissionLevel entityPOCO)
        {
            PermissionLevelKeys entityKeys = new PermissionLevelKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 