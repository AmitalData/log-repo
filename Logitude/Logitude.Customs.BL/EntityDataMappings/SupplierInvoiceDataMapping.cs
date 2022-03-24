
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{

    public partial class SupplierInvoiceDataMapping : IMapping<SupplierInvoicePM, SupplierInvoice>
    {

        public void CustomPMToPOCO(SupplierInvoicePM entityPM, SupplierInvoice entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.DeclarationId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.InvoiceCounterKey);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.DeclarationId = entityPM.DeclarationId;
                entityPOCO.InvoiceCounterKey = entityPM.InvoiceCounterKey;
                entityPOCO.Tenant = entityPM.Tenant;

            }


        }

        public void CustomPOCOToPM(SupplierInvoicePM entityPM, SupplierInvoice entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.IssueCountryName);
            CustomMappedPMProperties.Add(PMPropertyNames.PreferenceDocumentTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.InvoiceCurrencyTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.BuyerCountryName);
            CustomMappedPMProperties.Add(PMPropertyNames.PartyRelationshipName);
            CustomMappedPMProperties.Add(PMPropertyNames.BuyerRoleName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.VendorNumber);

            if (!string.IsNullOrWhiteSpace(entityPOCO.BuyerCountryCode))
            {
                CustomsCountryQueryService countryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM country = countryQueryService.GetSingle(entityPOCO.BuyerCountryCode, false, true);
                entityPM.BuyerCountryName = country.LocalName;
            }

            if (!string.IsNullOrWhiteSpace(entityPOCO.PartyRelationshipCode))
            {
                PartyRelationshipTypeQueryService partyRelationshipQueryService = new PartyRelationshipTypeQueryService(entityPOCO.Tenant);
                PartyRelationshipTypePM partyRelationship = partyRelationshipQueryService.GetSingle(entityPOCO.PartyRelationshipCode, false, true);
                entityPM.PartyRelationshipName = partyRelationship.LocalName;
            }

            if (!string.IsNullOrWhiteSpace(entityPOCO.BuyerRoleCode))
            {
                CustomerRoleTypeQueryService buyerRoleCodeQueryService = new CustomerRoleTypeQueryService(entityPOCO.Tenant);
                CustomerRoleTypePM buyerRoleCode = buyerRoleCodeQueryService.GetSingle(entityPOCO.BuyerRoleCode, false, true);
                entityPM.BuyerRoleName= buyerRoleCode.LocalName;
            } 

            if (!string.IsNullOrWhiteSpace(entityPOCO.IssueCountryCode))
            {
                CustomsCountryQueryService countryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM country = countryQueryService.GetSingle(entityPOCO.IssueCountryCode, false, true);
                entityPM.IssueCountryName = country.LocalName;
            }

            if (!string.IsNullOrWhiteSpace(entityPOCO.PreferenceDocumentTypeCode))
            {
                TradeAgreementQueryService tradeAgreementQueryService = new TradeAgreementQueryService(entityPOCO.Tenant);
                TradeAgreementPM preferenceDocumentType = tradeAgreementQueryService.GetSingle(entityPOCO.PreferenceDocumentTypeCode, false, true);
                entityPM.PreferenceDocumentTypeName = preferenceDocumentType.LocalName;
            }

            if (!string.IsNullOrWhiteSpace(entityPOCO.VendorId))
            {
                CustomsVendorQueryService vendorQueryService = new CustomsVendorQueryService(entityPOCO.Tenant);
                CustomsVendorPM vendor = vendorQueryService.GetSingle(entityPOCO.VendorId, false, true);
                entityPM.VendorName = vendor.VendorName;
                if (vendor != null) entityPM.VendorNumber = vendor.VendorNumber;
            }

            DeclarationQueryService declarationQueryService = new DeclarationQueryService(entityPOCO.Tenant);
            entityPM.IsValueForCustomsOnly = declarationQueryService.GetIsValueForCustomsOnlyFromDeclaration(entityPOCO.DeclarationId, entityPOCO.Tenant);
            DeclarationPM declarationPM = declarationQueryService.GetSingleDeclarationById(entityPOCO.DeclarationId, entityPOCO.Tenant);


            if (declarationPM != null && declarationPM.Direction == "E")
            {
                SupplierInvoiceModificationQueryService supplierInvoiceModificationQueryService = new SupplierInvoiceModificationQueryService(entityPOCO.Tenant);
                List<SupplierInvoiceModificationPM> listSupplierInvoiceModificationPMs = supplierInvoiceModificationQueryService.GetSupplierInvoiceModificationsForInvoice(entityPOCO.DeclarationId, entityPOCO.InvoiceCounterKey);
                foreach (SupplierInvoiceModificationPM item in listSupplierInvoiceModificationPMs)
                {
                    if (item.TypeCode == "67")
                    {
                        entityPM.ExportInsuranceAmount = String.Format("{0:0.00}", item.Amount) + " " + item.CurrencyTypeCode;
                    }
                    if(item.TypeCode == "144")
                    {
                        entityPM.ExportFreightAmount = String.Format("{0:0.00}", item.Amount) + " " + item.CurrencyTypeCode;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(entityPOCO.InvoiceCurrencyTypeCode))
            {
                CurrencyTypeQueryService currencyTypeQueryService = new CurrencyTypeQueryService(entityPOCO.Tenant);
                CurrencyTypePM currencyTypePM = currencyTypeQueryService.GetSingle(entityPOCO.InvoiceCurrencyTypeCode, false, true);
                entityPM.InvoiceCurrencyTypeName = currencyTypePM.LocalName;
            }
            if (!string.IsNullOrWhiteSpace(entityPOCO.InsruanceCurrencyTypeCode)) 
            {
                CurrencyTypeQueryService currencyTypeQueryService = new CurrencyTypeQueryService(entityPOCO.Tenant);
                CurrencyTypePM currencyTypePM = currencyTypeQueryService.GetSingle(entityPOCO.InsruanceCurrencyTypeCode, false, true);
                entityPM.InsruanceCurrencyTypeCodeName = currencyTypePM.LocalName;
            }



        }
    }
}



   