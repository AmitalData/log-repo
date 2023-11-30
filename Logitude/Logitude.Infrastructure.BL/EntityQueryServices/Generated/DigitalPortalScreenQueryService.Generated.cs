 
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
   public partial class DigitalPortalScreenQueryService: EntityQueryService<DigitalPortalScreen,DigitalPortalScreenKeys,DigitalPortalScreenPM,object,DigitalPortalScreenKeys>
   {
   
        DigitalPortalScreenRepository repository;
		IInfrastructureContext  context;
        public DigitalPortalScreenQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new DigitalPortalScreenRepository(context);
            Repository = repository;
            mapping = new DigitalPortalScreenDataMapping();
        }

        public DigitalPortalScreenQueryService(DigitalPortalScreenRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DigitalPortalScreenDataMapping();
        }

        public DigitalPortalScreenQueryService(IInfrastructureContext context)
        {
            this.repository = new DigitalPortalScreenRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DigitalPortalScreenDataMapping();
        }
		 
		public  DigitalPortalScreenPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DigitalPortalScreenKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DigitalPortalScreen entityPOCO)
        {
            DigitalPortalScreenKeys entityKeys = new DigitalPortalScreenKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 