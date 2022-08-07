 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityDataMappings;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Infrastructure.BL.EntityQueryServices
{ 
   public partial class DashboardQueryService: EntityQueryService<Dashboard,DashboardKeys,DashboardPM,object,DashboardKeys>
   {
   
        DashboardRepository repository;
		IInfrastructureContext  context;
        public DashboardQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
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

        public DashboardQueryService(IInfrastructureContext context)
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
	 