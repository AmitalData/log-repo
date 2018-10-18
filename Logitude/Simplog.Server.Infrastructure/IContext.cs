using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure
{
    public interface IContext
    {
        int SaveChanges();
        DbConnection GetConnection();
        DbContext GetActiveDbContext();
    }
}
