
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.Helpers.DataProviderHelpers
{
    public class CrossDockReleaseShipmentService
    {
        private int tenant;
        private ShipmentDataView shipmentDataView = null; 
        private ShipmentDataView masterShipmentDataView = null;
        private CrossDockReleaseDataProvider crossDockReleaseDataProvider;
        private CustomFieldResolver customFieldResolver;
        
        public CrossDockReleaseDataProvider FullCrossDockReleaseProviderFromShipment(string shipmentId ,CrossDockReleaseDataProvider dataProvider, int tenant)
        {
            crossDockReleaseDataProvider = dataProvider;
            this.tenant = tenant;
            if (!string.IsNullOrEmpty(shipmentId))
            {
                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                shipmentDataView = shipmentRepository.GetSingleShipmentDataView(shipmentId, tenant);
                if (shipmentDataView != null)
                {
                    if (!string.IsNullOrEmpty(shipmentDataView.MasterShipmentDataId))
                    {
                        masterShipmentDataView = shipmentRepository.GetSingleShipmentDataView(shipmentDataView.MasterShipmentDataId, tenant);
                        SetConnectedMasterShipmentGeneralFields();
                    }
                    SetShipmentGeneralFields();
                    SetOriginAndDestinationtShipmenFields();
                    SetShipmentPartnersFields();
                    SetShipmentCustomFields();
                    SetStorageDaysShipmenField();
                    SetProjectNumberField();
                    SetMasterShipmentNumberField();
                }
            }

            return crossDockReleaseDataProvider;
        }

        private void SetConnectedMasterShipmentGeneralFields()
        {
            if (masterShipmentDataView != null)
            {
                crossDockReleaseDataProvider.ImportManifest = masterShipmentDataView.ImportManifest;
                crossDockReleaseDataProvider.MasterImportManifest = masterShipmentDataView.ImportManifest;
            }
        }
        private void SetShipmentGeneralFields()
        {
            crossDockReleaseDataProvider.MainCarriageCarrierName = shipmentDataView.MainCarriageCarrierName;
            crossDockReleaseDataProvider.ShipmentNumber = shipmentDataView.ShipmentNumber;
            crossDockReleaseDataProvider.DeclarationDate = shipmentDataView.DeclarationDate;
            crossDockReleaseDataProvider.DeclarationNumber = shipmentDataView.DeclarationNumber;
            crossDockReleaseDataProvider.ValueofGoods = shipmentDataView.ValueOfGoods;
            crossDockReleaseDataProvider.GeneralDescriptionofGoods = shipmentDataView.DescriptionOfGoods;
            crossDockReleaseDataProvider.ValueofGoodsCurrency = GetValueofGoodsCurrencyCodeById(shipmentDataView.ValueOfGoodsCurrencyId, shipmentDataView.Tenant);
            crossDockReleaseDataProvider.IncotermCode = shipmentDataView.IncotermCode;
            crossDockReleaseDataProvider.IncotermName = GetIncotermNameById(shipmentDataView.IncotermId, shipmentDataView.Tenant);
            crossDockReleaseDataProvider.ConnectedShipmentTransportMode = shipmentDataView.TransportModeName;
            crossDockReleaseDataProvider.Trailer = shipmentDataView.TrailerNumber;
            crossDockReleaseDataProvider.StorageFreeDays = shipmentDataView.WarehouseStorageFreeDays;
            crossDockReleaseDataProvider.MainCarriageTruckerNumber = shipmentDataView.MainCarriageCarrierNumber;
        }
        private string GetIncotermNameById(string incotermId, int tenant)
        {
            string incotermName = string.Empty;
            if (!string.IsNullOrEmpty(incotermId))
            {
                IncotermQuery incotermQuery = new IncotermQuery(tenant);
                IncotermPM incotermPM = incotermQuery.GetSinglePM(incotermId, tenant);
                if (incotermPM != null) incotermName = incotermPM.Name;
            }
            return incotermName;
        }
        private void SetOriginAndDestinationtShipmenFields()
        {
            if (shipmentDataView.DirectionId == "D" && shipmentDataView.TransportModeId == "I")
            {
                crossDockReleaseDataProvider.Origin = shipmentDataView.MainCarriageFromCity;
                crossDockReleaseDataProvider.Destination = shipmentDataView.MainCarriageToCity;
                crossDockReleaseDataProvider.DestinationCountryName = GetCountryNameByCode(shipmentDataView.MainCarriageToCountryCode, tenant);
            }

            else
            {
                crossDockReleaseDataProvider.Origin = shipmentDataView.FromPortName;
                crossDockReleaseDataProvider.Destination = shipmentDataView.MainCarriageFinalDestinationPortName;
                crossDockReleaseDataProvider.DestinationCountryName = shipmentDataView.MainCarriageFinalDestinationCountryName;
            }
        }
        private void SetShipmentPartnersFields()
        {
            PartnerDetailsData shipperPartnerDetailsData = GetPartnerDetailsById(shipmentDataView.ShipperId, shipmentDataView.Tenant);
            PartnerDetailsData consigneePartnerDetails = GetPartnerDetailsById(shipmentDataView.ConsigneeId, shipmentDataView.Tenant);
            crossDockReleaseDataProvider.ShipperName = shipmentDataView.ShipperName;
            crossDockReleaseDataProvider.ConsigneeName = shipmentDataView.ConsigneeName;
            crossDockReleaseDataProvider.ShipperAddress = GetPartnerAddressByCardId(shipmentDataView.ShipperId, tenant);
            crossDockReleaseDataProvider.ConsigneeAddress = GetPartnerAddressByCardId(shipmentDataView.ConsigneeId, tenant);
            crossDockReleaseDataProvider.ShipperVATNumber = shipperPartnerDetailsData.VATNumber;
            crossDockReleaseDataProvider.ShipperContactPersonName = shipperPartnerDetailsData.PrimaryContactName;
            crossDockReleaseDataProvider.ShipperContactPersonEmail = shipperPartnerDetailsData.PrimaryContactEmail;
            crossDockReleaseDataProvider.ConsigneeVATNumber = consigneePartnerDetails.VATNumber;
            crossDockReleaseDataProvider.ConsigneeContactPersonName = consigneePartnerDetails.PrimaryContactName;
            crossDockReleaseDataProvider.ConsigneeContactPersonEmail = consigneePartnerDetails.PrimaryContactEmail;
        }
        private void SetShipmentCustomFields()
        {
            customFieldResolver = new CustomFieldResolver();
            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Shipment", shipmentDataView.Tenant).ToList();
            crossDockReleaseDataProvider.ShipmentField1 = ResolveCustomFieldValue("Field1", shipmentDataView.Field1, customFields);
            crossDockReleaseDataProvider.ShipmentField2 = ResolveCustomFieldValue("Field2", shipmentDataView.Field2, customFields);
            crossDockReleaseDataProvider.ShipmentField3 = ResolveCustomFieldValue("Field3", shipmentDataView.Field3, customFields);
            crossDockReleaseDataProvider.ShipmentField4 = ResolveCustomFieldValue("Field4", shipmentDataView.Field4, customFields);
            crossDockReleaseDataProvider.ShipmentField5 = ResolveCustomFieldValue("Field5", shipmentDataView.Field5, customFields);
            crossDockReleaseDataProvider.ShipmentField6 = ResolveCustomFieldValue("Field6", shipmentDataView.Field6, customFields);
            crossDockReleaseDataProvider.ShipmentField7 = ResolveCustomFieldValue("Field7", shipmentDataView.Field7, customFields);
            crossDockReleaseDataProvider.ShipmentField8 = ResolveCustomFieldValue("Field8", shipmentDataView.Field8, customFields);
            crossDockReleaseDataProvider.ShipmentField9 = ResolveCustomFieldValue("Field9", shipmentDataView.Field9, customFields);
            crossDockReleaseDataProvider.ShipmentField10 = ResolveCustomFieldValue("Field10", shipmentDataView.Field10, customFields);
        }
        private PartnerDetailsData GetPartnerDetailsById(string cardId, int tenant)
        {
            PartnerDetailsData partnerDetailsData = new PartnerDetailsData();
            if (!string.IsNullOrEmpty(cardId))
            {
                CardRepository cardRepository = new CardRepository(tenant);
                Card parnterCard = cardRepository.GetCardWithPrimaryContactById(cardId, tenant);
                if (parnterCard != null)
                {
                    partnerDetailsData.VATNumber = parnterCard.VatNumber;
                    if (parnterCard.PrimaryContact != null)
                    {
                        partnerDetailsData.PrimaryContactName = parnterCard.PrimaryContact.EnglishName;
                        partnerDetailsData.PrimaryContactEmail = parnterCard.PrimaryContact.Email;
                    }
                }
            }
            return partnerDetailsData;

        }
        private string GetCountryNameByCode(string countryCode, int tenant)
        {
            string countryName = string.Empty;
            if (!string.IsNullOrEmpty(countryCode))
            {
                CountryRepository countryRepository = new CountryRepository(tenant);
                Country country = countryRepository.GetSingleCountryByCode(countryCode, tenant, true);
                if (country != null) countryName = country.EnglishName;
            }
            return countryName;
        }
        private string GetPartnerAddressByCardId(string partnerId, int tenant)
        {
            string partnerAddress = string.Empty;
            AddressRepository addressRepository = new AddressRepository(tenant);
            Address address = addressRepository.GetMainAddressByCardId(partnerId, tenant);
            if (address != null) partnerAddress = DataProviders.General.GetAddress(address);
            return partnerAddress;
        }
        private string GetValueofGoodsCurrencyCodeById(string valueOfGoodsCurrencyId, int tenant)
        {
            string valueOfGoodsCurrencyCode = string.Empty;
            if (!string.IsNullOrEmpty(valueOfGoodsCurrencyId))
            {
                CurrencyQuery currencyQuery = new CurrencyQuery(tenant);
                CurrencyPM currencyPM = currencyQuery.GetSinglePM(valueOfGoodsCurrencyId, tenant);
                if (currencyPM != null)
                {
                    valueOfGoodsCurrencyCode = currencyPM.Code;
                }
            }
            return valueOfGoodsCurrencyCode;

        }
        private string ResolveCustomFieldValue(string fieldName, string fieldValue, List<ObjectField> objectFields)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(fieldValue))
            {
                ObjectField objectField = objectFields.Where(d => d.FieldName == fieldName).FirstOrDefault();
                if (objectField != null)
                {
                   
                    object value = customFieldResolver.GetFieldValue2(fieldValue, objectField, objectField.Tenant);
                    if (value != null) result = value.ToString();
                }
            }
            return result;

        }
        private void SetStorageDaysShipmenField()
        {
            int storageDays = 0;
            if (shipmentDataView.WarehouseLegActualReleaseDate != null && shipmentDataView.WarehouseLegActualEntryDate != null)
            {
                if (shipmentDataView.WarehouseLegActualReleaseDate >= shipmentDataView.WarehouseLegActualEntryDate)
                {
                    DateTime warehouseLegActualReleaseDate = (DateTime)shipmentDataView.WarehouseLegActualReleaseDate;
                    DateTime warehouseLegActualEntryDate = (DateTime)shipmentDataView.WarehouseLegActualEntryDate;
                    TimeSpan span = warehouseLegActualReleaseDate.Subtract(warehouseLegActualEntryDate);

                    storageDays = (int)Math.Round(span.TotalDays);
                }
            }
            crossDockReleaseDataProvider.StorageDays = storageDays;
        }
        private void SetProjectNumberField()
        {
            if (shipmentDataView.ShipmentLevelCode == "D") crossDockReleaseDataProvider.ProjectNumber = shipmentDataView.ProjectNumber;
            else if (shipmentDataView.ShipmentLevelCode == "H")
            {
                if (!string.IsNullOrEmpty(shipmentDataView.ProjectNumber)) crossDockReleaseDataProvider.ProjectNumber = shipmentDataView.ProjectNumber;
                else if (masterShipmentDataView != null) crossDockReleaseDataProvider.ProjectNumber = masterShipmentDataView.ProjectNumber;
            }
        }
        private void SetMasterShipmentNumberField()
        {
            if (shipmentDataView.ShipmentLevelCode == "C")
            {
                crossDockReleaseDataProvider.MasterShipmentNumber = shipmentDataView.ShipmentNumber;
            }

            else if (shipmentDataView.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(shipmentDataView.MasterShipmentDataId))
            {
                SetMasterShipmentNumberForConnectedHouse();
            }
        }
        private void SetMasterShipmentNumberForConnectedHouse()
        {
            if(masterShipmentDataView != null)
            {
                crossDockReleaseDataProvider.MasterShipmentNumber = masterShipmentDataView.ShipmentNumber;
            }
        }
    }

    public class PartnerDetailsData
    {
        public string VATNumber { get; set; }
        public string PrimaryContactName { get; set; }
        public string PrimaryContactEmail { get; set; }
    }
}