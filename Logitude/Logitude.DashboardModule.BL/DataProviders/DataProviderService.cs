using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public class DataProviderService
    {

        public string GetData(WidgetPM widget)
        {
            var entity = GetEntity(widget.EntityId);
            var entityFields = GetEntityFields(widget.EntityId).ToDictionary(e=>e.Id,e=>e);
            var groupBy = entityFields.ContainsKey(widget.GroupById) ? entityFields[widget.GroupById] : throw new Exception($"Meta Data Field '{widget.GroupById}' not found");
            var query = $@"select {groupBy.FieldCode} as GroupName, Value From {entity.TableName} group by {groupBy.FieldCode} ";
            return null;
        }

        private List<AnalyticsFactsFieldsMetaData> GetEntityFields(string entityId)
        {
            var analyticsFactsFieldsMetaDataRepository = new AnalyticsFactsFieldsMetaDataRepository(0);
            var entityFields = analyticsFactsFieldsMetaDataRepository.GetAll(0).Where(e=>e.AnalyticsFactsMetaDataId == entityId).ToList();

            return entityFields;
        }

        private AnalyticsFactsMetaData GetEntity(string entityId)
        {
            var analyticsFactsMetaDataRepository = new AnalyticsFactsMetaDataRepository(0);
            var entity = analyticsFactsMetaDataRepository.GetSingle(entityId, 0);
            if(entity == null)
                throw new Exception($"Meta Data entity '{entityId}' not found");
            return entity;
        }
    }
}
