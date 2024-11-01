using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Data
{
    public static class ApplicationAppInfo
    {

        public static bool WorkerRoleCall { get; set; }

        public static int GetDataBaseTimeOut()
        {
            int timeout = 120;
            if (WorkerRoleCall)
            {
                timeout = 1200;
            }
            return timeout;


        }

    }

}
