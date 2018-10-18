using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDocumentsMetaDataTypeWcfService" in both code and config file together.
    [ServiceContract]
    public interface IDocumentsMetaDataTypeWcfService
    {
        [OperationContract]
        Response Upsert(DocumentsMetaDataTypePM entityPM, bool batch);
    }
}
