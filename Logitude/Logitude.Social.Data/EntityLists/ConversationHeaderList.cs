using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Social.Data.EntityLists
{
    public partial class ConversationHeaderList
    {
        [DataMember]
        public string LasMessageUserId { get; set; }
        [DataMember]
        public string LasMessageUserName { get; set; }
        [DataMember]
        public string LasMessageBody { get; set; }

        [DataMember]
        public string MessageParticipants { get; set; }

        [DataMember]
        public DateTime? LastMessageDate { get; set; }

        [DataMember]
        public int MessageParticipantsCount { get; set; }

        [DataMember]
        public bool IsRead { get; set; }
    }
}
