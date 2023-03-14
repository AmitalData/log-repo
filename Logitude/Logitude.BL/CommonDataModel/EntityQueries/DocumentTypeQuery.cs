using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.DataContracts;
using Logitude.BL.ShipmentsModel.EntityLists;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools;
using Logitude.BL.DataContracts;
using Microsoft.Practices.Unity;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentTypeQuery
    {
        DocumentTypeRepository repository;
        private bool isFullAccounting;
        public DocumentTypeQuery()
        {
            repository = new DocumentTypeRepository();
        }

        public DocumentTypeQuery(int tenant)
        {
            repository = new DocumentTypeRepository(tenant);
            isFullAccounting = IsFullAccountingActivated(tenant);
        }

        public DocumentTypeQuery(DocumentTypeRepository repository)
        {
            this.repository = repository;
        }




        public DocumentTypePM GetSinglePMWithOutInclude(string id, int tenant)
        {

            return (from a in repository.context.DocumentTypes
                    where a.Id == id && a.Tenant == tenant
                    select new DocumentTypePM()
                    {
                        InActive = a.InActive,
                        Id = a.Id,
                        IsAir = a.IsAir,
                        IsDocIn = a.IsDocIn,
                        IsDocOut = a.IsDocOut,
                        IsInland = a.IsInland,
                        IsOcean = a.IsOcean,
                        Name = a.Name,
                        Notes = a.Notes,
                        Tenant = a.Tenant,
                        SearchFields = a.SearchFields,
                        Code = a.Code,
                        DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                        ObjectTableId = a.ObjectTableId,
                        Subject = a.Subject,
                        DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                        DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                        TemplateFormatCode = a.TemplateFormatCode,
                        IsMaster = a.IsMaster,
                        IsDirect = a.IsDirect,
                        IsHouse = a.IsHouse,
                        CustomControl = a.CustomControl,
                        AgentRoleId = a.AgentRoleId,
                        CustomerRoleId = a.CustomerRoleId,
                        IsAgentView = a.IsAgentView,
                        IsCustomerView = a.IsCustomerView,
                        IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                        IsReadOnly = a.IsReadOnly,
                        LimitedPrintCopyId = a.LimitedPrintCopyId,
                        IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                        IsCopiedAtSignup = a.IsCopiedAtSignup,
                        IsEnabledForCustomers = a.IsEnabledForCustomers,
                        CountryCode = a.CountryCode,
                        DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                        // DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                        OrderBy = a.OrderBy,
                        FileName = a.FileName,
                        IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                        IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                        IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                        SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                        IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                        IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                        IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                        IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                        PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                        AddedManually = a.AddedManually,
                        OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                        OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                        OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,
                    }).FirstOrDefault();
        }

        public DocumentTypePM GetSinglePM(string id, int tenant)
        {
            DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
            DocumentTypeCopyQuery documentCopiesQuery = new DocumentTypeCopyQuery(tenant);

            DocumentTypePM d = (from a in repository.context.DocumentTypes.Include("ObjectTable").Include("DocumentTypeCategory")
                                where a.Id == id && a.Tenant == tenant
                                select new DocumentTypePM()
                                {
                                    Id = a.Id,
                                    IsAir = a.IsAir,
                                    IsDocIn = a.IsDocIn,
                                    IsDocOut = a.IsDocOut,
                                    IsInland = a.IsInland,
                                    IsOcean = a.IsOcean,
                                    Name = a.Name,
                                    Notes = a.Notes,
                                    Tenant = a.Tenant,
                                    SearchFields = a.SearchFields,
                                    Code = a.Code,
                                    InActive = a.InActive,
                                    ObjectTableId = a.ObjectTableId,
                                    Subject = a.Subject,
                                    DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                    DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                    TemplateFormatCode = a.TemplateFormatCode,
                                    DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                    IsMaster = a.IsMaster,
                                    IsDirect = a.IsDirect,
                                    IsHouse = a.IsHouse,
                                    CustomControl = a.CustomControl,
                                    AgentRoleId = a.AgentRoleId,
                                    CustomerRoleId = a.CustomerRoleId,
                                    IsAgentView = a.IsAgentView,
                                    IsCustomerView = a.IsCustomerView,
                                    IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                    IsReadOnly = a.IsReadOnly,
                                    ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                    LimitedPrintCopyId = a.LimitedPrintCopyId,
                                    IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,

                                    IsCopiedAtSignup = a.IsCopiedAtSignup,
                                    IsEnabledForCustomers = a.IsEnabledForCustomers,
                                    CountryCode = a.CountryCode,
                                    DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                    DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                    OrderBy = a.OrderBy,
                                    FileName = a.FileName,
                                    IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                    IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                    IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                    SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                    IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                    IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                    IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                    IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                    PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                    AddedManually = a.AddedManually,
                                    OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                    OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                    OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                }).FirstOrDefault();

            d.DocumentTypeCustomFields = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(d.Id, d.Tenant).ToList();
            d.DocumentTypeCopies = documentCopiesQuery.GetDocumentTypeCopiesByDocumentType(d.Id, null, d.Tenant);

            return d;
        }

        public DocumentTypePM GetSinglePM(string id, string documentOutId, int tenant)
        {
            DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
            DocumentTypeCopyQuery documentCopiesQuery = new DocumentTypeCopyQuery(tenant);

            DocumentTypePM d = (from a in repository.context.DocumentTypes.Include("ObjectTable").Include("DocumentTypeCategory")
                                where a.Id == id && a.Tenant == tenant
                                select new DocumentTypePM()
                                {
                                    Id = a.Id,
                                    IsAir = a.IsAir,
                                    IsDocIn = a.IsDocIn,
                                    IsDocOut = a.IsDocOut,
                                    IsInland = a.IsInland,
                                    IsOcean = a.IsOcean,
                                    Name = a.Name,
                                    Notes = a.Notes,
                                    Tenant = a.Tenant,
                                    SearchFields = a.SearchFields,
                                    Code = a.Code,
                                    InActive = a.InActive,
                                    ObjectTableId = a.ObjectTableId,
                                    Subject = a.Subject,
                                    DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                    DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                    TemplateFormatCode = a.TemplateFormatCode,
                                    DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                    IsMaster = a.IsMaster,
                                    IsDirect = a.IsDirect,
                                    IsHouse = a.IsHouse,
                                    CustomControl = a.CustomControl,
                                    AgentRoleId = a.AgentRoleId,
                                    CustomerRoleId = a.CustomerRoleId,
                                    IsAgentView = a.IsAgentView,
                                    IsCustomerView = a.IsCustomerView,
                                    IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                    IsReadOnly = a.IsReadOnly,
                                    ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                    LimitedPrintCopyId = a.LimitedPrintCopyId,
                                    IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,

                                    IsCopiedAtSignup = a.IsCopiedAtSignup,
                                    IsEnabledForCustomers = a.IsEnabledForCustomers,
                                    CountryCode = a.CountryCode,
                                    DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                    DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                    OrderBy = a.OrderBy,
                                    FileName = a.FileName,
                                    IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                    IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                    IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                    SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                    IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                    IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                    IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                    IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                    PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                    AddedManually = a.AddedManually,
                                    OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                    OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                    OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                }).FirstOrDefault();

            d.DocumentTypeCustomFields = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(d.Id, d.Tenant).ToList();
            d.DocumentTypeCopies = documentCopiesQuery.GetDocumentTypeCopiesByDocumentType(d.Id, documentOutId, d.Tenant);

            d.DocumentTypeCopies = MarkIsOriginalDocumentCopy(d);
            return d;
        }

        private List<DocumentTypeCopyPM> MarkIsOriginalDocumentCopy(DocumentTypePM documentTypePM)
        {
            List<DocumentTypeCopyPM> documentTypeCopies = documentTypePM.DocumentTypeCopies;
            for (int i = 0; i < documentTypeCopies.Count(); i++)
            {
                if (documentTypeCopies[i].Code == documentTypePM.Code)
                {
                    documentTypeCopies[i].IsOriginal = true;
                    break;
                }
            }

            return documentTypeCopies;
        }

        public List<DocumentTypePM> GetDocumentTypePMsByTenant(int tenant)
        {
            DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
            DocumentTypeCopyQuery documentCopiesQuery = new DocumentTypeCopyQuery(tenant);

            List<DocumentTypePM> d = (from a in repository.context.DocumentTypes.Include("DocumentTypeCategory")
                                      where a.Tenant == tenant
                                      select new DocumentTypePM()
                                      {
                                          Id = a.Id,
                                          IsAir = a.IsAir,
                                          IsDocIn = a.IsDocIn,
                                          IsDocOut = a.IsDocOut,
                                          IsInland = a.IsInland,
                                          IsOcean = a.IsOcean,
                                          Name = a.Name,
                                          Notes = a.Notes,
                                          InActive = a.InActive,
                                          Tenant = a.Tenant,
                                          SearchFields = a.SearchFields,
                                          Code = a.Code,
                                          ObjectTableId = a.ObjectTableId,
                                          Subject = a.Subject,
                                          DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                          DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                          TemplateFormatCode = a.TemplateFormatCode,
                                          DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                          IsMaster = a.IsMaster,
                                          IsDirect = a.IsDirect,
                                          IsHouse = a.IsHouse,
                                          CustomControl = a.CustomControl,
                                          AgentRoleId = a.AgentRoleId,
                                          CustomerRoleId = a.CustomerRoleId,
                                          IsAgentView = a.IsAgentView,
                                          IsCustomerView = a.IsCustomerView,
                                          IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                          IsReadOnly = a.IsReadOnly,
                                          LimitedPrintCopyId = a.LimitedPrintCopyId,
                                          IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,

                                          IsCopiedAtSignup = a.IsCopiedAtSignup,
                                          IsEnabledForCustomers = a.IsEnabledForCustomers,
                                          CountryCode = a.CountryCode,
                                          DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                          DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                          OrderBy = a.OrderBy,
                                          FileName = a.FileName,
                                          IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                          IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                          IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                          SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                          IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                          IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                          IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                          IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                          PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                          AddedManually = a.AddedManually,
                                          OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                          OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                          OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                      }).ToList();

            foreach (DocumentTypePM doc in d)
            {
                doc.DocumentTypeCustomFields = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(doc.Id, doc.Tenant).ToList();
                doc.DocumentTypeCopies = documentCopiesQuery.GetDocumentTypeCopiesByDocumentType(doc.Id, null, doc.Tenant);
            }

            return d;
        }


        public DocumentTypePM GetSinglePMByCode(string code, int tenant)
        {
            DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
            DocumentTypeCopyQuery documentCopiesQuery = new DocumentTypeCopyQuery(tenant);

            DocumentTypePM docTypePm = (from a in repository.context.DocumentTypes.Include("ObjectTable").Include("DocumentTypeCategory")
                                        where a.Code == code && a.Tenant == tenant
                                        select new DocumentTypePM()
                                        {
                                            Id = a.Id,
                                            IsAir = a.IsAir,
                                            IsDocIn = a.IsDocIn,
                                            IsDocOut = a.IsDocOut,
                                            IsInland = a.IsInland,
                                            IsOcean = a.IsOcean,
                                            Name = a.Name,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            SearchFields = a.SearchFields,
                                            Code = a.Code,
                                            InActive = a.InActive,
                                            ObjectTableId = a.ObjectTableId,
                                            Subject = a.Subject,
                                            DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                            DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                            TemplateFormatCode = a.TemplateFormatCode,
                                            DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                            IsMaster = a.IsMaster,
                                            IsDirect = a.IsDirect,
                                            IsHouse = a.IsHouse,
                                            CustomControl = a.CustomControl,
                                            AgentRoleId = a.AgentRoleId,
                                            CustomerRoleId = a.CustomerRoleId,
                                            IsAgentView = a.IsAgentView,
                                            IsCustomerView = a.IsCustomerView,
                                            IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                            IsReadOnly = a.IsReadOnly,
                                            ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                            LimitedPrintCopyId = a.LimitedPrintCopyId,
                                            IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                            IsCopiedAtSignup = a.IsCopiedAtSignup,
                                            IsEnabledForCustomers = a.IsEnabledForCustomers,
                                            CountryCode = a.CountryCode,
                                            DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                            DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                            OrderBy = a.OrderBy,
                                            FileName = a.FileName,
                                            IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                            IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                            IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                            SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                            IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                            IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                            IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                            IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                            PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                            AddedManually = a.AddedManually,
                                            OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                            OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                            OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                        }).FirstOrDefault();

            if (docTypePm != null)
            {
                docTypePm.DocumentTypeCustomFields = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(docTypePm.Id, docTypePm.Tenant).ToList();
                docTypePm.DocumentTypeCopies = documentCopiesQuery.GetDocumentTypeCopiesByDocumentType(docTypePm.Id, null, docTypePm.Tenant);
            }

            return docTypePm;
        }

        public DocumentTypePM GetSinglePMByCodeAndTenant(string code, int tenant)
        {
            DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
            DocumentTypeCopyQuery documentCopiesQuery = new DocumentTypeCopyQuery(tenant);

            DocumentTypePM docTypePm = (from a in repository.context.DocumentTypes.Include("ObjectTable").Include("DocumentTypeCategory")
                                        where a.Code == code && a.Tenant == tenant
                                        select new DocumentTypePM()
                                        {
                                            Id = a.Id,
                                            IsAir = a.IsAir,
                                            IsDocIn = a.IsDocIn,
                                            IsDocOut = a.IsDocOut,
                                            IsInland = a.IsInland,
                                            IsOcean = a.IsOcean,
                                            Name = a.Name,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            SearchFields = a.SearchFields,
                                            Code = a.Code,
                                            InActive = a.InActive,
                                            ObjectTableId = a.ObjectTableId,
                                            Subject = a.Subject,
                                            DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                            DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                            TemplateFormatCode = a.TemplateFormatCode,
                                            DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                            IsMaster = a.IsMaster,
                                            IsDirect = a.IsDirect,
                                            IsHouse = a.IsHouse,
                                            CustomControl = a.CustomControl,
                                            AgentRoleId = a.AgentRoleId,
                                            CustomerRoleId = a.CustomerRoleId,
                                            IsAgentView = a.IsAgentView,
                                            IsCustomerView = a.IsCustomerView,
                                            IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                            IsReadOnly = a.IsReadOnly,
                                            ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                            LimitedPrintCopyId = a.LimitedPrintCopyId,
                                            IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                            IsCopiedAtSignup = a.IsCopiedAtSignup,
                                            IsEnabledForCustomers = a.IsEnabledForCustomers,
                                            CountryCode = a.CountryCode,
                                            DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                            DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                            OrderBy = a.OrderBy,
                                            FileName = a.FileName,
                                            IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                            IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                            IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                            SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                            IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                            IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                            IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                            IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                            PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                            AddedManually = a.AddedManually,
                                            OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                            OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                            OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                        }).FirstOrDefault();

            if (docTypePm != null)
            {
                docTypePm.DocumentTypeCustomFields = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(docTypePm.Id, docTypePm.Tenant).ToList();
                docTypePm.DocumentTypeCopies = documentCopiesQuery.GetDocumentTypeCopiesByDocumentType(docTypePm.Id, null, docTypePm.Tenant);
            }

            return docTypePm;
        }
        
        public DocumentTypePM GetDigitalSinglePMByCodeAndTenant(string code, int tenant)
        {
            DocumentTypePM docTypePm = repository.context
                                                 .DocumentTypes
                                                 .Where(a => a.Code == code 
                                                             && a.Tenant == tenant)
                                                 .Select(a => new DocumentTypePM()
                                                 {
                                                    Id = a.Id
                                                 })
                                                .FirstOrDefault();
            return docTypePm;
        }

        public DocumentTypeList GetDocumentTypeListById(string id, int tenant)
        {
            return (from a in repository.context.DocumentTypes
                    where a.Id == id
                    select new DocumentTypeList()
                    {

                        Id = a.Id,
                        Tenant = a.Tenant,
                        InActive = a.InActive,
                        IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                        LimitedPrintCopyId = a.LimitedPrintCopyId,
                        Subject = a.Subject,
                        IsCopiedAtSignup = a.IsCopiedAtSignup,
                        IsEnabledForCustomers = a.IsEnabledForCustomers,
                        CountryCode = a.CountryCode,
                        FileName = a.FileName,

                    }).FirstOrDefault();


        }



        public DocumentTypePM GetSingelDocumentTypeById(string documentId, int tenant)
        {
            DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
            DocumentTypeCopyQuery documentCopiesQuery = new DocumentTypeCopyQuery(tenant);

            DocumentTypePM docTypePm = (from a in repository.context.DocumentTypes.Include("ObjectTable").Include("DocumentTypeCategory")
                                        where a.Id == documentId
                                        select new DocumentTypePM()
                                        {
                                            Id = a.Id,
                                            IsAir = a.IsAir,
                                            IsDocIn = a.IsDocIn,
                                            IsDocOut = a.IsDocOut,
                                            IsInland = a.IsInland,
                                            IsOcean = a.IsOcean,
                                            Name = a.Name,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            SearchFields = a.SearchFields,
                                            Code = a.Code,
                                            InActive = a.InActive,
                                            ObjectTableId = a.ObjectTableId,
                                            Subject = a.Subject,
                                            DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                            DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                            TemplateFormatCode = a.TemplateFormatCode,
                                            DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                            IsMaster = a.IsMaster,
                                            IsDirect = a.IsDirect,
                                            IsHouse = a.IsHouse,
                                            CustomControl = a.CustomControl,
                                            AgentRoleId = a.AgentRoleId,
                                            CustomerRoleId = a.CustomerRoleId,
                                            IsAgentView = a.IsAgentView,
                                            IsCustomerView = a.IsCustomerView,
                                            IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                            IsReadOnly = a.IsReadOnly,
                                            ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                            LimitedPrintCopyId = a.LimitedPrintCopyId,
                                            IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                            IsCopiedAtSignup = a.IsCopiedAtSignup,
                                            IsEnabledForCustomers = a.IsEnabledForCustomers,
                                            CountryCode = a.CountryCode,
                                            DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                            DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                            OrderBy = a.OrderBy,
                                            FileName = a.FileName,
                                            IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                            IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                            IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                            SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                            IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                            IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                            IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                            IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                            PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                            AddedManually = a.AddedManually,
                                            OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                            OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                            OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                        }).FirstOrDefault();

            if (docTypePm != null)
            {
                docTypePm.DocumentTypeCustomFields = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(docTypePm.Id, docTypePm.Tenant).ToList();
                docTypePm.DocumentTypeCopies = documentCopiesQuery.GetDocumentTypeCopiesByDocumentType(docTypePm.Id, null, docTypePm.Tenant);
            }

            return docTypePm;
        }



        public List<DocumentTypeList> GetDocumentTypeListsByObjectTableId(string objectTableid, int tenant)
        {
            List<DocumentTypeList> d = (from a in repository.context.DocumentTypes
                                        where a.Tenant == tenant && a.ObjectTableId == objectTableid && a.IsDocOut
                                        select new DocumentTypeList()
                                        {
                                            Id = a.Id,
                                            Tenant = a.Tenant,
                                            Name = a.Name,
                                            Code = a.Code,
                                            ObjectTableId = a.ObjectTableId,
                                            TemplateFormatCode = a.TemplateFormatCode,
                                            OrderBy = a.OrderBy,
                                            FileName = a.FileName,
                                            AddedManually = a.AddedManually,
                                            OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                            OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                            OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,
                                            DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,

                                        }).ToList();
            return d;
        }

        public List<DocumentTypeList> GetDocumentTypeListsByObjectTableIdForAutomations(string objectTableid, int tenant)
        {
            List<DocumentTypeList> d = (from a in repository.context.DocumentTypes
                                        where a.Tenant == tenant && a.ObjectTableId == objectTableid && a.IsDocOut && a.TemplateFormatCode == "M"
                                        select new DocumentTypeList()
                                        {
                                            Id = a.Id,
                                            Tenant = a.Tenant,
                                            Name = a.Name,
                                            Code = a.Code,
                                            ObjectTableId = a.ObjectTableId,
                                            TemplateFormatCode = a.TemplateFormatCode,
                                            OrderBy = a.OrderBy,
                                            DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                            DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                            FileName = a.FileName,
                                            AddedManually = a.AddedManually,
                                            OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                            OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                            OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                        }).ToList();
            return d;
        }






        public List<DocumentTypeList> GetDocumentTypeListsByObjectTableAndTenant(string objectTableid, int tenant)
        {
            List<DocumentTypeList> d = (from a in repository.context.DocumentTypes
                                        where a.Tenant == tenant && a.ObjectTableId == objectTableid && (a.IsDocIn || a.IsDocOut)
                                        select new DocumentTypeList()
                                        {
                                            Id = a.Id,
                                            Tenant = a.Tenant,
                                            Name = a.Name,
                                            Code = a.Code,
                                            Notes = a.Notes,
                                            IsAir = a.IsAir,
                                            IsOcean = a.IsOcean,
                                            IsInland = a.IsInland,
                                            IsDocIn = a.IsDocIn,
                                            IsDocOut = a.IsDocOut,
                                            ObjectTableId = a.ObjectTableId,
                                            Subject = a.Subject,
                                            DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                            DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                            TemplateFormatCode = a.TemplateFormatCode,
                                            DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                            InActive = a.InActive,
                                            IsMaster = a.IsMaster,
                                            IsDirect = a.IsDirect,
                                            IsHouse = a.IsHouse,
                                            SearchFields = a.SearchFields,
                                            CustomControl = a.CustomControl,
                                            IsCustomerView = a.IsCustomerView,
                                            IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                            IsAgentView = a.IsAgentView,
                                            IsReadOnly = a.IsReadOnly,
                                            LimitedPrintCopyId = a.LimitedPrintCopyId,
                                            IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                            IsCopiedAtSignup = a.IsCopiedAtSignup,
                                            IsEnabledForCustomers = a.IsEnabledForCustomers,
                                            CountryCode = a.CountryCode,
                                            DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                            DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                            OrderBy = a.OrderBy,
                                            FileName = a.FileName,
                                            IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                            IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                            IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                            SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                            IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                            PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                            AddedManually = a.AddedManually,
                                            OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                            OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                            OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                        }).ToList();
            return d;
        }




        public IQueryable<DocumentTypePM> GetDocumentTypesByTenantAndTransportMode(int tenant, string transport)
        {
            IQueryable<DocumentTypePM> d = null;
            DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
            DocumentTypeCopyQuery documentCopiesQuery = new DocumentTypeCopyQuery(tenant);

            switch (transport)
            {
                case "A":
                    {
                        d = from a in repository.context.DocumentTypes.Include("DocumentTypeCategory")
                            where a.Tenant == tenant && a.IsAir == true
                            select new DocumentTypePM()
                            {
                                InActive = a.InActive,
                                Id = a.Id,
                                IsAir = a.IsAir,
                                IsDocIn = a.IsDocIn,
                                IsDocOut = a.IsDocOut,
                                IsInland = a.IsInland,
                                IsOcean = a.IsOcean,
                                Name = a.Name,
                                Notes = a.Notes,
                                Tenant = a.Tenant,
                                SearchFields = a.SearchFields,
                                Code = a.Code,
                                DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                ObjectTableId = a.ObjectTableId,
                                Subject = a.Subject,
                                DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                TemplateFormatCode = a.TemplateFormatCode,
                                IsMaster = a.IsMaster,
                                IsDirect = a.IsDirect,
                                IsHouse = a.IsHouse,
                                CustomControl = a.CustomControl,
                                AgentRoleId = a.AgentRoleId,
                                CustomerRoleId = a.CustomerRoleId,
                                IsAgentView = a.IsAgentView,
                                IsCustomerView = a.IsCustomerView,
                                IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                IsReadOnly = a.IsReadOnly,
                                LimitedPrintCopyId = a.LimitedPrintCopyId,
                                IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                IsCopiedAtSignup = a.IsCopiedAtSignup,
                                IsEnabledForCustomers = a.IsEnabledForCustomers,
                                CountryCode = a.CountryCode,
                                DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                OrderBy = a.OrderBy,
                                FileName = a.FileName,
                                IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                AddedManually = a.AddedManually,
                                OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,


                            };
                        break;
                    }
                case "O":
                    {
                        d = from a in repository.context.DocumentTypes.Include("DocumentTypeCategory")
                            where a.Tenant == tenant && a.IsOcean == true
                            select new DocumentTypePM()
                            {
                                Id = a.Id,
                                IsAir = a.IsAir,
                                IsDocIn = a.IsDocIn,
                                IsDocOut = a.IsDocOut,
                                IsInland = a.IsInland,
                                IsOcean = a.IsOcean,
                                Name = a.Name,
                                Notes = a.Notes,
                                SearchFields = a.SearchFields,
                                Tenant = a.Tenant,
                                InActive = a.InActive,
                                Code = a.Code,
                                DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                ObjectTableId = a.ObjectTableId,
                                Subject = a.Subject,
                                DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                TemplateFormatCode = a.TemplateFormatCode,
                                IsMaster = a.IsMaster,
                                IsDirect = a.IsDirect,
                                IsHouse = a.IsHouse,
                                CustomControl = a.CustomControl,
                                AgentRoleId = a.AgentRoleId,
                                CustomerRoleId = a.CustomerRoleId,
                                IsAgentView = a.IsAgentView,
                                IsCustomerView = a.IsCustomerView,
                                IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                IsReadOnly = a.IsReadOnly,
                                LimitedPrintCopyId = a.LimitedPrintCopyId,
                                IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                IsCopiedAtSignup = a.IsCopiedAtSignup,
                                IsEnabledForCustomers = a.IsEnabledForCustomers,
                                CountryCode = a.CountryCode,
                                DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                OrderBy = a.OrderBy,
                                FileName = a.FileName,
                                IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                AddedManually = a.AddedManually,
                                OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                            };
                        break;
                    }
                case "I":
                    {
                        d = from a in repository.context.DocumentTypes.Include("DocumentTypeCategory")
                            where a.Tenant == tenant && a.IsInland == true
                            select new DocumentTypePM()
                            {
                                Id = a.Id,
                                IsAir = a.IsAir,
                                IsDocIn = a.IsDocIn,
                                IsDocOut = a.IsDocOut,
                                IsInland = a.IsInland,
                                IsOcean = a.IsOcean,
                                Name = a.Name,
                                Notes = a.Notes,
                                SearchFields = a.SearchFields,
                                Tenant = a.Tenant,
                                InActive = a.InActive,
                                Code = a.Code,
                                DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                ObjectTableId = a.ObjectTableId,
                                Subject = a.Subject,
                                DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                TemplateFormatCode = a.TemplateFormatCode,
                                IsMaster = a.IsMaster,
                                IsDirect = a.IsDirect,
                                IsHouse = a.IsHouse,
                                CustomControl = a.CustomControl,
                                AgentRoleId = a.AgentRoleId,
                                CustomerRoleId = a.CustomerRoleId,
                                IsAgentView = a.IsAgentView,
                                IsCustomerView = a.IsCustomerView,
                                IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                IsReadOnly = a.IsReadOnly,
                                LimitedPrintCopyId = a.LimitedPrintCopyId,
                                IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                IsCopiedAtSignup = a.IsCopiedAtSignup,
                                IsEnabledForCustomers = a.IsEnabledForCustomers,
                                CountryCode = a.CountryCode,
                                DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                OrderBy = a.OrderBy,
                                FileName = a.FileName,
                                IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                AddedManually = a.AddedManually,
                                OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                            };
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            foreach (DocumentTypePM doc in d)
            {
                doc.DocumentTypeCustomFields = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(doc.Id, doc.Tenant).ToList();
                doc.DocumentTypeCopies = documentCopiesQuery.GetDocumentTypeCopiesByDocumentType(doc.Id, null, doc.Tenant);
            }
            return d;
        }


        public string GetDocumentTypeListIdByCodeAndTenant(string Code, int tenant)
        {
            string Id = (from a in repository.context.DocumentTypes
                         where a.Code == Code && a.Tenant == tenant
                         select a.Id).FirstOrDefault();

            return Id;
        }




        public IQueryable<DocumentTypeList> GetIQueryableEntityList(IQueryable<DocumentType> iQueryable)
        {
            IQueryable<DocumentTypeList> result = from documentType in iQueryable.Include("ObjectTable").Include("DocumentTypeCategory")
                                                  select new DocumentTypeList()
                                                  {
                                                      Tenant = documentType.Tenant,
                                                      Code = documentType.Code,
                                                      Id = documentType.Id,
                                                      IsAir = documentType.IsAir,
                                                      IsDocIn = documentType.IsDocIn,
                                                      IsDocOut = documentType.IsDocOut,
                                                      IsInland = documentType.IsInland,
                                                      IsOcean = documentType.IsOcean,
                                                      Name = documentType.Name,
                                                      Notes = documentType.Notes,
                                                      ObjectTableId = documentType.ObjectTableId,
                                                      Subject = documentType.Subject,
                                                      DocumentTypeDefaultHTMLTemplateId = documentType.DocumentTypeDefaultHTMLTemplateId,
                                                      DocumentTypeDefaultReportTemplateId = documentType.DocumentTypeDefaultReportTemplateId,
                                                      TemplateFormatCode = documentType.TemplateFormatCode,
                                                      DocumentTypeDefaultEditorTool = documentType.DocumentTypeDefaultEditorTool,
                                                      IsMaster = documentType.IsMaster,
                                                      IsDirect = documentType.IsDirect,
                                                      IsHouse = documentType.IsHouse,
                                                      SearchFields = documentType.SearchFields,
                                                      IsAgentView = documentType.IsAgentView,
                                                      IsCustomerView = documentType.IsCustomerView,
                                                      IsCustomerUploadPermission = documentType.IsCustomerUploadPermission,
                                                      ObjectTableName = documentType.ObjectTable != null ? documentType.ObjectTable.Name : "",
                                                      IsReadOnly = documentType.IsReadOnly,
                                                      LimitedPrintCopyId = documentType.LimitedPrintCopyId,
                                                      IsDocumentOneTimePrintLimited = documentType.IsDocumentOneTimePrintLimited,
                                                      InActive = documentType.InActive,
                                                      IsCopiedAtSignup = documentType.IsCopiedAtSignup,
                                                      IsEnabledForCustomers = documentType.IsEnabledForCustomers,
                                                      CountryCode = documentType.CountryCode,
                                                      DocumentTypeCategoryCode = documentType.DocumentTypeCategoryCode,
                                                      DocumentTypeCategoryName = documentType.DocumentTypeCategory != null ? documentType.DocumentTypeCategory.Name : null,
                                                      OrderBy = documentType.OrderBy,
                                                      FileName = documentType.FileName,
                                                      IsAgentSharedInDirect = documentType.IsAgentSharedInDirect,
                                                      IsAgentSharedInHouse = documentType.IsAgentSharedInHouse,
                                                      IsAgentSharedInMaster = documentType.IsAgentSharedInMaster,
                                                      SharedDocumentTypeCopyId = documentType.SharedDocumentTypeCopyId,
                                                      IsSystemAdditionalPrintingFields = documentType.IsSystemAdditionalPrintingFields,
                                                      PrintingFieldsScreenCode = documentType.PrintingFieldsScreenCode,
                                                      AddedManually = documentType.AddedManually,
                                                      OnPrintPopulateDateFieldName = documentType.OnPrintPopulateDateFieldName,
                                                      OnSendPopulateDateFieldName = documentType.OnSendPopulateDateFieldName,
                                                      OnUploadPopulateDateFieldName = documentType.OnUploadPopulateDateFieldName,


                                                  };

            return result;
        }


        internal DocumentTypePM GetDocumentType(string documentTypeId)
        {
            DocumentTypePM docTypePm = (from a in repository.context.DocumentTypes.Include("ObjectTable").Include("DocumentTypeCategory")
                                        where a.Id == documentTypeId
                                        select new DocumentTypePM()
                                        {
                                            Id = a.Id,
                                            IsAir = a.IsAir,
                                            IsDocIn = a.IsDocIn,
                                            IsDocOut = a.IsDocOut,
                                            IsInland = a.IsInland,
                                            IsOcean = a.IsOcean,
                                            Name = a.Name,
                                            Notes = a.Notes,
                                            Tenant = a.Tenant,
                                            SearchFields = a.SearchFields,
                                            Code = a.Code,
                                            InActive = a.InActive,
                                            ObjectTableId = a.ObjectTableId,
                                            Subject = a.Subject,
                                            DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                            DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                            TemplateFormatCode = a.TemplateFormatCode,
                                            DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                            IsMaster = a.IsMaster,
                                            IsDirect = a.IsDirect,
                                            IsHouse = a.IsHouse,
                                            CustomControl = a.CustomControl,
                                            AgentRoleId = a.AgentRoleId,
                                            CustomerRoleId = a.CustomerRoleId,
                                            IsAgentView = a.IsAgentView,
                                            IsCustomerView = a.IsCustomerView,
                                            IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                            IsReadOnly = a.IsReadOnly,
                                            ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                            LimitedPrintCopyId = a.LimitedPrintCopyId,
                                            IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                            IsCopiedAtSignup = a.IsCopiedAtSignup,
                                            IsEnabledForCustomers = a.IsEnabledForCustomers,
                                            CountryCode = a.CountryCode,
                                            DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                            DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                            OrderBy = a.OrderBy,
                                            FileName = a.FileName,
                                            IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                            IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                            IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                            SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                            IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                            IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                            IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                            IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                            PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                            AddedManually = a.AddedManually,
                                            OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                            OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                            OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                        }).FirstOrDefault();



            return docTypePm;

        }

        public List<DocumentTypePM> GetDocumentTypePMsByEnityIdAndTenant(string transportModeId, string shipmentLevelCode, string objecttableId, int tenant, string childrenObjectTableIds = null)
        {

            string[] childrenIds = !string.IsNullOrEmpty(childrenObjectTableIds) ? childrenObjectTableIds.Split(',') : new string[] { "" };

            IQueryable<DocumentTypePM> documentTypes = (from a in repository.context.DocumentTypes.Include("DocumentTypeCategory")
                                                        where a.Tenant == tenant && (a.ObjectTableId == objecttableId || childrenObjectTableIds.Contains(a.ObjectTableId)) && a.IsDocOut
                                                        select new DocumentTypePM()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            Name = a.Name,
                                                            Code = a.Code,
                                                            Notes = a.Notes,
                                                            IsAir = a.IsAir,
                                                            IsOcean = a.IsOcean,
                                                            IsInland = a.IsInland,
                                                            IsDocIn = a.IsDocIn,
                                                            IsDocOut = a.IsDocOut,
                                                            ObjectTableId = a.ObjectTableId,
                                                            Subject = a.Subject,
                                                            DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                                            DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                                            TemplateFormatCode = a.TemplateFormatCode,
                                                            DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                                            InActive = a.InActive,
                                                            IsMaster = a.IsMaster,
                                                            IsDirect = a.IsDirect,
                                                            IsHouse = a.IsHouse,
                                                            SearchFields = a.SearchFields,
                                                            CustomControl = a.CustomControl,
                                                            CustomerRoleId = a.CustomerRoleId,
                                                            AgentRoleId = a.AgentRoleId,
                                                            IsCustomerView = a.IsCustomerView,
                                                            IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                                            IsAgentView = a.IsAgentView,
                                                            IsReadOnly = a.IsReadOnly,
                                                            LimitedPrintCopyId = a.LimitedPrintCopyId,
                                                            IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                                            IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                            IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                            DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                                            CountryCode = a.CountryCode,
                                                            DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                                            OrderBy = a.OrderBy,
                                                            FileName = a.FileName,
                                                            IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                                            IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                                            IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                                            SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                                            IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                                            IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                                            IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                                            IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                                            PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                                            AddedManually = a.AddedManually,
                                                            OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                                            OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                                            OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,


                                                        });

            if (string.IsNullOrEmpty(childrenObjectTableIds))
            {
                documentTypes = documentTypes.Where(d => d.ObjectTableId == objecttableId);
            }

            documentTypes = FilterDocumentTypePMByTransportModeIdAndShipmentLevelCode(transportModeId, shipmentLevelCode, documentTypes);

            return documentTypes.ToList();
        }

        private IQueryable<DocumentTypePM> FilterDocumentTypePMByTransportModeIdAndShipmentLevelCode(string transportModeId, string shipmentLevelCode, IQueryable<DocumentTypePM> documentTypes)
        {
            #region transportModeId

            if (!string.IsNullOrEmpty(transportModeId))
            {
                switch (transportModeId)
                {
                    case "A":
                        {

                            documentTypes = documentTypes.Where(a => a.IsAir == true);
                            break;
                        }
                    case "O":
                        {
                            documentTypes = documentTypes.Where(a => a.IsOcean == true);
                            break;
                        }
                    case "I":
                        {
                            documentTypes = documentTypes.Where(a => a.IsInland == true);
                            break;
                        }
                }
            }


            #endregion transportModeId


            #region shipmentLevelCode
            if (!string.IsNullOrEmpty(shipmentLevelCode))
            {
                switch (shipmentLevelCode)
                {
                    case "C":
                        {
                            documentTypes = documentTypes.Where(a => a.IsMaster == true);
                            break;
                        }

                    case "M":
                        {
                            documentTypes = documentTypes.Where(a => a.IsMaster == true);
                            break;
                        }

                    case "D":
                        {
                            documentTypes = documentTypes.Where(a => a.IsDirect == true);
                            break;
                        }
                    case "H":
                        {
                            documentTypes = documentTypes.Where(a => a.IsHouse == true);
                            break;
                        }
                }

            }
            #endregion shipmentLevelCode
            return documentTypes;
        }

        public List<DocumentTypeList> GetDocumentTypeListsByEnityIdAndTenant(string transportModeId, string shipmentLevelCode, string objecttableId, int tenant)
        {
            IQueryable<DocumentTypeList> documentTypes = null;
            documentTypes = (from a in repository.context.DocumentTypes.Include("DocumentTypeCategory")
                             where a.Tenant == tenant && a.ObjectTableId == objecttableId && a.IsDocIn && !a.InActive
                             select new DocumentTypeList()
                             {
                                 Id = a.Id,
                                 Tenant = a.Tenant,
                                 Name = a.Name,
                                 Code = a.Code,
                                 Notes = a.Notes,
                                 IsAir = a.IsAir,
                                 IsOcean = a.IsOcean,
                                 IsInland = a.IsInland,
                                 IsDocIn = a.IsDocIn,
                                 IsDocOut = a.IsDocOut,
                                 ObjectTableId = a.ObjectTableId,
                                 Subject = a.Subject,
                                 DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                 DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                 TemplateFormatCode = a.TemplateFormatCode,
                                 DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                 InActive = a.InActive,
                                 IsMaster = a.IsMaster,
                                 IsDirect = a.IsDirect,
                                 IsHouse = a.IsHouse,
                                 SearchFields = a.SearchFields,
                                 CustomControl = a.CustomControl,
                                 IsCustomerView = a.IsCustomerView,
                                 IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                 IsAgentView = a.IsAgentView,
                                 IsReadOnly = a.IsReadOnly,
                                 LimitedPrintCopyId = a.LimitedPrintCopyId,
                                 IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                 IsCopiedAtSignup = a.IsCopiedAtSignup,
                                 IsEnabledForCustomers = a.IsEnabledForCustomers,
                                 DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                 CountryCode = a.CountryCode,
                                 DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                 OrderBy = a.OrderBy,
                                 FileName = a.FileName,
                                 IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                 IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                 IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                 SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                 IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                 PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                 AddedManually = a.AddedManually,
                                 OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                 OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                 OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,


                             });

            documentTypes = FilterDocumentTypeListByTransportModeIdAndShipmentLevelCode(transportModeId, shipmentLevelCode, documentTypes);


            return documentTypes.ToList();
        }

        private IQueryable<DocumentTypeList> FilterDocumentTypeListByTransportModeIdAndShipmentLevelCode(string transportModeId, string shipmentLevelCode, IQueryable<DocumentTypeList> documentTypes)
        {
            #region transportModeId

            if (!string.IsNullOrEmpty(transportModeId))
            {
                switch (transportModeId)
                {
                    case "A":
                        {

                            documentTypes = documentTypes.Where(a => a.IsAir == true);
                            break;
                        }
                    case "O":
                        {
                            documentTypes = documentTypes.Where(a => a.IsOcean == true);
                            break;
                        }
                    case "I":
                        {
                            documentTypes = documentTypes.Where(a => a.IsInland == true);
                            break;
                        }
                }
            }


            #endregion transportModeId


            #region shipmentLevelCode
            if (!string.IsNullOrEmpty(shipmentLevelCode))
            {
                switch (shipmentLevelCode)
                {
                    case "C":
                        {
                            documentTypes = documentTypes.Where(a => a.IsMaster == true);
                            break;
                        }

                    case "M":
                        {
                            documentTypes = documentTypes.Where(a => a.IsMaster == true);
                            break;
                        }

                    case "D":
                        {
                            documentTypes = documentTypes.Where(a => a.IsDirect == true);
                            break;
                        }
                    case "H":
                        {
                            documentTypes = documentTypes.Where(a => a.IsHouse == true);
                            break;
                        }
                }

            }
            #endregion shipmentLevelCode
            return documentTypes;
        }

        public List<DocumentTypePM> GetDocumentTypePMsByObjectTableAndTenant(string objectTableid, int tenant)
        {

            if (isFullAccounting)
            {
                List<DocumentTypePM> d = (from a in repository.context.DocumentTypes.Include("DocumentTypeCategory")
                                          where !a.InActive & a.Tenant == tenant && a.ObjectTableId == objectTableid && (a.IsDocIn || a.IsDocOut)
                                          select new DocumentTypePM()
                                          {
                                              Id = a.Id,
                                              Tenant = a.Tenant,
                                              Name = a.Name,
                                              Code = a.Code,
                                              Notes = a.Notes,
                                              IsAir = a.IsAir,
                                              IsOcean = a.IsOcean,
                                              IsInland = a.IsInland,
                                              IsDocIn = a.IsDocIn,
                                              IsDocOut = a.IsDocOut,
                                              ObjectTableId = a.ObjectTableId,
                                              Subject = a.Subject,
                                              DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                              DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                              TemplateFormatCode = a.TemplateFormatCode,
                                              DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                              InActive = a.InActive,
                                              IsMaster = a.IsMaster,
                                              IsDirect = a.IsDirect,
                                              IsHouse = a.IsHouse,
                                              SearchFields = a.SearchFields,
                                              CustomControl = a.CustomControl,
                                              CustomerRoleId = a.CustomerRoleId,
                                              AgentRoleId = a.AgentRoleId,
                                              IsCustomerView = a.IsCustomerView,
                                              IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                              IsAgentView = a.IsAgentView,
                                              IsReadOnly = a.IsReadOnly,
                                              LimitedPrintCopyId = a.LimitedPrintCopyId,
                                              IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                              IsCopiedAtSignup = a.IsCopiedAtSignup,
                                              IsEnabledForCustomers = a.IsEnabledForCustomers,
                                              CountryCode = a.CountryCode,
                                              DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                              DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                              OrderBy = a.OrderBy,
                                              FileName = a.FileName,
                                              IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                              IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                              IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                              SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                              IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                              IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                              IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                              IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                              PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                              AddedManually = a.AddedManually,
                                              OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                              OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                              OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                          }).ToList();

                return d;
            } else
            {
                List<DocumentTypePM> d = (from a in repository.context.DocumentTypes.Include("DocumentTypeCategory")
                                          where a.Tenant == tenant && a.ObjectTableId == objectTableid && (a.IsDocIn || a.IsDocOut)
                                          select new DocumentTypePM()
                                          {
                                              Id = a.Id,
                                              Tenant = a.Tenant,
                                              Name = a.Name,
                                              Code = a.Code,
                                              Notes = a.Notes,
                                              IsAir = a.IsAir,
                                              IsOcean = a.IsOcean,
                                              IsInland = a.IsInland,
                                              IsDocIn = a.IsDocIn,
                                              IsDocOut = a.IsDocOut,
                                              ObjectTableId = a.ObjectTableId,
                                              Subject = a.Subject,
                                              DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                              DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                              TemplateFormatCode = a.TemplateFormatCode,
                                              DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                              InActive = a.InActive,
                                              IsMaster = a.IsMaster,
                                              IsDirect = a.IsDirect,
                                              IsHouse = a.IsHouse,
                                              SearchFields = a.SearchFields,
                                              CustomControl = a.CustomControl,
                                              CustomerRoleId = a.CustomerRoleId,
                                              AgentRoleId = a.AgentRoleId,
                                              IsCustomerView = a.IsCustomerView,
                                              IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                              IsAgentView = a.IsAgentView,
                                              IsReadOnly = a.IsReadOnly,
                                              LimitedPrintCopyId = a.LimitedPrintCopyId,
                                              IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                              IsCopiedAtSignup = a.IsCopiedAtSignup,
                                              IsEnabledForCustomers = a.IsEnabledForCustomers,
                                              CountryCode = a.CountryCode,
                                              DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                              DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                              OrderBy = a.OrderBy,
                                              FileName = a.FileName,
                                              IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                              IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                              IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                              SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                              IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                              IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                              IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                              IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                              PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                              AddedManually = a.AddedManually,
                                              OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                              OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                              OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                          }).ToList();

                return d;
            }
            
        }

        private bool IsFullAccountingActivated(int tenant)
        {
            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantPOCO = tenantRepository.GetSingleTenant(tenant);
            bool isFullAccountingActivated = tenantPOCO.AccountingActivated;
            return isFullAccountingActivated;
        }


        public List<DocumentTypePM> GetFollowUpDocumentTypeByEntityId(string entityId, string objectTableName, int tenant)
        {
            FollowUpQuery followUpQuery = new FollowUpQuery();
            FollowUpRepository followUpRepository = new FollowUpRepository(tenant);
            List<FollowUp> followUpLists = followUpRepository.GetFollowUpsByEntityId(entityId, objectTableName, tenant);
            List<string> followUpDocumenttypeIds = new List<string>();
            foreach (FollowUp item in followUpLists)
            {
                if (!string.IsNullOrEmpty(item.DocumentTypeId)) followUpDocumenttypeIds.Add(item.DocumentTypeId);
            }

            List<DocumentTypePM> documentTypes = (from a in repository.context.DocumentTypes
                                                  where a.Tenant == tenant && followUpDocumenttypeIds.Contains(a.Id) && a.IsDocOut
                                                  select new DocumentTypePM()
                                                  {
                                                      Id = a.Id,
                                                      Tenant = a.Tenant,
                                                      Name = a.Name,
                                                      Code = a.Code,
                                                      Notes = a.Notes,
                                                      IsAir = a.IsAir,
                                                      IsOcean = a.IsOcean,
                                                      IsInland = a.IsInland,
                                                      IsDocIn = a.IsDocIn,
                                                      IsDocOut = a.IsDocOut,
                                                      ObjectTableId = a.ObjectTableId,
                                                      Subject = a.Subject,
                                                      DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                                      DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                                      TemplateFormatCode = a.TemplateFormatCode,
                                                      DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                                      InActive = a.InActive,
                                                      IsMaster = a.IsMaster,
                                                      IsDirect = a.IsDirect,
                                                      IsHouse = a.IsHouse,
                                                      SearchFields = a.SearchFields,
                                                      CustomControl = a.CustomControl,
                                                      CustomerRoleId = a.CustomerRoleId,
                                                      AgentRoleId = a.AgentRoleId,
                                                      IsCustomerView = a.IsCustomerView,
                                                      IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                                      IsAgentView = a.IsAgentView,
                                                      IsReadOnly = a.IsReadOnly,
                                                      LimitedPrintCopyId = a.LimitedPrintCopyId,
                                                      IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                                      IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                      IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                      DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                                      CountryCode = a.CountryCode,
                                                      DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                                      OrderBy = a.OrderBy,
                                                      FileName = a.FileName,
                                                      IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                                      IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                                      IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                                      SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                                      IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                                      IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                                      IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                                      IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                                      PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                                      AddedManually = a.AddedManually,
                                                      OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                                      OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                                      OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,


                                                  }).ToList();

            return documentTypes;
        }

        public DocumentTypeList GetSingleListByCodeAndTenant(string code, int tenant)
        {

            DocumentTypeList docTypeList = (from a in repository.context.DocumentTypes.Include("ObjectTable").Include("DocumentTypeCategory")
                                            where a.Code == code && a.Tenant == tenant
                                            select new DocumentTypeList()
                                            {
                                                Id = a.Id,
                                                IsAir = a.IsAir,
                                                IsDocIn = a.IsDocIn,
                                                IsDocOut = a.IsDocOut,
                                                IsInland = a.IsInland,
                                                IsOcean = a.IsOcean,
                                                Name = a.Name,
                                                Notes = a.Notes,
                                                Tenant = a.Tenant,
                                                SearchFields = a.SearchFields,
                                                Code = a.Code,
                                                InActive = a.InActive,
                                                ObjectTableId = a.ObjectTableId,
                                                Subject = a.Subject,
                                                DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                                DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                                TemplateFormatCode = a.TemplateFormatCode,
                                                DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                                IsMaster = a.IsMaster,
                                                IsDirect = a.IsDirect,
                                                IsHouse = a.IsHouse,
                                                CustomControl = a.CustomControl,
                                                IsAgentView = a.IsAgentView,
                                                IsCustomerView = a.IsCustomerView,
                                                IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                                IsReadOnly = a.IsReadOnly,
                                                ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                                LimitedPrintCopyId = a.LimitedPrintCopyId,
                                                IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                                IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                CountryCode = a.CountryCode,
                                                DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                                DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                                OrderBy = a.OrderBy,
                                                FileName = a.FileName,
                                                IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                                IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                                IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                                SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                                IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                                PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                                AddedManually = a.AddedManually,
                                                OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                                OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                                OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                            }).FirstOrDefault();


            return docTypeList;
        }

        public List<DocumentTypePM> GetTop5DocumentTypePMsByObjectTableId(string objectTableid, int tenant)
        {
            List<DocumentTypePM> d = (from a in repository.context.DocumentTypes
                                      orderby a.OrderBy ascending
                                      where a.Tenant == tenant && a.ObjectTableId == objectTableid && a.InActive == false
                                      select new DocumentTypePM()
                                      {
                                          Id = a.Id,
                                          IsAir = a.IsAir,
                                          IsDocIn = a.IsDocIn,
                                          IsDocOut = a.IsDocOut,
                                          IsInland = a.IsInland,
                                          IsOcean = a.IsOcean,
                                          Name = a.Name,
                                          Notes = a.Notes,
                                          Tenant = a.Tenant,
                                          SearchFields = a.SearchFields,
                                          Code = a.Code,
                                          InActive = a.InActive,
                                          ObjectTableId = a.ObjectTableId,
                                          Subject = a.Subject,
                                          DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                          DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                          TemplateFormatCode = a.TemplateFormatCode,
                                          DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                          IsMaster = a.IsMaster,
                                          IsDirect = a.IsDirect,
                                          IsHouse = a.IsHouse,
                                          CustomControl = a.CustomControl,
                                          AgentRoleId = a.AgentRoleId,
                                          CustomerRoleId = a.CustomerRoleId,
                                          IsAgentView = a.IsAgentView,
                                          IsCustomerView = a.IsCustomerView,
                                          IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                          IsReadOnly = a.IsReadOnly,
                                          ObjectTableName = a.ObjectTable != null ? a.ObjectTable.Name : null,
                                          LimitedPrintCopyId = a.LimitedPrintCopyId,
                                          IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                          IsCopiedAtSignup = a.IsCopiedAtSignup,
                                          IsEnabledForCustomers = a.IsEnabledForCustomers,
                                          CountryCode = a.CountryCode,
                                          DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                          DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                                          OrderBy = a.OrderBy,
                                          FileName = a.FileName,
                                          IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                          IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                          IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                          SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                          IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                          IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                          IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                          IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                          PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                          AddedManually = a.AddedManually,

                                      }).Take(4).ToList();
            return d;
        }


        public List<DocumentTypePM> GetDocumentTypesPMByObjectTableIdForDocumentPremissions(string objectTableid, int tenant)
        {
            List<DocumentTypePM> documentTypes = (from a in repository.context.DocumentTypes
                                                  where a.Tenant == tenant && a.ObjectTableId == objectTableid && (a.IsDocIn || (a.TemplateFormatCode == "P" && a.IsDocOut)) && !a.InActive
                                                  select new DocumentTypePM()
                                                  {
                                                      Id = a.Id,
                                                      IsAir = a.IsAir,
                                                      IsDocIn = a.IsDocIn,
                                                      IsDocOut = a.IsDocOut,
                                                      IsInland = a.IsInland,
                                                      IsOcean = a.IsOcean,
                                                      Name = a.Name,
                                                      Notes = a.Notes,
                                                      Tenant = a.Tenant,
                                                      SearchFields = a.SearchFields,
                                                      Code = a.Code,
                                                      InActive = a.InActive,
                                                      ObjectTableId = a.ObjectTableId,
                                                      Subject = a.Subject,
                                                      DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                                      DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                                      TemplateFormatCode = a.TemplateFormatCode,
                                                      DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                                      IsMaster = a.IsMaster,
                                                      IsDirect = a.IsDirect,
                                                      IsHouse = a.IsHouse,
                                                      CustomControl = a.CustomControl,
                                                      AgentRoleId = a.AgentRoleId,
                                                      CustomerRoleId = a.CustomerRoleId,
                                                      IsAgentView = a.IsAgentView,
                                                      IsCustomerView = a.IsCustomerView,
                                                      IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                                                      IsReadOnly = a.IsReadOnly,
                                                      LimitedPrintCopyId = a.LimitedPrintCopyId,
                                                      IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                                                      IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                      IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                      CountryCode = a.CountryCode,
                                                      DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                                                      OrderBy = a.OrderBy,
                                                      FileName = a.FileName,
                                                      IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                                      IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                                      IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                                      SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                                      IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                                                      IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                                                      IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                                                      IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                                                      PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                                                      AddedManually = a.AddedManually,
                                                      OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                                                      OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                                                      OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,

                                                  }).ToList();

            DocumentTypeCopyQuery documentCopiesQuery = new DocumentTypeCopyQuery(tenant);
            foreach (DocumentTypePM doc in documentTypes)
            {
                doc.DocumentTypeCopies = documentCopiesQuery.GetDocumentTypeCopiesByDocumentType(doc.Id, null, doc.Tenant);
            }

            return documentTypes;
        }


        public List<ShipmentShareDocumentsData> GetShareDocumentByObjectTableAndEntityIdAndshipmentLevel(string entityId, string agentId, string agentReference, string objectTableId, string shipmentLevelCode, int tenant, string shareDocumentsFrom)
        {

            List<ShipmentShareDocumentsData> shipmentShareDocumentsDataLists = new List<ShipmentShareDocumentsData>();
            List<DocumentTypeList> documentTypes = new List<DocumentTypeList>();
            IQueryable<DocumentTypeList> documentTypeLists = (from a in repository.context.DocumentTypes
                                                              where a.Tenant == tenant && (a.IsAgentSharedInHouse || a.IsAgentSharedInDirect || a.IsAgentSharedInMaster) && !a.InActive && a.ObjectTableId == objectTableId
                                                              select new DocumentTypeList()
                                                              {
                                                                  Id = a.Id,
                                                                  Tenant = a.Tenant,
                                                                  Name = a.Name,
                                                                  Code = a.Code,
                                                                  IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                                                                  IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                                                                  IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                                                                  SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                                                                  IsDocIn = a.IsDocIn,
                                                                  IsDocOut = a.IsDocOut,
                                                                  AddedManually = a.AddedManually,

                                                              });

            if (shipmentLevelCode == "C") documentTypeLists = documentTypeLists.Where(d => d.IsAgentSharedInMaster || d.IsAgentSharedInHouse);
            else if (shipmentLevelCode == "D") documentTypeLists = documentTypeLists.Where(d => d.IsAgentSharedInDirect);

            List<string> documentTypeIds = new List<string>();
            List<string> documentTypeCopyIds = new List<string>();
            List<string> documentOutCopyIds = new List<string>();

            if (documentTypeLists.Count() > 0)
            {


                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                AgentRepository agentRepository = new AgentRepository(commonContext);
                AgentSharedLogisticsKeyRepository agentSharedLogisticsKeyRepository = new AgentSharedLogisticsKeyRepository();
                if (shareDocumentsFrom == "Email")
                {
                    List<HouseSL> houseSLLists = new List<HouseSL>();
                    IShipmentsContext myContext = ShipmentsContext.GetContext(tenant);
                    ShipmentConsoleShipmentQuery shipmentConsoleShipmentQuery = new ShipmentConsoleShipmentQuery(myContext);
                    List<Shipment> housesShipments = shipmentConsoleShipmentQuery.GetMasterConnectedHouseShipments(entityId, tenant);
                    housesShipments.ForEach(houseShipment =>
                    {
                        houseSLLists.Add(new HouseSL { ShipmentNumber = houseShipment.ShipmentNumber });
                    });

                    return GetSharedDocumentTypes(new ShareDocumentTypesArgs { EntityId = entityId, AgentId = agentId, AgentReference = agentReference, ShipmentLevelCode = shipmentLevelCode, Tenant = tenant, ShareDocumentsFrom = shareDocumentsFrom, ShipmentShareDocumentsDataLists = shipmentShareDocumentsDataLists, DocumentTypeLists = documentTypeLists, DocumentTypeIds = documentTypeIds, DocumentTypeCopyIds = documentTypeCopyIds, DocumentOutCopyIds = documentOutCopyIds, AgentSharedLogisticsKey = null, AgentSharedManifestId = "", HouseSLLists = houseSLLists });

                }
                else
                {
                    Agent currentAgent = agentRepository.GetSingleAgent(tenant, agentId);
                    if (currentAgent != null)
                    {
                        AgentSharedLogisticsKey agentSharedLogisticsKey = agentSharedLogisticsKeyRepository.GetSingleAgentSharedLogisticsKey(currentAgent.AgentSharedLogisticsKey);

                        if (agentSharedLogisticsKey != null)
                        {
                            int sourceAgentTenant = tenant;
                            string agentSharedManifestId = "";
                            List<HouseSL> houseSLLists = new List<HouseSL>();


                            int destinationAgentTenant = GetDestinationAgentTenant(tenant, agentSharedLogisticsKey);

                            AgentSharedManifestQuery agentSharedManifestQuery = new AgentSharedManifestQuery(tenant);
                            AgentSharedManifestPM agentSharedManifest = agentSharedManifestQuery.GetAgentSharedManifestByAgentReference(agentReference, destinationAgentTenant);

                            if (agentSharedManifest != null)
                            {
                                agentSharedManifestId = agentSharedManifest.Id;
                                if (!string.IsNullOrEmpty(agentSharedManifest.ManifestXML))
                                {
                                    ManifestSL manifestSL = LogitudeXmlSerializer.DeserializeObject<ManifestSL>(agentSharedManifest.ManifestXML);
                                    if (manifestSL != null) houseSLLists = manifestSL.Houses;
                                }

                            }
                            else
                            {
                                CommunicationLogRepository communicationLogRepository = new CommunicationLogRepository(tenant);
                                CommunicationLog commLog = communicationLogRepository.GetCommunicationLogByEntityIdAndQueueNameAndSubject(entityId, "AgentsSharedLogisticsQueue", "Shared Manifest", tenant);
                                if (commLog != null)
                                {
                                    Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                                    {
                                        FileName = commLog.Document.Id,
                                        FolderName = commLog.Document.Folder,
                                        Extension = commLog.Document.Extension,
                                        Tenant = commLog.Document.Tenant,
                                        FileSize = commLog.Document.FileSize,
                                    };
                                    Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                                    byte[] datainByte = storageservice.Read(fileInfo);
                                    if (datainByte != null)
                                    {
                                        ManifestSL manifestSL = LogitudeXmlSerializer.DeserializeObject<ManifestSL>(datainByte);

                                        if (manifestSL != null)
                                        {
                                            agentSharedManifestId = manifestSL.AgentSharedManifestId;
                                            houseSLLists = manifestSL.Houses;
                                        }
                                    }

                                }
                            }

                            if (!string.IsNullOrEmpty(agentSharedManifestId))
                            {
                                shipmentShareDocumentsDataLists = GetSharedDocumentTypes(new ShareDocumentTypesArgs { EntityId = entityId, AgentId = agentId, AgentReference = agentReference, ShipmentLevelCode = shipmentLevelCode, Tenant = tenant, ShareDocumentsFrom = shareDocumentsFrom, ShipmentShareDocumentsDataLists = shipmentShareDocumentsDataLists, DocumentTypeLists = documentTypeLists, DocumentTypeIds = documentTypeIds, DocumentTypeCopyIds = documentTypeCopyIds, DocumentOutCopyIds = documentOutCopyIds, AgentSharedLogisticsKey = agentSharedLogisticsKey, AgentSharedManifestId = agentSharedManifestId, HouseSLLists = houseSLLists });
                            }

                        }
                    }
                }
            }


            return shipmentShareDocumentsDataLists;
        }

        private List<ShipmentShareDocumentsData> GetSharedDocumentTypes(ShareDocumentTypesArgs shareDocumentTypesArgs)
        {
            List<DocumentTypeList> documentTypes;
            #region House Shipment
            List<ShipmentList> shipmentLists = new List<ShipmentList>();
            List<AgentSharedManifesRefShipment> agentSharedManifesRefShipmentLists = new List<AgentSharedManifesRefShipment>();

            AgentSharedManifesRefShipment agentSharedManifesRef = new AgentSharedManifesRefShipment();
            agentSharedManifesRef.ShipmentId = shareDocumentTypesArgs.EntityId;
            agentSharedManifesRef.ShipmentLevelCode = shareDocumentTypesArgs.ShipmentLevelCode;
            agentSharedManifesRef.ShipmentNumber = shareDocumentTypesArgs.AgentReference;
            agentSharedManifesRef.AgentSharedManifestRef = shareDocumentTypesArgs.AgentSharedManifestId;
            agentSharedManifesRefShipmentLists.Add(agentSharedManifesRef);

            List<string> shipmentIds = new List<string>();
            if (shareDocumentTypesArgs.ShipmentLevelCode == "C" && shareDocumentTypesArgs.HouseSLLists != null && shareDocumentTypesArgs.HouseSLLists.Count > 0)
            {

                int index = 0;
                AgentSharedManifesRefShipment agentSharedManifesRefShipment = null;
                foreach (HouseSL houseSL in shareDocumentTypesArgs.HouseSLLists)
                {
                    index += 1;

                    agentSharedManifesRefShipment = new AgentSharedManifesRefShipment();
                    agentSharedManifesRefShipment.ShipmentNumber = houseSL.ShipmentNumber;
                    agentSharedManifesRefShipment.ShipmentLevelCode = "H";
                    agentSharedManifesRefShipment.AgentSharedManifestRef = shareDocumentTypesArgs.AgentSharedManifestId + "/" + index;
                    agentSharedManifesRefShipmentLists.Add(agentSharedManifesRefShipment);
                }

                ShipmentQuery shipmentQuery = new ShipmentQuery(shareDocumentTypesArgs.Tenant);
                shipmentLists = shipmentQuery.GetShipmentListsByMasterIdAndTenant(shareDocumentTypesArgs.EntityId, shareDocumentTypesArgs.Tenant);

                foreach (ShipmentList shipmentList in shipmentLists)
                {
                    agentSharedManifesRefShipment = agentSharedManifesRefShipmentLists.Where(d => d.ShipmentNumber == shipmentList.ShipmentNumber).FirstOrDefault();
                    if (agentSharedManifesRefShipment == null)
                    {
                        agentSharedManifesRefShipment = new AgentSharedManifesRefShipment();
                        agentSharedManifesRefShipment.ShipmentNumber = shipmentList.ShipmentNumber;
                        agentSharedManifesRefShipment.ShipmentLevelCode = "H";
                        agentSharedManifesRefShipment.AgentSharedManifestRef = "";
                        agentSharedManifesRefShipmentLists.Add(agentSharedManifesRefShipment);
                    }


                    agentSharedManifesRefShipment.ShipmentId = shipmentList.Id;
                    shipmentIds.Add(shipmentList.Id);
                }

            }
            shipmentIds.Add(shareDocumentTypesArgs.EntityId);

            #endregion

            #region get DocumentsFilingList

            if (shareDocumentTypesArgs.ShipmentLevelCode == "C" && shipmentLists.Count == 0)
            {
                shareDocumentTypesArgs.DocumentTypeLists = shareDocumentTypesArgs.DocumentTypeLists.Where(d => d.IsAgentSharedInMaster);
            }

            documentTypes = shareDocumentTypesArgs.DocumentTypeLists.ToList();


            foreach (DocumentTypeList documentType in documentTypes)
            {
                shareDocumentTypesArgs.DocumentTypeCopyIds.Add(documentType.SharedDocumentTypeCopyId);
                shareDocumentTypesArgs.DocumentTypeIds.Add(documentType.Id);
            }


            DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(shareDocumentTypesArgs.Tenant);
            List<DocumentsFilingList> documentsFilingLists = documentsFilingQuery.GetDocumentsFilingListsByDocumentTypeIdsAndEntityId(shareDocumentTypesArgs.DocumentTypeIds, shipmentIds, shareDocumentTypesArgs.Tenant);
            #endregion

            #region documentsFiling DocOut UpdateDate

            List<string> documentOutIds = new List<string>();
            foreach (DocumentsFilingList documentsFiling in documentsFilingLists.Where(d => d.DirectionCode == "O").ToList())
            {
                documentOutIds.Add(documentsFiling.Id);
            }

            List<DocumentOutPM> documentOutPMLists = new List<EntityPMs.DocumentOutPM>();
            List<DocumentOutCopyList> documentOutCopyLists = new List<DocumentOutCopyList>();
            List<Document> documentLists = new List<Document>();
            List<DocumentTypeCopyList> documentTypeCopyLists = new List<DocumentTypeCopyList>();

            if (documentOutIds.Count > 0)
            {
                DocumentOutQuery documentOutQuery = new DocumentOutQuery(shareDocumentTypesArgs.Tenant);
                DocumentOutCopyQuery dcumentOutCopyQuery = new DocumentOutCopyQuery(shareDocumentTypesArgs.Tenant);
                DocumentRepository documentRepository = new DocumentRepository(shareDocumentTypesArgs.Tenant);
                DocumentTypeCopyQuery documentTypeCopyQuery = new DocumentTypeCopyQuery(shareDocumentTypesArgs.Tenant);


                documentOutPMLists = documentOutQuery.GetDocumentOutPMsByDocOutIds(documentOutIds, shareDocumentTypesArgs.Tenant);
                if (shareDocumentTypesArgs.DocumentTypeCopyIds.Count > 0)
                {

                    documentOutCopyLists = dcumentOutCopyQuery.GeDocumentOutCopiesListsBydocumentTypeCopyIdsAndDocumentOutIds(shareDocumentTypesArgs.DocumentTypeCopyIds, documentOutIds, shareDocumentTypesArgs.Tenant);
                    documentTypeCopyLists = documentTypeCopyQuery.GetDocumentTypeCopyListsBydocumentTypeCopyIds(shareDocumentTypesArgs.DocumentTypeCopyIds, shareDocumentTypesArgs.Tenant);

                }

                foreach (DocumentOutCopyList documentOutCopy in documentOutCopyLists)
                {
                    shareDocumentTypesArgs.DocumentOutCopyIds.Add(documentOutCopy.Id);
                }

                if (shareDocumentTypesArgs.DocumentOutCopyIds.Count > 0) documentLists = documentRepository.GetDocumentsByDocumentIds(shareDocumentTypesArgs.DocumentOutCopyIds, shareDocumentTypesArgs.Tenant);


                if (documentOutPMLists.Count > 0)
                {
                    foreach (DocumentsFilingList documentsFiling in documentsFilingLists.Where(d => d.DirectionCode == "O").ToList())
                    {
                        DocumentOutPM documentOut = documentOutPMLists.Where(d => d.Id == documentsFiling.Id).FirstOrDefault();
                        if (documentOut != null)
                        {
                            DocumentTypeList documentType = documentTypes.Where(d => d.Id == documentsFiling.DocumentTypeId).FirstOrDefault();
                            DocumentOutCopyList documentOutCopyList = documentOutCopyLists.Where(d => d.DocumentOutId == documentOut.Id && d.DocumentTypeCopyId == documentType.SharedDocumentTypeCopyId).FirstOrDefault();
                            Document document = null;

                            if (documentOutCopyList != null && documentType != null) document = documentLists.Where(d => d.Id == documentOutCopyList.Id).FirstOrDefault();
                            if (document != null && !string.IsNullOrEmpty(document.CalculatedFileName)) documentsFiling.FileName = document.CalculatedFileName;
                            else
                            {
                                if (documentType != null)
                                {
                                    documentsFiling.FileName = documentType.Name;

                                    DocumentTypeCopyList documentTypeCopyList = documentTypeCopyLists.Where(d => d.Id == documentType.SharedDocumentTypeCopyId).FirstOrDefault();

                                    if (documentTypeCopyList != null)
                                    {
                                        if (documentType.Name != documentTypeCopyList.Name)
                                        {
                                            documentsFiling.FileName += (" - " + documentTypeCopyList.Name);

                                        }
                                    }
                                }
                            }


                            documentsFiling.UpdateDate = documentOutCopyList == null ? null : documentOut.IssuedDate;
                            documentsFiling.HasFile = document != null ? document.HasFile : false;
                            documentsFiling.FileSize = document != null ? document.FileSize : null;
                            documentsFiling.Extension = document != null ? document.Extension : null;
                            documentsFiling.DocumentId = document != null ? document.Id : null;


                        }
                    }
                }

            }

            #endregion

            #region BiludList
            foreach (AgentSharedManifesRefShipment item in agentSharedManifesRefShipmentLists)
            {
                ShipmentShareDocumentsData shipmentShareDocumentsData = new ShipmentShareDocumentsData();
                shipmentShareDocumentsData.EntityId = item.ShipmentId;
                shipmentShareDocumentsData.AgentSharedManifestRef = item.AgentSharedManifestRef;
                shipmentShareDocumentsData.ShipmentNumber = item.ShipmentNumber;
                shipmentShareDocumentsData.ShipmentLevelCode = item.ShipmentLevelCode;
                shipmentShareDocumentsData.ShipmentLevelName = item.ShipmentLevelCode == "C" ? "Master" : item.ShipmentLevelCode == "H" ? "House" : "Direct";
                shipmentShareDocumentsData.AgentId = shareDocumentTypesArgs.AgentId;
                if (shareDocumentTypesArgs.ShareDocumentsFrom != "Email")
                {
                    shipmentShareDocumentsData.TenantAgent = GetDestinationAgentTenant(shareDocumentTypesArgs.Tenant, shareDocumentTypesArgs.AgentSharedLogisticsKey);
                }

                List<ShareDocument> shareDocuments = new List<ShareDocument>();
                if (item.ShipmentLevelCode == "C")
                {
                    foreach (DocumentTypeList documentType in documentTypes.Where(d => d.IsAgentSharedInMaster).ToList())
                    {
                        FillShareDocumentLists(documentsFilingLists, item, shareDocuments, documentType);
                    }
                }

                else if (item.ShipmentLevelCode == "D")
                {
                    foreach (DocumentTypeList documentType in documentTypes.Where(d => d.IsAgentSharedInDirect).ToList())
                    {
                        FillShareDocumentLists(documentsFilingLists, item, shareDocuments, documentType);
                    }
                }

                else if (item.ShipmentLevelCode == "H")
                {
                    foreach (DocumentTypeList documentType in documentTypes.Where(d => d.IsAgentSharedInHouse).ToList())
                    {
                        FillShareDocumentLists(documentsFilingLists, item, shareDocuments, documentType);
                    }
                }

                shipmentShareDocumentsData.ShareDocuments = shareDocuments.OrderBy(d => d.DocumentTypeName).ToList();



                shareDocumentTypesArgs.ShipmentShareDocumentsDataLists.Add(shipmentShareDocumentsData);

            }
            #endregion
            return shareDocumentTypesArgs.ShipmentShareDocumentsDataLists;
        }

        private static int GetDestinationAgentTenant(int tenant, AgentSharedLogisticsKey agentSharedLogisticsKey)
        {
            return agentSharedLogisticsKey.Agent1Tenant == tenant ? agentSharedLogisticsKey.Agent2Tenant : agentSharedLogisticsKey.Agent1Tenant;
        }

        private ShareDocument GetInstanceFromSharedDocument(DocumentTypeList documentType, string entityId)
        {
            ShareDocument shareDocument = new ShareDocument();
            if (documentType != null)
            {
                shareDocument.DocumentTypeId = documentType.Id;
                shareDocument.DocumentTypeCode = documentType.Code;
                shareDocument.DocumentTypeName = documentType.Name;
                shareDocument.DocumentTypeCopyId = documentType.SharedDocumentTypeCopyId;
                shareDocument.EntityId = entityId;
            }

            return shareDocument;
        }
        private void FillShareDocumentLists(List<DocumentsFilingList> documentsFilingLists, AgentSharedManifesRefShipment item, List<ShareDocument> shareDocuments, DocumentTypeList documentType)
        {
            if (documentsFilingLists.Where(d => d.DocumentTypeId == documentType.Id && d.EntityId == item.ShipmentId).ToList().Count > 0)
            {
                foreach (DocumentsFilingList documentsFilingList in documentsFilingLists.Where(d => d.DocumentTypeId == documentType.Id && d.EntityId == item.ShipmentId).ToList())
                {
                    ShareDocument shareDocument = GetInstanceFromSharedDocument(documentType, item.ShipmentId);
                    shareDocument.SecurityId = documentsFilingList.SecurityId;
                    shareDocument.IsReady = documentsFilingList.HasFile;
                    shareDocument.LastUpdateDate = shareDocument.IsReady ? documentsFilingList.UpdateDate : null;
                    shareDocument.LastShareDate = documentsFilingList.LastShareDate;
                    shareDocument.FileName = documentsFilingList.FileName;
                    shareDocument.DirectionCode = documentsFilingList.DirectionCode;
                    shareDocument.DocumentId = documentsFilingList.DocumentId;
                    shareDocument.DocumentsFilingId = documentsFilingList.Id;
                    shareDocument.FileSize = documentsFilingList.FileSize;
                    shareDocument.Extension = documentsFilingList.Extension;
                    shareDocument.DocumentOutId = documentsFilingList.DirectionCode == "O" ? documentsFilingList.Id : null;
                    shareDocument.ActionButtonLabel = documentsFilingList.DirectionCode == "O" ? documentsFilingList.HasFile ? "Update" : "Build" : "Additional";

                    shareDocuments.Add(shareDocument);
                }

                if (documentType.IsDocIn)
                {
                    if (shareDocuments.Where(d => d.DocumentTypeId == documentType.Id && d.DirectionCode == "I" && d.IsReady).FirstOrDefault() == null)
                    {
                        foreach (ShareDocument shareItem in shareDocuments.Where(d => d.DocumentTypeId == documentType.Id && d.DirectionCode == "I"))
                        {
                            shareItem.ActionButtonLabel = "Upload";
                        }
                    }
                }

            }

            if (documentType.IsDocOut)
            {
                if (shareDocuments.Where(d => d.DocumentTypeId == documentType.Id && d.DirectionCode == "O").FirstOrDefault() == null)
                {
                    ShareDocument shareDocument = GetInstanceFromSharedDocument(documentType, item.ShipmentId);
                    shareDocument.DirectionCode = "O";
                    shareDocument.ActionButtonLabel = "Build";
                    shareDocuments.Add(shareDocument);
                }

            }
            if (documentType.IsDocIn)
            {
                if (shareDocuments.Where(d => d.DocumentTypeId == documentType.Id && d.DirectionCode == "I").FirstOrDefault() == null)
                {
                    ShareDocument shareDocument = GetInstanceFromSharedDocument(documentType, item.ShipmentId);
                    shareDocument.DirectionCode = "I";
                    shareDocument.ActionButtonLabel = "Upload";
                    shareDocuments.Add(shareDocument);
                }
            }


        }



        public List<DocumentTypePM> GetDocumentTypePMsListsByCodes(List<string> codeLists, int tenant)
        {
            List<DocumentTypePM> documentTypeLists = (from a in repository.context.DocumentTypes
                                                      where a.Tenant == tenant && codeLists.Contains(a.Code)
                                                      select new DocumentTypePM()
                                                      {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          Name = a.Name,
                                                          Code = a.Code,
                                                          DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                                                          DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                                                          DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                                                          AddedManually = a.AddedManually,

                                                      }).ToList();
            return documentTypeLists;
        }



        public string GetDocumentTypeIdByCode(string code, int tenant)
        {
            return (from a in repository.context.DocumentTypes where a.Code == code && a.Tenant == tenant select a.Id).FirstOrDefault();
        }

        public List<DocumentTypePM> GetDigitalDocuments(string objectTableName, int tenant)
        {
            List<DocumentTypePM> documentTypeLists = repository.context.DocumentTypes.Include("ObjectTable")
                                                                                      .Where(a => a.Tenant == tenant && a.IsCustomerUploadPermission && a.ObjectTable.Name == objectTableName)
                                                                                      .Select(a => new DocumentTypePM()
                                                                                      {
                                                                                          Id = a.Id,
                                                                                          Tenant = a.Tenant,
                                                                                          Name = a.Name,
                                                                                          Code = a.Code,
                                                                                      }).ToList();

            return documentTypeLists;
        }

        public IQueryable<DocumentTypePM> GetDocumentTypesByTenant(int tenant)
        {
            IQueryable<DocumentTypePM> d = null;
            DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
            DocumentTypeCopyQuery documentCopiesQuery = new DocumentTypeCopyQuery(tenant);

            d = from a in repository.context.DocumentTypes.Include("DocumentTypeCategory")
                where a.Tenant == tenant
                select new DocumentTypePM()
                {
                    InActive = a.InActive,
                    Id = a.Id,
                    IsAir = a.IsAir,
                    IsDocIn = a.IsDocIn,
                    IsDocOut = a.IsDocOut,
                    IsInland = a.IsInland,
                    IsOcean = a.IsOcean,
                    Name = a.Name,
                    Notes = a.Notes,
                    Tenant = a.Tenant,
                    SearchFields = a.SearchFields,
                    Code = a.Code,
                    DocumentTypeDefaultEditorTool = a.DocumentTypeDefaultEditorTool,
                    ObjectTableId = a.ObjectTableId,
                    Subject = a.Subject,
                    DocumentTypeDefaultHTMLTemplateId = a.DocumentTypeDefaultHTMLTemplateId,
                    DocumentTypeDefaultReportTemplateId = a.DocumentTypeDefaultReportTemplateId,
                    TemplateFormatCode = a.TemplateFormatCode,
                    IsMaster = a.IsMaster,
                    IsDirect = a.IsDirect,
                    IsHouse = a.IsHouse,
                    CustomControl = a.CustomControl,
                    AgentRoleId = a.AgentRoleId,
                    CustomerRoleId = a.CustomerRoleId,
                    IsAgentView = a.IsAgentView,
                    IsCustomerView = a.IsCustomerView,
                    IsCustomerUploadPermission = a.IsCustomerUploadPermission,
                    IsReadOnly = a.IsReadOnly,
                    LimitedPrintCopyId = a.LimitedPrintCopyId,
                    IsDocumentOneTimePrintLimited = a.IsDocumentOneTimePrintLimited,
                    IsCopiedAtSignup = a.IsCopiedAtSignup,
                    IsEnabledForCustomers = a.IsEnabledForCustomers,
                    CountryCode = a.CountryCode,
                    DocumentTypeCategoryCode = a.DocumentTypeCategoryCode,
                    DocumentTypeCategoryName = a.DocumentTypeCategory != null ? a.DocumentTypeCategory.Name : null,
                    OrderBy = a.OrderBy,
                    FileName = a.FileName,
                    IsAgentSharedInDirect = a.IsAgentSharedInDirect,
                    IsAgentSharedInHouse = a.IsAgentSharedInHouse,
                    IsAgentSharedInMaster = a.IsAgentSharedInMaster,
                    SharedDocumentTypeCopyId = a.SharedDocumentTypeCopyId,
                    IsAirDigitalSignRequired = a.IsAirDigitalSignRequired,
                    IsOceanDigitalSignRequired = a.IsOceanDigitalSignRequired,
                    IsInlandDigitalSignRequired = a.IsInlandDigitalSignRequired,
                    IsSystemAdditionalPrintingFields = a.IsSystemAdditionalPrintingFields,
                    PrintingFieldsScreenCode = a.PrintingFieldsScreenCode,
                    AddedManually = a.AddedManually,
                    OnPrintPopulateDateFieldName = a.OnPrintPopulateDateFieldName,
                    OnSendPopulateDateFieldName = a.OnSendPopulateDateFieldName,
                    OnUploadPopulateDateFieldName = a.OnUploadPopulateDateFieldName,


                };

            foreach (DocumentTypePM doc in d)
            {
                doc.DocumentTypeCustomFields = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(doc.Id, doc.Tenant).ToList();
                doc.DocumentTypeCopies = documentCopiesQuery.GetDocumentTypeCopiesByDocumentType(doc.Id, null, doc.Tenant);
            }

            return d;
        }

    }

    public class ShareDocumentTypesArgs
    {
        public string EntityId { get; set; }
        public string AgentId { get; set; }
        public string AgentReference { get; set; }
        public string ShipmentLevelCode { get; set; }
        public int Tenant { get; set; }
        public string ShareDocumentsFrom { get; set; }
        public List<ShipmentShareDocumentsData> ShipmentShareDocumentsDataLists { get; set; }
        public IQueryable<DocumentTypeList> DocumentTypeLists { get; set; }
        public List<string> DocumentTypeIds { get; set; }
        public List<string> DocumentTypeCopyIds { get; set; }
        public List<string> DocumentOutCopyIds { get; set; }
        public AgentSharedLogisticsKey AgentSharedLogisticsKey { get; set; }
        public string AgentSharedManifestId { get; set; }
        public List<HouseSL> HouseSLLists { get; set; }
    }
}