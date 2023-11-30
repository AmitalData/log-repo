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
    public class AirlineMapping
    {
        public static void MapEntity(AirlinePM entityPM, Airline entityPOCO, bool isNewState, Card entityCard)
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
            entityPOCO.AWBAccount = entityPM.AWBAccount;
            entityPOCO.Prefix = entityPM.Prefix;
            entityPOCO.AccountNumber = entityPM.AccountNumber;
            entityPOCO.CheckDigit = entityPM.CheckDigit;
            entityPOCO.LimitedLength = entityPM.LimitedLength;            
            entityPOCO.TTY = entityPM.TTY;
            entityPOCO.IsChampRegistered = entityPM.IsChampRegistered;
            entityPOCO.ChampNeedsRegistration = entityPM.ChampNeedsRegistration;
            entityPOCO.GLSHKPIMA = entityPM.GLSHKPIMA;
            entityPOCO.IsGLSHKRegistered = entityPM.IsGLSHKRegistered;
            entityPOCO.GLSHKNeedsRegistration = entityPM.GLSHKNeedsRegistration;           
            entityPOCO.ChampFWB = entityPM.ChampFWB;
            entityPOCO.ChampFHL = entityPM.ChampFHL;
            entityPOCO.ChampFSU = entityPM.ChampFSU;
            entityPOCO.ChampFSRFSA = entityPM.ChampFSRFSA;
            entityPOCO.ChampFVRFVA = entityPM.ChampFVRFVA;
            entityPOCO.ChampFFRFFA = entityPM.ChampFFRFFA;
            entityPOCO.GLSHKFWB = entityPM.GLSHKFWB;
            entityPOCO.GLSHKFHL = entityPM.GLSHKFHL;
            entityPOCO.GLSHKFSU = entityPM.GLSHKFSU;
            entityPOCO.GLSHKFSRFSA = entityPM.GLSHKFSRFSA;
            entityPOCO.GLSHKFVRFVA = entityPM.GLSHKFVRFVA;
            entityPOCO.GLSHKFFRFFA = entityPM.GLSHKFFRFFA;
            entityPOCO.IsAllowedInAirlinesRestriction = entityPM.IsAllowedInAirlinesRestriction;
            entityPOCO.RegistrationNotes = entityPM.RegistrationNotes;
            entityPOCO.ChampRegistrationRequested = entityPM.ChampRegistrationRequested;
            entityPOCO.GLSHKRegistrationRequested = entityPM.GLSHKRegistrationRequested;
            entityPOCO.HasAdaptations = entityPM.HasAdaptations;
            entityPOCO.ICAO = entityPM.ICAO;
            entityPOCO.RegistrationUpdatedBy = entityPM.RegistrationUpdatedBy;
            entityPOCO.IsManagingProduct = entityPM.IsManagingProduct;
            entityPOCO.IsProductMandatory = entityPM.IsProductMandatory;
            entityPOCO.IsDescriptionOfGoodsFromList = entityPM.IsDescriptionOfGoodsFromList;
            entityPOCO.ScheduleDays = entityPM.ScheduleDays;
            entityPOCO.NoAvailabilityInFVAMessages = entityPM.NoAvailabilityInFVAMessages;
            entityPOCO.IsDeclined = entityPM.IsDeclined;
            entityPOCO.DeclineNotes = entityPM.DeclineNotes;
            entityPOCO.PrimaryContactName = entityPM.PrimaryContactName;
            entityPOCO.PrimaryContactEmail = entityPM.PrimaryContactEmail;
            entityPOCO.PrimaryContactPhone = entityPM.PrimaryContactPhone;

            // Map To Card
            entityCard.UpdateDate = entityPM.UpdateDate;
            entityCard.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityCard.EnableConsolidationInvoices = entityPM.EnableConsolidationInvoices;
            entityCard.Website = entityPM.Website;            
            entityCard.Code = entityPM.Code;
            entityCard.EnglishName = entityPM.EnglishName;
            entityCard.InActive = entityPM.InActive;
            entityCard.LocalName = entityPM.LocalName;
            entityCard.PaymentTermId = entityPM.PaymentTermId;
            entityCard.ReceivablesAccountingCard = entityPM.ReceivablesAccountingCard;
            entityCard.AccountingVATSplit = entityPM.AccountingVATSplit;
            entityCard.BillToId = entityPM.BillToId;
            entityCard.PayablesAccountingCard = entityPM.PayablesAccountingCard;
            entityCard.VatNumber = entityPM.VatNumber;
            entityCard.Notes = entityPM.Remark;            
            entityCard.InvoiceCurrencyId = entityPM.InvoiceCurrencyId;
            entityCard.VatTypeId = entityPM.VatTypeId;
            entityCard.Swift = entityPM.Swift;
            entityCard.AccountNumber = entityPM.BankAccountNumber;
            entityCard.BankName = entityPM.BankName;
            entityCard.BankAddress = entityPM.BankAddress;
            entityCard.IBANNumber = entityPM.IBANNumber;
            entityCard.IRSNumber = entityPM.IRSNumber;
            entityCard.IRSPlace = entityPM.IRSPlace;
            entityCard.ExternalAccountingBusinessArea = entityPM.ExternalAccountingBusinessArea;
            entityCard.SATPaymentMethodCode = entityPM.PaymentMethodCode;
            entityCard.ExternalId2 = entityPM.ExternalId2;
            entityCard.SATForeignRFC = entityPM.SATForeignRFC;
            entityCard.MetodoPagoCode = entityPM.MetodoPagoCode;
            entityCard.UsoCFDICode = entityPM.UsoCFDICode;
            entityCard.ImageDetailId = entityPM.ImageDetailId;
            entityCard.RegimenFiscalCode = entityPM.RegimenFiscalCode;
            entityCard.SATCustomerName = entityPM.SATReceptorName;
            entityCard.ExportLocalCustomerGroupId = entityPM.ExportLocalCustomerGroupId;
            entityCard.ImportLocalCustomerGroupId = entityPM.ImportLocalCustomerGroupId;
            entityCard.SingleInvoiceTemplateId = entityPM.Card != null ? entityPM.Card.SingleInvoiceTemplateId : null;
            entityCard.CustomsInvoiceTemplateId = entityPM.Card != null ? entityPM.Card.CustomsInvoiceTemplateId : null;
            entityCard.ConsolidationInvoiceTemplateId = entityPM.Card != null ? entityPM.Card.ConsolidationInvoiceTemplateId : null;
            entityCard.ManifestInvoiceTemplateId = entityPM.Card != null ? entityPM.Card.ManifestInvoiceTemplateId : null;

            if (!entityPM.IsFirstContactToAdd)
            {
                entityCard.PrimaryContactId = entityPM.PrimaryContactId;
            }

            BuildSearchFields(entityPM, entityCard);
        }

        private static void BuildSearchFields(AirlinePM entityPM, Card entityCard)
        {
            string mySearchFields = "";
            
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.VatNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ReceivablesAccountingCard);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PayablesAccountingCard);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Prefix);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.CityName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityCard.CountryName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ICAO);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityCard.SearchFields = mySearchFields;
        }
    }
}