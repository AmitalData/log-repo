 
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
   public partial class WidgetQueryService: EntityQueryService<Widget,WidgetKeys,WidgetPM,DashboardPM,DashboardKeys>
   {
   
        WidgetRepository repository;
		IDashboardContext  context;
        public WidgetQueryService(int tenant)
        {
		    context = DashboardContext.GetContext(tenant);
            MainContext = context;
            repository = new WidgetRepository(context);
            Repository = repository;
            mapping = new WidgetDataMapping();
        }

        public WidgetQueryService(WidgetRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WidgetDataMapping();
        }

        public WidgetQueryService(IDashboardContext context)
        {
            this.repository = new WidgetRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WidgetDataMapping();
        }
		 
		public  WidgetPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WidgetKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Widget entityPOCO)
        {
            WidgetKeys entityKeys = new WidgetKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 