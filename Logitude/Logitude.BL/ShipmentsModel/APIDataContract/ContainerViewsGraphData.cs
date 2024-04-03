using Logitude.BL.ShipmentsModel.CustomFilters;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.BL.ShipmentsModel.APIDataContract
{
    public class ContainerViewsGraphData
    {
        public ContainerViewsGraphData()
        {
            Datas = new List<ContainerViewsGraphDataItem>();
        }

        public List<ContainerViewsGraphDataItem> Datas { get; set; }
    }

    public class ContainerViewsGraphDataItem
    {
        public string QueryCode { get; set; }
        public int Value { get; set; }
        public string Label { get; set; }
        public string ToolTip { get; set; }
    }


    public class ContainerViewsQueries
    {

        public List<ContainerViewsQueries> BuilQueries(int tenant, IShipmentsContext context)
        {
            ContainerCustomFilter customFilter = new ContainerCustomFilter(tenant);
            var queries = new List<ContainerViewsQueries>();
            queries.Add(new ContainerViewsQueries
            {
                QueryCode = "PendingPOLDeparture",
                Query = customFilter.GetFilteredQuery(BuildQueryOperations("PendingPOLDepartureFilter"), BuildDefaultQuery(context, tenant)),
                FeatureCode = GetFeatureCode("PendingPOLDeparture"),
                Label = "Pending POL Departure",
                ToolTip = "POL departure within the next two days"
            });
            queries.Add(new ContainerViewsQueries
            {
                QueryCode = "InTransitNew",
                Query = customFilter.GetFilteredQuery(BuildQueryOperations("InTransitFilter"), BuildDefaultQuery(context, tenant)),
                FeatureCode = GetFeatureCode("InTransitNew"),
                Label = "In Transit",
                ToolTip = "POL Departed but not arrived"
            });
            queries.Add(new ContainerViewsQueries
            {
                QueryCode = "PendingArrival",
                Query = customFilter.GetFilteredQuery(BuildQueryOperations("PendingArrivalFilter"), BuildDefaultQuery(context, tenant)),
                FeatureCode = GetFeatureCode("PendingArrival"),
                Label = "Pending Arrival",
                ToolTip = "POL Departed and estimated arrival within today"
            });
            queries.Add(new ContainerViewsQueries
            {
                QueryCode = "PendingDischarge",
                Query = customFilter.GetFilteredQuery(BuildQueryOperations("PendingDischargeFilter"), BuildDefaultQuery(context, tenant)),
                FeatureCode = GetFeatureCode("PendingDischarge"),
                Label = "Pending Discharge",
                ToolTip = "POD Arrived but not discharged"
            });
            queries.Add(new ContainerViewsQueries
            {
                QueryCode = "PendingGateOut",
                Query = customFilter.GetFilteredQuery(BuildQueryOperations("PendingGateOutFilter"), BuildDefaultQuery(context, tenant)),
                FeatureCode = GetFeatureCode("PendingGateOut"),
                Label = "Pending Gate Out",
                ToolTip = "POD Arrived and Discharged but not Gated Out"
            });
            queries.Add(new ContainerViewsQueries
            {
                QueryCode = "PendingDelivery",
                Query = customFilter.GetFilteredQuery(BuildQueryOperations("PendingDeliveryFilter"), BuildDefaultQuery(context, tenant)),
                FeatureCode = GetFeatureCode("PendingDelivery"),
                Label = "Pending Delivery",
                ToolTip = "Shipment Delivery departed but not arrived"
            });
            queries.Add(new ContainerViewsQueries
            {
                QueryCode = "PendingEmptyReturn",
                Query = customFilter.GetFilteredQuery(BuildQueryOperations("PendingEmptyReturnFilter"), BuildDefaultQuery(context, tenant)),
                FeatureCode = GetFeatureCode("PendingEmptyReturn"),
                Label = "Pending Empty Return",
                ToolTip = "POD Gated Out but empty container not returned"
            });
            return queries;
        }

        private IQueryable<Container> BuildDefaultQuery(IShipmentsContext context, int tenant)
        {
            return context.Containers.Where(x => x.Tenant == tenant);
        }

        private string GetFeatureCode(string queryCode)
        {
            return "Container.Q." + queryCode;
        }

        private QueryOperations BuildQueryOperations(string queryField)
        {
            return new QueryOperations
            {
                QueryFilterItems = new List<QueryFilterItem>()
                {
                    new QueryFilterItem
                    {
                        FieldName = queryField,
                        IsCustom = true
                    }
                }
            };
        }

        public string QueryCode { get; set; }
        public string Label { get; set; }
        public string ToolTip { get; set; }
        public string FeatureCode { get; set; }
        public IQueryable<Container> Query { get; set; }
    }

}
