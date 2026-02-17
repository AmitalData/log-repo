 
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
   public partial class ImporterPeriodicDeclarStatusQueryService: EntityQueryService<ImporterPeriodicDeclarStatus,ImporterPeriodicDeclarStatusKeys,ImporterPeriodicDeclarStatusPM,object,ImporterPeriodicDeclarStatusKeys>
   {
   
        ImporterPeriodicDeclarStatusRepository repository;
		ICustomContext  context;
        public ImporterPeriodicDeclarStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ImporterPeriodicDeclarStatusRepository(context);
            Repository = repository;
            mapping = new ImporterPeriodicDeclarStatusDataMapping();
        }

        public ImporterPeriodicDeclarStatusQueryService(ImporterPeriodicDeclarStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ImporterPeriodicDeclarStatusDataMapping();
        }

        public ImporterPeriodicDeclarStatusQueryService(ICustomContext context)
        {
            this.repository = new ImporterPeriodicDeclarStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ImporterPeriodicDeclarStatusDataMapping();
        }
		 
		public  ImporterPeriodicDeclarStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ImporterPeriodicDeclarStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ImporterPeriodicDeclarStatus entityPOCO)
        {
            ImporterPeriodicDeclarStatusKeys entityKeys = new ImporterPeriodicDeclarStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 