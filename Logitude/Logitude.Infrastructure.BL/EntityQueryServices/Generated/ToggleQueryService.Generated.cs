 
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
   public partial class ToggleQueryService: EntityQueryService<Toggle,ToggleKeys,TogglePM,object,ToggleKeys>
   {
   
        ToggleRepository repository;
		IInfrastructureContext  context;
        public ToggleQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new ToggleRepository(context);
            Repository = repository;
            mapping = new ToggleDataMapping();
        }

        public ToggleQueryService(ToggleRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ToggleDataMapping();
        }

        public ToggleQueryService(IInfrastructureContext context)
        {
            this.repository = new ToggleRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ToggleDataMapping();
        }
		 
		public  TogglePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ToggleKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Toggle entityPOCO)
        {
            ToggleKeys entityKeys = new ToggleKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 