 
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
   public partial class CollateralsRequestFileCondQueryService: EntityQueryService<CollateralsRequestFileCond,CollateralsRequestFileCondKeys,CollateralsRequestFileCondPM,CustomsCollateralsAnswerPM,CustomsCollateralsAnswerKeys>
   {
   
        CollateralsRequestFileCondRepository repository;
		ICustomContext  context;
        public CollateralsRequestFileCondQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new CollateralsRequestFileCondRepository(context);
            Repository = repository;
            mapping = new CollateralsRequestFileCondDataMapping();
        }

        public CollateralsRequestFileCondQueryService(CollateralsRequestFileCondRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new CollateralsRequestFileCondDataMapping();
        }

        public CollateralsRequestFileCondQueryService(ICustomContext context)
        {
            this.repository = new CollateralsRequestFileCondRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new CollateralsRequestFileCondDataMapping();
        }
		 
		public  CollateralsRequestFileCondPM GetSingle(string customscollateralid, string conditioncode, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new CollateralsRequestFileCondKeys(){ CustomsCollateralId = customscollateralid, ConditionCode = conditioncode, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(CollateralsRequestFileCond entityPOCO)
        {
            CollateralsRequestFileCondKeys entityKeys = new CollateralsRequestFileCondKeys() { CustomsCollateralId = entityPOCO.CustomsCollateralId, ConditionCode = entityPOCO.ConditionCode, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 