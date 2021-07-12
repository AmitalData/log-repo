using Logitude.CRMTests.Models;
using Logitude.CRMTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;

namespace Logitude.CRMTests.Services
{
    public class ActivityPhoneCallDataPreparation
    {
        public void Prepar()
        {
            try
            {
                ApiResponse<ActivityPM> response = APICaller.CallPost<ActivityPM>(GetValidActivityPhoneCall(), Urls.ActivitiesController, UserTenant.Token);
                ActivityPhoneCallDataMap(response.Data);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating activity phoneCall Before Feature Run :" + e.InnerException);
            }
        }

        private ActivityPM GetValidActivityPhoneCall()
        {
            return new ActivityBuilder()
                .WithDefualtValues()
                .Subject("pre specflow sub")
                .Description("pre specflow desc")
                .Duration(30)
                .DueDate(DateTime.Now.AddDays(14))
                .PriorityCode("Normal")
                .ActivityStatusCode("N")
                .ActivityTypeCode("CL")
                .CallWithId(new ActivityPhoneCallServices().GetDefaultContact())
                .Build();
        }

        private void ActivityPhoneCallDataMap(ActivityPM activity)
        {
            CRMData.ActivityPhoneCallId = activity.Id;
        }

    }
}
