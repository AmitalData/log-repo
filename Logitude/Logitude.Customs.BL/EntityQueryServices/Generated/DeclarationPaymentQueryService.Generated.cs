 
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
   public partial class DeclarationPaymentQueryService: EntityQueryService<DeclarationPayment,DeclarationPaymentKeys,DeclarationPaymentPM,DeclarationPM,DeclarationKeys>
   {
   
        DeclarationPaymentRepository repository;
		ICustomContext  context;
        public DeclarationPaymentQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationPaymentRepository(context);
            Repository = repository;
            mapping = new DeclarationPaymentDataMapping();
        }

        public DeclarationPaymentQueryService(DeclarationPaymentRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationPaymentDataMapping();
        }

        public DeclarationPaymentQueryService(ICustomContext context)
        {
            this.repository = new DeclarationPaymentRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationPaymentDataMapping();
        }
		 
		public  DeclarationPaymentPM GetSingle(string declarationid,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationPaymentKeys(){ DeclarationId = declarationid };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationPayment entityPOCO)
        {
            DeclarationPaymentKeys entityKeys = new DeclarationPaymentKeys() { DeclarationId = entityPOCO.DeclarationId,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 