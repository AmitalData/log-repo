 
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
   public partial class PriceStepQueryService: EntityQueryService<PriceStep,PriceStepKeys,PriceStepPM,object,PriceStepKeys>
   {
   
        PriceStepRepository repository;
		IInfrastructureContext  context;
        public PriceStepQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new PriceStepRepository(context);
            Repository = repository;
            mapping = new PriceStepDataMapping();
        }

        public PriceStepQueryService(PriceStepRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new PriceStepDataMapping();
        }

        public PriceStepQueryService(IInfrastructureContext context)
        {
            this.repository = new PriceStepRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new PriceStepDataMapping();
        }
		 
		public  PriceStepPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new PriceStepKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(PriceStep entityPOCO)
        {
            PriceStepKeys entityKeys = new PriceStepKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 