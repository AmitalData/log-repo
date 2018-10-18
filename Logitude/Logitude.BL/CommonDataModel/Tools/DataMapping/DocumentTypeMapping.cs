using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class DocumentTypeMapping
    {
        public static void MapEntity(DocumentTypePM documentTypePM, DocumentType documentType, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(documentTypePM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), documentTypePM.Tenant);

            if (isNewState)
            {
                documentType.Tenant = documentTypePM.Tenant;
            }
            documentType.IsAir = documentTypePM.IsAir;
            documentType.IsDocIn = documentTypePM.IsDocIn;
            documentType.IsDocOut = documentTypePM.IsDocOut;
            documentType.IsInland = documentTypePM.IsInland;
            documentType.IsOcean = documentTypePM.IsOcean;
            documentType.Name = documentTypePM.Name;
            documentType.Notes = documentTypePM.Notes;
            documentType.Tenant = documentTypePM.Tenant;
            documentType.Code = documentTypePM.Code;
            documentType.ObjectTableId = documentTypePM.ObjectTableId;
            documentType.DocumentTypeDefaultReportTemplateId = documentTypePM.DocumentTypeDefaultReportTemplateId;
            documentType.DocumentTypeDefaultHTMLTemplateId = documentTypePM.DocumentTypeDefaultHTMLTemplateId;
            documentType.TemplateFormatCode = documentTypePM.TemplateFormatCode;
            documentType.Subject = documentTypePM.Subject;
            documentType.DocumentTypeDefaultEditorTool = documentTypePM.DocumentTypeDefaultEditorTool;
            documentType.InActive = documentTypePM.InActive;
            documentType.IsMaster = documentTypePM.IsMaster;
            documentType.IsHouse = documentTypePM.IsHouse;
            documentType.IsDirect = documentTypePM.IsDirect;
            documentType.CustomControl = documentTypePM.CustomControl;
            documentType.SearchFields = documentTypePM.Code + "," + documentTypePM.Name;
            documentType.AgentRoleId = documentTypePM.AgentRoleId;
            documentType.CustomerRoleId = documentTypePM.CustomerRoleId;
            documentType.IsAgentView = documentTypePM.IsAgentView;
            documentType.IsCustomerView = documentTypePM.IsCustomerView;
            documentType.IsReadOnly = documentTypePM.IsReadOnly;
            documentType.IsDocumentOneTimePrintLimited = documentTypePM.IsDocumentOneTimePrintLimited;
            documentType.LimitedPrintCopyId = documentTypePM.LimitedPrintCopyId;

            documentType.CountryCode = documentTypePM.CountryCode;
            documentType.IsCopiedAtSignup = documentTypePM.IsCopiedAtSignup;
            documentType.IsEnabledForCustomers = documentTypePM.IsEnabledForCustomers;
            documentType.DocumentTypeCategoryCode = documentTypePM.DocumentTypeCategoryCode;
            documentType.OrderBy = documentTypePM.OrderBy;
            documentType.FileName = documentTypePM.FileName;

            documentType.IsAgentSharedInDirect = documentTypePM.IsAgentSharedInDirect;
            documentType.IsAgentSharedInHouse = documentTypePM.IsAgentSharedInHouse;
            documentType.IsAgentSharedInMaster = documentTypePM.IsAgentSharedInMaster;
            documentType.SharedDocumentTypeCopyId = documentTypePM.SharedDocumentTypeCopyId;
            documentType.IsAirDigitalSignRequired = documentTypePM.IsAirDigitalSignRequired;
            documentType.IsOceanDigitalSignRequired = documentTypePM.IsOceanDigitalSignRequired;
            documentType.IsInlandDigitalSignRequired = documentTypePM.IsInlandDigitalSignRequired;
            documentType.IsSystemAdditionalPrintingFields = documentTypePM.IsSystemAdditionalPrintingFields;
            documentType.PrintingFieldsScreenCode = documentTypePM.PrintingFieldsScreenCode;
        }
    }
}
