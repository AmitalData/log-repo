

using AmitalCloud.Infrastructure.APITools.Sign;
using System;
using System.ServiceModel;
namespace AmitalCloud.Infrastructure.APITools.Interface
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


}
