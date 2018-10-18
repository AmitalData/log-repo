 
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
   public partial class TicketCreatedByTypeQueryService: EntityQueryService<TicketCreatedByType,TicketCreatedByTypeKeys,TicketCreatedByTypePM,object,TicketCreatedByTypeKeys>
   {
   
        TicketCreatedByTypeRepository repository;
		ICRMContext  context;
        public TicketCreatedByTypeQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new TicketCreatedByTypeRepository(context);
            Repository = repository;
            mapping = new TicketCreatedByTypeDataMapping();
        }

        public TicketCreatedByTypeQueryService(TicketCreatedByTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TicketCreatedByTypeDataMapping();
        }

        public TicketCreatedByTypeQueryService(ICRMContext context)
        {
            this.repository = new TicketCreatedByTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TicketCreatedByTypeDataMapping();
        }
		 
		public  TicketCreatedByTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TicketCreatedByTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TicketCreatedByType entityPOCO)
        {
            TicketCreatedByTypeKeys entityKeys = new TicketCreatedByTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 