using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    [DataContract]
    public class OpportunityApiFilters
    {
        [DataMember]
        public bool MyOpportunities { get; set; }
        [DataMember]
        public bool IsOpen { get; set; }
        [DataMember]
        public string CustomerId { get; set; }
    }
}