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
    public class DocumentTypeTemplateMapping
    {
        public static void MapEntity(DocumentTypeTemplatePM entityPM, DocumentTypeTemplate poco, bool isNewEntity)
        {            
            if (isNewEntity)
            {
                poco.Tenant = entityPM.Tenant;
            }
            
            poco.Description = entityPM.Description;
            poco.DocumentTypeId = entityPM.DocumentTypeId;
            poco.LastUpdateDate = entityPM.LastUpdateDate;
            poco.LastUpdatedByUserId = entityPM.LastUpdatedByUserId;
            poco.TemplateBody = entityPM.TemplateBody;
            poco.TemplateBodyHtml = entityPM.TemplateBodyHtml;
            poco.TemplateBodyjson = entityPM.TemplateBodyjson;
            poco.TemplateType = entityPM.TemplateType;
            poco.InActive = entityPM.InActive;
            poco.EditorTool = entityPM.EditorTool;
            poco.VerticalShift = entityPM.VerticalShift;
            poco.HorizontalShift = entityPM.HorizontalShift;
            poco.Subject = entityPM.Subject;
            poco.CountryCode = entityPM.CountryCode;
            poco.IsCopiedAtSignup = entityPM.IsCopiedAtSignup;
            poco.IsEnabledForCustomers = entityPM.IsEnabledForCustomers;
            poco.Language = entityPM.Language;
            poco.InternalRemarks = entityPM.InternalRemarks;
            poco.OriginalTemplateId = entityPM.OriginalTemplateId;
            poco.ReplyTo = entityPM.ReplyTo;
            poco.From = entityPM.From;
            poco.TemplateHeaderHtml = entityPM.TemplateHeaderHtml;
            poco.TemplateFooterHtml = entityPM.TemplateFooterHtml;
            poco.TemplateFooterHeight = entityPM.TemplateFooterHeight;
            poco.TemplateHeaderHeight = entityPM.TemplateHeaderHeight;
            poco.TemplateTechnologyCode = entityPM.TemplateTechnologyCode;
            poco.CC = entityPM.CC;
           
         

        }
    }
}
