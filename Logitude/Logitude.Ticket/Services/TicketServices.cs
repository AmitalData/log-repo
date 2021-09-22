using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using Logitude.TicketTests.Models;
using Logitude.TicketTests.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.TicketTests.Services
{
    public class TicketServices
    {
        public TicketPM CreateInstance(Table ticketTable)
        {
            dynamic dataTable = ticketTable.CreateDynamicInstance();
            TicketClassificationPM ticketClassification = GetGeneralTicketClassification();
            return new TicketBuilder()
                .WithDefualtValues()
                .EntityType(new ObjectTableService().GetIdByName((string)dataTable.EntityType))
                .Subject((string)dataTable.Subject)
                .TicketDescription((string)dataTable.Description)
                .MainClassificationId(ticketClassification.Id)
                .SeverityId(ticketClassification.DefaultSeverityId)
                .EmployeeGroupId(ticketClassification.EmployeeGroupId)
                .StageId(GetStageIdByName((string)dataTable.Stage))
                .Build();
        }

        public TicketPM UpdateInstance(Table ticketTable, TicketPM ticket)
        {
            dynamic dataTable = ticketTable.CreateDynamicInstance();
            return new TicketBuilder()
                .WithModel(ticket)
                .EntityType(new ObjectTableService().GetIdByName((string)dataTable.EntityType))
                .Subject((string)dataTable.Subject)
                .TicketDescription((string)dataTable.Description)
                .Build();
        }

        public TicketClassificationPM GetGeneralTicketClassification()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
              .Filter1Name("ParentId")
              .Filter1Operator("equals")
              .Filter1Value(null)
              .Filter1Name("Inactive")
              .Filter1Operator("equals")
              .Filter1Value("false").Build();

            ApiResponse<IEnumerable<TicketClassificationPM>> response = APICaller.CallGetByFilters<IEnumerable<TicketClassificationPM>>(Urls.TicketclassificationViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.First();
        }

        public string GetStageIdByName(string stageName)
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("Name")
                .Filter1Operator("equals")
                .Filter1Value(stageName).Build();

            ApiResponse<IEnumerable<dynamic>> response = APICaller.CallGetByFilters<IEnumerable<dynamic>>(Urls.TicketStageViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?["Id"];
        }

    }
}
