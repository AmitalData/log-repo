using Logitude.CRMTests.Models;
using Logitude.CRMTests.Models.Builders;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.CRMTests.Services
{
    public class ActivityTaskServices
    {
        public ActivityPM CreateInstance(Table taskTable)
        {
            dynamic dataTable = taskTable.CreateDynamicInstance();
            return new ActivityBuilder()
                .WithDefualtValues()
                .Subject((string)dataTable.Subject)
                .Description((string)dataTable.Description)
                .StartDateTime(Convert.ToString(dataTable.StartDateTime).Length == 0 ? null : (DateTime?)dataTable.StartDateTime)
                .DueDate(Convert.ToString(dataTable.DueDate).Length == 0 ? null : (DateTime?)dataTable.DueDate)
                .PriorityCode((string)dataTable.Priority)
                .ActivityStatusCode("N")
                .ActivityTypeCode("TS")
                .BusinessUnitId("1311")
                .Build();
        }

    }
}
