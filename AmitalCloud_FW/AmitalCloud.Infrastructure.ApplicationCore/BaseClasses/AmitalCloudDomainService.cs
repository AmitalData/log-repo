using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Transactions;
using System.Web;

namespace AmitalCloud.Infrastructure.Application.BaseClasses
{
    public abstract class AmitalCloudDomainService : DomainService
    {
        public override bool Submit(ChangeSet changeSet)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                bool result = true;
                try
                {
                    result = base.Submit(changeSet);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    string Error = "";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Error += "Entity of type " + eve.Entry.Entity.GetType().Name + " in state " + eve.Entry.State + " has the following validation errors:";
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                        }
                    }
                    string authenticateduser = "";
                    try
                    {
                        authenticateduser = AmitalCloudSecurityUtility.GetAuthenticatedUser();
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
                return result;
            }
        }
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
                authenticateduser = AmitalCloudSecurityUtility.GetAuthenticatedUser();
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
            throw errorInfo.Error;
        }
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