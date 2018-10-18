 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class NotificationReplyRepository:IRepository<NotificationReply>
   {

       public List<NotificationReply> GetMulti(EntityKeyFields entityKeys)
       {

           NotificationKeys notificationKeys = entityKeys as NotificationKeys;

           return (from a in context.NotificationReplies
                   where a.NotificationId == notificationKeys.Id 
                   select a).ToList();
       }

   }

}
   