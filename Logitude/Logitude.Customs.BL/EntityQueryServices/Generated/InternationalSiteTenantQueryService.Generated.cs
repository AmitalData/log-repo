 
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
   public partial class InternationalSiteTenantQueryService: EntityQueryService<InternationalSiteTenant,InternationalSiteTenantKeys,InternationalSiteTenantPM,object,InternationalSiteTenantKeys>
   {
   
        InternationalSiteTenantRepository repository;
		ICustomContext  context;
        public InternationalSiteTenantQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new InternationalSiteTenantRepository(context);
            Repository = repository;
            mapping = new InternationalSiteTenantDataMapping();
        }

        public InternationalSiteTenantQueryService(InternationalSiteTenantRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new InternationalSiteTenantDataMapping();
        }

        public InternationalSiteTenantQueryService(ICustomContext context)
        {
            this.repository = new InternationalSiteTenantRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new InternationalSiteTenantDataMapping();
        }
		 
		public  InternationalSiteTenantPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new InternationalSiteTenantKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(InternationalSiteTenant entityPOCO)
        {
            InternationalSiteTenantKeys entityKeys = new InternationalSiteTenantKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 