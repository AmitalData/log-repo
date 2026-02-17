using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Simplog.Server.Infrastructure.Helpers
{
    public class TransactionFactory
    {

        public static TransactionScope GetNewTransactionSuppress()// testing 1 2 3  ???  
        {
            //#if ORACLE_DB

            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                return new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted });
            }

            //#endif

            return new TransactionScope(TransactionScopeOption.Suppress, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot });

        }

        public static TransactionScope GetNewTransaction(TimeSpan? timeOut = null)
        {
            //#if ORACLE_DB

            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                if (timeOut != null)
                {
                    return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { Timeout = timeOut.Value, IsolationLevel = IsolationLevel.ReadCommitted });
                }
                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted });
            }

            //#endif
            if (timeOut != null)
            {
                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot, Timeout = timeOut.Value });
            }
            return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot });

        }
        public static TransactionScope GetNewOracleReadCommittedTransaction(TimeSpan? timeOut = null)
        {
#if ORACLE_DB
            if (timeOut != null)
            {
                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { Timeout = timeOut.Value /*IsolationLevel = IsolationLevel.Snapshot*/ });
            }
            return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() {/*IsolationLevel = IsolationLevel.Snapshot*/ });
#endif
            if (timeOut == null)
            {
                timeOut = TimeSpan.FromMinutes(3);
            }
            if (timeOut != null)
            {
                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = timeOut.Value });
            }
            return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted });

        }
        public static TransactionScope GetNewSerializableTransaction(TimeSpan? timeOut = null)
        {
            //#if ORACLE_DB
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                if (timeOut != null)
                {
                    return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { Timeout = timeOut.Value, IsolationLevel = IsolationLevel.Serializable });
                }
                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Serializable });
            }
            //#endif
            if (timeOut != null)
            {
                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Serializable, Timeout = timeOut.Value });
            }
            return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Serializable });

        }

        public static TransactionScope GetTransaction(TimeSpan? timeOut = null)
        {
            //#if ORACLE_DB
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                if (timeOut != null)
                {
                    if (Transaction.Current != null)
                    {
                        return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { Timeout = timeOut.Value, IsolationLevel = IsolationLevel.ReadCommitted });
                    }
                    return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { Timeout = timeOut.Value, IsolationLevel = IsolationLevel.ReadCommitted });
                }
                if (Transaction.Current != null)
                {
                    return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted });
                }
                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted });
            }
            //#endif
            if (timeOut != null)
            {
                if (Transaction.Current != null)
                {
                    return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot, Timeout = timeOut.Value });
                }
                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot, Timeout = timeOut.Value });
            }
            if (Transaction.Current != null)
            {
                return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot });
            }
            return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.Snapshot });
        }

        public static TransactionScope GetNewTransactionWithDefaultIsolationLevel(TimeSpan? timeOut = null)
        {
            if (timeOut != null)
            {
                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { Timeout = timeOut.Value });
            }
            return new TransactionScope(TransactionScopeOption.RequiresNew);
        }

        public static TransactionScope GetNewReadCommittedTransaction(TimeSpan? timeOut = null)
        {
            if (timeOut != null)
            {
                return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = timeOut.Value });
            }
            return new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted });
        }
        // I USING THAT WHILE DEBUG AT IMMEDIATE WINDOW>
        //Simplog.Server.Infrastructure.Helpers.TransactionFactory.RegisterTransactionCompleted()
        public static void RegisterTransactionCompleted()
        {
            if (Transaction.Current == null) return;
            //Register for the transaction completed event for the current transaction
            Transaction.Current.TransactionCompleted += new TransactionCompletedEventHandler(Current_TransactionCompleted);


        }
        static void Current_TransactionCompleted(object sender, TransactionEventArgs e)
        {
            Console.WriteLine(Environment.StackTrace.ToString());
            Console.WriteLine("A transaction has completed:");
            Console.WriteLine("ID:             {0}", e.Transaction.TransactionInformation.LocalIdentifier);
            Console.WriteLine("Distributed ID: {0}", e.Transaction.TransactionInformation.DistributedIdentifier);
            Console.WriteLine("Status:         {0}", e.Transaction.TransactionInformation.Status);
            Console.WriteLine("IsolationLevel: {0}", e.Transaction.IsolationLevel);
        }
    }


    public class TransactionFactoryWrapper
    {
        public virtual IDisposable GetTransaction(TimeSpan? timeOut = null)
        {
            return TransactionFactory.GetTransaction(timeOut) as IDisposable;
        }
    }

}
