using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract
{
    public class BlobInfo
    {
       // public string BlobName { get; set; }
        public string BlobId { get; set; }
        public string Extension { get; set; }
        public long BlobSize { get; set; }
        public byte[] BlobChunk { get; set; }
        public int BlobChunkNumber { get; set; }
        public List<string> BlobChunkIdsList { get; set; }
        public long TotalSentChunksSize { get; set; }
        public bool UploadAsOneBlock { get; set; }


    }
}
