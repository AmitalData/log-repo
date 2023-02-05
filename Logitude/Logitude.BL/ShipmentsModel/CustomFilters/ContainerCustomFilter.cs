using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.CustomFilters
{
    public class ContainerCustomFilter
    {
        public int Tenant { get; set; }
        public ContainerCustomFilter(int tenant)
        {
            this.Tenant = tenant;
        }

        public IQueryable<Container> GetFilteredQuery(QueryOperations operations, IQueryable<Container> queryableData)
        {
            List<QueryFilterItem> queryFilters = operations.QueryFilterItems;
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(Tenant).Date;
            bool showIsCancelled = false;
            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    if (item.FieldName == "Containers")                                      
                        queryableData = queryableData.Where(d => !d.IsClosed && !d.IsCancelled);                    

                    else if (item.FieldName == "InTransitFilter")                    
                        queryableData = queryableData.Where(d => d.ActualPOLVesselDeparture != null && (d.TransshipmentCount == 0 || d.TransshipmentCount == null)&& d.ActualPODVesselArrival == null && d.EstimatedPODVesselArrival != todayDate);
                    
                    else if (item.FieldName == "InTransitwithTransshipmentsFilter")                    
                        queryableData = queryableData.Where(d => d.ActualPOLVesselDeparture != null && (d.TransshipmentCount != null && d.TransshipmentCount > 0 ) && d.ActualPODVesselArrival == null && d.EstimatedPODVesselArrival != todayDate);
                    
                    else if (item.FieldName == "PendingGateOutFilter")                    
                        queryableData = queryableData.Where(d => d.ActualPODVesselArrival != null &&  d.GateOut == null && d.ActualPODDischarge != null);
                    
                    else if (item.FieldName == "PendingEmptyReturnFilter")                    
                        queryableData = queryableData.Where(d => d.GateOut != null &&  d.ActualEmptyReturn == null);                    

                    else if (item.FieldName == "ClosedContainers")                    
                        queryableData = queryableData.Where(d => d.IsClosed);
                    
                    else if (item.FieldName == "PendingArrivalFilter")                    
                        queryableData = queryableData.Where(d => d.ActualPOLVesselDeparture != null && d.ActualPODVesselArrival == null && d.EstimatedPODVesselArrival == todayDate);
                    
                    else if (item.FieldName == "PendingDischargeFilter")                    
                        queryableData = queryableData.Where(d => d.ActualPODDischarge == null && d.ActualPODVesselArrival != null);
                    
                    else if (item.FieldName == "PendingDeliveryFilter")                    
                        queryableData = queryableData.Where(d => d.ShipmentDeliveryATD != null && d.ShipmentDeliveryATA == null);

                    else if (item.FieldName == "PendingPOLDepartureFilter")
                    {
                        DateTime twoDaysAgoDate = todayDate.AddDays(-2);

                        queryableData = from d in queryableData
                                        where d.ActualPOLVesselDeparture == null
                                        && d.EstimatedPOLVesselDeparture != null
                                        && (d.EstimatedPOLVesselDeparture <= twoDaysAgoDate)
                                        select d;
                    }

                    else if (item.FieldName == "PreviousContainersFilter")
                    {
                        var newDate = new DateTime(todayDate.Year, todayDate.Month, 1);
                        var start = newDate.AddMonths(-1).Date;
                        var end = newDate.AddDays(-1).Date;

                        queryableData = queryableData.Where(d => d.RequestDate != null && d.RequestDate >= start && d.RequestDate <= end);
                    }
                }

                if (item.FieldName == "IsCancelled")
                {
                    bool value = Convert.ToBoolean(item.FieldValue);
                    if (value)
                    {
                        showIsCancelled = true;
                    }
                }
            }

            return queryableData.Where(d => d.IsCancelled == showIsCancelled);
        }
    }
}
