using Logitude.CRMTests.Models;
using Logitude.CRMTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;


namespace Logitude.CRMTests.Services
{
    public class OpportunityServices
    {
        public OpportunityPM CreateInstance(Table appointmentTable)
        {
            dynamic dataTable = appointmentTable.CreateDynamicInstance();
            return new OpportunityBuilder()
                .WithDefualtValues()
                .Subject((string)dataTable.Subject)
                .NumberOfShipments((int)dataTable.ShipmentsCount)
                .OpportunityTypeId(GetOpportunityTypeIdByName((string)dataTable.OpportunityType))
                .StageId(GetStageIdByName((string)dataTable.Stage))
                .RatingCode((string)dataTable.Rating)
                .Build();
        }

        public string GetOpportunityTypeIdByName(string opportunityTypeName)
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("Name")
                .Filter1Operator("equals")
                .Filter1Value(opportunityTypeName).Build();

            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(Urls.OpportunityTypeViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?["Id"];
        }

        public string GetStageIdByName(string stageName)
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("Name")
                .Filter1Operator("equals")
                .Filter1Value(stageName).Build();

            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(Urls.StageViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?["Id"];
        }

        public OpportunityPM UpdateInstance(Table appointmentTable, OpportunityPM opportunity)
        {
            dynamic dataTable = appointmentTable.CreateDynamicInstance();
            return new OpportunityBuilder()
                .WithModel(opportunity)
                .Subject((string)dataTable.Subject)
                .OpportunityTypeId(GetOpportunityTypeIdByName((string)dataTable.OpportunityType))
                .NumberOfShipments((int)dataTable.ShipmentsCount)
                .StageId(GetStageIdByName((string)dataTable.Stage))
                .RatingCode((string)dataTable.Rating)
                .Build();
        }

    }
}
