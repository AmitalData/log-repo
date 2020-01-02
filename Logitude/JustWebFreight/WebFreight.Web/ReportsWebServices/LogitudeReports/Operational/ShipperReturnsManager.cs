using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.Operational
{
    public class ShipperReturnsManager
    {
        private int tenant;
        private DateTime? fromDate = null;
        private DateTime? toDate = null;
        private string shipperId = null;
        private string mainCarriageFromPortId;
        private string mainCarriageFinalDestinationPortId;
        private string subshipper;
        private IShipmentsContext shipmentsContext;
        private ShipmentRepository shipmentRepository;
        private CustomFieldResolver customFieldResolver;
        private ShipperReturnsDataProvider iDataProvider;
        private IQueryable<ShipmentJoinPackageList> iQueryable_ShipmentPackages;

        public ShipperReturnsManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            customFieldResolver = new CustomFieldResolver();
            shipmentsContext = ShipmentsContext.GetContext(tenant);
            shipmentRepository = new ShipmentRepository(tenant);
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations iQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
            this.FilterByDates(iQueryOperations);
            this.FilterByShipper(iQueryOperations);
            this.FilterByToFromPorts(iQueryOperations);
        }

        private void FilterByShipper(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_ShipperId = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ShipperId").FirstOrDefault();
            if (filterItem_ShipperId != null)
            {
                if (filterItem_ShipperId.FieldValue != null)
                {
                    shipperId = filterItem_ShipperId.FieldValue.ToString();
                }
            }
            QueryFilterItem filterItem_Subshipper = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "Subshipper").FirstOrDefault();
            if (filterItem_Subshipper != null)
            {
                if (filterItem_Subshipper.FieldValue != null)
                {
                    subshipper = filterItem_Subshipper.FieldValue.ToString();
                }
            }
        }

        private void FilterByToFromPorts(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_FromPort = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "MainCarriageFromPortId").FirstOrDefault();
            if (filterItem_FromPort != null)
            {
                if (filterItem_FromPort.FieldValue != null)
                {
                    mainCarriageFromPortId = filterItem_FromPort.FieldValue.ToString();
                }
            }
            QueryFilterItem filterItem_ToPort = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "MainCarriageFinalDestinationPortId").FirstOrDefault();
            if (filterItem_ToPort != null)
            {
                if (filterItem_ToPort.FieldValue != null)
                {
                    mainCarriageFinalDestinationPortId = filterItem_ToPort.FieldValue.ToString();
                }
            }
        }

        private void FilterByDates(QueryOperations iQueryOperations)
        {
            QueryFilterItem filterItem_FromDate = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = iQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            if (filterItem_FromDate != null)
            {
                if (filterItem_FromDate.FieldValue != null)
                {
                    fromDate = (DateTime)filterItem_FromDate.FieldValue;
                }
            }
            if (filterItem_ToDate != null)
            {
                if (filterItem_ToDate.FieldValue != null)
                {
                    toDate = (DateTime)filterItem_ToDate.FieldValue;
                }
            }
        }

        public byte[] GetData()
        {
            this.LoadDataProvider();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ShipperReturnsDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, iDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private void LoadDataProvider()
        {
            this.iDataProvider = new ShipperReturnsDataProvider()
            {
                ShipperReturnsPackagesList = new List<ShipperReturnsPackagesList>()
            };
            this.BuildReportHeader();
            this.BuildSourceData();
            this.BuildReportData();
        }

        private void BuildReportHeader()
        {
            iDataProvider.FromDate = this.fromDate;
            iDataProvider.ToDate = this.toDate;
            iDataProvider.ShipperId = this.shipperId;
            iDataProvider.MainCarriageFinalDestinationPortId = this.mainCarriageFromPortId;
            iDataProvider.MainCarriageFromPortId = this.mainCarriageFromPortId;
            iDataProvider.Subshipper = this.subshipper;
        }

        private void BuildSourceData()
        {
            this.iQueryable_ShipmentPackages = (from shipment in shipmentRepository.context.Shipments.Include("ShipperCard")
                                                join shipmentPackage in shipmentRepository.context.ShipmentPackages.Include("PackageType")
                                                on shipment.Id equals shipmentPackage.ShipmentId into JoinedData
                                                join sm in shipmentRepository.context.ShipmentMasterDatas.Include("MainCarriageFromPort").Include("MainCarriageToPort").Include("MainCarriageFinalDestinationPort").Include("MainCarriageCarrierCard")
                                                on shipment.MasterShipmentDataId equals sm.Id into shipmentJoin
                                                from jd in JoinedData.DefaultIfEmpty()
                                                from m in shipmentJoin.DefaultIfEmpty()
                                                where shipment.Tenant == tenant && shipment.TransportModeId == "A"
                                                select new ShipmentJoinPackageList()
                                                {
                                                    Id = shipment.Id + (!string.IsNullOrEmpty(jd.Id) ? jd.Id : ""),
                                                    ShipmentId = shipment.Id,
                                                    PackageId = jd.Id,
                                                    DirectionId = shipment.DirectionId,
                                                    ShipmentNumber = shipment.ShipmentNumber,
                                                    CreateDateTime = shipment.CreateDateTime,
                                                    ShipperName = shipment.ShipperCard != null ? shipment.ShipperCard.EnglishName : null,
                                                    ShipperId = shipment.ShipperId,
                                                    ContainerNumber = jd.ContainerNumber,
                                                    ShipmentTypeId = shipment.ShipmentTypeId,
                                                    StatusName = shipment.EntityStatus.Name,
                                                    AgentReference1 = shipment.AgentReference1,
                                                    AgentReference2 = shipment.AgentReference2,
                                                    CustomerReference1 = shipment.CustomerReference1,
                                                    CustomerReference2 = shipment.CustomerReference2,
                                                    TransportModeId = shipment.TransportModeId,
                                                    AgentId = shipment.AgentId,
                                                    ShipmentLevelCode = shipment.ShipmentLevelCode,
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
                                                    IsCancelled = shipment.IsCancelled,
                                                    DescriptionofGoods = shipment.DescriptionOfGoods,
                                                    House = shipment.House,
                                                    ConsigneeName = shipment.ConsigneeName,
                                                    ShipmentPackageReference1 = jd.Reference1,
                                                    ShipmentPackageReference2 = jd.Reference2,
                                                    ShipmentPackageReference3 = jd.Reference3,
                                                    ShipmentPackageReference4 = jd.Reference4,
                                                    OnCarriageToPortId = shipment.OnCarriageToPortId,
                                                    OnCarriageATD = shipment.OnCarriageATD,
                                                    OnCarriageATA = shipment.OnCarriageATA,
                                                    OnCarriageETA = shipment.OnCarriageETA,
                                                    MainCarriageETD = m.MainCarriageETD,
                                                    ATD = m.MainCarriageATD,
                                                    ContainerNotes = jd.Notes,
                                                    PackagesGrossWeight = jd.Weight,
                                                    PackagesVolumetricWeight = jd.VolumetricWeight,
                                                    PackagesQuantity = jd.Quantity,
                                                    ContainerFollowUp = jd.IsDeliveryFU,
                                                    SplitOnCarriage = shipment.SplitOnCarriage,
                                                    PackageOnCarriageATA = jd.OnCarriageATA,
                                                    PackageOnCarriageATD = jd.OnCarriageATD,
                                                    PackageOnCarriageETA = jd.OnCarriageETA,
                                                    PackageDliveryId = jd.DeliveryId,
                                                    IncotermId = shipment.IncotermId,
                                                    ShipperAddressId = shipment.ShipperAddressId,
                                                    ConsigneeAddressId = shipment.ConsigneeAddressId,
                                                    Volume = shipment.Volume,
                                                    PackageVolume = jd.Volume,
                                                    Reference1 = jd.Reference1,
                                                    NumberOfContainers = shipment.NumberOfContainers,
                                                    MainCarriageFinalDestinationPortId = shipment.LastFinalDestination,
                                                    MainCarriageCarrierPrefix = m.MainCarriageCarrierPrefix + m.MainCarriageCarrierNumber,
                                                });
           
            if (this.iQueryable_ShipmentPackages != null)
            {
                this.iQueryable_ShipmentPackages = this.iQueryable_ShipmentPackages.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ATD) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate) || System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));

                this.iQueryable_ShipmentPackages = this.iQueryable_ShipmentPackages.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ATD) <= System.Data.Entity.DbFunctions.TruncateTime(toDate) || System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                if (!string.IsNullOrEmpty(this.shipperId))
                {
                    iQueryable_ShipmentPackages = iQueryable_ShipmentPackages.Where(d => d.ShipperId == this.shipperId);
                }
                if (!string.IsNullOrEmpty(this.mainCarriageFromPortId))
                {
                    iQueryable_ShipmentPackages = iQueryable_ShipmentPackages.Where(d => d.MainCarriageFromPortId == this.mainCarriageFromPortId);
                }
                if (!string.IsNullOrEmpty(this.mainCarriageFinalDestinationPortId))
                {
                    iQueryable_ShipmentPackages = iQueryable_ShipmentPackages.Where(d => d.MainCarriageFinalDestinationPortId == this.mainCarriageFinalDestinationPortId);
                }

                if (!string.IsNullOrEmpty(this.subshipper))
                {
                    iQueryable_ShipmentPackages = iQueryable_ShipmentPackages.Where(d => d.Reference1 == this.subshipper);
                }
            }
        }

        private void BuildReportData()
        {
            if (this.iQueryable_ShipmentPackages != null)
            {
                var packages = new List<ShipperReturnsPackagesList>();
                var data = iQueryable_ShipmentPackages.ToList();
                foreach (var item in data)
                {
                    var itemRecord = new ShipperReturnsPackagesList();
                    itemRecord.FlightNumber = item.MainCarriageCarrierPrefix;
                    itemRecord.FlightDate = item.ATD != null ? item.ATD : item.MainCarriageETD;
                    itemRecord.Consignee = item.ConsigneeName;
                    itemRecord.HAWB = item.House;
                    itemRecord.MAWB = item.MasterNumber;
                    itemRecord.Quantity = item.PackagesQuantity;
                    itemRecord.GrossWeight = item.PackagesGrossWeight;
                    itemRecord.Volume = item.PackageVolume;
                    itemRecord.VolumetricWeight = item.PackagesVolumetricWeight;
                    customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, itemRecord, iDataProvider);
                    packages.Add(itemRecord);
                }
                this.iDataProvider.ShipperReturnsPackagesList = packages;
            }
        }
    }
}