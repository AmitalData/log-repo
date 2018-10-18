using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.WcfServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IXapFilesDatabaseWcfService" in both code and config file together.
    [ServiceContract]
    public interface IXapFilesDatabaseWcfService
    {
        [OperationContract]
        void UpsertXapFile(string fileName, byte[] fileData, bool isStaging);
        [OperationContract]
        XapFileInfo GetXapFile(string fileName, bool isStaging);
    }
}
