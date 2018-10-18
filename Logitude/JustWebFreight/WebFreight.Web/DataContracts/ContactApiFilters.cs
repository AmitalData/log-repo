using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    [DataContract]
    public class ContactApiFilters
    {
        [DataMember]
        public int Skip { get; set; }
        [DataMember]
        public int Take { get; set; }
        [DataMember]
        public bool ByCustomer { get; set; }
        [DataMember]
        public string SearchCode { get; set; }//for customer if by customer
        [DataMember]
        public bool ById { get; set; }//for customer if by customer
        [DataMember]
        public bool ByCode { get; set; }//for customer if by customer
        [DataMember]
        public bool ByVatNumber { get; set; }//for customer if by customer

        [DataMember]
        public string SearchFields { get; set; }

    }
}