using System;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure.Azure;
using System.Diagnostics;
using System.Data.SqlClient;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.SystemLogs
{
    public class ExceptionHandler
    {
        public static void HandleException(Exception exception, DateTime clientDate, int tenant, string userId, string userName, string ExtraMessage, string ip)
        {
            if (!DbContextBaseUtil.MaxPoolSizeWasReachedWhileSave.HasValue && exception != null && exception.ToString().Contains("max pool size was reached"))//  The timeout period elapsed prior to obtaining a connection from the pool.  This may have occurred because all pooled connections were in use and max pool size was reached.
            {
                DbContextBaseUtil.MaxPoolSizeWasReachedWhileSave = DateTime.Now;
            }
            exception = exception ?? new Exception(ExtraMessage??"");
            if (exception.ToString().Contains("max pool size was reached"))
            {
                if (InjectionUtil.Instance.IISManager != null)
                {
                    InjectionUtil.Instance.IISManager.RecycleMe();
                }

            }
            Debug.WriteLine(exception.ToString());
            AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Error);
            string ErrorMessage = "";

            if (!string.IsNullOrEmpty(ip))
            {
                if (ip.StartsWith("150.70"))
                {
                    return;
                }
            }

            if (exception != null)
            {
                if (exception.Message.Contains("Sorry you can't update this record right now. It's being updated by another user")
                    || exception.Message.Contains("WebFreight.Web.Security.AutenticationException")
                    || exception.Message.Contains("Sorry! this user is not the last signed user!")
                    || exception.Message.Contains("Can't create payment with future date")
                    || exception.Message.Contains("is required")
                    || exception.Message.Contains("already exists")
                    || exception.Message.Contains("Master field already used in another Shipment")
                    || exception.Message.Contains("You should have at least 1 invoice line")
                    || exception.Message.Contains("Some of invoice lines Vat Type Percentage is empty")
                    || exception.Message.Contains("Invoice line amount field must not be zero")
                    || exception.Message.Contains("You should have at least 1 invAPInvoice.M.VatTypePercentageEmptyoice line")
                    || exception.Message.Contains("Sorry! you have no permission to do this operation")
                      //|| exception.Message.Contains("לא ניתן לעדכן את הרשומה מכיוון שהיא נעולה ע")
                      )

                {
                    return;
                }

                //if (exception.Message.Contains("Physical connection is not usable"))
                //{
                //    SqlConnection.ClearAllPools();
                //}

                //if (exception.Message.Contains("An exception has been raised that is likely due to a transient failure. If you are connecting to a SQL Azure database consider using SqlAzureExecutionStrategy.")
                //    || exception.Message.Contains("The underlying provider failed on Open"))
                //{
                //    WebFreightEntryPoint.CheckConnectionStrategy = true;
                //    WebFreightEntryPoint.CheckConnectionStartDate = DateTime.UtcNow;

                //}

                if (!string.IsNullOrEmpty(ExtraMessage))
                {
                    ErrorMessage = ExtraMessage + Environment.NewLine;
                }

                ErrorMessage += exception.Message;

                if (exception.InnerException != null)
                {
                    ErrorMessage += Environment.NewLine + exception.InnerException.Message;

                    if (exception.InnerException.InnerException != null)
                    {
                        ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.Message;

                        if (exception.InnerException.InnerException.InnerException != null)
                        {
                            ErrorMessage += Environment.NewLine + exception.InnerException.InnerException.InnerException.Message;
                        }
                    }

                    //if (exception.InnerException.Message.Contains("Physical connection is not usable"))
                    //{
                    //    SqlConnection.ClearAllPools();
                    //}
                }

                if (clientDate == null)
                    clientDate = DateTime.Now;

                Debug.WriteLine("***HandleException** " + ErrorMessage);//May cause slowness ,But worth - If u Decides to delete ,Please inform itzik !!!!!
                AzureLog.SaveLogsInStorage(ErrorMessage, "E", clientDate, exception.Message, exception.StackTrace, tenant, userId, userName, ip,exception);

            }
        }

        public static void HandleDbException(Exception exception, string TypeOrUser, string ExtraMessage)
        {
            HandleException(exception, DateTime.Now, 0, TypeOrUser, "", ExtraMessage, "");
        }
    }
}