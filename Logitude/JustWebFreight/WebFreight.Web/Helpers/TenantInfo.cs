using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebFreight.Web.Helpers
{
    [DataContract]
    public class TenantInfo
    {
        [DataMember]
        public int Tenant { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}