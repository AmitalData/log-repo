 
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
   public partial class AnalyticsFactsMetaDataQueryService: EntityQueryService<AnalyticsFactsMetaData,AnalyticsFactsMetaDataKeys,AnalyticsFactsMetaDataPM,object,AnalyticsFactsMetaDataKeys>
   {
   
        AnalyticsFactsMetaDataRepository repository;
		IDashboardContext  context;
        public AnalyticsFactsMetaDataQueryService(int tenant)
        {
		    context = DashboardContext.GetContext(tenant);
            MainContext = context;
            repository = new AnalyticsFactsMetaDataRepository(context);
            Repository = repository;
            mapping = new AnalyticsFactsMetaDataDataMapping();
        }

        public AnalyticsFactsMetaDataQueryService(AnalyticsFactsMetaDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AnalyticsFactsMetaDataDataMapping();
        }

        public AnalyticsFactsMetaDataQueryService(IDashboardContext context)
        {
            this.repository = new AnalyticsFactsMetaDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AnalyticsFactsMetaDataDataMapping();
        }
		 
		public  AnalyticsFactsMetaDataPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AnalyticsFactsMetaDataKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AnalyticsFactsMetaData entityPOCO)
        {
            AnalyticsFactsMetaDataKeys entityKeys = new AnalyticsFactsMetaDataKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 