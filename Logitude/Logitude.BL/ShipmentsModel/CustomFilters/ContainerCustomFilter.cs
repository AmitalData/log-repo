using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                    {
                        queryableData = queryableData.Where(d => !d.IsClosed && !d.IsCancelled);  
                    }

                    else if (item.FieldName == "InTransitFilter")
                    {
                        var containerObjectTableName = "Container";
                        var allStatuses = GetAllStatusesByObjecTableName(containerObjectTableName, Tenant);
                        var allowedStatusWeight = allStatuses.FirstOrDefault(a => a.Code == "ARPD")?.StatusWeight;
                        queryableData = queryableData.Where(d => (d.ActualPOLVesselDeparture != null || d.ShipmentMainCarriageATD != null)
                                                             && (d.TransshipmentCount == 0 || d.TransshipmentCount == null)
                                                             && (d.ShipmentTransshipment1FromPort == null 
                                                             &&  d.ShipmentTransshipment2FromPort == null
                                                             &&  d.ShipmentTransshipment2FromPort == null)
                                                             && (d.ActualPODVesselArrival == null && d.ShipmentLastLegATA == null)
                                                             && (d.EstimatedPODVesselArrival != todayDate && d.ShipmentLastLegETA != todayDate)
                                                             && d.EntityStatus.StatusWeight < allowedStatusWeight);
                    }

                    else if (item.FieldName == "InTransitwithTransshipmentsFilter")
                    {
                        var containerObjectTableName = "Container";
                        var allStatuses = GetAllStatusesByObjecTableName(containerObjectTableName, Tenant);
                        var allowedStatusWeight = allStatuses.FirstOrDefault(a => a.Code == "ARPD")?.StatusWeight;
                        queryableData = queryableData.Where(d => (d.ActualPOLVesselDeparture != null || d.ShipmentMainCarriageATD != null)

                                                             && ((d.TransshipmentCount != null && d.TransshipmentCount > 0)
                                                             || d.ShipmentTransshipment3FromPort != null
                                                             || d.ShipmentTransshipment2FromPort != null
                                                             || d.ShipmentTransshipment1FromPort != null)
                                                             && (d.ActualPODVesselArrival == null && d.ShipmentLastLegATA == null)
                                                             && (d.EstimatedPODVesselArrival != todayDate && d.ShipmentLastLegETA != todayDate)

                                                             && d.EntityStatus.StatusWeight < allowedStatusWeight);                     
                    }

                    else if (item.FieldName == "PendingGateOutFilter")
                    {
                        var containerObjectTableName = "Container";
                        var allStatuses = GetAllStatusesByObjecTableName(containerObjectTableName, Tenant);
                        var allowedStatusWeight = allStatuses.FirstOrDefault(a => a.Code == "GTOT")?.StatusWeight;
                        queryableData = queryableData.Where(d => (d.ActualPODVesselArrival != null || d.ShipmentLastLegATA != null)
                                                              && d.GateOut == null
                                                              && d.ActualPODDischarge != null
                                                              && d.EntityStatus.StatusWeight < allowedStatusWeight);
                    }

                    else if (item.FieldName == "PendingEmptyReturnFilter")
                    {
                        var containerObjectTableName = "Container";
                        var allStatuses = GetAllStatusesByObjecTableName(containerObjectTableName, Tenant);
                        var allowedStatusWeight = allStatuses.FirstOrDefault(a => a.Code == "EMRT")?.StatusWeight;
                        queryableData = queryableData.Where(d => d.GateOut != null && (d.ActualEmptyReturn == null && d.EmptyContainerReturnATA == null)
                                                              || (d.ShipmentDeliveryATA != null && (d.GateOut == null || (d.ActualEmptyReturn == null && d.EmptyContainerReturnATA == null)))
                                                              && d.EntityStatus.StatusWeight < allowedStatusWeight);
                    }

                    else if (item.FieldName == "ClosedContainers")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed);
                    }

                    else if (item.FieldName == "PendingArrivalFilter")

                    {
                        var containerObjectTableName = "Container";
                        var allStatuses = GetAllStatusesByObjecTableName(containerObjectTableName, Tenant);
                        var allowedStatusWeight = allStatuses.FirstOrDefault(a => a.Code == "ARPD")?.StatusWeight;
                        queryableData = queryableData.Where(d => (d.ActualPOLVesselDeparture != null || d.ShipmentMainCarriageATD != null)
                                                              && (d.ActualPODVesselArrival == null && d.ShipmentLastLegATA == null)
                                                              && (d.EstimatedPODVesselArrival == todayDate || (d.EstimatedPODVesselArrival == null && d.ShipmentLastLegETA == todayDate))
                                                              && d.EntityStatus.StatusWeight < allowedStatusWeight);                      
                    }

                    else if (item.FieldName == "PendingDischargeFilter")
                    {
                        var containerObjectTableName = "Container";
                        var allStatuses = GetAllStatusesByObjecTableName(containerObjectTableName, Tenant);
                        var allowedStatusWeight = allStatuses.FirstOrDefault(a => a.Code == "DSCH")?.StatusWeight;
                        queryableData = queryableData.Where(d => d.ActualPODDischarge == null
                                                              && (d.ActualPODVesselArrival != null || d.ShipmentLastLegATA != null)
                                                              && d.EntityStatus.StatusWeight < allowedStatusWeight);                      
                    }

                    else if (item.FieldName == "PendingDeliveryFilter")
                    {
                        var shipmentObjectTableName = "Shipment";
                        var allStatuses = GetAllStatusesByObjecTableName(shipmentObjectTableName, Tenant);
                        var allowedStatusWeight = allStatuses.FirstOrDefault(a => a.Code == "SDLD")?.StatusWeight;
                        queryableData = queryableData.Include("ShipmentEntityStatus").Where(d => d.ShipmentDeliveryATD != null
                                                                                      && d.ShipmentDeliveryATA == null
                                                                                      && d.ShipmentEntityStatus.StatusWeight < allowedStatusWeight);                       
                    }

                    else if (item.FieldName == "PendingPOLDepartureFilter")
                    {
                        var containerObjectTableName = "Container";
                        var allStatuses = GetAllStatusesByObjecTableName(containerObjectTableName, Tenant);
                        var allowedStatusWeight = allStatuses.FirstOrDefault(a => a.Code == "POLD")?.StatusWeight;
                        DateTime afterTwoDaysDate = todayDate.AddDays(2);

                        queryableData = queryableData.Where(d => (d.ActualPOLVesselDeparture == null && d.ShipmentMainCarriageATD == null)
                                                              && (d.EstimatedPOLVesselDeparture != null || d.ShipmentMainCarriageETD != null)
                                                              && (d.EstimatedPOLVesselDeparture <= afterTwoDaysDate || (d.EstimatedPOLVesselDeparture == null && d.ShipmentMainCarriageETD <= afterTwoDaysDate))
                                                              && d.EntityStatus.StatusWeight < allowedStatusWeight);                     
                    }

                    else if (item.FieldName == "PreviousContainersFilter")
                    {
                        var newDate = new DateTime(todayDate.Year, todayDate.Month, 1);
                        var start = newDate.AddMonths(-1).Date;
                        var end = newDate.AddDays(-1).Date;

                        queryableData = queryableData.Where(d => d.RequestDate != null
                                                              && d.RequestDate >= start
                                                              && d.RequestDate <= end);                      
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
        private static List<EntityStatusList> GetAllStatusesByObjecTableName(string objectTableName, int tenant)
        {
            var entityStatusQuery = new EntityStatusQuery(tenant);
            var allStatuses = entityStatusQuery.GetEntityStatusByObjectTableNameAndTenant(objectTableName, tenant)
                                               .ToList();
            return allStatuses;
        }

    }
}
