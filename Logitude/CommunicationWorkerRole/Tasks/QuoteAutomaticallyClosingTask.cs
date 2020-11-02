using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebFreight.Web.QuoteModel;

namespace CommunicationWorkerRole.Tasks
{
    public class QuoteAutomaticallyClosingTask : TaskManagerBase
    {
        public QuoteAutomaticallyClosingTask(string Id, int tenant)
            : base(Id, tenant)
        {
        }

        public override void StartTask()
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction(new TimeSpan(3, 0, 0)))
                {
                    QuoteModelProcedureClass.ExecuteDailyAutomaticallyClosing();
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                string errorMessage = new StringBuilder().Append("Exception Message: ").AppendLine().Append(ex.Message).AppendLine().ToString();
                errorMessage += new StringBuilder().Append("Stack Trace:").AppendLine().Append(ex.StackTrace).AppendLine().ToString();
                throw new Exception(errorMessage);
            }
        }

        private void RunTask()
        {
           

            //ICommonDataContext iContext = CommonDataContext.GetContext(0);
            //List<int> AllTenants = (from d in iContext.Tenants select d.Id).ToList();
            //if (AllTenants != null)
            //{
            //    foreach (int iTenant in AllTenants)
            //    {
            //        try
            //        {
            //            using (TransactionScope scope = TransactionFactory.GetTransaction(new TimeSpan(3, 0, 0)))
            //            {
            //                QuoteModelProcedureClass.ExecuteSingleQuoteAutomaticallyClosing(iTenant);
            //                scope.Complete();
            //            }
            //        }

            //        catch (Exception ex)
            //        {
            //            ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Quote automatically closing worker role, Tenant: " + iTenant, null, null);
            //        }
            //    }
            //}
        }
    }
}
