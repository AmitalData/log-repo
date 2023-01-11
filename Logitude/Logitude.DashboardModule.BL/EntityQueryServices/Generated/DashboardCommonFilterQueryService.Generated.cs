 
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
   public partial class DashboardCommonFilterQueryService: EntityQueryService<DashboardCommonFilter,DashboardCommonFilterKeys,DashboardCommonFilterPM,object,DashboardCommonFilterKeys>
   {
   
        DashboardCommonFilterRepository repository;
		IDashboardContext  context;
        public DashboardCommonFilterQueryService(int tenant)
        {
		    context = DashboardContext.GetContext(tenant);
            MainContext = context;
            repository = new DashboardCommonFilterRepository(context);
            Repository = repository;
            mapping = new DashboardCommonFilterDataMapping();
        }

        public DashboardCommonFilterQueryService(DashboardCommonFilterRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DashboardCommonFilterDataMapping();
        }

        public DashboardCommonFilterQueryService(IDashboardContext context)
        {
            this.repository = new DashboardCommonFilterRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DashboardCommonFilterDataMapping();
        }
		 
		public  DashboardCommonFilterPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DashboardCommonFilterKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DashboardCommonFilter entityPOCO)
        {
            DashboardCommonFilterKeys entityKeys = new DashboardCommonFilterKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 