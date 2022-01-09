using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class WarehouseMapping
    {
        public static void MapEntity(WarehousePM entityPM, Warehouse entityPOCO, bool isNewEntity, Card entityCard)
        {
            if (isNewEntity)
            {
                entityCard.Code = entityPM.Code;
                entityCard.Id = entityPM.Id = entityPM.Id;
                entityCard.Tenant = entityPOCO.Tenant = entityPM.Tenant;
                entityCard.CreateDate = entityPM.CreateDate;
                entityCard.CreatedByUserId = entityPM.CreatedByUserId;
            }

            entityPOCO.AddedManually = entityPM.AddedManually;
            entityPOCO.FirmCode = entityPM.FirmCode;
            entityPOCO.TypeCode = entityPM.TypeCode;
            entityPOCO.MyWarehouse = entityPM.MyWarehouse;
            entityPOCO.PrimaryContactName = entityPM.PrimaryContactName;
            entityPOCO.PrimaryContactEmail = entityPM.PrimaryContactEmail;
            entityPOCO.PrimaryContactPhone = entityPM.PrimaryContactPhone;
            entityPOCO.ChargeStorage = entityPM.ChargeStorage;
            entityPOCO.CurrencyId = entityPM.CurrencyId;
            entityPOCO.AirWeightMeasurementCode = entityPM.AirWeightMeasurementCode;
            entityPOCO.OceanWeightMeasurementCode = entityPM.OceanWeightMeasurementCode;
            entityPOCO.InlandWeightMeasurementCode = entityPM.InlandWeightMeasurementCode;
            entityPOCO.AirWeightRoundingCode = entityPM.AirWeightRoundingCode;
            entityPOCO.OceanWeightRoundingCode = entityPM.OceanWeightRoundingCode;
            entityPOCO.InlandWeightRoundingCode = entityPM.InlandWeightRoundingCode;
            entityPOCO.Field1 = entityPM.Field1 != null ? entityPM.Field1.Value : null;
            entityPOCO.Field2 = entityPM.Field2 != null ? entityPM.Field2.Value : null;
            entityPOCO.Field3 = entityPM.Field3 != null ? entityPM.Field3.Value : null;
            entityPOCO.Field4 = entityPM.Field4 != null ? entityPM.Field4.Value : null;
            entityPOCO.Field5 = entityPM.Field5 != null ? entityPM.Field5.Value : null;
            entityPOCO.Field6 = entityPM.Field6 != null ? entityPM.Field6.Value : null;
            entityPOCO.Field7 = entityPM.Field7 != null ? entityPM.Field7.Value : null;
            entityPOCO.Field8 = entityPM.Field8 != null ? entityPM.Field8.Value : null;
            entityPOCO.Field9 = entityPM.Field9 != null ? entityPM.Field9.Value : null;
            entityPOCO.Field10 = entityPM.Field10 != null ? entityPM.Field10.Value : null;

            entityCard.UpdateDate = entityPM.UpdateDate;
            entityCard.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityCard.EnableConsolidationInvoices = entityPM.EnableConsolidationInvoices;
            entityCard.Code = entityPM.Code;
            entityCard.PartnerTypeId = entityPM.PartnerTypeId;
            entityCard.EnglishName = entityPM.EnglishName;
            entityCard.InActive = entityPM.InActive;
            entityCard.LocalName = entityPM.LocalName;
            entityCard.PaymentTermId = entityPM.PaymentTermId;
            entityCard.ReceivablesAccountingCard = entityPM.ReceivablesAccountingCard;
            entityCard.PayablesAccountingCard = entityPM.PayablesAccountingCard;
            entityCard.AccountingVATSplit = entityPM.AccountingVATSplit;
            entityCard.BillToId = entityPM.BillToId;
            entityCard.VatNumber = entityPM.VatNumber;
            entityCard.Notes = entityPM.Notes;
            entityCard.Website = entityPM.Website;
            entityCard.VatTypeId = entityPM.VatTypeId;
            entityCard.InvoiceCurrencyId = entityPM.InvoiceCurrencyId;
            entityCard.AccountNumber = entityPM.AccountNumber;
            entityCard.BankAddress = entityPM.BankAddress;
            entityCard.BankName = entityPM.BankName;
            entityCard.IBANNumber = entityPM.IBANNumber;
            entityCard.Swift = entityPM.Swift;
            entityCard.IRSNumber = entityPM.IRSNumber;
            entityCard.IRSPlace = entityPM.IRSPlace;
            entityCard.ExternalAccountingBusinessArea = entityPM.ExternalAccountingBusinessArea;
            entityCard.SATPaymentMethodCode = entityPM.PaymentMethodCode;
            entityCard.ExternalId2 = entityPM.ExternalId2;
            entityCard.MetodoPagoCode = entityPM.MetodoPagoCode;
            entityCard.UsoCFDICode = entityPM.UsoCFDICode;
            entityCard.SATForeignRFC = entityPM.SATForeignRFC;
            entityCard.StorageFreeDays = entityPM.StorageFreeDays;

            if (!entityPM.IsFirstContactToAdd)
            {
                entityCard.PrimaryContactId = entityPM.PrimaryContactId;
            }

            BuildSearchFields(entityPM, entityCard);
        }

        private static void BuildSearchFields(WarehousePM entityPM, Card entityCard)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.VatNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ReceivablesAccountingCard);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PayablesAccountingCard);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.CityName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.CountryName);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityCard.SearchFields = mySearchFields;
        }
    }
}