using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.ExtendedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.Batch
{
    class BatchInterestReportsCreationForCustomers : BatchTaskExecutionsService
    {
        public BatchInterestReportsCreationForCustomers(BatchTaskExecutionPM batchTaskExecution) : base(batchTaskExecution)
        {
        }

        public override void RunCode()
        {
            
        }
    }
}
