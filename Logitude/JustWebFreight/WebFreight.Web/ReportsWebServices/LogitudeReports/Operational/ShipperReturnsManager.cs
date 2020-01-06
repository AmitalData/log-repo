using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
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
        private IQueryable<ShipmentJoinPackageList> iQueryable_JoinShipmentPackages;

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

           
            this.iQueryable_JoinShipmentPackages = (from shipment in shipmentRepository.context.Shipments
                                                    join shipmentPackage in shipmentRepository.context.ShipmentPackages
                                                    on shipment.Id equals shipmentPackage.ShipmentId 
                                                    join shipmentMaster in shipmentRepository.context.ShipmentMasterDatas
                                                    on new { Id = shipment.MasterShipmentDataId }
                                                    equals new { Id = shipmentMaster.Id}
                                                    where shipment.Tenant == tenant && shipment.TransportModeId == "A" 
                                                    select new ShipmentJoinPackageList()
                                                    {
                                                        Id = shipment.Id + (!string.IsNullOrEmpty(shipment.Id) ? shipment.Id : ""),
                                                        ShipmentId = shipment.Id,
                                                        PackageId = shipmentPackage.Id,
                                                        ShipmentNumber = shipment.ShipmentNumber,
                                                        CreateDateTime = shipment.CreateDateTime,
                                                        ShipperName = shipment.ShipperCard != null ? shipment.ShipperCard.EnglishName : null,
                                                        ShipperId = shipment.ShipperId,
                                                        Field1 = shipment.Field1,
                                                        Field2 = shipment.Field2,
                                                        Field3 = shipment.Field3,
                                                        Field4 = shipment.Field4,
                                                        Field5 = shipment.Field5,
                                                        Field6 = shipment.Field6,
                                                        Field7 = shipment.Field7,
                                                        Field8 = shipment.Field8,
                                                        Field9 = shipment.Field9,
                                                        DescriptionofGoods = shipment.DescriptionOfGoods,
                                                        House = shipment.House,
                                                        ConsigneeName = shipment.ConsigneeName,
                                                        ShipmentPackageReference1 = shipmentPackage.Reference1,
                                                        ShipmentPackageReference2 = shipmentPackage.Reference2,
                                                        ShipmentPackageReference3 = shipmentPackage.Reference3,
                                                        ShipmentPackageReference4 = shipmentPackage.Reference4,
                                                        MainCarriageETD = shipmentMaster.MainCarriageETD,
                                                        ATD = shipmentMaster.MainCarriageATD,
                                                        PackagesGrossWeight = shipmentPackage.Weight,
                                                        PackagesVolumetricWeight = shipmentPackage.VolumetricWeight,
                                                        PackagesQuantity = shipmentPackage.Quantity,
                                                        ShipperAddressId = shipment.ShipperAddressId,
                                                        ConsigneeAddressId = shipment.ConsigneeAddressId,
                                                        Volume = shipment.Volume,
                                                        PackageVolume = shipmentPackage.Volume,
                                                        Reference1 = shipmentPackage.Reference1,
                                                        MasterNumber =  !string.IsNullOrEmpty(shipmentMaster.AirlinePrefix) && !string.IsNullOrEmpty(shipmentMaster.Master) ? shipmentMaster.AirlinePrefix + "-" + shipmentMaster.Master : "",
                                                        MainCarriageCarrierPrefix = shipmentMaster.MainCarriageCarrierPrefix + shipmentMaster.MainCarriageCarrierNumber,
                                                    });
           

            if (this.iQueryable_JoinShipmentPackages != null)
            {
                this.iQueryable_JoinShipmentPackages = this.iQueryable_JoinShipmentPackages.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ATD) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate) || System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) >= System.Data.Entity.DbFunctions.TruncateTime(fromDate));

                this.iQueryable_JoinShipmentPackages = this.iQueryable_JoinShipmentPackages.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.ATD) <= System.Data.Entity.DbFunctions.TruncateTime(toDate) || System.Data.Entity.DbFunctions.TruncateTime(d.MainCarriageETD) <= System.Data.Entity.DbFunctions.TruncateTime(toDate));
                if (!string.IsNullOrEmpty(this.shipperId))
                {
                    iQueryable_JoinShipmentPackages = iQueryable_JoinShipmentPackages.Where(d => d.ShipperId == this.shipperId);
                }
                if (!string.IsNullOrEmpty(this.mainCarriageFromPortId))
                {
                    iQueryable_JoinShipmentPackages = iQueryable_JoinShipmentPackages.Where(d => d.MainCarriageFromPortId == this.mainCarriageFromPortId);
                }
                if (!string.IsNullOrEmpty(this.mainCarriageFinalDestinationPortId))
                {
                    iQueryable_JoinShipmentPackages = iQueryable_JoinShipmentPackages.Where(d => d.MainCarriageFinalDestinationPortId == this.mainCarriageFinalDestinationPortId);
                }

                if (!string.IsNullOrEmpty(this.subshipper))
                {
                    iQueryable_JoinShipmentPackages = iQueryable_JoinShipmentPackages.Where(d => d.Reference1 == this.subshipper);
                }
            }
        }

        private void BuildReportData()
        {
            if (this.iQueryable_JoinShipmentPackages != null)
            {
                var packages = new List<ShipperReturnsPackagesList>();
                foreach (var item in iQueryable_JoinShipmentPackages)
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
                    customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, item, itemRecord);
                    packages.Add(itemRecord);
                }
                this.iDataProvider.ShipperReturnsPackagesList = packages;
            }
        }
    }
}