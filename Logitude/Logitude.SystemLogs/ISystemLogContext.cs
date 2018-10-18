using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.SystemLogs.POCOs;

namespace Logitude.SystemLogs
{
    public interface ISystemLogContext
    {
        IDbSet<ContactActivityLog> ContactActivityLogs { get; }
        IDbSet<ErrorLog> ErrorLogs { get; }
        IDbSet<BatchServicesLog> BatchServicesLogs { get; }
        IDbSet<FailedLoginLog> FailedLoginLogs { get; }
        IDbSet<FailedTokenLog> FailedTokenLogs { get; }

        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}
