 
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
   public partial class DeclarationReferantDataQueryService: EntityQueryService<DeclarationReferantData,DeclarationReferantDataKeys,DeclarationReferantDataPM,object,DeclarationReferantDataKeys>
   {
   
        DeclarationReferantDataRepository repository;
		ICustomContext  context;
        public DeclarationReferantDataQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationReferantDataRepository(context);
            Repository = repository;
            mapping = new DeclarationReferantDataDataMapping();
        }

        public DeclarationReferantDataQueryService(DeclarationReferantDataRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationReferantDataDataMapping();
        }

        public DeclarationReferantDataQueryService(ICustomContext context)
        {
            this.repository = new DeclarationReferantDataRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationReferantDataDataMapping();
        }
		 
		public  DeclarationReferantDataPM GetSingle(string declarationid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationReferantDataKeys(){ DeclarationId = declarationid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationReferantData entityPOCO)
        {
            DeclarationReferantDataKeys entityKeys = new DeclarationReferantDataKeys() { DeclarationId = entityPOCO.DeclarationId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 