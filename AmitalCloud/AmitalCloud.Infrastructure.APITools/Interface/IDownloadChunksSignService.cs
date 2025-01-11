using System;
using System.ServiceModel;
using AmitalCloud.Infrastructure.APITools.Sign;

namespace AmitalCloud.Infrastructure.APITools.Interface
{
    [ServiceContract]
    public interface IDownloadChunksSignService
    {
        [OperationContract]
        ResSignDataDownloader RegisterDownloadChunkOfBlobToSign(ReqSignData reqData);

        [OperationContract]
        BlobChunks DownloadBlobChunk(Guid BlobFileId, int ChunkId);
        //[OperationContract]
        //Task<Guid> CreateUploadSignedBlob(
        //    CreateUploadSignedBlobReq myBlobFile
        //    );

    }
}
