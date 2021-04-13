using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    [DataContract]
    public class AccountingPartnerApiFilters
    {
        [DataMember]
        public string SearchCode { get; set; }
        [DataMember]
        public bool ById { get; set; }
        [DataMember]
        public bool ByCode { get; set; }
        [DataMember]
        public bool ByVatNumber { get; set; }
    }
}