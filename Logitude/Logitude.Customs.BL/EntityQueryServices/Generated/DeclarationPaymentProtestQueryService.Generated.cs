 
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
   public partial class DeclarationPaymentProtestQueryService: EntityQueryService<DeclarationPaymentProtest,DeclarationPaymentProtestKeys,DeclarationPaymentProtestPM,DeclarationPaymentPM,DeclarationPaymentKeys>
   {
   
        DeclarationPaymentProtestRepository repository;
		ICustomContext  context;
        public DeclarationPaymentProtestQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationPaymentProtestRepository(context);
            Repository = repository;
            mapping = new DeclarationPaymentProtestDataMapping();
        }

        public DeclarationPaymentProtestQueryService(DeclarationPaymentProtestRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationPaymentProtestDataMapping();
        }

        public DeclarationPaymentProtestQueryService(ICustomContext context)
        {
            this.repository = new DeclarationPaymentProtestRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationPaymentProtestDataMapping();
        }
		 
		public  DeclarationPaymentProtestPM GetSingle(string declarationid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationPaymentProtestKeys(){ DeclarationId = declarationid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationPaymentProtest entityPOCO)
        {
            DeclarationPaymentProtestKeys entityKeys = new DeclarationPaymentProtestKeys() { DeclarationId = entityPOCO.DeclarationId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 