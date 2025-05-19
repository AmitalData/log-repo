using AmitalCloud.Infrastructure.Application.Helpers;
using AmitalCloud.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;
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
                catch (DbUpdateException e)
                {
                    HandleExceptionOnUpdate(e);
                    throw new ApplicationException("Database update failed", e);
                }
                scope.Complete();
                return result;
            }
        }

        protected override void OnError(DomainServiceErrorInfo errorInfo)
        {
            List<ChangeSetEntry>? errors = null;
            if (this.ChangeSet != null)
            {
                errors = this.ChangeSet?.ChangeSetEntries?.Where(f => f.HasError)?.ToList();
            }

            var (authenticateduser, ip) = AmitalCloudSecurityUtility.GetAuditInfo();

            ExceptionHandler.HandleException(errorInfo.Error, DateTime.Now, 0, "", authenticateduser, "OnError() " + errors?.ToString(), ip);
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

        private void HandleExceptionOnUpdate(DbUpdateException e)
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

            var (authenticateduser, ip) = AmitalCloudSecurityUtility.GetAuditInfo();

            ExceptionHandler.HandleException(new Exception(Error), DateTime.Now, 0, "", authenticateduser, "", ip);
        }
    }
}