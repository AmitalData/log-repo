using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.Tools.Validating
{
    public class TasksSchedulerValidator
    {
        public static void Validate(TasksSchedulerPM taskPM, TasksScheduler taskPoco)
        {
            ////if (taskPoco.Status == "In progress") WR can edit
            ////    throw new Exception("The task is in progress. You are not allowed to edit it");

            if (taskPM.StartDateTime != taskPoco.StartDateTime)
            {
                var utcNowDate = DateTime.UtcNow;
                if (taskPM.NextRunTimeUTC < utcNowDate)
                    throw new Exception("You can't select a past date");
            }

            if (taskPM.RepeatInMinutes != null && taskPM.RepeatInMinutes < 5)
                throw new Exception("The lowest value you can add in Repeat in Minutes field is 5");

            string requiredFieldMessage = GetRequiredFieldMessageTranslation(taskPM);
            if (taskPM.StartDateTime == null)
            {
                throw new ApplicationException(requiredFieldMessage.Replace("%FieldName", TranslateTextsClass.Translate("TasksScheduler.F.StartDateTime", taskPM.Tenant)));
            }
        }

        private static string GetRequiredFieldMessageTranslation(TasksSchedulerPM taskPM)
        {
            bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(taskPM.Tenant);
            string requiredFieldMessage = TranslateTextsClass.Translate("General.M.FieldIsRequired", taskPM.Tenant, showLocal);
            return requiredFieldMessage;
        }
    }
}
