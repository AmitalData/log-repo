using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.DataContracts
{
    [DataContract]
    public class FeatureAccessInfo
    {
        [DataMember]
        public string ObjectTableName { get; set; }
        [DataMember]
        public string FeatureCode { get; set; }
        [DataMember]
        public bool HasAccess { get; set; }
    }
}
