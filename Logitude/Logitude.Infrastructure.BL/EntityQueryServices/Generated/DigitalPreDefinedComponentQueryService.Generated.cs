 
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
   public partial class DigitalPreDefinedComponentQueryService: EntityQueryService<DigitalPreDefinedComponent,DigitalPreDefinedComponentKeys,DigitalPreDefinedComponentPM,object,DigitalPreDefinedComponentKeys>
   {
   
        DigitalPreDefinedComponentRepository repository;
		IInfrastructureContext  context;
        public DigitalPreDefinedComponentQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new DigitalPreDefinedComponentRepository(context);
            Repository = repository;
            mapping = new DigitalPreDefinedComponentDataMapping();
        }

        public DigitalPreDefinedComponentQueryService(DigitalPreDefinedComponentRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new DigitalPreDefinedComponentDataMapping();
        }

        public DigitalPreDefinedComponentQueryService(IInfrastructureContext context)
        {
            this.repository = new DigitalPreDefinedComponentRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new DigitalPreDefinedComponentDataMapping();
        }
		 
		public  DigitalPreDefinedComponentPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new DigitalPreDefinedComponentKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(DigitalPreDefinedComponent entityPOCO)
        {
            DigitalPreDefinedComponentKeys entityKeys = new DigitalPreDefinedComponentKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 