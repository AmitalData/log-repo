using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookConnection.Common.Contracts
{
    public interface IFeatureRepo
    {
        FeatureWcfServiceReference.FeatureAccessInfo[] GetActiveFeaturesForUser(FeatureWcfServiceReference.FeatureAccessInfo[] featuresList, int tenant, ref FeatureWcfServiceReference.Response response);
        Task<FeatureWcfServiceReference.GetActiveFeaturesForUserResponse> GetActiveFeaturesForUserAsync(FeatureWcfServiceReference.GetActiveFeaturesForUserRequest request);

    }
}
