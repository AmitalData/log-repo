 
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
   public partial class TicketSeverityQueryService: EntityQueryService<TicketSeverity,TicketSeverityKeys,TicketSeverityPM,object,TicketSeverityKeys>
   {
   
        TicketSeverityRepository repository;
		ICRMContext  context;
        public TicketSeverityQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new TicketSeverityRepository(context);
            Repository = repository;
            mapping = new TicketSeverityDataMapping();
        }

        public TicketSeverityQueryService(TicketSeverityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TicketSeverityDataMapping();
        }

        public TicketSeverityQueryService(ICRMContext context)
        {
            this.repository = new TicketSeverityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TicketSeverityDataMapping();
        }
		 
		public  TicketSeverityPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TicketSeverityKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TicketSeverity entityPOCO)
        {
            TicketSeverityKeys entityKeys = new TicketSeverityKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 