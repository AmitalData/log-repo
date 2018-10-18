using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.EntityLists;
using Logitude.WarehouseLib.Data.CustomFilters;

namespace Logitude.WarehouseLib.Data.EntityListQueryServices
{

    public partial class WarehouseEntryListQueryService
    {
        private IQueryable<WarehouseEntryList> GetIqueryableList(IQueryable<WarehouseEntry> iQueryable)
        {
            IQueryable<WarehouseEntryList> query = (from a in iQueryable
                                                    select new WarehouseEntryList()
                                                    {

                                                        Id = a.Id,
                                                        Tenant = a.Tenant,
                                                        CreateDate = a.CreateDate,
                                                        CreatedByUserId = a.CreatedByUserId,
                                                        UpdateDate = a.UpdateDate,
                                                        UpdatedByUserId = a.UpdatedByUserId,
                                                        ShipmentNumber = a.ShipmentNumber,
                                                        ExpectedEntryDate = a.ExpectedEntryDate,
                                                        TotalPieces = a.TotalPieces,
                                                        TotalGrossWeight = a.TotalGrossWeight,
                                                        GrossWeightUnitCode = a.GrossWeightUnitCode,
                                                        TotalVolume = a.TotalVolume,
                                                        EntryNumber = a.EntryNumber,
                                                        EntryReference = a.EntryReference,
                                                        ActualEntryDate = a.ActualEntryDate,
                                                        WarehouseId = a.WarehouseId,
                                                        StatusCode = a.StatusCode,
                                                        HouseNumber = a.HouseNumber,
                                                        MasterNumber = a.MasterNumber,
                                                        CustomerId = a.CustomerId,
                                                        ReceivedBy = a.ReceivedBy,
                                                        SpecialInstruction = a.SpecialInstruction,
                                                        DirectionId = a.DirectionId,
                                                        TransportModeId = a.TransportModeId,
                                                        FromPortId = a.FromPortId,
                                                        ToPortId = a.ToPortId,
                                                        Origin = a.FromPort!=null ? a.FromPort.EnglishName:"",
                                                        Destination = a.ToPort != null ? a.ToPort.EnglishName:"",
                                                        CustomerName = a.Customer != null ? a.Customer.EnglishName : "",
                                                        WarehouseName = a.Warehouse != null ? a.Warehouse.Card!=null? a.Warehouse.Card .EnglishName: "" : "",
                                                        StatusName = a.WarehouseEntryStatus != null ? a.WarehouseEntryStatus.Name : "",
                                                        ShipperId = a.ShipperId,
                                                        ConsigneeId = a.ConsigneeId, 
                                                        ShipperReference1 = a.ShipperReference1,
                                                        ShipperReference2 = a.ShipperReference2,
                                                        ConsigneeReference1 =a.ConsigneeReference1,
                                                        ConsigneeReference2=a.ConsigneeReference2,
                                                        ShipmentId = a.ShipmentId, 
                                                        ShipperName = a.Shipper != null ? a.Shipper.EnglishName : "",
                                                        ConsigneeName = a.ConsigneeCard != null ? a.ConsigneeCard.EnglishName : "",
                                                        DirectionName = a.Direction != null ? a.Direction.Name : "",
                                                        TransportModeName = a.TransportMode != null ? a.TransportMode.Name : "",
                                                    });

     

            return query;
        }

        private IQueryable<WarehouseEntry> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<WarehouseEntry> iQueryable, int tenant)
        {
            return WarehouseEntryCustomFilter.GetFilteredQuery(queryOperations, iQueryable, tenant);            
        }
        private IQueryable<WarehouseEntry> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<WarehouseEntry> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	