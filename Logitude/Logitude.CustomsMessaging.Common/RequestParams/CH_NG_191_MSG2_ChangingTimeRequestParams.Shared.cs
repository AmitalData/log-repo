using System;
using System.Collections.Generic;
using System.Linq;


namespace Logitude.CustomsMessaging.Common.RequestParams
{
    public class CH_NG_191_MSG2_ChangingTimeRequestParams : RequestParamsBase
    {
        public int Id { get; set; }
        public DateTime? DateSearchFrom { get; set; }
        public DateTime? DateSearchTo { get; set; }
        public string RequestType { get; set; }
        public bool DateSearchFromSpecified { get; set; }
        public bool DateSearchToSpecified { get; set; }
        public bool QueueDateSpecified { get; set; }
        public DateTime? QueueDate { get; set; }
        public string PhysicalCheckId { get; set; }
        public bool BringQueueForwardIndicator { get; set; }
        public string CheckTypeCode { get; set; }
       
    }
}