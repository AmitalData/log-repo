 
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
   public partial class DashboardGlobalFilterQueryService: EntityQueryService<DashboardGlobalFilter,DashboardGlobalFilterKeys,DashboardGlobalFilterPM,DashboardPM,DashboardKeys>
   {
   
        DashboardGlobalFilterRepository repository;
		IDashboardContext  context;
        public DashboardGlobalFilterQueryService(int tenant)
        {
		    context = DashboardContext.GetContext(tenant);
            MainContext = context;
            repository = new DashboardGlobalFilterRepository(context);
            Repository = repository;
            mapping = new DashboardGlobalFilterDataMapping();
        }

        public DashboardGlobalFilterQueryService(DashboardGlobalFilterRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DashboardGlobalFilterDataMapping();
        }

        public DashboardGlobalFilterQueryService(IDashboardContext context)
        {
            this.repository = new DashboardGlobalFilterRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DashboardGlobalFilterDataMapping();
        }
		 
		public  DashboardGlobalFilterPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DashboardGlobalFilterKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DashboardGlobalFilter entityPOCO)
        {
            DashboardGlobalFilterKeys entityKeys = new DashboardGlobalFilterKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 