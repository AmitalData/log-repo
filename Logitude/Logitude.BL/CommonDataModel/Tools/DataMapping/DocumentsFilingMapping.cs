using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Logitude.Customs.Def.Messaging.Customs;
using Logitude.BL.Resolvers;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class DocumentsFilingMapping
    {
        public static void MapEntity(DocumentsFilingPM entityPM, DocumentsFiling poco, bool isNewState)
        {



            ContactRepository contactRep = new ContactRepository(entityPM.Tenant);
            ContactPM loggedContact = LoggedContactResolver.GetLoggedContact(entityPM.Tenant); 
            DocumentTypeRepository documentTypeRepository = new DocumentTypeRepository(entityPM.Tenant);

            DocumentType docType = documentTypeRepository.GetSingleDocumentTypes(entityPM.DocumentTypeId, entityPM.Tenant);
            if (isNewState)
            {
                poco.Code = entityPM.Code;
                poco.Tenant = entityPM.Tenant;
                poco.CreateDate = entityPM.CreateDate;
                poco.SecurityId = entityPM.SecurityId;
				poco.IsFromCloud = entityPM.IsFromCloud;

			}
			if (string.IsNullOrEmpty(entityPM.ForwarderDocumentId))
            {
                poco.ComputedForwarderDocumentId = entityPM.Id;
            }
            else
            {
                poco.ComputedForwarderDocumentId = entityPM.ForwarderDocumentId;
            }
            poco.BillToId = entityPM.InvoiceBillTo;
            poco.Tenant = entityPM.Tenant;
            poco.DocumentId = entityPM.DocumentId;
            poco.DocumentTypeId = entityPM.DocumentTypeId;
            poco.DirectionCode = entityPM.DirectionCode;
            poco.EntityId = entityPM.EntityId;
            poco.ObjectTableId = entityPM.ObjectTableId;
            poco.ChildEntityId = entityPM.ChildEntityId;
            poco.ChildEntityReference = entityPM.ChildEntityReference;
            poco.ChildObjectTableId = entityPM.ChildObjectTableId;
            poco.Notes = entityPM.Notes;
            poco.CreatedByUserId = entityPM.CreatedByUserId;
            poco.OwnerId = entityPM.OwnerId;
            poco.Description = entityPM.Description;
            poco.HasCopies = entityPM.HasCopies;

            poco.FolderId = entityPM.FolderId;
            poco.IsDeleted = entityPM.IsDeleted;
            poco.DeletedByUserId = entityPM.DeletedByUserId;
            poco.DeleteDateTime = entityPM.DeleteDateTime;
            poco.IsSharedWithCustomer = entityPM.IsSharedWithCustomer;
            poco.IsSharedWithForwarder = entityPM.IsSharedWithForwarder;
            poco.SignRequestByUserEmail = entityPM.SignRequestByUserEmail;
            poco.CancellSignRequest = entityPM.CancellSignRequest;

            poco.ForwarderDocumentId = entityPM.ForwarderDocumentId;

            poco.LastVersion = entityPM.LastVersion;

            if (!entityPM.IsHybrid)
            {
                poco.CustomerDocumentId = entityPM.CustomerDocumentId;
                poco.CustomerTenantNumber = entityPM.CustomerTenantNumber;
            }
            poco.ComputedCustomerDocumentId = string.IsNullOrEmpty(poco.CustomerDocumentId) ? entityPM.Id : poco.CustomerDocumentId;

            poco.SearchFields = entityPM.Code + entityPM.Description + entityPM.Notes + entityPM.EntityReference + "," + entityPM.ChildEntityReference + "," + entityPM.ExternalEntityReference + "," + (docType != null ? docType.Code + "," + docType.Name + "," : ",");
            //if (!string.IsNullOrEmpty(entityPM.FileName))
            //{
            //    poco.SearchFields += entityPM.FileName + ",";
            //}
            //if (!string.IsNullOrEmpty(entityPM.DocumentId))
            //{
            //    poco.SearchFields += entityPM.DocumentId + ",";
            //}
            if (isNewState)
            {
                poco.SearchFields = poco.SearchFields + loggedContact.EnglishName + "," + loggedContact.LocalName;
            }
            //if (entityPM.IsLogBox)
            //{
            //    poco.ex
            //}
            foreach (DocumentsFilingMetaDataValuePM metadatavalue in entityPM.DocumentsFilingMetaDataValues)
            {
                if (!string.IsNullOrEmpty(metadatavalue.MetaDataValue))
                {
                    poco.SearchFields += "," + metadatavalue.MetaDataValue;
                }
            }

            if (entityPM.Received && !poco.Received)
            {
                poco.ReceivedDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }

            poco.Received = entityPM.Received;
            poco.ReceivedByUserId = entityPM.ReceivedByUserId;
            poco.ReceivedByByContactId = entityPM.ReceivedByByContactId;

            poco.ReceivedDate = entityPM.ReceivedDate;

            poco.StatusCode = entityPM.StatusCode;

            poco.EntityReference = entityPM.EntityReference;
            poco.ExternalEntityName = entityPM.ExternalEntityName;
            poco.ExternalEntityReference = entityPM.ExternalEntityReference;
            MapUpdatedByUserId(entityPM, poco, loggedContact);

            poco.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);


            poco.IsDigitallySigned = entityPM.IsDigitallySigned;
            poco.SignersList = entityPM.SignersList;

            if (entityPM.IsDeleted)
            {
                poco.IsRequested = false;
                poco.IsDigitalSignRequired = false;
            }
            else
            {
                poco.IsRequested = entityPM.IsRequested;
                poco.IsDigitalSignRequired = entityPM.IsDigitalSignRequired;
            }

            poco.SecurityId = entityPM.SecurityId;
            poco.LastVersion = entityPM.LastVersion;
            poco.OrigionalDocumentId = entityPM.OrigionalDocumentId;
            poco.IsSharedIn = entityPM.IsSharedIn;
            poco.IsSharedOut = entityPM.IsSharedOut;
            poco.LastShareDate = entityPM.LastShareDate;
            poco.SignDueDate = entityPM.SignDueDate;
            poco.EntityNumber = entityPM.EntityNumber;
            poco.IsTransferdToQBO = entityPM.IsTransferdToQBO;
            poco.ReceivedByPartner = entityPM.ReceivedByPartner;
			poco.IsFromCloud = entityPM.IsFromCloud;
            poco.FileDataMD5Hash = entityPM.FileDataMD5Hash;


        }

		private static void MapUpdatedByUserId(DocumentsFilingPM entityPM, DocumentsFiling poco, ContactPM loggedContact)
        {
            if (string.IsNullOrEmpty(entityPM.UpdatedByUserId))
            {
                return;
            }
            if (entityPM.IsHybrid)
            {
                poco.UpdatedByUserId = entityPM.UpdatedByUserId;
                return;
            }
            if (!string.IsNullOrEmpty(entityPM.UpdatedByUserId))
            {
                poco.UpdatedByUserId = entityPM.UpdatedByUserId;
                return;
            }
            if (loggedContact != null)
            {
                poco.UpdatedByUserId = loggedContact.Id;
                return;
            }

            var userId = "";
            var cntxt = RequestSheetContext.Current.GetContextOrDefault();
            if (cntxt != null)
            {
                userId = cntxt.GetUserFromRequestParam();
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                userId = entityPM.UpdatedByUserId;
            }

            poco.UpdatedByUserId = userId;
        }
    }
}
