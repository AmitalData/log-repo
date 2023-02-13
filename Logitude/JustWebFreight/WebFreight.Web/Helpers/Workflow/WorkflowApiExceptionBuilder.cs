using Logitude.Server.Tools;
using Logitude.SystemLogs;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace WebFreight.Web.Helpers.Workflow
{
    public class WorkflowApiExceptionBuilder
    {
        public static APIException BuildException(Exception exception, int tenant)
        {
            APIException apiException = new APIException();
            if (exception is SqlException sqlException)
            {
                HandleSqlException(sqlException, exception, apiException);
            }
            else if (exception.InnerException is SqlException sqlInnerException)
            {
                HandleSqlException(sqlInnerException, exception, apiException);
            }
            else if (exception is DbUpdateException dbUpdateException &&
                     dbUpdateException?.InnerException?.InnerException is SqlException dbUpdateSqlException)
            {
                HandleSqlException(dbUpdateSqlException, exception, apiException);
            }
            else if (exception is InvalidOperationException invalidOperationException &&
                     IsTimeoutMaxPoolSizeWasReachedException(invalidOperationException))
            {
                apiException.ErrorType = ErrorTypes.TimeoutMaxPoolSizeWasReachedException;
                apiException.ErrorMessage = exception.Message;
                apiException.ShortErrorMessage = ShortErrorMessages.TimeoutMaxPoolSizeWasReachedException;
            }
            else
            {
                apiException.ErrorType = ErrorTypes.ApplicationException;
                apiException.ErrorMessage = exception.Message;
                apiException.ShortErrorMessage = exception.Message;
            }

            AzureLog.SaveLogsInStorage(apiException.ErrorMessage, "E", DateTime.Now, exception.Message, exception.StackTrace, tenant, null, "Workflow", null, exception);

            return apiException;
        }

        private static void HandleSqlException(SqlException sqlException, Exception exception, APIException apiException)
        {
            if (IsConnectionException(sqlException))
            {
                apiException.ErrorType = ErrorTypes.ConnectionException;
                apiException.ErrorMessage = sqlException.Message;
                apiException.ShortErrorMessage = ShortErrorMessages.ConnectionException;
            }
            else if (IsTimeoutException(sqlException))
            {
                apiException.ErrorType = ErrorTypes.TimeoutException;
                apiException.ErrorMessage = sqlException.Message;
                apiException.ShortErrorMessage = ShortErrorMessages.TimeoutException;
            }
            else if (IsSnapshotException(sqlException))
            {
                apiException.ErrorType = ErrorTypes.SnapshotException;
                apiException.ErrorMessage = sqlException.Message;
                apiException.ShortErrorMessage = ShortErrorMessages.SnapshotException;
            }
            else
            {
                apiException.ErrorType = ErrorTypes.ApplicationException;
                apiException.ErrorMessage = exception.Message;
                apiException.ShortErrorMessage = exception.Message;
            }
        }

        private static bool IsTimeoutException(SqlException sqlException)
        {
            return sqlException.Number == SqlExceptionNumbers.TimeoutExpired;
        }

        private static bool IsSnapshotException(SqlException sqlException)
        {
            return sqlException.Number == SqlExceptionNumbers.SnapshotIsolationTransactionAbortedDueToUpdateConflict;
        }

        private static bool IsConnectionException(SqlException sqlException)
        {
            return sqlException.Number == SqlExceptionNumbers.AnErrorWhileEstablishingAConnectionToTheServer;
        }

        private static bool IsTimeoutMaxPoolSizeWasReachedException(InvalidOperationException invalidOperationException)
        {
            return invalidOperationException.Message.ToString().ToLower().Contains("this may have occurred because all pooled connections were in use and max pool size was reached");
        }
    }
}