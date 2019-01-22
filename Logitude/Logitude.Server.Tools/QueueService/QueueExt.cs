using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
    public static class QueueExt
    {

        public enum QueuePropertyNames : int
        {
            Priority, LastExecAt, Retries, InterfaceTypeCode,
            DebugMode,
            ProcessState,
            Tenant, DcaAnalyzeAggregateKey,
            CorrelationId

        }



        public static TValue GetProperty<TValue>(this Logitude.Server.Tools.QueueService.QueueResponse msg, QueuePropertyNames key, TValue defualtValue)
            where TValue : IConvertible
        {

            TValue value = default(TValue);
            if (!msg.MessageValues.ContainsKey(key.ToString()))
            {
                msg.MessageValues[key.ToString()] = defualtValue.ToString();
            }

            var val = msg.MessageValues[key.ToString()].ChangeValue<TValue>();
            return val;

        }
        public static TValue GetProperty<TValue>(this Logitude.Server.Tools.QueueService.CustomDBQueueMessage msg, QueuePropertyNames key, TValue defualtValue)
            where TValue : IConvertible
        {

            TValue value = default(TValue);
            if (!msg.Properties.ContainsKey(key.ToString()))
            {
                msg.Properties[key.ToString()] = defualtValue.ToString();
            }

            var val = msg.Properties[key.ToString()].ChangeValue<TValue>();
            return val;

        }
        public static TValue GetProperty<TValue>(this BrokeredMessage msg, QueuePropertyNames key, TValue defualtValue)
            where TValue : IConvertible
        {

            TValue value = default(TValue);
            if (!msg.Properties.ContainsKey(key.ToString()))
            {
                msg.Properties[key.ToString()] = defualtValue;
            }
            var val = (TValue)msg.Properties[key.ToString()];
            return val;

        }
        public static TValue SetProperty<TValue>(this BrokeredMessage msg, QueuePropertyNames key, TValue newValue)
            where TValue : IConvertible
        {
            msg.GetProperty<TValue>(key, newValue);

            msg.Properties[key.ToString()] = newValue; ;
            return newValue;

        }
        public static TValue SetProperty<TValue>(this Logitude.Server.Tools.QueueService.CustomDBQueueMessage msg, QueuePropertyNames key, TValue newValue)
            where TValue : IConvertible
        {
            msg.GetProperty<TValue>(key, newValue);

            msg.Properties[key.ToString()] = newValue.ToString(); ;
            return newValue;

        }
        
        public static T ChangeValue<T>(this string value)
            where T : IConvertible
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
        public static bool SafeComplete(this BrokeredMessage msg)
        {
            try
            {
                // Mark brokered message as complete.
                msg.Complete();

                // Return a result indicating that the message has been completed successfully.
                return true;
            }
            catch (MessageLockLostException)
            {
                // It's too late to compensate the loss of a message lock. We should just ignore it so that it does not break the receive loop.
                // We should be prepared to receive the same message again.
            }
            catch (MessagingException)
            {
                // There is nothing we can do as the connection may have been lost, 
                // or the underlying topic/subscription may have been removed.
                // If Complete() fails with this exception, the only recourse is to prepare to receive another message (possibly the same one).
            }

            return false;
        }

        public static bool SafeAbandon(this BrokeredMessage msg)
        {
            try
            {
                // Abandons a brokered message. This will cause the Service Bus to
                // unlock the message and make it available to be received again, 
                // either by the same consumer or by another competing consumer.


                msg.Abandon(msg.Properties);
                ///msg.Abandon();

                // Return a result indicating that the message has been abandoned successfully.
                return true;
            }
            catch (MessageLockLostException)
            {
                // It's too late to compensate the loss of a message lock.
                // We should just ignore it so that it does not break the receive loop.
                // We should be prepared to receive the same message again.
            }
            catch (MessagingException)
            {
                // There is nothing we can do as the connection may have been lost,
                //  or the underlying topic/subscription may have been removed.
                // If Abandon() fails with this exception, the only recourse is to receive another message (possibly the same one).
            }

            return false;
        }
    }

    public enum SBQueueNames
    {
        //CustomsMessagingOutBQ, 
        CustomsMessagingSheetBQ, SendDataToExternalServicesBQ, updateclosedtables, SendWEBAPIMessage2MamanQ
    }
    
}
