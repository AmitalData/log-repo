	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.Customs.Data.CustomFilters;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class DeclarationListQueryService
    {
#if false
        private IQueryable<DeclarationList> GetIqueryableList_old(IQueryable<Declaration> iQueryable)
        {
            IQueryable<DeclarationList> query = (from a in iQueryable.Include("DeclarationOffice").Include("AutonomyRegionType").Include("CustomerCard").Include("EntitleImporterCountry").Include("ImporterEntitlementType").Include("ImporterPassCountry").Include("ProcedureCurrent").Include("TransferImporterCountry")
                                                 select new DeclarationList()
                                                 {
                                                     Id = a.Id,
                                                     AgentId = a.AgentId,
                                                     AutonomyRegionTypeName = a.AutonomyRegionType.EnglishName,
                                                     CIFValue = a.CIFValue,
                                                     ConstraintCode = a.ConstraintCode,
                                                     CreatedByUserId = a.CreatedByUserId,
                                                     CustomerName = a.CustomerCard.EnglishName,
                                                     CustomFileNo = a.CustomFileNo,
                                                     DealValue = a.DealValue,
                                                     DeclarationDocumentId = a.DeclarationDocumentId,
                                                     DeclarationNumber = a.DeclarationNumber,
                                                     DeclarationOfficeCode = a.DeclarationOfficeCode,
                                                     DeclarationStatusTypeCode = a.DeclarationStatusTypeCode,
                                                     EntitleImporterCountryName = a.EntitleImporterCountry.EnglishName,
                                                     EntitleImporterId = a.EntitleImporterId,
                                                     ExternalDeclarationNumber = a.ExternalDeclarationNumber,
                                                     HatraDate = a.HatraDate,
                                                     ImporterEntitlementTypeName = a.ImporterEntitlementType.EnglishName,

                                                     ImporterPassCountryName = a.ImporterPassCountry.EnglishName,
                                                     IsChanged = a.IsChanged,
                                                     FileState = a.FileState,
                                                     LoadingFactor = a.LoadingFactor,
                                                     PaymentDate = a.PaymentDate,
                                                     ProcedureCurrentName = a.ItemGovernmentProcedureCurrent.EnglishName,
                                                     TaxationDateTime = a.TaxationDateTime,
                                                     Tenant = a.Tenant,
                                                     TotalTax = a.TotalTax,
                                                     TransferImporterCountryName = a.TransferImporterCountry.EnglishName,

                                                     VersionId = a.VersionId,

                                                     EntitleImporterCountryCode = a.EntitleImporterCountryCode,
                                                     ImporterPassCountryCode = a.ImporterPassCountryCode,
                                                     ProcedureCurrentCode = a.ProcedureCurrentCode,

                                                     CustomerId = a.CustomerId,
                                                     ImporterEntitlementTypeCode = a.ImporterEntitlementTypeCode,
                                                     TransferImporterCountryCode = a.TransferImporterCountryCode,
                                                     SearchFields = a.SearchFields,
                                                     DeclarationOfficeName = a.DeclarationOffice == null ? null : a.DeclarationOffice.EnglishName,
                                                     DeclarationNumberandVersionId = a.DeclarationNumber + (string.IsNullOrEmpty(a.VersionId) ? "" : " " + a.VersionId),

                                                 });



            return query;
        }
        private IQueryable<DeclarationList> GetIqueryableList(IQueryable<Declaration> iQueryable)
        {
            IQueryable<DeclarationList> query = (from a in iQueryable
                                                 //.Include("DeclarationOffice").Include("AutonomyRegionType").Include("CustomerCard").Include("EntitleImporterCountry").Include("ImporterEntitlementType").Include("ImporterPassCountry").Include("ProcedureCurrent").Include("TransferImporterCountry")
                                                 select new DeclarationList()
                                                 {
                                                     Id = a.Id,

                                                     //  AgentId = a.AgentId,
                                                     //  AutonomyRegionTypeName = a.AutonomyRegionType.EnglishName,
                                                     CIFValue = a.CIFValue,
                                                     ConstraintCode = a.ConstraintCode,
                                                     CreatedByUserId = a.CreatedByUserId,
                                                     CustomerName = a.CustomerCard.EnglishName,
                                                     CustomFileNo = a.CustomFileNo,
                                                     DealValue = a.DealValue,
                                                     DeclarationDocumentId = a.DeclarationDocumentId,
                                                     DeclarationNumber = a.DeclarationNumber,
                                                     DeclarationOfficeCode = a.DeclarationOfficeCode,
                                                     DeclarationStatusTypeCode = a.DeclarationStatusTypeCode,
                                                     //  EntitleImporterCountryName = a.EntitleImporterCountry.EnglishName,
                                                     EntitleImporterId = a.EntitleImporterId,
                                                     ExternalDeclarationNumber = a.ExternalDeclarationNumber,
                                                     HatraDate = a.HatraDate,
                                                     //ImporterEntitlementTypeName = a.ImporterEntitlementType.EnglishName,
                                                     ImporterId = a.ImporterId,
                                                     //ImporterPassCountryName = a.ImporterPassCountry.EnglishName,
                                                     IsChanged = a.IsChanged,
                                                     FileState = a.FileState,
                                                     LoadingFactor = a.LoadingFactor,
                                                     PaymentDate = a.PaymentDate,
                                                     //ProcedureCurrentName = a.ItemGovernmentProcedureCurrent.EnglishName,
                                                     TaxationDateTime = a.TaxationDateTime,
                                                     Tenant = a.Tenant,
                                                     TotalTax = a.TotalTax,
                                                     ///TransferImporterCountryName = a.TransferImporterCountry.EnglishName,
                                                     TransferImporterId = a.TransferImporterId,
                                                     VersionId = a.VersionId,
                                                     AutonomyRegionTypeCode = a.AutonomyRegionTypeCode,
                                                     EntitleImporterCountryCode = a.EntitleImporterCountryCode,
                                                     ImporterPassCountryCode = a.ImporterPassCountryCode,
                                                     ProcedureCurrentCode = a.ProcedureCurrentCode,
                                                     DeclarationDocumentTypeCode = a.DeclarationDocumentTypeCode,
                                                     CustomerId = a.CustomerId,
                                                     ImporterEntitlementTypeCode = a.ImporterEntitlementTypeCode,
                                                     TransferImporterCountryCode = a.TransferImporterCountryCode,
                                                     SearchFields = a.SearchFields,
                                                     //DeclarationOfficeName = a.DeclarationOffice == null ? null : a.DeclarationOffice.EnglishName,
                                                     //DeclarationNumberandVersionId = a.DeclarationNumber + (string.IsNullOrEmpty(a.VersionId) ? "" : " " + a.VersionId),

                                                 });



            return query;
        }
        
#endif


        private IQueryable<DeclarationList> GetIqueryableList(IQueryable<Declaration> iQueryable)
        {




            IQueryable<DeclarationList> query = (from a in iQueryable.Include("DeclarationOffice").Include("AutonomyRegionType").Include("CustomerCard").Include("EntitleImporterCountry").Include("ImporterEntitlementType").Include("ImporterPassCountry").Include("ProcedureCurrent").Include("TransferImporterCountry").Include("Department").Include("DeclarationStatusType")
                                                 //.Include("CreatedByUser.Contact")
                                                 .Include("Importer").Include("EntitleImporter").Include("TransferImporter").Include("ImporterType").Include("TransferImporterType").Include("EntitleImporterType").Include("StorageStatus").Include("FreightPaymentMethod")
                                                 select new DeclarationList()
                                                 {
                                                     Id = a.Id,
                                                     // AgentId = a.AgentId,
                                                     AutonomyRegionTypeName = a.AutonomyRegionType.LocalName,
                                                     CIFValue = a.CIFValue,
                                                     //   ConstraintCode = a.ConstraintCode,
                                                     // CreatedByUserId = a.CreatedByUserId,
                                                     CustomerName = a.IsCourierDeclaration ? a.ImporterName : (a.CustomerCard.LocalName != null ? a.CustomerCard.LocalName : a.CustomerCard.EnglishName),
                                                     CustomFileNo = a.CustomFileNo,
                                                     DealValue = a.DealValue,
                                                     //  DeclarationDocumentId = a.DeclarationDocumentId,
                                                     DeclarationNumber = a.DeclarationNumber,

                                                     EntitleImporterCountryName = a.EntitleImporterCountry.LocalName,
                                                     //  EntitleImporterId = a.EntitleImporterId,
                                                     ExternalDeclarationNumber = a.ExternalDeclarationNumber,
                                                     HatraDate = a.HatraDate,
                                                     ImporterEntitlementTypeName = a.ImporterEntitlementType.LocalName,

                                                     ImporterPassCountryName = a.ImporterPassCountry.LocalName,
                                                     IsChanged = a.IsChanged,
                                                     FileState = a.FileState,
                                                     LoadingFactor = a.LoadingFactor,
                                                     PaymentDate = a.PaymentDate,
                                                     ProcedureCurrentName = a.GovernmentProcedureCurrent.LocalName,
                                                     TaxationDateTime = a.TaxationDateTime,
                                                     Tenant = a.Tenant,
                                                     TotalTax = a.TotalTax,
                                                     TransferImporterCountryName = a.TransferImporterCountry.LocalName,
                                                     //   VersionId = a.VersionId,
                                                     DeclarationVersionId = a.VersionId,
                                                     CreateDateTime = a.CreateDateTime,
                                                     UpdateDateTime = a.UpdateDateTime,
                                                     EntitleImporterCountryCode = a.EntitleImporterCountryCode,
                                                     ImporterPassCountryCode = a.ImporterPassCountryCode,
                                                     ProcedureCurrentCode = a.ProcedureCurrentCode,
                                                     CustomerCode = a.CustomerCard == null ? null : a.CustomerCard.Code,
                                                     CustomerId = a.CustomerId,
                                                     ImporterEntitlementTypeCode = a.ImporterEntitlementTypeCode,
                                                     TransferImporterCountryCode = a.TransferImporterCountryCode,
                                                     SearchFields = a.SearchFields,
                                                     DeclarationOfficeName = a.DeclarationOffice == null ? null : a.DeclarationOffice.LocalName,
                                                     DeclarationNumberandVersionId = a.DeclarationNumber + (string.IsNullOrEmpty(a.VersionId) ? "" : " " + a.VersionId),
                                                     //ErrosXml = a.ErrosXml,
                                                     //   TransportModeId = a.TransportModeId,
                                                     AutonomyRegionTypeCode = a.AutonomyRegionTypeCode,
                                                     //  ReferentUserId = a.ReferentUserId,
                                                     DeclarationStatusTypeName = a.DeclarationStatusType == null ? null : a.DeclarationStatusType.LocalName,
                                                     IsCancelled = a.IsCancelled,
                                                     PlatformFee = a.PlatformFee,
                                                     StorageSiteCode = a.StorageSiteCode,
                                                     DeclarationStatusTypeCode = a.DeclarationStatusTypeCode,
                                                     DeclarationOfficeCode = a.DeclarationOfficeCode,
                                                     DepartmentId = a.DepartmentId,
                                                     DepartmentName = a.Department.LocalName,
                                                     //   EntitleImporterName = a.en
                                                     //  ImporterName = a.Client == null ? null : a.Client.FullName,
                                                     TransportModeName = a.CustomsTransportMode == null ? null : a.CustomsTransportMode.LocalName,
                                                     EntitleImporterCode = a.EntitleImporterCode,
                                                     CreatedByUserName =
                                                     //a.CreatedByUser != null ? (a.CreatedByUser.Contact.LocalName != null ? a.CreatedByUser.Contact.LocalName : a.CreatedByUser.Contact.EnglishName) : null,

                                                     a.CreatedByUser.Code,
                                                     ImporterAddress = a.ImporterAddress,
                                                     ImporterName = a.Importer != null ? a.Importer.FullName : a.ImporterName,
                                                     EntitleImporterName = a.EntitleImporter.FullName,
                                                     UserNotes = a.UserNotes,
                                                     ImporterTypeCode = a.ImporterTypeCode,
                                                     ImporterTypeName = a.ImporterType != null ? a.ImporterType.LocalName : null,
                                                     TransferImporterTypeCode = a.TransferImporterTypeCode,
                                                     TransferImporterTypeName = a.TransferImporterType != null ? a.TransferImporterType.LocalName : null,
                                                     EntitleImporterTypeCode = a.EntitleImporterTypeCode,
                                                     EntitleImporterTypeName = a.EntitleImporterType != null ? a.EntitleImporterType.LocalName : null,
                                                     HasConstraint = a.HasConstraint,
                                                     StorageSiteName = a.StorageSiteName,
                                                     SignerPersonalId = a.SignerPersonalId,
                                                     IsConvertedDeclaration = a.IsConvertedDeclaration,
                                                     CorrectionsXml = a.CorrectionsXml,
                                                     SignedByUserId = a.SignedByUserId,
                                                     ImporterCode = a.ImporterCode,
                                                     StorageStatusCode = a.StorageStatusCode,
                                                     StorageStatusName = a.StorageStatus == null ? null : a.StorageStatus.LocalName,
                                                     WeightValue = a.WeightValue,
                                                     WeightValueName = a.FreightPaymentMethod != null ? a.FreightPaymentMethod.LocalName : null,
                                                     IsCourierDeclaration = a.IsCourierDeclaration,
                                                     CourierSearchFields = a.CourierSearchFields,
                                                     CourierHAWB = a.CourierHAWB,
                                                     CourierCustomStatusCode = a.CourierCustomStatusCode,
                                                     CourierCustomStatusName = a.CourierCustomStatus != null ? a.CourierCustomStatus.LocalName : null,
                                                     ManifestCargoStatusCode = a.ManifestCargoStatusCode,
                                                     ManifestCargoStatusName = a.ManifestCargoStatus != null ? a.ManifestCargoStatus.LocalName : null,
                                                     CourierSuspentionReasonCode = a.CourierSuspentionReasonCode,
                                                     CourierSuspentionReasonName = a.AgentTalkBackType != null ? a.AgentTalkBackType.LocalName : null,
                                                     AcceptanceStatusCode = a.AcceptanceStatusCode,
                                                     AcceptanceStatusName = a.AcceptanceStatus != null ? a.AcceptanceStatus.LocalName : null,
                                                     IsClose = a.IsClose,
                                                     CasualImporterAddress1 = a.CasualImporterAddress1,
                                                     CasualImporterAddress2 = a.CasualImporterAddress2,
                                                     CasualImporterCity = a.CasualImporterCity,
                                                     CasualImporterZipCode = a.CasualImporterZipCode,
                                                     CasualImporterFax = a.CasualImporterFax,
                                                     CasualImporterEmail = a.CasualImporterEmail,
                                                     CasualImporterTel = a.CasualImporterTel,
                                                     CasualImporterContact = a.CasualImporterContact,
                                                     CasualSupplierAddress = a.CasualSupplierAddress,
                                                     CasualSupplierName = a.CasualSupplierName,
                                                     //MamanErrorXml = a.MamanErrorXml,
                                                     ItemsProcessTypesList = a.ItemsProcessTypesList,
                                                     CourierSuspentionCode = a.CourierSuspentionCode,
                                                     CourierSuspentionName = a.CourierSuspention != null ? a.CourierSuspention.LocalName : null,
                                                     DepositionStatusCode = a.DepositionStatusCode,
                                                 });



            return query;
        }

        private IQueryable<Declaration> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Declaration> iQueryable, int tenant)
        {
            DeclarationCustomFilters filters = new DeclarationCustomFilters();

            iQueryable = filters.GetFilteredQuery(queryOperations, iQueryable);

            iQueryable = filters.GetFreelancerDeclarations(queryOperations, iQueryable, tenant);

            return iQueryable;
        }
	}


}
	