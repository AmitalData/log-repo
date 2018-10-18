 
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
   public partial class TicketClassificationQueryService: EntityQueryService<TicketClassification,TicketClassificationKeys,TicketClassificationPM,object,TicketClassificationKeys>
   {
   
        TicketClassificationRepository repository;
		ICRMContext  context;
        public TicketClassificationQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new TicketClassificationRepository(context);
            Repository = repository;
            mapping = new TicketClassificationDataMapping();
        }

        public TicketClassificationQueryService(TicketClassificationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TicketClassificationDataMapping();
        }

        public TicketClassificationQueryService(ICRMContext context)
        {
            this.repository = new TicketClassificationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TicketClassificationDataMapping();
        }
		 
		public  TicketClassificationPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TicketClassificationKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TicketClassification entityPOCO)
        {
            TicketClassificationKeys entityKeys = new TicketClassificationKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 