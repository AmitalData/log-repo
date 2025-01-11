using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.APITools.Interface
{
    public interface IPOAExpireReminder
    {
        void StartRun(string taskId, int seedDefaultTenant);
    }
}
