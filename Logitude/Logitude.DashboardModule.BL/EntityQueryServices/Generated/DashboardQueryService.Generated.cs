 
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
   public partial class DashboardQueryService: EntityQueryService<Dashboard,DashboardKeys,DashboardPM,object,DashboardKeys>
   {
   
        DashboardRepository repository;
		IDashboardContext  context;
        public DashboardQueryService(int tenant)
        {
		    context = DashboardContext.GetContext(tenant);
            MainContext = context;
            repository = new DashboardRepository(context);
            Repository = repository;
            mapping = new DashboardDataMapping();
        }

        public DashboardQueryService(DashboardRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DashboardDataMapping();
        }

        public DashboardQueryService(IDashboardContext context)
        {
            this.repository = new DashboardRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DashboardDataMapping();
        }
		 
		public  DashboardPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DashboardKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Dashboard entityPOCO)
        {
            DashboardKeys entityKeys = new DashboardKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 