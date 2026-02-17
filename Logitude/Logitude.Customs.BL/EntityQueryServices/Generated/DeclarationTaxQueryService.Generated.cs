 
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
   public partial class DeclarationTaxQueryService: EntityQueryService<DeclarationTax,DeclarationTaxKeys,DeclarationTaxPM,DeclarationPM,DeclarationKeys>
   {
   
        DeclarationTaxRepository repository;
		ICustomContext  context;
        public DeclarationTaxQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationTaxRepository(context);
            Repository = repository;
            mapping = new DeclarationTaxDataMapping();
        }

        public DeclarationTaxQueryService(DeclarationTaxRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationTaxDataMapping();
        }

        public DeclarationTaxQueryService(ICustomContext context)
        {
            this.repository = new DeclarationTaxRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationTaxDataMapping();
        }
		 
		public  DeclarationTaxPM GetSingle(string declarationid, string taxtypecode,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationTaxKeys(){ DeclarationId = declarationid, TaxTypeCode = taxtypecode };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationTax entityPOCO)
        {
            DeclarationTaxKeys entityKeys = new DeclarationTaxKeys() { DeclarationId = entityPOCO.DeclarationId, TaxTypeCode = entityPOCO.TaxTypeCode,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 