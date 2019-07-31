 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityDataMappings;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.CRM.BL.EntityQueryServices
{ 
   public partial class OccasionTypeQueryService: EntityQueryService<OccasionType,OccasionTypeKeys,OccasionTypePM,object,OccasionTypeKeys>
   {
   
        OccasionTypeRepository repository;
		ICRMContext  context;
        public OccasionTypeQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OccasionTypeRepository(context);
            Repository = repository;
            mapping = new OccasionTypeDataMapping();
        }

        public OccasionTypeQueryService(OccasionTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OccasionTypeDataMapping();
        }

        public OccasionTypeQueryService(ICRMContext context)
        {
            this.repository = new OccasionTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OccasionTypeDataMapping();
        }
		 
		public  OccasionTypePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OccasionTypeKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OccasionType entityPOCO)
        {
            OccasionTypeKeys entityKeys = new OccasionTypeKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 