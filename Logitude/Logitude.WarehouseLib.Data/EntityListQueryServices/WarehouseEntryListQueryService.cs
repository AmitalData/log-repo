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
            string objcetTableId = new ObjectTableRepository(0).GetObjectTableIdByName("WarehouseEntry");

            IQueryable<WarehouseEntryList> query = (from a in iQueryable
                                                    join customFieldsMainObject in context.CustomFieldsMainObjects.Where(d => d.ObjectTableId == objcetTableId) on a.Id equals customFieldsMainObject.EntityId into customFieldsMainObjectJoin
                                                    from customFieldsMainObject in customFieldsMainObjectJoin.DefaultIfEmpty()
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
                                                        Origin = a.FromTypeCode == "PORT" ? a.FromPort != null ? a.FromPort.EnglishName : "" : a.FromCountry != null ? a.FromCountry.EnglishName : "",
                                                        Destination = a.ToTypeCode == "PORT" ? a.ToPort != null ? a.ToPort.EnglishName : "" : a.ToCountry != null ? a.ToCountry.EnglishName : "",
                                                        CustomerName = a.Customer != null ? a.Customer.EnglishName : "",
                                                        WarehouseName = a.Warehouse != null ? a.Warehouse.Card != null ? a.Warehouse.Card.EnglishName : "" : "",
                                                        StatusName = a.WarehouseEntryStatus != null ? a.WarehouseEntryStatus.Name : "",
                                                        ShipperId = a.ShipperId,
                                                        ConsigneeId = a.ConsigneeId,
                                                        ShipperReference1 = a.ShipperReference1,
                                                        ShipperReference2 = a.ShipperReference2,
                                                        ConsigneeReference1 = a.ConsigneeReference1,
                                                        ConsigneeReference2 = a.ConsigneeReference2,
                                                        ShipmentId = a.ShipmentId,
                                                        ShipperName = a.Shipper != null ? a.Shipper.EnglishName : "",
                                                        ConsigneeName = a.ConsigneeCard != null ? a.ConsigneeCard.EnglishName : "",
                                                        DirectionName = a.Direction != null ? a.Direction.Name : "",
                                                        TransportModeName = a.TransportMode != null ? a.TransportMode.Name : "",
                                                        MasterShipmentNumber = a.MasterShipmentNumber,
                                                        Notes = a.Notes,
                                                        Field1 = customFieldsMainObject != null ? customFieldsMainObject.Field1 : null,
                                                        Field2 = customFieldsMainObject != null ? customFieldsMainObject.Field2 : null,
                                                        Field3 = customFieldsMainObject != null ? customFieldsMainObject.Field3 : null,
                                                        Field4 = customFieldsMainObject != null ? customFieldsMainObject.Field4 : null,
                                                        Field5 = customFieldsMainObject != null ? customFieldsMainObject.Field5 : null,
                                                        Field6 = customFieldsMainObject != null ? customFieldsMainObject.Field6 : null,
                                                        Field7 = customFieldsMainObject != null ? customFieldsMainObject.Field7 : null,
                                                        Field8 = customFieldsMainObject != null ? customFieldsMainObject.Field8 : null,
                                                        Field9 = customFieldsMainObject != null ? customFieldsMainObject.Field9 : null,
                                                        Field10 = customFieldsMainObject != null ? customFieldsMainObject.Field10 : null,
                                                        Field11 = customFieldsMainObject != null ? customFieldsMainObject.Field11 : null,
                                                        Field12 = customFieldsMainObject != null ? customFieldsMainObject.Field12 : null,
                                                        Field13 = customFieldsMainObject != null ? customFieldsMainObject.Field13 : null,
                                                        Field14 = customFieldsMainObject != null ? customFieldsMainObject.Field14 : null,
                                                        Field15 = customFieldsMainObject != null ? customFieldsMainObject.Field15 : null,
                                                        Field16 = customFieldsMainObject != null ? customFieldsMainObject.Field16 : null,
                                                        Field17 = customFieldsMainObject != null ? customFieldsMainObject.Field17 : null,
                                                        Field18 = customFieldsMainObject != null ? customFieldsMainObject.Field18 : null,
                                                        Field19 = customFieldsMainObject != null ? customFieldsMainObject.Field19 : null,
                                                        Field20 = customFieldsMainObject != null ? customFieldsMainObject.Field20 : null,
                                                        Field21 = customFieldsMainObject != null ? customFieldsMainObject.Field21 : null,
                                                        Field22 = customFieldsMainObject != null ? customFieldsMainObject.Field22 : null,
                                                        Field23 = customFieldsMainObject != null ? customFieldsMainObject.Field23 : null,
                                                        Field24 = customFieldsMainObject != null ? customFieldsMainObject.Field24 : null,
                                                        Field25 = customFieldsMainObject != null ? customFieldsMainObject.Field25 : null,
                                                        Field26 = customFieldsMainObject != null ? customFieldsMainObject.Field26 : null,
                                                        Field27 = customFieldsMainObject != null ? customFieldsMainObject.Field27 : null,
                                                        Field28 = customFieldsMainObject != null ? customFieldsMainObject.Field28 : null,
                                                        Field29 = customFieldsMainObject != null ? customFieldsMainObject.Field29 : null,
                                                        Field30 = customFieldsMainObject != null ? customFieldsMainObject.Field30 : null,
                                                        Field31 = customFieldsMainObject != null ? customFieldsMainObject.Field31 : null,
                                                        Field32 = customFieldsMainObject != null ? customFieldsMainObject.Field32 : null,
                                                        Field33 = customFieldsMainObject != null ? customFieldsMainObject.Field33 : null,
                                                        Field34 = customFieldsMainObject != null ? customFieldsMainObject.Field34 : null,
                                                        Field35 = customFieldsMainObject != null ? customFieldsMainObject.Field35 : null,
                                                        Field36 = customFieldsMainObject != null ? customFieldsMainObject.Field36 : null,
                                                        Field37 = customFieldsMainObject != null ? customFieldsMainObject.Field37 : null,
                                                        Field38 = customFieldsMainObject != null ? customFieldsMainObject.Field38 : null,
                                                        Field39 = customFieldsMainObject != null ? customFieldsMainObject.Field39 : null,
                                                        Field40 = customFieldsMainObject != null ? customFieldsMainObject.Field40 : null,
                                                        Field41 = customFieldsMainObject != null ? customFieldsMainObject.Field41 : null,
                                                        Field42 = customFieldsMainObject != null ? customFieldsMainObject.Field42 : null,
                                                        Field43 = customFieldsMainObject != null ? customFieldsMainObject.Field43 : null,
                                                        Field44 = customFieldsMainObject != null ? customFieldsMainObject.Field44 : null,
                                                        Field45 = customFieldsMainObject != null ? customFieldsMainObject.Field45 : null,
                                                        Field46 = customFieldsMainObject != null ? customFieldsMainObject.Field46 : null,
                                                        Field47 = customFieldsMainObject != null ? customFieldsMainObject.Field47 : null,
                                                        Field48 = customFieldsMainObject != null ? customFieldsMainObject.Field48 : null,
                                                        Field49 = customFieldsMainObject != null ? customFieldsMainObject.Field49 : null,
                                                        Field50 = customFieldsMainObject != null ? customFieldsMainObject.Field50 : null,
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
	