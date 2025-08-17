using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.Customs.Data;
using Simplog.Global.Data.GlobalModel;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Data.Entity.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using System.Text.RegularExpressions;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.Args;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentsFilingQuery
    {
        DocumentsFilingRepository repository;



        public DocumentsFilingQuery(int tenant)
        {
            repository = new DocumentsFilingRepository(tenant);
        }

        public DocumentsFilingQuery(DocumentsFilingRepository repository)
        {
            this.repository = repository;
        }

        public DocumentsFilingPM GetDocumentsFilingByDocumentId(string docId, int tenant)
        {
            DocumentsFilingPM extDocPm = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                          where a.DocumentId == docId && a.Tenant == tenant
                                          select new DocumentsFilingPM()
                                          {
                                              Id = a.Id,
                                              DocumentId = a.DocumentId,
                                              Code = a.Code,
                                              DirectionCode = a.DirectionCode,
                                              Description = a.Description,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreateDate = a.CreateDate,
                                              ObjectTableId = a.ObjectTableId,
                                              ChildEntityId = a.ChildEntityId,
                                              ChildObjectTableId = a.ChildObjectTableId,
                                              ChildEntityReference = a.ChildEntityReference,
                                              DocumentTypeId = a.DocumentTypeId,
                                              EntityId = a.EntityId,
                                              HasCopies = a.HasCopies,
                                              Notes = a.Notes,
                                              OwnerId = a.OwnerId,
                                              SearchFields = a.SearchFields,
                                              Tenant = a.Tenant,
                                              FileExtension = a.Document != null ? a.Document.Extension : null,
                                              HasFile = a.Document != null ? a.Document.HasFile : false,
                                              FileName = a.Document != null ? a.Document.FileName : null,
                                              FileSize = a.Document != null ? a.Document.FileSize : null,
                                              Folder = a.Document != null ? a.Document.Folder : null,
                                              DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                              IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                              IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                              CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                              Received = a.Received,
                                              ReceivedDate = a.ReceivedDate,
                                              ReceivedByUserId = a.ReceivedByUserId,
                                              ReceivedByByContactId = a.ReceivedByByContactId,
                                              Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                              StatusCode = a.StatusCode,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DepartmentId = a.DepartmentId,
                                              BranchId = a.BranchId,
                                              FolderId = a.FolderId,
                                              IsDeleted = a.IsDeleted,
                                              DeleteDateTime = a.DeleteDateTime,
                                              DeletedByUserId = a.DeletedByUserId,
                                              IsDigitallySigned = a.IsDigitallySigned,
                                              SignersList = a.SignersList,
                                              IsSharedWithCustomer = a.IsSharedWithCustomer,
                                              IsSharedWithForwarder = a.IsSharedWithForwarder,
                                              ForwarderDocumentId = a.ForwarderDocumentId,
                                              CustomerDocumentId = a.CustomerDocumentId,
                                              SecurityId = a.SecurityId,

                                              LastVersion = a.LastVersion,
                                              CustomerTenantNumber = a.CustomerTenantNumber,
                                              IsRequested = a.IsRequested,
                                              ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                              SignRequestByUserEmail = a.SignRequestByUserEmail,
                                              CancellSignRequest = a.CancellSignRequest,
                                              OrigionalDocumentId = a.OrigionalDocumentId,
                                              LastShareDate = a.LastShareDate,
                                              IsSharedIn = a.IsSharedIn,
                                              IsSharedOut = a.IsSharedOut,
                                              SignDueDate = a.SignDueDate,
                                              IsDigitalSignRequired = a.IsDigitalSignRequired,
                                              BackedupExternally = a.BackedupExternally,
                                              IsTransferdToQBO =  a.IsTransferdToQBO,

                                              IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                              IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                              IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                              ReceivedByPartner = a.ReceivedByPartner,
											  IsFromCloud = a.IsFromCloud,

										  }).FirstOrDefault();


            if (extDocPm != null)
            {
                SetDocumentFollowUp(extDocPm);
                DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            }
            return extDocPm;
        }

        private static void SetDocumentFollowUp(DocumentsFilingPM document, string followUpId = null)
        {
            if (followUpId == null) followUpId = new FollowUpRepository(document.Tenant).GetFollowUpIdByDocumentsFilingId(document.Tenant, document.Id);
            if (string.IsNullOrEmpty(followUpId)) return;

            document.FollowUpCount = 1;
            document.HasFollowUp = true;
            document.FollowUpId = followUpId;
        }

        public DocumentsFilingPM GetSinglePMByCode(string code, int tenant)
        {
            DocumentsFilingPM extDocPm = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                          where a.Code == code && a.Tenant == tenant
                                          select new DocumentsFilingPM()
                                          {
                                              Id = a.Id,
                                              DocumentId = a.DocumentId,
                                              Code = a.Code,
                                              DirectionCode = a.DirectionCode,
                                              Description = a.Description,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreateDate = a.CreateDate,
                                              ObjectTableId = a.ObjectTableId,
                                              ChildEntityId = a.ChildEntityId,
                                              ChildObjectTableId = a.ChildObjectTableId,
                                              ChildEntityReference = a.ChildEntityReference,
                                              DocumentTypeId = a.DocumentTypeId,
                                              EntityId = a.EntityId,
                                              HasCopies = a.HasCopies,
                                              Notes = a.Notes,
                                              OwnerId = a.OwnerId,
                                              SearchFields = a.SearchFields,
                                              Tenant = a.Tenant,
                                              FileExtension = a.Document != null ? a.Document.Extension : null,
                                              HasFile = a.Document != null ? a.Document.HasFile : false,
                                              FileName = a.Document != null ? a.Document.FileName : null,
                                              FileSize = a.Document != null ? a.Document.FileSize : null,
                                              Folder = a.Document != null ? a.Document.Folder : null,
                                              DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                              IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                              IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                              CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                              Received = a.Received,
                                              ReceivedDate = a.ReceivedDate,
                                              ReceivedByUserId = a.ReceivedByUserId,
                                              ReceivedByByContactId = a.ReceivedByByContactId,
                                              Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                              StatusCode = a.StatusCode,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              EntityReference = a.EntityReference,
                                              ExternalEntityName = a.ExternalEntityName,
                                              ExternalEntityReference = a.ExternalEntityReference,
                                              DepartmentId = a.DepartmentId,
                                              BranchId = a.BranchId,
                                              FolderId = a.FolderId,
                                              IsDeleted = a.IsDeleted,
                                              DeleteDateTime = a.DeleteDateTime,
                                              DeletedByUserId = a.DeletedByUserId,
                                              IsDigitallySigned = a.IsDigitallySigned,
                                              SignersList = a.SignersList,
                                              IsSharedWithCustomer = a.IsSharedWithCustomer,
                                              IsSharedWithForwarder = a.IsSharedWithForwarder,
                                              CustomerDocumentId = a.CustomerDocumentId,
                                              ForwarderDocumentId = a.ForwarderDocumentId,
                                              CustomerTenantNumber = a.CustomerTenantNumber,
                                              LastVersion = a.LastVersion,

                                              IsRequested = a.IsRequested,
                                              ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                              SignRequestByUserEmail = a.SignRequestByUserEmail,
                                              CancellSignRequest = a.CancellSignRequest,
                                              OrigionalDocumentId = a.OrigionalDocumentId,
                                              LastShareDate = a.LastShareDate,
                                              IsSharedIn = a.IsSharedIn,
                                              IsSharedOut = a.IsSharedOut,
                                              SignDueDate = a.SignDueDate,
                                              IsDigitalSignRequired = a.IsDigitalSignRequired,
                                              BackedupExternally = a.BackedupExternally,
                                              IsTransferdToQBO = a.IsTransferdToQBO,

                                              IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                              IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                              IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                              ReceivedByPartner = a.ReceivedByPartner,
											  IsFromCloud = a.IsFromCloud,

										  }).FirstOrDefault();

            if (extDocPm != null)
            {
                SetDocumentFollowUp(extDocPm);
                DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            }

            return extDocPm;
        }

        public DocumentsFilingPM GetDocumentsFilingByDocumentCode(string code, int tenant)
        {
            DocumentsFilingPM extDocPm = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                          where a.Code == code && a.Tenant == tenant
                                          select new DocumentsFilingPM()
                                          {
                                              Id = a.Id,
                                              DocumentId = a.DocumentId,
                                              Code = a.Code,
                                              DirectionCode = a.DirectionCode,
                                              Description = a.Description,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreateDate = a.CreateDate,
                                              ObjectTableId = a.ObjectTableId,
                                              ChildEntityId = a.ChildEntityId,
                                              ChildObjectTableId = a.ChildObjectTableId,
                                              ChildEntityReference = a.ChildEntityReference,
                                              DocumentTypeId = a.DocumentTypeId,
                                              EntityId = a.EntityId,
                                              HasCopies = a.HasCopies,
                                              Notes = a.Notes,
                                              OwnerId = a.OwnerId,
                                              SearchFields = a.SearchFields,
                                              Tenant = a.Tenant,
                                              FileExtension = a.Document != null ? a.Document.Extension : null,
                                              HasFile = a.Document != null ? a.Document.HasFile : false,
                                              FileName = a.Document != null ? a.Document.FileName : null,
                                              FileSize = a.Document != null ? a.Document.FileSize : null,
                                              Folder = a.Document != null ? a.Document.Folder : null,
                                              DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                              IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                              IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                              CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                              Received = a.Received,
                                              ReceivedDate = a.ReceivedDate,
                                              ReceivedByUserId = a.ReceivedByUserId,
                                              ReceivedByByContactId = a.ReceivedByByContactId,
                                              Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                              StatusCode = a.StatusCode,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              EntityReference = a.EntityReference,
                                              ExternalEntityName = a.ExternalEntityName,
                                              ExternalEntityReference = a.ExternalEntityReference,
                                              DepartmentId = a.DepartmentId,
                                              BranchId = a.BranchId,
                                              FolderId = a.FolderId,
                                              IsDeleted = a.IsDeleted,
                                              DeleteDateTime = a.DeleteDateTime,
                                              DeletedByUserId = a.DeletedByUserId,
                                              IsDigitallySigned = a.IsDigitallySigned,
                                              SignersList = a.SignersList,
                                              IsSharedWithCustomer = a.IsSharedWithCustomer,
                                              IsSharedWithForwarder = a.IsSharedWithForwarder,
                                              CustomerDocumentId = a.CustomerDocumentId,
                                              ForwarderDocumentId = a.ForwarderDocumentId,
                                              CustomerTenantNumber = a.CustomerTenantNumber,
                                              LastVersion = a.LastVersion,

                                              IsRequested = a.IsRequested,
                                              ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                              SignRequestByUserEmail = a.SignRequestByUserEmail,
                                              CancellSignRequest = a.CancellSignRequest,
                                              OrigionalDocumentId = a.OrigionalDocumentId,
                                              LastShareDate = a.LastShareDate,
                                              IsSharedIn = a.IsSharedIn,
                                              IsSharedOut = a.IsSharedOut,
                                              SignDueDate = a.SignDueDate,
                                              IsDigitalSignRequired = a.IsDigitalSignRequired,
                                              BackedupExternally = a.BackedupExternally,
                                              SecurityId = a.SecurityId,
                                              IsTransferdToQBO = a.IsTransferdToQBO,

                                              IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                              IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                              IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                              ReceivedByPartner = a.ReceivedByPartner,
											  IsFromCloud = a.IsFromCloud,

										  }).FirstOrDefault();

            if (extDocPm != null)
            {
                SetDocumentFollowUp(extDocPm);
                DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            }

            return extDocPm;
        }

        public DocumentsFilingPM GetSinglePM(string id, int tenant, bool checkOcr = false)
        {
            if (!string.IsNullOrEmpty(id))
            { 
                id = Regex.Replace(id, " ", "+");
            }
              DocumentsFilingPM extDocPm = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                          where a.Id == id && a.Tenant == tenant
                                          select new DocumentsFilingPM()
                                          {
                                              Id = a.Id,
                                              DocumentId = a.DocumentId,
                                              Code = a.Code,
                                              DirectionCode = a.DirectionCode,
                                              Description = a.Description,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreateDate = a.CreateDate,
                                              ObjectTableId = a.ObjectTableId,
                                              ChildEntityId = a.ChildEntityId,
                                              ChildObjectTableId = a.ChildObjectTableId,
                                              ChildEntityReference = a.ChildEntityReference,
                                              DocumentTypeId = a.DocumentTypeId,
                                              EntityId = a.EntityId,
                                              HasCopies = a.HasCopies,
                                              Notes = a.Notes,
                                              OwnerId = a.OwnerId,
                                              SearchFields = a.SearchFields,
                                              Tenant = a.Tenant,
                                              FileExtension = a.Document != null ? a.Document.Extension : null,
                                              HasFile = a.Document != null ? a.Document.HasFile : false,
                                              FileName = a.Document != null ? a.Document.FileName : null,
                                              FileSize = a.Document != null ? a.Document.FileSize : null,
                                              Folder = a.Document != null ? a.Document.Folder : null,
                                              DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                              IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                              IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                              CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                              Received = a.Received,
                                              ReceivedDate = a.ReceivedDate,
                                              ReceivedByUserId = a.ReceivedByUserId,
                                              ReceivedByByContactId = a.ReceivedByByContactId,
                                              Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                              StatusCode = a.StatusCode,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              EntityReference = a.EntityReference,
                                              ExternalEntityName = a.ExternalEntityName,
                                              ExternalEntityReference = a.ExternalEntityReference,
                                              DepartmentId = a.DepartmentId,
                                              BranchId = a.BranchId,
                                              FolderId = a.FolderId,
                                              IsDeleted = a.IsDeleted,
                                              DeleteDateTime = a.DeleteDateTime,
                                              DeletedByUserId = a.DeletedByUserId,
                                              IsDigitallySigned = a.IsDigitallySigned,
                                              SignersList = a.SignersList,
                                              IsSharedWithCustomer = a.IsSharedWithCustomer,
                                              IsSharedWithForwarder = a.IsSharedWithForwarder,
                                              CustomerDocumentId = a.CustomerDocumentId,
                                              ForwarderDocumentId = a.ForwarderDocumentId,
                                              CustomerTenantNumber = a.CustomerTenantNumber,
                                              SecurityId = a.SecurityId,
                                              LastVersion = a.LastVersion,
                                              IsRequested = a.IsRequested,
                                              ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                              SignRequestByUserEmail = a.SignRequestByUserEmail,
                                              CancellSignRequest = a.CancellSignRequest,
                                              OrigionalDocumentId = a.OrigionalDocumentId,
                                              LastShareDate = a.LastShareDate,
                                              IsSharedIn = a.IsSharedIn,
                                              IsSharedOut = a.IsSharedOut,
                                              SignDueDate = a.SignDueDate,
                                              IsDigitalSignRequired = a.IsDigitalSignRequired,
                                              BackedupExternally = a.BackedupExternally,
                                              IsTransferdToQBO = a.IsTransferdToQBO,

                                              IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                              IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                              IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                              ReceivedByPartner = a.ReceivedByPartner,
											  IsFromCloud = a.IsFromCloud,

										  }).FirstOrDefault();

            if (extDocPm != null)
            {
                FollowUpRepository followUpRepository = new FollowUpRepository(tenant);
                List<FollowUp> FollowUps = followUpRepository.GetFollowUps(tenant).ToList();
                if (FollowUps != null)
                {
                    List<FollowUp> docFollowUp = FollowUps.Where(d => d.DocumentsFilingId == extDocPm.Id && d.Tenant == extDocPm.Tenant).ToList();
                    if (docFollowUp.Count != 0)
                    {
                        extDocPm.FollowUpCount = docFollowUp.Count;
                        extDocPm.FollowUpId = docFollowUp.FirstOrDefault().Id;
                        extDocPm.HasFollowUp = docFollowUp.Any();
                    }
                }
                if (checkOcr)
                {
                    OcrDocumentRepository ocrDocumentRepository = new OcrDocumentRepository(tenant);
                    extDocPm.OcrReference = ocrDocumentRepository.GetSingleByDocId(id, tenant)?.Reference;
                }

                SetDocumentFollowUp(extDocPm);
                DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            }
            return extDocPm;
        }

        public DocumentsFilingPM GetSinglePMByForwarderId(string id, int tenant)
        {
            DocumentsFilingPM extDocPm = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                          where a.ForwarderDocumentId == id && a.IsDeleted == false && a.Tenant == tenant
                                          select new DocumentsFilingPM()
                                          {
                                              Id = a.Id,
                                              DocumentId = a.DocumentId,
                                              Code = a.Code,
                                              DirectionCode = a.DirectionCode,
                                              Description = a.Description,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreateDate = a.CreateDate,
                                              ObjectTableId = a.ObjectTableId,
                                              ChildEntityId = a.ChildEntityId,
                                              ChildObjectTableId = a.ChildObjectTableId,
                                              ChildEntityReference = a.ChildEntityReference,
                                              DocumentTypeId = a.DocumentTypeId,
                                              EntityId = a.EntityId,
                                              HasCopies = a.HasCopies,
                                              Notes = a.Notes,
                                              OwnerId = a.OwnerId,
                                              SearchFields = a.SearchFields,
                                              Tenant = a.Tenant,
                                              FileExtension = a.Document != null ? a.Document.Extension : null,
                                              HasFile = a.Document != null ? a.Document.HasFile : false,
                                              FileName = a.Document != null ? a.Document.FileName : null,
                                              FileSize = a.Document != null ? a.Document.FileSize : null,
                                              Folder = a.Document != null ? a.Document.Folder : null,
                                              DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                              IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                              IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                              CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                              Received = a.Received,
                                              ReceivedDate = a.ReceivedDate,
                                              ReceivedByUserId = a.ReceivedByUserId,
                                              ReceivedByByContactId = a.ReceivedByByContactId,
                                              Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                              StatusCode = a.StatusCode,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              EntityReference = a.EntityReference,
                                              ExternalEntityName = a.ExternalEntityName,
                                              ExternalEntityReference = a.ExternalEntityReference,
                                              DepartmentId = a.DepartmentId,
                                              BranchId = a.BranchId,
                                              FolderId = a.FolderId,
                                              IsDeleted = a.IsDeleted,
                                              DeleteDateTime = a.DeleteDateTime,
                                              DeletedByUserId = a.DeletedByUserId,
                                              IsDigitallySigned = a.IsDigitallySigned,
                                              SignersList = a.SignersList,
                                              IsSharedWithCustomer = a.IsSharedWithCustomer,
                                              IsSharedWithForwarder = a.IsSharedWithForwarder,
                                              CustomerDocumentId = a.CustomerDocumentId,
                                              ForwarderDocumentId = a.ForwarderDocumentId,
                                              CustomerTenantNumber = a.CustomerTenantNumber,
                                              SecurityId = a.SecurityId,
                                              LastVersion = a.LastVersion,
                                              IsRequested = a.IsRequested,
                                              ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                              SignRequestByUserEmail = a.SignRequestByUserEmail,
                                              CancellSignRequest = a.CancellSignRequest,
                                              OrigionalDocumentId = a.OrigionalDocumentId,
                                              LastShareDate = a.LastShareDate,
                                              IsSharedIn = a.IsSharedIn,
                                              IsSharedOut = a.IsSharedOut,
                                              SignDueDate = a.SignDueDate,
                                              IsDigitalSignRequired = a.IsDigitalSignRequired,
                                              IsTransferdToQBO = a.IsTransferdToQBO,
                                              BackedupExternally = a.BackedupExternally,

                                              IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                              IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                              IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                              ReceivedByPartner = a.ReceivedByPartner,
											  IsFromCloud = a.IsFromCloud,

										  }).FirstOrDefault();

            if (extDocPm != null)
            {
                SetDocumentFollowUp(extDocPm);
                DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            }
            return extDocPm;
        }

        public DocumentsFilingPM GetSinglePMByCustomerId(string id, int tenant)
        {
            DocumentsFilingPM extDocPm = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                          where a.CustomerDocumentId == id && a.IsDeleted == false && a.Tenant == tenant
                                          select new DocumentsFilingPM()
                                          {
                                              Id = a.Id,
                                              DocumentId = a.DocumentId,
                                              Code = a.Code,
                                              DirectionCode = a.DirectionCode,
                                              Description = a.Description,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreateDate = a.CreateDate,
                                              ObjectTableId = a.ObjectTableId,
                                              ChildEntityId = a.ChildEntityId,
                                              ChildObjectTableId = a.ChildObjectTableId,
                                              ChildEntityReference = a.ChildEntityReference,
                                              DocumentTypeId = a.DocumentTypeId,
                                              EntityId = a.EntityId,
                                              HasCopies = a.HasCopies,
                                              Notes = a.Notes,
                                              OwnerId = a.OwnerId,
                                              SearchFields = a.SearchFields,
                                              Tenant = a.Tenant,
                                              FileExtension = a.Document != null ? a.Document.Extension : null,
                                              HasFile = a.Document != null ? a.Document.HasFile : false,
                                              FileName = a.Document != null ? a.Document.FileName : null,
                                              FileSize = a.Document != null ? a.Document.FileSize : null,
                                              Folder = a.Document != null ? a.Document.Folder : null,
                                              DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                              IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                              IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                              CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                              Received = a.Received,
                                              ReceivedDate = a.ReceivedDate,
                                              ReceivedByUserId = a.ReceivedByUserId,
                                              ReceivedByByContactId = a.ReceivedByByContactId,
                                              Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                              StatusCode = a.StatusCode,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              EntityReference = a.EntityReference,
                                              ExternalEntityName = a.ExternalEntityName,
                                              ExternalEntityReference = a.ExternalEntityReference,
                                              DepartmentId = a.DepartmentId,
                                              BranchId = a.BranchId,
                                              FolderId = a.FolderId,
                                              IsDeleted = a.IsDeleted,
                                              DeleteDateTime = a.DeleteDateTime,
                                              DeletedByUserId = a.DeletedByUserId,
                                              IsDigitallySigned = a.IsDigitallySigned,
                                              SignersList = a.SignersList,
                                              IsSharedWithCustomer = a.IsSharedWithCustomer,
                                              IsSharedWithForwarder = a.IsSharedWithForwarder,
                                              CustomerDocumentId = a.CustomerDocumentId,
                                              ForwarderDocumentId = a.ForwarderDocumentId,
                                              CustomerTenantNumber = a.CustomerTenantNumber,
                                              SecurityId = a.SecurityId,
                                              LastVersion = a.LastVersion,
                                              IsRequested = a.IsRequested,
                                              ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                              SignRequestByUserEmail = a.SignRequestByUserEmail,
                                              CancellSignRequest = a.CancellSignRequest,
                                              OrigionalDocumentId = a.OrigionalDocumentId,
                                              LastShareDate = a.LastShareDate,
                                              IsSharedIn = a.IsSharedIn,
                                              IsSharedOut = a.IsSharedOut,
                                              SignDueDate = a.SignDueDate,
                                              IsDigitalSignRequired = a.IsDigitalSignRequired,
                                              BackedupExternally = a.BackedupExternally,
                                              IsTransferdToQBO = a.IsTransferdToQBO,

                                              IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                              IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                              IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                              ReceivedByPartner = a.ReceivedByPartner,
											  IsFromCloud = a.IsFromCloud,

										  }).FirstOrDefault();

            if (extDocPm != null)
            {
                SetDocumentFollowUp(extDocPm);
                DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            }
            return extDocPm;
        }

        public DocumentsFilingPM GetSinglePMBySecurityId(string securityId, int tenant)
        {
            DocumentsFilingPM extDocPm = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                          where a.SecurityId == securityId && a.Tenant == tenant
                                          select new DocumentsFilingPM()
                                          {
                                              Id = a.Id,
                                              DocumentId = a.DocumentId,
                                              Code = a.Code,
                                              DirectionCode = a.DirectionCode,
                                              Description = a.Description,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreateDate = a.CreateDate,
                                              ObjectTableId = a.ObjectTableId,
                                              ChildEntityId = a.ChildEntityId,
                                              ChildObjectTableId = a.ChildObjectTableId,
                                              ChildEntityReference = a.ChildEntityReference,
                                              DocumentTypeId = a.DocumentTypeId,
                                              EntityId = a.EntityId,
                                              HasCopies = a.HasCopies,
                                              Notes = a.Notes,
                                              OwnerId = a.OwnerId,
                                              SearchFields = a.SearchFields,
                                              Tenant = a.Tenant,
                                              FileExtension = a.Document != null ? a.Document.Extension : null,
                                              HasFile = a.Document != null ? a.Document.HasFile : false,
                                              FileName = a.Document != null ? a.Document.FileName : null,
                                              FileSize = a.Document != null ? a.Document.FileSize : null,
                                              Folder = a.Document != null ? a.Document.Folder : null,
                                              DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                              IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                              IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                              CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                              Received = a.Received,
                                              ReceivedDate = a.ReceivedDate,
                                              ReceivedByUserId = a.ReceivedByUserId,
                                              ReceivedByByContactId = a.ReceivedByByContactId,
                                              Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                              StatusCode = a.StatusCode,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              EntityReference = a.EntityReference,
                                              ExternalEntityName = a.ExternalEntityName,
                                              ExternalEntityReference = a.ExternalEntityReference,
                                              DepartmentId = a.DepartmentId,
                                              BranchId = a.BranchId,
                                              FolderId = a.FolderId,
                                              IsDeleted = a.IsDeleted,
                                              DeleteDateTime = a.DeleteDateTime,
                                              DeletedByUserId = a.DeletedByUserId,
                                              IsDigitallySigned = a.IsDigitallySigned,
                                              SignersList = a.SignersList,
                                              IsSharedWithCustomer = a.IsSharedWithCustomer,
                                              IsSharedWithForwarder = a.IsSharedWithForwarder,
                                              CustomerDocumentId = a.CustomerDocumentId,
                                              ForwarderDocumentId = a.ForwarderDocumentId,
                                              CustomerTenantNumber = a.CustomerTenantNumber,
                                              SecurityId = a.SecurityId,
                                              LastVersion = a.LastVersion,
                                              IsRequested = a.IsRequested,
                                              ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                              SignRequestByUserEmail = a.SignRequestByUserEmail,
                                              CancellSignRequest = a.CancellSignRequest,
                                              OrigionalDocumentId = a.OrigionalDocumentId,
                                              LastShareDate = a.LastShareDate,
                                              IsSharedIn = a.IsSharedIn,
                                              IsSharedOut = a.IsSharedOut,
                                              SignDueDate = a.SignDueDate,
                                              IsDigitalSignRequired = a.IsDigitalSignRequired,
                                              BackedupExternally = a.BackedupExternally,
                                              IsTransferdToQBO = a.IsTransferdToQBO,

                                              IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                              IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                              IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                              ReceivedByPartner = a.ReceivedByPartner,
											  IsFromCloud = a.IsFromCloud,

										  }).FirstOrDefault();

            if (extDocPm != null)
            {
                SetDocumentFollowUp(extDocPm);
                DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            }
            return extDocPm;
        }

        public List<DocumentsFilingPM> GetDocumentsFilingPMs(int tenant)
        {
            List<DocumentsFilingPM> externalDocumentPMs = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                                           where a.Tenant == tenant
                                                           select new DocumentsFilingPM()
                                                           {
                                                               Id = a.Id,
                                                               DocumentId = a.DocumentId,
                                                               Code = a.Code,
                                                               DirectionCode = a.DirectionCode,
                                                               Description = a.Description,
                                                               CreatedByUserId = a.CreatedByUserId,
                                                               CreateDate = a.CreateDate,
                                                               ObjectTableId = a.ObjectTableId,
                                                               ChildEntityId = a.ChildEntityId,
                                                               ChildObjectTableId = a.ChildObjectTableId,
                                                               ChildEntityReference = a.ChildEntityReference,
                                                               DocumentTypeId = a.DocumentTypeId,
                                                               EntityId = a.EntityId,
                                                               HasCopies = a.HasCopies,
                                                               Notes = a.Notes,
                                                               OwnerId = a.OwnerId,
                                                               SearchFields = a.SearchFields,
                                                               Tenant = a.Tenant,
                                                               FileExtension = a.Document != null ? a.Document.Extension : null,
                                                               HasFile = a.Document != null ? a.Document.HasFile : false,
                                                               FileName = a.Document != null ? a.Document.FileName : null,
                                                               FileSize = a.Document != null ? a.Document.FileSize : null,
                                                               Folder = a.Document != null ? a.Document.Folder : null,
                                                               DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                               DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                               DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                                               IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                                               IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                                               CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                               Received = a.Received,
                                                               ReceivedDate = a.ReceivedDate,
                                                               ReceivedByUserId = a.ReceivedByUserId,
                                                               ReceivedByByContactId = a.ReceivedByByContactId,
                                                               Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                                               StatusCode = a.StatusCode,
                                                               UpdateDate = a.UpdateDate,
                                                               UpdatedByUserId = a.UpdatedByUserId,
                                                               CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                               CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                               DepartmentId = a.DepartmentId,
                                                               BranchId = a.BranchId,
                                                               FolderId = a.FolderId,
                                                               IsDeleted = a.IsDeleted,
                                                               DeleteDateTime = a.DeleteDateTime,
                                                               DeletedByUserId = a.DeletedByUserId,
                                                               IsDigitallySigned = a.IsDigitallySigned,
                                                               SignersList = a.SignersList,
                                                               IsSharedWithCustomer = a.IsSharedWithCustomer,
                                                               IsSharedWithForwarder = a.IsSharedWithForwarder,
                                                               CustomerDocumentId = a.CustomerDocumentId,
                                                               ForwarderDocumentId = a.ForwarderDocumentId,
                                                               SecurityId = a.SecurityId,

                                                               LastVersion = a.LastVersion,
                                                               CustomerTenantNumber = a.CustomerTenantNumber,
                                                               IsRequested = a.IsRequested,
                                                               ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                                               SignRequestByUserEmail = a.SignRequestByUserEmail,
                                                               CancellSignRequest = a.CancellSignRequest,
                                                               OrigionalDocumentId = a.OrigionalDocumentId,
                                                               LastShareDate = a.LastShareDate,
                                                               IsSharedIn = a.IsSharedIn,
                                                               IsSharedOut = a.IsSharedOut,
                                                               SignDueDate = a.SignDueDate,
                                                               IsDigitalSignRequired = a.IsDigitalSignRequired,
                                                               BackedupExternally = a.BackedupExternally,
                                                               IsTransferdToQBO = a.IsTransferdToQBO,

                                                               IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                                               IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                                               IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                                               ReceivedByPartner = a.ReceivedByPartner,
															   IsFromCloud = a.IsFromCloud,

														   }).ToList();

            ICustomsDocumentQueryServiceExt customsDocumentQueryService = ContainerAccessor.Container.Resolve(typeof(ICustomsDocumentQueryServiceExt), "CustomsDocumentQueryServiceExt", new ParameterOverride("", 1)) as ICustomsDocumentQueryServiceExt;
            //CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(tenant);
            var followUpIds = new FollowUpRepository(tenant).GetFollowUpIdByDocumentsFilingIds(tenant, externalDocumentPMs.Select(x => x.Id).ToArray());

            DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
            //List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValuesList = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByTenantAndDocumentIds(tenant, externalDocumentPMs.Select(a => a.Id).ToArray()).ToList();

            foreach (DocumentsFilingPM extDocPm in externalDocumentPMs)
            {
                List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValuesList = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList(); //documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByTenant(tenant).ToList();
                SetDocumentFollowUp(extDocPm, followUpIds.ContainsKey(extDocPm.Id) ? followUpIds[extDocPm.Id] : string.Empty);
                CustomsDocumentPM customsDoc = customsDocumentQueryService.GetSingle(extDocPm.Id, false, false, tenant);
                if (customsDoc != null)
                {
                    extDocPm.CustomsDocumentTypeName = customsDoc.DocumentTypeName;
                    extDocPm.CustomsDocumentTypeCode = customsDoc.DocumentTypeCode;
                    extDocPm.IsMetaDataReady = customsDoc.IsMetaDataReady;
                }

                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValuesList;//.Where(d => d.DocumentsFilingId == extDocPm.Id && d.Tenant == extDocPm.Tenant).ToList();
            }

            return externalDocumentPMs;
        }

        public List<DocumentsFilingPM> GetDocumentsFilingPMs(string directionCode, int tenant)
        {
            List<DocumentsFilingPM> externalDocumentPMs = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                                           where a.Tenant == tenant && a.DirectionCode == directionCode
                                                           select new DocumentsFilingPM()
                                                           {
                                                               Id = a.Id,
                                                               DocumentId = a.DocumentId,
                                                               Code = a.Code,
                                                               DirectionCode = a.DirectionCode,
                                                               Description = a.Description,
                                                               CreatedByUserId = a.CreatedByUserId,
                                                               CreateDate = a.CreateDate,
                                                               ObjectTableId = a.ObjectTableId,
                                                               ChildEntityId = a.ChildEntityId,
                                                               ChildObjectTableId = a.ChildObjectTableId,
                                                               ChildEntityReference = a.ChildEntityReference,
                                                               DocumentTypeId = a.DocumentTypeId,
                                                               EntityId = a.EntityId,
                                                               HasCopies = a.HasCopies,
                                                               Notes = a.Notes,
                                                               OwnerId = a.OwnerId,
                                                               SearchFields = a.SearchFields,
                                                               Tenant = a.Tenant,
                                                               FileExtension = a.Document != null ? a.Document.Extension : null,
                                                               HasFile = a.Document != null ? a.Document.HasFile : false,
                                                               FileName = a.Document != null ? a.Document.FileName : null,
                                                               FileSize = a.Document != null ? a.Document.FileSize : null,
                                                               Folder = a.Document != null ? a.Document.Folder : null,
                                                               DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                               DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                               DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                                               IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                                               IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                                               CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                               Received = a.Received,
                                                               ReceivedDate = a.ReceivedDate,
                                                               ReceivedByUserId = a.ReceivedByUserId,
                                                               ReceivedByByContactId = a.ReceivedByByContactId,
                                                               Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                                               StatusCode = a.StatusCode,
                                                               UpdateDate = a.UpdateDate,
                                                               UpdatedByUserId = a.UpdatedByUserId,
                                                               CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                               CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,

                                                               EntityReference = a.EntityReference,
                                                               ExternalEntityName = a.ExternalEntityName,
                                                               ExternalEntityReference = a.ExternalEntityReference,
                                                               DepartmentId = a.DepartmentId,
                                                               BranchId = a.BranchId,
                                                               FolderId = a.FolderId,
                                                               IsDeleted = a.IsDeleted,
                                                               DeleteDateTime = a.DeleteDateTime,
                                                               DeletedByUserId = a.DeletedByUserId,
                                                               IsDigitallySigned = a.IsDigitallySigned,
                                                               SignersList = a.SignersList,
                                                               IsSharedWithCustomer = a.IsSharedWithCustomer,
                                                               IsSharedWithForwarder = a.IsSharedWithForwarder,
                                                               CustomerDocumentId = a.CustomerDocumentId,
                                                               ForwarderDocumentId = a.ForwarderDocumentId,
                                                               SecurityId = a.SecurityId,

                                                               LastVersion = a.LastVersion,
                                                               CustomerTenantNumber = a.CustomerTenantNumber,
                                                               IsRequested = a.IsRequested,
                                                               ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                                               SignRequestByUserEmail = a.SignRequestByUserEmail,
                                                               CancellSignRequest = a.CancellSignRequest,
                                                               OrigionalDocumentId = a.OrigionalDocumentId,
                                                               LastShareDate = a.LastShareDate,
                                                               IsSharedIn = a.IsSharedIn,
                                                               IsSharedOut = a.IsSharedOut,
                                                               SignDueDate = a.SignDueDate,
                                                               IsDigitalSignRequired = a.IsDigitalSignRequired,
                                                               BackedupExternally = a.BackedupExternally,
                                                               IsTransferdToQBO = a.IsTransferdToQBO,

                                                               IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                                               IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                                               IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                                               ReceivedByPartner = a.ReceivedByPartner,
															   IsFromCloud = a.IsFromCloud,

														   }).ToList();

            ICustomsDocumentQueryServiceExt customsDocumentQueryService = ContainerAccessor.Container.Resolve(typeof(ICustomsDocumentQueryServiceExt), "CustomsDocumentQueryServiceExt", new ParameterOverride("", 1)) as ICustomsDocumentQueryServiceExt;
            //CustomsDocumentQueryService customsDocumentQueryService = new CustomsDocumentQueryService(tenant);
            var followUpIds = new FollowUpRepository(tenant).GetFollowUpIdByDocumentsFilingIds(tenant, externalDocumentPMs.Select(x => x.Id).ToArray());
            DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
            //List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValuesList = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByTenantAndDocumentIds(tenant, externalDocumentPMs.Select(a=>a.Id).ToArray()).ToList();

            foreach (DocumentsFilingPM extDocPm in externalDocumentPMs)
            {
                List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValuesList = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList(); //documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByTenant(tenant).ToList();
                SetDocumentFollowUp(extDocPm, followUpIds.ContainsKey(extDocPm.Id) ? followUpIds[extDocPm.Id] : string.Empty);
                CustomsDocumentPM customsDoc = customsDocumentQueryService.GetSingle(extDocPm.Id, false, false,tenant);
                if (customsDoc != null)
                {
                    extDocPm.CustomsDocumentTypeName = customsDoc.DocumentTypeName;
                    extDocPm.CustomsDocumentTypeCode = customsDoc.DocumentTypeCode;
                    extDocPm.IsMetaDataReady = customsDoc.IsMetaDataReady;
                }

                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValuesList;//.Where(d => d.DocumentsFilingId == extDocPm.Id && d.Tenant == extDocPm.Tenant).ToList();
            }

            return externalDocumentPMs;
        }



        public List<DocumentsFilingPM> GetDocumentsFilingPMsByEntityIdAndObjectTableAndDirectionCode(string entityId, string childEntityId, string objectTableId, string directionCode, int tenant)
        {
            (repository.context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 


            ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);
            ObjectTablePM shipmentObject = null;
            string shipmentObjectId = null;
            if (objectTableRep.IsObjectTableMaster(objectTableId))
            {
                shipmentObject = ObjectTableQuery.GetObjectTableByCode("Shipment", tenant);
                shipmentObjectId = shipmentObject.Id;
            }

            IQueryable<DocumentsFilingPM> externalDocumentPMs;
            List<DocumentsFilingPM> result;

            var Today = DateTime.Today;
            var LastWeek = DateTime.Today.AddDays(-7);
            externalDocumentPMs = (from a in repository.context.DocumentsFilings.Include("Document").Include("DocumentType")
                                   where a.Tenant == tenant && a.EntityId == entityId && (a.ObjectTableId == objectTableId || a.ObjectTableId == shipmentObjectId)
                                   && a.DirectionCode == directionCode && a.IsDeleted == false
                                   select new DocumentsFilingPM()
                                   {
                                       Id = a.Id,
                                       DocumentId = a.DocumentId,
                                       Code = a.Code,
                                       DirectionCode = a.DirectionCode,
                                       Description = a.Description,
                                       CreatedByUserId = a.CreatedByUserId,
                                       CreateDate = a.CreateDate,
                                       ObjectTableId = a.ObjectTableId,
                                       ChildEntityId = a.ChildEntityId,
                                       ChildObjectTableId = a.ChildObjectTableId,
                                       ChildEntityReference = a.ChildEntityReference,
                                       DocumentTypeId = a.DocumentTypeId,
                                       EntityId = a.EntityId,
                                       HasCopies = a.HasCopies,
                                       Notes = a.Notes,
                                       OwnerId = a.OwnerId,
                                       SearchFields = a.SearchFields,
                                       Tenant = a.Tenant,
                                       FileExtension = a.Document != null ? a.Document.Extension : null,
                                       HasFile = a.Document != null ? a.Document.HasFile : false,
                                       FileName = a.Document != null ? a.Document.FileName : null,
                                       FileSize = a.Document != null ? a.Document.FileSize : null,
                                       Folder = a.Document != null ? a.Document.Folder : null,
                                       DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                       DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                       DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                       IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                       IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                       //CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                       Received = a.Received,
                                       ReceivedDate = a.ReceivedDate,
                                       ReceivedByUserId = a.ReceivedByUserId,
                                       ReceivedByByContactId = a.ReceivedByByContactId,
                                       Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                       StatusCode = a.StatusCode,
                                       UpdateDate = a.UpdateDate,
                                       UpdatedByUserId = a.UpdatedByUserId,
                                       CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                       CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,

                                       EntityReference = a.EntityReference,
                                       ExternalEntityName = a.ExternalEntityName,
                                       ExternalEntityReference = a.ExternalEntityReference,
                                       DepartmentId = a.DepartmentId,
                                       BranchId = a.BranchId,
                                       FolderId = a.FolderId,
                                       IsDeleted = a.IsDeleted,
                                       DeleteDateTime = a.DeleteDateTime,
                                       DeletedByUserId = a.DeletedByUserId,
                                       IsDigitallySigned = a.IsDigitallySigned,
                                       SignersList = a.SignersList,
                                       IsSharedWithCustomer = a.IsSharedWithCustomer,
                                       IsSharedWithForwarder = a.IsSharedWithForwarder,
                                       CustomerDocumentId = a.CustomerDocumentId,
                                       ForwarderDocumentId = a.ForwarderDocumentId,
                                       DocumentCategoryCode = a.DocumentType.DocumentTypeCategory.Code,
                                       DocumentCategoryName = a.DocumentType.DocumentTypeCategory.Name,
                                       CreateDateWords = a.CreateDate >= Today ? "Today" : (a.CreateDate > LastWeek && a.CreateDate < Today ? "Last 7 Days" : "Older"),
                                       SecurityId = a.SecurityId,
                                       CustomerTenantNumber = a.CustomerTenantNumber,
                                       LastVersion = a.LastVersion,
                                       IsRequested = a.IsRequested,
                                       //ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                       SignRequestByUserEmail = a.SignRequestByUserEmail,
                                       CancellSignRequest = a.CancellSignRequest,
                                       OrigionalDocumentId = a.OrigionalDocumentId,
                                       LastShareDate = a.LastShareDate,
                                       IsSharedIn = a.IsSharedIn,
                                       IsSharedOut = a.IsSharedOut,
                                       SignDueDate = a.SignDueDate,
                                       IsDigitalSignRequired = a.IsDigitalSignRequired,
                                       BackedupExternally = a.BackedupExternally,

                                       IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                       IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                       IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                       ReceivedByPartner = a.ReceivedByPartner,
									   IsFromCloud = a.IsFromCloud,

								   });

            if (!string.IsNullOrEmpty(childEntityId)) externalDocumentPMs = externalDocumentPMs.Where(d => d.ChildEntityId == childEntityId);

            result = externalDocumentPMs.ToList();

            #region Contacts 
            List<string> contactIds = new List<string>();
            foreach (DocumentsFilingPM item in result.Where(d => !string.IsNullOrEmpty(d.CreatedByUserId) || !string.IsNullOrEmpty(d.ReceivedByUserId)).ToList())
            {
                if (!string.IsNullOrEmpty(item.CreatedByUserId))
                {
                    if (!contactIds.Contains(item.CreatedByUserId)) contactIds.Add(item.CreatedByUserId);
                }

                if (!string.IsNullOrEmpty(item.ReceivedByUserId))
                {
                    if (!contactIds.Contains(item.ReceivedByUserId)) contactIds.Add(item.ReceivedByUserId);
                }

                if (!string.IsNullOrEmpty(item.ReceivedByByContactId))
                {
                    if (!contactIds.Contains(item.ReceivedByByContactId)) contactIds.Add(item.ReceivedByByContactId);
                }
            }

            List<Contact> contactLists = null;
            if (contactIds.Count > 0)
            {
                ContactRepository contactRepository = new ContactRepository(tenant);
                contactLists = contactRepository.GetContactListsByListids(contactIds, tenant).ToList();
            }

            foreach (DocumentsFilingPM item in result.Where(d => !string.IsNullOrEmpty(d.CreatedByUserId) || !string.IsNullOrEmpty(d.ReceivedByUserId)).ToList())
            {
                if (!string.IsNullOrEmpty(item.CreatedByUserId))
                {
                    Contact createdByUser = contactLists.Where(d => d.Id == item.CreatedByUserId).FirstOrDefault();
                    if (createdByUser != null) item.CreatedByUserName = createdByUser.EnglishName;
                }

                if (!string.IsNullOrEmpty(item.ReceivedByUserId))
                {
                    var receivedByUserId = item.ReceivedByUserId;
                    if (!string.IsNullOrEmpty(item.ReceivedByByContactId))
                    {
                        receivedByUserId = item.ReceivedByByContactId;
                    }

                    Contact receivedByUser = contactLists.Where(d => d.Id == receivedByUserId).FirstOrDefault();
                    if (receivedByUser != null) item.ReceivedByUserName = receivedByUser.EnglishName;
                }
            }
            #endregion

            return result;
        }


        public List<DocumentsFilingPM> GetDocumentsFilingPMsByEntityIdAndObjectTable(string entityId, string childEntityId, string objectTableId, string directionCode, int tenant)
        {
            (repository.context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);
            ObjectTablePM shipmentObject = null;
            string shipmentObjectId = null;
            if (objectTableRep.IsObjectTableMaster(objectTableId))
            {
                shipmentObject = ObjectTableQuery.GetObjectTableByCode("Shipment", tenant);
                shipmentObjectId = shipmentObject.Id;
            }

            List<DocumentsFilingPM> externalDocumentPMs;
            var Today = DateTime.Today;
            var LastWeek = DateTime.Today.AddDays(-7);
            if (string.IsNullOrEmpty(childEntityId))
            {

                externalDocumentPMs = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                       where a.Tenant == tenant && a.EntityId == entityId && (a.ObjectTableId == objectTableId || a.ObjectTableId == shipmentObjectId)
                                       && a.DirectionCode == directionCode
                                       select new DocumentsFilingPM()
                                       {
                                           Id = a.Id,
                                           DocumentId = a.DocumentId,
                                           Code = a.Code,
                                           DirectionCode = a.DirectionCode,
                                           Description = a.Description,
                                           CreatedByUserId = a.CreatedByUserId,
                                           CreateDate = a.CreateDate,
                                           ObjectTableId = a.ObjectTableId,
                                           ChildEntityId = a.ChildEntityId,
                                           ChildObjectTableId = a.ChildObjectTableId,
                                           ChildEntityReference = a.ChildEntityReference,
                                           DocumentTypeId = a.DocumentTypeId,
                                           EntityId = a.EntityId,
                                           HasCopies = a.HasCopies,
                                           Notes = a.Notes,
                                           OwnerId = a.OwnerId,
                                           SearchFields = a.SearchFields,
                                           Tenant = a.Tenant,
                                           FileExtension = a.Document != null ? a.Document.Extension : null,
                                           HasFile = a.Document != null ? a.Document.HasFile : false,
                                           FileName = a.Document != null ? a.Document.FileName : null,
                                           FileSize = a.Document != null ? a.Document.FileSize : null,
                                           Folder = a.Document != null ? a.Document.Folder : null,
                                           DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                           IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                           IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                           CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                           Received = a.Received,
                                           ReceivedDate = a.ReceivedDate,
                                           ReceivedByUserId = a.ReceivedByUserId,
                                           ReceivedByByContactId = a.ReceivedByByContactId,
                                           Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                           StatusCode = a.StatusCode,
                                           UpdateDate = a.UpdateDate,
                                           UpdatedByUserId = a.UpdatedByUserId,
                                           CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,

                                           EntityReference = a.EntityReference,
                                           ExternalEntityName = a.ExternalEntityName,
                                           ExternalEntityReference = a.ExternalEntityReference,
                                           DepartmentId = a.DepartmentId,
                                           BranchId = a.BranchId,
                                           FolderId = a.FolderId,
                                           IsDeleted = a.IsDeleted,
                                           DeleteDateTime = a.DeleteDateTime,
                                           DeletedByUserId = a.DeletedByUserId,
                                           IsDigitallySigned = a.IsDigitallySigned,
                                           SignersList = a.SignersList,
                                           IsSharedWithCustomer = a.IsSharedWithCustomer,
                                           IsSharedWithForwarder = a.IsSharedWithForwarder,
                                           CustomerDocumentId = a.CustomerDocumentId,
                                           ForwarderDocumentId = a.ForwarderDocumentId,
                                           DocumentCategoryCode = a.DocumentType.DocumentTypeCategory.Code,
                                           DocumentCategoryName = a.DocumentType.DocumentTypeCategory.Name,
                                           CreateDateWords = a.CreateDate >= Today ? "Today" : (a.CreateDate > LastWeek && a.CreateDate < Today ? "Last 7 Days" : "Older"),
                                           SecurityId = a.SecurityId,
                                           CustomerTenantNumber = a.CustomerTenantNumber,
                                           LastVersion = a.LastVersion,

                                           IsRequested = a.IsRequested,
                                           ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                           SignRequestByUserEmail = a.SignRequestByUserEmail,
                                           CancellSignRequest = a.CancellSignRequest,
                                           OrigionalDocumentId = a.OrigionalDocumentId,
                                           LastShareDate = a.LastShareDate,
                                           IsSharedIn = a.IsSharedIn,
                                           IsSharedOut = a.IsSharedOut,
                                           SignDueDate = a.SignDueDate,
                                           IsDigitalSignRequired = a.IsDigitalSignRequired,
                                           BackedupExternally = a.BackedupExternally,

                                           IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                           IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                           IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                           ReceivedByPartner = a.ReceivedByPartner,
										   IsFromCloud = a.IsFromCloud,

									   }).ToList();
            }
            else
            {
                externalDocumentPMs = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                       where a.Tenant == tenant && a.EntityId == entityId && a.ChildEntityId == childEntityId && (a.ObjectTableId == objectTableId || a.ObjectTableId == shipmentObjectId)
                                       select new DocumentsFilingPM()
                                       {
                                           Id = a.Id,
                                           DocumentId = a.DocumentId,
                                           Code = a.Code,
                                           DirectionCode = a.DirectionCode,
                                           Description = a.Description,
                                           CreatedByUserId = a.CreatedByUserId,
                                           CreateDate = a.CreateDate,
                                           ObjectTableId = a.ObjectTableId,
                                           ChildEntityId = a.ChildEntityId,
                                           ChildObjectTableId = a.ChildObjectTableId,
                                           ChildEntityReference = a.ChildEntityReference,
                                           DocumentTypeId = a.DocumentTypeId,
                                           EntityId = a.EntityId,
                                           HasCopies = a.HasCopies,
                                           Notes = a.Notes,
                                           OwnerId = a.OwnerId,
                                           SearchFields = a.SearchFields,
                                           Tenant = a.Tenant,
                                           FileExtension = a.Document != null ? a.Document.Extension : null,
                                           HasFile = a.Document != null ? a.Document.HasFile : false,
                                           FileName = a.Document != null ? a.Document.FileName : null,
                                           FileSize = a.Document != null ? a.Document.FileSize : null,
                                           Folder = a.Document != null ? a.Document.Folder : null,
                                           DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                           IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                           IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                           CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                           Received = a.Received,
                                           ReceivedDate = a.ReceivedDate,
                                           ReceivedByUserId = a.ReceivedByUserId,
                                           ReceivedByByContactId = a.ReceivedByByContactId,
                                           Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                           StatusCode = a.StatusCode,
                                           UpdateDate = a.UpdateDate,
                                           UpdatedByUserId = a.UpdatedByUserId,
                                           CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,

                                           EntityReference = a.EntityReference,
                                           ExternalEntityName = a.ExternalEntityName,
                                           ExternalEntityReference = a.ExternalEntityReference,
                                           DepartmentId = a.DepartmentId,
                                           BranchId = a.BranchId,
                                           FolderId = a.FolderId,
                                           IsDeleted = a.IsDeleted,
                                           DeleteDateTime = a.DeleteDateTime,
                                           DeletedByUserId = a.DeletedByUserId,
                                           IsDigitallySigned = a.IsDigitallySigned,
                                           SignersList = a.SignersList,
                                           IsSharedWithCustomer = a.IsSharedWithCustomer,
                                           IsSharedWithForwarder = a.IsSharedWithForwarder,
                                           CustomerDocumentId = a.CustomerDocumentId,
                                           ForwarderDocumentId = a.ForwarderDocumentId,
                                           CreateDateWords = a.CreateDate >= Today ? "Today" : (a.CreateDate > LastWeek && a.CreateDate < Today ? "Last 7 Days" : "Older"),
                                           SecurityId = a.SecurityId,

                                           LastVersion = a.LastVersion,
                                           CustomerTenantNumber = a.CustomerTenantNumber,
                                           IsRequested = a.IsRequested,
                                           ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                           SignRequestByUserEmail = a.SignRequestByUserEmail,
                                           CancellSignRequest = a.CancellSignRequest,
                                           OrigionalDocumentId = a.OrigionalDocumentId,
                                           LastShareDate = a.LastShareDate,
                                           IsSharedIn = a.IsSharedIn,
                                           IsSharedOut = a.IsSharedOut,
                                           SignDueDate = a.SignDueDate,
                                           IsDigitalSignRequired = a.IsDigitalSignRequired,
                                           BackedupExternally = a.BackedupExternally,

                                           IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                           IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                           IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                           ReceivedByPartner = a.ReceivedByPartner,
										   IsFromCloud = a.IsFromCloud,

									   }).ToList();
            }

    
            var followUpIds = new FollowUpRepository(tenant).GetFollowUpIdByDocumentsFilingIds(tenant, externalDocumentPMs.Select(x => x.Id).ToArray());
            DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);

            foreach (DocumentsFilingPM extDocPm in externalDocumentPMs)
            {
                List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValuesList = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList(); //documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByTenant(tenant).ToList();
                DocumentsMetaDataTypeRepository TypesRepo = new DocumentsMetaDataTypeRepository(tenant);
                var DREL = TypesRepo.GetSingleDocumentsMetaDataTypeByCode("DREL", tenant);
                var CREF = TypesRepo.GetSingleDocumentsMetaDataTypeByCode("CREF", tenant);
                if (DREL != null && CREF != null)
                {
                    var MyDRELData = documentsFilingMetaDataValuesList.Where(a => a.DocumentsMetaDataTypeId == DREL.Id).FirstOrDefault();
                    if (MyDRELData != null)
                    {
                        var MyCREFData = documentsFilingMetaDataValuesList.Where(a => a.DocumentsMetaDataTypeId == CREF.Id).FirstOrDefault();
                        if (MyCREFData != null && !string.IsNullOrEmpty(MyCREFData.MetaDataValue) && MyDRELData.MetaDataValue.ToLower() == "true")
                        {
                            extDocPm.IsCustomReference = true;
                            extDocPm.CustomReference = MyCREFData.MetaDataValue;
                        }
                    }
                }
                SetDocumentFollowUp(extDocPm, followUpIds.ContainsKey(extDocPm.Id) ? followUpIds[extDocPm.Id] : string.Empty);
                //CustomsDocumentPM customsDoc = customsDocumentQueryService.GetSingle(extDocPm.Id, false, false,tenant);
                //if (customsDoc != null)
                //{
                //    extDocPm.CustomsDocumentTypeName = customsDoc.DocumentTypeName;
                //    extDocPm.CustomsDocumentTypeCode = customsDoc.DocumentTypeCode;
                //    extDocPm.IsMetaDataReady = customsDoc.IsMetaDataReady;
                //}

                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValuesList;//.Where(d => d.DocumentsFilingId == extDocPm.Id && d.Tenant == extDocPm.Tenant).ToList();
            }

            return externalDocumentPMs;
        }

        public List<DocumentsFilingPM> GetDocumentsFilingPMsAsAttachmentByEntityIdAndObjectTable(string entityId, string childEntityId, string objectTableId, string directionCode, int tenant)
        {
            (repository.context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);
            ObjectTablePM shipmentObject = null;
            string shipmentObjectId = null;
            if (objectTableRep.IsObjectTableMaster(objectTableId))
            {
                shipmentObject = ObjectTableQuery.GetObjectTableByCode("Shipment", tenant);
                shipmentObjectId = shipmentObject.Id;
            }

            List<DocumentsFilingPM> externalDocumentPMs;
            var Today = DateTime.Today;
            var LastWeek = DateTime.Today.AddDays(-7);
            if (string.IsNullOrEmpty(childEntityId))
            {

                externalDocumentPMs = (from a in repository.context.DocumentsFilings.Include("Document").Include("DocumentType")
                                       where a.Tenant == tenant && a.EntityId == entityId && (a.ObjectTableId == objectTableId || a.ObjectTableId == shipmentObjectId)
                                       && a.DirectionCode == directionCode && a.IsDeleted == false
                                       select new DocumentsFilingPM()
                                       {
                                           Id = a.Id,
                                           DocumentId = a.DocumentId,
                                           Code = a.Code,
                                           DirectionCode = a.DirectionCode,
                                           Description = a.Description,
                                           CreatedByUserId = a.CreatedByUserId,
                                           CreateDate = a.CreateDate,
                                           ObjectTableId = a.ObjectTableId,
                                           ChildEntityId = a.ChildEntityId,
                                           ChildObjectTableId = a.ChildObjectTableId,
                                           ChildEntityReference = a.ChildEntityReference,
                                           DocumentTypeId = a.DocumentTypeId,
                                           EntityId = a.EntityId,
                                           HasCopies = a.HasCopies,
                                           Notes = a.Notes,
                                           OwnerId = a.OwnerId,
                                           SearchFields = a.SearchFields,
                                           Tenant = a.Tenant,
                                           FileExtension = a.Document != null ? a.Document.Extension : null,
                                           HasFile = a.Document != null ? a.Document.HasFile : false,
                                           FileName = a.Document != null ? a.Document.FileName : null,
                                           FileSize = a.Document != null ? a.Document.FileSize : null,
                                           Folder = a.Document != null ? a.Document.Folder : null,
                                           DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                           IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                           IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                           Received = a.Received,
                                           ReceivedDate = a.ReceivedDate,
                                           ReceivedByUserId = a.ReceivedByUserId,
                                           ReceivedByByContactId = a.ReceivedByByContactId,
                                           Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                           StatusCode = a.StatusCode,
                                           UpdateDate = a.UpdateDate,
                                           UpdatedByUserId = a.UpdatedByUserId,
                                           CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           EntityReference = a.EntityReference,
                                           ExternalEntityName = a.ExternalEntityName,
                                           ExternalEntityReference = a.ExternalEntityReference,
                                           DepartmentId = a.DepartmentId,
                                           BranchId = a.BranchId,
                                           FolderId = a.FolderId,
                                           IsDeleted = a.IsDeleted,
                                           DeleteDateTime = a.DeleteDateTime,
                                           DeletedByUserId = a.DeletedByUserId,
                                           IsDigitallySigned = a.IsDigitallySigned,
                                           SignersList = a.SignersList,
                                           IsSharedWithCustomer = a.IsSharedWithCustomer,
                                           IsSharedWithForwarder = a.IsSharedWithForwarder,
                                           CustomerDocumentId = a.CustomerDocumentId,
                                           ForwarderDocumentId = a.ForwarderDocumentId,
                                           CreateDateWords = a.CreateDate >= Today ? "Today" : (a.CreateDate > LastWeek && a.CreateDate < Today ? "Last 7 Days" : "Older"),
                                           SecurityId = a.SecurityId,
                                           LastVersion = a.LastVersion,
                                           CustomerTenantNumber = a.CustomerTenantNumber,
                                           IsRequested = a.IsRequested,
                                           SignRequestByUserEmail = a.SignRequestByUserEmail,
                                           CancellSignRequest = a.CancellSignRequest,
                                           OrigionalDocumentId = a.OrigionalDocumentId,
                                           LastShareDate = a.LastShareDate,
                                           IsSharedIn = a.IsSharedIn,
                                           IsSharedOut = a.IsSharedOut,
                                           SignDueDate = a.SignDueDate,
                                           IsDigitalSignRequired = a.IsDigitalSignRequired,
                                           BackedupExternally = a.BackedupExternally,
                                           CalculatedFileName = a.Document != null ? a.Document.CalculatedFileName : null,

                                           IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                           IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                           IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                           ReceivedByPartner = a.ReceivedByPartner,
										   IsFromCloud = a.IsFromCloud,

									   }).ToList();
            }
            else
            {
                externalDocumentPMs = (from a in repository.context.DocumentsFilings.Include("Document").Include("DocumentType")
                                       where a.Tenant == tenant && a.EntityId == entityId && a.ChildEntityId == childEntityId && (a.ObjectTableId == objectTableId || a.ObjectTableId == shipmentObjectId)
                                       select new DocumentsFilingPM()
                                       {
                                           Id = a.Id,
                                           DocumentId = a.DocumentId,
                                           Code = a.Code,
                                           DirectionCode = a.DirectionCode,
                                           Description = a.Description,
                                           CreatedByUserId = a.CreatedByUserId,
                                           CreateDate = a.CreateDate,
                                           ObjectTableId = a.ObjectTableId,
                                           ChildEntityId = a.ChildEntityId,
                                           ChildObjectTableId = a.ChildObjectTableId,
                                           ChildEntityReference = a.ChildEntityReference,
                                           DocumentTypeId = a.DocumentTypeId,
                                           EntityId = a.EntityId,
                                           HasCopies = a.HasCopies,
                                           Notes = a.Notes,
                                           OwnerId = a.OwnerId,
                                           SearchFields = a.SearchFields,
                                           Tenant = a.Tenant,
                                           FileExtension = a.Document != null ? a.Document.Extension : null,
                                           HasFile = a.Document != null ? a.Document.HasFile : false,
                                           FileName = a.Document != null ? a.Document.FileName : null,
                                           FileSize = a.Document != null ? a.Document.FileSize : null,
                                           Folder = a.Document != null ? a.Document.Folder : null,
                                           DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                           IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                           IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                           Received = a.Received,
                                           ReceivedDate = a.ReceivedDate,
                                           ReceivedByUserId = a.ReceivedByUserId,
                                           ReceivedByByContactId = a.ReceivedByByContactId,
                                           Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                           StatusCode = a.StatusCode,
                                           UpdateDate = a.UpdateDate,
                                           UpdatedByUserId = a.UpdatedByUserId,
                                           CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           EntityReference = a.EntityReference,
                                           ExternalEntityName = a.ExternalEntityName,
                                           ExternalEntityReference = a.ExternalEntityReference,
                                           DepartmentId = a.DepartmentId,
                                           BranchId = a.BranchId,
                                           FolderId = a.FolderId,
                                           IsDeleted = a.IsDeleted,
                                           DeleteDateTime = a.DeleteDateTime,
                                           DeletedByUserId = a.DeletedByUserId,
                                           IsDigitallySigned = a.IsDigitallySigned,
                                           SignersList = a.SignersList,
                                           IsSharedWithCustomer = a.IsSharedWithCustomer,
                                           IsSharedWithForwarder = a.IsSharedWithForwarder,
                                           CustomerDocumentId = a.CustomerDocumentId,
                                           ForwarderDocumentId = a.ForwarderDocumentId,
                                           CreateDateWords = a.CreateDate >= Today ? "Today" : (a.CreateDate > LastWeek && a.CreateDate < Today ? "Last 7 Days" : "Older"),
                                           SecurityId = a.SecurityId,
                                           LastVersion = a.LastVersion,
                                           CustomerTenantNumber = a.CustomerTenantNumber,
                                           IsRequested = a.IsRequested,
                                           SignRequestByUserEmail = a.SignRequestByUserEmail,
                                           CancellSignRequest = a.CancellSignRequest,
                                           OrigionalDocumentId = a.OrigionalDocumentId,
                                           LastShareDate = a.LastShareDate,
                                           IsSharedIn = a.IsSharedIn,
                                           IsSharedOut = a.IsSharedOut,
                                           SignDueDate = a.SignDueDate,
                                           IsDigitalSignRequired = a.IsDigitalSignRequired,
                                           BackedupExternally = a.BackedupExternally,
                                           CalculatedFileName = a.Document != null ? a.Document.CalculatedFileName : null,

                                           IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                           IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                           IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                           ReceivedByPartner = a.ReceivedByPartner,
										   IsFromCloud = a.IsFromCloud,

									   }).ToList();
            }
            return externalDocumentPMs;
        }



        public List<DocumentsFilingPM> GetDocumentsFilingPMsByEntityId(string entityId, string directionCode, int tenant)
        {

            List<DocumentsFilingPM> externalDocumentPMs = new List<DocumentsFilingPM>();
            externalDocumentPMs = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                   where a.Tenant == tenant && a.EntityId == entityId && a.DirectionCode == directionCode && a.IsDeleted == false
                                   select new DocumentsFilingPM()
                                   {
                                       Id = a.Id,
                                       DocumentId = a.DocumentId,
                                       Code = a.Code,
                                       DirectionCode = a.DirectionCode,
                                       Description = a.Description,
                                       CreatedByUserId = a.CreatedByUserId,
                                       CreateDate = a.CreateDate,
                                       ObjectTableId = a.ObjectTableId,
                                       ChildEntityId = a.ChildEntityId,
                                       ChildObjectTableId = a.ChildObjectTableId,
                                       ChildEntityReference = a.ChildEntityReference,
                                       DocumentTypeId = a.DocumentTypeId,
                                       EntityId = a.EntityId,
                                       HasCopies = a.HasCopies,
                                       Notes = a.Notes,
                                       OwnerId = a.OwnerId,
                                       SearchFields = a.SearchFields,
                                       Tenant = a.Tenant,
                                       FileExtension = a.Document != null ? a.Document.Extension : null,
                                       HasFile = a.Document != null ? a.Document.HasFile : false,
                                       FileName = a.Document != null ? a.Document.FileName : null,
                                       FileSize = a.Document != null ? a.Document.FileSize : null,
                                       Folder = a.Document != null ? a.Document.Folder : null,
                                       DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                       DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                       DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                       IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                       IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                       CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                       Received = a.Received,
                                       ReceivedDate = a.ReceivedDate,
                                       ReceivedByUserId = a.ReceivedByUserId,
                                       ReceivedByByContactId = a.ReceivedByByContactId,
                                       Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                       StatusCode = a.StatusCode,
                                       UpdateDate = a.UpdateDate,
                                       UpdatedByUserId = a.UpdatedByUserId,
                                       CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                       CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,

                                       EntityReference = a.EntityReference,
                                       ExternalEntityName = a.ExternalEntityName,
                                       ExternalEntityReference = a.ExternalEntityReference,
                                       DepartmentId = a.DepartmentId,
                                       BranchId = a.BranchId,
                                       FolderId = a.FolderId,
                                       IsDeleted = a.IsDeleted,
                                       DeleteDateTime = a.DeleteDateTime,
                                       DeletedByUserId = a.DeletedByUserId,
                                       IsDigitallySigned = a.IsDigitallySigned,
                                       SignersList = a.SignersList,
                                       IsSharedWithCustomer = a.IsSharedWithCustomer,
                                       IsSharedWithForwarder = a.IsSharedWithForwarder,
                                       CustomerDocumentId = a.CustomerDocumentId,
                                       ForwarderDocumentId = a.ForwarderDocumentId,
                                       SecurityId = a.SecurityId,

                                       LastVersion = a.LastVersion,
                                       CustomerTenantNumber = a.CustomerTenantNumber,
                                       IsRequested = a.IsRequested,
                                       ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                       SignRequestByUserEmail = a.SignRequestByUserEmail,
                                       CancellSignRequest = a.CancellSignRequest,
                                       OrigionalDocumentId = a.OrigionalDocumentId,
                                       LastShareDate = a.LastShareDate,
                                       IsSharedIn = a.IsSharedIn,
                                       IsSharedOut = a.IsSharedOut,
                                       SignDueDate = a.SignDueDate,
                                       IsDigitalSignRequired = a.IsDigitalSignRequired,
                                       BackedupExternally = a.BackedupExternally,

                                       IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                       IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                       IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                       ReceivedByPartner = a.ReceivedByPartner,
									   IsFromCloud = a.IsFromCloud,

								   }).ToList();

            ICustomsDocumentQueryServiceExt customsDocumentQueryService = ContainerAccessor.Container.Resolve(typeof(ICustomsDocumentQueryServiceExt), "CustomsDocumentQueryServiceExt", new ParameterOverride("", 1)) as ICustomsDocumentQueryServiceExt;
            var followUpIds = new FollowUpRepository(tenant).GetFollowUpIdByDocumentsFilingIds(tenant, externalDocumentPMs.Select(x => x.Id).ToArray());
            DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);

            foreach (DocumentsFilingPM extDocPm in externalDocumentPMs)
            {
                List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValuesList = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList(); //documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByTenant(tenant).ToList();
                SetDocumentFollowUp(extDocPm, followUpIds.ContainsKey(extDocPm.Id) ? followUpIds[extDocPm.Id] : string.Empty);
                CustomsDocumentPM customsDoc = customsDocumentQueryService.GetSingle(extDocPm.Id, false, false,tenant);
                if (customsDoc != null)
                {
                    extDocPm.CustomsDocumentTypeName = customsDoc.DocumentTypeName;
                    extDocPm.CustomsDocumentTypeCode = customsDoc.DocumentTypeCode;
                    extDocPm.IsMetaDataReady = customsDoc.IsMetaDataReady;
                }

                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValuesList;//.Where(d => d.DocumentsFilingId == extDocPm.Id && d.Tenant == extDocPm.Tenant).ToList();
            }

            return externalDocumentPMs;
        }

        public List<DocumentsFilingPM> GetDocumentsFilingPMsByEntityIdWithoutCustomsDetails(string entityId, string directionCode, int tenant)
        {

            List<DocumentsFilingPM> externalDocumentPMs = new List<DocumentsFilingPM>();
            externalDocumentPMs = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                   where a.Tenant == tenant && a.EntityId == entityId && a.DirectionCode == directionCode && a.IsDeleted == false
                                   select new DocumentsFilingPM()
                                   {
                                       Id = a.Id,
                                       DocumentId = a.DocumentId,
                                       Code = a.Code,
                                       DirectionCode = a.DirectionCode,
                                       Description = a.Description,
                                       CreatedByUserId = a.CreatedByUserId,
                                       CreateDate = a.CreateDate,
                                       ObjectTableId = a.ObjectTableId,
                                       ChildEntityId = a.ChildEntityId,
                                       ChildObjectTableId = a.ChildObjectTableId,
                                       ChildEntityReference = a.ChildEntityReference,
                                       DocumentTypeId = a.DocumentTypeId,
                                       EntityId = a.EntityId,
                                       HasCopies = a.HasCopies,
                                       Notes = a.Notes,
                                       OwnerId = a.OwnerId,
                                       SearchFields = a.SearchFields,
                                       Tenant = a.Tenant,
                                       FileExtension = a.Document != null ? a.Document.Extension : null,
                                       HasFile = a.Document != null ? a.Document.HasFile : false,
                                       FileName = a.Document != null ? a.Document.FileName : null,
                                       FileSize = a.Document != null ? a.Document.FileSize : null,
                                       Folder = a.Document != null ? a.Document.Folder : null,
                                       DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                       DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                       DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                       IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                       IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                       CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                       Received = a.Received,
                                       ReceivedDate = a.ReceivedDate,
                                       ReceivedByUserId = a.ReceivedByUserId,
                                       ReceivedByByContactId = a.ReceivedByByContactId,
                                       Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                       StatusCode = a.StatusCode,
                                       UpdateDate = a.UpdateDate,
                                       UpdatedByUserId = a.UpdatedByUserId,
                                       CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                       CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,

                                       EntityReference = a.EntityReference,
                                       ExternalEntityName = a.ExternalEntityName,
                                       ExternalEntityReference = a.ExternalEntityReference,
                                       DepartmentId = a.DepartmentId,
                                       BranchId = a.BranchId,
                                       FolderId = a.FolderId,
                                       IsDeleted = a.IsDeleted,
                                       DeleteDateTime = a.DeleteDateTime,
                                       DeletedByUserId = a.DeletedByUserId,
                                       IsDigitallySigned = a.IsDigitallySigned,
                                       SignersList = a.SignersList,
                                       IsSharedWithCustomer = a.IsSharedWithCustomer,
                                       IsSharedWithForwarder = a.IsSharedWithForwarder,
                                       CustomerDocumentId = a.CustomerDocumentId,
                                       ForwarderDocumentId = a.ForwarderDocumentId,
                                       SecurityId = a.SecurityId,

                                       LastVersion = a.LastVersion,
                                       CustomerTenantNumber = a.CustomerTenantNumber,
                                       IsRequested = a.IsRequested,
                                       ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                       SignRequestByUserEmail = a.SignRequestByUserEmail,
                                       CancellSignRequest = a.CancellSignRequest,
                                       OrigionalDocumentId = a.OrigionalDocumentId,
                                       LastShareDate = a.LastShareDate,
                                       IsSharedIn = a.IsSharedIn,
                                       IsSharedOut = a.IsSharedOut,
                                       SignDueDate = a.SignDueDate,
                                       IsDigitalSignRequired = a.IsDigitalSignRequired,
                                       BackedupExternally = a.BackedupExternally,

                                       IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                       IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                       IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                       ReceivedByPartner = a.ReceivedByPartner,
									   IsFromCloud = a.IsFromCloud,

								   }).ToList();

            var followUpIds = new FollowUpRepository(tenant).GetFollowUpIdByDocumentsFilingIds(tenant, externalDocumentPMs.Select(x => x.Id).ToArray());
            DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);

            foreach (DocumentsFilingPM extDocPm in externalDocumentPMs)
            {
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
                SetDocumentFollowUp(extDocPm, followUpIds.ContainsKey(extDocPm.Id) ? followUpIds[extDocPm.Id] : string.Empty);
            }

            return externalDocumentPMs;
        }

        public bool HaveDocumentsFilingPMsByEntityIdAndDirectionCode(string entityId, string directionCode, int tenant)
        {
            return (from a in repository.context.DocumentsFilings
                   where a.Tenant == tenant && a.EntityId == entityId && a.DirectionCode == directionCode && a.IsDeleted == false
                   select a).Any();
        }

        public IQueryable<DocumentsFilingList> GetIQueryableEntityList(IQueryable<DocumentsFilingsView> iQueryable)
        {


            IQueryable<DocumentsFilingList> result = from a in iQueryable

                                                     select new DocumentsFilingList()
                                                     {
                                                         Id = a.Id,
                                                         DocumentId = a.DocumentId,
                                                         Code = a.Code,
                                                         DirectionCode = a.DirectionCode,
                                                         Description = a.Description,
                                                         CreatedByUserId = a.CreatedByUserId,
                                                         CreateDate = a.CreateDate,
                                                         ObjectTableId = a.ObjectTableId,
                                                         ChildEntityId = a.ChildEntityId,
                                                         ChildObjectTableId = a.ChildObjectTableId,
                                                         ChildEntityReference = a.ChildEntityReference,
                                                         DocumentTypeId = a.DocumentTypeId,
                                                         EntityId = a.EntityId,
                                                         HasCopies = a.HasCopies,
                                                         Notes = a.Notes,
                                                         OwnerId = a.OwnerId,
                                                         SearchFields = a.SearchFields,
                                                         Tenant = a.Tenant,
                                                         Extension = a.Extension,
                                                         HasFile = a.HasFile != null ? a.HasFile.Value : false,
                                                         FileName = a.FileName,
                                                         FileSize = a.FileSize,
                                                         DocumentTypeCode = a.DocumentTypeCode,
                                                         DocumentTypeName = a.DocumentTypeName,
                                                         DoucmentTypeTemplateFormatCode = a.DoucmentTypeTemplateFormatCode,
                                                         IsAgentView = a.IsAgentView,
                                                         IsCustomerView = a.IsCustomerView,
                                                         CreatedByUserName = a.CreatedByUserName,
                                                         Received = a.Received,
                                                         ReceivedDate = a.ReceivedDate,
                                                         ReceivedByUserId = a.ReceivedByUserId,
                                                         StatusCode = a.StatusCode,
                                                         UpdateDate = a.UpdateDate,
                                                         UpdatedByUserId = a.UpdatedByUserId,
                                                         CustomsDocumentTypeCode = a.DocumentTypeCode,
                                                         CustomsDocumentTypeName = a.DocumentTypeName,
                                                         OwnerName = a.OwnerName,
                                                         EntityReference = a.EntityReference,
                                                         ExternalEntityName = a.ExternalEntityName,
                                                         ExternalEntityReference = a.ExternalEntityReference,
                                                         Folder = a.Folder,

                                                         FolderId = a.FolderId,
                                                         IsDeleted = a.IsDeleted,
                                                         DeleteDateTime = a.DeleteDateTime,
                                                         DeletedByUserId = a.DeletedByUserId,
                                                         IsDigitallySigned = a.IsDigitallySigned,
                                                         SignersList = a.SignersList,
                                                         IsSharedWithCustomer = a.IsSharedWithCustomer,
                                                         IsSharedWithForwarder = a.IsSharedWithForwarder,
                                                         CustomerDocumentId = a.CustomerDocumentId,
                                                         ForwarderDocumentId = a.ForwarderDocumentId,
                                                         SecurityId = a.SecurityId,
                                                         CustomerTenantNumber = a.CustomerTenantNumber,
                                                         CustomsDocId = a.CustomsDocId,
                                                         //BackedupExternally = a.BackedupExternally,
                                                     };
            return result;

        }

        public IQueryable<DocumentsFilingList> GetIQueryableEntityList(IQueryable<DocumentsFiling> iQueryable)
        {


            IQueryable<DocumentsFilingList> result = from a in iQueryable.Include("CreatedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")

                                                     select new DocumentsFilingList()
                                                     {
                                                         Id = a.Id,
                                                         DocumentId = a.DocumentId,
                                                         Code = a.Code,
                                                         DirectionCode = a.DirectionCode,
                                                         Description = a.Description,
                                                         CreatedByUserId = a.CreatedByUserId,
                                                         CreateDate = a.CreateDate,
                                                         ObjectTableId = a.ObjectTableId,
                                                         ChildEntityId = a.ChildEntityId,
                                                         ChildObjectTableId = a.ChildObjectTableId,
                                                         ChildEntityReference = a.ChildEntityReference,
                                                         DocumentTypeId = a.DocumentTypeId,
                                                         EntityId = a.EntityId,
                                                         HasCopies = a.HasCopies,
                                                         Notes = a.Notes,
                                                         OwnerId = a.OwnerId,
                                                         SearchFields = a.SearchFields,
                                                         Tenant = a.Tenant,
                                                         Extension = a.Document != null ? a.Document.Extension : null,
                                                         HasFile = a.Document != null ? a.Document.HasFile : false,
                                                         FileName = a.Document != null ? a.Document.FileName : null,
                                                         FileSize = a.Document != null ? a.Document.FileSize : null,
                                                         DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                         DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                         DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                                         IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                                         IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                                         CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                         Received = a.Received,
                                                         ReceivedDate = a.ReceivedDate,
                                                         ReceivedByUserId = a.ReceivedByUserId,
                                                         ReceivedByByContactId = a.ReceivedByByContactId,
                                                         Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                                         StatusCode = a.StatusCode,
                                                         UpdateDate = a.UpdateDate,
                                                         UpdatedByUserId = a.UpdatedByUserId,
                                                         CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                         CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                         OwnerName = a.Owner != null ? a.Owner.Contact.EnglishName : null,
                                                         EntityReference = a.EntityReference,
                                                         ExternalEntityName = a.ExternalEntityName,
                                                         ExternalEntityReference = a.ExternalEntityReference,
                                                         Folder = a.Document != null ? a.Document.Folder : null,

                                                         FolderId = a.FolderId,
                                                         IsDeleted = a.IsDeleted,
                                                         DeleteDateTime = a.DeleteDateTime,
                                                         DeletedByUserId = a.DeletedByUserId,
                                                         IsDigitallySigned = a.IsDigitallySigned,
                                                         SignersList = a.SignersList,
                                                         IsSharedWithCustomer = a.IsSharedWithCustomer,
                                                         IsSharedWithForwarder = a.IsSharedWithForwarder,
                                                         CustomerDocumentId = a.CustomerDocumentId,
                                                         ForwarderDocumentId = a.ForwarderDocumentId,
                                                         SecurityId = a.SecurityId,
                                                         CustomerTenantNumber = a.CustomerTenantNumber,
                                                         SignRequestByUserEmail = a.SignRequestByUserEmail,
                                                         CancellSignRequest = a.CancellSignRequest,
                                                         OrigionalDocumentId = a.OrigionalDocumentId,
                                                         IsRequested = a.IsRequested,
                                                         LastShareDate = a.LastShareDate,
                                                         IsSharedIn = a.IsSharedIn,
                                                         IsSharedOut = a.IsSharedOut,
                                                         SignDueDate = a.SignDueDate,
                                                         IsDigitalSignRequired = a.IsDigitalSignRequired,
                                                         BackedupExternally = a.BackedupExternally,
                                                         ReceivedByPartner = a.ReceivedByPartner,
                                                     };
            return result;
        }

 
        private string AddFiltersToSqlScript(QueryFilterItem filter, string whereClose, string script, string dbms)
        {
            string value = filter.FieldValue.ToString();

            if (string.IsNullOrEmpty(value))
            {
                return whereClose;
            }
            string value2 = null;
            if (filter.FieldValue2 != null)
            {
                value2 = filter.FieldValue2.ToString();
            }
            if (filter.FieldValue.ToString().ToLower() == "true")
            {
                value = "1";
            }
            if (filter.FieldValue.ToString().ToLower() == "false")
            {
                value = "0";
            }
            string prefix = "";
            if (script.Contains("DocumentsFilings." + filter.FieldName))
            {
                prefix = "DocumentsFilings.";
            }
            switch (filter.Operator)
            {
                case "LargerThan":
                    {
                        DateTime datetime;
                        DateTime.TryParse(value, out datetime);
                        if (datetime != null)
                        {
                            value = datetime.ToString("yyyy-MM-dd HH:mm:ss");
                            if (dbms == "oracle")
                            {
                                value = "TO_DATE(" + "'" + value + "'" + ",'YYYY-MM-DD HH24:MI:SS')";

                            }
                            else
                            {
                                value = "convert(datetime," + "'" + value + "')";
                            }
                            whereClose = whereClose + " and " + prefix + filter.FieldName + " > " + value;
                        }
                        else
                        {
                            whereClose = whereClose + " and " + prefix + filter.FieldName + " > '" + value + "'";
                        }

                        break;
                    }
                case "GreaterThanOrEqual":
                    {
                        DateTime datetime;
                        DateTime.TryParse(value, out datetime);
                        if (datetime != null)
                        {
                            value = datetime.ToString("yyyy-MM-dd HH:mm:ss");
                            if (dbms == "oracle")
                            {
                                value = "TO_DATE(" + "'" + value + "'" + ",'YYYY-MM-DD HH24:MI:SS')";
                            }
                            else
                            {
                                value = "convert(datetime," + "'" + value + "')";
                            }
                            whereClose = whereClose + " and " + prefix + filter.FieldName + " >= " + value;
                        }
                        else
                        {
                            whereClose = whereClose + " and " + prefix + filter.FieldName + " >= '" + value + "'";
                        }
                        break;
                    }

                case "LessThan":
                    {
                        DateTime datetime;
                        DateTime.TryParse(value, out datetime);
                        if (datetime != null)
                        {
                            value = datetime.ToString("yyyy-MM-dd HH:mm:ss");
                            if (dbms == "oracle")
                            {
                                value = "TO_DATE(" + "'" + value + "'" + ",'YYYY-MM-DD HH24:MI:SS')";

                            }
                            else
                            {
                                value = "convert(datetime," + "'" + value + "')";
                            }
                            whereClose = whereClose + " and " + prefix + filter.FieldName + " < " + value;
                        }
                        else
                        {
                            whereClose = whereClose + " and " + prefix + filter.FieldName + " < '" + value + "'";
                        }
                        break;
                    }
                case "LessThanOrEqual":
                    {
                        DateTime datetime;
                        DateTime.TryParse(value, out datetime);
                        if (datetime != null)
                        {
                            value = datetime.ToString("yyyy-MM-dd HH:mm:ss");
                            if (dbms == "oracle")
                            {
                                value = "TO_DATE(" + "'" + value + "'" + ",'YYYY-MM-DD HH24:MI:SS')";

                            }
                            else
                            {
                                value = "convert(datetime," + "'" + value + "')";
                            }
                            whereClose = whereClose + " and " + prefix + filter.FieldName + " <= " + value;
                        }
                        else
                        {
                            whereClose = whereClose + " and " + prefix + filter.FieldName + " <= '" + value + "'";
                        }
                        break;
                    }
                case "StartsWith":
                    {

                        whereClose = whereClose + " and " + prefix + filter.FieldName + " like '" + value + "%'";
                        break;
                    }
                case "Contains":
                    {

                        whereClose = whereClose + " and " + prefix + filter.FieldName + " like '%" + value + "%'";
                        break;
                    }

                case "Between":
                    {
                        DateTime datetime;
                        DateTime.TryParse(value, out datetime);
                        if (datetime != null)
                        {
                            value = datetime.ToString("yyyy-MM-dd HH:mm:ss");
                            if (dbms == "oracle")
                            {
                                value = "TO_DATE(" + "'" + value + "'" + ",'YYYY-MM-DD HH24:MI:SS')";

                            }
                            else
                            {
                                value = "convert(datetime," + "'" + value + "')";
                            }

                        }

                        DateTime datetime2;
                        DateTime.TryParse(value2, out datetime2);
                        if (datetime2 != null)
                        {
                            value2 = datetime2.ToString("yyyy-MM-dd HH:mm:ss");
                            if (dbms == "oracle")
                            {
                                value2 = "TO_DATE(" + "'" + value2 + "'" + ",'YYYY-MM-DD HH24:MI:SS')";

                            }
                            else
                            {
                                value2 = "convert(datetime," + "'" + value2 + "')";
                            }

                        }

                        if (datetime != null && datetime2 != null)
                        {
                            whereClose = whereClose + " and " + prefix + filter.FieldName + " > " + value + " and " + prefix + filter.FieldName + " < " + value2;
                        }
                        else
                        {
                            whereClose = whereClose + " and " + prefix + filter.FieldName + " > '" + value + "'" + " and " + prefix + filter.FieldName + " < '" + value2 + "'";
                        }
                        break;
                    }

                case "NotEqual":
                    {

                        whereClose = whereClose + " and " + prefix + filter.FieldName + " <> '" + value + "'";
                        break;


                    }

                default:
                    {
                        whereClose = whereClose + " and " + prefix + filter.FieldName + " = '" + value + "'";
                        break;
                    }
            }
            return whereClose;
        }

        public List<DocumentsFilingPM> GetDocumentsFilingsForRelatedDocuments(string entityId, string childEntityId, string objectTableId, string directionCode, string referenceNumber, List<string> externalEntityReferences, int tenant, string declarationType = null)
        {

            ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);

            List<DocumentsFilingPM> externalDocumentPMs;

            if (string.IsNullOrEmpty(childEntityId))
            {
                (repository.context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 
                
                    externalDocumentPMs = (from a in repository.context.DocumentsFilings
                                       //.Include("CreatedByUser.Contact")
                                       .Include("Document").Include("DocumentType")
                                       //.Include("Owner.Contact")
                                       where a.Tenant == tenant && ((a.EntityId == entityId && a.ObjectTableId == objectTableId && (a.ExternalEntityName != "EFIFILEM" && a.ExternalEntityName != "MFIFILEM")) 
                                       || (a.ExternalEntityName == "CFIFILEM" && a.ExternalEntityReference == referenceNumber) 
                                       || ((a.ExternalEntityName == "EFIFILEM" || a.ExternalEntityName == "MFIFILEM" )&& a.EntityReference == referenceNumber))
                                       && a.DirectionCode == directionCode && a.IsDeleted == false
                                       select new DocumentsFilingPM()
                                       {
                                           Id = a.Id,
                                           DocumentId = a.DocumentId,
                                           Code = a.Code,
                                           DirectionCode = a.DirectionCode,
                                           Description = a.Description,
                                           CreatedByUserId = a.CreatedByUserId,
                                           CreateDate = a.CreateDate,
                                           ObjectTableId = a.ObjectTableId,
                                           ChildEntityId = a.ChildEntityId,
                                           ChildObjectTableId = a.ChildObjectTableId,
                                           ChildEntityReference = a.ChildEntityReference,
                                           DocumentTypeId = a.DocumentTypeId,
                                           EntityId = a.EntityId,
                                           HasCopies = a.HasCopies,
                                           Notes = a.Notes,
                                           OwnerId = a.OwnerId,
                                           SearchFields = a.SearchFields,
                                           Tenant = a.Tenant,
                                           FileExtension = a.Document != null ? a.Document.Extension : null,
                                           HasFile = a.Document != null ? a.Document.HasFile : false,
                                           FileName = a.Document != null ? a.Document.FileName : null,
                                           FileSize = a.Document != null ? a.Document.FileSize : null,
                                           Folder = a.Document != null ? a.Document.Folder : null,
                                           DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                           IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                           IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                           //CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                           Received = a.Received,
                                           ReceivedDate = a.ReceivedDate,
                                           ReceivedByUserId = a.ReceivedByUserId,
                                           ReceivedByByContactId = a.ReceivedByByContactId,
                                           Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                           StatusCode = a.StatusCode,
                                           UpdateDate = a.UpdateDate,
                                           UpdatedByUserId = a.UpdatedByUserId,
                                           CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           //ExternalCode = a.ExternalCode,
                                           EntityReference = a.EntityReference,
                                           ExternalEntityName = a.ExternalEntityName,
                                           ExternalEntityReference = a.ExternalEntityReference,
                                           DepartmentId = a.DepartmentId,
                                           BranchId = a.BranchId,
                                           FolderId = a.FolderId,
                                           IsDeleted = a.IsDeleted,
                                           DeleteDateTime = a.DeleteDateTime,
                                           DeletedByUserId = a.DeletedByUserId,
                                           IsDigitallySigned = a.IsDigitallySigned,
                                           SignersList = a.SignersList,
                                           IsSharedWithCustomer = a.IsSharedWithCustomer,
                                           IsSharedWithForwarder = a.IsSharedWithForwarder,
                                           CustomerDocumentId = a.CustomerDocumentId,
                                           ForwarderDocumentId = a.ForwarderDocumentId,
                                           SecurityId = a.SecurityId,
                                           DocumentCategoryCode = a.DocumentType.DocumentTypeCategoryCode,
 
                                           LastVersion = a.LastVersion,
                                           CustomerTenantNumber = a.CustomerTenantNumber,
                                           IsRequested = a.IsRequested,
                                           //ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                           SignRequestByUserEmail = a.SignRequestByUserEmail,
                                           CancellSignRequest = a.CancellSignRequest,
                                           OrigionalDocumentId = a.OrigionalDocumentId,
                                           LastShareDate = a.LastShareDate,
                                           IsSharedIn = a.IsSharedIn,
                                           IsSharedOut = a.IsSharedOut,
                                           SignDueDate = a.SignDueDate,
                                           IsDigitalSignRequired = a.IsDigitalSignRequired,
                                           BackedupExternally = a.BackedupExternally,                                          

                                           IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                           IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                           IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                           ReceivedByPartner = a.ReceivedByPartner,
										   IsFromCloud = a.IsFromCloud,

									   }).ToList();

            }
            else
            {
                externalDocumentPMs = (from a in repository.context.DocumentsFilings
                                       .Include("Document").Include("DocumentType")                                     
                                       where a.Tenant == tenant && a.EntityId == entityId && a.ChildEntityId == childEntityId && (a.ObjectTableId == objectTableId || a.ExternalEntityReference == referenceNumber)
                                       select new DocumentsFilingPM()
                                       {
                                           Id = a.Id,
                                           DocumentId = a.DocumentId,
                                           Code = a.Code,
                                           DirectionCode = a.DirectionCode,
                                           Description = a.Description,
                                           CreatedByUserId = a.CreatedByUserId,
                                           CreateDate = a.CreateDate,
                                           ObjectTableId = a.ObjectTableId,
                                           ChildEntityId = a.ChildEntityId,
                                           ChildObjectTableId = a.ChildObjectTableId,
                                           ChildEntityReference = a.ChildEntityReference,
                                           DocumentTypeId = a.DocumentTypeId,
                                           EntityId = a.EntityId,
                                           HasCopies = a.HasCopies,
                                           Notes = a.Notes,
                                           OwnerId = a.OwnerId,
                                           SearchFields = a.SearchFields,
                                           Tenant = a.Tenant,
                                           FileExtension = a.Document != null ? a.Document.Extension : null,
                                           HasFile = a.Document != null ? a.Document.HasFile : false,
                                           FileName = a.Document != null ? a.Document.FileName : null,
                                           FileSize = a.Document != null ? a.Document.FileSize : null,
                                           Folder = a.Document != null ? a.Document.Folder : null,
                                           DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                           IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                           IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                           //CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                           Received = a.Received,
                                           ReceivedDate = a.ReceivedDate,
                                           ReceivedByUserId = a.ReceivedByUserId,
                                           ReceivedByByContactId = a.ReceivedByByContactId,
                                           Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                           StatusCode = a.StatusCode,
                                           UpdateDate = a.UpdateDate,
                                           UpdatedByUserId = a.UpdatedByUserId,
                                           CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           //ExternalCode = a.ExternalCode,
                                           EntityReference = a.EntityReference,
                                           ExternalEntityName = a.ExternalEntityName,
                                           ExternalEntityReference = a.ExternalEntityReference,
                                           DepartmentId = a.DepartmentId,
                                           BranchId = a.BranchId,
                                           FolderId = a.FolderId,
                                           IsDeleted = a.IsDeleted,
                                           DeleteDateTime = a.DeleteDateTime,
                                           DeletedByUserId = a.DeletedByUserId,
                                           SignersList = a.SignersList,
                                           IsDigitallySigned = a.IsDigitallySigned,
                                           IsSharedWithCustomer = a.IsSharedWithCustomer,
                                           IsSharedWithForwarder = a.IsSharedWithForwarder,
                                           CustomerDocumentId = a.CustomerDocumentId,
                                           ForwarderDocumentId = a.ForwarderDocumentId,
                                           SecurityId = a.SecurityId,

                                           LastVersion = a.LastVersion,
                                           CustomerTenantNumber = a.CustomerTenantNumber,
                                           IsRequested = a.IsRequested,
                                           //ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                           SignRequestByUserEmail = a.SignRequestByUserEmail,
                                           CancellSignRequest = a.CancellSignRequest,
                                           OrigionalDocumentId = a.OrigionalDocumentId,
                                           LastShareDate = a.LastShareDate,
                                           IsSharedIn = a.IsSharedIn,
                                           IsSharedOut = a.IsSharedOut,
                                           SignDueDate = a.SignDueDate,
                                           IsDigitalSignRequired = a.IsDigitalSignRequired,
                                           BackedupExternally = a.BackedupExternally,

                                           IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                           IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                           IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                           ReceivedByPartner = a.ReceivedByPartner,
										   IsFromCloud = a.IsFromCloud,

									   }).ToList();
            }


            if (externalEntityReferences != null)
            {
                List<DocumentsFilingPM> docs = (from a in repository.context.DocumentsFilings
                                                //.Include("CreatedByUser.Contact")
                                                //.Include("ReceivedByUser.Contact")
                                                .Include("Document").Include("DocumentType")
                                                    //.Include("Owner.Contact")
                                                where a.Tenant == tenant && externalEntityReferences.Any(e => a.ExternalEntityReference.IndexOf(e + ",") == 0 || a.ExternalEntityReference == e)


                                                select new DocumentsFilingPM()
                                                {
                                                    Id = a.Id,
                                                    DocumentId = a.DocumentId,
                                                    Code = a.Code,
                                                    DirectionCode = a.DirectionCode,
                                                    Description = a.Description,
                                                    CreatedByUserId = a.CreatedByUserId,
                                                    CreateDate = a.CreateDate,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ChildEntityId = a.ChildEntityId,
                                                    ChildObjectTableId = a.ChildObjectTableId,
                                                    ChildEntityReference = a.ChildEntityReference,
                                                    DocumentTypeId = a.DocumentTypeId,
                                                    EntityId = a.EntityId,
                                                    HasCopies = a.HasCopies,
                                                    Notes = a.Notes,
                                                    OwnerId = a.OwnerId,
                                                    SearchFields = a.SearchFields,
                                                    Tenant = a.Tenant,
                                                    FileExtension = a.Document != null ? a.Document.Extension : null,
                                                    HasFile = a.Document != null ? a.Document.HasFile : false,
                                                    FileName = a.Document != null ? a.Document.FileName : null,
                                                    FileSize = a.Document != null ? a.Document.FileSize : null,
                                                    Folder = a.Document != null ? a.Document.Folder : null,
                                                    DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                    DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                    DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                                    IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                                    IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                                    //CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                    Received = a.Received,
                                                    ReceivedDate = a.ReceivedDate,
                                                    ReceivedByUserId = a.ReceivedByUserId,
                                                    ReceivedByByContactId = a.ReceivedByByContactId,
                                                    Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                                    StatusCode = a.StatusCode,
                                                    UpdateDate = a.UpdateDate,
                                                    UpdatedByUserId = a.UpdatedByUserId,
                                                    CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                    CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                    //ExternalCode = a.ExternalCode,
                                                    EntityReference = a.EntityReference,
                                                    ExternalEntityName = a.ExternalEntityName,
                                                    ExternalEntityReference = a.ExternalEntityReference,
                                                    DepartmentId = a.DepartmentId,
                                                    BranchId = a.BranchId,
                                                    FolderId = a.FolderId,
                                                    IsDeleted = a.IsDeleted,
                                                    DeleteDateTime = a.DeleteDateTime,
                                                    DeletedByUserId = a.DeletedByUserId,
                                                    SignersList = a.SignersList,
                                                    IsDigitallySigned = a.IsDigitallySigned,
                                                    IsSharedWithCustomer = a.IsSharedWithCustomer,
                                                    IsSharedWithForwarder = a.IsSharedWithForwarder,
                                                    CustomerDocumentId = a.CustomerDocumentId,
                                                    ForwarderDocumentId = a.ForwarderDocumentId,
                                                    SecurityId = a.SecurityId,

                                                    LastVersion = a.LastVersion,
                                                    CustomerTenantNumber = a.CustomerTenantNumber,
                                                    IsRequested = a.IsRequested,
                                                    //ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                                    SignRequestByUserEmail = a.SignRequestByUserEmail,
                                                    CancellSignRequest = a.CancellSignRequest,
                                                    OrigionalDocumentId = a.OrigionalDocumentId,
                                                    IsDigitalSignRequired = a.IsDigitalSignRequired,
                                                    BackedupExternally = a.BackedupExternally,
 
                                                    IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                                    IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                                    IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                                    ReceivedByPartner = a.ReceivedByPartner,
													IsFromCloud = a.IsFromCloud,

												}).ToList();
 
                externalDocumentPMs = externalDocumentPMs.Concat(docs).ToList();


                if (declarationType == "E") { 
                   externalDocumentPMs = externalDocumentPMs.GroupBy(x => x.Id).Select(x => x.First()).ToList();
                }


            }

            ICustomsDocumentQueryServiceExt customsDocumentQueryService = ContainerAccessor.Container.Resolve(typeof(ICustomsDocumentQueryServiceExt), "CustomsDocumentQueryServiceExt", new ParameterOverride("", 1)) as ICustomsDocumentQueryServiceExt;
             FollowUpRepository followUpRepository = new FollowUpRepository(tenant);
            List<FollowUp> FollowUps=null;

       
            var resMode = new { DefaultValue = "" };

            if (directionCode!="E")
            {
                FollowUps = followUpRepository.GetFollowUps(tenant).ToList();

            }
             var followUpIds = new FollowUpRepository(tenant).GetFollowUpIdByDocumentsFilingIds(tenant, externalDocumentPMs.Select(x => x.Id).ToArray());
 
            DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);

            foreach (DocumentsFilingPM extDocPm in externalDocumentPMs)
            {
                List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValuesList = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList(); //documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByTenant(tenant).ToList();
                SetDocumentFollowUp(extDocPm, followUpIds.ContainsKey(extDocPm.Id) ? followUpIds[extDocPm.Id] : string.Empty);

                CustomsDocumentPM customsDoc = customsDocumentQueryService.GetSingleByDocFileId(extDocPm.Id,tenant);
                if (customsDoc != null)
                {
                    extDocPm.CustomsDocumentTypeName = customsDoc.DocumentTypeName;
                    extDocPm.CustomsDocumentTypeCode = customsDoc.DocumentTypeCode;
                    extDocPm.IsMetaDataReady = customsDoc.IsMetaDataReady;
                    extDocPm.CustomsDocumentStatusCode = customsDoc.DocumentStatusCode;
                    extDocPm.ExternalAttachmentId = customsDoc.ExternalAttachmentId;
                    extDocPm.OcrStatusCode = customsDoc.OcrStatusCode;
                    extDocPm.OcrScore = customsDoc.OcrScore;
                    extDocPm.OcrReference = customsDoc.OcrReference;
                    extDocPm.OcrNotConnect = customsDoc.OcrNotConnect;

                }

                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValuesList;//.Where(d => d.DocumentsFilingId == extDocPm.Id && d.Tenant == extDocPm.Tenant).ToList();
            }


            return externalDocumentPMs.Where(d => d.HasFile).ToList();
        }
        public List<DocumentsFilingPM> GetDocumentsFilingListByDocumentType(string documentTypeId, string objectTableId, string entityId, int tenant)
        {
            var extDocPms = (from a in repository.context.DocumentsFilings.Include("ReceivedByUser.Contact").Include("CreatedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                          where a.DocumentTypeId == documentTypeId && a.Tenant == tenant && a.ObjectTableId == objectTableId && a.EntityId == entityId && a.IsDeleted == false

                                          select new DocumentsFilingPM()
                                          {
                                              Id = a.Id,
                                              DocumentId = a.DocumentId,
                                              Code = a.Code,
                                              DirectionCode = a.DirectionCode,
                                              Description = a.Description,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreateDate = a.CreateDate,
                                              ObjectTableId = a.ObjectTableId,
                                              ChildEntityId = a.ChildEntityId,
                                              ChildObjectTableId = a.ChildObjectTableId,
                                              ChildEntityReference = a.ChildEntityReference,
                                              DocumentTypeId = a.DocumentTypeId,
                                              EntityId = a.EntityId,
                                              HasCopies = a.HasCopies,
                                              Notes = a.Notes,
                                              OwnerId = a.OwnerId,
                                              SearchFields = a.SearchFields,
                                              Tenant = a.Tenant,
                                              FileExtension = a.Document != null ? a.Document.Extension : null,
                                              HasFile = a.Document != null ? a.Document.HasFile : false,
                                              FileName = a.Document != null ? a.Document.FileName : null,
                                              FileSize = a.Document != null ? a.Document.FileSize : null,
                                              Folder = a.Document != null ? a.Document.Folder : null,
                                              DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                              IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                              IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                              CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                              Received = a.Received,
                                              ReceivedDate = a.ReceivedDate,
                                              ReceivedByUserId = a.ReceivedByUserId,
                                              Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                              StatusCode = a.StatusCode,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              EntityReference = a.EntityReference,
                                              ExternalEntityName = a.ExternalEntityName,
                                              ExternalEntityReference = a.ExternalEntityReference,
                                              DepartmentId = a.DepartmentId,
                                              BranchId = a.BranchId,
                                              FolderId = a.FolderId,
                                              IsDeleted = a.IsDeleted,
                                              DeleteDateTime = a.DeleteDateTime,
                                              DeletedByUserId = a.DeletedByUserId,
                                              IsDigitallySigned = a.IsDigitallySigned,
                                              IsSharedWithCustomer = a.IsSharedWithCustomer,
                                              IsSharedWithForwarder = a.IsSharedWithForwarder,
                                              CustomerDocumentId = a.CustomerDocumentId,
                                              ForwarderDocumentId = a.ForwarderDocumentId,
                                              SecurityId = a.SecurityId,

                                              LastVersion = a.LastVersion,
                                              CustomerTenantNumber = a.CustomerTenantNumber,
                                              IsRequested = a.IsRequested,
                                              ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                              SignRequestByUserEmail = a.SignRequestByUserEmail,
                                              CancellSignRequest = a.CancellSignRequest,
                                              OrigionalDocumentId = a.OrigionalDocumentId,
                                              LastShareDate = a.LastShareDate,
                                              IsSharedIn = a.IsSharedIn,
                                              IsSharedOut = a.IsSharedOut,
                                              SignDueDate = a.SignDueDate,
                                              IsDigitalSignRequired = a.IsDigitalSignRequired,
                                              BackedupExternally = a.BackedupExternally,
											  IsFromCloud = a.IsFromCloud,

										  }).ToList();
            FollowUpRepository followUpRepository = new FollowUpRepository(tenant);
            List<FollowUp> FollowUps = followUpRepository.GetFollowUps(tenant).ToList();
            foreach (DocumentsFilingPM extDocPm in extDocPms)
            {
                if (FollowUps != null)
                {
                    List<FollowUp> docFollowUp = FollowUps.Where(d => d.DocumentsFilingId == extDocPm.Id && d.Tenant == extDocPm.Tenant).ToList();
                    if (docFollowUp.Count != 0)
                    {
                        extDocPm.FollowUpCount = docFollowUp.Count;
                        extDocPm.FollowUpId = docFollowUp.FirstOrDefault().Id;
                        extDocPm.HasFollowUp = docFollowUp.Any();
                    }
                }
                DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            }
            return extDocPms;
        }

        public DocumentsFilingPM GetDocumentsFilingByDocumentType(string documentTypeId, string objectTableId, string entityId, int tenant)
        {
            DocumentsFilingPM extDocPm = (from a in repository.context.DocumentsFilings.Include("ReceivedByUser.Contact").Include("CreatedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                          where a.DocumentTypeId == documentTypeId && a.Tenant == tenant && a.ObjectTableId == objectTableId && a.EntityId == entityId && a.IsDeleted == false

                                          select new DocumentsFilingPM()
                                          {
                                              Id = a.Id,
                                              DocumentId = a.DocumentId,
                                              Code = a.Code,
                                              DirectionCode = a.DirectionCode,
                                              Description = a.Description,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreateDate = a.CreateDate,
                                              ObjectTableId = a.ObjectTableId,
                                              ChildEntityId = a.ChildEntityId,
                                              ChildObjectTableId = a.ChildObjectTableId,
                                              ChildEntityReference = a.ChildEntityReference,
                                              DocumentTypeId = a.DocumentTypeId,
                                              EntityId = a.EntityId,
                                              HasCopies = a.HasCopies,
                                              Notes = a.Notes,
                                              OwnerId = a.OwnerId,
                                              SearchFields = a.SearchFields,
                                              Tenant = a.Tenant,
                                              FileExtension = a.Document != null ? a.Document.Extension : null,
                                              HasFile = a.Document != null ? a.Document.HasFile : false,
                                              FileName = a.Document != null ? a.Document.FileName : null,
                                              FileSize = a.Document != null ? a.Document.FileSize : null,
                                              Folder = a.Document != null ? a.Document.Folder : null,
                                              DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                              IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                              IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                              CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                              Received = a.Received,
                                              ReceivedDate = a.ReceivedDate,
                                              ReceivedByUserId = a.ReceivedByUserId,
                                              ReceivedByByContactId = a.ReceivedByByContactId,
                                              Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                              StatusCode = a.StatusCode,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              EntityReference = a.EntityReference,
                                              ExternalEntityName = a.ExternalEntityName,
                                              ExternalEntityReference = a.ExternalEntityReference,
                                              DepartmentId = a.DepartmentId,
                                              BranchId = a.BranchId,
                                              FolderId = a.FolderId,
                                              IsDeleted = a.IsDeleted,
                                              DeleteDateTime = a.DeleteDateTime,
                                              DeletedByUserId = a.DeletedByUserId,
                                              IsDigitallySigned = a.IsDigitallySigned,
                                              IsSharedWithCustomer = a.IsSharedWithCustomer,
                                              IsSharedWithForwarder = a.IsSharedWithForwarder,
                                              CustomerDocumentId = a.CustomerDocumentId,
                                              ForwarderDocumentId = a.ForwarderDocumentId,
                                              SecurityId = a.SecurityId,

                                              LastVersion = a.LastVersion,
                                              CustomerTenantNumber = a.CustomerTenantNumber,
                                              IsRequested = a.IsRequested,
                                              ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                              SignRequestByUserEmail = a.SignRequestByUserEmail,
                                              CancellSignRequest = a.CancellSignRequest,
                                              OrigionalDocumentId = a.OrigionalDocumentId,
                                              LastShareDate = a.LastShareDate,
                                              IsSharedIn = a.IsSharedIn,
                                              IsSharedOut = a.IsSharedOut,
                                              SignDueDate = a.SignDueDate,
                                              IsDigitalSignRequired = a.IsDigitalSignRequired,
                                              BackedupExternally = a.BackedupExternally,

                                              IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                              IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                              IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                              ReceivedByPartner = a.ReceivedByPartner,
											  IsFromCloud = a.IsFromCloud,

										  }).FirstOrDefault();

            if (extDocPm != null)
            {
                SetDocumentFollowUp(extDocPm);
                DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            }

            return extDocPm;
        }
        public DocumentsFilingPM GetDocumentsFilingByChild(string documentTypeId, string paymentNumber, int tenant)
        {
            DocumentsFilingPM extDocPm = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                          where a.DocumentTypeId == documentTypeId && a.Tenant == tenant && a.ChildEntityReference == paymentNumber && a.IsDeleted == false

                                          select new DocumentsFilingPM()
                                          {
                                              Id = a.Id,
                                              DocumentId = a.DocumentId,
                                              Code = a.Code,
                                              DirectionCode = a.DirectionCode,
                                              Description = a.Description,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreateDate = a.CreateDate,
                                              ObjectTableId = a.ObjectTableId,
                                              ChildEntityId = a.ChildEntityId,
                                              ChildObjectTableId = a.ChildObjectTableId,
                                              ChildEntityReference = a.ChildEntityReference,
                                              DocumentTypeId = a.DocumentTypeId,
                                              EntityId = a.EntityId,
                                              HasCopies = a.HasCopies,
                                              Notes = a.Notes,
                                              OwnerId = a.OwnerId,
                                              SearchFields = a.SearchFields,
                                              Tenant = a.Tenant,
                                              FileExtension = a.Document != null ? a.Document.Extension : null,
                                              HasFile = a.Document != null ? a.Document.HasFile : false,
                                              FileName = a.Document != null ? a.Document.FileName : null,
                                              FileSize = a.Document != null ? a.Document.FileSize : null,
                                              Folder = a.Document != null ? a.Document.Folder : null,
                                              DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                              IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                              IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                              CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                              Received = a.Received,
                                              ReceivedDate = a.ReceivedDate,
                                              ReceivedByUserId = a.ReceivedByUserId,
                                              ReceivedByByContactId = a.ReceivedByByContactId,
                                              Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                              StatusCode = a.StatusCode,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              EntityReference = a.EntityReference,
                                              ExternalEntityName = a.ExternalEntityName,
                                              ExternalEntityReference = a.ExternalEntityReference,
                                              DepartmentId = a.DepartmentId,
                                              BranchId = a.BranchId,
                                              FolderId = a.FolderId,
                                              IsDeleted = a.IsDeleted,
                                              DeleteDateTime = a.DeleteDateTime,
                                              DeletedByUserId = a.DeletedByUserId,
                                              IsDigitallySigned = a.IsDigitallySigned,
                                              IsSharedWithCustomer = a.IsSharedWithCustomer,
                                              IsSharedWithForwarder = a.IsSharedWithForwarder,
                                              CustomerDocumentId = a.CustomerDocumentId,
                                              ForwarderDocumentId = a.ForwarderDocumentId,
                                              LastVersion = a.LastVersion,
                                              IsRequested = a.IsRequested,
                                              ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                              SignRequestByUserEmail = a.SignRequestByUserEmail,
                                              CancellSignRequest = a.CancellSignRequest,
                                              OrigionalDocumentId = a.OrigionalDocumentId,
                                              LastShareDate = a.LastShareDate,
                                              IsSharedIn = a.IsSharedIn,
                                              IsSharedOut = a.IsSharedOut,
                                              SignDueDate = a.SignDueDate,
                                              IsDigitalSignRequired = a.IsDigitalSignRequired,
                                              BackedupExternally = a.BackedupExternally,

                                              IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                              IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                              IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                              ReceivedByPartner = a.ReceivedByPartner,
											  IsFromCloud = a.IsFromCloud,

										  }).FirstOrDefault();

            if (extDocPm != null)
            {
                SetDocumentFollowUp(extDocPm);
                DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            }

            return extDocPm;
        }

        public IQueryable<DocumentsFiling> GetByexternalentityreference(string externalentityname, string externalentityreference, int tenant)
        {
            (repository.context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 
            return (from a in repository.context.DocumentsFilings
                    where a.ExternalEntityReference == externalentityreference && a.ExternalEntityName == externalentityname && a.Tenant == tenant && a.IsDeleted == false
                    select a);


                                          
        }


        public DocumentsFilingPM GetDocumentsFilingByChildId(string documentTypeId, string childId, string childObjectTableId, string childEntityReference, int tenant)
        {
            DocumentsFilingPM extDocPm = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                          where a.DocumentTypeId == documentTypeId && a.Tenant == tenant && a.ChildEntityId == childId && a.ChildObjectTableId == childObjectTableId && a.ChildEntityReference == childEntityReference
                                          && a.IsDeleted == false

                                          select new DocumentsFilingPM()
                                          {
                                              Id = a.Id,
                                              DocumentId = a.DocumentId,
                                              Code = a.Code,
                                              DirectionCode = a.DirectionCode,
                                              Description = a.Description,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreateDate = a.CreateDate,
                                              ObjectTableId = a.ObjectTableId,
                                              ChildEntityId = a.ChildEntityId,
                                              ChildObjectTableId = a.ChildObjectTableId,
                                              ChildEntityReference = a.ChildEntityReference,
                                              DocumentTypeId = a.DocumentTypeId,
                                              EntityId = a.EntityId,
                                              HasCopies = a.HasCopies,
                                              Notes = a.Notes,
                                              OwnerId = a.OwnerId,
                                              SearchFields = a.SearchFields,
                                              Tenant = a.Tenant,
                                              FileExtension = a.Document != null ? a.Document.Extension : null,
                                              HasFile = a.Document != null ? a.Document.HasFile : false,
                                              FileName = a.Document != null ? a.Document.FileName : null,
                                              FileSize = a.Document != null ? a.Document.FileSize : null,
                                              Folder = a.Document != null ? a.Document.Folder : null,
                                              DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                              IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                              IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                              CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                              Received = a.Received,
                                              ReceivedDate = a.ReceivedDate,
                                              ReceivedByUserId = a.ReceivedByUserId,
                                              ReceivedByByContactId = a.ReceivedByByContactId,
                                              Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                              StatusCode = a.StatusCode,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              EntityReference = a.EntityReference,
                                              ExternalEntityName = a.ExternalEntityName,
                                              ExternalEntityReference = a.ExternalEntityReference,
                                              DepartmentId = a.DepartmentId,
                                              BranchId = a.BranchId,
                                              FolderId = a.FolderId,
                                              IsDeleted = a.IsDeleted,
                                              DeleteDateTime = a.DeleteDateTime,
                                              DeletedByUserId = a.DeletedByUserId,
                                              IsDigitallySigned = a.IsDigitallySigned,
                                              IsSharedWithCustomer = a.IsSharedWithCustomer,
                                              IsSharedWithForwarder = a.IsSharedWithForwarder,
                                              CustomerDocumentId = a.CustomerDocumentId,
                                              ForwarderDocumentId = a.ForwarderDocumentId,
                                              SecurityId = a.SecurityId,

                                              LastVersion = a.LastVersion,
                                              CustomerTenantNumber = a.CustomerTenantNumber,
                                              IsRequested = a.IsRequested,
                                              ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                              SignRequestByUserEmail = a.SignRequestByUserEmail,
                                              CancellSignRequest = a.CancellSignRequest,
                                              OrigionalDocumentId = a.OrigionalDocumentId,
                                              LastShareDate = a.LastShareDate,
                                              IsSharedIn = a.IsSharedIn,
                                              IsSharedOut = a.IsSharedOut,
                                              SignDueDate = a.SignDueDate,
                                              IsDigitalSignRequired = a.IsDigitalSignRequired,
                                              BackedupExternally = a.BackedupExternally,

                                              IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                              IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                              IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                              ReceivedByPartner = a.ReceivedByPartner,
											  IsFromCloud = a.IsFromCloud,

										  }).FirstOrDefault();

            if (extDocPm != null)
            {
                SetDocumentFollowUp(extDocPm);
                DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList();
            }

            return extDocPm;
        }


        public bool CheckMissingDocForEntity(string entityId, string objectTableId, int tenant)
        {
            //ObjectTabelRepository objectTableRep = new ObjectTabelRepository(tenant);
            //ObjectTablePM shipmentObject = null;
            //string shipmentObjectId = null;
            //if (objectTableRep.IsObjectTableShipment(objectTableId))
            //{
            //    shipmentObject = ObjectTabelQuery.GetObjectTableByCode("Shipment", tenant);
            //    shipmentObjectId = shipmentObject.Id;
            //}
            ////DocumentsFilingPM externalDocumentPMs;
            //var temp = (from a in repository.context.DocumentsFilings.Include("Document")
            //            where a.Tenant == tenant && a.EntityId == entityId && (a.ObjectTableId == objectTableId || a.ObjectTableId == shipmentObjectId) && ((a.DocumentType.Code == "740" || a.DocumentType.Code == "706" || a.DocumentType.Code == "380") && a.Document.HasFile == false)
            //            select a);
            //if (temp.Count() > 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            //return temp.Any();
            int count = GetMissingDocCountForEntity(entityId, objectTableId, tenant);
            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            //return externalDocumentPMs;


        }

        public int GetMissingDocCountForEntity(string entityId, string objectTableId, int tenant, bool IsArchived = true)
        {

            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);

            var IsShipmentArchived = IsArchived;
            if (IsShipmentArchived)
            {
                IsShipmentArchived = shipmentRepository.IsShipmentArchived(entityId, tenant);
            }
            if (IsShipmentArchived)
            {
                return 0;
            }
            else
            {
                ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);
                ObjectTablePM shipmentObject = null;
                string shipmentObjectId = null;
                if (objectTableRep.IsObjectTableShipment(objectTableId))
                {
                    shipmentObject = ObjectTableQuery.GetObjectTableByCode("Shipment", tenant);
                    shipmentObjectId = shipmentObject.Id;
                }
                var temp = (from a in repository.context.DocumentsFilings.Include("Document")
                            where a.Tenant == tenant && a.EntityId == entityId && (a.ObjectTableId == objectTableId || a.ObjectTableId == shipmentObjectId) && a.Document.HasFile == false && !a.IsDeleted//((a.DocumentType.Code == "740" || a.DocumentType.Code == "706" || a.DocumentType.Code == "380") &&
                            select a);
                return temp.Count();

            }



        }

        public string GetDirectionForEntity(string entityId, string objectTableId, int tenant)
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            return shipmentRepository.getShipmentDirection(entityId, tenant);
        }

        public bool GetIfSignRequiredForEntity(string entityId, int tenant)
        {


            var temp = (from a in repository.context.DocumentsFilings.Include("Document")
                        where a.Tenant == tenant && a.EntityId == entityId && a.IsDigitalSignRequired == true && !a.IsDeleted && a.Document.HasFile
                        select a);

            if (temp.Count() > 0) // so we ignore the current document
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool GetIfSignRequiredForEntity(string entityId, int tenant,bool CurrentDocSignReqStatus)
        {


            var temp = (from a in repository.context.DocumentsFilings.Include("Document")
                        where a.Tenant == tenant && a.EntityId == entityId && a.IsDigitalSignRequired == true && !a.IsDeleted && a.Document.HasFile
                        select a);

            if (temp.Count() > 1) // so we ignore the current document
            {
                return true;
            }
            else if (CurrentDocSignReqStatus == true)
            {
                return true;
            }
            else {
                return false;
            }

        }

        public bool GetIfIsRequestedForEntity(string entityId, int tenant,DocumentsFilingPM entityPM = null)
        {

            IQueryable<DocumentsFiling> temp = null;
            if (entityPM != null && entityPM.IsDeleted)
            {
                temp = (from a in repository.context.DocumentsFilings.Include("Document")
                        where a.Tenant == tenant && a.EntityId == entityId && a.IsRequested == true && !a.IsDeleted && a.Id != entityPM.Id
                        select a);
            }
            else
            {
                temp = (from a in repository.context.DocumentsFilings.Include("Document")
                        where a.Tenant == tenant && a.EntityId == entityId && a.IsRequested == true && !a.IsDeleted
                        select a);
            }
            //var temp = (from a in repository.context.DocumentsFilings.Include("Document")
            //            where a.Tenant == tenant && a.EntityId == entityId && a.IsRequested == true && !a.IsDeleted
            //            select a);

            if (temp.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public int GetRequestedDocCountForEntity(string entityId, int tenant,DocumentsFilingPM entityPM = null)
        {
            IQueryable<DocumentsFiling> temp = null;
            if (entityPM != null && entityPM.IsDeleted)
            {
                temp = (from a in repository.context.DocumentsFilings.Include("Document")
                            where a.Tenant == tenant && a.EntityId == entityId && a.IsRequested == true && !a.IsDeleted && a.Id != entityPM.Id
                            select a);
            }
            else
            {
                temp = (from a in repository.context.DocumentsFilings.Include("Document")
                            where a.Tenant == tenant && a.EntityId == entityId && a.IsRequested == true && !a.IsDeleted
                            select a);
            }
            
            return temp.Count();
        }


        public string GetMissingDocsNamesForEntity(string entityId, string objectTableId, int tenant, bool IsArchived = true)
        {
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            var IsShipmentArchived = IsArchived;
            if (IsShipmentArchived)
            {
                IsShipmentArchived = shipmentRepository.IsShipmentArchived(entityId, tenant);
            }


            if (IsShipmentArchived)
            {
                return "";
            }
            else
            {
                ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);
                ObjectTablePM shipmentObject = null;
                string shipmentObjectId = null;
                if (objectTableRep.IsObjectTableShipment(objectTableId))
                {
                    shipmentObject = ObjectTableQuery.GetObjectTableByCode("Shipment", tenant);
                    shipmentObjectId = shipmentObject.Id;
                }

                var temp = (from a in repository.context.DocumentsFilings.Include("Document")
                            where a.Tenant == tenant && a.EntityId == entityId && (a.ObjectTableId == objectTableId || a.ObjectTableId == shipmentObjectId) && a.Document.HasFile == false //((a.DocumentType.Code == "740" || a.DocumentType.Code == "706" || a.DocumentType.Code == "380") &&
                            select a.DocumentType.Name);
                return string.Join(",", temp);

            }

        }

        public List<string> GetDocumentsFilingPMsIdsByEntityId(string entityId, string directionCode, int tenant)
        {
            List<string> externalDocumentPMs = new List<string>();
            externalDocumentPMs = (from a in repository.context.DocumentsFilings
                                   where a.Tenant == tenant && a.EntityId == entityId && a.DirectionCode == directionCode && (a.Document != null ? a.Document.HasFile == true : false)
                                   select a.Id).ToList();

            return externalDocumentPMs;
        }

        public List<DocumentsFilingPM> GetInputDocumentsFilingPMsByEntityId(string entityId, int tenant)
        {
            List<DocumentsFilingPM> documentsFilingPMs = GetDocumentFilingPMs(entityId, tenant);

            CustomReferenceDocumentsMetaDataType customReferenceDocumentsMeta = BuildCustomerReferenceDocumentsMetaDataType(tenant);
          
            if (customReferenceDocumentsMeta.DREL != null && customReferenceDocumentsMeta.CREF != null)
                MapCustomReferenceForDocumentFilings(tenant, documentsFilingPMs, customReferenceDocumentsMeta);
            
            return documentsFilingPMs;
        }

        private static CustomReferenceDocumentsMetaDataType BuildCustomerReferenceDocumentsMetaDataType(int tenant)
        {
            DocumentsMetaDataTypeRepository TypesRepo = new DocumentsMetaDataTypeRepository(tenant);
            CustomReferenceDocumentsMetaDataType customReferenceDocumentsMeta = new CustomReferenceDocumentsMetaDataType();
            customReferenceDocumentsMeta.DREL = TypesRepo.GetSingleDocumentsMetaDataTypeByCode("DREL", tenant);
            customReferenceDocumentsMeta.CREF = TypesRepo.GetSingleDocumentsMetaDataTypeByCode("CREF", tenant);
            return customReferenceDocumentsMeta;
        }

        private static void MapCustomReferenceForDocumentFilings(int tenant, List<DocumentsFilingPM> DocumentsFilingPMs, CustomReferenceDocumentsMetaDataType customReferenceDocumentsMeta)
        {
            foreach (DocumentsFilingPM documentsFilingPM in DocumentsFilingPMs)
                MapSingleDocumentFilingCustomsReference(tenant, customReferenceDocumentsMeta, documentsFilingPM);
        }

        private static void MapSingleDocumentFilingCustomsReference(int tenant, CustomReferenceDocumentsMetaDataType metaDataType, DocumentsFilingPM documentsFilingPM)
        {
            DocumentsFilingMetaDataValuePM drelMetaDataValue = GetDRELMetaDataValueOfDocumentFiling(tenant, metaDataType, documentsFilingPM);
            if (drelMetaDataValue != null)
            {
                DocumentsFilingMetaDataValuePM crefMetaDataValue = GetCREFMetaDataValueOfDocumentFiling(tenant, metaDataType, documentsFilingPM);
                if (crefMetaDataValue != null && !string.IsNullOrEmpty(crefMetaDataValue.MetaDataValue) && drelMetaDataValue.MetaDataValue.ToLower() == "true")
                {
                    documentsFilingPM.IsCustomReference = true;
                    documentsFilingPM.CustomReference = crefMetaDataValue.MetaDataValue;
                }
            }
        }

        private static DocumentsFilingMetaDataValuePM GetCREFMetaDataValueOfDocumentFiling(int tenant, CustomReferenceDocumentsMetaDataType metaDataType, DocumentsFilingPM documentsFilingPM)
        {
            List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValuesList = GetDocumentsFilingMetaDataValuesList(tenant, documentsFilingPM);
            var crefMetaDataValue = documentsFilingMetaDataValuesList.Where(a => a.DocumentsMetaDataTypeId == metaDataType.CREF.Id).FirstOrDefault();
            return crefMetaDataValue;
        }

        private static DocumentsFilingMetaDataValuePM GetDRELMetaDataValueOfDocumentFiling(int tenant, CustomReferenceDocumentsMetaDataType metaDataType, DocumentsFilingPM documentsFilingPM)
        {
            List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValuesList = GetDocumentsFilingMetaDataValuesList(tenant, documentsFilingPM);
            var drelMetaDataValue = documentsFilingMetaDataValuesList.Where(a => a.DocumentsMetaDataTypeId == metaDataType.DREL.Id).FirstOrDefault();
            return drelMetaDataValue;
        }

        private static List<DocumentsFilingMetaDataValuePM> GetDocumentsFilingMetaDataValuesList(int tenant, DocumentsFilingPM extDocPm)
        {
            DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
            List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValuesList = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList(); //documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByTenant(tenant).ToList();
            return documentsFilingMetaDataValuesList;
        }

        private List<DocumentsFilingPM> GetDocumentFilingPMs(string entityId, int tenant)
        {
            return (from a in repository.context.DocumentsFilings.Include("DocumentType")
                    where a.Tenant == tenant && a.EntityId == entityId && a.IsDeleted == false && a.DirectionCode == "I" && a.DocumentId != null && a.DocumentType.IsCustomerView == true
                    select new DocumentsFilingPM()
                    {
                        Id = a.Id,
                        BillToId = a.BillToId,
                        DocumentId = a.DocumentId,
                        Code = a.Code,
                        DirectionCode = a.DirectionCode,
                        Description = a.Description,
                        CreatedByUserId = a.CreatedByUserId,
                        CreateDate = a.CreateDate,
                        ObjectTableId = a.ObjectTableId,
                        ChildEntityId = a.ChildEntityId,
                        ChildObjectTableId = a.ChildObjectTableId,
                        ChildEntityReference = a.ChildEntityReference,
                        DocumentTypeId = a.DocumentTypeId,
                        DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                        DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                        DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                        EntityId = a.EntityId,
                        HasCopies = a.HasCopies,
                        Notes = a.Notes,
                        OwnerId = a.OwnerId,
                        OwnerName = a.Owner != null ? a.Owner.Contact.EnglishName : null,
                        SearchFields = a.SearchFields,
                        Tenant = a.Tenant,
                        FileExtension = a.Document != null ? a.Document.Extension : null,
                        HasFile = a.Document != null ? a.Document.HasFile : false,
                        FileName = a.Document != null ? a.Document.FileName : null,
                        FileSize = a.Document != null ? a.Document.FileSize : null,
                        Folder = a.Document != null ? a.Document.Folder : null,

                        IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                        IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                        CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                        Received = a.Received,
                        ReceivedDate = a.ReceivedDate,
                        ReceivedByUserId = a.ReceivedByUserId,
                        ReceivedByByContactId = a.ReceivedByByContactId,
                        Name = a.DocumentType != null ? a.DocumentType.Name : null,
                        StatusCode = a.StatusCode,
                        UpdateDate = a.UpdateDate,
                        UpdatedByUserId = a.UpdatedByUserId,
                        CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                        CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                        ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,

                        EntityReference = a.EntityReference,
                        ExternalEntityName = a.ExternalEntityName,
                        ExternalEntityReference = a.ExternalEntityReference,
                        DepartmentId = a.DepartmentId,
                        BranchId = a.BranchId,
                        FolderId = a.FolderId,
                        IsDeleted = a.IsDeleted,
                        DeleteDateTime = a.DeleteDateTime,
                        DeletedByUserId = a.DeletedByUserId,
                        IsDigitallySigned = a.IsDigitallySigned,
                        SignersList = a.SignersList,
                        IsSharedWithCustomer = a.IsSharedWithCustomer,
                        IsSharedWithForwarder = a.IsSharedWithForwarder,
                        CustomerDocumentId = a.CustomerDocumentId,
                        ForwarderDocumentId = a.ForwarderDocumentId,
                        SecurityId = a.SecurityId,
                        CustomerTenantNumber = a.CustomerTenantNumber,
                        IsRequested = a.IsRequested,
                        SignRequestByUserEmail = a.SignRequestByUserEmail,
                        CancellSignRequest = a.CancellSignRequest,
                        OrigionalDocumentId = a.OrigionalDocumentId,
                        CalculatedFileName = a.Document != null ? !string.IsNullOrEmpty(a.Document.CalculatedFileName) ? a.Document.CalculatedFileName : a.Document.FileName : "",
                        LastShareDate = a.LastShareDate,
                        IsSharedIn = a.IsSharedIn,
                        IsSharedOut = a.IsSharedOut,
                        SignDueDate = a.SignDueDate,
                        IsDigitalSignRequired = a.IsDigitalSignRequired,
                        BackedupExternally = a.BackedupExternally,

                        IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                        IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                        IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                        ReceivedByPartner = a.ReceivedByPartner,
                        IsFromCloud = a.IsFromCloud,
                    }).ToList();
        }

        public List<DocumentsFilingPM> GetDocumentsFilingPMsByEntityId(string entityId, int tenant)
        {
            IQueryable<DocumentsFilingPM> result = from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                                   where a.Tenant == tenant && a.EntityId == entityId && a.IsDeleted == false
                                                   select new DocumentsFilingPM()
                                                   {
                                                       Id = a.Id,
                                                       DocumentId = a.DocumentId,
                                                       Code = a.Code,
                                                       DirectionCode = a.DirectionCode,
                                                       Description = a.Description,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       CreateDate = a.CreateDate,
                                                       ObjectTableId = a.ObjectTableId,
                                                       ChildEntityId = a.ChildEntityId,
                                                       ChildObjectTableId = a.ChildObjectTableId,
                                                       ChildEntityReference = a.ChildEntityReference,
                                                       DocumentTypeId = a.DocumentTypeId,
                                                       DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                       DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                       IsCustomerUploadPermission = a.DocumentType != null ? a.DocumentType.IsCustomerUploadPermission : false,
                                                       DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                                       EntityId = a.EntityId,
                                                       HasCopies = a.HasCopies,
                                                       Notes = a.Notes,
                                                       OwnerId = a.OwnerId,
                                                       OwnerName = a.Owner != null ? a.Owner.Contact.EnglishName : null,
                                                       SearchFields = a.SearchFields,
                                                       Tenant = a.Tenant,
                                                       FileExtension = a.Document != null ? a.Document.Extension : null,
                                                       HasFile = a.Document != null ? a.Document.HasFile : false,
                                                       FileName = a.Document != null ? a.Document.FileName : null,
                                                       FileSize = a.Document != null ? a.Document.FileSize : null,
                                                       Folder = a.Document != null ? a.Document.Folder : null,

                                                       IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                                       IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                                       CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                       Received = a.Received,
                                                       ReceivedDate = a.ReceivedDate,
                                                       ReceivedByUserId = a.ReceivedByUserId,
                                                       ReceivedByByContactId = a.ReceivedByByContactId,
                                                       Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                                       StatusCode = a.StatusCode,
                                                       UpdateDate = a.UpdateDate,
                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                       CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                       CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                       ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,

                                                       EntityReference = a.EntityReference,
                                                       ExternalEntityName = a.ExternalEntityName,
                                                       ExternalEntityReference = a.ExternalEntityReference,
                                                       DepartmentId = a.DepartmentId,
                                                       BranchId = a.BranchId,
                                                       FolderId = a.FolderId,
                                                       IsDeleted = a.IsDeleted,
                                                       DeleteDateTime = a.DeleteDateTime,
                                                       DeletedByUserId = a.DeletedByUserId,
                                                       IsDigitallySigned = a.IsDigitallySigned,
                                                       SignersList = a.SignersList,
                                                       IsSharedWithCustomer = a.IsSharedWithCustomer,
                                                       IsSharedWithForwarder = a.IsSharedWithForwarder,
                                                       CustomerDocumentId = a.CustomerDocumentId,
                                                       ForwarderDocumentId = a.ForwarderDocumentId,
                                                       SecurityId = a.SecurityId,
                                                       CustomerTenantNumber = a.CustomerTenantNumber,
                                                       IsRequested = a.IsRequested,
                                                       SignRequestByUserEmail = a.SignRequestByUserEmail,
                                                       CancellSignRequest = a.CancellSignRequest,
                                                       OrigionalDocumentId = a.OrigionalDocumentId,
                                                       CalculatedFileName = a.Document != null ? !string.IsNullOrEmpty(a.Document.CalculatedFileName) ? a.Document.CalculatedFileName : a.Document.FileName : "",
                                                       LastShareDate = a.LastShareDate,
                                                       IsSharedIn = a.IsSharedIn,
                                                       IsSharedOut = a.IsSharedOut,
                                                       SignDueDate = a.SignDueDate,
                                                       IsDigitalSignRequired = a.IsDigitalSignRequired,
                                                       BackedupExternally = a.BackedupExternally,

                                                       IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                                       IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                                       IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                                       ReceivedByPartner = a.ReceivedByPartner,
													   IsFromCloud = a.IsFromCloud,

												   };
                return result.ToList();
        }

        public List<DocumentsFilingPM> GetDocumentsFilingPMsByEntityId(int tenant, string entityId, string objectTableId)
        {
            IQueryable<DocumentsFilingPM> result = from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                                   where a.Tenant == tenant && a.EntityId == entityId && a.ObjectTableId == objectTableId && a.IsDeleted == false
                                                   select new DocumentsFilingPM()
                                                   {
                                                       Id = a.Id,
                                                       DocumentId = a.DocumentId,
                                                       Code = a.Code,
                                                       DirectionCode = a.DirectionCode,
                                                       Description = a.Description,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       CreateDate = a.CreateDate,
                                                       ObjectTableId = a.ObjectTableId,
                                                       ChildEntityId = a.ChildEntityId,
                                                       ChildObjectTableId = a.ChildObjectTableId,
                                                       ChildEntityReference = a.ChildEntityReference,
                                                       DocumentTypeId = a.DocumentTypeId,
                                                       EntityId = a.EntityId,
                                                       HasCopies = a.HasCopies,
                                                       Notes = a.Notes,
                                                       OwnerId = a.OwnerId,
                                                       SearchFields = a.SearchFields,
                                                       Tenant = a.Tenant,
                                                       FileExtension = a.Document != null ? a.Document.Extension : null,
                                                       HasFile = a.Document != null ? a.Document.HasFile : false,
                                                       FileName = a.Document != null ? a.Document.FileName : null,
                                                       FileSize = a.Document != null ? a.Document.FileSize : null,
                                                       Folder = a.Document != null ? a.Document.Folder : null,
                                                       DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                       DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                       DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                                       IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                                       IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                                       CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                       Received = a.Received,
                                                       ReceivedDate = a.ReceivedDate,
                                                       ReceivedByUserId = a.ReceivedByUserId,
                                                       ReceivedByByContactId = a.ReceivedByByContactId,
                                                       Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                                       StatusCode = a.StatusCode,
                                                       UpdateDate = a.UpdateDate,
                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                       CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                       CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                       EntityReference = a.EntityReference,
                                                       ExternalEntityName = a.ExternalEntityName,
                                                       ExternalEntityReference = a.ExternalEntityReference,
                                                       DepartmentId = a.DepartmentId,
                                                       BranchId = a.BranchId,
                                                       FolderId = a.FolderId,
                                                       IsDeleted = a.IsDeleted,
                                                       DeleteDateTime = a.DeleteDateTime,
                                                       DeletedByUserId = a.DeletedByUserId,
                                                       IsDigitallySigned = a.IsDigitallySigned,
                                                       SignersList = a.SignersList,
                                                       IsSharedWithCustomer = a.IsSharedWithCustomer,
                                                       IsSharedWithForwarder = a.IsSharedWithForwarder,
                                                       CustomerDocumentId = a.CustomerDocumentId,
                                                       ForwarderDocumentId = a.ForwarderDocumentId,
                                                       SecurityId = a.SecurityId,

                                                       LastVersion = a.LastVersion,
                                                       CustomerTenantNumber = a.CustomerTenantNumber,
                                                       IsRequested = a.IsRequested,
                                                       ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                                       SignRequestByUserEmail = a.SignRequestByUserEmail,
                                                       CancellSignRequest = a.CancellSignRequest,
                                                       OrigionalDocumentId = a.OrigionalDocumentId,
                                                       LastShareDate = a.LastShareDate,
                                                       IsSharedIn = a.IsSharedIn,
                                                       IsSharedOut = a.IsSharedOut,
                                                       SignDueDate = a.SignDueDate,
                                                       IsDigitalSignRequired = a.IsDigitalSignRequired,
                                                       BackedupExternally = a.BackedupExternally,

                                                       IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                                       IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                                       IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                                       ReceivedByPartner = a.ReceivedByPartner,
													   IsFromCloud = a.IsFromCloud,

												   };
            return result.ToList();
        }

        public List<DocumentsFilingPM> GetDocumentsFilingsByIdForRelatedDocuments(string entityId, string childEntityId, string objectTableId, string directionCode, int tenant, List<string> externalEntityReferences)
        {
            ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);

            List<DocumentsFilingPM> externalDocumentPMs;

            if (string.IsNullOrEmpty(childEntityId))
            {
                externalDocumentPMs = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                       where a.Tenant == tenant && (a.EntityId == entityId && a.ObjectTableId == objectTableId) && a.DirectionCode == directionCode
                                       select new DocumentsFilingPM()
                                       {
                                           Id = a.Id,
                                           DocumentId = a.DocumentId,
                                           Code = a.Code,
                                           DirectionCode = a.DirectionCode,
                                           Description = a.Description,
                                           CreatedByUserId = a.CreatedByUserId,
                                           CreateDate = a.CreateDate,
                                           ObjectTableId = a.ObjectTableId,
                                           ChildEntityId = a.ChildEntityId,
                                           ChildObjectTableId = a.ChildObjectTableId,
                                           ChildEntityReference = a.ChildEntityReference,
                                           DocumentTypeId = a.DocumentTypeId,
                                           EntityId = a.EntityId,
                                           HasCopies = a.HasCopies,
                                           Notes = a.Notes,
                                           OwnerId = a.OwnerId,
                                           SearchFields = a.SearchFields,
                                           Tenant = a.Tenant,
                                           FileExtension = a.Document != null ? a.Document.Extension : null,
                                           HasFile = a.Document != null ? a.Document.HasFile : false,
                                           FileName = a.Document != null ? a.Document.FileName : null,
                                           FileSize = a.Document != null ? a.Document.FileSize : null,
                                           Folder = a.Document != null ? a.Document.Folder : null,
                                           DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                           IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                           IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                           CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                           Received = a.Received,
                                           ReceivedDate = a.ReceivedDate,
                                           ReceivedByUserId = a.ReceivedByUserId,
                                           ReceivedByByContactId = a.ReceivedByByContactId,
                                           Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                           StatusCode = a.StatusCode,
                                           UpdateDate = a.UpdateDate,
                                           UpdatedByUserId = a.UpdatedByUserId,
                                           CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           //ExternalCode = a.ExternalCode,
                                           EntityReference = a.EntityReference,
                                           ExternalEntityName = a.ExternalEntityName,
                                           ExternalEntityReference = a.ExternalEntityReference,
                                           DepartmentId = a.DepartmentId,
                                           BranchId = a.BranchId,
                                           FolderId = a.FolderId,
                                           IsDeleted = a.IsDeleted,
                                           DeleteDateTime = a.DeleteDateTime,
                                           DeletedByUserId = a.DeletedByUserId,
                                           IsDigitallySigned = a.IsDigitallySigned,
                                           SignersList = a.SignersList,
                                           IsSharedWithCustomer = a.IsSharedWithCustomer,
                                           IsSharedWithForwarder = a.IsSharedWithForwarder,
                                           CustomerDocumentId = a.CustomerDocumentId,
                                           ForwarderDocumentId = a.ForwarderDocumentId,
                                           SecurityId = a.SecurityId,
                                           CustomerTenantNumber = a.CustomerTenantNumber,
                                           LastVersion = a.LastVersion,
                                           SignRequestByUserEmail = a.SignRequestByUserEmail,
                                           CancellSignRequest = a.CancellSignRequest,
                                           OrigionalDocumentId = a.OrigionalDocumentId,
                                           LastShareDate = a.LastShareDate,
                                           IsSharedIn = a.IsSharedIn,
                                           IsSharedOut = a.IsSharedOut,
                                           SignDueDate = a.SignDueDate,
                                           IsDigitalSignRequired = a.IsDigitalSignRequired,
                                           BackedupExternally = a.BackedupExternally,
                                           DocumentCategoryCode = a.DocumentType.DocumentTypeCategoryCode,

                                           IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                           IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                           IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                           ReceivedByPartner = a.ReceivedByPartner,
										   IsFromCloud = a.IsFromCloud,

									   }).ToList();
            }
            else
            {
                externalDocumentPMs = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                       where a.Tenant == tenant && a.EntityId == entityId && a.ChildEntityId == childEntityId && (a.ObjectTableId == objectTableId)
                                       select new DocumentsFilingPM()
                                       {
                                           Id = a.Id,
                                           DocumentId = a.DocumentId,
                                           Code = a.Code,
                                           DirectionCode = a.DirectionCode,
                                           Description = a.Description,
                                           CreatedByUserId = a.CreatedByUserId,
                                           CreateDate = a.CreateDate,
                                           ObjectTableId = a.ObjectTableId,
                                           ChildEntityId = a.ChildEntityId,
                                           ChildObjectTableId = a.ChildObjectTableId,
                                           ChildEntityReference = a.ChildEntityReference,
                                           DocumentTypeId = a.DocumentTypeId,
                                           EntityId = a.EntityId,
                                           HasCopies = a.HasCopies,
                                           Notes = a.Notes,
                                           OwnerId = a.OwnerId,
                                           SearchFields = a.SearchFields,
                                           Tenant = a.Tenant,
                                           FileExtension = a.Document != null ? a.Document.Extension : null,
                                           HasFile = a.Document != null ? a.Document.HasFile : false,
                                           FileName = a.Document != null ? a.Document.FileName : null,
                                           FileSize = a.Document != null ? a.Document.FileSize : null,
                                           Folder = a.Document != null ? a.Document.Folder : null,
                                           DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                           IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                           IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                           CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                           Received = a.Received,
                                           ReceivedDate = a.ReceivedDate,
                                           ReceivedByUserId = a.ReceivedByUserId,
                                           ReceivedByByContactId = a.ReceivedByByContactId,
                                           Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                           StatusCode = a.StatusCode,
                                           UpdateDate = a.UpdateDate,
                                           UpdatedByUserId = a.UpdatedByUserId,
                                           CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                           CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                           //ExternalCode = a.ExternalCode,
                                           EntityReference = a.EntityReference,
                                           ExternalEntityName = a.ExternalEntityName,
                                           ExternalEntityReference = a.ExternalEntityReference,
                                           DepartmentId = a.DepartmentId,
                                           BranchId = a.BranchId,
                                           FolderId = a.FolderId,
                                           IsDeleted = a.IsDeleted,
                                           DeleteDateTime = a.DeleteDateTime,
                                           DeletedByUserId = a.DeletedByUserId,
                                           SignersList = a.SignersList,
                                           IsDigitallySigned = a.IsDigitallySigned,
                                           IsSharedWithCustomer = a.IsSharedWithCustomer,
                                           IsSharedWithForwarder = a.IsSharedWithForwarder,
                                           CustomerDocumentId = a.CustomerDocumentId,
                                           ForwarderDocumentId = a.ForwarderDocumentId,
                                           SecurityId = a.SecurityId,
                                           CustomerTenantNumber = a.CustomerTenantNumber,
                                           LastVersion = a.LastVersion,
                                           SignRequestByUserEmail = a.SignRequestByUserEmail,
                                           CancellSignRequest = a.CancellSignRequest,
                                           OrigionalDocumentId = a.OrigionalDocumentId,
                                           LastShareDate = a.LastShareDate,
                                           IsSharedIn = a.IsSharedIn,
                                           IsSharedOut = a.IsSharedOut,
                                           SignDueDate = a.SignDueDate,
                                           IsDigitalSignRequired = a.IsDigitalSignRequired,
                                           BackedupExternally = a.BackedupExternally,

                                           IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                           IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                           IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                           ReceivedByPartner = a.ReceivedByPartner,
										   IsFromCloud = a.IsFromCloud,

									   }).ToList();
            }
            if (externalEntityReferences != null)
            {
                List<DocumentsFilingPM> docs = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                                where a.Tenant == tenant && externalEntityReferences.Contains(a.ExternalEntityReference)
                                                select new DocumentsFilingPM()
                                                {
                                                    Id = a.Id,
                                                    DocumentId = a.DocumentId,
                                                    Code = a.Code,
                                                    DirectionCode = a.DirectionCode,
                                                    Description = a.Description,
                                                    CreatedByUserId = a.CreatedByUserId,
                                                    CreateDate = a.CreateDate,
                                                    ObjectTableId = a.ObjectTableId,
                                                    ChildEntityId = a.ChildEntityId,
                                                    ChildObjectTableId = a.ChildObjectTableId,
                                                    ChildEntityReference = a.ChildEntityReference,
                                                    DocumentTypeId = a.DocumentTypeId,
                                                    EntityId = a.EntityId,
                                                    HasCopies = a.HasCopies,
                                                    Notes = a.Notes,
                                                    OwnerId = a.OwnerId,
                                                    SearchFields = a.SearchFields,
                                                    Tenant = a.Tenant,
                                                    FileExtension = a.Document != null ? a.Document.Extension : null,
                                                    HasFile = a.Document != null ? a.Document.HasFile : false,
                                                    FileName = a.Document != null ? a.Document.FileName : null,
                                                    FileSize = a.Document != null ? a.Document.FileSize : null,
                                                    Folder = a.Document != null ? a.Document.Folder : null,
                                                    DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                    DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                    DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                                    IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                                    IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                                    CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                    Received = a.Received,
                                                    ReceivedDate = a.ReceivedDate,
                                                    ReceivedByUserId = a.ReceivedByUserId,
                                                    ReceivedByByContactId = a.ReceivedByByContactId,
                                                    Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                                    StatusCode = a.StatusCode,
                                                    UpdateDate = a.UpdateDate,
                                                    UpdatedByUserId = a.UpdatedByUserId,
                                                    CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                    CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                    //ExternalCode = a.ExternalCode,
                                                    EntityReference = a.EntityReference,
                                                    ExternalEntityName = a.ExternalEntityName,
                                                    ExternalEntityReference = a.ExternalEntityReference,
                                                    DepartmentId = a.DepartmentId,
                                                    BranchId = a.BranchId,
                                                    FolderId = a.FolderId,
                                                    IsDeleted = a.IsDeleted,
                                                    DeleteDateTime = a.DeleteDateTime,
                                                    DeletedByUserId = a.DeletedByUserId,
                                                    SignersList = a.SignersList,
                                                    IsDigitallySigned = a.IsDigitallySigned,
                                                    IsSharedWithCustomer = a.IsSharedWithCustomer,
                                                    IsSharedWithForwarder = a.IsSharedWithForwarder,
                                                    CustomerDocumentId = a.CustomerDocumentId,
                                                    ForwarderDocumentId = a.ForwarderDocumentId,
                                                    SecurityId = a.SecurityId,
                                                    CustomerTenantNumber = a.CustomerTenantNumber,
                                                    LastVersion = a.LastVersion,
                                                    SignRequestByUserEmail = a.SignRequestByUserEmail,
                                                    CancellSignRequest = a.CancellSignRequest,
                                                    OrigionalDocumentId = a.OrigionalDocumentId,
                                                    IsDigitalSignRequired = a.IsDigitalSignRequired,
                                                    BackedupExternally = a.BackedupExternally,

                                                    IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                                    IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                                    IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                                    ReceivedByPartner = a.ReceivedByPartner,
													IsFromCloud = a.IsFromCloud,

												}).ToList();
                var cmp = new DocumentsFilingPMComparer();
                externalDocumentPMs = externalDocumentPMs.Concat(docs).Distinct(cmp).ToList();
            }

            ICustomsDocumentQueryServiceExt customsDocumentQueryService = ContainerAccessor.Container.Resolve(typeof(ICustomsDocumentQueryServiceExt), "CustomsDocumentQueryServiceExt", new ParameterOverride("", 1)) as ICustomsDocumentQueryServiceExt;
            var followUpIds = new FollowUpRepository(tenant).GetFollowUpIdByDocumentsFilingIds(tenant, externalDocumentPMs.Select(x => x.Id).ToArray());
            DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);

            foreach (DocumentsFilingPM extDocPm in externalDocumentPMs)
            {
                List<DocumentsFilingMetaDataValuePM> documentsFilingMetaDataValuesList = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByDocumentIdTenant(extDocPm.Id, tenant).ToList(); //documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByTenant(tenant).ToList();
                SetDocumentFollowUp(extDocPm, followUpIds.ContainsKey(extDocPm.Id) ? followUpIds[extDocPm.Id] : string.Empty);

                CustomsDocumentPM customsDoc = customsDocumentQueryService.GetSingleByDocFileId(extDocPm.Id,tenant);
                if (customsDoc != null)
                {
                    extDocPm.CustomsDocumentTypeName = customsDoc.DocumentTypeName;
                    extDocPm.CustomsDocumentTypeCode = customsDoc.DocumentTypeCode;
                    extDocPm.IsMetaDataReady = customsDoc.IsMetaDataReady;
                    extDocPm.CustomsDocumentStatusCode = customsDoc.DocumentStatusCode;
                    extDocPm.ExternalAttachmentId = customsDoc.ExternalAttachmentId;
                    extDocPm.OcrStatusCode = customsDoc.OcrStatusCode;
                    extDocPm.OcrScore = customsDoc.OcrScore;
                    extDocPm.OcrReference = customsDoc.OcrReference;
                    extDocPm.OcrNotConnect = customsDoc.OcrNotConnect;

                }

                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValuesList;//.Where(d => d.DocumentsFilingId == extDocPm.Id && d.Tenant == extDocPm.Tenant).ToList();
            }

            return externalDocumentPMs.Where(d => d.HasFile).ToList();
        }
       

        public List<DocumentsFilingPM> GetDocumentsFilingsByRferenceForRelatedDocuments(int tenant, List<string> externalEntityReferences)
        {
            ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);

            List<DocumentsFilingPM> externalDocumentPMs;

            externalDocumentPMs = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                   where a.Tenant == tenant && externalEntityReferences.Contains(a.ExternalEntityReference)
                                   select new DocumentsFilingPM()
                                   {
                                       Id = a.Id,
                                       DocumentId = a.DocumentId,
                                       Code = a.Code,
                                       DirectionCode = a.DirectionCode,
                                       Description = a.Description,
                                       CreatedByUserId = a.CreatedByUserId,
                                       CreateDate = a.CreateDate,
                                       ObjectTableId = a.ObjectTableId,
                                       ChildEntityId = a.ChildEntityId,
                                       ChildObjectTableId = a.ChildObjectTableId,
                                       ChildEntityReference = a.ChildEntityReference,
                                       DocumentTypeId = a.DocumentTypeId,
                                       EntityId = a.EntityId,
                                       HasCopies = a.HasCopies,
                                       Notes = a.Notes,
                                       OwnerId = a.OwnerId,
                                       SearchFields = a.SearchFields,
                                       Tenant = a.Tenant,
                                       FileExtension = a.Document != null ? a.Document.Extension : null,
                                       HasFile = a.Document != null ? a.Document.HasFile : false,
                                       FileName = a.Document != null ? a.Document.FileName : null,
                                       FileSize = a.Document != null ? a.Document.FileSize : null,
                                       Folder = a.Document != null ? a.Document.Folder : null,
                                       DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                       DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                       DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                       IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                       IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                       CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                       Received = a.Received,
                                       ReceivedDate = a.ReceivedDate,
                                       ReceivedByUserId = a.ReceivedByUserId,
                                       ReceivedByByContactId = a.ReceivedByByContactId,
                                       Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                       StatusCode = a.StatusCode,
                                       UpdateDate = a.UpdateDate,
                                       UpdatedByUserId = a.UpdatedByUserId,
                                       CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                       CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                       //ExternalCode = a.ExternalCode,
                                       EntityReference = a.EntityReference,
                                       ExternalEntityName = a.ExternalEntityName,
                                       ExternalEntityReference = a.ExternalEntityReference,
                                       DepartmentId = a.DepartmentId,
                                       BranchId = a.BranchId,
                                       FolderId = a.FolderId,
                                       IsDeleted = a.IsDeleted,
                                       DeleteDateTime = a.DeleteDateTime,
                                       DeletedByUserId = a.DeletedByUserId,
                                       IsDigitallySigned = a.IsDigitallySigned,
                                       SignersList = a.SignersList,
                                       IsSharedWithCustomer = a.IsSharedWithCustomer,
                                       IsSharedWithForwarder = a.IsSharedWithForwarder,
                                       CustomerDocumentId = a.CustomerDocumentId,
                                       ForwarderDocumentId = a.ForwarderDocumentId,
                                       SecurityId = a.SecurityId,
                                       CustomerTenantNumber = a.CustomerTenantNumber,
                                       LastVersion = a.LastVersion,
                                       SignRequestByUserEmail = a.SignRequestByUserEmail,
                                       CancellSignRequest = a.CancellSignRequest,
                                       OrigionalDocumentId = a.OrigionalDocumentId,
                                       IsDigitalSignRequired = a.IsDigitalSignRequired,
                                       BackedupExternally = a.BackedupExternally,
                                       DocumentCategoryCode =a.DocumentType.DocumentTypeCategoryCode,

                                       IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                       IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                       IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                       ReceivedByPartner = a.ReceivedByPartner,
									   IsFromCloud = a.IsFromCloud,

								   }).ToList();


            ICustomsDocumentQueryServiceExt customsDocumentQueryService = ContainerAccessor.Container.Resolve(typeof(ICustomsDocumentQueryServiceExt), "CustomsDocumentQueryServiceExt", new ParameterOverride("", 1)) as ICustomsDocumentQueryServiceExt;
            var followUpIds = new FollowUpRepository(tenant).GetFollowUpIdByDocumentsFilingIds(tenant, externalDocumentPMs.Select(x => x.Id).ToArray());
            DocumentsFilingMetaDataValueQuery documentsFilingMetaDataValueQuery = new DocumentsFilingMetaDataValueQuery(tenant);
            var documentsFilingMetaDataValuesList = documentsFilingMetaDataValueQuery.GetDocumentsFilingMetaDataValuePMsByTenant1(tenant);

            foreach (DocumentsFilingPM extDocPm in externalDocumentPMs)
            {
                SetDocumentFollowUp(extDocPm, followUpIds.ContainsKey(extDocPm.Id) ? followUpIds[extDocPm.Id] : string.Empty);
                CustomsDocumentPM customsDoc = customsDocumentQueryService.GetSingle(extDocPm.Id, false, false,tenant);
                if (customsDoc != null)
                {
                    extDocPm.CustomsDocumentTypeName = customsDoc.DocumentTypeName;
                    extDocPm.CustomsDocumentTypeCode = customsDoc.DocumentTypeCode;
                    extDocPm.IsMetaDataReady = customsDoc.IsMetaDataReady;
                    extDocPm.CustomsDocumentStatusCode = customsDoc.DocumentStatusCode;
                    extDocPm.ExternalAttachmentId = customsDoc.ExternalAttachmentId;
                }

                extDocPm.DocumentsFilingMetaDataValues = documentsFilingMetaDataValuesList.Where(d => d.DocumentsFilingId == extDocPm.Id && d.Tenant == extDocPm.Tenant).ToList();
            }

            return externalDocumentPMs.Where(d => d.HasFile).ToList();
        }
        public List<string> GetSharedWithAgentDocumentsFilingPMsIdsByEntityId(string entityId, string directionCode, int tenant)
        {
            List<string> externalDocumentPMs = new List<string>();
            externalDocumentPMs = (from a in repository.context.DocumentsFilings
                                   where a.Tenant == tenant && a.EntityId == entityId && a.IsSharedWithForwarder == true && a.DirectionCode == directionCode && (a.Document != null ? a.Document.HasFile == true : false)
                                   select a.Id).ToList();

            return externalDocumentPMs;
        }



        public string GetSecurityIdByDocumentsFilingId(string id, int tenant)
        {
            string securityId = (from a in repository.context.DocumentsFilings
                                 where a.Tenant == tenant && a.Id == id
                                 select a).FirstOrDefault().SecurityId;

            return securityId;


        }

        public bool IsEntityHasSharedDocs(string entityId, int tenant)
        {
            var extDocPm = (from a in repository.context.DocumentsFilings
                            where a.EntityId == entityId && a.Tenant == tenant && a.IsSharedWithForwarder == true && a.IsDeleted == false
                            select a);
            return (extDocPm.Count() > 0);
        }

        public DocumentsFilingPM GetSinglePMBySignRequestEmail(string Email, int Tenant)
        {
            DocumentsFilingPM extDocPm = (from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                          where a.SignRequestByUserEmail == Email && a.Tenant == Tenant && a.Document != null && a.Document.HasFile
                                          orderby a.SignDueDate ascending
                                          select new DocumentsFilingPM()
                                          {
                                              Id = a.Id,
                                              DocumentId = a.DocumentId,
                                              Code = a.Code,
                                              DirectionCode = a.DirectionCode,
                                              Description = a.Description,
                                              CreatedByUserId = a.CreatedByUserId,
                                              CreateDate = a.CreateDate,
                                              ObjectTableId = a.ObjectTableId,
                                              ChildEntityId = a.ChildEntityId,
                                              ChildObjectTableId = a.ChildObjectTableId,
                                              ChildEntityReference = a.ChildEntityReference,
                                              DocumentTypeId = a.DocumentTypeId,
                                              EntityId = a.EntityId,
                                              HasCopies = a.HasCopies,
                                              Notes = a.Notes,
                                              OwnerId = a.OwnerId,
                                              SearchFields = a.SearchFields,
                                              Tenant = a.Tenant,
                                              FileExtension = a.Document != null ? a.Document.Extension : null,
                                              HasFile = a.Document != null ? a.Document.HasFile : false,
                                              FileName = a.Document != null ? a.Document.FileName : null,
                                              FileSize = a.Document != null ? a.Document.FileSize : null,
                                              Folder = a.Document != null ? a.Document.Folder : null,
                                              DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                              IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                              IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                              CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                              Received = a.Received,
                                              ReceivedDate = a.ReceivedDate,
                                              ReceivedByUserId = a.ReceivedByUserId,
                                              ReceivedByByContactId = a.ReceivedByByContactId,
                                              Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                              StatusCode = a.StatusCode,
                                              UpdateDate = a.UpdateDate,
                                              UpdatedByUserId = a.UpdatedByUserId,
                                              CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                              CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                              EntityReference = a.EntityReference,
                                              ExternalEntityName = a.ExternalEntityName,
                                              ExternalEntityReference = a.ExternalEntityReference,
                                              DepartmentId = a.DepartmentId,
                                              BranchId = a.BranchId,
                                              FolderId = a.FolderId,
                                              IsDeleted = a.IsDeleted,
                                              DeleteDateTime = a.DeleteDateTime,
                                              DeletedByUserId = a.DeletedByUserId,
                                              IsDigitallySigned = a.IsDigitallySigned,
                                              SignersList = a.SignersList,
                                              IsSharedWithCustomer = a.IsSharedWithCustomer,
                                              IsSharedWithForwarder = a.IsSharedWithForwarder,
                                              CustomerDocumentId = a.CustomerDocumentId,
                                              ForwarderDocumentId = a.ForwarderDocumentId,
                                              CustomerTenantNumber = a.CustomerTenantNumber,
                                              SecurityId = a.SecurityId,
                                              LastVersion = a.LastVersion,
                                              IsRequested = a.IsRequested,
                                              ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,
                                              SignRequestByUserEmail = a.SignRequestByUserEmail,
                                              CancellSignRequest = a.CancellSignRequest,
                                              OrigionalDocumentId = a.OrigionalDocumentId,
                                              LastShareDate = a.LastShareDate,
                                              IsSharedIn = a.IsSharedIn,
                                              IsSharedOut = a.IsSharedOut,
                                              SignDueDate = a.SignDueDate,
                                              IsDigitalSignRequired = a.IsDigitalSignRequired,
                                              BackedupExternally = a.BackedupExternally,

                                              IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                              IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                              IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                              ReceivedByPartner = a.ReceivedByPartner,
											  IsFromCloud = a.IsFromCloud,
										  }).FirstOrDefault();

            
            return extDocPm;
        }


        public List<DocumentsFilingList> GetDocumentsFilingListsByDocumentTypeIdsAndEntityId(List<string> documentTypeIds, List<string> entityIds, int tenant)
        {
            List<DocumentsFilingList> documentsFilingLists = (from a in repository.context.DocumentsFilings.Include("Document")
                                                              where a.Tenant == tenant && documentTypeIds.Contains(a.DocumentTypeId) && entityIds.Contains(a.EntityId)
                                                              select new DocumentsFilingList()
                                                              {
                                                                  Id = a.Id,
                                                                  DocumentTypeId = a.DocumentTypeId,
                                                                  EntityId = a.EntityId,
                                                                  Tenant = a.Tenant,
                                                                  HasFile = a.Document != null ? a.Document.HasFile : false,
                                                                  LastShareDate = a.LastShareDate,
                                                                  IsSharedIn = a.IsSharedIn,
                                                                  IsSharedOut = a.IsSharedOut,
                                                                  DirectionCode = a.DirectionCode,
                                                                  UpdateDate = a.UpdateDate,
                                                                  SecurityId = a.SecurityId,
                                                                  CalculatedFileName = a.Document != null ? a.Document.CalculatedFileName : "",
                                                                  FileName = a.Document != null ? a.Document.FileName : "",
                                                                  DocumentId = a.Document != null ? a.Document.Id : "",
                                                                  FileSize = a.Document != null ? a.Document.FileSize : null,
                                                                  Extension = a.Document != null ? a.Document.Extension : "",
                                                                  ReceivedByPartner = a.ReceivedByPartner,

                                                              }).ToList();



            return documentsFilingLists;
        }

        public List<DocumentsFilingList> GetDocumentsFilingListsBysecurityIds(List<string> securityIds, int tenant)
        {
            List<DocumentsFilingList> documentsFilingLists = (from a in repository.context.DocumentsFilings.Include("Document")
                                                              where a.Tenant == tenant && securityIds.Contains(a.SecurityId)
                                                              select new DocumentsFilingList()
                                                              {
                                                                  HasFile = a.Document != null ? a.Document.HasFile : false,
                                                                  DirectionCode = a.DirectionCode,
                                                                  DocumentId = a.DocumentId,
                                                                  ReceivedDate = a.ReceivedDate,
                                                                  Id = a.Id,
                                                                  Description = a.Description,
                                                                  DocumentTypeId = a.DocumentTypeId,
                                                                  SecurityId = a.SecurityId,
                                                                  Code = a.Code,
                                                                  EntityReference = a.EntityReference,
                                                                  ReceivedByPartner = a.ReceivedByPartner,
                                                              }).ToList();



            return documentsFilingLists;
        }


        public List<DocumentsFilingPM> GetDocumentsFilingsByIds(string ids, int tenant)
        {
            List<DocumentsFiling> documentsFilings = repository.GetDocumentsFilingsByIds(ids, tenant);
            List<DocumentsFilingPM> documentFilingPMs = (from a in documentsFilings
                                                         select new DocumentsFilingPM()
                                                         {
                                                             Id = a.Id,
                                                             DocumentId = a.DocumentId,
                                                             Code = a.Code,
                                                             DirectionCode = a.DirectionCode,
                                                             Description = a.Description,
                                                             CreatedByUserId = a.CreatedByUserId,
                                                             CreateDate = a.CreateDate,
                                                             ObjectTableId = a.ObjectTableId,
                                                             ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : "",
                                                             ChildEntityId = a.ChildEntityId,
                                                             ChildObjectTableId = a.ChildObjectTableId,
                                                             ChildEntityReference = a.ChildEntityReference,
                                                             DocumentTypeId = a.DocumentTypeId,
                                                             EntityId = a.EntityId,
                                                             HasCopies = a.HasCopies,
                                                             Notes = a.Notes,
                                                             OwnerId = a.OwnerId,
                                                             SearchFields = a.SearchFields,
                                                             Tenant = a.Tenant,
                                                             EntityReference = a.EntityReference,
                                                             DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                             DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                             DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                                             EntityNumber = a.EntityNumber,
                                                             BackedupExternally = a.BackedupExternally,
                                                             ReceivedByPartner = a.ReceivedByPartner,
															 IsFromCloud = a.IsFromCloud,

														 }).ToList();
            return documentFilingPMs;

        }

        public bool IsEntityHasDocs(string entityId, int tenant)
        {
            var extDocPm = (from a in repository.context.DocumentsFilings
                            where a.EntityId == entityId && a.Tenant == tenant && a.IsDeleted == false && a.DocumentId != null
                            select a);
            return (extDocPm.Count() > 0);
        }


        public List<string> GetDocumentFilingIdsByTenantCreateDate(int tenant, DateTime StartDate, DateTime EndDate, bool includeBackedDocuments)
        {
            if (includeBackedDocuments)

                return (from a in repository.context.DocumentsFilings.Include("Document")
                        where a.Tenant == tenant && a.CreateDate >= StartDate && a.CreateDate < EndDate && a.DocumentId != null && a.Document.HasFile == true
                        select a.Id).ToList();

            else
                return (from a in repository.context.DocumentsFilings.Include("Document")
                        where a.Tenant == tenant && a.CreateDate >= StartDate && a.CreateDate < EndDate && a.DocumentId != null && a.Document.HasFile == true && a.BackedupExternally == false
                        select a.Id).ToList();
        }

        public List<DocumentsFilingPM> GetRequestedDocumentsFilingPMsByEntityId(string entityId, int tenant)
        {
            IQueryable<DocumentsFilingPM> result = from a in repository.context.DocumentsFilings.Include("CreatedByUser.Contact").Include("ReceivedByUser.Contact").Include("Document").Include("DocumentType").Include("Owner.Contact")
                                                   where a.Tenant == tenant && a.EntityId == entityId && (a.IsDigitalSignRequired == true || a.IsRequested == true)
                                                   select new DocumentsFilingPM()
                                                   {
                                                       Id = a.Id,
                                                       DocumentId = a.DocumentId,
                                                       Code = a.Code,
                                                       DirectionCode = a.DirectionCode,
                                                       Description = a.Description,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       CreateDate = a.CreateDate,
                                                       ObjectTableId = a.ObjectTableId,
                                                       ChildEntityId = a.ChildEntityId,
                                                       ChildObjectTableId = a.ChildObjectTableId,
                                                       ChildEntityReference = a.ChildEntityReference,
                                                       DocumentTypeId = a.DocumentTypeId,
                                                       DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                       DocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                       DoucmentTypeTemplateFormatCode = a.DocumentType != null ? a.DocumentType.TemplateFormatCode : null,
                                                       EntityId = a.EntityId,
                                                       HasCopies = a.HasCopies,
                                                       Notes = a.Notes,
                                                       OwnerId = a.OwnerId,
                                                       OwnerName = a.Owner != null ? a.Owner.Contact.EnglishName : null,
                                                       SearchFields = a.SearchFields,
                                                       Tenant = a.Tenant,
                                                       FileExtension = a.Document != null ? a.Document.Extension : null,
                                                       HasFile = a.Document != null ? a.Document.HasFile : false,
                                                       FileName = a.Document != null ? a.Document.FileName : null,
                                                       FileSize = a.Document != null ? a.Document.FileSize : null,
                                                       Folder = a.Document != null ? a.Document.Folder : null,

                                                       IsAgentView = a.DocumentType != null ? a.DocumentType.IsAgentView : false,
                                                       IsCustomerView = a.DocumentType != null ? a.DocumentType.IsCustomerView : false,
                                                       CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.EnglishName : null,
                                                       Received = a.Received,
                                                       ReceivedDate = a.ReceivedDate,
                                                       ReceivedByUserId = a.ReceivedByUserId,
                                                       ReceivedByByContactId = a.ReceivedByByContactId,
                                                       Name = a.DocumentType != null ? a.DocumentType.Name : null,
                                                       StatusCode = a.StatusCode,
                                                       UpdateDate = a.UpdateDate,
                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                       CustomsDocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
                                                       CustomsDocumentTypeName = a.DocumentType != null ? a.DocumentType.Name : null,
                                                       ReceivedByUserName = a.ReceivedByUser != null ? a.ReceivedByUser.Contact.EnglishName : null,

                                                       EntityReference = a.EntityReference,
                                                       ExternalEntityName = a.ExternalEntityName,
                                                       ExternalEntityReference = a.ExternalEntityReference,
                                                       DepartmentId = a.DepartmentId,
                                                       BranchId = a.BranchId,
                                                       FolderId = a.FolderId,
                                                       IsDeleted = a.IsDeleted,
                                                       DeleteDateTime = a.DeleteDateTime,
                                                       DeletedByUserId = a.DeletedByUserId,
                                                       IsDigitallySigned = a.IsDigitallySigned,
                                                       SignersList = a.SignersList,
                                                       IsSharedWithCustomer = a.IsSharedWithCustomer,
                                                       IsSharedWithForwarder = a.IsSharedWithForwarder,
                                                       CustomerDocumentId = a.CustomerDocumentId,
                                                       ForwarderDocumentId = a.ForwarderDocumentId,
                                                       SecurityId = a.SecurityId,
                                                       CustomerTenantNumber = a.CustomerTenantNumber,
                                                       IsRequested = a.IsRequested,
                                                       SignRequestByUserEmail = a.SignRequestByUserEmail,
                                                       CancellSignRequest = a.CancellSignRequest,
                                                       OrigionalDocumentId = a.OrigionalDocumentId,
                                                       CalculatedFileName = a.Document != null ? !string.IsNullOrEmpty(a.Document.CalculatedFileName) ? a.Document.CalculatedFileName : a.Document.FileName : "",
                                                       LastShareDate = a.LastShareDate,
                                                       IsSharedIn = a.IsSharedIn,
                                                       IsSharedOut = a.IsSharedOut,
                                                       SignDueDate = a.SignDueDate,
                                                       IsDigitalSignRequired = a.IsDigitalSignRequired,
                                                       BackedupExternally = a.BackedupExternally,

                                                       IsAgentSharedInHouse = a.DocumentType != null ? a.DocumentType.IsAgentSharedInHouse : false,
                                                       IsAgentSharedInDirect = a.DocumentType != null ? a.DocumentType.IsAgentSharedInDirect : false,
                                                       IsAgentSharedInMaster = a.DocumentType != null ? a.DocumentType.IsAgentSharedInMaster : false,
                                                       ReceivedByPartner = a.ReceivedByPartner,
													   IsFromCloud = a.IsFromCloud,

												   };
            return result.ToList();
        }

        
        public DocumentsFilingPM GetDocumentsFilingPMByEntityIdAndObjectTableIdAndDocumentTypeId(string entityId, string objectTableId, string documentTypeId,int tenant)
        {
            DocumentsFilingPM documentsFilingPM = (from a in repository.context.DocumentsFilings.Include("Document")
                                                    where a.Tenant == tenant && a.EntityId == entityId && a.ObjectTableId == objectTableId && a.DocumentTypeId == documentTypeId
                                                   select new DocumentsFilingPM()
                                                    {
                                                        Id = a.Id,
                                                        DocumentId = a.DocumentId,
                                                        Tenant = a.Tenant,
                                                        FileExtension = a.Document != null ? a.Document.Extension : null,
                                                        HasFile = a.Document != null ? a.Document.HasFile : false,
                                                        FileName = a.Document != null ? !string.IsNullOrEmpty(a.Document.CalculatedFileName)? a.Document.CalculatedFileName : a.Document.FileName : null,
                                                        FileSize = a.Document != null ? a.Document.FileSize : null,
                                                        Folder = a.Document != null ? a.Document.Folder : null,
                                                       EntityId = a.EntityId,
                                                       ObjectTableId = a.ObjectTableId,
                                                       OwnerId = a.OwnerId,
                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                       UpdateDate = a.UpdateDate,
                                                       CreateDate =a.CreateDate,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       SecurityId = a.SecurityId,
                                                       ReceivedByPartner = a.ReceivedByPartner,
													   IsFromCloud = a.IsFromCloud,

												   }).FirstOrDefault();

            return documentsFilingPM;
        }

        public IQueryable<DocumentsFilingList> GetDocumentsFilingThatHasFileByEntityIdsAndObjectTableIdAndDocumentTypeId(List<string> entityIds, string objectTableId, string documentTypeId)
        {
            IQueryable<DocumentsFilingList> documentsFilingLists = (from a in repository.context.DocumentsFilings.Include("Document")
                                                                    where a.Document.HasFile && entityIds.Contains(a.EntityId) && a.DocumentTypeId == documentTypeId && a.ObjectTableId == objectTableId
                                                                    select new DocumentsFilingList()
                                                                    {
                                                                        Id = a.Id,
                                                                        DocumentTypeId = a.DocumentTypeId,
                                                                        EntityId = a.EntityId,
                                                                        Tenant = a.Tenant,
                                                                        CreateDate = a.CreateDate,
                                                                        SecurityId = a.SecurityId,
                                                                        DocumentId = a.DocumentId,
                                                                        ReceivedByPartner = a.ReceivedByPartner,
                                                                    });
            return documentsFilingLists;
        }


		public DocumentsFilingPM GetDocumentsFilingsByExportFile(string exportFile, int tenant)
		{

			List<DocumentsFilingPM> documentsFilingList = (from a in repository.context.DocumentsFilings

													   where a.Tenant == tenant && a.ExternalEntityReference == exportFile && (a.ExternalEntityName == "EFIFILEM" || a.ExternalEntityName == "MFIFILEM")
														 select new DocumentsFilingPM()
														 {
															 Id = a.Id,
                                                             Code = a.Code,
															 DocumentTypeCode = a.DocumentType != null ? a.DocumentType.Code : null,
															 EntityId = a.EntityId,
															 Tenant = a.Tenant,	
														 }).ToList();




			if (documentsFilingList == null || documentsFilingList.Count() == 0) return null;

            List<string> docsField = documentsFilingList.Select(x => x.Id).ToList();
			ICustomsDocumentQueryServiceExt customsDocumentQueryService = ContainerAccessor.Container.Resolve(typeof(ICustomsDocumentQueryServiceExt), "CustomsDocumentQueryServiceExt", new ParameterOverride("", 1)) as ICustomsDocumentQueryServiceExt;
			CustomsDocumentPM customsDoc = customsDocumentQueryService.GetDocumentsByDocsFileIdAndTypeClosing(docsField, tenant);
            var documentsFiling = documentsFilingList.Where(x => x.Id == customsDoc?.DocumentsFilingId)?.Select(y => new DocumentsFilingPM
			{ 
                Id = y.Id,
                Code = y.Code,
                DocumentTypeCode = y.DocumentTypeCode,
                EntityId = y.EntityId,
                Tenant = y.Tenant,
				IsNotCustomsDocId = string.IsNullOrEmpty(customsDoc?.CustomsDocId) ? true : false,
			}).FirstOrDefault();
		
			return documentsFiling;
		}
		public List<string> GetCOOEDocument(string declarationId, string certificateOfOriginId, int tenant)
		{

			DocumentTypeRepository documentTypeRep = new DocumentTypeRepository(tenant);
			DocumentType type = documentTypeRep.GetSingleDocumentTypeByCode("COOE", tenant);
			ObjectTableRepository objectTableRep = new ObjectTableRepository(tenant);
			ObjectTable objectTable = objectTableRep.GetObjectTableByName("Customs.Declaration", tenant, true);
			ObjectTable objectTableCertificate = objectTableRep.GetObjectTableByName("Customs.CertificateOfOrigin", tenant, true);

			if (type != null)
			{
				if (type.ObjectTableId == objectTable.Id)
				{
					string documentTypeId = type.Id;

                    var DocumentsFilingList = (from a in repository.GetByEntityAndChiled(objectTable.Id, declarationId, objectTableCertificate.Id, certificateOfOriginId, tenant, documentTypeId)
                                               select a.DocumentId);

					if (!LogitudeSettings.GetLogitudeCustomsSettingsMInject(tenant).IsConnectedToUniFreight)
					{
                        
						return DocumentsFilingList.ToList();

					}


				}
			}
			return null;
		}
	}

    public class DocumentsFilingPMComparer : IEqualityComparer<DocumentsFilingPM>
    {
        public bool Equals(DocumentsFilingPM x, DocumentsFilingPM y)
            => x?.Id == y?.Id;
        public int GetHashCode(DocumentsFilingPM obj)
            => obj.Id?.GetHashCode() ?? 0;
    }
}