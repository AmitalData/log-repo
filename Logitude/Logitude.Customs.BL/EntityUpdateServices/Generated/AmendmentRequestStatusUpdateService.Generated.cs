 
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
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;

namespace Logitude.Customs.BL.EntityUpdateServices
{ 
   public partial class AmendmentRequestStatusUpdateService:EntityUpdateService<AmendmentRequestStatus,AmendmentRequestStatusPM,EntityPM>
   {
   
        AmendmentRequestStatusRepository entityRepository;
        public AmendmentRequestStatusUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICustomContext  context = mainContext as CustomContext;
            context = context ??mainContext as ICustomContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new AmendmentRequestStatusDataMapping();
            Repository = new AmendmentRequestStatusRepository(context);
        }

       
        private ICustomContext currentContext;
        public AmendmentRequestStatusUpdateService(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AmendmentRequestStatusUpdateService(ICustomContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(AmendmentRequestStatusPM entityPM)
        {
            AmendmentRequestStatusKeys entityKeys = new AmendmentRequestStatusKeys() { Code = entityPM.Code };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(AmendmentRequestStatusPM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(AmendmentRequestStatusPM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 