using Logitude.CRMTests.Models;
using Logitude.CRMTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;

namespace Logitude.CRMTests.Services
{
    public class ActivityAppointmentDataPreparation
    {
        public void Prepar()
        {
            try
            {
                ApiResponse<ActivityPM> response = APICaller.CallPost<ActivityPM>(GetValidActivityAppointmentPM(), Urls.ActivitiesController, UserTenant.Token);
                ActivityAppointmentDataMap(response.Data);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating activity Appointment Before Feature Run :" + e.InnerException);
            }
        }

        private ActivityPM GetValidActivityAppointmentPM()
        {
            return new ActivityBuilder()
                .WithDefualtValues()
                .Subject("pre specflow sub")
                .Location("pre specflow lcoation")
                .Description("pre specflow desc")
                .ActivityTimeTypeCode("FR")
                .Duration(30)
                .StartDateTime(DateTime.Now.AddDays(13))
                .EndDateTime(DateTime.Now.AddDays(14))
                .PriorityCode("Normal")
                .ActivityStatusCode("N")
                .ActivityTypeCode("AP")
                .Build();

        }

        private void ActivityAppointmentDataMap(ActivityPM activity)
        {
            CRMData.ActivityAppointmentId = activity.Id;
        }

    }
}
