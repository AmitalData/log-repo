 
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
   public partial class DashboardGlobalPresetFilterQueryService: EntityQueryService<DashboardGlobalPresetFilter,DashboardGlobalPresetFilterKeys,DashboardGlobalPresetFilterPM,object,DashboardGlobalPresetFilterKeys>
   {
   
        DashboardGlobalPresetFilterRepository repository;
		IDashboardContext  context;
        public DashboardGlobalPresetFilterQueryService(int tenant)
        {
		    context = DashboardContext.GetContext(tenant);
            MainContext = context;
            repository = new DashboardGlobalPresetFilterRepository(context);
            Repository = repository;
            mapping = new DashboardGlobalPresetFilterDataMapping();
        }

        public DashboardGlobalPresetFilterQueryService(DashboardGlobalPresetFilterRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DashboardGlobalPresetFilterDataMapping();
        }

        public DashboardGlobalPresetFilterQueryService(IDashboardContext context)
        {
            this.repository = new DashboardGlobalPresetFilterRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DashboardGlobalPresetFilterDataMapping();
        }
		 
		public  DashboardGlobalPresetFilterPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DashboardGlobalPresetFilterKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DashboardGlobalPresetFilter entityPOCO)
        {
            DashboardGlobalPresetFilterKeys entityKeys = new DashboardGlobalPresetFilterKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 