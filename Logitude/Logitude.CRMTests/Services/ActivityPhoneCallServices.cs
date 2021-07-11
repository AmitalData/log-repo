using Logitude.CRMTests.Models;
using Logitude.CRMTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
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
                .CallWithId(GetContactId())
                .Build();
        }
        private string GetContactId()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues().Build();
            ApiResponse<IEnumerable<ContactPM>> response = APICaller.CallGetByFilters<IEnumerable<ContactPM>>(Urls.ContactViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault()?.Id;
        }
    }
}
