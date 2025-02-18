using System;
using System.Data.Common;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.Interfaces
{
    public interface IContext : IDisposable
    {
        DbConnection GetConnection();
        DbContext GetActiveDbContext();
        void SetAsModified(object entity);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
        void Dispose(bool disposing);
        int Tenant { get; }
        Database Database { get; }
    }
}
