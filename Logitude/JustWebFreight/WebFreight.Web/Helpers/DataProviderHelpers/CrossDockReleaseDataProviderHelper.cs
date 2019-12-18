using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.EntityQueryServices;
using Logitude.WarehouseLib.Data;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.Helpers.DataProviderHelpers
{
    public class CrossDockReleaseDataProviderHelper
    {
        public byte[] LoadDataToCrossDockReleaseDataProvider(string entityId, int tenant)
        {
            CrossDockReleaseDataProvider dataprovider = LoadCrossDockReleaseDataProvider(entityId, tenant);
            XmlSerializer serializer = new XmlSerializer(typeof(CrossDockReleaseDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, dataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        public byte[] LoadCrossDockReleaseDataProvider_GroupByEntry(string entityId, int tenant)
        {
            CrossDockReleaseDataProvider dataprovider = this.BuildCrossDockReleaseDataProvider_GroupByEntry(entityId, tenant);
            XmlSerializer serializer = new XmlSerializer(typeof(CrossDockReleaseDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, dataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        private CrossDockReleaseDataProvider LoadCrossDockReleaseDataProvider(string entityId, int tenant)
        {
            CrossDockReleaseDataProvider dataProvider = new CrossDockReleaseDataProvider();
            WarehouseReleaseQueryService warehouseReleaseQueryService = new WarehouseReleaseQueryService(tenant);
            WarehouseReleasePM warehouseReleasePM = warehouseReleaseQueryService.GetSingle(entityId, true, false);

            if (warehouseReleasePM != null)
            {
                dataProvider.ExpectedReleaseDate = warehouseReleasePM.ExpectedReleaseDate;
                dataProvider.ActualReleaseDate = warehouseReleasePM.ActualReleaseDate;
                dataProvider.CustomerRef1 = warehouseReleasePM.CustomerRef1;
                dataProvider.CustomerRef2 = warehouseReleasePM.CustomerRef2;
                dataProvider.CustomerName = warehouseReleasePM.CustomerName;
                dataProvider.House = warehouseReleasePM.HouseNumber;
                dataProvider.Master = warehouseReleasePM.MasterNumber;
                dataProvider.IntenalNotes = warehouseReleasePM.Notes;
                dataProvider.SpecialInstruction = warehouseReleasePM.SpecialInstruction;
                dataProvider.ReleaseBy = warehouseReleasePM.ReleaseBy;
                dataProvider.ReleaseNumber = warehouseReleasePM.ReleaseNumber;

                if (!string.IsNullOrEmpty(warehouseReleasePM.UpdatedByUserId))
                {
                    ContactQuery contactQuery = new ContactQuery(warehouseReleasePM.Tenant);
                    dataProvider.UpdatedBy = contactQuery.GetContactNameId(warehouseReleasePM.UpdatedByUserId, warehouseReleasePM.Tenant);
                }

                if (!string.IsNullOrEmpty(warehouseReleasePM.WarehouseId))
                {
                    CardQuery cardQuery = new CardQuery(tenant);
                    CardList card = cardQuery.GetCardListForWareHouseById(warehouseReleasePM.WarehouseId, tenant);
                    if (card != null)
                    {
                        dataProvider.WarehouseCode = card.Code;
                        dataProvider.WarehouseName = card.EnglishName;
                    }

                    AddressQuery addressQuery = new AddressQuery(tenant);
                    AddressPM entityAddress = addressQuery.GetAddressPMByTypeAndCard(warehouseReleasePM.WarehouseId, "M", tenant);

                    if (entityAddress != null)
                    {
                        dataProvider.WraehousePhone = entityAddress.PhoneNumber;
                        dataProvider.WarehouseCountry = entityAddress.CountryName;
                        dataProvider.WarehouseAddress1 = entityAddress.Address1;
                        dataProvider.WarehouseAddress2 = entityAddress.Address2;
                        dataProvider.WarehouseZipCode = entityAddress.ZipCode;
                        dataProvider.WarehouseCity = entityAddress.City;
                        dataProvider.WarehouseState = entityAddress.StateEnglishName;
                    }
                }

                if (warehouseReleasePM.WarehouseReleasePackages != null && warehouseReleasePM.WarehouseReleasePackages.Count > 0)
                {
                    dataProvider.ReleasePackages = FullPackage(warehouseReleasePM);
                }

                dataProvider.TenantLogo = DataProviders.General.GetLogo(tenant);

                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                Tenant myTenant = (from a in commonContext.Tenants where a.Id == tenant select a).FirstOrDefault();

                if (!string.IsNullOrEmpty(myTenant.AddressId))
                {
                    AddressRepository addressRepository = new AddressRepository(commonContext);
                    Address address = addressRepository.GetSingleAddress(myTenant.AddressId, tenant);

                    if (address != null)
                    {
                        dataProvider.TenantAddress = DataProviders.General.GetAddress(address);
                    }
                }

                if (!string.IsNullOrEmpty(warehouseReleasePM.ShipmentId))
                {
                    ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                    ShipmentDataView shipmentDataView = shipmentRepository.GetSingleShipmentDataView(warehouseReleasePM.ShipmentId, tenant);
                    if (shipmentDataView != null)
                    {
                        if (shipmentDataView.DirectionId == "D" && shipmentDataView.TransportModeId == "I")
                        {
                            dataProvider.Origin = shipmentDataView.MainCarriageFromCity;
                            dataProvider.Destination = shipmentDataView.MainCarriageToCity;

                            CountryRepository countryRepository = new CountryRepository(tenant);
                            Country country = countryRepository.GetSingleCountryByCode(shipmentDataView.MainCarriageToCountryCode, tenant);

                            if (country != null)
                            {
                                dataProvider.DestinationCountryName = country.EnglishName;
                            }                                
                        }

                        else
                        {
                            dataProvider.Origin = shipmentDataView.FromPortName;
                            dataProvider.Destination = shipmentDataView.MainCarriageFinalDestinationPortName;
                            dataProvider.DestinationCountryName = shipmentDataView.MainCarriageFinalDestinationCountryName;
                        }

                        dataProvider.MainCarriageCarrierName = shipmentDataView.MainCarriageCarrierName;
                        dataProvider.ShipperName = shipmentDataView.ShipperName;
                        dataProvider.ConsigneeName = shipmentDataView.ConsigneeName;
                        dataProvider.ShipmentNumber = shipmentDataView.ShipmentNumber;
                        if (!string.IsNullOrEmpty(shipmentDataView.ShipperId))
                        {
                            AddressRepository addressRepository = new AddressRepository(commonContext);
                            Address address = addressRepository.GetMainAddressByCardId(shipmentDataView.ShipperId, tenant);
                            if (address != null)
                            {
                                dataProvider.ShipperAddress = DataProviders.General.GetAddress(address);
                            }
                        }

                        if (!string.IsNullOrEmpty(shipmentDataView.ConsigneeId))
                        {
                            AddressRepository addressRepository = new AddressRepository(commonContext);
                            Address address = addressRepository.GetMainAddressByCardId(shipmentDataView.ConsigneeId, tenant);
                            if (address != null)
                            {
                                dataProvider.ConsigneeAddress = DataProviders.General.GetAddress(address);
                            }
                        }
                    }
                }
            }

            return dataProvider;
        }        
        private List<ReleasePackage> FullPackage(WarehouseReleasePM warehouseReleasePM)
        {
            List<ReleasePackage> result = new List<ReleasePackage>();
            foreach (WarehouseReleasePackagePM package in warehouseReleasePM.WarehouseReleasePackages)
            {
                var item = new ReleasePackage();
                item.ContainerNumber = package.ContainerNumber;
                item.DescriptionOfGoods = package.Description;
                item.Dimensions = package.Dimensions;
                item.Harmonize = package.Harmonize;
                item.PackageType = package.PackageTypeName;
                item.Quantity = package.Quantity;
                item.Volume = package.Volume;
                item.VolumeUnit = warehouseReleasePM.VolumeUnitCode;
                item.Weight = package.Weight;
                item.WeightUnit = warehouseReleasePM.GrossWeightUnitCode;
                item.Seal = package.Seal;
                item.VolumetricWeight = package.VolumetricWeight;
                item.VolumetricWeightUnit = warehouseReleasePM.ChargeableWeightUnitCode;
                //item.InStock = package.st

                #region Car Details
                item.Make = package.Make;
                item.Model = package.Model;
                item.Year = package.Year;
                item.Color = package.Color;
                item.ChassisNumber = package.ChassisNumber;
                item.RegistrationNumber = package.RegistrationNumber;

                if (!string.IsNullOrEmpty(package.CountryId))
                {
                    Country country = CountryRepository.GetSingleCountry(package.CountryId, warehouseReleasePM.Tenant, true);
                    if (country != null)
                    {
                        item.CountryName = country.EnglishName;
                    }
                }
                #endregion

                result.Add(item);
            }

            return result;
        }

        //New Document CRR
        private CrossDockReleaseDataProvider BuildCrossDockReleaseDataProvider_GroupByEntry(string entityId, int tenant)
        {
            CrossDockReleaseDataProvider dataProvider = new CrossDockReleaseDataProvider();
            dataProvider.ReleasePackagesGroupList = new List<ReleasePackageGroup>();

            IWarehouseContext warehouseContext = WarehouseContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            WarehouseReleaseQueryService warehouseReleaseQueryService = new WarehouseReleaseQueryService(warehouseContext);
            WarehouseEntryPackagesReleaseRepository warehouseEntryPackagesReleaseRepository = new WarehouseEntryPackagesReleaseRepository(warehouseContext);
            WarehouseEntryPackageRepository warehouseEntryPackageRepository = new WarehouseEntryPackageRepository(warehouseContext);
            WarehouseEntryRepository warehouseEntryRepository = new WarehouseEntryRepository(warehouseContext);
            PortRepository portRepository = new PortRepository(commonContext);
            AddressRepository addressRepository = new AddressRepository(commonContext);

            WarehouseReleasePM warehouseReleasePM = warehouseReleaseQueryService.GetSingle(entityId, true, false);
            if (warehouseReleasePM != null)
            {
                dataProvider.IntenalNotes = warehouseReleasePM.Notes;
                dataProvider.ReleaseNumber = warehouseReleasePM.ReleaseNumber;
                dataProvider.DimensionsHeader = "(L - W - H) (" + warehouseReleasePM.DimensionsUnitCode + ")";
                dataProvider.VolumetricWeightUnit = warehouseReleasePM.ChargeableWeightUnitCode;
                dataProvider.VolumeUnit = warehouseReleasePM.VolumeUnitCode;
                dataProvider.WeightUnit = warehouseReleasePM.GrossWeightUnitCode;

                //ReleaseReference
                string refrence = warehouseReleasePM.CustomerRef1;
                if(!string.IsNullOrEmpty(warehouseReleasePM.CustomerRef2))
                {
                    if (string.IsNullOrEmpty(refrence))
                    {
                        refrence = warehouseReleasePM.CustomerRef2;
                    }

                    else
                    {
                        refrence = refrence + ", " + warehouseReleasePM.CustomerRef2;
                    }
                }

                dataProvider.ReleaseReference = refrence;

                //ReleaseDate
                if (warehouseReleasePM.ActualReleaseDate != null)
                {
                    dataProvider.ReleaseDate = warehouseReleasePM.ActualReleaseDate;
                    dataProvider.ReleaseDateIndicator = "Actual";
                }

                else if(warehouseReleasePM.ExpectedReleaseDate != null)
                {
                    dataProvider.ReleaseDate = warehouseReleasePM.ExpectedReleaseDate;
                    dataProvider.ReleaseDateIndicator = "Expected";
                }

                //Destination
                string destinationCountryName = "";
                switch(warehouseReleasePM.ToTypeCode)
                {
                    case "PORT":
                        {
                            Port toPort = portRepository.GetSinglePort(tenant, warehouseReleasePM.ToPortId);
                            if(toPort != null)
                            {
                                Country country = CountryRepository.GetSingleCountry(toPort.CountryId, tenant, true);
                                if(country != null)
                                {
                                    destinationCountryName = country.EnglishName;
                                }
                            }

                            break;
                        }

                    case "PART":
                        {
                            Address toAddress = addressRepository.GetSingleAddress(warehouseReleasePM.ToAddressId, tenant);
                            if(toAddress != null)
                            {
                                Country country = CountryRepository.GetSingleCountry(toAddress.CountryId, tenant, true);
                                if (country != null)
                                {
                                    destinationCountryName = country.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            Country country = CountryRepository.GetSingleCountry(warehouseReleasePM.ToAddressCountryId, tenant, true);
                            if (country != null)
                            {
                                destinationCountryName = country.EnglishName;
                            }

                            break;
                        }
                }
                
                dataProvider.DestinationCountryName = destinationCountryName;

                //Partners
                if (!string.IsNullOrEmpty(warehouseReleasePM.ShipmentId))
                {
                    ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                    Shipment shipment = shipmentRepository.GetSingleShipment(warehouseReleasePM.ShipmentId, tenant);

                    if(shipment != null)
                    {
                        dataProvider.ShipperName = shipment.ShipperName;
                        dataProvider.ConsigneeName = shipment.ConsigneeName;
                    }
                }

                //Packages
                List<string> releasePackagesIds = warehouseReleasePM.WarehouseReleasePackages.Select(s => s.Id).ToList();
                List<WarehouseEntryPackagesRelease> entryPackagesReleases = warehouseEntryPackagesReleaseRepository.GetWarehouseEntryPackagesReleaseByReleasePackageIds(releasePackagesIds, tenant);

                List<ReleasePackage> tempList = new List<ReleasePackage>();
                foreach (WarehouseReleasePackagePM releasePackage in warehouseReleasePM.WarehouseReleasePackages)
                {
                    ReleasePackage item = new ReleasePackage();
                    item.ContainerNumber = releasePackage.ContainerNumber;
                    item.DescriptionOfGoods = releasePackage.Description;
                    item.Dimensions = releasePackage.Dimensions;
                    item.Harmonize = releasePackage.Harmonize;
                    item.PackageType = releasePackage.PackageTypeName;
                    item.Quantity = releasePackage.Quantity;
                    item.Volume = releasePackage.Volume;
                    item.VolumeUnit = warehouseReleasePM.VolumeUnitCode;
                    item.Weight = releasePackage.Weight;
                    item.WeightUnit = warehouseReleasePM.GrossWeightUnitCode;
                    item.Seal = releasePackage.Seal;
                    item.VolumetricWeight = releasePackage.VolumetricWeight;
                    item.VolumetricWeightUnit = warehouseReleasePM.ChargeableWeightUnitCode;

                    WarehouseEntryPackagesRelease entryPackageRelease = entryPackagesReleases.Where(d => d.ReleasePackageId == releasePackage.Id).FirstOrDefault();
                    if (entryPackageRelease != null)
                    {
                        WarehouseEntryPackage entryPackage = warehouseEntryPackageRepository.GetSingle(entryPackageRelease.EntryPackageId, tenant);

                        if (entryPackage != null)
                        {
                            item.InStock = entryPackage.Instock;

                            WarehouseEntry warehouseEntry = warehouseEntryRepository.GetSingle(entryPackage.WarehouseEntryId, tenant);
                            if (warehouseEntry != null)
                            {
                                item.EntryId = warehouseEntry.Id;
                                item.EntryNumber = warehouseEntry.EntryNumber;
                            }
                        }
                    }

                    #region Car Details
                    item.Make = releasePackage.Make;
                    item.Model = releasePackage.Model;
                    item.Year = releasePackage.Year;
                    item.Color = releasePackage.Color;
                    item.ChassisNumber = releasePackage.ChassisNumber;
                    item.RegistrationNumber = releasePackage.RegistrationNumber;

                    if (!string.IsNullOrEmpty(releasePackage.CountryId))
                    {
                        Country country = CountryRepository.GetSingleCountry(releasePackage.CountryId, warehouseReleasePM.Tenant, true);
                        if (country != null)
                        {
                            item.CountryName = country.EnglishName;
                        }
                    }
                    #endregion

                    tempList.Add(item);
                }

                tempList = tempList.OrderBy(or => or.EntryNumber).ToList();

                List<ReleasePackageGroup> finalResults = (from p in tempList
                                                          group p by new { p.EntryId, p.EntryNumber } into g
                                                          select new ReleasePackageGroup()
                                                          {
                                                              EntryId = g.Key.EntryId,
                                                              EntryNumber = g.Key.EntryNumber,
                                                              ReleasePackagesList = g.ToList(),
                                                          }).ToList();

                dataProvider.ReleasePackagesGroupList = finalResults.OrderBy(d => d.EntryNumber).ToList();
            }

            return dataProvider;
        }
    }
}