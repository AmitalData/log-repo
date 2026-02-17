 
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
   public partial class NotificationTypeQueryService: EntityQueryService<NotificationType,NotificationTypeKeys,NotificationTypePM,object,NotificationTypeKeys>
   {
   
        NotificationTypeRepository repository;
		ICustomContext  context;
        public NotificationTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new NotificationTypeRepository(context);
            Repository = repository;
            mapping = new NotificationTypeDataMapping();
        }

        public NotificationTypeQueryService(NotificationTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new NotificationTypeDataMapping();
        }

        public NotificationTypeQueryService(ICustomContext context)
        {
            this.repository = new NotificationTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new NotificationTypeDataMapping();
        }
		 
		public  NotificationTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new NotificationTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(NotificationType entityPOCO)
        {
            NotificationTypeKeys entityKeys = new NotificationTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 