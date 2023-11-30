 
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
   public partial class WidgetMeasureQueryService: EntityQueryService<WidgetMeasure,WidgetMeasureKeys,WidgetMeasurePM,WidgetPM,WidgetKeys>
   {
   
        WidgetMeasureRepository repository;
		IDashboardContext  context;
        public WidgetMeasureQueryService(int tenant)
        {
		    context = DashboardContext.GetContext(tenant);
            MainContext = context;
            repository = new WidgetMeasureRepository(context);
            Repository = repository;
            mapping = new WidgetMeasureDataMapping();
        }

        public WidgetMeasureQueryService(WidgetMeasureRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WidgetMeasureDataMapping();
        }

        public WidgetMeasureQueryService(IDashboardContext context)
        {
            this.repository = new WidgetMeasureRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WidgetMeasureDataMapping();
        }
		 
		public  WidgetMeasurePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WidgetMeasureKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WidgetMeasure entityPOCO)
        {
            WidgetMeasureKeys entityKeys = new WidgetMeasureKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 