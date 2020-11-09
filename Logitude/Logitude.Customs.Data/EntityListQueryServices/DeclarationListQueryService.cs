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
using System.Web;

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
            /*
            var qCourierPendingReasonCode =
                (from dp in context.DeclarationPendings
                 where dp.Status == "A"
                 group dp by dp.DeclarationID into gDeclarationPendings
                 select new
                 {
                     DeclarationId = gDeclarationPendings.Key,
                     CourierPendingReasonCode = gDeclarationPendings.Count() == 1 ? gDeclarationPendings.FirstOrDefault().CourierPendingReasonCode : null,
                 }
                    );
            var qCourierPendingReasonLocalName = (
                from rs in qCourierPendingReasonCode
                join tablecode in context.CourierPendingReasons on rs.CourierPendingReasonCode equals tablecode.Code
                into myinnerjoin
                from tablecode1 in myinnerjoin.DefaultIfEmpty()
                select new
                {
                    DeclarationID = rs.DeclarationId,
                    CourierPendingReasonName = tablecode1 != null ? tablecode1.LocalName : "רשימה"
                }
                   );
            
            */

            var qJoin =
(from p in context.CourierDeclarations
 //join dec in context.Declarations
 //                    on p.DeclarationId equals dec.Id
 //                    into DecJoin
 //from myDeclarations in DecJoin
 
 join sts1 in context.DeclarationCourierStatuses
                     on p.DeclarationId equals sts1.DeclarationId
                     into DeclarationCourierStatusesJoin

 

 from myDeclarationCourierStatuses in DeclarationCourierStatusesJoin
 
 select new { p.CourierMasterId, p.CourierMaster/*, myDeclarations*/, myDeclarationCourierStatuses }
 );

            var qMyJoin =
                (
                from rec in qJoin
                select new MyDecJoin
                {
                    DeclarationId = rec.myDeclarationCourierStatuses.DeclarationId/*myDeclarations.Id*/,
                    //CourierMasterId = rec.CourierMasterId,
                    IsClosedForFollowUp = rec.myDeclarationCourierStatuses != null ? rec.myDeclarationCourierStatuses.IsClosedForFollowUp : false,
                    //FastIndividualProcessName = "",
                    FastIndividualProcessCode = rec.myDeclarationCourierStatuses != null ? rec.myDeclarationCourierStatuses.FastIndividualProcessCode : null,
                    TotalInvoiceAmountInUSD = rec.myDeclarationCourierStatuses != null ? rec.myDeclarationCourierStatuses.TotalInvoiceAmountInUSD : null,
                    IsPending902 = rec.myDeclarationCourierStatuses != null ? (rec.myDeclarationCourierStatuses.CourierPendingReasonList.Contains("902") ? true : false) : false,
                    IsPending900 = rec.myDeclarationCourierStatuses != null ? (rec.myDeclarationCourierStatuses.CourierPendingReasonList.Contains("900") ? true : false) : false,
                    //CourierPendingReasonList = rec.myDeclarationCourierStatuses != null ? rec.myDeclarationCourierStatuses.CourierPendingReasonList : null,
                    MAWB = rec.CourierMaster != null ? rec.CourierMaster.MAWB : null,
                    IsCourierMissingClassification = rec.myDeclarationCourierStatuses != null ? rec.myDeclarationCourierStatuses.IsCourierMissingClassification : false,
                    IsPendingNotNull = rec.myDeclarationCourierStatuses != null ? (rec.myDeclarationCourierStatuses.CourierPendingReasonList != null && rec.myDeclarationCourierStatuses.CourierPendingReasonList.Length > 0 ? true : false) : false,
                }
                );

    //        var q1stConsignments =
    //            (from a in context.Consignments
    //             group a by a.DeclarationId into gConsignments
    //             select gConsignments.Take(1))
    //                 .SelectMany(r => r);
    //        q1stConsignments =
    //(from a in context.Consignments
    // group a by a.DeclarationId into gConsignments
    // select gConsignments.FirstOrDefault());


            var qConsignmentNumber = (from a in context.Consignments
                    group a by a.DeclarationId into gConsignments
                    select
                    new
                    {
                        DeclarationId = gConsignments.Key,
                        ConsignmentNumber = gConsignments.Min(r => r.ConsignmentNumber)
                    });

            var q1stConsignments =
                (from a in context.Consignments
                 join c in qConsignmentNumber
                 on new { a.DeclarationId, a.ConsignmentNumber } equals new { c.DeclarationId, c.ConsignmentNumber }
                 select a
                 );

            var qOriginalDeclarations = context.Declarations.Where(x =>  x.IsAmendment !=true);
                                     


            bool test = false;
            if (test)
            {
                var myMyJoin = qMyJoin.ToList();
                var s = q1stConsignments.ToList();
                /*var pr = qCourierPendingReasonLocalName.ToList();*/
            }
            int tenant = 1;
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                tenant = authToken.Tenant;
            }
            catch (Exception)
            {

                // throw;
            }
            bool isCourierEnv = context.CustomsSettings.FirstOrDefault(r => r.Tenant == tenant).CompanyType =="B" ;
            if (!isCourierEnv)
            {
                qMyJoin = (from rec in context.CourierDeclarations.Where(r => r.DeclarationId == "-1")
                           select new MyDecJoin() {
                               DeclarationId = rec.DeclarationId,
                               //CourierMasterId = rec.CourierMasterId,
                               IsClosedForFollowUp = false,
                               //FastIndividualProcessName = "",
                               FastIndividualProcessCode = "",
                               TotalInvoiceAmountInUSD = 1,
                               IsPending902 = true,
                               IsPending900 = true,
                               //CourierPendingReasonList= "",
                               MAWB = "",
                               IsCourierMissingClassification = true,
                               IsPendingNotNull = true,
                           });
                //qMyJoin = Enumerable.Empty<MyDecJoin>().AsQueryable();
                q1stConsignments = context.Consignments.Where(r => r.DeclarationId == "-1");
                //q1stConsignments = Enumerable.Empty<Consignment>().AsQueryable();
            }


            IQueryable<DeclarationList> query = (from a in iQueryable.Include("DeclarationOffice").Include("AutonomyRegionType").Include("CustomerCard").Include("EntitleImporterCountry").Include("ImporterEntitlementType").Include("ImporterPassCountry").Include("ProcedureCurrent").Include("TransferImporterCountry").Include("Department").Include("DeclarationStatusType")
                                                 //.Include("CreatedByUser.Contact")
                                                 .Include("Importer").Include("EntitleImporter").Include("TransferImporter").Include("ImporterType").Include("TransferImporterType").Include("EntitleImporterType").Include("StorageStatus").Include("FreightPaymentMethod")
                                                 .Include("CustomsCountry").Include("CustomsShip")

                                                 join recJoin in qMyJoin
                                                              on a.Id equals recJoin.DeclarationId
                                                              into qrecJoin
                                                 from myJoin in qrecJoin.DefaultIfEmpty()


                                                 join recConsignment in q1stConsignments
                                                 on a.Id equals recConsignment.DeclarationId into qjoinConsignments
                                                 from myJoinConsignment in qjoinConsignments.DefaultIfEmpty()


                                                 join recOriginalDeclarations in qOriginalDeclarations
                                                 on a.AmendmentOriginalDeclartation equals recOriginalDeclarations.Id
                                                 into originalDeclarations
                                                 from myJoinOriginalDeclaration in originalDeclarations.DefaultIfEmpty()

                                                     /*
                                                     join pr in qCourierPendingReasonLocalName
                                                     on a.Id equals pr.DeclarationID into leftjoinCourierPendingReasonLocalName
                                                     from mypr in leftjoinCourierPendingReasonLocalName.DefaultIfEmpty()
                                                     */

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
                                                     //CustomsFileNo = qJoin != null ? (qJoin.FirstOrDefault().myDeclarations != null ? qJoin.FirstOrDefault().myDeclarations.CustomFileNo : null ) : null,
                                                     //CourierHAWB = qJoin != null ? (qJoin.FirstOrDefault().myDeclarations != null ? qJoin.FirstOrDefault().myDeclarations.CourierHAWB : null) : null,
                                                     AmendmentDontDisplayInList = a.AmendmentDontDisplayInList,




                                                     IsClosedForFollowUp = myJoin != null ? myJoin.IsClosedForFollowUp : false,
                                                     FastIndividualProcessCode = myJoin != null ? myJoin.FastIndividualProcessCode : null,
                                                     //FastIndividualProcessName = myJoin != null ? myJoin.FastIndividualProcessName : null,
                                                     TotalInvoiceAmountInUSD = myJoin != null ? myJoin.TotalInvoiceAmountInUSD : null,
                                                     IsPending902 = myJoin != null ? myJoin.IsPending902 : false,
                                                     IsPending900 = myJoin != null ? myJoin.IsPending900 : false,
                                                     IsPendingNotNull = myJoin != null ? myJoin.IsPendingNotNull : false,
                                                     //CourierPendingReasonList = myJoin != null ? myJoin.CourierPendingReasonList : null,
                                                     /*CourierPendingReasonList = mypr != null ? mypr.CourierPendingReasonName : null,*/
                                                     //CourierPendingReasonName = a.CourierPendingReasonName,

                                                     MAWB = myJoin != null ? myJoin.MAWB : null,
                                                     IsCourierMissingClassification = myJoin != null ? myJoin.IsCourierMissingClassification : false,
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
                                                     ShipCode = a.ShipCode,
                                                     ShipName = a.CustomsShip != null ? a.CustomsShip.EnglishName : "",
                                                     IsExporterConfirmation = a.IsExporterConfirmation,
                                                     TruckerId=a.Trucker.Card.Code,
                                                 });




            return query;
        }

        private IQueryable<Declaration> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Declaration> iQueryable, int tenant)
        {
            DeclarationCustomFilters filters = new DeclarationCustomFilters();

            iQueryable = filters.GetFilteredQuery(queryOperations, iQueryable);

            iQueryable = filters.GetFreelancerDeclarations(queryOperations, iQueryable, tenant);

            iQueryable = iQueryable.Where(x => x.AmendmentDontDisplayInList != true);

            return iQueryable;
        }
        /*
        private string getCourierPendingReasonName(DeclarationPM entityPM)
        {
            var courierPendingReasonList = entityPM.CourierPendingReasonList;
            if (!string.IsNullOrWhiteSpace(courierPendingReasonList))
            {
                if (courierPendingReasonList.Contains(","))
                {
                    courierPendingReasonList = "רשימה";
                }
                else
                {
                    CourierPendingReasonQueryService myCourierPendingReasonQueryService = new CourierPendingReasonQueryService(entityPM.Tenant);
                    CourierPendingReasonPM courierPendingReasonPM = myCourierPendingReasonQueryService.GetSingle(courierPendingReasonList, false, false);
                    courierPendingReasonList = courierPendingReasonPM.LocalName;
                }
            }
            return courierPendingReasonList;
        }

        */

        private string getFastIndividualProcessName(string fastIndividualProcessCode)
        {
            if (fastIndividualProcessCode != null)
            {
                if (fastIndividualProcessCode == "F")
                {
                    fastIndividualProcessCode = "מהיר";
                }
                else if (fastIndividualProcessCode == "I")
                {
                    fastIndividualProcessCode = "פרטני";
                }
            }
            return fastIndividualProcessCode;
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
                        this.FastIndividualProcessName = "מהיר";
                        break;
                    case "I":
                        this.FastIndividualProcessName = "פרטני";
                        break;
                    default:
                        break;
                }
            }
            */
        }
        public  decimal? TotalInvoiceAmountInUSD { get; set; }
        public bool IsPending902 { get; set; }
        public bool IsPending900 { get; set; }
        //public string CourierPendingReasonList { get; set; }
        public string MAWB { get; set; }
        public bool IsCourierMissingClassification { get; set; }
        public bool IsPendingNotNull { get; set; }
    }
}
	