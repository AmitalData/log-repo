using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.Data.EntityLists
{
     
    public partial class OpportunityList
    {
        [DataMember]
        public DateTime? LastActivityDate { get; set; }
        [DataMember]
        public string LastActivityTypeName { get; set; }
        [DataMember]
        public string LastActivityByUserName { get; set; }
        [DataMember]
        public byte[] LastModified { get; set; }
        [DataMember]
        public string LastStageName { get; set; }

        [DataMember]
        public string NumberOfShipmentsForeground { get; set; }
    }
}
