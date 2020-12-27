using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Logitude.Server.Tools;

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
            poco.BCC = entityPM.BCC;
            poco.DefultAttachmentsXML = entityPM.DefultAttachmentsXML;
            poco.To = entityPM.To;
            poco.AutomationId = entityPM.AutomationId;


            if (entityPM.IsDefultAttachmentsXMLChanged)
            {
                poco.DefultAttachmentsXML = GetDefultAttachmentsXML(entityPM);
                entityPM.IsDefultAttachmentsXMLChanged = false;
            }



        }

        private static string GetDefultAttachmentsXML(DocumentTypeTemplatePM entityPM )
        {
            string result = string.Empty;
            if (entityPM.DocumentDefultAttachments != null)
            {
                System.Type type1 = "string".GetType();
                System.Type[] types = new System.Type[1];
                types[0] = type1;
                result = LogitudeXmlSerializer.SerializeObjectToElementString(entityPM.DocumentDefultAttachments, types);
            }
            return result;
        }






    }
}
