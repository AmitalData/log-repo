using System;
using System.IO;
using System.Threading.Tasks;
using NLog;

namespace CustomsBook
{
    internal class Program
    {
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();

        static async Task Main(string[] args)
        {
            LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(Path.Combine(AppDomain.CurrentDomain.BaseDirectory , "NLog.config"));
            
            logger.Debug(message: "Start TaskScheduler");

            await UpdateCustomsBook.Run();

            await UpdateAzureSearchAIData.Update();

            //Console.ReadLine();
        }
    }
}
