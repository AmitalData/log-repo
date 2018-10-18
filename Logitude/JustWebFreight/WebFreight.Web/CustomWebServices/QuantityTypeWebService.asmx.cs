using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebFreight.Web.CustomWebServices
{
    /// <summary>
    /// Summary description for QuantityTypeWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class QuantityTypeWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string GetQuantityType(string classificationCode, int tenant)
        {
            PropertiesDetailsHistoryPM propertiesDetailsHistory = null;
            MeasurmentUnitPM measurmentUnit = null;
            string QuantityTypeCode = null;
            
            
            CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(tenant);
            CustomsItemPM customsItem = customsItemQueryService.GetCustomsItemByClassificationCode(classificationCode);
         

            PropertiesDetailsHistoryQueryService propertiesDetailsHistoryQueryService = new PropertiesDetailsHistoryQueryService(tenant);
            if (customsItem != null)
            {
                propertiesDetailsHistory = propertiesDetailsHistoryQueryService.GetPropertiesDetailsHistoryByCustomsItemId(customsItem.ID);

            }
            MeasurmentUnitQueryService measurmentUnitQueryService = new MeasurmentUnitQueryService(tenant);

            if (propertiesDetailsHistory != null && propertiesDetailsHistory.MeasurementUnitID.HasValue)
            {

               measurmentUnit = measurmentUnitQueryService.GetMeasurmentUnitByMalamId(propertiesDetailsHistory.MeasurementUnitID.Value);

            }

            if (measurmentUnit != null)
            {
                 QuantityTypeCode = measurmentUnit.Code;
            }

             return QuantityTypeCode;

        }
    }
}
