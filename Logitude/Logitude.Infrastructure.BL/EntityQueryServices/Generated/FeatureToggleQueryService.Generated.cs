 
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
   public partial class FeatureToggleQueryService: EntityQueryService<FeatureToggle,FeatureToggleKeys,FeatureTogglePM,object,FeatureToggleKeys>
   {
   
        FeatureToggleRepository repository;
		IInfrastructureContext  context;
        public FeatureToggleQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new FeatureToggleRepository(context);
            Repository = repository;
            mapping = new FeatureToggleDataMapping();
        }

        public FeatureToggleQueryService(FeatureToggleRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new FeatureToggleDataMapping();
        }

        public FeatureToggleQueryService(IInfrastructureContext context)
        {
            this.repository = new FeatureToggleRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new FeatureToggleDataMapping();
        }
		 
		public  FeatureTogglePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new FeatureToggleKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(FeatureToggle entityPOCO)
        {
            FeatureToggleKeys entityKeys = new FeatureToggleKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 