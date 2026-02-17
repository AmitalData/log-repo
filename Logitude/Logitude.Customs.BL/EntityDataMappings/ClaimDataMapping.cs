
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
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel.Repositories;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ClaimDataMapping: IMapping<ClaimPM, Claim>
   {

        public void CustomPMToPOCO(ClaimPM entityPM, Claim entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
               
                entityPOCO.Id = entityPM.Id;             
                entityPOCO.Tenant = entityPM.Tenant;
            }

            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        private void BuildSearchFields(ClaimPM entityPM, Claim entityPOCO, bool isNewEntity)
        {
            string result = "";

            if (!string.IsNullOrEmpty(entityPM.TapagNumber)) // from tapag
            {
                result = string.IsNullOrEmpty(result) ? entityPM.TapagNumber : result + "," + entityPM.TapagNumber;
            }

            if (isNewEntity)
            {
                foreach (ClaimsRelatedEntityPM item in entityPM.ClaimsRelatedEntities)
                {
                    if (!string.IsNullOrEmpty(item.ClaimEntityNumber))
                    {
                        result = string.IsNullOrEmpty(result) ? item.ClaimEntityNumber : result + "," + item.ClaimEntityNumber;
                    }

                    if (!string.IsNullOrEmpty(item.ExternalClaimNumber))
                    {
                        result = string.IsNullOrEmpty(result) ? item.ExternalClaimNumber : result + "," + item.ExternalClaimNumber;
                    }

                    if (!string.IsNullOrEmpty(item.ClaimRequestNumber))
                    {
                        result = string.IsNullOrEmpty(result) ? item.ClaimRequestNumber : result + "," + item.ClaimRequestNumber;
                    }
                }
            }
            else
            {
                ClaimsRelatedEntityRepository claimsRelatedEntityRepository = new ClaimsRelatedEntityRepository(entityPM.Tenant);
                ClaimKeys entityKeys = new ClaimKeys() { Id = entityPM.Id };
                List<ClaimsRelatedEntity> claimsRelatedEntityList = claimsRelatedEntityRepository.GetMulti(entityKeys);
                foreach (var item in claimsRelatedEntityList)
                {
                    if (!string.IsNullOrEmpty(item.ClaimEntityNumber))
                    {
                        result = string.IsNullOrEmpty(result) ? item.ClaimEntityNumber : result + "," + item.ClaimEntityNumber;
                    }

                    if (!string.IsNullOrEmpty(item.ExternalClaimNumber))
                    {
                        result = string.IsNullOrEmpty(result) ? item.ExternalClaimNumber : result + "," + item.ExternalClaimNumber;
                    }

                    if (!string.IsNullOrEmpty(item.ClaimRequestNumber))
                    {
                        result = string.IsNullOrEmpty(result) ? item.ClaimRequestNumber : result + "," + item.ClaimRequestNumber;
                    }
                }
            }
            
            entityPM.SearchFields = result.ToLower();
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(ClaimPM entityPM, Claim entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ImporterClaimTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.PassportCountryTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.PassportTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.ClaimSubmiterTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.BeneficiaryActivityTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.AccountCountryName);
            CustomMappedPMProperties.Add(PMPropertyNames.AccountBranchName);
            CustomMappedPMProperties.Add(PMPropertyNames.AccountCurrencyTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.CustomerName);
            CustomMappedPMProperties.Add(PMPropertyNames.ImporterName);
            CustomMappedPMProperties.Add(PMPropertyNames.TapagTypeName);
            CustomMappedPMProperties.Add(PMPropertyNames.ReferantName);

            if (entityPOCO.ImporterClaimTypeCode != null)
            {
                ImporterTypeForClaimQueryService importerTypeForClaimQueryService = new ImporterTypeForClaimQueryService(entityPOCO.Tenant);
                ImporterTypeForClaimPM importerTypeForClaimPM = importerTypeForClaimQueryService.GetSingle(entityPOCO.ImporterClaimTypeCode, false, true);
                entityPM.ImporterClaimTypeName = importerTypeForClaimPM.LocalName;
            }

            if (entityPOCO.PassportCountryTypeCode != null)
            {
                CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM customsCountryPM = customsCountryQueryService.GetSingle(entityPOCO.PassportCountryTypeCode, false, true);
                entityPM.PassportCountryTypeName = customsCountryPM.LocalName;
            }

            if (entityPOCO.PassportTypeCode != null)
            {
                PassportTypeQueryService passportTypeQueryService = new PassportTypeQueryService(entityPOCO.Tenant);
                PassportTypePM passportTypePM = passportTypeQueryService.GetSingle(entityPOCO.PassportTypeCode, false, true);
                entityPM.PassportTypeName = passportTypePM.LocalName;
            }

            if (entityPOCO.ClaimSubmiterTypeCode != null)
            {
                CustomerActivityTypeQueryService customerActivityTypeQueryService = new CustomerActivityTypeQueryService(entityPOCO.Tenant);
                CustomerActivityTypePM customerActivityTypePM = customerActivityTypeQueryService.GetSingle(entityPOCO.ClaimSubmiterTypeCode, false, true);
                entityPM.ClaimSubmiterTypeName = customerActivityTypePM.LocalName;
            }

            if (entityPOCO.BeneficiaryActivityTypeCode != null)
            {
                CustomerActivityTypeQueryService customerActivityTypeQueryService = new CustomerActivityTypeQueryService(entityPOCO.Tenant);
                CustomerActivityTypePM customerActivityTypePM = customerActivityTypeQueryService.GetSingle(entityPOCO.BeneficiaryActivityTypeCode, false, true);
                entityPM.BeneficiaryActivityTypeName = customerActivityTypePM.LocalName;
            }

            if (entityPOCO.AccountCountryCode != null)
            {
                CustomsCountryQueryService customsCountryQueryService = new CustomsCountryQueryService(entityPOCO.Tenant);
                CustomsCountryPM customsCountryPM = customsCountryQueryService.GetSingle(entityPOCO.AccountCountryCode, false, true);
                entityPM.AccountCountryName = customsCountryPM.LocalName;
            }

            if (entityPOCO.AccountBranchCode != null)
            {
                CustomsBranchQueryService customsBranchQueryService = new CustomsBranchQueryService(entityPOCO.Tenant);
                CustomsBranchPM customsBranchPM = customsBranchQueryService.GetSingle(entityPOCO.AccountBranchCode, false, true);
                entityPM.AccountBranchName = customsBranchPM.LocalName;
            }

            if (entityPOCO.AccountCurrencyTypeCode != null)
            {
                CurrencyTypeQueryService currencyTypeQueryService = new CurrencyTypeQueryService(entityPOCO.Tenant);
                CurrencyTypePM currencyTypePM = currencyTypeQueryService.GetSingle(entityPOCO.AccountCurrencyTypeCode, false, true);
                entityPM.AccountCurrencyTypeName = currencyTypePM.LocalName;
            }

            TapagQueryService tapagQueryService = new TapagQueryService(entityPOCO.Tenant);
            TapagPM tapag = tapagQueryService.GetSingle(entityPOCO.Id, false, false);
            if (tapag != null)
            {
                entityPM.TapagNumber = tapag.TapagNumber;
                entityPM.CustomerId = tapag.CustomerId;
                entityPM.CustomerName = tapag.CustomerName;
                entityPM.ImporterId = tapag.ImporterId;
                entityPM.ImporterName = tapag.ImporterName;
                entityPM.TapagTypeCode = tapag.TapagTypeCode;
                entityPM.TapagTypeName = tapag.TapagTypeName;
                entityPM.LeadingFileNumber = tapag.LeadingFileNumber;
                entityPM.CreateDate = tapag.CreateDate;
                entityPM.FollowDate = tapag.FollowDate;
                entityPM.ValidityDate = tapag.ValidityDate;
                entityPM.IsClosed = tapag.IsClosed;
                entityPM.CustomsBranchCode = tapag.CustomsBranchCode;
                entityPM.ReferantId = tapag.ReferantId;
                entityPM.ReferantName = tapag.ReferantName;
            }
        }
   }


}
   