using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebFreight.Web.DataContracts;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFeatureWcfService" in both code and config file together.
    [ServiceContract]
    public interface IFeatureWcfService
    {
        [OperationContract]
        List<FeatureAccessInfo> GetActiveFeaturesForUser(List<FeatureAccessInfo> featuresList, int tenant, ref Response response);

        [OperationContract]
        bool CheckOutlookVersion(string Version);
    }


    
}
