using WebFreight.Web.CustomWebServices.SignChunks.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.CustomWebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISignService" in both code and config file together.
    [ServiceContract]
    public interface ISignService : ISignServiceUpdaterApplicationBlock ,
IDowonloadChunksSignService, IUploadChunksSignService

        
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
