using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CommunicationAttachmentMapping
    {
        public static void MapEntity(CommunicationAttachmentPM entityPM, CommunicationAttachment poco, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewState)
            {
                poco.Tenant = entityPM.Tenant;
            }

            
            poco.DocumentId = entityPM.DocumentId;
            poco.CommunicationLogId = entityPM.CommunicationLogId;
           
        }
    }
}