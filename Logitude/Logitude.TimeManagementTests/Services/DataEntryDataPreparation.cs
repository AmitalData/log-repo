using System;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using Logitude.TimeManagementTests.Models;
using Logitude.TimeManagementTests.Models.Builders;

namespace Logitude.TimeManagementTests.Services
{
    public class DataEntryDataPreparation
    {
        public void Prepar()
        {
            try
            {
                ApiResponse<TimeManagementAPIHelper> response = APICaller.CallPut<TimeManagementAPIHelper>(GetValidDataEntryPM(), Urls.TimeManagementDomainController, UserTenant.Token);
                GetValidDataEntry(response.Data);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating Data Entry Before Feature Run :" + e.InnerException);
            }
        }

        private TimeManagementAPIHelper GetValidDataEntryPM()
        {
            return new TimeManagementAPIHelperBuilder()
                .WithDefualtValues()
                .LocationCode("Office")
                .ItemsPM(GetTMEmployeeTime())
                .Build();
        }

        private TMEmployeeTimePM GetTMEmployeeTime()
        {
            return new TMEmployeeTimePMBuilder().WithDefualtValues()
                .LocationCode("Office")
                .Description("pre specflow desc")
                .TimeInMinutes(45)
                .DateOfWork(DateTime.Now)
                .Build();
        }

        private void GetValidDataEntry(TimeManagementAPIHelper dataEntry)
        {
            TimeManagementData.DataEntry = dataEntry;
        }
    }
}
