using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DashboardModule.BL.DataProviders
{
    public class DataProviderFactory
    {
        const string ShipmentAnalyticsMetaData = "ShipmentAnalytics";
        public IDataProviderService GetDataProviderService(string metaDataName)
        {
            switch (metaDataName)
            {
                case ShipmentAnalyticsMetaData:
                    return new ShipmentDataProviderService();
                default:
                    throw new Exception($"Meta Data Name {metaDataName} not found");
            }
            
        }
    }
}
