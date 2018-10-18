 
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
using Logitude.Customs.BL.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class DecConsAcceptanceQueryService: EntityQueryService<DecConsAcceptance,DecConsAcceptanceKeys,DecConsAcceptancePM,object,DecConsAcceptanceKeys>
   {
   
        DecConsAcceptanceRepository repository;
		ICustomContext  context;
        public DecConsAcceptanceQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new DecConsAcceptanceRepository(context);
            Repository = repository;
            mapping = new DecConsAcceptanceDataMapping();
        }

        public DecConsAcceptanceQueryService(DecConsAcceptanceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DecConsAcceptanceDataMapping();
        }

        public DecConsAcceptanceQueryService(ICustomContext context)
        {
            this.repository = new DecConsAcceptanceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DecConsAcceptanceDataMapping();
        }
		 
		public  DecConsAcceptancePM GetSingle(string declarationid, int consignmentnumber, int linenumber,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DecConsAcceptanceKeys(){ DeclarationId = declarationid, ConsignmentNumber = consignmentnumber, LineNumber = linenumber };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DecConsAcceptance entityPOCO)
        {
            DecConsAcceptanceKeys entityKeys = new DecConsAcceptanceKeys() { DeclarationId = entityPOCO.DeclarationId, ConsignmentNumber = entityPOCO.ConsignmentNumber, LineNumber = entityPOCO.LineNumber,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 