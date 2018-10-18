 
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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityDataMappings;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data;

namespace Logitude.CRM.BL.EntityUpdateServices
{ 
   public partial class OpportunityAdditionalServiceUpdateService:EntityUpdateService<OpportunityAdditionalService,OpportunityAdditionalServicePM,OpportunityPM>
   {
   
        OpportunityAdditionalServiceRepository entityRepository;
        public OpportunityAdditionalServiceUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICRMContext  context = mainContext as CRMContext;
            context = context ??mainContext as ICRMContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new OpportunityAdditionalServiceDataMapping();
            Repository = new OpportunityAdditionalServiceRepository(context);
        }

       
        private ICRMContext currentContext;
        public OpportunityAdditionalServiceUpdateService(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OpportunityAdditionalServiceUpdateService(ICRMContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(OpportunityAdditionalServicePM entityPM)
        {
            OpportunityAdditionalServiceKeys entityKeys = new OpportunityAdditionalServiceKeys() { OpportunityId = entityPM.OpportunityId, AdditionalServiceId = entityPM.AdditionalServiceId };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(OpportunityAdditionalServicePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(OpportunityAdditionalServicePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 