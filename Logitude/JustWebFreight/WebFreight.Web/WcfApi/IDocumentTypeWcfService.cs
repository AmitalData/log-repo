using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDocumentTypeWcfService" in both code and config file together.
    [ServiceContract]
    public interface IDocumentTypeWcfService
    {
        [OperationContract]
        List<DocumentTypeList> GetDocumentTypes(string objectTableName, int tenant, int skip, int take, ref Response response);
        [OperationContract]
        Response Upsert(DocumentTypePM entityPM, bool batch);

        [OperationContract]
        DocumentTypePM GetDocumentTypeByCode(string code, int tenant, ref Response response);
    }
}
