 
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
   public partial class NotificationTenantDefinitionQueryService: EntityQueryService<NotificationTenantDefinition,NotificationTenantDefinitionKeys,NotificationTenantDefinitionPM,object,NotificationTenantDefinitionKeys>
   {
   
        NotificationTenantDefinitionRepository repository;
		ICustomContext  context;
        public NotificationTenantDefinitionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new NotificationTenantDefinitionRepository(context);
            Repository = repository;
            mapping = new NotificationTenantDefinitionDataMapping();
        }

        public NotificationTenantDefinitionQueryService(NotificationTenantDefinitionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new NotificationTenantDefinitionDataMapping();
        }

        public NotificationTenantDefinitionQueryService(ICustomContext context)
        {
            this.repository = new NotificationTenantDefinitionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new NotificationTenantDefinitionDataMapping();
        }
		 
		public  NotificationTenantDefinitionPM GetSingle(string id,bool getComposition, bool getFromCache)
        {
             EntityKeys = new NotificationTenantDefinitionKeys(){ Id = id };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(NotificationTenantDefinition entityPOCO)
        {
            NotificationTenantDefinitionKeys entityKeys = new NotificationTenantDefinitionKeys() { Id = entityPOCO.Id,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 