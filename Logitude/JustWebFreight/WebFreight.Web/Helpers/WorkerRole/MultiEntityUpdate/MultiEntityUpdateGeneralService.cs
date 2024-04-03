using Logitude.BL.InfrastructureModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.WorkerRole.MultiEntityUpdate
{
    public class MultiEntityUpdateGeneralService
    {

        public MultiEntityUpdateGeneralService()
        {


        }


        public bool IsCompletedQueueService(MultiEntityUpdateLogPM multiEntityUpdateLogPM)
        {
            if (multiEntityUpdateLogPM == null) return true;
            if (multiEntityUpdateLogPM.RetryNumber >= 2) return true;
            if (multiEntityUpdateLogPM.StatusCode != "W" && multiEntityUpdateLogPM.StatusCode != "P") return true;
            return false;
        }



        public string GetFullExceptionMessageFromException(Exception exception)
        {
            if (exception == null) return string.Empty;
            string exceptionMessage = exception.Message;
            if (exception.InnerException != null) exceptionMessage = exceptionMessage + Environment.NewLine + exception.InnerException;
            if (exception.StackTrace != null) exceptionMessage = exceptionMessage + Environment.NewLine + "Stack trace: " + exception.StackTrace;
            return exceptionMessage;
        }



        public IEnumerable<List<T>> SplitListIntoNList<T>(List<T> fullList, int nSize)

        {
            for (int i = 0; i < fullList.Count; i += nSize)
            {
                yield return fullList.GetRange(i, Math.Min(nSize, fullList.Count - i)).ToList();
            }
        }

    }
}