 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Customs.BL.EntityQueryServices
{ 
   public partial class GovernmentProcTypeTenantQueryService: EntityQueryService<GovernmentProcTypeTenant,GovernmentProcTypeTenantKeys,GovernmentProcTypeTenantPM,object,GovernmentProcTypeTenantKeys>
   {
   
        GovernmentProcTypeTenantRepository repository;
		ICustomContext  context;
        public GovernmentProcTypeTenantQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new GovernmentProcTypeTenantRepository(context);
            Repository = repository;
            mapping = new GovernmentProcTypeTenantDataMapping();
        }

        public GovernmentProcTypeTenantQueryService(GovernmentProcTypeTenantRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new GovernmentProcTypeTenantDataMapping();
        }

        public GovernmentProcTypeTenantQueryService(ICustomContext context)
        {
            this.repository = new GovernmentProcTypeTenantRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new GovernmentProcTypeTenantDataMapping();
        }
		 
		public  GovernmentProcTypeTenantPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new GovernmentProcTypeTenantKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(GovernmentProcTypeTenant entityPOCO)
        {
            GovernmentProcTypeTenantKeys entityKeys = new GovernmentProcTypeTenantKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 