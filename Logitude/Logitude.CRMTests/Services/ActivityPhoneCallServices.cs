using Logitude.CRMTests.Models;
using Logitude.CRMTests.Models.Builders;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.CRMTests.Services
{
    public class ActivityPhoneCallServices
    {
        public ActivityPM CreateInstance(Table taskTable)
        {
            dynamic dataTable = taskTable.CreateDynamicInstance();
            return new ActivityBuilder()
                .WithDefualtValues()
                .Subject((string)dataTable.Subject)
                .Description((string)dataTable.Description)
                .Duration((int)dataTable.Duration)
                .DueDate(Convert.ToString(dataTable.DueDate).Length == 0 ? null : (DateTime?)dataTable.DueDate)
                .PriorityCode((string)dataTable.Priority)
                .ActivityStatusCode("N")
                .ActivityTypeCode("CL")
                .Build();
        }
    }
}
