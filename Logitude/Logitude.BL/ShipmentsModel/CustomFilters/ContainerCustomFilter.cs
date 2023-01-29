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
            //bool showIsClosed = false;
            bool showIsCancelled = false;
            foreach (QueryFilterItem item in queryFilters)
            {
                if (item.IsCustom)
                {
                    //if (item.FieldName == "IsClosed")
                    //{
                    //    bool value = Convert.ToBoolean(item.FieldValue);
                    //    if (value)
                    //    {
                    //        showIsClosed = true;
                    //    }
                    //}

                    if (item.FieldName == "Containers")
                    {
                       
                        queryableData = queryableData.Where(d => !d.IsClosed && !d.IsCancelled);
                    }
                    if (item.FieldName == "PendingPOLDepartureFilter")
                    {
                       
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(Tenant).Date;
                        DateTime twoDaysAgoDate = todayDate.AddDays(-2);

                        queryableData = from d in queryableData
                                        where d.ActualPOLVesselDeparture == null 
                                        && d.EstimatedPOLVesselDeparture != null
                                        && (d.EstimatedPOLVesselDeparture <= twoDaysAgoDate)
                                        select d;
                       // queryableData = queryableData.Where(d => d.ActualPODDeparture == null && d.EstimatedPOLVesselDeparture != null && d.ActualPODVesselArrival == null);
                    }
                    if (item.FieldName == "InTransitFilter")
                    {
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(Tenant).Date;
                        queryableData = queryableData.Where(d => d.ActualPOLVesselDeparture != null && d.TransshipmentCount == 0 && d.ActualPODVesselArrival == null && d.EstimatedPODVesselArrival != todayDate);
                        //queryableData = queryableData.Where(d => d.ActualPOLVesselDeparture != null && (d.TransshipmentCount == 0 || d.TransshipmentCount == null) && d.ActualPODVesselArrival == null);
                    }
                    if (item.FieldName == "InTransitwithTransshipmentsFilter")
                    {
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(Tenant).Date;
                        queryableData = queryableData.Where(d => d.ActualPOLVesselDeparture != null && (d.TransshipmentCount > 0 ) && d.ActualPODVesselArrival == null && d.EstimatedPODVesselArrival == todayDate);
                    }
                    if (item.FieldName == "PendingGateOutFilter")
                    {
                        queryableData = queryableData.Where(d => d.ActualPODVesselArrival != null &&  d.GateOut == null && d.ActualPODDischarge != null);
                    }
                    if (item.FieldName == "PendingEmptyReturnFilter")
                    {
                        queryableData = queryableData.Where(d => d.GateOut != null &&  d.ActualEmptyReturn == null);
                    }

                    if (item.FieldName == "ClosedContainers")
                    {
                        queryableData = queryableData.Where(d => d.IsClosed);
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

            //if (showIsCancelled)
            //{
                queryableData = queryableData.Where(d => d.IsCancelled == showIsCancelled);
            //}
            //else
            //if (showIsClosed)
            //{
            //    queryableData= queryableData.Where(d => d.IsClosed == showIsClosed && d.IsCancelled == showIsCancelled);
            //}

            return queryableData;
        }
    }
}
