
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
using Logitude.Customs.BL.EntityQueryServices;
using System.Globalization;
using System.Threading;

namespace Logitude.Customs.BL.EntityDataMappings
{

    public partial class CustomsDocumentMetaDataValueDataMapping : IMapping<CustomsDocumentMetaDataValuePM, CustomsDocumentMetaDataValue>
    {

        public void CustomPMToPOCO(CustomsDocumentMetaDataValuePM entityPM, CustomsDocumentMetaDataValue entityPOCO)
        {
            //var save = Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern;
            //try
            //{
                //Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern = @"dd\/MM\/yyyy";
                CustomMappedPOCOProperties.Add(POCOPropertyNames.CustomsDocumentId);
                this.CustomMappedPOCOProperties.Add(POCOPropertyNames.MetaDataTypeCode);
                if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
                {

                    entityPOCO.CustomsDocumentId = entityPM.CustomsDocumentId;
                    entityPOCO.MetaDataTypeCode = entityPM.MetaDataTypeCode;

                }

                //if (!String.IsNullOrWhiteSpace(entityPM.MetaDataValue))
                //{
                //    DateTime valueDate;
                //    if (DateTime.TryParse(entityPM.MetaDataValue, out valueDate))
                //    {
                //        this.CustomMappedPOCOProperties.Add(POCOPropertyNames.MetaDataValue);
                //        entityPOCO.MetaDataValue = valueDate.ToString("dd.MM.yy");
                //    }
                //}
            //}
            //finally
            //{
            //    //Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern = save;
            //}

        }

        public void CustomPOCOToPM(CustomsDocumentMetaDataValuePM entityPM, CustomsDocumentMetaDataValue entityPOCO)
        {

            //var save = Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern;
            //try
            //{
                //Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern = @"dd\/MM\/yyyy";

                CustomMappedPMProperties.Add(PMPropertyNames.MetaDataTypeName);
                if (entityPOCO.MetaDataTypeCode != null)
                {
                    CustomMetaDataTypeQueryService customMetaDataTypeQueryService = new CustomMetaDataTypeQueryService(entityPOCO.Tenant);
                    CustomMetaDataTypePM customMetaDataType = customMetaDataTypeQueryService.GetSingle(entityPOCO.MetaDataTypeCode, false, true);
                    entityPM.MetaDataTypeName = customMetaDataType.LocalName;
                }
            //    var saveCurrentCulture = Thread.CurrentThread.CurrentCulture;
            //    var saveCurrentUICulture = Thread.CurrentThread.CurrentUICulture;


            //    if (!string.IsNullOrWhiteSpace(entityPOCO.MetaDataValue))
            //    {
            //        DateTime valueDate;
            //        String[] allFormats = new string[] {

            //        "dd.MM.yy"
            //    };
            //        if (DateTime.TryParseExact(entityPOCO.MetaDataValue, allFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out valueDate))
            //        {
            //            this.CustomMappedPMProperties.Add(PMPropertyNames.MetaDataValue);
            //            entityPM.MetaDataValue = valueDate.ToString();
            //        }
            //    }
            //    if (saveCurrentCulture != Thread.CurrentThread.CurrentCulture)
            //    {
            //        throw new Exception("ddd");
            //    }
            //    if (saveCurrentUICulture != Thread.CurrentThread.CurrentUICulture)
            //    {
            //        throw new Exception("ddd");
            //    }
            //}
            //finally
            //{
            //    //Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern = save;
            //}
        }
    }


}
   