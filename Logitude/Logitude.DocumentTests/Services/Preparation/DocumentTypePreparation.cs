using Logitude.DocumentTests.Models;
using Logitude.DocumentTests.Models.Codes;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DocumentTests.Services.Preparation
{
    public class DocumentTypePreparation
    {
        public void Prepare()
        {
            DocumentData.DocumentTypeAirManifestId = GetAirManifest();
            DocumentData.DocumentTypeCustomsId = GetCustoms();
            DocumentData.DocumentTypeGeneralMessageId = GetGeneralMessage();
        }


        private string GetAirManifest()
        {
            return GetIdByCode(DocumentTypeCodes.AirManifest) ?? Create(GetAirManifestInstance());
        }
        private string GetCustoms()
        {
            return GetIdByCode(DocumentTypeCodes.Customs) ?? Create(GetCustomsInstance());
        }
        private string GetGeneralMessage()
        {
            return GetIdByCode(DocumentTypeCodes.Customs) ?? Create(GetGeneralMessageInstance());
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

        private string Create(DocumentTypePM documentTypePM)
        {
            return APICaller.CallPost<DocumentTypePM>(documentTypePM, Urls.DocumentTypesController, UserTenant.Token).Data.Id;
        }

        private DocumentTypePM GetAirManifestInstance()
        {
            return new DocumentTypePM()
            {
                Tenant = UserTenant.Tenant,
                AddedManually = true,
                Code = DocumentTypeCodes.AirManifest,
                Name = "Air Manifest",
                IsDocIn = true,
                IsDocOut = true,
                DocumentTypeCategoryCode = DocumentTypeCategoryCodes.Others,
                ObjectTableId = DocumentData.ShipmentObjectTableId,
                TemplateFormatCode = TemplateFormatCodes.Print
            };
        }
        private DocumentTypePM GetCustomsInstance()
        {
            return new DocumentTypePM()
            {
                Tenant = UserTenant.Tenant,
                AddedManually = true,
                Code = DocumentTypeCodes.Customs,
                Name = "Customs",
                IsDocIn = true,
                IsDocOut = true,
                DocumentTypeCategoryCode = DocumentTypeCategoryCodes.Others,
                ObjectTableId = new ObjectTableService().GetIdByName("Shipment"),
                TemplateFormatCode = TemplateFormatCodes.Message
            };
        }
        private DocumentTypePM GetGeneralMessageInstance()
        {
            return new DocumentTypePM()
            {
                Tenant = UserTenant.Tenant,
                AddedManually = true,
                Code = DocumentTypeCodes.GeneralMessage,
                Name = "General Message",
                IsDocIn = true,
                IsDocOut = true,
                DocumentTypeCategoryCode = DocumentTypeCategoryCodes.Others,
                ObjectTableId = new ObjectTableService().GetIdByName("Shipment"),
                TemplateFormatCode = TemplateFormatCodes.Message
            };
        }

    }
}
