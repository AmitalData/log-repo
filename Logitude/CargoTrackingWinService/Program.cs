using CargoTrackingWinService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CargoTrackingWinService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {


#if RELEASE
            ApplicationInfo.Mode = "Release";
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
#else

            ApplicationInfo.Mode = "Debug";
            Service1 myService = new Service1();
            myService.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);               
            
#endif


 

        }
    }
}
