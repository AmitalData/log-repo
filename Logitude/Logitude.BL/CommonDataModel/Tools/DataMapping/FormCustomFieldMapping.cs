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
    public class FormCustomFieldMapping
    {
        public static void MapEntity(FormCustomFieldPM entityPM, FormCustomField poco, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewState)
            {
                poco.Tenant = entityPM.Tenant;
            }

            poco.Value = entityPM.Value;
            poco.DocumentTypeId = entityPM.DocumentTypeId;
            poco.FieldCode = entityPM.FieldCode;
            poco.ObjectTableId = entityPM.ObjectTableId;
            poco.EntityId = entityPM.EntityId;
            
        }
    }
}