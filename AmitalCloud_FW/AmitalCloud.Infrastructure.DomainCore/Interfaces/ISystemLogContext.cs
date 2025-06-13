using AmitalCloud.Infrastructure.Domain.EntityClasses;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System.Data.Entity;

namespace AmitalCloud.Infrastructure.Data.Context
{
    public interface ISystemLogContext : IContext
    {
        IDbSet<ContactActivityLog> ContactActivityLogs { get; }
        IDbSet<ErrorLog> ErrorLogs { get; }
        IDbSet<BatchServicesLog> BatchServicesLogs { get; }
        IDbSet<FailedLoginLog> FailedLoginLogs { get; }

        void DetectChanges();
    }
}
