 
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
   public partial class NotificationDefinitionQueryService: EntityQueryService<NotificationDefinition,NotificationDefinitionKeys,NotificationDefinitionPM,object,NotificationDefinitionKeys>
   {
   
        NotificationDefinitionRepository repository;
		ICustomContext  context;
        public NotificationDefinitionQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new NotificationDefinitionRepository(context);
            Repository = repository;
            mapping = new NotificationDefinitionDataMapping();
        }

        public NotificationDefinitionQueryService(NotificationDefinitionRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new NotificationDefinitionDataMapping();
        }

        public NotificationDefinitionQueryService(ICustomContext context)
        {
            this.repository = new NotificationDefinitionRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new NotificationDefinitionDataMapping();
        }
		 
		public  NotificationDefinitionPM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new NotificationDefinitionKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(NotificationDefinition entityPOCO)
        {
            NotificationDefinitionKeys entityKeys = new NotificationDefinitionKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 