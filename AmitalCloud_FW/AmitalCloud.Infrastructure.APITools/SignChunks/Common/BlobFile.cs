using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AmitalCloud.Infrastructure.APITools.Sign
{
    [DataContract]
    public class BlobFile
    {
        public Guid BlobFileId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public long SizeOnClient { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public DateTime? CreatedOn { get; set; }

        [DataMember]
        public string MD5HashClient { get; set; }

        public DateTime CreatedOnServerAt { get; set; }

        [DataMember]
        public byte[] BlobChunks1st { get; set; }

        public virtual ICollection<BlobChunks> Chunks { get; set; }

        public long CurrentTotalSize { get; set; }
    }
}
