using System.Collections.Generic;
using System.Linq;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddLeadSources
    {
        public static void AddLeadSource(LeadSourceDetails LeadSourceDetails, LeadSourceRepository LeadSourceRepository, Dictionary<string, LeadSource> tenantLeadSources)
        {
            if (tenantLeadSources.Keys.Contains(LeadSourceDetails.Code))
            {
                LeadSource LeadSource = tenantLeadSources[LeadSourceDetails.Code];

                LeadSource.Tenant = LeadSourceDetails.Tenant;
                LeadSource.Name = LeadSourceDetails.Name;
                LeadSource.Code = LeadSourceDetails.Code;
                LeadSource.SearchFields = LeadSourceDetails.Code + "," + LeadSourceDetails.Name;
                LeadSourceRepository.Update(LeadSource);
            }
            else
            {
                LeadSource newLeadSource = new LeadSource()
                {
                    Tenant = LeadSourceDetails.Tenant,
                    Name = LeadSourceDetails.Name,
                    Code = LeadSourceDetails.Code,
                    SearchFields = LeadSourceDetails.Code + "," + LeadSourceDetails.Name,
                    Id = IdCounter.GetNumber("LeadSource", LeadSourceDetails.Tenant).ToString(),

                };
                LeadSourceRepository.Add(newLeadSource);
            }
        }
    }
}