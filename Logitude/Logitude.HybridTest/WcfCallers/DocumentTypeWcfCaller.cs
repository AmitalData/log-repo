using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest.WcfCallers
{
    class DocumentTypeWcfCaller
    {
        public static Response CallDocumentTypeUpsert()
        {
            DocumentTypePM entityPM = new DocumentTypePM()
            {
                Code = HybridData.DocumentTypeCode,
                Name = "Hybrid DocumentType",
                ObjectTableName = "Shipment",
                DocumentTypeCategoryCode = "O",
                IsDocIn = true,
                IsDocOut = true,
                Tenant = TestEnvironmentGlobalParameters.Tenant,
            };

            InvokedProperties serviceProperties = new InvokedProperties
            {
                ServiceName = "DocumentType",
                ServiceOperation = "Upsert",
                ServiceType = typeof(DocumentTypePM),
                ServiceFilterType = null,
            };

            Response serviceResponse = new Response();
            object[] serviceParameters = new object[] { entityPM, false };
            WcfServiceInvoker.InvokeServiceMethod(serviceProperties, serviceParameters, ref serviceResponse);
            if(serviceResponse.Result != null)
                HybridData.DocumentTypeId = serviceResponse.Result;
            return serviceResponse;
        }
        public static Response PrepareDocumentType()
        {
            if(HybridData.DocumentTypeId == null)
            {
                return CallDocumentTypeUpsert();
            }
            return new Response();
        }
    }
}
