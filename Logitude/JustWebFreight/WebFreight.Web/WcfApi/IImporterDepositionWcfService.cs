using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System.ServiceModel;


namespace WebFreight.Web.WcfApi
{
    [ServiceContract]
    public interface IImporterDepositionWcfService
    {
        [OperationContract]
        void SendImporterDepositionToLogBox(int tenant, ref Response response);

    }








}
