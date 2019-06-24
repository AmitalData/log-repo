 
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
   public partial class OccasionQueryService: EntityQueryService<Occasion,OccasionKeys,OccasionPM,object,OccasionKeys>
   {
   
        OccasionRepository repository;
		ICRMContext  context;
        public OccasionQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new OccasionRepository(context);
            Repository = repository;
            mapping = new OccasionDataMapping();
        }

        public OccasionQueryService(OccasionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OccasionDataMapping();
        }

        public OccasionQueryService(ICRMContext context)
        {
            this.repository = new OccasionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OccasionDataMapping();
        }
		 
		public  OccasionPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OccasionKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Occasion entityPOCO)
        {
            OccasionKeys entityKeys = new OccasionKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 