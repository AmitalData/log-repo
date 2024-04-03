 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Simplog.Data.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Web;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityDataMappings;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data;

namespace Logitude.Infrastructure.BL.EntityUpdateServices
{ 
   public partial class DigitalPortalLanguageUpdateService:EntityUpdateService<DigitalPortalLanguage,DigitalPortalLanguagePM,EntityPM>
   {
   
        DigitalPortalLanguageRepository entityRepository;
        public DigitalPortalLanguageUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            IInfrastructureContext  context = mainContext as InfrastructureContext;
            context = context ??mainContext as IInfrastructureContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new DigitalPortalLanguageDataMapping();
            Repository = new DigitalPortalLanguageRepository(context);
        }

       
        private IInfrastructureContext currentContext;
        public DigitalPortalLanguageUpdateService(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public DigitalPortalLanguageUpdateService(IInfrastructureContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(DigitalPortalLanguagePM entityPM)
        {
            DigitalPortalLanguageKeys entityKeys = new DigitalPortalLanguageKeys() { Code = entityPM.Code };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(DigitalPortalLanguagePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(DigitalPortalLanguagePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 