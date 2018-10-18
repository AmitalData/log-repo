using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebFreight.Web.DataContracts
{


    [DataContract]
    public class AutoSignUpData
    {

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string ContactName { get; set; }

        [DataMember]
        public int NumberOfBranches { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public int NumberOfUsers { get; set; }

        [DataMember]
        public string Comments { get; set; }



    }
}