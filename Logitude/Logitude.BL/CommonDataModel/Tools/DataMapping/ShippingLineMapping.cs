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
    public class ShippingLineMapping
    {
        public static void MapEntity(ShippingLinePM entityPM, ShippingLine entityPOCO, bool isNewState, Card entityCard)
        {
            if (isNewState)
            {
                entityCard.Code = entityPM.Code;
                entityCard.Id = entityPM.Id = entityPM.Id;
                entityCard.Tenant = entityPOCO.Tenant = entityPM.Tenant;
                entityCard.CreateDate = entityPM.CreateDate;
                entityCard.CreatedByUserId = entityPM.CreatedByUserId;
            }

            entityPOCO.AddedManually = entityPM.AddedManually;
            entityPOCO.OurCreditNumber = entityPM.OurCreditNumber;
            entityPOCO.ShippingAgentId = entityPM.ShippingAgentId;
            entityPOCO.SCACCode = entityPM.SCACCode;
            entityPOCO.IsINTTRARegistered = entityPM.IsINTTRARegistered;
            entityPOCO.INTTRARegistrationNotes = entityPM.INTTRARegistrationNotes;
            entityPOCO.PrimaryContactName = entityPM.PrimaryContactName;
            entityPOCO.PrimaryContactEmail = entityPM.PrimaryContactEmail;
            entityPOCO.PrimaryContactPhone = entityPM.PrimaryContactPhone;
            entityPOCO.CBSA = entityPM.CBSA;
            entityPOCO.CAAT = entityPM.CAAT;

            entityCard.UpdateDate = entityPM.UpdateDate;
            entityCard.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityCard.EnableConsolidationInvoices = entityPM.EnableConsolidationInvoices;
            entityCard.Code = entityPM.Code;
            entityCard.PartnerTypeId = entityPM.CarrierTypeId;
            entityCard.EnglishName = entityPM.EnglishName;
            entityCard.InActive = entityPM.InActive;
            entityCard.LocalName = entityPM.LocalName;
            entityCard.PaymentTermId = entityPM.PaymentTermId;
            entityCard.ReceivablesAccountingCard = entityPM.ReceivablesAccountingCard;
            entityCard.PayablesAccountingCard = entityPM.PayablesAccountingCard;
            entityCard.VatNumber = entityPM.VatNumber;
            entityCard.Notes = entityPM.Remark;
            entityCard.Website = entityPM.Website;
            entityCard.InvoiceCurrencyId = entityPM.InvoiceCurrencyId;
            entityCard.VatTypeId = entityPM.VatTypeId;
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
            entityCard.SATForeignRFC = entityPM.SATForeignRFC;
            entityCard.MetodoPagoCode = entityPM.MetodoPagoCode;
            entityCard.UsoCFDICode = entityPM.UsoCFDICode;
            if (!entityPM.IsFirstContactToAdd)
            {
                entityCard.PrimaryContactId = entityPM.PrimaryContactId;
            }
            

            BuildSearchFields(entityPM, entityCard);
        }

        private static void BuildSearchFields(ShippingLinePM entityPM, Card entityCard)
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
