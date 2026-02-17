 
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
   public partial class NotificationReplyQueryService: EntityQueryService<NotificationReply,NotificationReplyKeys,NotificationReplyPM,NotificationPM,NotificationKeys>
   {
   
        NotificationReplyRepository repository;
		ICustomContext  context;
        public NotificationReplyQueryService(int tenant)
        {
		    context = CustomContext.GetContext(tenant);
            MainContext = context;
            repository = new NotificationReplyRepository(context);
            Repository = repository;
            mapping = new NotificationReplyDataMapping();
        }

        public NotificationReplyQueryService(NotificationReplyRepository repository)
        {
            this.repository = repository;
            Repository = repository;
            mapping = new NotificationReplyDataMapping();
        }

        public NotificationReplyQueryService(ICustomContext context)
        {
            this.repository = new NotificationReplyRepository(context);
            this.context = context;

            MainContext = context;
            Repository = repository;
            mapping = new NotificationReplyDataMapping();
        }
		 
		public  NotificationReplyPM GetSingle(string notificationid, int line,bool getComposition, bool getFromCache)
        {
             EntityKeys = new NotificationReplyKeys(){ NotificationId = notificationid, Line = line };

			 return base.GetSingle(EntityKeys, getComposition, getFromCache);
        }

       
	    protected override EntityKeyFields GetKeys(NotificationReply entityPOCO)
        {
            NotificationReplyKeys entityKeys = new NotificationReplyKeys() { NotificationId = entityPOCO.NotificationId, Line = entityPOCO.Line,  };
            return entityKeys;
        }
     
	 
   }
   
}
	 