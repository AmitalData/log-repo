using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.DataContracts
{
    [DataContract]
    public class PortApiFilters
    {
        [DataMember]
        public string PortCode { get; set; }
        [DataMember]
        public string CountryCode { get; set; }
    }
}
