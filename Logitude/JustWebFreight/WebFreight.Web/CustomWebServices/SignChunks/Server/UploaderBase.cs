
using WebFreight.Web.CustomWebServices.SignChunks.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace WebFreight.Web.CustomWebServices.SignChunks.Server
{
    //public abstract class UploaderBase<ReqData,ResData>         where ReqData : ReqDataBase, new()         where ResData :  ResDataUploaderBase, new()
    public abstract class SignUploader
    {
        public static List<CreateUploadSignedBlobReq> _ListUploadSignedBlobFileReq = new List<CreateUploadSignedBlobReq>();
        public
            //UploaderBase()
            SignUploader()
        {

        }




        public Guid CreateUploadSignedBlob(
            CreateUploadSignedBlobReq myUploadSignedBlobFileReq
            )
        {


            LargeUploadService.AddUploadBlobFileSet(myUploadSignedBlobFileReq);

           if (myUploadSignedBlobFileReq.SizeOnClient == myUploadSignedBlobFileReq.CurrentTotalSize)
           {
               var myBytes = LargeUploadService.FinishUploadFileAsync(myUploadSignedBlobFileReq.BlobFileId);
               //JustDoIt(myUploadSignedBlobFileReq);

               return myUploadSignedBlobFileReq.BlobFileId;

           }
        

            _ListUploadSignedBlobFileReq.Add(new CreateUploadSignedBlobReq()
            {
                BlobFileId = myUploadSignedBlobFileReq.BlobFileId ,
                InterfaceTypeCode = myUploadSignedBlobFileReq.InterfaceTypeCode ,
                CustomsRequestsSheetId = myUploadSignedBlobFileReq.CustomsRequestsSheetId,
                currTenant  = myUploadSignedBlobFileReq.currTenant
            });

            return myUploadSignedBlobFileReq.BlobFileId;
        }

        protected virtual void JustDoIt(CreateUploadSignedBlobReq myUploadSignedBlobFileReq)
        {
            throw new NotImplementedException();
        }


    }
}