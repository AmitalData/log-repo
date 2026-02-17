 
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
   public partial class ImporterTypeForClaimQueryService: EntityQueryService<ImporterTypeForClaim,ImporterTypeForClaimKeys,ImporterTypeForClaimPM,object,ImporterTypeForClaimKeys>
   {
   
        ImporterTypeForClaimRepository repository;
		ICustomContext  context;
        public ImporterTypeForClaimQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ImporterTypeForClaimRepository(context);
            Repository = repository;
            mapping = new ImporterTypeForClaimDataMapping();
        }

        public ImporterTypeForClaimQueryService(ImporterTypeForClaimRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ImporterTypeForClaimDataMapping();
        }

        public ImporterTypeForClaimQueryService(ICustomContext context)
        {
            this.repository = new ImporterTypeForClaimRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ImporterTypeForClaimDataMapping();
        }
		 
		public  ImporterTypeForClaimPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ImporterTypeForClaimKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ImporterTypeForClaim entityPOCO)
        {
            ImporterTypeForClaimKeys entityKeys = new ImporterTypeForClaimKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 