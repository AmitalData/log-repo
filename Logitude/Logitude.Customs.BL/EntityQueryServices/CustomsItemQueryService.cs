using Logitude.Customs.Def.Contracts;
using Logitude.Customs.Def.EntityPMs;
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

        public string GetQuantityTypeByClassificationCode(string classificationCode, int tenant)
        {
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

    }
}
