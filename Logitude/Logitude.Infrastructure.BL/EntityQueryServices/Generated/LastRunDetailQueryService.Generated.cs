 
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
   public partial class LastRunDetailQueryService: EntityQueryService<LastRunDetail,LastRunDetailKeys,LastRunDetailPM,object,LastRunDetailKeys>
   {
   
        LastRunDetailRepository repository;
		IInfrastructureContext  context;
        public LastRunDetailQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new LastRunDetailRepository(context);
            Repository = repository;
            mapping = new LastRunDetailDataMapping();
        }

        public LastRunDetailQueryService(LastRunDetailRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new LastRunDetailDataMapping();
        }

        public LastRunDetailQueryService(IInfrastructureContext context)
        {
            this.repository = new LastRunDetailRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new LastRunDetailDataMapping();
        }
		 
		public  LastRunDetailPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new LastRunDetailKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(LastRunDetail entityPOCO)
        {
            LastRunDetailKeys entityKeys = new LastRunDetailKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 