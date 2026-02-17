using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.EntityQueryServices;
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
                        }

                        else
                        {
                            dataProvider.Origin = shipmentDataView.FromPortName;
                            dataProvider.Destination = shipmentDataView.MainCarriageFinalDestinationPortName;
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
    }
}