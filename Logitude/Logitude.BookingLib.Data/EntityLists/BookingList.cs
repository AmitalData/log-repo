using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BookingLib.Data.EntityLists
{
    public partial class BookingList
    {
        [DataMember]
        public DateTime? LastActivityDate { get; set; }
        [DataMember]
        public string LastActivityTypeName { get; set; }
        [DataMember]
        public string LastActivityByUserName { get; set; }

        [DataMember]
        public string Prefix { get; set; }

        [DataMember]
        public string LastSentByUserName { get; set; }

        [DataMember]
        public string MainCarriageFromPortCode { get; set; }

        [DataMember]
        public string MainCarriageFinalDestinationPortCode { get; set; }

    }
}
