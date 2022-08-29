using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.CustomWebServices.SignChunks.Common
{
    [ServiceContract]
    public interface IDowonloadChunksSignService
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



    [DataContract]
    public class ReqDataBase
    {
        [DataMember]
        public int? MaxChunkSize { get; set; }
    }

    [DataContract]
    public class CreateUploadSignedBlobReq : BlobFile
    {
        [DataMember]
        public String CustomsRequestsSheetId { get; set; }
        [DataMember]
        public String InterfaceTypeCode { get; set; }
        [DataMember]
        public String CurrentSignCertificate { get; set; }
        [DataMember]
        public int currTenant { get; set; }


    }

    [DataContract]
    public class ReqSignData : ReqDataBase
    {
        [DataMember]
        public string CurrentSignCertificate { get; set; }
        [DataMember]
        public bool isCompanySignOn { get; set; }
        [DataMember]
        public bool isPersonalSignOn { get; set; }


    }
    [DataContract]
    public class ResSignDataDownloader : ResDataDownloaderBase
    {
        [DataMember]
        public String CustomsRequestsSheetId { get; set; }
        [DataMember]
        public String InterfaceTypeCode { get; set; }
        [DataMember]
        public int? currTenant { get; set; }

    }
    [DataContract]
    public class ReceiveBytesToSignReq
    {
        [DataMember]
        public string CurrentSignCertificate { get; set; }
        [DataMember]
        public bool isCompanySignOn { get; set; }
        [DataMember]
        public bool isPersonalSignOn { get; set; }
    }
    [DataContract]
    public class ReceiveBytesToSignResponse
    {
        [DataMember]
        public Byte[] ReceiveBytesToSign { get; set; }


        [DataMember]
        public String CustomsRequestsSheetId { get; set; }
        [DataMember]
        public String InterfaceTypeCode { get; set; }
        [DataMember]
        public int currTenant { get; set; }


    }



    [DataContract]
    public class ExportReqSignData : ReqSignData
    {
        [DataMember]
        public int Tenant { get; set; }


        [DataMember]
        public bool ToCheckSignCertificate { get; set; }

    }

    [DataContract]
    public class ResponseExportSignTask : ReceiveBytesToSignResponse
    {

        [DataMember]
        public string queueId { get; set; }


        [DataMember]
        public bool HasError { get; set; }
        [DataMember]
        public bool IsAuthenticationError { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string InnerErrorMessage { get; set; }


        [DataMember]
        public SignCertificateCheck ResultSignCertificateCheck { get; set; }



    }

    [DataContract]
    public class SignCertificateCheck
    {
        [DataMember]
        public bool isCompanySignOn { get; set; }
        [DataMember]
        public bool isPersonalSignOn { get; set; }

    }

}
