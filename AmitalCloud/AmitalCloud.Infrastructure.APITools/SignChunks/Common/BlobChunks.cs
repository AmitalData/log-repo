using System;
using System.Runtime.Serialization;

namespace AmitalCloud.Infrastructure.APITools.Sign
{
    public class BlobChunks
    {
        public Guid BlobFileId { get; set; }
        [DataMember]
        public int ChunkId { get; set; }
        [DataMember]
        public int Length { get; set; }
        [DataMember]
        public byte[] Data { get; set; }

        public DateTime CreateAt { get; set; }
        //public virtual BlobFile BlobFile { get; set; }
    }
}
