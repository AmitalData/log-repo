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
    public class CardMapping
    {
        public static void MapEntity(CardPM entityPM, Card entityPOCO, bool isNewState)
        {
            if (isNewState)
            {
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.CreateDate = entityPM.CreateDate;
                entityPOCO.CreatedByUserId = entityPM.CreatedByUserId;
            }

            entityPOCO.UpdateDate = entityPM.UpdateDate;
            entityPOCO.UpdatedByUserId = entityPM.UpdatedByUserId;
            entityPOCO.ReceivablesAccountingCard = entityPM.ReceivablesAccountingCard;
            entityPOCO.PayablesAccountingCard = entityPM.PayablesAccountingCard;
            entityPOCO.EnglishName = entityPM.EnglishName;
            entityPOCO.InActive = entityPM.InActive;
            entityPOCO.LocalName = entityPM.LocalName;
            entityPOCO.Notes = entityPM.Notes;
            entityPOCO.PartnerTypeId = entityPM.PartnerTypeId;
            entityPOCO.PaymentTermId = entityPM.PaymentTermId;
            entityPOCO.VatNumber = entityPM.VatNumber;
            entityPOCO.Website = entityPM.Website;
            entityPOCO.VatTypeId = entityPM.VatTypeId;
            entityPOCO.InvoiceCurrencyId = entityPM.InvoiceCurrencyId;
            entityPOCO.InvitationDate = entityPM.InvitationDate;
            entityPOCO.SharedLogisticsInvitationStatusCode = entityPM.SharedLogisticsInvitationStatusCode;
            entityPOCO.LastLoginDate = entityPM.LastLoginDate;
            entityPOCO.ClassifierId = entityPM.ClassifierId;
            entityPOCO.CollectorId = entityPM.CollectorId;
            entityPOCO.PrimaryContactId = entityPM.PrimaryContactId;
            entityPOCO.IsCustomer = entityPM.IsCustomer;
            entityPOCO.EnableConsolidationInvoices = entityPM.EnableConsolidationInvoices;
            entityPOCO.CityName = entityPM.CityName;
            entityPOCO.CountryId = entityPM.CountryId;
            entityPOCO.CountryName = entityPM.CountryName;
            entityPOCO.IsActiveForMobile = entityPM.IsActiveForMobile;
            entityPOCO.IRSPlace = entityPM.IRSPlace;
            entityPOCO.IRSNumber = entityPM.IRSNumber;
            entityPOCO.SupportNotes = entityPM.SupportNotes;
            entityPOCO.GLAccountId = entityPM.GLAccountId;
            entityPOCO.ExternalAccountingBusinessArea = entityPM.ExternalAccountingBusinessArea;
            entityPOCO.SATPaymentMethodCode = entityPM.SATPaymentMethodCode;
            entityPOCO.ExternalId2 = entityPM.ExternalId2;
            entityPOCO.SATForeignRFC = entityPM.SATForeignRFC;
            entityPOCO.MetodoPagoCode = entityPM.MetodoPagoCode;
            entityPOCO.UsoCFDICode = entityPM.UsoCFDICode;
            entityPOCO.UsoCFDICode = entityPM.UsoCFDICode;
            entityPOCO.StateName = entityPM.StateName;
            entityPOCO.IsInternationalPartner = entityPM.IsInternationalPartner;
            entityPOCO.IsAutonomy = entityPM.IsAutonomy;

            BuildSearchFields(entityPM, entityPOCO);
        }

        private static void BuildSearchFields(CardPM entityPM, Card entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.VatNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ReceivablesAccountingCard);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PayablesAccountingCard);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPOCO.CityName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPOCO.CountryName);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}