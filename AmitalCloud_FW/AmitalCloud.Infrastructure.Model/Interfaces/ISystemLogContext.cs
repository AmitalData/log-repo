using AmitalCloud.Infrastructure.Model.EntityClasses;
using System.Data.Entity;

namespace AmitalCloud.Infrastructure.Model.Interfaces
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
