 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class ExternalFieldMappingQueryService: EntityQueryService<ExternalFieldMapping,ExternalFieldMappingKeys,ExternalFieldMappingPM,object,ExternalFieldMappingKeys>
   {
   
        ExternalFieldMappingRepository repository;
		ICustomContext  context;
        public ExternalFieldMappingQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ExternalFieldMappingRepository(context);
            Repository = repository;
            mapping = new ExternalFieldMappingDataMapping();
        }

        public ExternalFieldMappingQueryService(ExternalFieldMappingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ExternalFieldMappingDataMapping();
        }

        public ExternalFieldMappingQueryService(ICustomContext context)
        {
            this.repository = new ExternalFieldMappingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ExternalFieldMappingDataMapping();
        }
		 
		public  ExternalFieldMappingPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ExternalFieldMappingKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ExternalFieldMapping entityPOCO)
        {
            ExternalFieldMappingKeys entityKeys = new ExternalFieldMappingKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 