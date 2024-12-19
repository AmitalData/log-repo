using AmitalCloud.Infrastructure.APITools.Sign;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace AmitalCloud.Infrastructure.APITools.BaseClasses
{
    public class RestDataDownloaderBase
    {
        public RestDataDownloaderBase()
        {
            BlobChunksOnly1stWithData = new List<BlobChunks>();
        }

        [DataMember]
        public string ServerMD5Hash { set; get; }
        [DataMember]
        public List<BlobChunks> BlobChunksOnly1stWithData { set; get; }

    }

}
