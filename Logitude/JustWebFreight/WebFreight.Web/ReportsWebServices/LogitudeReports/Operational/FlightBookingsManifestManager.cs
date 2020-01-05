using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Operational
{
    public class FlightBookingsManifestManager
    {
        private int tenant;
        private DateTime fromDate;
        private DateTime toDate;
        private string flightNumber = null;
        private string mainCarriageFromPortId = null;
        private string mainCarriageFinalDestinationPortId = null;
        private string clearingAgentId = null;
        private PortRepository portRepository;
        private CardRepository cardRepository;

        public FlightBookingsManifestManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            this.portRepository = new PortRepository(commonDataContext);
            this.cardRepository = new CardRepository(commonDataContext);

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations myQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_FromDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_FlightNumber = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FlightNumber").FirstOrDefault();
            QueryFilterItem filterItem_FromPortId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "MainCarriageFromPortId").FirstOrDefault();
            QueryFilterItem filterItem_FinalDestinationPortId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "MainCarriageFinalDestinationPortId").FirstOrDefault();
            QueryFilterItem filterItem_ClearingAgentId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ClearingAgentId").FirstOrDefault();

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out fromDate);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out toDate);
            }

            if (filterItem_FlightNumber != null)
            {
                if (filterItem_FlightNumber.FieldValue != null)
                {
                    flightNumber = filterItem_FlightNumber.FieldValue.ToString();
                }
            }

            if (filterItem_FromPortId != null)
            {
                if (filterItem_FromPortId.FieldValue != null)
                {
                    mainCarriageFromPortId = filterItem_FromPortId.FieldValue.ToString();
                }
            }

            if (filterItem_FinalDestinationPortId != null)
            {
                if (filterItem_FinalDestinationPortId.FieldValue != null)
                {
                    mainCarriageFinalDestinationPortId = filterItem_FinalDestinationPortId.FieldValue.ToString();
                }
            }

            if (filterItem_ClearingAgentId != null)
            {
                if (filterItem_ClearingAgentId.FieldValue != null)
                {
                    clearingAgentId = filterItem_ClearingAgentId.FieldValue.ToString();
                }
            }
        }

        public byte[] GetData()
        {
            FlightBookingsManifestDataProvider myDataProvider = this.LoadDataProvider();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FlightBookingsManifestDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private FlightBookingsManifestDataProvider LoadDataProvider()
        {
            FlightBookingsManifestDataProvider myDataProvider = new FlightBookingsManifestDataProvider();

            IQueryable<ShipmentJoinPackageList> shipmentPackageList = this.BuildShipmentPackageList();
            List<ShipmentJoinPackageList> filteredShipmentPackageList = this.Filter(shipmentPackageList);

            CustomFieldResolver customFieldResolver = new CustomFieldResolver();

            if (filteredShipmentPackageList != null && filteredShipmentPackageList.Count > 0)
            {
                List<ReportGroupData> myDataList = new List<ReportGroupData>();
                foreach (ShipmentJoinPackageList shipmentPackage in filteredShipmentPackageList)
                {
                    ReportGroupData myDataRecord = new ReportGroupData();
                    myDataRecord.House = shipmentPackage.House;
                    myDataRecord.CommodityNumber = shipmentPackage.CommodityNumber;
                    myDataRecord.Master = shipmentPackage.MasterNumber;
                    myDataRecord.Shipper = shipmentPackage.ShipperName;
                    myDataRecord.Consignee = shipmentPackage.ConsigneeName;
                    myDataRecord.Quantity = shipmentPackage.PackageQuantity;
                    myDataRecord.Weight = shipmentPackage.PackagesGrossWeight;
                    myDataRecord.Volume = shipmentPackage.PackageVolume;
                    myDataRecord.VolumetricWeight = shipmentPackage.PackageVolumeitricWeight;
                    myDataRecord.MoveType = shipmentPackage.MoveTypeName;
                    myDataRecord.CustomAgentImportId = shipmentPackage.CustomAgentImportId;
                    myDataRecord.CustomAgentImportName = shipmentPackage.CustomAgentImportName;
                    myDataRecord.PackageReference4 = shipmentPackage.ShipmentPackageReference4;

                    customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipmentPackage, myDataRecord);

                    myDataList.Add(myDataRecord);
                }

                if (myDataList.Count > 0)
                {
                    List<ReportGroup> commodityAgentResults = (from p in myDataList
                                                               group p by new { p.CommodityNumber, p.CustomAgentImportId, p.CustomAgentImportName } 
                                                               into g
                                                               orderby g.Key.CommodityNumber 
                                                               select new ReportGroup()
                                                               {
                                                                   CustomAgentImportId = g.Key.CustomAgentImportId,
                                                                   CustomAgentImportName = g.Key.CustomAgentImportName,
                                                                   CommodityNumber = g.Key.CommodityNumber,
                                                                   ReportGroupDataList = g.ToList(),
                                                               }).ToList();

                    List<ReportGroup> masterCommodityAgentResults = (from p in myDataList
                                                                     group p by new { p.Master, p.CommodityNumber, p.CustomAgentImportId, p.CustomAgentImportName } 
                                                                     into g
                                                                     orderby g.Key.Master
                                                                     select new ReportGroup()
                                                                     {
                                                                         MasterNumber = g.Key.Master,
                                                                         CustomAgentImportId = g.Key.CustomAgentImportId,
                                                                         CustomAgentImportName = g.Key.CustomAgentImportName,
                                                                         CommodityNumber = g.Key.CommodityNumber,
                                                                         ReportGroupDataList = g.ToList(),
                                                                     }).ToList();

                    List<ReportGroup> reference4Results = (from p in myDataList
                                                           group p by new { p.PackageReference4 } 
                                                           into g
                                                           orderby g.Key.PackageReference4
                                                           select new ReportGroup()
                                                           {
                                                               Reference4 = g.Key.PackageReference4,
                                                               ReportGroupDataList = g.ToList(),
                                                           }).ToList();

                    myDataProvider.CommodityAgentGroupList = commodityAgentResults.OrderBy(d => d.CommodityNumber).ToList();
                    myDataProvider.MasterCommodityAgentGroupList = masterCommodityAgentResults.OrderBy(d => d.MasterNumber).ToList();
                    myDataProvider.Reference4GroupList = reference4Results.OrderBy(d => d.Reference4).ToList();
                }
            }

            myDataProvider.FromDate = fromDate;
            myDataProvider.ToDate = toDate;
            myDataProvider.FlightNumber = flightNumber;

            if(!string.IsNullOrEmpty(mainCarriageFromPortId))
            {
                Simplog.Data.CommonDataModel.EntityPOCOs.Port fromPort = portRepository.GetSinglePort(mainCarriageFromPortId, tenant);
                if(fromPort != null)
                {
                    myDataProvider.Origin = fromPort.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(mainCarriageFinalDestinationPortId))
            {
                Simplog.Data.CommonDataModel.EntityPOCOs.Port finalPort = portRepository.GetSinglePort(mainCarriageFinalDestinationPortId, tenant);
                if (finalPort != null)
                {
                    myDataProvider.Destination = finalPort.EnglishName;
                }
            }

            if (!string.IsNullOrEmpty(clearingAgentId))
            {
                Simplog.Data.CommonDataModel.EntityPOCOs.Card clearingAgent = cardRepository.GetSingleCard(clearingAgentId, tenant);
                if (clearingAgent != null)
                {
                    myDataProvider.CustomAgent = clearingAgent.EnglishName;
                }
            }

            return myDataProvider;
        }

        private IQueryable<ShipmentJoinPackageList> BuildShipmentPackageList()
        {
            IShipmentsContext context = ShipmentsContext.GetContext(tenant);
            IQueryable<ShipmentJoinPackageList> dataList =
                (from shipment in context.Shipments.Include("MoveType").Include("CustomAgentImportCard")
                 join shipmentPackage in context.ShipmentPackages.Include("PackageType")
                 on shipment.Id equals shipmentPackage.ShipmentId into JoinedData
                 join masterData in context.ShipmentMasterDatas.Include("MainCarriageFromPort").Include("MainCarriageFinalDestinationPort")
                 on shipment.MasterShipmentDataId equals masterData.Id into shipmentJoin
                 from master in shipmentJoin.DefaultIfEmpty()
                 from package in JoinedData
                 where shipment.Tenant == tenant && shipment.TransportModeId == "A" && shipment.ShipmentLevelCode != "C"
                 select new ShipmentJoinPackageList()
                 {
                     Id = shipment.Id + (!string.IsNullOrEmpty(package.Id) ? package.Id : ""),

                     //Shipment
                     ShipmentId = shipment.Id,
                     DirectionId = shipment.DirectionId,
                     ShipmentNumber = shipment.ShipmentNumber,
                     CreateDateTime = shipment.CreateDateTime,
                     ShipperName = shipment.ShipperName,
                     ConsigneeName = shipment.ConsigneeName,
                     CustomAgentImportId = shipment.CustomAgentImportId,
                     CustomAgentImportName = shipment.CustomAgentImportCard == null ? null : shipment.CustomAgentImportCard.EnglishName,
                     House = shipment.House,
                     MoveTypeName = shipment.MoveType == null ? null : shipment.MoveType.MoveTypeEnglishName,
                     Field1 = shipment.Field1,
                     Field2 = shipment.Field2,
                     Field3 = shipment.Field3,
                     Field4 = shipment.Field4,
                     Field5 = shipment.Field5,
                     Field6 = shipment.Field6,
                     Field7 = shipment.Field7,
                     Field8 = shipment.Field8,
                     Field9 = shipment.Field9,
                     Field10 = shipment.Field10,
                     Field11 = shipment.Field11,
                     Field12 = shipment.Field12,
                     Field13 = shipment.Field13,
                     Field14 = shipment.Field14,
                     Field15 = shipment.Field15,
                     Field16 = shipment.Field16,
                     Field17 = shipment.Field17,
                     Field18 = shipment.Field18,
                     Field19 = shipment.Field19,
                     Field20 = shipment.Field20,
                     Field21 = shipment.Field21,
                     Field22 = shipment.Field22,
                     Field23 = shipment.Field23,
                     Field24 = shipment.Field24,
                     Field25 = shipment.Field25,
                     Field26 = shipment.Field26,
                     Field27 = shipment.Field27,
                     Field28 = shipment.Field28,
                     Field29 = shipment.Field29,
                     Field30 = shipment.Field30,
                     Field31 = shipment.Field31,
                     Field32 = shipment.Field32,
                     Field33 = shipment.Field33,
                     Field34 = shipment.Field34,
                     Field35 = shipment.Field35,
                     Field36 = shipment.Field36,
                     Field37 = shipment.Field37,
                     Field38 = shipment.Field38,
                     Field39 = shipment.Field39,
                     Field40 = shipment.Field40,

                     //Master
                     MainCarriageFromPortId = master.MainCarriageFromPortId,
                     MainCarriageFromPortName = master.MainCarriageFromPort != null ? master.MainCarriageFromPort.EnglishName : null,
                     MainCarriageFinalDestinationPortId = master.MainCarriageFinalDestinationPortId,
                     MainCarriageFinalDestinationPortName = master.MainCarriageFinalDestinationPort != null ? master.MainCarriageFinalDestinationPort.EnglishName : null,
                     MasterNumber = master.Master,
                     MainCarriageCarrierCode = master.MainCarriageCarrierPrefix,
                     MainCarriageCarrierNumber = master.MainCarriageCarrierNumber,
                     MainCarriageETD = master.MainCarriageETD,
                     MainCarriageATD = master.MainCarriageATD,

                     //Package
                     PackageId = package.Id,
                     CommodityNumber = package.CommodityNumber,
                     PackageQuantity = package.Quantity,
                     PackagesGrossWeight = package.Weight,
                     PackageVolume = package.Volume,
                     PackageVolumeitricWeight = package.VolumetricWeight,
                     ShipmentPackageReference1 = package.Reference1,
                     ShipmentPackageReference2 = package.Reference2,
                     ShipmentPackageReference3 = package.Reference3,
                     ShipmentPackageReference4 = package.Reference4,
                 });

            return dataList;
        }
        private List<ShipmentJoinPackageList> Filter(IQueryable<ShipmentJoinPackageList> shipmentPackageList)
        {
            if (fromDate != null)
            {
                shipmentPackageList = (from d in shipmentPackageList
                                       where
                             (d.MainCarriageETD != null && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate))
                             ||
                             (d.MainCarriageATD != null && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate))
                                       select d);
            }

            if (toDate != null)
            {
                shipmentPackageList = (from d in shipmentPackageList
                                       where
                             (d.MainCarriageETD != null && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) <= System.Data.Entity.DbFunctions.TruncateTime(toDate))
                             ||
                             (d.MainCarriageATD != null && System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageATD) <= System.Data.Entity.DbFunctions.TruncateTime(toDate))
                                       select d);
            }

            if (!string.IsNullOrEmpty(mainCarriageFromPortId))
            {
                shipmentPackageList = shipmentPackageList.Where(d => d.MainCarriageFromPortId == mainCarriageFromPortId);
            }

            if (!string.IsNullOrEmpty(mainCarriageFinalDestinationPortId))
            {
                shipmentPackageList = shipmentPackageList.Where(d => d.MainCarriageFinalDestinationPortId == mainCarriageFinalDestinationPortId);
            }

            if (!string.IsNullOrEmpty(clearingAgentId))
            {
                shipmentPackageList = shipmentPackageList.Where(d => d.CustomAgentImportId == clearingAgentId);
            }

            if (!string.IsNullOrEmpty(flightNumber))
            {
                shipmentPackageList = shipmentPackageList.Where(d => (d.MainCarriageCarrierCode + d.MainCarriageCarrierNumber) == flightNumber);
            }

            return shipmentPackageList.ToList();
        }
    }
}