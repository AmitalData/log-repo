using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.RabbitMQ
{
    public class RabbitmqHelper
    {
        internal static bool CreateChannelUntilSuccess(RabbitmqConnection connection, out string errMessage)
        {
            int counter = 0;
            int sec = 5;
            while (true)
            {
                counter++;
                if (!CreateChannel(connection, out errMessage))
                {
                    Thread.Sleep(sec * 1000);
                }
                if (counter % 100 == 0)
                {
                    Thread.Sleep(30 * 1000);
                    sec++;
                    counter = 0;
                    sec = Math.Max(30, sec);
                }
            }
        }
        internal static bool CreateChannel(RabbitmqConnection connection, out string errMessage)
        {
            errMessage = "";
            CloseChannel(connection);
            var factory = GetConnectionFactory();
            try
            {
                connection.Connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                errMessage = $"Failed to close connection{Environment.NewLine}{ex.ToString()}";
               // MyThreadManager.LogMe(errMessage, connection.ThreadParam, true);
                return (false);
            }
            try
            {
                connection.Channel = connection.Connection.CreateModel();
            }
            catch (Exception ex)
            {
                errMessage = $"Failed to close channel{Environment.NewLine}{ex.ToString()}";
               // MyThreadManager.LogMe(errMessage, connection.ThreadParam, true);
                return (false);
            }
            connection.ChannelCreateDate = DateTime.Now;
            return (true);
        }

        internal static bool CloseChannel(RabbitmqConnection connection)
        {
            if (connection == null)
                return (true);
            if (connection.Channel != null)
            {
                try
                {
                    if (connection.Channel.IsOpen)
                        connection.Channel.Close();
                    Thread.Sleep(500);
                    connection.Channel.Dispose();
                    connection.Channel = null;
                }
                catch (Exception ex)
                {
                  //  MyThreadManager.LogMe($"Failed to close channel{Environment.NewLine}{ex.ToString()}", connection.ThreadParam, true);
                    return (false);
                }
            }
            if (connection.Connection != null)
            {
                try
                {
                    connection.Connection.Close();
                    Thread.Sleep(500);
                    connection.Connection.Dispose();
                    connection.Connection = null;
                }
                catch (Exception ex)
                {
                   // MyThreadManager.LogMe($"Failed to close connection{Environment.NewLine}{ex.ToString()}", connection.ThreadParam, true);
                    return (false);
                }
            }
            return (true);
        }

        internal static bool IsConnected(RabbitmqConnection connection)
        {
            try
            {
                if (connection.Channel == null)
                    return (false);
                if (connection.Channel.IsOpen)
                {

                    return (true);
                }
                return (false);
            }
            catch (Exception ex)
            {
               // MyThreadManager.LogMe($"IsConnected check failed{Environment.NewLine}{ex.ToString()}", connection.ThreadParam, true);
                return (false);
            }
        }

        internal static int GetMessageCount(RabbitmqConnection connection, string queuename)
        {
            if (IsConnected(connection))
            {
                var result = connection.Channel.QueueDeclarePassive(queue: queuename);
                if (result != null)
                    return ((int)result.MessageCount);
            }
            return (-1);
        }

        public static ConnectionFactory GetConnectionFactory()
        {
            //var factory = new ConnectionFactory() { HostName = "unimq", UserName = "v5101", Password = "Aa123" };
            return (new ConnectionFactory() { HostName = ConfigurationManager.AppSettings["RabbitmqHost"], UserName = ConfigurationManager.AppSettings["RabbitmqUsername"], Password = ConfigurationManager.AppSettings["RabbitmqPassword"] }); ;
        }

        public static bool DeclareQueue(RabbitmqConnection connection, string queuename, out string errMessage)
        {
            errMessage = "";
            try
            {
                var args = new Dictionary<string, object>();
                //lazy = store messages to disk => no lost messages in case on rabbitmq restart 
                args.Add("x-queue-mode", "lazy");
                connection.Channel.QueueDeclare(queue: queuename,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: args);
                return (true);
            }
            catch (Exception ex)
            {
                errMessage = $"Failed to QueueDeclare{Environment.NewLine}{ex.ToString()}";
                //MyThreadManager.LogMe(errMessage, connection.ThreadParam, true);
                return (false);
            }
        }

        public static void DeclareQueue(IModel channel, string queuename)
        {
            try
            {
                var args = new Dictionary<string, object>();
                //lazy = store messages to disk => no lost messages in case on rabbitmq restart 
                args.Add("x-queue-mode", "lazy");
                channel.QueueDeclare(queue: queuename,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: args);
            }
            catch (Exception ex)
            {

               // Logger.LogMe($"Failed to QueueDeclare '{queuename}'{Environment.NewLine}{ex.ToString()}", true);
                throw;
            }
        }
        //internal static byte[] MyJsonSerializer(object obj)
        //{
        //    var options = new JsonSerializerOptions { WriteIndented = true };
        //    string jsonString = JsonSerializer.Serialize(obj, options);
        //    return (Encoding.UTF8.GetBytes(jsonString));
        //}

        //internal static string MyJsonSerializerAsString(object obj)
        //{
        //    var options = new JsonSerializerOptions { WriteIndented = true };
        //    string jsonString = JsonSerializer.Serialize(obj, options);
        //    return (jsonString);
        //}


    }

}
