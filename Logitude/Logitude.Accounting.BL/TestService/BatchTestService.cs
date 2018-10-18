using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.TestService
{
    public class BatchTestService : BatchTaskExecutionsService
    {
        public BatchTestService(BatchTaskExecutionPM batchTaskExecution):base(batchTaskExecution)
        {

        }

        public override void RunCode()
        {
            int x = 0;
            int y = 900;

            Debug.WriteLine(this.BatchTaskExecution.StatusCode);
            base.RunCode();
            Debug.WriteLine(this.BatchTaskExecution.StatusCode);
        }
    }
}
