using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
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
        CrossDockEntryDataProvider crossDockEntryDataProvider;
        public WarehouseEntryPM warehouseEntryPM;
        public byte[] LoadDataToCrossDockEntryDataProvider(string entityId, int tenant, string userId)
        {
            CrossDockEntryDataProvider dataprovider = LoadCrossDockEntryDataProvider(entityId, tenant, userId);
            XmlSerializer serializer = new XmlSerializer(typeof(CrossDockEntryDataProvider));
            using (MemoryStream memstream = new MemoryStream())
            {
                serializer.Serialize(memstream, dataprovider);
                memstream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memstream);
                string content = reader.ReadToEnd();
                byte[] bytearray = memstream.ToArray();
                return bytearray;
            }

        }

        public CrossDockEntryDataProvider LoadCrossDockEntryDataProvider(string entityId, int tenant, string userId = null)
        {
            crossDockEntryDataProvider = new CrossDockEntryDataProvider();
            WarehouseEntryQueryService warehouseEntryQueryService = new WarehouseEntryQueryService(tenant);
            warehouseEntryPM = warehouseEntryQueryService.GetSingle(entityId, true, false);
            if (warehouseEntryPM != null)
            {
                SetCrossDockGeneralFields(tenant);
                SetCrossDockTenantDetails(tenant);
                SetCrossDockTruckerDetails(tenant);
                SetCrossDockCountryName(tenant);
                SetCrossDockShipmentDetails(tenant);
                List<CardList> cardLists = GetCardList(tenant);
                SetCrossDockWarehouseDetails(cardLists, tenant);
                SetCrossDockShipperDetails(cardLists, tenant);
                SetCrossDockConsigneeDetails(cardLists, tenant);
                SetCrossDockLoggedUserDetails(userId, tenant);
                SetCrossDockWeightDetails();
                SetCrossDockVolumeDetails();
                if (warehouseEntryPM.WarehouseEntryPackages != null && warehouseEntryPM.WarehouseEntryPackages.Count > 0)
                {
                    crossDockEntryDataProvider.EntryPackages = FullPackage(warehouseEntryPM);
                    crossDockEntryDataProvider.NumberOfPackages = GetNumberOfWarehouseEntryPackages(warehouseEntryPM);
                }
            }
            return crossDockEntryDataProvider;
        }

        private void SetCrossDockWeightDetails()
        {

            double? grossWeightInKG = General.ComputeWeightInSelectedUnit((double?)warehouseEntryPM.TotalGrossWeight, warehouseEntryPM.GrossWeightUnitCode, "KG");
            double? grossWeightInLB = General.ComputeWeightInSelectedUnit((double?)warehouseEntryPM.TotalGrossWeight, warehouseEntryPM.GrossWeightUnitCode, "LB");
            if (grossWeightInKG != null)
            {
                crossDockEntryDataProvider.GrossWeightInKG = String.Format("{0:#,0.00}", grossWeightInKG.Value);
            }
            if (grossWeightInLB != null)
            {
                crossDockEntryDataProvider.GrossWeightInLB = String.Format("{0:#,0.00}", grossWeightInLB.Value);
            }

        }

        private void SetCrossDockVolumeDetails()
        {
            crossDockEntryDataProvider.VolumeInCBM = General.ComputeVolumeInSelectedUnit((double?)warehouseEntryPM.TotalVolume, warehouseEntryPM.VolumeUnitCode,"CBM");
            crossDockEntryDataProvider.VolumeInCBF = General.ComputeVolumeInSelectedUnit((double?)warehouseEntryPM.TotalVolume, warehouseEntryPM.VolumeUnitCode, "CBF");
        }
        private void SetWarehouseEntryPackageWeightDetails(EntryPackage entryPackage, double? packageWeight)
        {
            double? warehouseEntryPackageWeightInKG = General.ComputeWeightInSelectedUnit(packageWeight, entryPackage.WeightUnit, "KG");
            double? warehouseEntryPackageWeightInLB = General.ComputeWeightInSelectedUnit(packageWeight, entryPackage.WeightUnit, "LB");
            if (warehouseEntryPackageWeightInKG != null)
            {
                entryPackage.GrossWeightInKG = String.Format("{0:#,0.00}", warehouseEntryPackageWeightInKG.Value);
            }
            if (warehouseEntryPackageWeightInLB != null)
            {
                entryPackage.GrossWeightInLB = String.Format("{0:#,0.00}", warehouseEntryPackageWeightInLB.Value);
            }

        }
        private void SetWarehouseEntryPackageVolumeDetails(EntryPackage entryPackage, double? packageVolume)
        {
            entryPackage.VolumeInCBM = General.ComputeVolumeInSelectedUnit(packageVolume, entryPackage.VolumeUnit, "CBM");
            entryPackage.VolumeInCBF = General.ComputeVolumeInSelectedUnit(packageVolume, entryPackage.VolumeUnit, "CBF");
        }
        private int GetNumberOfWarehouseEntryPackages(WarehouseEntryPM warehouseEntryPM)
        {
            int result = 0;
            foreach (WarehouseEntryPackagePM package in warehouseEntryPM.WarehouseEntryPackages)
            {
                result += package.Quantity;
            }
            return result;
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
                item.Location = package.Location;
                SetWarehouseEntryPackageVolumeDetails(item, (double?)package.Volume);
                SetWarehouseEntryPackageWeightDetails(item, (double?)package.Weight);

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

        private void SetCrossDockGeneralFields(int tenant)
        {
            crossDockEntryDataProvider.House = warehouseEntryPM.HouseNumber;
            crossDockEntryDataProvider.Origin = warehouseEntryPM.Origin;
            crossDockEntryDataProvider.Master = warehouseEntryPM.MasterNumber;
            crossDockEntryDataProvider.BarCode = warehouseEntryPM.EntryReference;
            crossDockEntryDataProvider.ReceivedBy = warehouseEntryPM.ReceivedBy;
            crossDockEntryDataProvider.Destination = warehouseEntryPM.Destination;
            crossDockEntryDataProvider.EntryNumber = warehouseEntryPM.EntryNumber;
            crossDockEntryDataProvider.ShipperName = warehouseEntryPM.ShipperName;
            crossDockEntryDataProvider.IntenalNotes = warehouseEntryPM.Notes;
            crossDockEntryDataProvider.CustomerRef1 = warehouseEntryPM.CustomerRef1;
            crossDockEntryDataProvider.CustomerRef2 = warehouseEntryPM.CustomerRef2;
            crossDockEntryDataProvider.CustomerName = warehouseEntryPM.CustomerName;
            crossDockEntryDataProvider.ConsigneeName = warehouseEntryPM.ConsigneeName;
            crossDockEntryDataProvider.ActualEntryDate = warehouseEntryPM.ActualEntryDate;
            crossDockEntryDataProvider.ExpectedEntryDate = warehouseEntryPM.ExpectedEntryDate;
            crossDockEntryDataProvider.SpecialInstruction = warehouseEntryPM.SpecialInstruction;
            crossDockEntryDataProvider.EntryTruckerReference = warehouseEntryPM.TruckerReference;
            crossDockEntryDataProvider.TenantLogo = DataProviders.General.GetLogo(tenant);
            crossDockEntryDataProvider.UpdatedBy = GetContactNameById(warehouseEntryPM.UpdatedByUserId, tenant);
        }

        private string GetContactNameById(string contactId, int tenant)
        {
            string contactName = string.Empty;
            if (!string.IsNullOrEmpty(contactId))
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                contactName = contactQuery.GetContactNameId(contactId, tenant);
            }
            return contactName;
        }

        private List<CardList> GetCardList(int tenant)
        {
            List<string> cardIds = new List<string>();
            List<CardList> cardLists = new List<CardList>();
            cardIds.Add(warehouseEntryPM.WarehouseId);

            if (!string.IsNullOrEmpty(warehouseEntryPM.ShipperId) && !cardIds.Contains(warehouseEntryPM.ShipperId))
            {
                cardIds.Add(warehouseEntryPM.ShipperId);
            }
            if (!string.IsNullOrEmpty(warehouseEntryPM.ConsigneeId) && !cardIds.Contains(warehouseEntryPM.ConsigneeId))
            {
                cardIds.Add(warehouseEntryPM.ConsigneeId);
            }

            if (cardIds.Count > 0)
            {
                CardQuery cardQuery = new CardQuery(tenant);
                cardLists = cardQuery.GetCardListsByCardIds(cardIds, tenant);
            }

            return cardLists;
        }

        private void SetCrossDockTenantDetails(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant myTenant = tenantRepository.GetSingleTenantOnly(tenant);
            crossDockEntryDataProvider.TenantName = myTenant.Company;

            if (!string.IsNullOrEmpty(myTenant.AddressId))
            {
                AddressRepository addressRepository = new AddressRepository(tenant);
                Address address = addressRepository.GetSingleAddress(myTenant.AddressId, tenant);

                if (address != null)
                {
                    crossDockEntryDataProvider.TenantAddress = DataProviders.General.GetAddress(address);
                }

            }
        }

        private void SetCrossDockTruckerDetails(int tenant)
        {
            if (!string.IsNullOrEmpty(warehouseEntryPM.TruckerId))
            {
                Card truckerCard = CardRepository.GetSingleCard(warehouseEntryPM.TruckerId, tenant, true);
                if (truckerCard != null)
                {
                    crossDockEntryDataProvider.Trucker = truckerCard.EnglishName;
                    crossDockEntryDataProvider.EntryTruckerCode = truckerCard.Code;
                    crossDockEntryDataProvider.EntryTruckerName = truckerCard.EnglishName;
                }
            }
            crossDockEntryDataProvider.TruckerReference = warehouseEntryPM.TruckerReference;
        }

        private void SetCrossDockCountryName(int tenant)
        {
            if (!string.IsNullOrEmpty(warehouseEntryPM.ToPortId))
            {
                PortRepository portRepository = new PortRepository(tenant);
                Port destinationPort = portRepository.GetSinglePort(warehouseEntryPM.ToPortId, tenant);
                if (destinationPort != null)
                {
                    crossDockEntryDataProvider.DestinationCountryName = destinationPort.Country == null ? "" : destinationPort.Country.EnglishName;
                    crossDockEntryDataProvider.DestinationCountryCode = destinationPort.Country == null ? "" : destinationPort.Country.Code;
                    crossDockEntryDataProvider.DestinationPortCode = destinationPort.Code;
                }
            }
        }

        private void SetCrossDockWarehouseDetails(List<CardList> cardLists, int tenant)
        {
            if (!string.IsNullOrEmpty(warehouseEntryPM.WarehouseId))
            {
                CardQuery cardQuery = new CardQuery(tenant);
                CardList card = cardLists.Where(d => d.Id == warehouseEntryPM.WarehouseId).FirstOrDefault();
                if (card != null)
                {
                    crossDockEntryDataProvider.WarehouseCode = card.Code;
                    crossDockEntryDataProvider.WarehouseName = card.EnglishName;
                    crossDockEntryDataProvider.Notes = card.Notes;
                }

                AddressQuery addressQuery = new AddressQuery(tenant);
                AddressPM entityAddress = addressQuery.GetAddressPMByTypeAndCard(warehouseEntryPM.WarehouseId, "M", tenant);

                if (entityAddress != null)
                {
                    crossDockEntryDataProvider.WraehousePhone = entityAddress.PhoneNumber;
                    crossDockEntryDataProvider.WarehouseCountry = entityAddress.CountryName;
                    crossDockEntryDataProvider.WarehouseAddress1 = entityAddress.Address1;
                    crossDockEntryDataProvider.WarehouseAddress2 = entityAddress.Address2;
                    crossDockEntryDataProvider.WarehouseZipCode = entityAddress.ZipCode;
                    crossDockEntryDataProvider.WarehouseCity = entityAddress.City;
                    crossDockEntryDataProvider.WarehouseState = entityAddress.StateEnglishName;
                }
            }
        }

        private void SetCrossDockShipmentDetails(int tenant)
        {
            if (!string.IsNullOrEmpty(warehouseEntryPM.ShipmentId))
            {
                CrossDockEntryShipmentService crossDockEntryShipmentService = new CrossDockEntryShipmentService();
                crossDockEntryDataProvider = crossDockEntryShipmentService.FullCrossDockEntryProviderFromShipment(crossDockEntryDataProvider, warehouseEntryPM.ShipmentId, warehouseEntryPM.Tenant);

                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                Shipment shipment = shipmentRepository.GetSingleShipment(warehouseEntryPM.ShipmentId, tenant);
                if (shipment != null)
                {
                    crossDockEntryDataProvider.ProjectNumber = shipment.ProjectNumber;

                    if(shipment.ShipmentLevelCode == "H")
                    {
                        if(!string.IsNullOrEmpty(shipment.MasterShipmentDataId))
                        {
                            Shipment masterShipment = shipmentRepository.GetSingleShipment(shipment.MasterShipmentDataId, tenant);
                            if(masterShipment!= null)
                            {
                                crossDockEntryDataProvider.MasterProjectNumber = masterShipment.ProjectNumber;
                            }
                        }
                    }

                    else
                    {
                        crossDockEntryDataProvider.MasterProjectNumber = shipment.ProjectNumber;
                    }
                }
            }            
        }

        private void SetCrossDockShipperDetails(List<CardList> cardLists, int tenant)
        {
            if (!string.IsNullOrEmpty(warehouseEntryPM.ShipperId))
            {
                CardList card = cardLists.Where(d => d.Id == warehouseEntryPM.ShipperId).FirstOrDefault();

                if (card != null)
                {
                    crossDockEntryDataProvider.ShipperName = card.EnglishName;
                }

                Address address = GetMainAddressByCardId(tenant , warehouseEntryPM.ShipperId);
                if (address != null)
                {
                    crossDockEntryDataProvider.ShipperAddress = DataProviders.General.GetAddress(address);
                }
            }
        }

        private void SetCrossDockConsigneeDetails(List<CardList> cardLists, int tenant)
        {
            if (!string.IsNullOrEmpty(warehouseEntryPM.ConsigneeId))
            {
                CardList card = cardLists.Where(d => d.Id == warehouseEntryPM.ConsigneeId).FirstOrDefault();
                if (card != null)
                {
                    crossDockEntryDataProvider.ConsigneeName = card.EnglishName;
                }

                Address address = GetMainAddressByCardId(tenant , warehouseEntryPM.ConsigneeId);
                if (address != null)
                {
                    crossDockEntryDataProvider.ConsigneeAddress = DataProviders.General.GetAddress(address);
                }
            }

            crossDockEntryDataProvider.ConsigneeReference1 = warehouseEntryPM.ConsigneeReference1;
            crossDockEntryDataProvider.ConsigneeReference2 = warehouseEntryPM.ConsigneeReference2; 
        }

        private Address GetMainAddressByCardId(int tenant , string cardId)
        {
            AddressRepository addressRepository = new AddressRepository(tenant);
            Address address = addressRepository.GetMainAddressByCardId(cardId, tenant);
            return address;
        }

        private void SetCrossDockLoggedUserDetails(string userId, int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact loggeduser = contactRepository.GetSingleContact(userId, tenant);
            if (loggeduser != null)
            {
                crossDockEntryDataProvider.LoggedUserName = loggeduser.EnglishName;
                crossDockEntryDataProvider.LoggedUserEmail = loggeduser.Email;
            }
        }
    }

    public class DataTestProvider
    {
        public string Abed { get; set; }
    }
}