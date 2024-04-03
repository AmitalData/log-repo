 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityDataMappings;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityKeys;
using Logitude.Infrastructure.Data;
using Simplog.Server.Infrastructure;
namespace Logitude.Infrastructure.BL.EntityQueryServices
{ 
   public partial class DigitalFieldSecurityQueryService: EntityQueryService<DigitalFieldSecurity,DigitalFieldSecurityKeys,DigitalFieldSecurityPM,object,DigitalFieldSecurityKeys>
   {
   
        DigitalFieldSecurityRepository repository;
		IInfrastructureContext  context;
        public DigitalFieldSecurityQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new DigitalFieldSecurityRepository(context);
            Repository = repository;
            mapping = new DigitalFieldSecurityDataMapping();
        }

        public DigitalFieldSecurityQueryService(DigitalFieldSecurityRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DigitalFieldSecurityDataMapping();
        }

        public DigitalFieldSecurityQueryService(IInfrastructureContext context)
        {
            this.repository = new DigitalFieldSecurityRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DigitalFieldSecurityDataMapping();
        }
		 
		public  DigitalFieldSecurityPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DigitalFieldSecurityKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DigitalFieldSecurity entityPOCO)
        {
            DigitalFieldSecurityKeys entityKeys = new DigitalFieldSecurityKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 