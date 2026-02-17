using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddIndustries
    {
        public static void AddIndustry(IndustryDetails industryDetails, IndustryRepository industryRepository, Dictionary<string, Industry> tenantIndustries)
        {
            if (tenantIndustries.Keys.Contains(industryDetails.Code))
            {
                Industry industry = tenantIndustries[industryDetails.Code];

                industry.Tenant = industryDetails.Tenant;
                industry.Name = industryDetails.Name;
                industry.Code = industryDetails.Code;
                industry.SearchFields = industryDetails.Code + "," + industryDetails.Name;
                industryRepository.Update(industry);
            }
            else
            {
                Industry newIndustry = new Industry()
                {
                    Tenant = industryDetails.Tenant,
                    Name = industryDetails.Name,
                    Code = industryDetails.Code,
                    SearchFields = industryDetails.Code + "," + industryDetails.Name,
                    Id = IdCounter.GetNumber("Industry", industryDetails.Tenant).ToString(),

                };
                industryRepository.Add(newIndustry);
            }
        }
    }
}