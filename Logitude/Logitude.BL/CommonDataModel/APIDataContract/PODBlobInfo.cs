using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract
{
    public class PODBlobInfo
    {
        public string FileName { get; set; }
        public string BlobId { get; set; }
        public string Extension { get; set; }
        public long BlobSize { get; set; }
        public byte[] BlobChunk { get; set; }
        public int BlobChunkNumber { get; set; }
        public List<string> BlobChunkIdsList { get; set; }
        public long TotalSentChunksSize { get; set; }

        public string ShipmentNumber { get; set; }
        public int Tenant { get; set; }
        public string SecurityKey { get; set; }
        public string DocumentId { get; set; }
    }
}
