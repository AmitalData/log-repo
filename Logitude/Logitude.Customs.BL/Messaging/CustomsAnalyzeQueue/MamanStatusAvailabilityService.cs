using Logitude.Customs.BL.CloseTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.Messaging.CustomsAnalyzeQueue
{
    public class MamanStatusAvailabilityService : CustomAnalyzerQueueBase
    {
        public MamanStatusAvailabilityService(InterfaceDetails MyInterfaceDetails)
            :base( MyInterfaceDetails)
        {

        }

        protected override string AnalyzeData(string communicationsData)
        {
            throw new NotImplementedException();
        }
    }
}
