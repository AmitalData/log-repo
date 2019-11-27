using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class DocumentsMetaDataTypeWcfCaller
    {
        public static Response PrepareDocumentsMetaDataType()
        {
            //if (HybridData.DocumentsMetaDataTypeId == null)
            //{
            //    return CallDocumentsMetaDataTypeUpsert();
            //}
            return new Response();
        }
        public static Response CallDocumentsMetaDataTypeUpsert()
        {
            DocumentsMetaDataTypePM entityPM = new DocumentsMetaDataTypePM()
            {
                //Code = HybridData.DocumentsMetaDataTypeCode,
                //Name = "Hybrid DocumentsMetaDataType",
                //DisplayName = "Hybrid DocumentsMetaDataType",
                //InActive = false,
                //ObjectTableName = "Shipment",
                //StatusWeight = 0,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "DocumentsMetaDataType",
                ServiceOperation = "Upsert",
                ServiceType = typeof(DocumentsMetaDataTypePM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            //if(!serviceResponse.HasError && serviceResponse.Result != null)
                //HybridData.DocumentsMetaDataTypeId = serviceResponse.Result;
            return serviceResponse;
        }
    }
}
