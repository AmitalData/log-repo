using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.Data.DataContracts.SIIRequest
{
    public class ReleaseRequestApiResponseDto
    {
        public int RequestNumber { get; set; }
        public int ResponseCode { get; set; }
        public string ValidationMessages { get; set; }
    }
}
