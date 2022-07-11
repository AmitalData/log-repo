using Logitude.Customs.BL.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Testers
{
    public class CourierTester
    {
        public static void CourierSchedulerServiceTest()
        {
            var courierSchedulerService = new CourierSchedulerService();
            var date = courierSchedulerService.Send2715Immediate(3, "1-1692152", DateTime.Now.AddHours(3));// if date === null  => SendImmediate

        }
    }
}
