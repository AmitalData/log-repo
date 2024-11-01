using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using AmitalCloud.Infrastructure.Domain.Interfaces;
namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        //The following Property is going to hold the context object
        IContext Context { get; }

        //Start the database Transaction
 //       void CreateTransaction();
        void CreateTransactionScope(TransactionScopeOption option);
        //Commit the database Transaction
        void Commit();

        //Rollback the database Transaction
        void Rollback();

        //DbContext Class SaveChanges method
        void Save();
    }
}
