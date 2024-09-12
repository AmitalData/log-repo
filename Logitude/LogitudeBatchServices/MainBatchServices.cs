using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace LogitudeBatchServices
{
    public static class MainBatchServices
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main(string[] args)
        {
            //if (Environment.UserInteractive)
            //{
            //    LogitudeBatchServices service1 = new LogitudeBatchServices(args);
            //    service1.TestStartupAndStop(args);
            //}
            //else
            //{
             if (args.Count() > 0)
            {
                //string myArgs = "";
                //foreach (var arg in args)
                //{
                //    myArgs += arg + " ";
                //}
                LogitudeBatchServices PatchService = new LogitudeBatchServices(args);
                PatchService.StartLogitudeBatchServices(args);
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

        //}

    }
}
