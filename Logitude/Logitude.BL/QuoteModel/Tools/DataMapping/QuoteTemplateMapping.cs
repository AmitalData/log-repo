using Simplog.Data.QuoteModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data;
namespace Logitude.BL.QuoteModel.Tools.DataMapping
{
    public class QuoteTemplateMapping
    {
        public static void MappingQuoteTemplate(QuoteTemplatePM itemPM, QuoteTemplate itemPoco, bool isNewEntity)
        {

            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
            }

            itemPoco.Name = itemPM.Name;
            itemPoco.IsTemplate = itemPM.IsTemplate;
            itemPoco.HeaderDocId = itemPM.HeaderDocId;
            itemPoco.FooterDocId = itemPM.FooterDocId;
            itemPoco.Tenant = itemPM.Tenant;
            itemPoco.QuoteTemplateSettingId = itemPM.QuoteTemplateSettingId;
            itemPoco.OriginalQuoteTemplateId = itemPM.OriginalQuoteTemplateId;
            itemPoco.CreatedByUserId = itemPM.CreatedByUserId;
            itemPoco.CreateDate = itemPM.CreateDate;
            itemPoco.UpdatedByUserId = itemPM.UpdatedByUserId;
            itemPoco.UpdateDate = itemPM.UpdateDate;
            itemPoco.SearchFields = itemPM.SearchFields;
            itemPoco.TemplateTypeCode = itemPM.TemplateTypeCode;
            itemPoco.IsDefault = itemPM.IsDefault;
            itemPoco.InActive = itemPM.InActive;
            itemPoco.IsEnabledForCustomers = itemPM.IsEnabledForCustomers;
            itemPoco.IsCopiedAtSignup = itemPM.IsCopiedAtSignup;

            string mySearchFields = "";

           

            if (!string.IsNullOrEmpty(itemPM.Name))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? itemPM.Name : mySearchFields + "," + itemPM.Name;
            }

            if (!string.IsNullOrEmpty(itemPM.TemplateTypeCode))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? itemPM.TemplateTypeCode : mySearchFields + "," + itemPM.TemplateTypeCode;
            }

            itemPM.SearchFields = mySearchFields;
            itemPoco.SearchFields = mySearchFields;

        }
    }
}