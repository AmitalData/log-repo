using System.Collections.Generic;
using System.ServiceModel;

namespace AmitalCloud.Infrastructure.APITools.Interface
{
    [ServiceContract]
    public interface ISignServiceUpdaterApplicationBlock
    {
        [OperationContract]
        void UpdaterApplicationBlock(List<string> signServerFeatures, string state, out string updaterApplicationBlockLink,
            out bool isMust,
            ref string queryStringMoreParams);
    }
}
