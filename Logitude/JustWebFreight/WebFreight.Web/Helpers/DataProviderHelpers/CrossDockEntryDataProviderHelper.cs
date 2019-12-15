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
    public class CrossDockEntryDataProviderHelper
    {

       public byte[] LoadDataToCrossDockEntryDataProvider(string entityId, int tenant)
        {
            CrossDockEntryDataProvider dataprovider = LoadCrossDockEntryDataProvider(entityId, tenant);
            XmlSerializer serializer = new XmlSerializer(typeof(CrossDockEntryDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, dataprovider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;

        }

       private CrossDockEntryDataProvider LoadCrossDockEntryDataProvider(string entityId, int tenant)
        {
            CrossDockEntryDataProvider dataProvider = new CrossDockEntryDataProvider();
            WarehouseEntryQueryService warehouseEntryQueryService = new WarehouseEntryQueryService(tenant);
            WarehouseEntryPM warehouseEntryPM = warehouseEntryQueryService.GetSingle(entityId,true,false);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            if (warehouseEntryPM != null)
            {
                dataProvider.ExpectedEntryDate = warehouseEntryPM.ExpectedEntryDate;
                dataProvider.ActualEntryDate = warehouseEntryPM.ActualEntryDate;
                dataProvider.CustomerRef1 = warehouseEntryPM.CustomerRef1;
                dataProvider.CustomerRef2 = warehouseEntryPM.CustomerRef2;
                dataProvider.CustomerName = warehouseEntryPM.CustomerName;
                dataProvider.House = warehouseEntryPM.HouseNumber;
                dataProvider.Master = warehouseEntryPM.MasterNumber;
                dataProvider.IntenalNotes = warehouseEntryPM.Notes;
                dataProvider.SpecialInstruction = warehouseEntryPM.SpecialInstruction;
                dataProvider.ReceivedBy = warehouseEntryPM.ReceivedBy;
                dataProvider.Origin = warehouseEntryPM.Origin;
                dataProvider.Destination = warehouseEntryPM.Destination;
                dataProvider.ShipperName = warehouseEntryPM.ShipperName;
                dataProvider.ConsigneeName =  warehouseEntryPM.ConsigneeName;
                dataProvider.EntryNumber = warehouseEntryPM.EntryNumber;

                List<string> cardIds = new List<string>();
                List<CardList> cardLists = new List<CardList>();
                cardIds.Add(warehouseEntryPM.WarehouseId);

                if (!string.IsNullOrEmpty(warehouseEntryPM.ShipperId) && !cardIds.Contains(warehouseEntryPM.ShipperId)) cardIds.Add(warehouseEntryPM.ShipperId);
                if (!string.IsNullOrEmpty(warehouseEntryPM.ConsigneeId) && !cardIds.Contains(warehouseEntryPM.ConsigneeId)) cardIds.Add(warehouseEntryPM.ConsigneeId);

                if (cardIds.Count > 0)
                {
                    CardQuery cardQuery = new CardQuery(tenant);
                    cardLists = cardQuery.GetCardListsByCardIds(cardIds,tenant);
                }

                if (!string.IsNullOrEmpty(warehouseEntryPM.UpdatedByUserId))
                {
                    ContactQuery contactQuery = new ContactQuery(warehouseEntryPM.Tenant);
                    dataProvider.UpdatedBy = contactQuery.GetContactNameId(warehouseEntryPM.UpdatedByUserId, warehouseEntryPM.Tenant);
                }


                if (!string.IsNullOrEmpty(warehouseEntryPM.ShipperId))
                {
                    CardList card = cardLists.Where(d => d.Id == warehouseEntryPM.ShipperId).FirstOrDefault();

                    if (card != null) dataProvider.ShipperName = card.EnglishName;

                    AddressRepository addressRepository = new AddressRepository(commonContext);
                    Address address = addressRepository.GetMainAddressByCardId(warehouseEntryPM.ShipperId, tenant);
                    if (address != null)
                    {
                        dataProvider.ShipperAddress = DataProviders.General.GetAddress(address);
                    }
                }

                if (!string.IsNullOrEmpty(warehouseEntryPM.ConsigneeId))
                {
                    CardList card = cardLists.Where(d => d.Id == warehouseEntryPM.ConsigneeId).FirstOrDefault();
                    if (card != null) dataProvider.ConsigneeName = card.EnglishName;

                    AddressRepository addressRepository = new AddressRepository(commonContext);
                    Address address = addressRepository.GetMainAddressByCardId(warehouseEntryPM.ConsigneeId, tenant);
                    if (address != null)
                    {
                        dataProvider.ConsigneeAddress = DataProviders.General.GetAddress(address);
                    }
                }


                if (!string.IsNullOrEmpty(warehouseEntryPM.WarehouseId))
                {
                    CardQuery cardQuery = new CardQuery(tenant);
                    CardList card = cardLists.Where(d => d.Id == warehouseEntryPM.WarehouseId).FirstOrDefault();
                    if (card != null)
                    {
                        dataProvider.WarehouseCode = card.Code;
                        dataProvider.WarehouseName = card.EnglishName;
                    }

                    AddressQuery addressQuery = new AddressQuery(tenant);
                    AddressPM entityAddress = addressQuery.GetAddressPMByTypeAndCard(warehouseEntryPM.WarehouseId, "M", tenant);

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

                if (warehouseEntryPM.WarehouseEntryPackages != null && warehouseEntryPM.WarehouseEntryPackages.Count > 0)
                {
                    dataProvider.EntryPackages = FullPackage(warehouseEntryPM);
                }


                dataProvider.TenantLogo = DataProviders.General.GetLogo(tenant);
                

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

                if (!string.IsNullOrEmpty(warehouseEntryPM.ShipmentId))
                {
                    ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                    ShipmentDataView shipmentDataView = shipmentRepository.GetSingleShipmentDataView(warehouseEntryPM.ShipmentId, tenant);
                    if (shipmentDataView != null)
                    {
                        dataProvider.MainCarriageCarrierName = shipmentDataView.MainCarriageCarrierName;
                        dataProvider.ShipmentNumber = shipmentDataView.ShipmentNumber;
                    }
                }
                if (!string.IsNullOrEmpty(warehouseEntryPM.ToPortId))
                {
                    PortRepository portRepository = new PortRepository(tenant);
                    Port destinationPort = portRepository.GetSinglePort(warehouseEntryPM.ToPortId, tenant);
                    if (destinationPort != null)
                    {
                        dataProvider.DestinationCountryName = destinationPort.Country == null ? "" : destinationPort.Country.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(warehouseEntryPM.TruckerId))
                {
                    Card truckerCard = CardRepository.GetSingleCard(warehouseEntryPM.TruckerId, tenant, true);
                    if (truckerCard != null)
                    {
                        dataProvider.Trucker = truckerCard == null ? "" : truckerCard.EnglishName;
                    }
                }

                dataProvider.BarCode = warehouseEntryPM.EntryReference;

            }
            return dataProvider;
        }

       private List<EntryPackage> FullPackage(WarehouseEntryPM warehouseEntryPM)
        {
            List<EntryPackage> result = new List<EntryPackage>();
            foreach (WarehouseEntryPackagePM package in warehouseEntryPM.WarehouseEntryPackages)
            {
                var item = new EntryPackage();
                item.ContainerNumber = package.ContainerNumber;
                item.DescriptionOfGoods = package.Description;
                item.Dimensions = package.Dimensions;
                item.Harmonize = package.Harmonize;
                item.PackageType = package.PackageTypeName;
                item.Quantity = package.Quantity;
                item.Volume = package.Volume;
                item.VolumeUnit = warehouseEntryPM.VolumeUnitCode;
                item.Weight = package.Weight;
                item.WeightUnit = warehouseEntryPM.GrossWeightUnitCode;
                item.Seal = package.Seal;
                item.VolumetricWeight = package.VolumetricWeight;
                item.VolumetricWeightUnit = warehouseEntryPM.ChargeableWeightUnitCode;
                item.InStock = package.Instock;

                #region Car Details
                item.Make = package.Make;
                item.Model = package.Model;
                item.Year = package.Year;
                item.Color = package.Color;
                item.ChassisNumber = package.ChassisNumber;
                item.RegistrationNumber = package.RegistrationNumber;

                if (!string.IsNullOrEmpty(package.CountryId))
                {
                    Country country = CountryRepository.GetSingleCountry(package.CountryId, warehouseEntryPM.Tenant, true);
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