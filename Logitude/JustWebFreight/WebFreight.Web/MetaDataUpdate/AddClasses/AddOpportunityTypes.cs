using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.Repsitories;
using Logitude.Server.Tools.Counters;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddOpportunityTypes
    {
        public static void AddOpportunityType(OpportunityTypeDetails OpportunityTypeDetails, OpportunityTypeRepository OpportunityTypeRepository, Dictionary<string, OpportunityType> tenantOpportunityTypes)
        {
            if (tenantOpportunityTypes.Keys.Contains(OpportunityTypeDetails.Code))
            {
                //OpportunityType reason = tenantOpportunityTypes[OpportunityTypeDetails.Code];

                //reason.Tenant = OpportunityTypeDetails.Tenant;
                //reason.Name = OpportunityTypeDetails.Name;
                //reason.Code = OpportunityTypeDetails.Code;
                //reason.SearchFields = OpportunityTypeDetails.Name + "," + OpportunityTypeDetails.Code;
                //OpportunityTypeRepository.Update(reason);
            }
            else
            {
                OpportunityType newReason = new OpportunityType()
                {
                    Tenant = OpportunityTypeDetails.Tenant,
                    Name = OpportunityTypeDetails.Name,
                    Code = OpportunityTypeDetails.Code,
                    SearchFields = OpportunityTypeDetails.Name + "," + OpportunityTypeDetails.Code,
                    Id = IdCounter.GetNumber("OpportunityType", OpportunityTypeDetails.Tenant).ToString(),

                };
                OpportunityTypeRepository.Add(newReason);
            }
        }
    }
}