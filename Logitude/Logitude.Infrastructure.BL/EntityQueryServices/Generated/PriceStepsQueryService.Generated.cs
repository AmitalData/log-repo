 
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
   public partial class PriceStepsQueryService: EntityQueryService<PriceSteps,PriceStepsKeys,PriceStepsPM,object,PriceStepsKeys>
   {
   
        PriceStepsRepository repository;
		IInfrastructureContext  context;
        public PriceStepsQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new PriceStepsRepository(context);
            Repository = repository;
            mapping = new PriceStepsDataMapping();
        }

        public PriceStepsQueryService(PriceStepsRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PriceStepsDataMapping();
        }

        public PriceStepsQueryService(IInfrastructureContext context)
        {
            this.repository = new PriceStepsRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PriceStepsDataMapping();
        }
		 
		public  PriceStepsPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PriceStepsKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PriceSteps entityPOCO)
        {
            PriceStepsKeys entityKeys = new PriceStepsKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 