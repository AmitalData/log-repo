
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
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{

    public partial class CustomBankDataMapping : IMapping<CustomBankPM, CustomBank>
    {

        public void CustomPMToPOCO(CustomBankPM entityPM, CustomBank entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {

                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;


            } 

            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        private void BuildSearchFields(CustomBankPM entityPM, CustomBank entityPOCO, bool isInsert)
        {

            string result = "";
            result = entityPM.AccountNumber + ',' + entityPM.BankCode + ',' + entityPM.BranchCode + ',' + entityPM.InternalCode + ',' + entityPM.LocalName + ',' + entityPM.EnglishName + ',' + entityPM.BankAddress;
            entityPM.SearchFields = result.ToLower();
            entityPOCO.SearchFields = entityPM.SearchFields;
        }


        public void CustomPOCOToPM(CustomBankPM entityPM, CustomBank entityPOCO)
        {
            this.CustomMappedPMProperties.Add(PMPropertyNames.BankName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.BranchName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.PayerTypeName);


            if (entityPOCO.BankCode != null)
            {
                BankQueryService bankQueryService = new BankQueryService(entityPOCO.Tenant);
                BankPM bank = bankQueryService.GetSingle(entityPOCO.BankCode, false, true);
                entityPM.BankName = bank != null?  bank.LocalName: null;
            }

            if (entityPOCO.CustomsBranchId != null)
            {
                CustomsBranchQueryService customsBranchQueryService = new CustomsBranchQueryService(entityPOCO.Tenant);
                CustomsBranchPM branch = customsBranchQueryService.GetSingle(entityPOCO.CustomsBranchId, false, true);
                
                entityPM.BranchName = branch != null?  branch.LocalName: null;
            }

            if (entityPOCO.PayerTypeCode != null)
            {
                CustomerActivityTypeQueryService customerActivityTypeQueryService = new CustomerActivityTypeQueryService(entityPOCO.Tenant);
                CustomerActivityTypePM activityType = customerActivityTypeQueryService.GetSingle(entityPOCO.PayerTypeCode, false, true);
                entityPM.PayerTypeName = activityType!= null? activityType.LocalName: null;
            }

            entityPM.Name = entityPOCO.LocalName;
            if(string.IsNullOrWhiteSpace(entityPOCO.LocalName))
            {
                entityPM.Name = entityPOCO.EnglishName;
            }
        }
    }


}
   