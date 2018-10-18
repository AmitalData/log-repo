using WebFreight.Web.CustomWebServices.SignChunks.Common;
using WebFreight.Web.CustomWebServices.SignChunks.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading.Tasks;
using System.Web;

namespace WebFreight.Web.CustomWebServices
{
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class SignService 
    {

        public ResSignDataDownloader RegisterDownloadChunkOfBlobToSign(ReqSignData reqData)
        {
            
            var mySignDownloader = new SignDownloader();
            var myResSignDataDownloader = mySignDownloader.Doit(reqData);
            return myResSignDataDownloader;
            
        }
        public BlobChunks DownloadBlobChunk(Guid BlobFileId, int ChunkId)
        {
            var largeDownloadUploadService = new LargeDownloadService();
            return largeDownloadUploadService.DownloadBlobChunksAsync(BlobFileId, ChunkId);
        }



    }
    public partial class SignService //: IUploadChunksSignService
    {

        static List<CreateUploadSignedBlobReq> _ListUploadSignedBlobFileReq = new List<CreateUploadSignedBlobReq>();

        public Guid CreateUploadSignedBlob(CreateUploadSignedBlobReq myUploadSignedBlobFileReq)
        {


            LargeUploadService.AddUploadBlobFileSet(myUploadSignedBlobFileReq);
            lock (_ListUploadSignedBlobFileReq)
            {
                _ListUploadSignedBlobFileReq.Add(new CreateUploadSignedBlobReq()
                {
                    BlobFileId = myUploadSignedBlobFileReq.BlobFileId,
                    InterfaceTypeCode = myUploadSignedBlobFileReq.InterfaceTypeCode,
                    CustomsRequestsSheetId = myUploadSignedBlobFileReq.CustomsRequestsSheetId,
                    currTenant = myUploadSignedBlobFileReq.currTenant
                });
            }
            

            if (myUploadSignedBlobFileReq.SizeOnClient == myUploadSignedBlobFileReq.CurrentTotalSize)
            {
                var myBytes = LargeUploadService.FinishUploadFileAsync(myUploadSignedBlobFileReq.BlobFileId);
                JustCompleteResponseAllSignBytes(myUploadSignedBlobFileReq.BlobFileId, myBytes);

                return myUploadSignedBlobFileReq.BlobFileId;

            }


            

            return myUploadSignedBlobFileReq.BlobFileId;
        }

        void JustCompleteResponseAllSignBytes(Guid blobFileId ,Byte[] mySignedBytes)
        {
            CreateUploadSignedBlobReq myUploadSignedBlobFileReq = null;
            lock (_ListUploadSignedBlobFileReq)
            {
                myUploadSignedBlobFileReq = _ListUploadSignedBlobFileReq.Single(rec => rec.BlobFileId == blobFileId);
                _ListUploadSignedBlobFileReq.Remove(myUploadSignedBlobFileReq);
            }
            
            this.CompleteResponseSignBytes(
                myUploadSignedBlobFileReq.currTenant,
                myUploadSignedBlobFileReq.CustomsRequestsSheetId,
                myUploadSignedBlobFileReq.InterfaceTypeCode,
                myUploadSignedBlobFileReq.CurrentSignCertificate,
                mySignedBytes);
                
 
        }






        public void AddUploadBlobFileChunkAsync(Guid blobFileId, int chunkId, byte[] data)
        {
            LargeUploadService.AddUploadBlobFileChunkAsync(blobFileId, chunkId, data);
        }

        public bool FinishUploadFileAsync(Guid blobFileId)
        {

            var myBytes = LargeUploadService.FinishUploadFileAsync(blobFileId);
            JustCompleteResponseAllSignBytes(blobFileId, myBytes);

            return true;


        }
    }
    


    public class SignDownloader : DownloaderBase<ReqSignData, ResSignDataDownloader>
    {


        protected override Stream GetMyStream(ReqSignData reqData)
        {
            String CustomsRequestsSheetId; string InterfaceTypeCode; int? currTenant;
            //await Task.Delay(10);
            var mySignService = new SignService();
            var ret = mySignService.ReceiveBytesToSign(reqData.CurrentSignCertificate, reqData.isCompanySignOn, reqData.isPersonalSignOn,
            out CustomsRequestsSheetId, out InterfaceTypeCode, out currTenant);
            
            var myResSignDataDownloader = this._ResData as ResSignDataDownloader;

            myResSignDataDownloader.CustomsRequestsSheetId = CustomsRequestsSheetId;
            myResSignDataDownloader.InterfaceTypeCode = InterfaceTypeCode;
            myResSignDataDownloader.currTenant = currTenant;
            if (ret == null)
            {
                return null;
            }
            var myMemoryStream = new MemoryStream(ret) as Stream;
            return myMemoryStream;
        }
        protected override ResSignDataDownloader ManipulateResponse(ResSignDataDownloader resData)
        {
            return base.ManipulateResponse(resData);
        }

    }
}
