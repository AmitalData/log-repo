 
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
   public partial class OpportunityProductLocationUpdateService:EntityUpdateService<OpportunityProductLocation,OpportunityProductLocationPM,OpportunityProductPM>
   {
   
        OpportunityProductLocationRepository entityRepository;
        public OpportunityProductLocationUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICRMContext  context = mainContext as CRMContext;
            context = context ??mainContext as ICRMContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new OpportunityProductLocationDataMapping();
            Repository = new OpportunityProductLocationRepository(context);
        }

       
        private ICRMContext currentContext;
        public OpportunityProductLocationUpdateService(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public OpportunityProductLocationUpdateService(ICRMContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(OpportunityProductLocationPM entityPM)
        {
            OpportunityProductLocationKeys entityKeys = new OpportunityProductLocationKeys() { OpportunityId = entityPM.OpportunityId, OpportunityProductTypeCode = entityPM.OpportunityProductTypeCode, LineNumber = entityPM.LineNumber };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(OpportunityProductLocationPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(OpportunityProductLocationPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 