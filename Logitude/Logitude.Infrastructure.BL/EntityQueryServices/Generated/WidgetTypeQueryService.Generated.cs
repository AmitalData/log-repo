 
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
   public partial class WidgetTypeQueryService: EntityQueryService<WidgetType,WidgetTypeKeys,WidgetTypePM,object,WidgetTypeKeys>
   {
   
        WidgetTypeRepository repository;
		IInfrastructureContext  context;
        public WidgetTypeQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new WidgetTypeRepository(context);
            Repository = repository;
            mapping = new WidgetTypeDataMapping();
        }

        public WidgetTypeQueryService(WidgetTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new WidgetTypeDataMapping();
        }

        public WidgetTypeQueryService(IInfrastructureContext context)
        {
            this.repository = new WidgetTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new WidgetTypeDataMapping();
        }
		 
		public  WidgetTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new WidgetTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(WidgetType entityPOCO)
        {
            WidgetTypeKeys entityKeys = new WidgetTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 