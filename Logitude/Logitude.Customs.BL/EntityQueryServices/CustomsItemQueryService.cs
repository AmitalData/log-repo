using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.StimulReport;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class CustomsItemQueryService
    {
        public List<CustomsItemPM> GetAll()
        {
            var pocos = repository.GetAll().ToList();
            var pms = pocos.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }

        public CustomsItemPM GetCustomsItemByClassificationCode(string classificationCode)
        {
            CustomsItemPM pm = null;
          
            var poco = repository.GetCustomsItemByClassificationCode(classificationCode);

            if (poco != null)
            {
                 pm = this.GetEntityPM(poco);
            }
            return pm;
        }

        public List<CustomsItemPM> GetAllCustomsItemByClassificationCode(string classificationCode)
        {
            var pm=new List<CustomsItemPM>();

            var poco = repository.GetAllCustomsItemByClassificationCode(classificationCode);

            foreach(var item in poco)
            {
                pm.Add(GetEntityPM(item));
            }
            return pm;
        }

        public string GetQuantityTypeByClassificationCode(string classificationCode, int tenant)
        {
            if (string.IsNullOrWhiteSpace(classificationCode)) return null;
            if (classificationCode.Length > 10)
            {
                classificationCode = classificationCode.Substring(0,10);
            }
            CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(tenant);
            CustomsItemPM customsItem = customsItemQueryService.GetCustomsItemByClassificationCode(classificationCode);

            PropertiesDetailsHistoryPM propertiesDetailsHistory = null;
            MeasurmentUnitPM measurmentUnit = null;
            string QuantityTypeCode = null;

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

        public string GetQuantityTypeByClassificationWithMultiCustomItems(string classificationCode, int tenant, bool isExport, bool getFromCache=true)
        {
            if (string.IsNullOrWhiteSpace(classificationCode)) return null;
            if (classificationCode.Length > 10)
            {
                classificationCode = classificationCode.Substring(0, 10);
            }
            CustomsItemQueryService customsItemQueryService = new CustomsItemQueryService(tenant);
            var customsItems = customsItemQueryService.GetAllCustomsItemByClassificationCode(classificationCode);
            customsItems = customsItems.FindAll(x => x.CustomsItemCategoryID == 1);

            customsItems = isExport ?
                customsItems.FindAll(x => x.CustomsBookTypeID == 2 || x.CustomsBookTypeID == 3) :
                customsItems.FindAll(x => x.CustomsBookTypeID == 1);

            PropertiesDetailsHistoryPM propertiesDetailsHistory = null;
            MeasurmentUnitPM measurmentUnit = null;
            string QuantityTypeCode = null;

            PropertiesDetailsHistoryQueryService propertiesDetailsHistoryQueryService = new PropertiesDetailsHistoryQueryService(tenant);


            foreach(var customsItem in customsItems)
            {
                propertiesDetailsHistory = propertiesDetailsHistoryQueryService.GetPropertiesDetailsHistoryByCustomsItemId(customsItem.ID, getFromCache);
                if(propertiesDetailsHistory != null && propertiesDetailsHistory.StartDate < DateTime.Now && propertiesDetailsHistory.EndDate > DateTime.Now)
                {
                    break;
                }
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
