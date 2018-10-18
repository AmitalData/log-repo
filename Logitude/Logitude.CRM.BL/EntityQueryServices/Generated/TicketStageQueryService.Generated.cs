 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityDataMappings;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityKeys;
using Logitude.CRM.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.CRM.BL.EntityQueryServices
{ 
   public partial class TicketStageQueryService: EntityQueryService<TicketStage,TicketStageKeys,TicketStagePM,object,TicketStageKeys>
   {
   
        TicketStageRepository repository;
		ICRMContext  context;
        public TicketStageQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new TicketStageRepository(context);
            Repository = repository;
            mapping = new TicketStageDataMapping();
        }

        public TicketStageQueryService(TicketStageRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TicketStageDataMapping();
        }

        public TicketStageQueryService(ICRMContext context)
        {
            this.repository = new TicketStageRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TicketStageDataMapping();
        }
		 
		public  TicketStagePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TicketStageKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TicketStage entityPOCO)
        {
            TicketStageKeys entityKeys = new TicketStageKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 