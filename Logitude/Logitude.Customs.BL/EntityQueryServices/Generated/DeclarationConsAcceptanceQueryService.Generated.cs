 
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
   public partial class DeclarationConsAcceptanceQueryService: EntityQueryService<DeclarationConsAcceptance,DeclarationConsAcceptanceKeys,DeclarationConsAcceptancePM,object,DeclarationConsAcceptanceKeys>
   {
   
        DeclarationConsAcceptanceRepository repository;
		ICustomContext  context;
        public DeclarationConsAcceptanceQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DeclarationConsAcceptanceRepository(context);
            Repository = repository;
            mapping = new DeclarationConsAcceptanceDataMapping();
        }

        public DeclarationConsAcceptanceQueryService(DeclarationConsAcceptanceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DeclarationConsAcceptanceDataMapping();
        }

        public DeclarationConsAcceptanceQueryService(ICustomContext context)
        {
            this.repository = new DeclarationConsAcceptanceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DeclarationConsAcceptanceDataMapping();
        }
		 
		public  DeclarationConsAcceptancePM GetSingle(string declarationid, int consignmentnumber, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DeclarationConsAcceptanceKeys(){ DeclarationId = declarationid, ConsignmentNumber = consignmentnumber, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DeclarationConsAcceptance entityPOCO)
        {
            DeclarationConsAcceptanceKeys entityKeys = new DeclarationConsAcceptanceKeys() { DeclarationId = entityPOCO.DeclarationId, ConsignmentNumber = entityPOCO.ConsignmentNumber, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 