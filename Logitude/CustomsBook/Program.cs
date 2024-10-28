using System;
using System.IO;
using NLog;

namespace CustomsBook
{
    internal class Program
    {
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory , "NLog.config"));
            
            logger.Debug("Start TaskScheduler");

            UpdateCustomsBook.Run().Wait();

            UpdateAzureSearchAIData.Update();

            //Console.ReadLine();
        }
    }
}
