 
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
   public partial class ContainerSettingQueryService: EntityQueryService<ContainerSetting,ContainerSettingKeys,ContainerSettingPM,object,ContainerSettingKeys>
   {
   
        ContainerSettingRepository repository;
		IInfrastructureContext  context;
        public ContainerSettingQueryService(int tenant)
        {
		    context = InfrastructureContext.GetContext(tenant);
            MainContext = context;
            repository = new ContainerSettingRepository(context);
            Repository = repository;
            mapping = new ContainerSettingDataMapping();
        }

        public ContainerSettingQueryService(ContainerSettingRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new ContainerSettingDataMapping();
        }

        public ContainerSettingQueryService(IInfrastructureContext context)
        {
            this.repository = new ContainerSettingRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new ContainerSettingDataMapping();
        }
		 
		public  ContainerSettingPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new ContainerSettingKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(ContainerSetting entityPOCO)
        {
            ContainerSettingKeys entityKeys = new ContainerSettingKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 