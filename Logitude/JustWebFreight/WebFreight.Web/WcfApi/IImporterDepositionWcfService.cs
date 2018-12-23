using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System.ServiceModel;
using System.Threading.Tasks;

namespace WebFreight.Web.WcfApi
{
    [ServiceContract]
    public interface IImporterDepositionWcfService
    {
        [OperationContract]
        Task<Response>SendImporterDepositionToLogBox(ImporterDepositionPM importerDepositionPM);

    }








}
