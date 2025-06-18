using AmitalCloud.Infrastructure.Data.Services;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using Microsoft.ServiceBus.Messaging;
using System;

namespace AmitalCloud.Infrastructure.Data.Helpers
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
        public static TValue GetProperty<TValue>(this QueueResponse msg, QueuePropertyNames key, TValue defualtValue)
            where TValue : IConvertible
        {
            if (!msg.MessageValues.ContainsKey(key.ToString()))
            {
                msg.MessageValues[key.ToString()] = defualtValue.ToString();
            }
            var val = msg.MessageValues[key.ToString()].ChangeValue<TValue>();
            return val;
        }
        public static TValue GetProperty<TValue>(this CustomDBQueueMessage msg, QueuePropertyNames key, TValue defualtValue)
            where TValue : IConvertible
        {
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
        public static TValue SetProperty<TValue>(this CustomDBQueueMessage msg, QueuePropertyNames key, TValue newValue)
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
                msg.Complete();
                return true;
            }
            catch (MessageLockLostException)
            {
            }
            catch (MessagingException)
            {
            }
            return false;
        }
        public static bool SafeAbandon(this BrokeredMessage msg)
        {
            try
            {
                msg.Abandon(msg.Properties);
                return true;
            }
            catch (MessageLockLostException)
            {
            }
            catch (MessagingException)
            {
            }
            return false;
        }
    }
    public enum SBQueueNames
    {
        CustomsMessagingSheetBQ, SendDataToExternalServicesBQ, updateclosedtables, SendWEBAPIMessage2MamanQ
            , AnalyzeQueueMQ
            , CustomsHSMSignWR
    }
}
