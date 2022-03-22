using Logitude.CRMTests.Models;
using Logitude.CRMTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;

namespace Logitude.CRMTests.Services
{
    public class ActivityTaskDataPreparation
    {
        public void Prepar()
        {
            try
            {
                ApiResponse<ActivityPM> response = APICaller.CallPost<ActivityPM>(GetValidActivityTaskPM(), Urls.ActivitiesController, UserTenant.Token);
                ActivityTaskDataMap(response.Data);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating activity task Before Feature Run :" + e.InnerException);
            }
        }

        private ActivityPM GetValidActivityTaskPM()
        {
            return new ActivityBuilder()
                .WithDefualtValues()
                .Subject("pre specflow sub")
                .Description("pre specflow desc")
                .StartDateTime(null)
                .DueDate(null)
                .PriorityCode("Normal")
                .ActivityStatusCode("N")
                .ActivityTypeCode("TS")
                .Build();
        }

        private void ActivityTaskDataMap(ActivityPM activity)
        {
            CRMData.ActivityTaskId = activity.Id;
        }

    }
}
