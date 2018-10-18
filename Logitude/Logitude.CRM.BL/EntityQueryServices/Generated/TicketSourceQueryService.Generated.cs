 
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
   public partial class TicketSourceQueryService: EntityQueryService<TicketSource,TicketSourceKeys,TicketSourcePM,object,TicketSourceKeys>
   {
   
        TicketSourceRepository repository;
		ICRMContext  context;
        public TicketSourceQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new TicketSourceRepository(context);
            Repository = repository;
            mapping = new TicketSourceDataMapping();
        }

        public TicketSourceQueryService(TicketSourceRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new TicketSourceDataMapping();
        }

        public TicketSourceQueryService(ICRMContext context)
        {
            this.repository = new TicketSourceRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new TicketSourceDataMapping();
        }
		 
		public  TicketSourcePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new TicketSourceKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(TicketSource entityPOCO)
        {
            TicketSourceKeys entityKeys = new TicketSourceKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 