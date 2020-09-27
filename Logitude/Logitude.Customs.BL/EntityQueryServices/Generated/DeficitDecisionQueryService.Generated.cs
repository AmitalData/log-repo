 
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
   public partial class DeficitDecisionQueryService: EntityQueryService<DeficitDecision,DeficitDecisionKeys,DeficitDecisionPM,DeficitPM,DeficitKeys>
   {
   
        DeficitDecisionRepository repository;
		ICustomContext  context;
        public DeficitDecisionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeficitDecisionRepository(context);
            Repository = repository;
            mapping = new DeficitDecisionDataMapping();
        }

        public DeficitDecisionQueryService(DeficitDecisionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeficitDecisionDataMapping();
        }

        public DeficitDecisionQueryService(ICustomContext context)
        {
            this.repository = new DeficitDecisionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeficitDecisionDataMapping();
        }
		 
		public  DeficitDecisionPM GetSingle(string deficitid, string declarationid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeficitDecisionKeys(){ DeficitId = deficitid, DeclarationId = declarationid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeficitDecision entityPOCO)
        {
            DeficitDecisionKeys entityKeys = new DeficitDecisionKeys() { DeficitId = entityPOCO.DeficitId, DeclarationId = entityPOCO.DeclarationId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 