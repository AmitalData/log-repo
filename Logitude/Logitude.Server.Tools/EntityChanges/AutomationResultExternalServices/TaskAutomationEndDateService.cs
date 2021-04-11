using Logitude.Server.Tools.EntityChanges.AutomationResult;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.EntityChanges.AutomationResultExternalServices
{
   public class TaskAutomationEndDateService: GeneralAutomationResultService
    {

        private int tenant;
        public TaskAutomationEndDateService( int tenant)
        {
            this.tenant = tenant;

        }

        public DateTime? GetDate(string endDateValue  , string endDateTypeValue, object entityPM)
        {
            if (string.IsNullOrEmpty(endDateValue)) return null;

            if (endDateTypeValue == "Field")
            {
                var result = GetPropertyValueFromObject(endDateValue, entityPM);
                return !string.IsNullOrEmpty(result) ? (DateTime?)DateTime.Parse(result) : null;
            }

            if (endDateTypeValue == "CalcateDate")
            {
                return TenantServerConfigration.GetCurrentDateTime(tenant).AddDays(GetNumberOfDays(endDateValue));
            }

            return DateTime.Parse(endDateValue);
        }

        private static int GetNumberOfDays(string endDateValue)
        {
            int numberOfDays = 0;
            var endDatePartValues = endDateValue.Split('*');
            if (endDatePartValues.Length == 3)
            {
                string partDateOperator = endDatePartValues[1];
                numberOfDays = Int32.Parse(endDatePartValues[2]);
                if (partDateOperator == "-") numberOfDays = (numberOfDays * -1);
            }

            return numberOfDays;
        }

    }
}
