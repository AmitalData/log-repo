using System.Runtime.Serialization;
using System.ServiceModel;

namespace Logitude.BL.Helpers.ExportServer
{

    [ServiceContract]
    public interface ILoginWcfService
    {
        [OperationContract]
        Response LoginByCredential(string email, APICredentialsParameters apiCredentialsParam);
    }

    [DataContract(Name = "APICredentialsParameters", Namespace = "http://schemas.datacontract.org/2004/07/WebFreight.Web.Helpers")]
    public class APICredentialsParameters
    {
        [DataMember]
        public string PrimaryKey { get; set; }
        [DataMember]
        public int Tenant { get; set; }
    }

    [DataContract(Name = "Response", Namespace = "http://schemas.datacontract.org/2004/07/Logitude.Server.Tools")]
    public class Response
    {
        [DataMember]
        public bool HasError { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string InnerErrorMessage { get; set; }
        [DataMember]
        public string Result { get; set; }
    }
}