using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public class GlobalFilterService
    {
        private readonly WidgetPM widget;
        private readonly AnalyticsFactsMetaData entity;
        public GlobalQueryFilterItem compareWithPreviousFilterItem;
        public string widgetType;

        public GlobalFilterService(WidgetPM widget, AnalyticsFactsMetaData entity, string widgetType)
        {
            this.widget = widget;
            this.entity = entity;
            this.widgetType = widgetType;
        }

        public string AddGlobalFilters()
        {
            if (string.IsNullOrEmpty(widget.GlobalFilters)) return widget.Filters;
            var globalFilters = BuildQueryFilterItems();
            if (globalFilters == null || globalFilters.Count == 0) return widget.Filters;

            QueryFilterItem widgetFilters = widget.Filters != null ? Newtonsoft.Json.JsonConvert.DeserializeObject<QueryFilterItem>(widget.Filters) : null;
            if (widgetFilters == null)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(new QueryFilterItem
                {
                    IsAnalyticsMetadatas = true,
                    FilterType = "And",
                    Operator = "",
                    FieldDataType = "",
                    QueryFilterItems = globalFilters
                });
            }

            var queryFilter = new QueryFilterItem
            {
                IsAnalyticsMetadatas = true,
                FilterType = "And",
                Operator = "",
                FieldDataType = "",
                QueryFilterItems = new List<QueryFilterItem>()
            };
            queryFilter.QueryFilterItems.Add(widgetFilters);
            queryFilter.QueryFilterItems.AddRange(globalFilters);

            return Newtonsoft.Json.JsonConvert.SerializeObject(queryFilter);
        }

        private List<QueryFilterItem> BuildQueryFilterItems()
        {
            List<QueryFilterItem> queryFilterItems = new List<QueryFilterItem>();
            foreach (var globalQueryFilterItem in Newtonsoft.Json.JsonConvert.DeserializeObject<List<GlobalQueryFilterItem>>(widget.GlobalFilters))
            {
                if (!globalQueryFilterItem.IsCommon && !globalQueryFilterItem.IsPreset && entity.Id != globalQueryFilterItem.DataSetId) continue;
                if (globalQueryFilterItem.CompareWithPrevious)
                {
                    this.compareWithPreviousFilterItem = globalQueryFilterItem;
                    if (widgetType == "kpi") continue;
                }
                var queryFilterItem = MapFilterObjectToQueryFilterItem(globalQueryFilterItem);
                if (queryFilterItem == null) continue;
                queryFilterItems.Add(queryFilterItem);
            }
            return queryFilterItems;
        }

        private QueryFilterItem MapFilterObjectToQueryFilterItem(GlobalQueryFilterItem globalQueryFilterItem)
        {
            var commonGlobalFilterItem = globalQueryFilterItem.IsCommon ? CommonGlobalFilterProvider.GetFilterItem(globalQueryFilterItem.FieldName, entity.TableName) : null;
            if (globalQueryFilterItem.IsCommon && commonGlobalFilterItem == null) return null;

            var queryFilterItem = new QueryFilterItem();
            queryFilterItem.IsAnalyticsMetadatas = true;
            queryFilterItem.FilterType = "And";
            queryFilterItem.Operator = globalQueryFilterItem.Operator;
            queryFilterItem.FieldDataType = globalQueryFilterItem.IsCommon ? commonGlobalFilterItem.Type : globalQueryFilterItem.DataTypeCode;
            queryFilterItem.FieldName = globalQueryFilterItem.IsCommon ? commonGlobalFilterItem.Name : globalQueryFilterItem.FieldName;
            queryFilterItem.DateGroupCode = globalQueryFilterItem.DateGroupCode;
            queryFilterItem.FieldValue = globalQueryFilterItem.FieldValue;
            queryFilterItem.FieldValue2 = globalQueryFilterItem.FieldValue2;
            queryFilterItem.FieldValue3 = globalQueryFilterItem.FieldValue3;
            //queryFilterItem.CompareWithPrevious = globalQueryFilterItem.CompareWithPrevious;
            return queryFilterItem;
        }

        public object GetPropValue(object src, string propName)
        {
            try { return src.GetType().GetProperty(propName).GetValue(src, null); }
            catch (Exception) { return null; }

        }

        public class GlobalQueryFilterItem
        {
            public string Operator { get; set; }
            public string DataTypeCode { get; set; }
            public string FieldName { get; set; }
            public object FieldValue { get; set; }
            public object FieldValue2 { get; set; }
            public object FieldValue3 { get; set; }
            public string DateGroupCode { get; set; }
            public string DataSetId { get; set; }
            public bool IsCommon { get; set; }
            public bool IsPreset { get; set; }
            public bool CompareWithPrevious { get; set; }
        }
    }
}
