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
    public class DocumentTypeCustomFieldMapping
    {
        public static void MapEntity(DocumentTypeCustomFieldPM entityPM, DocumentTypeCustomField poco, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewState)
            {
                poco.Tenant = entityPM.Tenant;
            }

            poco.DefaultValue = entityPM.DefaultValue;
            poco.DocumentTypeId = entityPM.DocumentTypeId;
            poco.FieldCode = entityPM.FieldCode;
            poco.FieldDataTypeCode = entityPM.FieldDataTypeCode;
            poco.InActive = entityPM.InActive;
            poco.IsRequired = entityPM.IsRequired;            
            poco.MultiLine = entityPM.MultiLine;
            poco.Name = entityPM.Name;
            poco.IndexOrder = entityPM.IndexOrder;
        }
    }
}