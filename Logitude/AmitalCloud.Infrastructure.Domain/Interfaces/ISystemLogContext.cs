using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;

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
