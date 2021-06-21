 
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
   public partial class ContainerizationQueryService: EntityQueryService<Containerization,ContainerizationKeys,ContainerizationPM,object,ContainerizationKeys>
   {
   
        ContainerizationRepository repository;
		ICustomContext  context;
        public ContainerizationQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ContainerizationRepository(context);
            Repository = repository;
            mapping = new ContainerizationDataMapping();
        }

        public ContainerizationQueryService(ContainerizationRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ContainerizationDataMapping();
        }

        public ContainerizationQueryService(ICustomContext context)
        {
            this.repository = new ContainerizationRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ContainerizationDataMapping();
        }
		 
		public  ContainerizationPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ContainerizationKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(Containerization entityPOCO)
        {
            ContainerizationKeys entityKeys = new ContainerizationKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 