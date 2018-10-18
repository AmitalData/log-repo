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

    public partial class WarehouseReleaseListQueryService
    {
        private IQueryable<WarehouseReleaseList> GetIqueryableList(IQueryable<WarehouseRelease> iQueryable)
        {
            IQueryable<WarehouseReleaseList> query = (from a in iQueryable
                                                      select new WarehouseReleaseList()
                                                      {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          CreateDate = a.CreateDate,
                                                          CreatedByUserId = a.CreatedByUserId,
                                                          UpdateDate = a.UpdateDate,
                                                          UpdatedByUserId = a.UpdatedByUserId,
                                                          ReleaseNumber = a.ReleaseNumber,
                                                          ShipmentNumber = a.ShipmentNumber,
                                                          ExpectedReleaseDate = a.ExpectedReleaseDate,
                                                          ActualReleaseDate = a.ActualReleaseDate,
                                                          ReleaseBy = a.ReleaseBy,
                                                          SpecialInstruction = a.SpecialInstruction,
                                                          TotalPieces = a.TotalPieces,
                                                          TotalGrossWeight = a.TotalGrossWeight,
                                                          GrossWeightUnitCode = a.GrossWeightUnitCode,
                                                          TotalVolume = a.TotalVolume,
                                                          CustomerName = a.Customer != null ? a.Customer.EnglishName : "",
                                                          WarehouseName = a.Warehouse != null ? a.Warehouse.Card != null ? a.Warehouse.Card.EnglishName : "" : "",
                                                          StatusName = a.WarehouseReleaseStatus != null ? a.WarehouseReleaseStatus.Name : "",
                                                          StatusCode = a.StatusCode,
                                                          References = a.CustomerRef1!= null ? a.CustomerRef2 != null ? a.CustomerRef1 + "," + a.CustomerRef2 : a.CustomerRef1 : a.CustomerRef1,
                                                          ReleaseDate = a.ActualReleaseDate != null ? a.ActualReleaseDate : a.ExpectedReleaseDate,
                                                          Notes = a.Notes,
                                                          HouseNumber = a.HouseNumber,
                                                          MasterNumber = a.MasterNumber,
                                                          WarehouseId = a.WarehouseId,
                                                          ShipmentId = a.ShipmentId,
                                                          CustomerId = a.CustomerId,
                                                          SearchFields = a.SearchFields,
                                                          TransportModeId = a.TransportModeId,
                                                          DirectionId = a.DirectionId,
                                                          TotalQuantity = a.TotalQuantity,
                                                          DirectionName = a.Direction != null ? a.Direction.Name : "",
                                                          TransportModeName = a.TransportMode != null ? a.TransportMode.Name : "",
                                                      });
            return query;
        }

        private IQueryable<WarehouseRelease> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<WarehouseRelease> iQueryable, int tenant)
        {
            return WarehouseReleaseCustomFilter.GetFilteredQuery(queryOperations, iQueryable, tenant);
        }
        private IQueryable<WarehouseRelease> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<WarehouseRelease> iQueryable, int tenant)
        {
            return iQueryable;
        }
    }
}
	