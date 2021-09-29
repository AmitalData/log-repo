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
            return GetIdByCode(DocumentTypeTemplateCodes.AirManifest) ?? Create(GetAirManifestTemplateInstance());
        }
        

        private string GetIdByCode(string code)
        {
            var filter = new ApiQueryFilters() { 
                GetAll = true,
                Filter1Name = "Code",
                Filter1Value = code
            };
            return APICaller.CallGetByFilters<List<DocumentTypeList>>(Urls.DocumentTypeViewsGetByFilters, UserTenant.Token, filter)?.Data?.FirstOrDefault()?.Id;
        }

        private string Create(DocumentTypeTemplatePM documentTypeTemplatePM)
        {
            return APICaller.CallPost<DocumentTypeTemplatePM>(documentTypeTemplatePM, Urls.DocumentTypesController, UserTenant.Token).Data.Id;
        }

        private DocumentTypeTemplatePM GetAirManifestTemplateInstance()
        {
            return new DocumentTypeTemplatePM()
            {
                Tenant = UserTenant.Tenant,
                DocumentTypeCode = DocumentTypeTemplateCodes.AirManifest,
                DocumentTypeId = DocumentData.DocumentTypeAirManifestId,
                ObjectTableId = DocumentData.ShipmentObjectTableId,
            };
        }

    }
}
