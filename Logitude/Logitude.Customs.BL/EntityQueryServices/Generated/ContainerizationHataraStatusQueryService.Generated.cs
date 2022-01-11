 
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
   public partial class ContainerizationHatataStatusQueryService: EntityQueryService<ContainerizationHatataStatus,ContainerizationHatataStatusKeys,ContainerizationHatataStatusPM,object,ContainerizationHatataStatusKeys>
   {
   
        ContainerizationHatataStatusRepository repository;
		ICustomContext  context;
        public ContainerizationHatataStatusQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new ContainerizationHatataStatusRepository(context);
            Repository = repository;
            mapping = new ContainerizationHatataStatusDataMapping();
        }

        public ContainerizationHatataStatusQueryService(ContainerizationHatataStatusRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ContainerizationHatataStatusDataMapping();
        }

        public ContainerizationHatataStatusQueryService(ICustomContext context)
        {
            this.repository = new ContainerizationHatataStatusRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ContainerizationHatataStatusDataMapping();
        }
		 
		public  ContainerizationHatataStatusPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ContainerizationHatataStatusKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ContainerizationHatataStatus entityPOCO)
        {
            ContainerizationHatataStatusKeys entityKeys = new ContainerizationHatataStatusKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 