

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Threading.Tasks;

namespace WebFreight.Web.CustomWebServices.SignChunks.Common
{


    [ServiceContract]
    public interface IUploadChunksSignService
    {
        [OperationContract]
        Guid CreateUploadSignedBlob(CreateUploadSignedBlobReq myUploadSignedBlobFileReq);
        [OperationContract]
        void AddUploadBlobFileChunkAsync(Guid blobFileId, int chunkId, byte[] data);
        [OperationContract]
        bool FinishUploadFileAsync(Guid blobFileId);
    }
    [DataContract]
    public class ResDataDownloaderBase
    {
        public ResDataDownloaderBase()
        {
            BlobChunksOnly1stWithData = new List<BlobChunks>();
        }
       
           [DataMember]
        public string ServerMD5Hash { set; get; }
        [DataMember]
        public List<BlobChunks> BlobChunksOnly1stWithData { set; get; }

    }
    
}
