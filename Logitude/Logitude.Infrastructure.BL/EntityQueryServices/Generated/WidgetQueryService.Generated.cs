 
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
   public partial class WidgetQueryService: EntityQueryService<Widget,WidgetKeys,WidgetPM,DashboardPM,DashboardKeys>
   {
   
        WidgetRepository repository;
		IInfrastructureContext  context;
        public WidgetQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
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

        public WidgetQueryService(IInfrastructureContext context)
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
	 