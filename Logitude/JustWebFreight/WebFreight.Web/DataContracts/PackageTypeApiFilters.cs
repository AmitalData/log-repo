using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebFreight.Web.DataContracts
{
    [DataContract]
    public class PackageTypeApiFilters
    {
        [DataMember]
        public int Skip { get; set; }
        [DataMember]
        public int Take { get; set; }
      
        [DataMember]
        public string SearchFields { get; set; }
    }
}
