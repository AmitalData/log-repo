using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Customs.Data.CustomFilters;
using System.Web;
using System.Data.Entity.SqlServer;

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


        private IQueryable<DeclarationList> GetIqueryableList(IQueryable<Declaration> iQueryable, int? tenant = null)
        {
            var arrAmentmentStatus = new string[] { "6", "7", "8", "10" };

            if (tenant == null)
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                tenant = tenant = authToken.Tenant;
            }

            bool isCourierEnv = context.CustomsSettings.FirstOrDefault(r => r.Tenant == tenant).CompanyType == "B";

            if (!isCourierEnv)
            {
                IQueryable<DeclarationList> query = (from a in iQueryable.Include("DeclarationOffice").Include("AutonomyRegionType").Include("CustomerCard").Include("EntitleImporterCountry").Include("ImporterEntitlementType").Include("ImporterPassCountry").Include("ProcedureCurrent").Include("TransferImporterCountry").Include("Department").Include("DeclarationStatusType")
                                                     //.Include("CreatedByUser.Contact")
                                                     .Include("Importer").Include("EntitleImporter").Include("TransferImporter").Include("ImporterType").Include("TransferImporterType").Include("EntitleImporterType").Include("StorageStatus").Include("FreightPaymentMethod")
                                                     .Include("CustomsCountry").Include("CustomsShip").Include("TransportMode")
                                                     //join recConsignment in context.Consignments.Include("CargoType")
                                                     //.Select(x => new { x.DeclarationId, x.ConsignmentNumber, x.CargoDescription, x.CargoType.LocalName, x.SecondCargoID, x.ThirdCargoID, x.ManifestNumber })
                                                     //on a.Id equals recConsignment.DeclarationId into qjoinConsignments
                                                     //from myJoinConsignment in qjoinConsignments.DefaultIfEmpty()

                                                     //join c in qConsignmentNumber
                                                     //.Select(x => new { x.DeclarationId, x.ConsignmentNumber })
                                                     //on new { myJoinConsignment.DeclarationId, myJoinConsignment.ConsignmentNumber } equals new { c.DeclarationId, c.ConsignmentNumber }


                                                     join recOriginalDeclarations in context.Declarations.Where(x => x.IsAmendment != true)
                                                     .Select(x => new { x.CustomFileNo, x.DeclarationNumber, x.Id })
                                                     on a.AmendmentOriginalDeclartation equals recOriginalDeclarations.Id
                                                     into originalDeclarations
                                                     from myJoinOriginalDeclaration in originalDeclarations.DefaultIfEmpty()

                                                    // join recDisplayDeclarations in context.Declarations.Where(x => x.AmendmentDontDisplayInList != true && !string.IsNullOrEmpty(x.AmendmentOriginalDeclartation))
                                                    //.Select(x => new { x.DeclarationNumber, x.AmendmentOriginalDeclartation })
                                                    //on a.AmendmentOriginalDeclartation equals recDisplayDeclarations.AmendmentOriginalDeclartation
                                                    //into displayDeclarations
                                                    // from myJoinDisplayDeclarations in displayDeclarations.DefaultIfEmpty()

                                                     join AmendmentRequestStatus in context.AmendmentRequestStatuses
                                                     .Select(x => new { x.Code, x.LocalName })
                                                                on a.AmendmentStatus equals AmendmentRequestStatus.Code
                                                                into qStatusAmendJoin
                                                     from myJoinAmendmentRequest in qStatusAmendJoin.DefaultIfEmpty()

                                                     join cooStatusViews in context.CooStatusViews
                                                     on (a.IsAmendment == true ? a.AmendmentOriginalDeclartation : a.Id) equals cooStatusViews.DeclarationId into cooStatusViewsJoin
                                                     from MyDeclarationCooStatusViews in cooStatusViewsJoin.DefaultIfEmpty()

                                                     select new DeclarationList()
                                                     {
                                                         Id = a.Id,
                                                         AutonomyRegionTypeName = a.AutonomyRegionType.LocalName,
                                                         CIFValue = a.CIFValue,
                                                         CustomerName = a.CustomerCard.LocalName != null ? a.CustomerCard.LocalName : a.CustomerCard.EnglishName,
                                                         CustomFileNo = a.CustomFileNo,
                                                         DealValue = a.DealValue,
                                                        DeclarationNumber = !string.IsNullOrEmpty(a.DeclarationNumber) ? a.DeclarationNumber : myJoinOriginalDeclaration.DeclarationNumber,//(!string.IsNullOrEmpty(myJoinOriginalDeclaration.DeclarationNumber) ? myJoinOriginalDeclaration.DeclarationNumber : myJoinDisplayDeclarations.DeclarationNumber),
                                                         ReferentUserName = a.ReferentUser == null ? null : a.ReferentUser.Code,
                                                         ReferentUserId = a.ReferentUserId,
                                                         EntitleImporterCountryName = a.EntitleImporterCountry.LocalName,
                                                         ExternalDeclarationNumber = a.ExternalDeclarationNumber,
                                                         HatraDate = a.HatraDate,
                                                         ImporterEntitlementTypeName = a.ImporterEntitlementType.LocalName,

                                                         ImporterPassCountryName = a.ImporterPassCountry.LocalName,
                                                         IsChanged = a.IsChanged,
                                                         FileState = a.FileState,
                                                         LoadingFactor = a.LoadingFactor,
                                                         PaymentDate = a.PaymentDate,
                                                         IsSubmitDeclaration = a.IsSubmitDeclaration,
                                                         ProcedureCurrentName = a.GovernmentProcedureCurrent.LocalName,
                                                         TaxationDateTime = a.TaxationDateTime,
                                                         ExportTaxationDateTime = a.TaxationDateTime,
                                                         Tenant = a.Tenant,
                                                         TotalTax = a.TotalTax,
                                                         ExportFlightDate = a.ExportFlightDate,
                                                         TransferImporterCountryName = a.TransferImporterCountry.LocalName,
                                                         DeclarationVersionId = a.VersionId,
                                                         CreateDateTime = a.CreateDateTime,
                                                         UpdateDateTime = a.UpdateDateTime,
                                                         EntitleImporterCountryCode = a.EntitleImporterCountryCode,
                                                         ImporterPassCountryCode = a.ImporterPassCountryCode,
                                                         ImporterPassCountryForExport = a.ImporterPassCountryCode,
                                                         ProcedureCurrentCode = a.ProcedureCurrentCode,
                                                         CustomerCode = a.CustomerCard == null ? null : a.CustomerCard.Code,
                                                         CustomerId = a.CustomerId,
                                                         ImporterEntitlementTypeCode = a.ImporterEntitlementTypeCode,
                                                         TransferImporterCountryCode = a.TransferImporterCountryCode,
                                                         SearchFields = a.SearchFields,
                                                         DeclarationOfficeName = a.DeclarationOffice == null ? null : a.DeclarationOffice.LocalName,
                                                         DeclarationNumberandVersionId = a.DeclarationNumber + (string.IsNullOrEmpty(a.VersionId) ? "" : " " + a.VersionId),
                                                         AutonomyRegionTypeCode = a.AutonomyRegionTypeCode,
                                                         DeclarationStatusTypeName = a.DeclarationStatusType == null ? null : a.DeclarationStatusType.LocalName,
                                                         IsCancelled = a.IsCancelled,
                                                         PlatformFee = a.PlatformFee,
                                                         StorageSiteCode = a.StorageSiteCode,
                                                         DeclarationStatusTypeCode = a.DeclarationStatusTypeCode,
                                                         DeclarationOfficeCode = a.DeclarationOfficeCode,
                                                         DeclarationOfficeNameForExport = a.DeclarationOffice == null ? null : a.DeclarationOffice.LocalName,
                                                         ExportDeclarationOfficeCode = a.ExportDeclarationOfficeCode,
                                                         ExportAutonomyRegionTypeCode = a.ExportAutonomyRegionTypeCode,
                                                         DepartmentId = a.DepartmentId,
                                                         DepartmentName = a.Department == null ? null : a.Department.LocalName,
                                                         TransportModeName = a.TransportMode != null ? a.TransportMode.LocalName : null,
                                                         EntitleImporterCode = a.EntitleImporterCode,
                                                         CreatedByUserName =

                                                         a.CreatedByUser.Code,
                                                         IsHatraDateNull = a.HatraDate == null,
                                                         ImporterAddress = a.ImporterAddress,
                                                         ImporterAddressForExport = a.ImporterAddress,
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
                                                         ExporterImporterCode = a.ImporterCode,
                                                         ImporterNameForExport = a.Importer != null ? a.Importer.FullName : a.ImporterName,
                                                         StorageStatusCode = a.StorageStatusCode,
                                                         StorageStatusName = a.StorageStatus == null ? null : a.StorageStatus.LocalName,
                                                         WeightValue = a.WeightValue,
                                                         WeightValueName = a.FreightPaymentMethod != null ? a.FreightPaymentMethod.LocalName : null,
                                                         IsCourierDeclaration = a.IsCourierDeclaration,
                                                         //CourierSearchFields = a.CourierSearchFields,
                                                         //CourierHAWB = a.CourierHAWB,
                                                        // CourierCustomStatusCode = a.CourierCustomStatusCode,
                                                         //CourierCustomStatusName = a.CourierCustomStatus != null ? a.CourierCustomStatus.LocalName : null,
                                                         ManifestCargoStatusCode = a.ManifestCargoStatusCode,
                                                         ManifestCargoStatusName = a.ManifestCargoStatus != null ? a.ManifestCargoStatus.LocalName : null,
                                                        // CourierSuspentionReasonCode = a.CourierSuspentionReasonCode,
                                                        // CourierSuspentionReasonName = a.AgentTalkBackType != null ? a.AgentTalkBackType.LocalName : null,
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
                                                         ItemsProcessTypesList = a.ItemsProcessTypesList,
                                                         //CourierSuspentionCode = a.CourierSuspentionCode,
                                                         //CourierSuspentionName = a.CourierSuspention != null ? a.CourierSuspention.LocalName : null,
                                                         DepositionStatusCode = a.DepositionStatusCode,
                                                         AmendmentDontDisplayInList = a.AmendmentDontDisplayInList,
                                                         //CargoDescription = myJoinConsignment != null ? myJoinConsignment.CargoDescription : null,
                                                         IsPaymentProtested = a.IsPaymentProtested,
                                                         DeclarationNoAmendment = myJoinOriginalDeclaration.DeclarationNumber,
                                                         CustomFileAmendment = myJoinOriginalDeclaration.CustomFileNo,
                                                         AmendmentStatus = a.AmendmentStatus,
                                                         AmendmentRequestNumber = a.AmendmentRequestNumber,
                                                         AmendmentCorrectedByUserName = a.AmendmentCorrectedByUser != null ? a.AmendmentCorrectedByUser.Code : null,
                                                         AmendmentissueDate = a.AmendmentissueDate
                                                         ,
                                                         Direction = a.Direction,
                                                         ExportFile = a.ExportFile,
                                                         DeclarationDocumentTypeCode = a.DeclarationDocumentTypeCode,
                                                         AgentRoleCode = a.AgentRoleCode,
                                                         DestinationCountryCode = a.DestinationCountryCode,
                                                         DestinationCountryName = a.CustomsCountry != null ? a.CustomsCountry.EnglishName : "",
                                                         LoadingDateTime = a.LoadingDateTime,
                                                         ShipCodeName = a.CustomsShip != null ? a.CustomsShip.EnglishName : "",
                                                         IsExporterConfirmation = a.IsExporterConfirmation,
                                                         CreateDateForExport = a.CreateDateTime,
                                                         TransportModeForExport = a.TransportModeId,
                                                         TransportModeId = a.TransportModeId,
                                                         CustomFileForExport = a.CustomFileNo,
                                                         //CargoTypeName = myJoinConsignment != null ? myJoinConsignment.LocalName : null,
                                                         //SecondCargoID = myJoinConsignment != null ? myJoinConsignment.SecondCargoID : null,
                                                         //ThirdCargoID = myJoinConsignment != null ? myJoinConsignment.ThirdCargoID : null,
                                                         //ManifestNumber = myJoinConsignment != null ? myJoinConsignment.ManifestNumber : null,
                                                         PhysicalCheck = a.PhysicalCheck,
                                                         PhysicalCheckName = a.PhysicalCheck == null ? "ללם בדיקה" : a.PhysicalCheckCode.Name,
                                                         DeclarationTypeCode = a.DeclarationTypeCode,
                                                         DeclarationTypeName = a.DeclarationType.LocalName,
                                                         IsExportDeclarationAmendments = arrAmentmentStatus.Contains(a.AmendmentStatus),
                                                         FOBValueNIS = a.FOBValueNIS,
                                                         FOBValueDollar = a.FOBValueDollar,
                                                         AmendmentStatusName = myJoinAmendmentRequest != null ? myJoinAmendmentRequest.LocalName : null,
                                                         ExportLoadingPortCode = a.ExportLoadingPortCode,
                                                         LoadingPortName = a.ExportLoadingPort.LocalName,
                                                         ExcludeManifest = a.ExcludeManifest,
                                                         AmendmentRejectionReason = a.AmendmentRejectionReason,
                                                         ShipmentId = a.ShipmentId,
                                                         CooStatusName = MyDeclarationCooStatusViews.Status,                                                         
                                                         CooStatusCode = MyDeclarationCooStatusViews.StatusCode,
													 });

                return query;

            }


            else
            {

                var qConsignmentNumber = (from a in context.Consignments
                                          group a by a.DeclarationId into gConsignments
                                          select
                                          new
                                          {
                                              DeclarationId = gConsignments.Key,
                                              ConsignmentNumber = gConsignments.Min(r => r.ConsignmentNumber),

                                          });

                IQueryable<DeclarationList> query2 = (from a in iQueryable.Include("DeclarationOffice").Include("AutonomyRegionType").Include("CustomerCard").Include("EntitleImporterCountry").Include("ImporterEntitlementType").Include("ImporterPassCountry").Include("ProcedureCurrent").Include("TransferImporterCountry").Include("Department").Include("DeclarationStatusType")
                                     .Include("Importer").Include("ImporterType").Include("FreightPaymentMethod")
                                     .Include("CustomsCountry")


                                                      join cdJoin in context.CourierDeclarations.Include("CourierMaster").Include("Card")
                                                      .Select(x => new
                                                      {
                                                          x.DeclarationId,
                                                          x.CourierMaster.IntegratorCode,
                                                          x.CourierMaster.Card.LocalName,
                                                          x.CourierMaster.MAWB
                                                      })
                                                                    on a.Id equals cdJoin.DeclarationId
                                                                   into cdJoin_
                                                      from cd in cdJoin_.DefaultIfEmpty()

                                                      join dcs in context.DeclarationCourierStatuses
                                                      .Select(x => new
                                                      {
                                                          x.DeclarationId,
                                                          x.IsClosedForFollowUp,
                                                          x.FastIndividualProcessCode,
                                                          x.TotalInvoiceAmountInUSD,
                                                          x.CourierPendingReasonList,
                                                          x.IsCourierMissingClassification,
                                                          x.TerminalReleaseDate
                                                      })
                                                      on a.Id equals dcs.DeclarationId


                                                      join c in qConsignmentNumber
                                                      .Select(x => new { x.DeclarationId, x.ConsignmentNumber })
                                                      on a.Id equals c.DeclarationId into leftJoin
                                                      from leftJoinResult in leftJoin.DefaultIfEmpty()

                                                      join recConsignment in context.Consignments.Include("CargoType")
                                                      .Select(x => new { x.DeclarationId, x.ConsignmentNumber, x.CargoDescription, x.CargoType.LocalName, x.SecondCargoID, x.ThirdCargoID, x.ManifestNumber })
                                                      on new { leftJoinResult.DeclarationId, leftJoinResult.ConsignmentNumber } equals new { recConsignment.DeclarationId, recConsignment.ConsignmentNumber }
                                                      into qjoinConsignments
                                                      from myJoinConsignment in qjoinConsignments.DefaultIfEmpty()


                                                      
                                                      join recOriginalDeclarations in context.Declarations.Where(x => x.IsAmendment != true)
                                                      .Select(x => new { x.CustomFileNo, x.DeclarationNumber, x.Id })
                                                      on a.AmendmentOriginalDeclartation equals recOriginalDeclarations.Id
                                                      into originalDeclarations
                                                      from myJoinOriginalDeclaration in originalDeclarations.DefaultIfEmpty()

                                                      join recDisplayDeclarations in context.Declarations.Where(x => x.AmendmentDontDisplayInList != true && x.AmendmentOriginalDeclartation != null)
                                                     .Select(x => new { x.DeclarationNumber, x.AmendmentOriginalDeclartation })
                                                     on a.AmendmentOriginalDeclartation equals recDisplayDeclarations.AmendmentOriginalDeclartation
                                                     into displayDeclarations
                                                      from myJoinDisplayDeclarations in displayDeclarations.DefaultIfEmpty()

                                                      join AmendmentRequestStatus in context.AmendmentRequestStatuses
                                                      .Select(x => new { x.Code, x.LocalName })
                                                                 on a.AmendmentStatus equals AmendmentRequestStatus.Code
                                                                 into qStatusAmendJoin
                                                      from myJoinAmendmentRequest in qStatusAmendJoin.DefaultIfEmpty()

                                                      join qBDeclarationTaxesByTaxTypeCodeViews in context.DecTaxesByTaxTypeCodeViews on
                                                      new { DeclarationId = a.Id, Tenant = a.Tenant } equals
                                                      new { DeclarationId = qBDeclarationTaxesByTaxTypeCodeViews.DeclarationId, Tenant = qBDeclarationTaxesByTaxTypeCodeViews.Tenant }
                                                      into qBDeclarationTaxesByTaxTypeCodeViewsJoin
                                                      from MyDeclarationTaxesByTaxTypeCodeViews in qBDeclarationTaxesByTaxTypeCodeViewsJoin.DefaultIfEmpty()

                                                      select new DeclarationList()
                                                      {
                                                          Id = a.Id,
                                                          // AgentId = a.AgentId,
                                                          AutonomyRegionTypeName = a.AutonomyRegionType.LocalName,
                                                          CIFValue = a.CIFValue,
                                                          CustomerName = a.IsCourierDeclaration ? a.ImporterName : (a.CustomerCard.LocalName != null ? a.CustomerCard.LocalName : a.CustomerCard.EnglishName),
                                                          CustomFileNo = a.CustomFileNo,
                                                          DealValue = a.DealValue,
                                                          DeclarationNumber = !string.IsNullOrEmpty(a.DeclarationNumber) ? a.DeclarationNumber : (!string.IsNullOrEmpty(myJoinOriginalDeclaration.DeclarationNumber) ? myJoinOriginalDeclaration.DeclarationNumber : myJoinDisplayDeclarations.DeclarationNumber),

                                                          ExternalDeclarationNumber = a.ExternalDeclarationNumber,
                                                          HatraDate = a.HatraDate,

                                                          ImporterPassCountryName = a.ImporterPassCountry.LocalName,
                                                          IsChanged = a.IsChanged,
                                                          FileState = a.FileState,
                                                          LoadingFactor = a.LoadingFactor,
                                                          PaymentDate = a.PaymentDate,
                                                          IsSubmitDeclaration = a.IsSubmitDeclaration,
                                                          ProcedureCurrentName = a.GovernmentProcedureCurrent.LocalName,
                                                          TaxationDateTime = a.TaxationDateTime,
                                                          Tenant = a.Tenant,
                                                          TotalTax = a.TotalTax,
                                                          DeclarationVersionId = a.VersionId,
                                                          CreateDateTime = a.CreateDateTime,
                                                          UpdateDateTime = a.UpdateDateTime,
                                                          ImporterPassCountryCode = a.ImporterPassCountryCode,
                                                          ProcedureCurrentCode = a.ProcedureCurrentCode,
                                                          CustomerCode = a.CustomerCard == null ? null : a.CustomerCard.Code,
                                                          CustomerId = a.CustomerId,
                                                          SearchFields = a.SearchFields,
                                                          DeclarationOfficeName = a.DeclarationOffice == null ? null : a.DeclarationOffice.LocalName,
                                                          DeclarationNumberandVersionId = a.DeclarationNumber + (string.IsNullOrEmpty(a.VersionId) ? "" : " " + a.VersionId),
                                                          AutonomyRegionTypeCode = a.AutonomyRegionTypeCode,

                                                          DeclarationStatusTypeName = a.DeclarationStatusType == null ? null : a.DeclarationStatusType.LocalName,
                                                          IsCancelled = a.IsCancelled,
                                                          PlatformFee = a.PlatformFee,
                                                          StorageSiteCode = a.StorageSiteCode,
                                                          DeclarationStatusTypeCode = a.DeclarationStatusTypeCode,
                                                          DeclarationOfficeCode = a.DeclarationOfficeCode,
                                                          DeclarationOfficeNameForExport = a.DeclarationOffice == null ? null : a.DeclarationOffice.LocalName,


                                                          DepartmentId = a.DepartmentId,
                                                          DepartmentName = a.Department == null ? null : a.Department.LocalName,
                                                          CreatedByUserName =

                                                         a.CreatedByUser.Code,
                                                          IsHatraDateNull = a.HatraDate == null,
                                                          ImporterAddress = a.ImporterAddress,
                                                          ImporterName = a.Importer != null ? a.Importer.FullName : a.ImporterName,
                                                          EntitleImporterName = a.EntitleImporter.FullName,
                                                          UserNotes = a.UserNotes,
                                                          ImporterTypeCode = a.ImporterTypeCode,
                                                          ImporterTypeName = a.ImporterType != null ? a.ImporterType.LocalName : null,
                                                          HasConstraint = a.HasConstraint,
                                                          StorageSiteName = a.StorageSiteName,
                                                          SignerPersonalId = a.SignerPersonalId,
                                                          IsConvertedDeclaration = a.IsConvertedDeclaration,
                                                          CorrectionsXml = a.CorrectionsXml,
                                                          SignedByUserId = a.SignedByUserId,
                                                          ImporterCode = a.ImporterCode,
                                                          ExporterImporterCode = a.ImporterCode,
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
                                                          ItemsProcessTypesList = a.ItemsProcessTypesList,
                                                          CourierSuspentionCode = a.CourierSuspentionCode,
                                                          CourierSuspentionName = a.CourierSuspention != null ? a.CourierSuspention.LocalName : null,
                                                          DepositionStatusCode = a.DepositionStatusCode,
                                                          AmendmentDontDisplayInList = a.AmendmentDontDisplayInList,
                                                          IsClosedForFollowUp = dcs != null && dcs.IsClosedForFollowUp,
                                                          FastIndividualProcessCode = dcs != null ? dcs.FastIndividualProcessCode : null,
                                                          TotalInvoiceAmountInUSD = dcs != null ? dcs.TotalInvoiceAmountInUSD : null,
                                                          IsPending902 = dcs != null ? dcs.CourierPendingReasonList != null && dcs.CourierPendingReasonList.Contains("902") : false,
                                                          IsPendingNotNull = (dcs != null && dcs.CourierPendingReasonList != null && dcs.CourierPendingReasonList.Length > 0),
                                                          CourierPendingReasonList = dcs != null ? dcs.CourierPendingReasonList : null,
                                                          IntegratorCode = cd != null ? cd.IntegratorCode : null,
                                                          IntegratorName = cd != null ? cd.LocalName : null,

                                                          MAWB = cd != null ? cd.MAWB : null,
                                                          IsCourierMissingClassification = dcs != null ? dcs.IsCourierMissingClassification : false,
                                                          CargoDescription = myJoinConsignment != null ? myJoinConsignment.CargoDescription : null,
                                                          IsPaymentProtested = a.IsPaymentProtested,
                                                          DeclarationNoAmendment = myJoinOriginalDeclaration.DeclarationNumber,
                                                          CustomFileAmendment = myJoinOriginalDeclaration.CustomFileNo,
                                                          AmendmentStatus = a.AmendmentStatus,
                                                          AmendmentRequestNumber = a.AmendmentRequestNumber,
                                                          AmendmentCorrectedByUserName = a.AmendmentCorrectedByUser != null ? a.AmendmentCorrectedByUser.Code : null,
                                                          AmendmentissueDate = a.AmendmentissueDate
                                                         ,
                                                          Direction = a.Direction,
                                                          ExportFile = a.ExportFile,
                                                          DeclarationDocumentTypeCode = a.DeclarationDocumentTypeCode,
                                                          AgentRoleCode = a.AgentRoleCode,
                                                          DestinationCountryCode = a.DestinationCountryCode,
                                                          DestinationCountryName = a.CustomsCountry != null ? a.CustomsCountry.EnglishName : "",
                                                          LoadingDateTime = a.LoadingDateTime,

                                                          IsExporterConfirmation = a.IsExporterConfirmation,
                                                          CreateDateForExport = a.CreateDateTime,
                                                          TransportModeForExport = a.TransportModeId,
                                                          TransportModeId = a.TransportModeId,
                                                          CustomFileForExport = a.CustomFileNo,
                                                          CargoTypeName = myJoinConsignment != null ? myJoinConsignment.LocalName : null,
                                                          SecondCargoID = myJoinConsignment != null ? myJoinConsignment.SecondCargoID : null,
                                                          ThirdCargoID = myJoinConsignment != null ? myJoinConsignment.ThirdCargoID : null,
                                                          ManifestNumber = myJoinConsignment != null ? myJoinConsignment.ManifestNumber : null,
                                                          TerminalReleaseDate = dcs != null ? dcs.TerminalReleaseDate : null,
                                                          PhysicalCheck = a.PhysicalCheck,
                                                          PhysicalCheckName = a.PhysicalCheck == null ? "��� �����" : a.PhysicalCheckCode.Name,
                                                          DeclarationTypeCode = a.DeclarationTypeCode,

                                                          DeclarationTypeName = a.DeclarationType.LocalName,


                                                          FOBValueNIS = a.FOBValueNIS,
                                                          FOBValueDollar = a.FOBValueDollar,

                                                          AmendmentStatusName = myJoinAmendmentRequest != null ? myJoinAmendmentRequest.LocalName : null,

                                                          MehesFee = MyDeclarationTaxesByTaxTypeCodeViews.MehesFee,
                                                          VATReshimonFee = MyDeclarationTaxesByTaxTypeCodeViews.VATReshimonFee,
                                                          SecurityFee = MyDeclarationTaxesByTaxTypeCodeViews.SecurityFee,
                                                          ComputerFee = MyDeclarationTaxesByTaxTypeCodeViews.ComputerFee,
                                                          
                                                      });


                return query2;
            }



        }



        public IQueryable<Declaration> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Declaration> iQueryable, int tenant)
        {
            DeclarationCustomFilters filters = new DeclarationCustomFilters();

            iQueryable = filters.GetFilteredQuery(queryOperations, iQueryable, tenant, context);

            iQueryable = filters.GetFreelancerDeclarations(queryOperations, iQueryable, tenant);

            if (queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "IsAmendment") == null)
            {
                iQueryable = iQueryable.Where(x => x.AmendmentDontDisplayInList != true);

            }
            if (queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "IsContainerization") != null)
            {
                var query1 = (from a in context.Consignments
                              where a.ExportContainerizationID == null
                              select a);
                iQueryable = iQueryable.Where(x => query1.Any(c => c.DeclarationId == x.Id));
            }
            if (queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "PhysicalCheck") != null)
            {
                var qPhCh = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "PhysicalCheck");
                string physicalCheck;
                switch (qPhCh.FieldValue.ToString())
                {
                    case "N":
                        iQueryable = iQueryable.Where(x => x.PhysicalCheck == null);
                        break;
                    case "":
                        iQueryable = iQueryable.Where(x => x.PhysicalCheck == null);
                        break;
                    default:
                        iQueryable = iQueryable.Where(x => x.PhysicalCheck == qPhCh.FieldValue.ToString());

                        physicalCheck = qPhCh.FieldValue.ToString();
                        break;
                }

            }
            var filter = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "CustomerName");

            if (filter != null)
            {
                iQueryable = iQueryable.Where(x => (x.CustomerCard.LocalName != null ? x.CustomerCard.LocalName : x.CustomerCard.EnglishName).ToLower().StartsWith(filter.FieldValue.ToString().ToLower()));
            }

            var filter1 = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "IsConsOfDecEquelsCont");
            if (filter1 != null)
            {
                var query1 = (from a in context.Consignments
                              where a.ExportContainerizationID == filter1.FieldValue.ToString()
                              select a).Select(x => x.DeclarationId).ToList();

                iQueryable = iQueryable.Where(x => query1.Contains(x.Id));
            }


            return iQueryable;
        }
 

        private string getFastIndividualProcessName(string fastIndividualProcessCode)
        {
            if (fastIndividualProcessCode != null)
            {
                if (fastIndividualProcessCode == "F")
                {
                    fastIndividualProcessCode = "����";
                }
                else if (fastIndividualProcessCode == "I")
                {
                    fastIndividualProcessCode = "�����";
                }
            }
            return fastIndividualProcessCode;
        }

        private IQueryable<DeclarationList> GetIqueryableListForContainerization(IQueryable<Declaration> iQueryable, int tenant, string containerID, string CargoTypeCode, string ManifestNumber, string SecondCargoID, string ThirdCargoID)
        {

            var IsNewContainerization = string.IsNullOrEmpty(CargoTypeCode) && string.IsNullOrEmpty(ManifestNumber) && string.IsNullOrEmpty(SecondCargoID) && string.IsNullOrEmpty(ThirdCargoID);
            var qConsignmentNumber = (from a in context.Consignments
                                      where string.IsNullOrEmpty(containerID) || a.ExportContainerizationID == containerID
                                      group a by a.DeclarationId into gConsignments
                                      select
                                      new
                                      {
                                          DeclarationId = gConsignments.Key,
                                          ConsignmentNumber = IsNewContainerization ? gConsignments.Min(r => r.ConsignmentNumber) :
                                          gConsignments.Where(r => r.Tenant == tenant && r.CargoTypeCode == CargoTypeCode && r.ManifestNumber == ManifestNumber && r.SecondCargoID == SecondCargoID && r.ThirdCargoID == ThirdCargoID).Select(a => a.ConsignmentNumber).FirstOrDefault()
                                      });

            var q1stConsignments =
                (from a in context.Consignments.Include("CargoType")
                 join c in qConsignmentNumber
                 on new { a.DeclarationId, a.ConsignmentNumber } equals new { c.DeclarationId, c.ConsignmentNumber }
                 select a
                 );


            // int tenant = 1;
            // try
            // {

            //     string token = HttpContext.Current.Request.Headers["Token"];
            //     AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            //     tenant = authToken.Tenant;
            // }
            // catch (Exception)
            // {

            //     // throw;
            // }
            //  q1stConsignments = context.Consignments.Where(r => r.DeclarationId == "-1");


            IQueryable<DeclarationList> query = (from a in iQueryable.Include("ProcedureCurrent").Include("Importer")
                                                 join recConsignment in q1stConsignments
                                                 on a.Id equals recConsignment.DeclarationId into qjoinConsignments
                                                 from myJoinConsignment in qjoinConsignments.DefaultIfEmpty()
                                                 where a.AmendmentDontDisplayInList != true

                                                 select new DeclarationList()
                                                 {
                                                     Id = a.Id,
                                                     DeclarationNumber = a.DeclarationNumber,
                                                     Tenant = a.Tenant,
                                                     CreateDateTime = a.CreateDateTime,
                                                     CustomFileNo = a.CustomFileNo,
                                                     SearchFields = a.SearchFields,
                                                     DeclarationStatusTypeName = a.DeclarationStatusType == null ? null : a.DeclarationStatusType.LocalName,
                                                     TransportModeForExport = a.TransportModeId,
                                                     TransportModeId = a.TransportModeId,
                                                     DeclarationStatusTypeCode = a.DeclarationStatusTypeCode,
                                                     TransportModeName = a. TransportMode == null ? null : a.TransportMode.LocalName,
                                                     ExportFile = a.ExportFile,
                                                     PaymentDate = a.PaymentDate,
                                                     IsSubmitDeclaration = a.IsSubmitDeclaration,
                                                     CargoTypeName = myJoinConsignment != null && myJoinConsignment.CargoType != null ? myJoinConsignment.CargoType.LocalName : null,
                                                     SecondCargoID = myJoinConsignment != null ? myJoinConsignment.SecondCargoID : null,
                                                     ThirdCargoID = myJoinConsignment != null ? myJoinConsignment.ThirdCargoID : null,
                                                     ManifestNumber = myJoinConsignment != null ? myJoinConsignment.ManifestNumber : null,
                                                     Direction = a.Direction,
                                                     ProcedureCurrentName = a.GovernmentProcedureCurrent.LocalName,
                                                     TaxationDateTime = a.TaxationDateTime,
                                                     CustomerName = a.IsCourierDeclaration ? a.ImporterName : (a.CustomerCard.LocalName != null ? a.CustomerCard.LocalName : a.CustomerCard.EnglishName),
                                                     CargoTypeCode = myJoinConsignment.CargoTypeCode
                                                 });




            return query;
        }
        public List<DeclarationList> GetListForContainerization(QueryOperations queryOperations, int tenant, string containerID, string CargoTypeCode, string ManifestNumber, string SecondCargoID, string ThirdCargoID)
        {
            IQueryable<Declaration> iQueryable = GetIqueryable(tenant, queryOperations);

            GenericFilter filter = new GenericFilter();
            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<Declaration>(nonListQueryOperation, iQueryable);
            IQueryable<DeclarationList> query2 = GetIqueryableListForContainerization(iQueryable, tenant, containerID, CargoTypeCode, ManifestNumber, SecondCargoID, ThirdCargoID);
            query2 = filter.GetFilteredQuery<DeclarationList>(listQueryOperation, query2);

            query2 = GetByFilters(queryOperations, tenant, iQueryable, query2);
            return query2.ToList();
        }

        private IQueryable<Declaration> GetIqueryable(int tenant, QueryOperations queryOperations)
        {
            IQueryable<Declaration> iQueryable = (from a in context.Declarations

                                                  where a.Tenant == tenant
                                                  select a);
            iQueryable = ApplyCustomFilters(queryOperations, iQueryable, tenant);
            return iQueryable;
        }

        public IQueryable<DeclarationList> GetByFilters(QueryOperations queryOperations, int tenant, IQueryable<Declaration> iQueryable = null, IQueryable<DeclarationList> query2 = null) {

            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();

            if (iQueryable == null)
            {
                iQueryable = GetIqueryable(tenant, queryOperations);
            }

            int skippedPorts = queryOperations.PageIndex;

            if (query2 == null)
            {
                QueryOperations nonListQueryOperation = new QueryOperations();
                nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
                QueryOperations listQueryOperation = new QueryOperations();
                listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

                iQueryable = filter.GetFilteredQuery<Declaration>(nonListQueryOperation, iQueryable);

                query2 = GetIqueryableList(iQueryable, tenant);

                query2 = filter.GetFilteredQuery<DeclarationList>(listQueryOperation, query2);
            }

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DeclarationList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> DeclarationObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.Declaration", tenant).ToList();

                ObjectField objectField = (from a in DeclarationObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<DeclarationList, string>(queryOperations, query2);
                    }
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<DeclarationList, string>(queryOperations, query2);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<DeclarationList, double>(queryOperations, query2);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<DeclarationList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<DeclarationList, int>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<DeclarationList, bool>(queryOperations, query2);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    query2 = sortClass.GetSorterQuery<DeclarationList, decimal>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderByDescending(d => d.TaxationDateTime);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.TaxationDateTime);
            }
            if (!queryOperations.GetAll)
            {
                query2 = query2.Skip(skippedPorts);
                query2 = query2.Take(queryOperations.PageSize);
            }
            return query2;


        }

    }

    public class MyDecJoin
    {
        //public string CourierMasterId { get; set; }
        public bool IsClosedForFollowUp { get; set; }
        public string DeclarationId { get; set; }
        /*private string mycode;*/
        //public string FastIndividualProcessName { get; set; }
        public string FastIndividualProcessCode
        {
            get; set;
            /*
            get { return mycode; }
            set
            {
                mycode = value;
                switch (mycode)
                {
                    case "F":
                        this.FastIndividualProcessName = "����";
                        break;
                    case "I":
                        this.FastIndividualProcessName = "�����";
                        break;
                    default:
                        break;
                }
            }
            */
        }
        public decimal? TotalInvoiceAmountInUSD { get; set; }
        public bool IsPending902 { get; set; }
        public bool IsPending900 { get; set; }
        public string CourierPendingReasonList { get; set; }
        public string MAWB { get; set; }
        public DateTime? TerminalReleaseDate { get; set; }
        public bool IsCourierMissingClassification { get; set; }
        public bool IsPendingNotNull { get; set; }

        public string IntegratorCode { get; set; }

        public string IntegratorName { get; set; }

    }
}
