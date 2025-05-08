using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Text;
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
                catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
                {
                    var errorMessages = new StringBuilder();

                    var entries = e.Entries;
                    foreach (var entry in entries)
                    {
                        errorMessages.AppendLine($"Entity of type {entry.Entity.GetType().Name} in state {entry.State} caused an error.");
                    }

                    if (e.InnerException != null)
                    {
                        errorMessages.AppendLine("Inner exception: " + e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            errorMessages.AppendLine("Inner-inner exception: " + e.InnerException.InnerException.Message);
                        }
                    }

                    string Error = errorMessages.ToString();

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
                    if (HttpContextHelper.Request != null)
                    {
                        string currentIP = HttpContextHelper.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContextHelper.HttpContext.Connection.RemoteIpAddress?.ToString();
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
            if (HttpContextHelper.Request != null)
            {
                string currentIP = HttpContextHelper.Request.Headers["X-Real-IP"];
                if (string.IsNullOrEmpty(currentIP))
                {
                    currentIP = HttpContextHelper.HttpContext?.Connection?.RemoteIpAddress?.ToString();
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