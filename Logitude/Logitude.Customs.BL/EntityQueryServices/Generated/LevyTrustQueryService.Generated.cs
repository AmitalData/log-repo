 
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
   public partial class LevyTrustQueryService: EntityQueryService<LevyTrust,LevyTrustKeys,LevyTrustPM,object,LevyTrustKeys>
   {
   
        LevyTrustRepository repository;
		ICustomContext  context;
        public LevyTrustQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new LevyTrustRepository(context);
            Repository = repository;
            mapping = new LevyTrustDataMapping();
        }

        public LevyTrustQueryService(LevyTrustRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new LevyTrustDataMapping();
        }

        public LevyTrustQueryService(ICustomContext context)
        {
            this.repository = new LevyTrustRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new LevyTrustDataMapping();
        }
		 
		public  LevyTrustPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new LevyTrustKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(LevyTrust entityPOCO)
        {
            LevyTrustKeys entityKeys = new LevyTrustKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 