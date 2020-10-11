
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class NotificationReplyDataMapping: IMapping<NotificationReplyPM, NotificationReply>
   {

        public void CustomPMToPOCO(NotificationReplyPM entityPM, NotificationReply entityPOCO)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.NotificationId = entityPM.NotificationId;
                entityPOCO.Line = entityPM.Line;
                entityPOCO.Tenant = entityPM.Tenant;
           


            }

        }

        public void CustomPOCOToPM(NotificationReplyPM entityPM, NotificationReply entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.RepliedByUserName);

            if (entityPOCO.RepliedByUserId != null)
            {
                UserRepository rep = new UserRepository(entityPOCO.Tenant);
                User user = rep.GetSingleUser(entityPOCO.RepliedByUserId, entityPOCO.Tenant, false);
                if (user == null)
                {
                    user = rep.GetSingleUser(entityPOCO.RepliedByUserId, 0, false);
                }
                user = user ?? new User();
                //var qs= new SystemDataQuery(entityPOCO.Tenant);
                //var user = qs.GetSinglePM(entityPOCO.RepliedByUserId, entityPOCO.Tenant);

                entityPM.RepliedByUserName = user.Contact != null ? user.Contact.LocalName: null;
            }
        }
   }


}
   