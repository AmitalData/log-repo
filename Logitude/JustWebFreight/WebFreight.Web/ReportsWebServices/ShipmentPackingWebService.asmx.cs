using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataProviders;
using Simplog.Data.CommonDataModel;
using Simplog.Data.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>
    /// Summary description for ShipmentPackingWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ShipmentPackingWebService : System.Web.Services.WebService
    {
        public ShipmentPM shipment;
        [WebMethod]
        public byte[] GetShipmentPackingData(string shipmentId, int tenant)
        {
            ShipmentPackingDataProvider provider = this.BuildProvider(shipmentId, tenant);
            XmlSerializer serializer = new XmlSerializer(typeof(ShipmentPackingDataProvider));

            using (MemoryStream memstream = new MemoryStream())
            {
                serializer.Serialize(memstream, provider);
                memstream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memstream);
                string content = reader.ReadToEnd();
                byte[] bytearray = memstream.ToArray();
                return bytearray;
            }
        }

        private ShipmentPackingDataProvider BuildProvider(string shipmentId, int tenant)
        {
            ShipmentPackingDataProvider provider = new ShipmentPackingDataProvider();
            ShipmentPackingDataProviderService shipmentPackingService = new ShipmentPackingDataProviderService();

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            AddressRepository addressRepository = new AddressRepository(commonContext);
            PortRepository portRepository = new PortRepository(commonContext);
            CountryRepository countryRepository = new CountryRepository(commonContext);

            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);

            shipment = shipmentQuery.GetSinglePM(shipmentId, tenant);

            if (shipment != null)
            {
                Tenant myTenant = (from a in commonContext.Tenants where a.Id == tenant select a).FirstOrDefault();
                if (myTenant != null)
                {
                    provider.Signature = myTenant.Signature;
                }

                provider.FileNumber = string.IsNullOrEmpty(shipment.ShipmentNumber) ? "" : shipment.ShipmentNumber;
                provider.TotalGrossWeight = shipment.GrossWeight;
                provider.TotalNumberOfPackages = shipment.NumberOfPackages;

                shipmentPackingService.MapWeightDetails(provider, shipment);
                shipmentPackingService.MapVolumeDetails(provider, shipment);
                shipmentPackingService.MapCarrierType(provider, shipment);
                provider.CustomerReference1 = shipment.CustomerReference1;
                provider.CustomerReference2 = shipment.CustomerReference2;
                provider.HouseNumber = shipment.House;
                provider.DescriptionOfGoods = shipment.DescriptionOfGoods;
                provider.PortOfLading = shipment.MainCarriageFromPortName;

                #region ShipmentMethod
                string shipmentMethod = "";
                TransportModeRepository transportModeRepository = new TransportModeRepository(tenant);
                TransportMode transportMode = transportModeRepository.GetSingleTransportMode(shipment.TransportModeId);
                if (transportMode != null)
                {
                    shipmentMethod = transportMode.Name;
                }

                ShipmentTypeRepository shipmentTypeRepository = new ShipmentTypeRepository(tenant);
                ShipmentType shipmentType = shipmentTypeRepository.GetSingleShipmentType(shipment.ShipmentTypeId);
                if (shipmentType != null)
                {
                    shipmentMethod = string.IsNullOrEmpty(shipmentMethod) ? shipmentType.Name : shipmentMethod + " " + shipmentType.Name;
                }

                provider.ShipmentMethod = shipmentMethod;
                #endregion

                #region Shipper & Consignee

                Card shipperCard = CardRepository.GetSingleCard(shipment.ShipperId, tenant, false);
                if (shipperCard != null)
                {
                    provider.Shipper = shipperCard.EnglishName;
                }

                Card consigneeCard = CardRepository.GetSingleCard(shipment.ConsigneeId, tenant, false);
                if (consigneeCard != null)
                {
                    provider.Consignee = consigneeCard.EnglishName;
                }
                
                Address shipperAddress = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);
                if (shipperAddress != null)
                {
                    if (shipperAddress.IsLocalLanguage)
                    {
                        if (shipperCard != null)
                        {
                            provider.Shipper = shipperCard.LocalName;
                        }
                    }

                    provider.ShipperAddress = DataProviders.General.GetAddress(shipperAddress);
                }

                Address consigneeAddress = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);
                if (consigneeAddress != null)
                {
                    if (consigneeAddress.IsLocalLanguage)
                    {
                        if (consigneeCard != null)
                        {
                            provider.Consignee = consigneeCard.LocalName;
                        }
                    }

                    provider.ConsigneeAddress = DataProviders.General.GetAddress(consigneeAddress);
                }
                #endregion

                #region Final Destination
                ShipmentPickUpDelivery myDelivery = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                     where d.ShipmentId == shipment.Id && d.PickUpDeliveryTypeCode == "DELV"
                                                     select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                Port onCarriageToPort = (from a in commonContext.Ports
                                         where a.Id == shipment.OnCarriageToPortId
                                         select a).FirstOrDefault();

                Port onForwardingToPort = (from a in commonContext.Ports
                                           where a.Id == shipment.OnForwardingToPortId
                                           select a).FirstOrDefault();

                Port mainCarriageToPort = (from a in commonContext.Ports
                                           where a.Id == shipment.MainCarriageToPortId
                                           select a).FirstOrDefault();

                if (myDelivery != null)
                {
                    switch (myDelivery.PickUpDeliveryToTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myDelivery.ToPartnerCardId))
                                {
                                    Address myPartnerAddress = addressRepository.GetMainAddressByCardId(myDelivery.ToPartnerCardId, tenant);
                                    if (myPartnerAddress != null)
                                    {
                                        provider.DestinationPortName = myPartnerAddress.City;
                                        provider.DestinationPortCountryName = shipmentPackingService.GetDestinationPortCountryName(
                                            new MapDestinationPortCountryNameParameters()
                                            {
                                                Tenant = myPartnerAddress.Tenant,
                                                CountryRepository = countryRepository,
                                                CountryId = myPartnerAddress.CountryId
                                            });
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myDelivery.ToPortId))
                                {
                                    Port myPort = portRepository.GetSinglePort(tenant, myDelivery.ToPortId);
                                    if (myPort != null)
                                    {
                                        provider.DestinationPortName = myPort.EnglishName;
                                        provider.DestinationPortCountryName = myPort.CountryName;
                                    }  
                                }

                                break;
                            }

                        case "CASL":
                            {
                                string myCity = myDelivery.ToAddressCity;
                                if (!string.IsNullOrEmpty(myCity))
                                {
                                    provider.DestinationPortName = myCity;
                                }

                                provider.DestinationPortCountryName = shipmentPackingService.GetDestinationPortCountryName(
                                    new MapDestinationPortCountryNameParameters()
                                    {
                                        Tenant = myDelivery.Tenant,
                                        CountryRepository = countryRepository,
                                        CountryId = myDelivery.ToAddressCountryId
                                    });
                                
                                break;
                            }
                    }
                }

                else if (onForwardingToPort != null)
                {
                    provider.DestinationPortName = onForwardingToPort.EnglishName;
                    provider.DestinationPortCountryName = onForwardingToPort.CountryName;
                }

                else if (onCarriageToPort != null)
                {
                    provider.DestinationPortName = onCarriageToPort.EnglishName;
                    provider.DestinationPortCountryName = onCarriageToPort.CountryName;
                }

                else
                {
                    DestinationPortDetails destinationPortDetails = shipmentPackingService.GetDestinationPortDetails(shipment, mainCarriageToPort);

                    provider.DestinationPortName = destinationPortDetails.Name;
                    provider.DestinationPortCountryName = destinationPortDetails.CountryName;
                }

                #endregion

                PackageTypeRepository packageTypeRepository = new PackageTypeRepository(tenant);
                ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(tenant);
                ShipmentPackageItemRepository shipmentPackageItemRepository = new ShipmentPackageItemRepository(tenant);

                List<ShipmentPackage> shipmentPackages = shipmentPackageRepository.GetShipmentPackagesForShipmentTenant(shipmentId, tenant).ToList();
                double? totalAmountOfPackageItems = 0;
                if (shipmentPackages.Count > 0)
                {
                    provider.ShipmentPackages = new List<ShipmentPackageProvider>();

                    foreach (ShipmentPackage shipmentPackage in shipmentPackages)
                    {
                        List<ShipmentPackageItem> shipmentPackageItems = shipmentPackageItemRepository.GetShipmentPackageItemsbyPackageId(shipmentPackage.Id, tenant).ToList();

                        if (shipmentPackageItems.Count > 0)
                        {
                            ShipmentPackageProvider shipmentPackageProvider = new ShipmentPackageProvider();

                            shipmentPackageProvider.PackageItems = new List<PackageItemProvider>();
                            shipmentPackageProvider.PackageItemsDetails = new List<PackageItemProviderDetails>();
                            shipmentPackageProvider.Quantity = shipmentPackage.Quantity + "";
                            shipmentPackageProvider.PackageType = shipmentPackage.PackageType == null || string.IsNullOrEmpty(shipmentPackage.PackageType.EnglishName) ? "" : shipmentPackage.PackageType.EnglishName;
                            shipmentPackageProvider.Seal = string.IsNullOrEmpty(shipmentPackage.ShipperSeal) ? "" : shipmentPackage.ShipperSeal;
                            shipmentPackageProvider.ContainerNumber = string.IsNullOrEmpty(shipmentPackage.ContainerNumber) ? "" : shipmentPackage.ContainerNumber;
                            shipmentPackageProvider.GrossWeight = shipmentPackage.Weight;

                            if (!string.IsNullOrEmpty(shipmentPackage.HorseId))
                            {
                                Horse horse = (from pa in commonContext.Horses
                                               where pa.Id == shipmentPackage.HorseId
                                               select pa).FirstOrDefault();

                                if (horse != null)
                                {
                                    shipmentPackageProvider.HorseName = horse.Name;
                                    shipmentPackageProvider.HorseYearOfBirth = horse.YearOfBirth;
                                    shipmentPackageProvider.HorseColor = horse.Color;
                                    shipmentPackageProvider.HorseBreed = horse.Breed;
                                    shipmentPackageProvider.HorseDiscipline = horse.Discipline;
                                    shipmentPackageProvider.HorseTravelBehavior = horse.TravelBehavior;
                                    shipmentPackageProvider.HorseMicochipNumber = horse.MicochipNumber;
                                    shipmentPackageProvider.HorsePassportNumber = horse.PassportNumber;
                                    shipmentPackageProvider.HorseCurrentStable = horse.CurrentStable;
                                    shipmentPackageProvider.HorseOwner = horse.Owner;
                                    shipmentPackageProvider.HorseRemarks = horse.Remarks;

                                    if (!string.IsNullOrEmpty(horse.CountryOfBirthId))
                                    {
                                        Country country = (from pa in commonContext.Countries
                                                           where pa.Id == horse.CountryOfBirthId
                                                           select pa).FirstOrDefault();

                                        if (country != null)
                                        {
                                            shipmentPackageProvider.HorseCountryOfBirthName = country.EnglishName;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(horse.GenderCode))
                                    {
                                        HorseGender horseGender = (from pa in commonContext.HorseGenders
                                                                   where pa.Code == horse.GenderCode
                                                                   select pa).FirstOrDefault();

                                        if (horseGender != null)
                                        {
                                            shipmentPackageProvider.HorseGender = horseGender.Name;
                                        }
                                    }
                                }
                            }

                            PackageType packageType = packageTypeRepository.GetSinglePackageType(shipmentPackage.PackageTypeId, tenant);
                            if (packageType != null)
                            {
                                shipmentPackageProvider.ContainerSize = packageType.PrintAs;
                            }

                            int i = 0;
                            foreach (ShipmentPackageItem item in shipmentPackageItems)
                            {
                                shipmentPackageProvider.PackageItemsDetails.Add(shipmentPackingService.GetPackageItemDetails(item));
                                for (int t = 0; t < item.Quantity; t++)
                                {
                                    i += 1;

                                    shipmentPackageProvider.PackageItems.Add(new PackageItemProvider()
                                    {
                                        Index = i,
                                        Description = item.Description,
                                        Value = item.GoodsValue == null ? "" : String.Format("{0:#,0.00}", item.GoodsValue)
                                    });
                                }
                            }

                            totalAmountOfPackageItems += (double?) shipmentPackingService.GetTotalAmountOfPackageItems(shipmentPackageItems);

                            provider.ShipmentPackages.Add(shipmentPackageProvider);
                        }
                    }

                }

                MapTotalFieldsParameters mapTotalFieldsParameters = new MapTotalFieldsParameters()
                {
                    Tenant = tenant,
                    ShipmentPackingDataProvider = provider,
                    Shipment = shipment,
                    TotalAmountOfPackageItems = totalAmountOfPackageItems
                };
                shipmentPackingService.MapTotalFields(mapTotalFieldsParameters); 
            }

            DocumentType currentdocumentType = commonContext.DocumentTypes.Where(doc => doc.Code == "PALI" && doc.Tenant == tenant).FirstOrDefault();

            if (currentdocumentType != null)
            {
                List<FormCustomField> customfieldsList = commonContext.FormCustomFields.Where(fc => fc.DocumentTypeId == currentdocumentType.Id && fc.EntityId == shipment.Id && fc.Tenant == tenant).ToList();
                List<DocumentTypeCustomField> documentCustomfieldsList = commonContext.DocumentTypeCustomFields.Where(fc => fc.DocumentTypeId == currentdocumentType.Id && fc.Tenant == tenant).ToList();
                FormCustomField PackDate1 = (from a in customfieldsList where a.FieldCode == "PackDate" && a.EntityId == shipment.Id select a).FirstOrDefault();
                DocumentTypeCustomField PackDate2 = (from a in documentCustomfieldsList where a.FieldCode == "PackDate" select a).FirstOrDefault();


                provider.PackDate = PackDate1 != null ? PackDate1.Value : PackDate2 != null ? PackDate2.DefaultValue:null;

                FormCustomField Remarks1 = (from a in customfieldsList where a.FieldCode == "Remarks" select a).FirstOrDefault();
                DocumentTypeCustomField Remarks2 = (from a in documentCustomfieldsList where a.FieldCode == "Remarks" select a).FirstOrDefault();

                provider.Remarks = Remarks1 != null ? Remarks1.Value : (Remarks2 != null ? Remarks2.DefaultValue : "");
            }

            return provider;
        }

    }

    public class MapTotalFieldsParameters
    {
        public int Tenant;
        public ShipmentPackingDataProvider ShipmentPackingDataProvider;
        public ShipmentPM Shipment;
        public double? TotalAmountOfPackageItems;
    }
    public class MapDestinationPortCountryNameParameters
    {
        public int Tenant;
        public CountryRepository CountryRepository;
        public string CountryId;
    }

    public class DestinationPortDetails
    {
        public string Name;
        public string CountryName;
    }
}
