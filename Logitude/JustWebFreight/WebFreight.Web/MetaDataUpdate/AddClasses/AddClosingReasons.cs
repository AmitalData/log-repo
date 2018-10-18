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
    public class AddClosingReasons
    {
        public static void AddClosingReason(ClosingReasonDetails ClosingReasonDetails, OpportunityClosingReasonRepository ClosingReasonRepository, Dictionary<string, OpportunityClosingReason> tenantClosingReasons)
        {
            if (tenantClosingReasons.Keys.Contains(ClosingReasonDetails.Code))
            {
                //OpportunityClosingReason reason = tenantClosingReasons[ClosingReasonDetails.Code];

                //reason.Tenant = ClosingReasonDetails.Tenant;
                //reason.Name = ClosingReasonDetails.Name;
                //reason.LocalName = ClosingReasonDetails.LocalName;
                //reason.AddedManually = false;
                //reason.Code = ClosingReasonDetails.Code;
                //reason.IsClosedLost = ClosingReasonDetails.IsClosedLost;
                //reason.SearchFields = ClosingReasonDetails.Name + "," + ClosingReasonDetails.LocalName;
                //ClosingReasonRepository.Update(reason);
            }
            else
            {
                OpportunityClosingReason newReason = new OpportunityClosingReason()
                {
                    Tenant = ClosingReasonDetails.Tenant,
                    Name = ClosingReasonDetails.Name,
                    LocalName = ClosingReasonDetails.LocalName,
                    AddedManually = false,
                    Code = ClosingReasonDetails.Code,
                    IsClosedLost = ClosingReasonDetails.IsClosedLost,
                    SearchFields = ClosingReasonDetails.Name + "," + ClosingReasonDetails.LocalName,
                    Id = IdCounter.GetNumber("OpportunityClosingReason", ClosingReasonDetails.Tenant).ToString(),

                };
                ClosingReasonRepository.Add(newReason);
            }
        }
    }
}