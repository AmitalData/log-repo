using Logitude.CRMTests.Models;
using Logitude.CRMTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.CRMTests.Services
{
    public class ActivityPhoneCallServices
    {
        public ActivityPM CreateInstance(Table phoneCallTable)
        {
            dynamic dataTable = phoneCallTable.CreateDynamicInstance();
            return new ActivityBuilder()
                .WithDefualtValues()
                .Subject((string)dataTable.Subject)
                .Description((string)dataTable.Description)
                .Duration((int)dataTable.Duration)
                .DueDate(Convert.ToString(dataTable.DueDate).Length == 0 ? null : (DateTime?)dataTable.DueDate)
                .PriorityCode((string)dataTable.Priority)
                .ActivityStatusCode("N")
                .ActivityTypeCode("CL")
                .CallWithId(UserTenant.UserId)
                .Build();
        }

        public ActivityPM UpdateInstance(Table appointmentTable, ActivityPM activity)
        {
            dynamic dataTable = appointmentTable.CreateDynamicInstance();
            return new ActivityBuilder()
                .WithModel(activity)
                .Subject((string)dataTable.Subject)
                .Description((string)dataTable.Description)
                .Duration((int)dataTable.Duration)
                .DueDate(Convert.ToString(dataTable.DueDate).Length == 0 ? null : (DateTime?)dataTable.DueDate)
                .PriorityCode((string)dataTable.Priority)
                .Build();
        }

    }
}
