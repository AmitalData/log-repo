using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.Helpers.DataProviderHelpers
{
    public class CrossDockEntryShipmentService
    {
        private ShipmentDataView shipmentDataView = null;
        private CrossDockEntryDataProvider crossDockEntryDataProvider;
        private CustomFieldResolver customFieldResolver;

        public CrossDockEntryDataProvider FullCrossDockEntryProviderFromShipment(CrossDockEntryDataProvider dataProvider, string shipmentId, int tenant)
        {
            crossDockEntryDataProvider = dataProvider;
            if (!string.IsNullOrEmpty(shipmentId))
            {
                ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
                shipmentDataView = shipmentRepository.GetSingleShipmentDataView(shipmentId, tenant);
                if (shipmentDataView != null)
                {
                    SetShipmentGeneralFields();
                    SetShipmentCustomFields();
                }
            }
            return crossDockEntryDataProvider;
        }

        private void SetShipmentGeneralFields()
        {
            crossDockEntryDataProvider.MainCarriageCarrierName = shipmentDataView.MainCarriageCarrierName;
            crossDockEntryDataProvider.ShipmentNumber = shipmentDataView.ShipmentNumber;
            crossDockEntryDataProvider.MainCarriageCarrierNumber = shipmentDataView.MainCarriageCarrierNumber;
            crossDockEntryDataProvider.FinalDestination = shipmentDataView.LastFinalDestination;
            crossDockEntryDataProvider.IncotermCode = shipmentDataView.IncotermCode;
            crossDockEntryDataProvider.ValueofGoods = shipmentDataView.ValueOfGoods;
            crossDockEntryDataProvider.GeneralDescriptionofGoods = shipmentDataView.DescriptionOfGoods;
            crossDockEntryDataProvider.NotifyName = shipmentDataView.Notify1Name;
            crossDockEntryDataProvider.ValueofGoodsCurrency = GetValueofGoodsCurrencyCodeById(shipmentDataView.ValueOfGoodsCurrencyId, shipmentDataView.Tenant);
            crossDockEntryDataProvider.IncotermName = GetIncotermNameById(shipmentDataView.IncotermId, shipmentDataView.Tenant);
            SetNotifyAddressById(shipmentDataView.Notify1AddressId, shipmentDataView.Tenant);
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

        private string GetIncotermNameById(string incotermId, int tenant)
        {
            string incotermName = string.Empty;
            if (!string.IsNullOrEmpty(incotermId))
            {
                IncotermQuery incotermQuery = new IncotermQuery(tenant);
                IncotermPM incotermPM = incotermQuery.GetSinglePM(incotermId, tenant);
                if (incotermPM != null)
                {
                    incotermName = incotermPM.Name;
                }
            }
            return incotermName;
        }

        private void SetNotifyAddressById(string notifyId, int tenant)
        {
            if (!string.IsNullOrEmpty(shipmentDataView.Notify1AddressId))
            {
                AddressRepository addressRepository = new AddressRepository(tenant);
                Address address = addressRepository.GetSingleAddress(shipmentDataView.Notify1AddressId, tenant);

                if (address != null)
                {
                    crossDockEntryDataProvider.NotifyAddress = DataProviders.General.GetAddress(address);
                }

            }
        }
        
        private void SetShipmentCustomFields()
        {
            customFieldResolver = new CustomFieldResolver(shipmentDataView.Tenant);
            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Shipment", shipmentDataView.Tenant).ToList();
            crossDockEntryDataProvider.ShipmentField1 = ResolveCustomFieldValue("Field1", shipmentDataView.Field1, customFields);
            crossDockEntryDataProvider.ShipmentField2 = ResolveCustomFieldValue("Field2", shipmentDataView.Field2, customFields);
            crossDockEntryDataProvider.ShipmentField3 = ResolveCustomFieldValue("Field3", shipmentDataView.Field3, customFields);
            crossDockEntryDataProvider.ShipmentField4 = ResolveCustomFieldValue("Field4", shipmentDataView.Field4, customFields);
            crossDockEntryDataProvider.ShipmentField5 = ResolveCustomFieldValue("Field5", shipmentDataView.Field5, customFields);
            crossDockEntryDataProvider.ShipmentField6 = ResolveCustomFieldValue("Field6", shipmentDataView.Field6, customFields);
            crossDockEntryDataProvider.ShipmentField7 = ResolveCustomFieldValue("Field7", shipmentDataView.Field7, customFields);
            crossDockEntryDataProvider.ShipmentField8 = ResolveCustomFieldValue("Field8", shipmentDataView.Field8, customFields);
            crossDockEntryDataProvider.ShipmentField9 = ResolveCustomFieldValue("Field9", shipmentDataView.Field9, customFields);
            crossDockEntryDataProvider.ShipmentField10 = ResolveCustomFieldValue("Field10", shipmentDataView.Field10, customFields);
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
    }
}