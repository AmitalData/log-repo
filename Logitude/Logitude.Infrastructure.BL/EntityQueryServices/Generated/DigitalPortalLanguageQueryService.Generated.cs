 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityDataMappings;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Infrastructure.BL.EntityQueryServices
{ 
   public partial class DigitalPortalLanguageQueryService: EntityQueryService<DigitalPortalLanguage,DigitalPortalLanguageKeys,DigitalPortalLanguagePM,object,DigitalPortalLanguageKeys>
   {
   
        DigitalPortalLanguageRepository repository;
		IInfrastructureContext  context;
        public DigitalPortalLanguageQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new DigitalPortalLanguageRepository(context);
            Repository = repository;
            mapping = new DigitalPortalLanguageDataMapping();
        }

        public DigitalPortalLanguageQueryService(DigitalPortalLanguageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DigitalPortalLanguageDataMapping();
        }

        public DigitalPortalLanguageQueryService(IInfrastructureContext context)
        {
            this.repository = new DigitalPortalLanguageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DigitalPortalLanguageDataMapping();
        }
		 
		public  DigitalPortalLanguagePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DigitalPortalLanguageKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DigitalPortalLanguage entityPOCO)
        {
            DigitalPortalLanguageKeys entityKeys = new DigitalPortalLanguageKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 