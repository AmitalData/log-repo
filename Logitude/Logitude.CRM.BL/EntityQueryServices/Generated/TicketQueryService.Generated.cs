 
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
   public partial class TicketQueryService: EntityQueryService<Ticket,TicketKeys,TicketPM,object,TicketKeys>
   {
   
        TicketRepository repository;
		ICRMContext  context;
        public TicketQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new TicketRepository(context);
            Repository = repository;
            mapping = new TicketDataMapping();
        }

        public TicketQueryService(TicketRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TicketDataMapping();
        }

        public TicketQueryService(ICRMContext context)
        {
            this.repository = new TicketRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TicketDataMapping();
        }
		 
		public  TicketPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TicketKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Ticket entityPOCO)
        {
            TicketKeys entityKeys = new TicketKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 