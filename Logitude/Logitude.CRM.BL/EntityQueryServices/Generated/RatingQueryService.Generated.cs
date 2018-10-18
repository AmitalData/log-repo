 
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
   public partial class RatingQueryService: EntityQueryService<Rating,RatingKeys,RatingPM,object,RatingKeys>
   {
   
        RatingRepository repository;
		ICRMContext  context;
        public RatingQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new RatingRepository(context);
            Repository = repository;
            mapping = new RatingDataMapping();
        }

        public RatingQueryService(RatingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new RatingDataMapping();
        }

        public RatingQueryService(ICRMContext context)
        {
            this.repository = new RatingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new RatingDataMapping();
        }
		 
		public  RatingPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new RatingKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Rating entityPOCO)
        {
            RatingKeys entityKeys = new RatingKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 