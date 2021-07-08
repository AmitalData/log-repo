using Logitude.CRMTests.Models;
using Logitude.CRMTests.Models.Builders;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.CRMTests.Services
{
    public class ActivityAppointmentServices
    {
        public ActivityPM CreateInstance(Table taskTable)
        {
            dynamic dataTable = taskTable.CreateDynamicInstance();
            return new ActivityBuilder()
                .WithDefualtValues()
                .Subject((string)dataTable.Subject)
                .Location((string)dataTable.Location)
                .Description((string)dataTable.Description)
                .ActivityTimeTypeCode((string)dataTable.ActivityTimeType)
                .Duration((int)dataTable.Duration)
                .StartDateTime(Convert.ToString(dataTable.StartDateTime).Length == 0 ? null : (DateTime?)dataTable.StartDateTime)
                .EndDateTime(Convert.ToString(dataTable.EndDateTime).Length == 0 ? null : (DateTime?)dataTable.EndDateTime)
                .PriorityCode((string)dataTable.Priority)
                .ActivityStatusCode("N")
                .ActivityTypeCode("AP")
                .Build();
        }

    }
}
