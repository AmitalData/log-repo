 
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
   public partial class OriginCriterionQueryService: EntityQueryService<OriginCriterion,OriginCriterionKeys,OriginCriterionPM,object,OriginCriterionKeys>
   {
   
        OriginCriterionRepository repository;
		ICustomContext  context;
        public OriginCriterionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new OriginCriterionRepository(context);
            Repository = repository;
            mapping = new OriginCriterionDataMapping();
        }

        public OriginCriterionQueryService(OriginCriterionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new OriginCriterionDataMapping();
        }

        public OriginCriterionQueryService(ICustomContext context)
        {
            this.repository = new OriginCriterionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new OriginCriterionDataMapping();
        }
		 
		public  OriginCriterionPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new OriginCriterionKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(OriginCriterion entityPOCO)
        {
            OriginCriterionKeys entityKeys = new OriginCriterionKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 