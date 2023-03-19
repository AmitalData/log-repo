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
    public class AgentMapping
    {
        public static void MapEntity(AgentPM entityPM, Agent entityPOCO, bool isNewState, Card entityCard)
        {
            if (isNewState)
            {
                entityCard.Code = entityPM.Code;
                entityCard.Id = entityPM.Id = entityPM.Id;
                entityCard.Tenant = entityPOCO.Tenant = entityPM.Tenant;
                entityCard.CreateDate = entityPM.CreateDate;
                entityCard.CreatedByUserId = entityPM.CreatedByUserId;
            }

            entityCard.UpdateDate = entityPM.UpdateDate;
            entityCard.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityCard.ReceivablesAccountingCard = entityPM.ReceivablesAccountingCard;
            entityCard.AccountingVATSplit = entityPM.AccountingVATSplit;
            entityCard.BillToId = entityPM.BillToId;
            entityCard.PayablesAccountingCard = entityPM.PayablesAccountingCard;
            entityCard.EnglishName = entityPM.EnglishName;
            entityCard.InActive = entityPM.InActive;
            entityCard.LocalName = entityPM.LocalName;
            entityCard.Notes = entityPM.Notes;
            entityCard.PartnerTypeId = entityPM.PartnerTypeId;
            entityCard.PaymentTermId = entityPM.PaymentTermId;
            entityCard.VatNumber = entityPM.VatNumber;
            entityCard.Website = entityPM.Website;
            entityCard.InvoiceCurrencyId = entityPM.InvoiceCurrencyId;
            entityCard.VatTypeId = entityPM.VatTypeId;
            entityCard.AccountNumber = entityPM.AccountNumber;
            entityCard.BankName = entityPM.BankName;
            entityCard.BankAddress = entityPM.BankAddress;
            entityCard.IBANNumber = entityPM.IBANNumber;
            entityCard.Swift = entityPM.Swift;
            entityCard.EnableConsolidationInvoices = entityPM.EnableConsolidationInvoices;
            entityCard.IRSNumber = entityPM.IRSNumber;
            entityCard.IRSPlace = entityPM.IRSPlace;
            entityCard.ExternalAccountingBusinessArea = entityPM.ExternalAccountingBusinessArea;
            entityCard.SATPaymentMethodCode = entityPM.PaymentMethodCode;
            entityCard.ExternalId2 = entityPM.ExternalId2;
            entityCard.SATForeignRFC = entityPM.SATForeignRFC;
            entityCard.MetodoPagoCode = entityPM.MetodoPagoCode;
            entityCard.UsoCFDICode = entityPM.UsoCFDICode;
            entityCard.StorageFreeDays = entityPM.StorageFreeDays;
            entityCard.ImageDetailId = entityPM.ImageDetailId;
            entityCard.RegimenFiscalCode = entityPM.RegimenFiscalCode;
            entityCard.SATCustomerName = entityPM.SATReceptorName;
            entityCard.ExportLocalCustomerGroupId = entityPM.ExportLocalCustomerGroupId;
            entityCard.ImportLocalCustomerGroupId = entityPM.ImportLocalCustomerGroupId;
            entityCard.EORInumber = entityPM.EORInumber;
            
            entityCard.SingleInvoiceTemplateId = entityPM.Card != null ? entityPM.Card.SingleInvoiceTemplateId : null;
            entityCard.CustomsInvoiceTemplateId = entityPM.Card != null ? entityPM.Card.CustomsInvoiceTemplateId : null;
            entityCard.ConsolidationInvoiceTemplateId = entityPM.Card != null ? entityPM.Card.ConsolidationInvoiceTemplateId : null;
            entityCard.ManifestInvoiceTemplateId = entityPM.Card != null ? entityPM.Card.ManifestInvoiceTemplateId : null;

            entityPOCO.CASSCode = entityPM.CASSCode;
            entityPOCO.IATACode = entityPM.IATACode;
            entityPOCO.RegulatedAgentCode = entityPM.RegulatedAgentCode;
            entityPOCO.AgentSharedLogisticsKey = entityPM.AgentSharedLogisticsKey;
            entityPOCO.IsCreditLimitEnabled = entityPM.IsCreditLimitEnabled;
            entityPOCO.BlockNewInvoiceCreation = entityPM.BlockNewInvoiceCreation;
            entityPOCO.BlockNewShipmentCreation = entityPM.BlockNewShipmentCreation;
            entityPOCO.PrimaryContactName = entityPM.PrimaryContactName;
            entityPOCO.PrimaryContactEmail = entityPM.PrimaryContactEmail;
            entityPOCO.PrimaryContactPhone = entityPM.PrimaryContactPhone;

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

            if (!entityPM.IsFirstContactToAdd)
            {
                entityCard.PrimaryContactId = entityPM.PrimaryContactId;
            }

            BuildSearchFields(entityPM, entityCard);
        }

        private static void BuildSearchFields(AgentPM entityPM, Card entityCard)
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