using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;
using Logitude.BL.Security;
using Logitude.SystemLogs;
using Simplog.Server.Infrastructure.Helpers;


namespace Logitude.BL.Helpers
{
    public class AmitalCloudDomainService : DomainService
    {
        public override bool Submit(ChangeSet changeSet)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())//new TransactionScope(TransactionScopeOption.RequiresNew /*,new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot }*/))
            {
                bool f = true;


                try
                {
                    f = base.Submit(changeSet);
                }

                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    string Error = "";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                        foreach (var ve in eve.ValidationErrors)
                        {
                            //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            //ve.PropertyName, ve.ErrorMessage);

                            Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                        }
                    }


                    string authenticateduser = "";

                    try
                    {
                        authenticateduser = SecurityUtility.GetAuthenticatedUser();
                    }

                    catch
                    {
                        authenticateduser = "UnKnown";
                    }
                    string ip = "";
                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        ip = currentIP;
                    }
                    ExceptionHandler.HandleException(new Exception(Error), DateTime.Now, 0, "", authenticateduser, "", ip);
                    throw new Exception(Error);
                }


                scope.Complete();

                return f;
            }
        }

        //        private static TransactionScope GetNewTransaction()
        //        {
        //#if ORACLE_DB
        //            return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { /*IsolationLevel = IsolationLevel.Snapshot*/ });
        //#endif
        //            return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot });
        //        }

        protected override void OnError(DomainServiceErrorInfo errorInfo)
        {
            if (this.ChangeSet != null)
            {
                this.ChangeSet.ChangeSetEntries.Where(f => f.HasError == true);
            }

            base.OnError(errorInfo);

            string authenticateduser = "";
            try
            {
                authenticateduser = Security.SecurityUtility.GetAuthenticatedUser();
            }
            catch
            {
                authenticateduser = "UnKnown";
            }

            string ip = "";
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                if (string.IsNullOrEmpty(currentIP))
                {
                    currentIP = HttpContext.Current.Request.UserHostAddress;
                }
                ip = currentIP;
            }
            ExceptionHandler.HandleException(errorInfo.Error, DateTime.Now, 0, "", authenticateduser, "OnError()", ip);
            //string ErrorMessage;
            //ErrorMessage = errorInfo.Error.Message;

            //if (errorInfo.Error.InnerException != null)
            //{
            //    ErrorMessage += Environment.NewLine + errorInfo.Error.InnerException.Message;
            //}

            //ErrorMessage += Environment.NewLine + errorInfo.ToString();
            //if (!string.IsNullOrEmpty(errorInfo.Error.StackTrace))
            //{
            //    ErrorMessage += Environment.NewLine + errorInfo.Error.StackTrace;
            //}

            //AzureLog.SaveLogsInStorage(ErrorMessage, "E", DateTime.Now, errorInfo.Error.Message, errorInfo.Error.StackTrace, 0, ServiceContext.User.Identity.Name, ServiceContext.User.Identity.Name);
            throw errorInfo.Error;
        }



        //public static bool OnSubmitChanges(DomainService baseDomain, ChangeSet changeSet)
        //{
        //    using (TransactionScope scope = TransactionFactory.GetTransaction())
        //    {
        //        bool f = true;

        //        try
        //        {
        //            f = baseDomain.Submit(changeSet);
        //        }

        //        catch (Exception ex)
        //        {
        //            throw new DomainException("Submit failed", ex);
        //        }

        //        scope.Complete();

        //        return f;
        //    }
        //}

        //public static void OnError(DomainServiceErrorInfo errorInfo, DomainServiceContext serviceContext)
        //{
        //    string ErrorMessage;
        //    ErrorMessage = errorInfo.Error.Message;

        //    if (errorInfo.Error.InnerException != null)
        //    {
        //        ErrorMessage += Environment.NewLine + errorInfo.Error.InnerException.Message;
        //    }

        //    ErrorMessage += Environment.NewLine + errorInfo.ToString();
        //    if (!string.IsNullOrEmpty(errorInfo.Error.StackTrace))
        //    {
        //        ErrorMessage += Environment.NewLine + errorInfo.Error.StackTrace;
        //    }

        //    AzureLog.SaveLogsInStorage(ErrorMessage, "E", 0, serviceContext.User.Identity.Name, serviceContext.User.Identity.Name);
        //    throw errorInfo.Error;
        //}
        public override System.Collections.IEnumerable Query(QueryDescription queryDescription, out IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> validationErrors, out int totalCount)
        {
            return base.Query(queryDescription, out validationErrors, out totalCount);
        }

        public override object Invoke(InvokeDescription invokeDescription, out IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> validationErrors)
        {
            return base.Invoke(invokeDescription, out validationErrors);
        }

    }
}