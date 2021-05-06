using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutlookConnection.Common.DocumentTypeWcfServiceReference;

namespace OutlookConnection.Common.Contracts
{
    interface IDocumentTypeRepo
    {
        DocumentTypeWcfServiceReference.Response Upsert(DocumentTypePM entityPM, bool batch);

        OutlookConnection.Common.DocumentTypeWcfServiceReference.DocumentTypeList[] GetDocumentTypes(string objectTableName, int tenant, int skip, int take, ref Response response);


    }
}
