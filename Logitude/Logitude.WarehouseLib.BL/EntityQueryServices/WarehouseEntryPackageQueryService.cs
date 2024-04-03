
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.Data.EntityLists;
using Logitude.WarehouseLib.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Security;
using System.Web;
using Logitude.WarehouseLib.BL.Helpers;
using Logitude.WarehouseLib.BL.DataContracts;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.WarehouseLib.Data.EntityListQueryServices;
using Logitude.WarehouseLib.Data;

namespace Logitude.WarehouseLib.BL.EntityQueryServices
{

    public partial class WarehouseEntryPackageQueryService
    {
        public List<WarehouseEntryPackagePM> GetWarehouseEntryPackagePMListsByCustomerIdIdAndWarehouseId(string customerId, string warehouseId, int tenant)
        {

            List<WarehouseEntryPackagePM> myResult = (from a in context.WarehouseEntryPackages.Include("WarehouseEntry").Include("PackageType")
                                                      where a.Tenant == tenant && a.WarehouseEntry.WarehouseId == warehouseId && a.WarehouseEntry.CustomerId == customerId && a.Instock > 0
                                                      select new WarehouseEntryPackagePM()
                                                      {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          Seal = a.Seal,
                                                          Description = a.Description,
                                                          ContainerNumber = a.ContainerNumber,
                                                          Dimensions = a.PackageType != null && a.PackageType.IsContainer ? "" : a.Length + "-" + a.Width + "-" + a.Height,
                                                          Harmonize = a.Harmonize,
                                                          Height = a.Height,
                                                          Instock = a.Instock,
                                                          ReleaseQTY = a.Instock,
                                                          Length = a.Length,
                                                          PackageTypeName = a.PackageType.EnglishName,
                                                          WarehouseEntryId = a.WarehouseEntryId,
                                                          Width = a.Width,
                                                          PackageTypeId = a.PackageTypeId,
                                                          Volume = a.Volume,
                                                          Quantity = a.Quantity,
                                                          Weight = a.Weight,
                                                          CustomerId = a.WarehouseEntry.CustomerId,
                                                          WarehouseId = a.WarehouseEntry.Id,
                                                          IsContainer = a.IsContainer,
                                                          IsConnectedToShipment = a.IsConnectedToShipment,
                                                          WarehouseEntryNumber = a.WarehouseEntry.EntryNumber,
                                                      }).ToList();
            return myResult;
        }


        public List<WarehouseEntryPackagePM> GetWarehouseEntryPackagePMListsByShipmentIdAndWarehouseIdAndCustomerId(string warehouseId, string customerId, string shipmentId, int tenant)
        {
            if (shipmentId != null && shipmentId != "undefined" && shipmentId != "null")
            {

                List<WarehouseEntryPackagePM> myResult = (from a in context.WarehouseEntryPackages.Include("WarehouseEntry").Include("PackageType")
                                                          where a.Tenant == tenant && a.WarehouseEntry.CustomerId == customerId && a.WarehouseEntry.WarehouseId == warehouseId && a.WarehouseEntry.ShipmentId == shipmentId && a.Instock > 0
                                                          select new WarehouseEntryPackagePM()
                                                          {
                                                              Id = a.Id,
                                                              Tenant = a.Tenant,
                                                              Seal = a.Seal,
                                                              Description = a.Description,
                                                              ContainerNumber = a.ContainerNumber,
                                                              Dimensions = a.IsContainer ? "" : a.Length + "-" + a.Width + "-" + a.Height,
                                                              Harmonize = a.Harmonize,
                                                              Height = a.Height,
                                                              Instock = a.Instock,
                                                              ReleaseQTY = 0,
                                                              InstockTemp = a.Instock,
                                                              Length = a.Length,
                                                              PackageTypeName = a.PackageType.EnglishName,
                                                              WarehouseEntryId = a.WarehouseEntryId,
                                                              Width = a.Width,
                                                              PackageTypeId = a.PackageTypeId,
                                                              Volume = a.Volume,
                                                              Quantity = a.Quantity,
                                                              Weight = a.Weight,
                                                              CustomerId = a.WarehouseEntry.CustomerId,
                                                              WarehouseId = a.WarehouseEntry.WarehouseId,
                                                              ActualEntryDate = a.WarehouseEntry.ActualEntryDate,
                                                              IsContainer = a.IsContainer,
                                                              FromPortId = a.WarehouseEntry.FromPortId,
                                                              ToPortId = a.WarehouseEntry.ToPortId,
                                                              TransportModeId = a.WarehouseEntry.TransportModeId,
                                                              DirectionId = a.WarehouseEntry.DirectionId,
                                                              IsConnectedToShipment = a.IsConnectedToShipment,
                                                              VolumetricWeight = a.VolumetricWeight,
                                                              ChargeableWeightUnitCode = a.WarehouseEntry.ChargeableWeightUnitCode,
                                                              WarehouseEntryNumber = a.WarehouseEntry.EntryNumber,
                                                          }).ToList();




                foreach (WarehouseEntryPackagePM item in myResult)
                {
                    if (!string.IsNullOrEmpty(item.ContainerNumber))
                    {
                        item.ContainerNumberWarning = ContainerNumberWarehouseValidator.Validate(item.ContainerNumber);
                    }

                }

                return myResult;
            }

            else
            {


                List<WarehouseEntryPackagePM> myResult = (from a in context.WarehouseEntryPackages.Include("WarehouseEntry").Include("PackageType")
                                                          where a.Tenant == tenant && a.WarehouseEntry.CustomerId == customerId && a.WarehouseEntry.WarehouseId == warehouseId && a.Instock > 0
                                                          select new WarehouseEntryPackagePM()
                                                          {
                                                              Id = a.Id,
                                                              Tenant = a.Tenant,
                                                              Seal = a.Seal,
                                                              Description = a.Description,
                                                              ContainerNumber = a.ContainerNumber,
                                                              Dimensions = a.IsContainer ? "" : a.Length + "-" + a.Width + "-" + a.Height,
                                                              Harmonize = a.Harmonize,
                                                              Height = a.Height,
                                                              Instock = a.Instock,
                                                              ReleaseQTY = 0,
                                                              InstockTemp = a.Instock,
                                                              Length = a.Length,
                                                              PackageTypeName = a.PackageType.EnglishName,
                                                              WarehouseEntryId = a.WarehouseEntryId,
                                                              Width = a.Width,
                                                              PackageTypeId = a.PackageTypeId,
                                                              Volume = a.Volume,
                                                              Quantity = a.Quantity,
                                                              Weight = a.Weight,
                                                              CustomerId = a.WarehouseEntry.CustomerId,
                                                              WarehouseId = a.WarehouseEntry.WarehouseId,
                                                              ActualEntryDate = a.WarehouseEntry.ActualEntryDate,
                                                              IsContainer = a.IsContainer,
                                                              FromPortId = a.WarehouseEntry.FromPortId,
                                                              ToPortId = a.WarehouseEntry.ToPortId,
                                                              TransportModeId = a.WarehouseEntry.TransportModeId,
                                                              DirectionId = a.WarehouseEntry.DirectionId,
                                                              IsConnectedToShipment = a.IsConnectedToShipment,
                                                              VolumetricWeight = a.VolumetricWeight,
                                                              ChargeableWeightUnitCode = a.WarehouseEntry.ChargeableWeightUnitCode,
                                                              WarehouseEntryNumber = a.WarehouseEntry.EntryNumber,
                                                          }).ToList();




                foreach (WarehouseEntryPackagePM item in myResult)
                {
                    if (!string.IsNullOrEmpty(item.ContainerNumber))
                    {
                        item.ContainerNumberWarning = ContainerNumberWarehouseValidator.Validate(item.ContainerNumber);
                    }

                }

                return myResult;

            }
        }




        public List<WarehouseEntryPackageItem> GetWarehouseEntryPackageItemForInventoryReport(WarehouseEntryPackageArgs warehouseEntryPackageArgs)
        {
            List<WarehouseEntryPackageItem> myResult = new List<WarehouseEntryPackageItem>();

            IQueryable<WarehouseEntryPackageItem> warehouseEntryPackageItemLists = (from a in context.WarehouseEntryPackages.Include("WarehouseEntry").Include("WarehouseEntry.Customer").Include("WarehouseEntry.Warehouse").Include("PackageType")
                                                                                    where a.Tenant == warehouseEntryPackageArgs.Tenant && a.WarehouseEntry != null && a.WarehouseEntry.ActualEntryDate != null && a.Instock>0
                                                                                    select new WarehouseEntryPackageItem()
                                                                                    {
                                                                                        WarehouseEntryPackagId = a.Id,
                                                                                        WarehouseId = a.WarehouseEntry != null ? a.WarehouseEntry.WarehouseId : "",
                                                                                        CustomerId = a.WarehouseEntry != null ? a.WarehouseEntry.CustomerId : "",
                                                                                        WarehouseName = a.WarehouseEntry != null ? a.WarehouseEntry.Warehouse != null ? a.WarehouseEntry.Warehouse.Card != null ? a.WarehouseEntry.Warehouse.Card.EnglishName : "" : "" : "",
                                                                                        CustomerReference = a.WarehouseEntry.CustomerRef1 + (!string.IsNullOrEmpty(a.WarehouseEntry.CustomerRef1) ? !string.IsNullOrEmpty(a.WarehouseEntry.CustomerRef2) ? " , " : "" : "") + a.WarehouseEntry.CustomerRef2,
                                                                                        Description = a.Description,
                                                                                        EntryNumber = a.WarehouseEntry != null ? a.WarehouseEntry.EntryNumber : "",
                                                                                        Dimensions = a.IsContainer ? "" : a.Length + "-" + a.Width + "-" + a.Height,
                                                                                        QuantityNotRelease = a.Instock,
                                                                                        PackageType = a.PackageType != null ? a.PackageType.EnglishName : "",
                                                                                        ActualEntryDate = a.WarehouseEntry.ActualEntryDate,
                                                                                        HouseNumber = a.WarehouseEntry != null ? a.WarehouseEntry.HouseNumber : "",
                                                                                        MasterNumber = a.WarehouseEntry != null ? a.WarehouseEntry.MasterNumber : "",
                                                                                        ContainerNumber = a.ContainerNumber,
                                                                                        ShipmentId = a.WarehouseEntry != null ? a.WarehouseEntry.ShipmentId : "",
                                                                                        Location = a.Location,
                                                                                        ShipperId = a.WarehouseEntry != null ? a.WarehouseEntry.ShipperId : "",
                                                                                        ConsigneeId = a.WarehouseEntry != null ? a.WarehouseEntry.ConsigneeId : "",
                                                                                        DirectionId = a.WarehouseEntry != null ? a.WarehouseEntry.DirectionId : "",
                                                                                        GrossWeight = a.Weight,
                                                                                        Volume = a.Volume,
                                                                                        Quantity = a.Quantity,
                                                                                        InternalNotes = a.WarehouseEntry.Notes,
                                                                                        SpecialInstructions = a.WarehouseEntry.SpecialInstruction,
                                                                                        VolumetricWeight = a.VolumetricWeight,
                                                                                        EntryReference = a.WarehouseEntry != null ? a.WarehouseEntry.EntryReference : "",
                                                                                        Commodity = a.CommodityNumber,
                                                                                        ShipmentNumber = a.WarehouseEntry.ShipmentNumber,
                                                                                    });



            if (!string.IsNullOrEmpty(warehouseEntryPackageArgs.ShipmentId)) warehouseEntryPackageItemLists = warehouseEntryPackageItemLists.Where(d => d.ShipmentId == warehouseEntryPackageArgs.ShipmentId);
            if (!string.IsNullOrEmpty(warehouseEntryPackageArgs.CustomerId)) warehouseEntryPackageItemLists = warehouseEntryPackageItemLists.Where(d => d.CustomerId == warehouseEntryPackageArgs.CustomerId);
            if (!string.IsNullOrEmpty(warehouseEntryPackageArgs.WarehouseId)) warehouseEntryPackageItemLists = warehouseEntryPackageItemLists.Where(d => d.WarehouseId == warehouseEntryPackageArgs.WarehouseId);
            if (!string.IsNullOrEmpty(warehouseEntryPackageArgs.ShipperConsigneesId)) warehouseEntryPackageItemLists = warehouseEntryPackageItemLists.Where(d => d.ConsigneeId == warehouseEntryPackageArgs.ShipperConsigneesId || d.ShipperId == warehouseEntryPackageArgs.ShipperConsigneesId);
            myResult = warehouseEntryPackageItemLists.ToList();

      
            #region Fill Prop

            #region ShipmentsLists
            List<ShipmentPM> shipmentLists = null;
            List<string> shipmentids = myResult.GroupBy(d => d.ShipmentId).Select(d => d.First().ShipmentId).ToList();
            if (shipmentids.Count > 0)
            {
                ShipmentQuery shipmentQuery = new ShipmentQuery(warehouseEntryPackageArgs.Tenant);
                shipmentLists = shipmentQuery.GetShipmentsForInventoryReport(shipmentids);
            }

            #endregion

            #region cardLists
            List<string> cardIds = myResult.GroupBy(d => d.CustomerId).Select(d => d.First().CustomerId).ToList();
            foreach (WarehouseEntryPackageItem item in myResult)
            {
                if (item.DirectionId == "I" || item.DirectionId == "C")
                {
                    if (!cardIds.Contains(item.ShipperId)) cardIds.Add(item.ShipperId);
                }
                else if (!cardIds.Contains(item.ConsigneeId)) cardIds.Add(item.ConsigneeId);

            }



            List<CardList> cardLists = null;
            CardQuery cardQuery = new CardQuery(warehouseEntryPackageArgs.Tenant);
            if (cardIds.Count > 0) cardLists = cardQuery.GetCardListsByCardIds(cardIds, warehouseEntryPackageArgs.Tenant);
            #endregion

            foreach (WarehouseEntryPackageItem item in myResult)
            {
                if (item.ActualEntryDate != null)
                {
                    item.DaysInWarehouse = DaysBetween(TenantServerConfigration.GetCurrentDateTime(warehouseEntryPackageArgs.Tenant), (DateTime)item.ActualEntryDate);
                }

                if (!string.IsNullOrEmpty(item.CustomerId) && cardLists != null)
                {
                    CardList cardList = cardLists.Where(d => d.Id == item.CustomerId).FirstOrDefault();
                    if (cardList != null) item.CustomerName = cardList.EnglishName;

                }

                if (!string.IsNullOrEmpty(item.ShipperId) || !string.IsNullOrEmpty(item.ConsigneeId))
                {
                    CardList cardList = null;
                    if (item.DirectionId == "I" || item.DirectionId == "C")
                    {
                        cardList = cardLists.Where(d => d.Id == item.ShipperId).FirstOrDefault();
                    }
                    else cardList = cardLists.Where(d => d.Id == item.ConsigneeId).FirstOrDefault();

                    if (cardList != null) item.ShipperConsignee = cardList.EnglishName;


                }


                if (!string.IsNullOrEmpty(item.ShipmentId))
                {
                    ShipmentPM shipmentPM = shipmentLists.Where(d => d.Id == item.ShipmentId).FirstOrDefault();
                    if (shipmentPM != null)
                    {
                        item.ShipmentNumber = shipmentPM.ShipmentNumber;
                    }
                }

            }

            myResult = ApplyDaysInWarehouseFilters(myResult, warehouseEntryPackageArgs);
            #endregion

            return myResult;


        }

        private List<WarehouseEntryPackageItem> ApplyDaysInWarehouseFilters(List<WarehouseEntryPackageItem> warehouseEntryPackageItemLists, WarehouseEntryPackageArgs warehouseEntryPackageArgs)
        {
            List<WarehouseEntryPackageItem> myResult = warehouseEntryPackageItemLists;
            if (myResult.Count() > 0 && !string.IsNullOrEmpty(warehouseEntryPackageArgs.DaysInWarehouseOperatorFilterValue))
            {
                switch (warehouseEntryPackageArgs.DaysInWarehouseOperatorFilterValue)
                {
                    case "Equals":
                        myResult = myResult.Where(d => d.DaysInWarehouse == warehouseEntryPackageArgs.DaysInWarehouseValue).ToList();
                        break;
                    case "NotEquals":
                        myResult = myResult.Where(d => d.DaysInWarehouse != warehouseEntryPackageArgs.DaysInWarehouseValue).ToList();
                        break;
                    case "GreaterThan":
                        myResult = myResult.Where(d => d.DaysInWarehouse > warehouseEntryPackageArgs.DaysInWarehouseValue).ToList();
                        break;
                    case "Lessthan":
                        myResult = myResult.Where(d => d.DaysInWarehouse < warehouseEntryPackageArgs.DaysInWarehouseValue).ToList();
                        break;
                    case "GreaterThanOREqualTo":
                        myResult = myResult.Where(d => d.DaysInWarehouse >= warehouseEntryPackageArgs.DaysInWarehouseValue).ToList();
                        break;
                    case "LessThanOrEqualTo":
                        myResult = myResult.Where(d => d.DaysInWarehouse <= warehouseEntryPackageArgs.DaysInWarehouseValue).ToList();
                        break;
                }
            }
            return myResult;
        }

        public List<string> GetWarehouseEntryIds(string customerId, string warehouseId, int tenant)
        {
            List<string> ss = (from a in context.WarehouseEntryPackages
                               where a.Tenant == tenant && a.WarehouseEntry != null && a.WarehouseEntry.ActualEntryDate != null
                               select a.ContainerNumber).ToList();
            return ss;



        }





        public int DaysBetween(DateTime createdate, DateTime actualEntryDate)
        {
            if (actualEntryDate != null)
            {
                TimeSpan span = createdate.Subtract(actualEntryDate);
                return (int)span.TotalDays;

            }
            else return 0;

        }


    }

  

}
