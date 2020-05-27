 
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
   public partial class SupportMailboxQueryService: EntityQueryService<SupportMailbox,SupportMailboxKeys,SupportMailboxPM,object,SupportMailboxKeys>
   {
   
        SupportMailboxRepository repository;
		ICRMContext  context;
        public SupportMailboxQueryService(int tenant)
        {
		    context = CRMContext.GetContext(tenant);
            MainContext = context;
            repository = new SupportMailboxRepository(context);
            Repository = repository;
            mapping = new SupportMailboxDataMapping();
        }

        public SupportMailboxQueryService(SupportMailboxRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new SupportMailboxDataMapping();
        }

        public SupportMailboxQueryService(ICRMContext context)
        {
            this.repository = new SupportMailboxRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new SupportMailboxDataMapping();
        }
		 
		public  SupportMailboxPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new SupportMailboxKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(SupportMailbox entityPOCO)
        {
            SupportMailboxKeys entityKeys = new SupportMailboxKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 