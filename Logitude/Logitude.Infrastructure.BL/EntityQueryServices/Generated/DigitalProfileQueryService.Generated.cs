 
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
   public partial class DigitalProfileQueryService: EntityQueryService<DigitalProfile,DigitalProfileKeys,DigitalProfilePM,object,DigitalProfileKeys>
   {
   
        DigitalProfileRepository repository;
		IInfrastructureContext  context;
        public DigitalProfileQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new DigitalProfileRepository(context);
            Repository = repository;
            mapping = new DigitalProfileDataMapping();
        }

        public DigitalProfileQueryService(DigitalProfileRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DigitalProfileDataMapping();
        }

        public DigitalProfileQueryService(IInfrastructureContext context)
        {
            this.repository = new DigitalProfileRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DigitalProfileDataMapping();
        }
		 
		public  DigitalProfilePM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DigitalProfileKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DigitalProfile entityPOCO)
        {
            DigitalProfileKeys entityKeys = new DigitalProfileKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 