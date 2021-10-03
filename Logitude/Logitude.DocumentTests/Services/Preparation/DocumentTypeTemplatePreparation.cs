using Logitude.DocumentTests.Models;
using Logitude.DocumentTests.Models.Codes;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DocumentTests.Services.Preparation
{
    public class DocumentTypeTemplatePreparation
    {
        public void Prepare()
        {
            DocumentData.DocumentTypeAirManifestTemplateId = GetAirManifestTemplate();
           
        }


        private string GetAirManifestTemplate()
        {
            return GetIdByDocumentTypeId(DocumentData.DocumentTypeAirManifestId) ?? Create(GetAirManifestTemplateInstance());
        }
        

        private string GetIdByDocumentTypeId(string documentTypeAirManifestId)
        {
            return APICaller.CallGet<List<DocumentTypeList>>(Urls.GetDocumentTypeTemplate(documentTypeAirManifestId, UserTenant.Tenant), UserTenant.Token)?.Data?.FirstOrDefault()?.Id;
        }

        private string Create(DocumentTypeTemplatePM documentTypeTemplatePM)
        {
            return APICaller.CallPost<DocumentTypeTemplatePM>(documentTypeTemplatePM, Urls.DocumentTypeTemplatesController, UserTenant.Token).Data.Id;
        }

        private DocumentTypeTemplatePM GetAirManifestTemplateInstance()
        {
            return new DocumentTypeTemplatePM()
            {
                Tenant = UserTenant.Tenant,
                DocumentTypeCode = DocumentTypeTemplateCodes.AirManifest,
                DocumentTypeId = DocumentData.DocumentTypeAirManifestId,
                ObjectTableId = DocumentData.ShipmentObjectTableId,
                Description = "Description",
                TemplateType = TemplateFormatCodes.Message
            };
        }

    }
}
