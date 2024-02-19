using System.ServiceModel;
using WebFreight.Web.Controllers.WebServices.Services;

namespace WebFreight.Web.WcfApi
{
    [ServiceContract]
    public interface IShaamWcfService
    {
        [OperationContract]
        ApiToShaamRes CreateConfirmationNumber(string invoiceJson, int tenant, bool? testEnvironment = true);
    }
}
