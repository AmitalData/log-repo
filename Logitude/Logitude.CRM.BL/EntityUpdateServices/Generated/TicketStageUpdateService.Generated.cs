 
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
   public partial class TicketStageUpdateService:EntityUpdateService<TicketStage,TicketStagePM,EntityPM>
   {
   
        TicketStageRepository entityRepository;
        public TicketStageUpdateService(IContext mainContext,Dictionary<string,IContext> additionalContexts, int tenant)
            : base(mainContext,additionalContexts, tenant)
        {
            ICRMContext  context = mainContext as CRMContext;
            context = context ??mainContext as ICRMContext ; //Up line is A BUG -and i need it 4 Fakes
            Mapping = new TicketStageDataMapping();
            Repository = new TicketStageRepository(context);
        }

       
        private ICRMContext currentContext;
        public TicketStageUpdateService(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public TicketStageUpdateService(ICRMContext context)
        {
            currentContext = context;
        }

		
		protected override EntityKeyFields GetKeys(TicketStagePM entityPM)
        {
            TicketStageKeys entityKeys = new TicketStageKeys() { Id = entityPM.Id };
            return entityKeys;
        }

		
	    protected override void FillDefaultValuesOnCreate(TicketStagePM entityPM)
        {
 
		}
		protected override void FillDefaultValuesOnUpdate(TicketStagePM entityPM)
		{
 
		}
		
		 
	 
   }
   
}
	 