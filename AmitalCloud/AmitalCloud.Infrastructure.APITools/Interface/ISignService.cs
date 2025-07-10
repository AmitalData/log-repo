using AmitalCloud.Infrastructure.APITools.Sign;
using System;
using System.ServiceModel;

namespace AmitalCloud.Infrastructure.APITools.Interface
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISignService" in both code and config file together.
    [ServiceContract]
    public interface ISignService : ISignServiceUpdaterApplicationBlock, IDownloadChunksSignService, IUploadChunksSignService
    {
        [OperationContract]
        void DoWork();
        [OperationContract]
        Byte[] ReceiveBytesToSign(string CurrentSignCertificate, bool isCompanySignOn, bool isPersonalSignOn,
            out String CustomsRequestsSheetId, out string InterfaceTypeCode, out int? currTenant);

        [OperationContract]
        ReceiveBytesToSignResponse GetBytesToSign(ReceiveBytesToSignReq myReceiveBytesToSignReq);

        [OperationContract]
        void CompleteResponseSignBytes(int tenant, String CustomsRequestsSheetId, string InterfaceTypeCode, string CurrentSignCertificateName, Byte[] mySignBytes);
        [OperationContract]
        void GetTenantFromCertificate(string CurrentSignCertificate, out string PersonalTenantCommaDelimitedList, out string CompanyTenant);


    }

}
