 
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
   public partial class AssigneeNotificationTypeQueryService: EntityQueryService<AssigneeNotificationType,AssigneeNotificationTypeKeys,AssigneeNotificationTypePM,object,AssigneeNotificationTypeKeys>
   {
   
        AssigneeNotificationTypeRepository repository;
		ICustomContext  context;
        public AssigneeNotificationTypeQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new AssigneeNotificationTypeRepository(context);
            Repository = repository;
            mapping = new AssigneeNotificationTypeDataMapping();
        }

        public AssigneeNotificationTypeQueryService(AssigneeNotificationTypeRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new AssigneeNotificationTypeDataMapping();
        }

        public AssigneeNotificationTypeQueryService(ICustomContext context)
        {
            this.repository = new AssigneeNotificationTypeRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new AssigneeNotificationTypeDataMapping();
        }
		 
		public  AssigneeNotificationTypePM GetSingle(string code,bool getComposition, bool getFromCache)
        {
             EntityKeys = new AssigneeNotificationTypeKeys(){ Code = code };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(AssigneeNotificationType entityPOCO)
        {
            AssigneeNotificationTypeKeys entityKeys = new AssigneeNotificationTypeKeys() { Code = entityPOCO.Code,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 