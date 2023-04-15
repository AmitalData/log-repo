 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Accounting.BL.EntityQueryServices
{ 
   public partial class EventRemarkQueryService: EntityQueryService<EventRemark,EventRemarkKeys,EventRemarkPM,object,EventRemarkKeys>
   {
   
        EventRemarkRepository repository;
		IAccountingContext  context;
        public EventRemarkQueryService(int tenant)
        {
		    context = AccountingContext.GetContext(tenant);
            MainContext = context;
            repository = new EventRemarkRepository(context);
            Repository = repository;
            mapping = new EventRemarkDataMapping();
        }

        public EventRemarkQueryService(EventRemarkRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new EventRemarkDataMapping();
        }

        public EventRemarkQueryService(IAccountingContext context)
        {
            this.repository = new EventRemarkRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new EventRemarkDataMapping();
        }
		 
		public  EventRemarkPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new EventRemarkKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(EventRemark entityPOCO)
        {
            EventRemarkKeys entityKeys = new EventRemarkKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 