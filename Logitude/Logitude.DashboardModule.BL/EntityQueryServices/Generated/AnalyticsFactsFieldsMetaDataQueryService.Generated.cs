 
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
   public partial class AnalyticsFactsFieldsMetaDataQueryService: EntityQueryService<AnalyticsFactsFieldsMetaData,AnalyticsFactsFieldsMetaDataKeys,AnalyticsFactsFieldsMetaDataPM,object,AnalyticsFactsFieldsMetaDataKeys>
   {
   
        AnalyticsFactsFieldsMetaDataRepository repository;
		IDashboardContext  context;
        public AnalyticsFactsFieldsMetaDataQueryService(int tenant)
        {
		    context = DashboardContext.GetContext(tenant);
            MainContext = context;
            repository = new AnalyticsFactsFieldsMetaDataRepository(context);
            Repository = repository;
            mapping = new AnalyticsFactsFieldsMetaDataDataMapping();
        }

        public AnalyticsFactsFieldsMetaDataQueryService(AnalyticsFactsFieldsMetaDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AnalyticsFactsFieldsMetaDataDataMapping();
        }

        public AnalyticsFactsFieldsMetaDataQueryService(IDashboardContext context)
        {
            this.repository = new AnalyticsFactsFieldsMetaDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AnalyticsFactsFieldsMetaDataDataMapping();
        }
		 
		public  AnalyticsFactsFieldsMetaDataPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AnalyticsFactsFieldsMetaDataKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AnalyticsFactsFieldsMetaData entityPOCO)
        {
            AnalyticsFactsFieldsMetaDataKeys entityKeys = new AnalyticsFactsFieldsMetaDataKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 