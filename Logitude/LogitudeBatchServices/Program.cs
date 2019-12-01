using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LogitudeBatchServices
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                LogitudeBatchServices service1 = new LogitudeBatchServices(args);
                service1.TestStartupAndStop(args);
            }
            else
            {
                if (args.Count() > 0)
                {
                    LogitudeBatchServices service1 = new LogitudeBatchServices(args);
                    service1.StartMe(args[0]);
                }
                else
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                        {
                             new LogitudeBatchServices()
                        };
                    ServiceBase.Run(ServicesToRun);
                }
            }

        }

    }
}
