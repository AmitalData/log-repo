using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomsWorkerRole.Queue
{
#if false //need EnterpriseLibrary.WindowsAzure.TransientFaultHandling
    

    public abstract class BaseController : Controller
    {
        protected RetryStrategy retryStrategy;
        protected RetryPolicy<SqlAzureTransientErrorDetectionStrategy> retryPolicy;

        protected BaseController()
        {
            retryStrategy = new Incremental(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
            retryPolicy = new RetryPolicy<SqlAzureTransientErrorDetectionStrategy>(retryStrategy);

            retryPolicy.Retrying += (sender, args) => LogRetry(args);
        }
       

        void doit()
        {
            //var retryManager = EnterpriseLibraryContainer.Current.GetInstance<RetryManager>();
            
            //var retryPolicy = retryManager.GetRetryPolicy<StorageTransientErrorDetectionStrategy>(ConfigurationHelper.ReadFromServiceConfigFile(Constants.DefaultRetryStrategyForTableStorageOperationsKey));
            var retryPolicy = new RetryPolicy()

            var waitTimeout = TimeSpan.FromSeconds(10);

            // Declare an action acting as a callback whenever a message arrives on a queue.
            AsyncCallback completeReceive = null;

            // Declare an action acting as a callback whenever a non-transient
            // exception occurs while receiving or processing messages.
            Action<Exception> recoverReceive = null;

            // Declare a cancellation token that is used to signal an exit from the receive loop.
            var cts = new CancellationTokenSource();

            // Declare an action implementing the main processing logic for received messages.
            Action<BrokeredMessage> processMessage = ((msg) =>
            {
                // Put your custom processing logic here. DO NOT swallow any exceptions.
            });

            // Declare an action responsible for the core operations in the message receive loop.
            Action receiveMessage = (() =>
            {
                // Use a retry policy to execute the Receive action in an asynchronous and reliable fashion.
                retryPolicy.ExecuteAction
                (
                    (cb) =>
                    {
                        // Start receiving a new message asynchronously.
                        queueClient.BeginReceive(waitTimeout, cb, null);
                    },
                    (ar) =>
                    {
                        // Make sure we are not told to stop receiving while we were waiting for a new message.
                        if (!cts.IsCancellationRequested)
                        {
                            // Complete the asynchronous operation. 
                            // This may throw an exception that will be handled internally by retry policy.
                            BrokeredMessage msg = queueClient.EndReceive(ar);

                            // Check if we actually received any messages.
                            if (msg != null)
                            {
                                // Make sure we are not told to stop receiving while we were waiting for a new message.
                                if (!cts.IsCancellationRequested)
                                {
                                    try
                                    {
                                        // Process the received message.
                                        processMessage(msg);

                                        // With PeekLock mode, we should mark the processed message as completed.
                                        if (queueClient.Mode == ReceiveMode.PeekLock)
                                        {
                                            // Mark brokered message as completed at which point it's removed from the queue.
                                            msg.SafeComplete();
                                        }
                                    }
                                    catch
                                    {
                                        // With PeekLock mode, we should mark the failed message as abandoned.
                                        if (queueClient.Mode == ReceiveMode.PeekLock)
                                        {
                                            // Abandons a brokered message. 
                                            // This will cause Service Bus to unlock the message and make it available 
                                            // to be received again, either by the same consumer or by another completing consumer.
                                            msg.SafeAbandon();
                                        }

                                        // Re-throw the exception so that we can report it in the fault handler.
                                        throw;
                                    }
                                    finally
                                    {
                                        // Ensure that any resources allocated by a BrokeredMessage instance are released.
                                        msg.Dispose();
                                    }
                                }
                                else
                                {
                                    // If we were told to stop processing, 
                                    // the current message needs to be unlocked and return back to the queue.
                                    if (queueClient.Mode == ReceiveMode.PeekLock)
                                    {
                                        msg.SafeAbandon();
                                    }
                                }
                            }
                        }

                        // Invoke a custom callback method to indicate that we 
                        // have completed an iteration in the message receive loop.
                        completeReceive(ar);
                    },
                    (ex) =>
                    {
                        // Invoke a custom action to indicate that we have encountered an exception and
                        // need further decision as to whether to continue receiving messages.
                        recoverReceive(ex);
                    });
            });

            // Initialize a custom action acting as a callback whenever a message arrives on a queue.
            completeReceive = ((ar) =>
            {
                if (!cts.IsCancellationRequested)
                {
                    // Continue receiving and processing new messages until we are told to stop.
                    receiveMessage();
                }
            });

            // Initialize a custom action acting as a callback whenever a
            // non-transient exception occurs while receiving or processing messages.
            recoverReceive = ((ex) =>
            {
                // Just log an exception. Do not allow an unhandled exception to
                // terminate the message receive loop abnormally.
                Trace.TraceError(ex.Message);

                if (!cts.IsCancellationRequested)
                {
                    // Continue receiving and processing new messages until
                    // we are told to stop regardless of any exceptions.
                    receiveMessage();
                }
            });

            // Start receiving messages asynchronously.
            receiveMessage();

            // Perform any other work. Message will keep arriving asynchronously 
            // while we are busy doing something else.

            // Stop the message receive loop gracefully.
            cts.Cancel();
        }
    }
#endif
}
