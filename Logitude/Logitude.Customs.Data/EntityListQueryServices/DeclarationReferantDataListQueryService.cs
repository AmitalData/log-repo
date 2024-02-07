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
using Logitude.Customs.Data.CustomFilters;
using Logitude.Customs.Data.DataContracts;
using System.Runtime.Remoting.Contexts;
//using System.Data.Entity;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class DeclarationReferantDataListQueryService
    {
        private IQueryable<DeclarationReferantDataList> GetIqueryableList(IQueryable<DeclarationReferantData> iQueryable)
        {

            IQueryable<DeclarationReferantDataList> query = (from a in iQueryable.Include("CustomsVendor")
                                                             join d in context.Declarations
                                                             .Include("CustomerCard")
                                                             .Include("DeclarationOffice")
                                                             .Include("DeclarationStatusType")
                                                             .Include("Importer")
                                                             .Include("PackageType")
                                                             on a.DeclarationIdToDisplay equals d.Id

                                                             //join declarationStatus in context.DeclarationStatuses
                                                             //on a.DeclarationIdToDisplay equals declarationStatus.DeclarationId into qjoinDeclarationStatuses
                                                             

                                                             select new DeclarationReferantDataList()
                                                             {
                                                                 Tenant = a.Tenant,

                                                                 DeclarationId = a.DeclarationIdToDisplay,

                                                                 OrderNumber = a.OrderNumber,

                                                                 EstimatedArrivalDate = a.EstimatedArrivalDate,

                                                                 Weight = a.Weight,

                                                                 ClassificationStatus = a.ClassificationStatus,

                                                                 ControllerStatus = a.ControllerStatus,

                                                                 CollectionOfMoneyStatus = a.CollectionOfMoneyStatus,

                                                                 FollowUpDate = a.FollowUpDate,

                                                                 WithPaper = a.WithPaper,

                                                                 IsClosedForFollowUp = a.IsClosedForFollowUp,

                                                                 IsClassificationRemarks = a.IsClassificationRemarks,

                                                                 IsControllerRemarks = a.IsControllerRemarks,

                                                                 PreClassification = a.PreClassification,

                                                                 CustomFileNo = d.CustomFileNo,

                                                                 CustomerName = (d.CustomerCard.LocalName != null ? d.CustomerCard.LocalName : d.CustomerCard.EnglishName),

                                                                 TransportModeId = d.TransportModeId,

                                                                 DeclarationOfficeCode = d.DeclarationOfficeCode,
                                                                 DeclarationOfficeName = d.DeclarationOffice.LocalName,

                                                                 VendorName = a.CustomsVendor.VendorName,
                                                                 ArrivalDate = a.ArrivalDate != null ? a.ArrivalDate : a.EstimatedArrivalDate,
                                                                 ATAOrETA = a.ArrivalDate != null ? "ATA" : "ETA",

                                                                 DeclarationStatusTypeName = d.DeclarationStatusType == null ? null : d.DeclarationStatusType.LocalName,

                                                                 DeclarationStatusTypeCode = d.DeclarationStatusTypeCode,
                                                                 ExceptionReasonsList = a.ExceptionReasonsList,
                                                                 ReferentUserId = d.ReferentUserId,
                                                                 ReferantName = d.ReferentUser.Contact.LocalName,
                                                                 DepartmentId = d.DepartmentId,
                                                                 AvailabilityDate = d.AvailabilityDate,

                                                                 NewFile = a.NewFile,
                                                                 Favorite = a.Favorite,
                                                                 ///itzik : 
                                                                 IsCustomerLogBoxActivated = d.CustomerCard.Customer.LogBoxActivated,
                                                                 SortedColumns = (a.NewFile && a.Favorite ? 1 : (a.NewFile ? 2 : (a.Favorite ? 3 : 4))),
                                                                 IsCancelled = d.IsCancelled,

                                                                 /*
LEFT OUTER JOIN AMINETNXT_MAIN.Contacts Extent8 ON Extent1.ClassifiedUserId = Extent8.Id
LEFT OUTER JOIN AMINETNXT_MAIN.Contacts Extent9 ON Extent9.Id = Extent1.CollectorUserId
LEFT OUTER JOIN AMINETNXT_MAIN.Contacts Extent10 ON Extent10.Id = Extent1.ControllerUserId
                                                                  */
                                                                 ClassifiedUserName = a.ClassifiedUser.Contact.LocalName,
                                                                 CollectorUserName = a.CollectorUser.Contact.LocalName,
                                                                 ControllerUserName = a.ControllerUser.Contact.LocalName,

                                                                 LastStatusDate = a.LastStatusDate,
                                                                 LastStatusName = a.LastStatusName,
                                                                 OrderMoney = a.OrderMoney,
                                                                 ForwarderName = a.ForwarderCard.LocalName,
                                                                 PackageQuantity = a.PackageQuantity,
                                                                 FclLclName = a.FclLclCodeTable == null ? null : a.FclLclCodeTable.Name,
                                                                 StorageSiteCode = d.StorageSiteCode,
                                                                 HatraDate = d.HatraDate,
                                                                 PaymentDate = d.PaymentDate,
                                                                 TaxationDateTime = d.TaxationDateTime,
                                                                 CustomerCode = d.CustomerCard == null ? null : d.CustomerCard.Code,
                                                                 ImporterCode = d.ImporterCode,
                                                                 ProcedureCurrentCode = d.ProcedureCurrentCode,
                                                                 ImporterFile = a.ImporterFile,
                                                                 AEOImporter = d.Importer.FacilitationTypeCode,
                                                                 TeamName = a.ReferantTeam.LocalName,
                                                                 Team = a.ReferantTeam.Code,
                                                                 FileOpenDate = a.FileOpenDate,
                                                                 ProcedureCurrentName = d.GovernmentProcedureCurrent.LocalName,
                                                                 StorageSiteName = d.StorageSiteName,
                                                                 ImporterName = d.Importer != null ? d.Importer.FullName : d.ImporterName,
                                                                 IsAvailabilityDateNull = d.AvailabilityDate == null,
                                                                 IsPaymentDateNull = d.PaymentDate == null,
                                                                 DeclarationNumber = d.DeclarationNumber,
                                                                 IsHatraDateNull = d.HatraDate == null,
                                                                 RequestedCustomsDocId = d.RequestedCustomsDocId,
                                                                 PaymentDate_Date = d.PaymentDate,
                                                                 PaymentDate_Time = d.PaymentDate != null ? System.Data.Entity.DbFunctions.CreateTime(d.PaymentDate.Value.Hour, d.PaymentDate.Value.Minute, 0).Value.Hours.ToString() + ":" + System.Data.Entity.DbFunctions.CreateTime(d.PaymentDate.Value.Hour, d.PaymentDate.Value.Minute, 0).Value.Minutes.ToString() : "",
                                                                 IsClose = d.IsClose,
                                                                 PhysicalCheck = d.PhysicalCheckCode.Code,
                                                                 FclLcl = a.FclLcl,
                                                                 Actions = "",
                                                                 CancelRequestStatusCode = d.CancelRequestStatusCode,
                                                                 IsExceptionReasonsListNull = string.IsNullOrEmpty(a.ExceptionReasonsList),
                                                                  IsManualPayment = a.IsManualPayment,
                                                                 Commodity=a.Commodity,
                                                                 ReferantUserName="",
                                                                 DepartmentName="",                                                                

                                                                 PackageTypeCode = a.PackageType.LocalName,
                                                                 LastStatusRemarks = a.LastStatusRemarks,
                                                                 RemoveInclusiveVisibility = "",
                                                                 Hawb = a.Hawb,
                                                                 Mawb=a.Mawb,
                                                                 ImporterApproval  = a.ImporterApproval,
                                                                 CustomerId=d.CustomerId,
                                                                 VendorId=a.VendorId,
                                                                 //OccuredStatuses = qjoinDeclarationStatuses.DefaultIfEmpty().Select(r=>r.StatusCode.Status_Code),
                                                             });
            return query;

        }

        private IQueryable<DeclarationReferantData> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeclarationReferantData> iQueryable, int tenant)
        {
            DeclarationReferantDataCustomFilters filters = new DeclarationReferantDataCustomFilters();
            var filter = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "RetrievData");
            var OccuredStatusesFilter = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "OccuredStatuses");
            var NotOccuredStatusesFilter = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "NotOccuredStatuses");


            if (OccuredStatusesFilter != null && !string.IsNullOrEmpty(OccuredStatusesFilter.FieldValue.ToString()))
            {
                var q = context.DeclarationStatuses.Where(decStatus => OccuredStatusesFilter.FieldValue.ToString().Contains(decStatus.StatusCode.Status_Code)).Select(r => r.DeclarationId);
                iQueryable = (from a in iQueryable.Where(r => q.Contains(r.DeclarationId)) select a);
            }
            if (NotOccuredStatusesFilter != null && !string.IsNullOrEmpty(NotOccuredStatusesFilter.FieldValue.ToString()))
            {
                var notContainsFilter = NotOccuredStatusesFilter.FieldValue.ToString();

                var notContainsQuery = context.DeclarationStatuses
                    .Where(decStatus => notContainsFilter.Contains(decStatus.StatusCode.Status_Code))
                    .Select(r => r.DeclarationId);

                iQueryable = iQueryable.Where(r => !notContainsQuery.Contains(r.DeclarationId));

            }

            iQueryable = filters.GetFreelancerDeclarationReferantDatas(queryOperations, iQueryable, tenant, context);

            return iQueryable;

        }

        public DeclarationReferantDataSummary GetQueriesCounts(int tenant, List<string> refId, List<string> depId, string transportMode)
        {
            DeclarationReferantDataSummary declarationReferantDataSummary = new DeclarationReferantDataSummary();
            IQueryable<DeclarationReferantDataList> declarationReferantDatas = (from a in context.DeclarationReferantDatas
                                                                                join d in context.Declarations
                                                                                on a.DeclarationIdToDisplay equals d.Id
                                                                                where a.Tenant == tenant && d.IsCancelled == false
                                                                                select new DeclarationReferantDataList()
                                                                                {
                                                                                    IsClosedForFollowUp = a.IsClosedForFollowUp,
                                                                                    FollowUpDate = a.FollowUpDate,
                                                                                    PreClassification = a.PreClassification,
                                                                                    ClassificationStatus = a.ClassificationStatus,
                                                                                    ControllerStatus = a.ControllerStatus,
                                                                                    CollectionOfMoneyStatus = a.CollectionOfMoneyStatus,
                                                                                    IsAvailabilityDateNull = d.AvailabilityDate == null,
                                                                                    IsPaymentDateNull = d.PaymentDate == null,
                                                                                    IsHatraDateNull = d.HatraDate == null,
                                                                                    TransportModeId = d.TransportModeId,
                                                                                    ReferentUserId = d.ReferentUserId,
                                                                                    DepartmentId = d.DepartmentId,
                                                                                    RequestedCustomsDocId = d.RequestedCustomsDocId,
                                                                                    IsCancelled = d.IsCancelled,
                                                                                    IsClose = d.IsClose,
                                                                                    IsExceptionReasonsListNull = string.IsNullOrEmpty(a.ExceptionReasonsList),
                                                                                    CancelRequestStatusCode = d.CancelRequestStatusCode

                                                                                });

            if (refId.Count > 0)
            {
                declarationReferantDatas = declarationReferantDatas.Where(x => refId.Contains(x.ReferentUserId));
            }
            if (transportMode != "All")
            {
                declarationReferantDatas = declarationReferantDatas.Where(x => x.TransportModeId == transportMode);
            }
            if (depId.Count > 0)
            {
                declarationReferantDatas = declarationReferantDatas.Where(x => depId.Contains(x.DepartmentId));
            }

            var date2 = DateTime.Today.Date.AddDays(1);

            var qGroupIt = (from a in declarationReferantDatas
                            group a by 1 into groupBy1
                            select new DeclarationReferantDataSummary
                            {
                                FilesInProcess = groupBy1.Count(x => x.IsClosedForFollowUp != "1"),
                                TrackingCases = groupBy1.Count(x => x.FollowUpDate != null && x.FollowUpDate > DateTime.Today.Date && x.FollowUpDate < date2 && x.IsClosedForFollowUp != "1"),
                                FilesInOCR = groupBy1.Count(x => x.PreClassification == "P" && x.IsClosedForFollowUp != "1"),
                                FilesInSivug = groupBy1.Count(x => x.ClassificationStatus == "P" && x.IsClosedForFollowUp != "1"),
                                FilesInReview = groupBy1.Count(x => x.ControllerStatus == "P" && x.IsClosedForFollowUp != "1"),
                                FilesInCreditControl = groupBy1.Count(x => x.CollectionOfMoneyStatus == "P" && x.IsClosedForFollowUp != "1"),
                                FilesAvailableFreeOfCharge = groupBy1.Count(x => x.IsPaymentDateNull == true && x.IsAvailabilityDateNull == false && x.IsClosedForFollowUp != "1"),
                                AllCases = declarationReferantDatas.Count(),
                                FilesWithoutRelease = groupBy1.Count(x => x.IsPaymentDateNull == false && x.IsHatraDateNull == true && x.IsClose == false && x.IsClosedForFollowUp != "1" && x.CancelRequestStatusCode != "5"),
                                FilesInProcess_A = groupBy1.Count(x => x.IsClosedForFollowUp != "1" && x.IsAvailabilityDateNull == false && x.IsClosedForFollowUp != "1"),
                                TrackingCases_A = groupBy1.Count(x => x.FollowUpDate == DateTime.Today && x.IsAvailabilityDateNull == false && x.IsClosedForFollowUp != "1"),
                                FilesInOCR_A = groupBy1.Count(x => x.PreClassification == "P" && x.IsAvailabilityDateNull == false && x.IsClosedForFollowUp != "1"),
                                FilesInSivug_A = groupBy1.Count(x => x.ClassificationStatus == "P" && x.IsAvailabilityDateNull == false && x.IsClosedForFollowUp != "1"),
                                FilesInReview_A = groupBy1.Count(x => x.ControllerStatus == "P" && x.IsAvailabilityDateNull == false && x.IsClosedForFollowUp != "1"),
                                FilesInCreditControl_A = groupBy1.Count(x => x.CollectionOfMoneyStatus == "P" && x.IsAvailabilityDateNull == false && x.IsClosedForFollowUp != "1"),
                                FilesAvailableFreeOfCharge_A = groupBy1.Count(x => x.IsPaymentDateNull == true && x.IsAvailabilityDateNull == false && x.IsClosedForFollowUp != "1"),
                                AllCases_A = groupBy1.Count(x => x.IsAvailabilityDateNull == false),
                                FilesInAllInclusive = groupBy1.Count(x => x.IsHatraDateNull == false && x.RequestedCustomsDocId == 1),
                                FilesInAllInclusive_A = groupBy1.Count(x => x.IsHatraDateNull == false && x.RequestedCustomsDocId == 1 && x.IsAvailabilityDateNull == false),
                                FilesRejectedByController = groupBy1.Count(x => x.ControllerStatus == "X" && x.IsClosedForFollowUp != "1"),
                                FilesRejectedByController_A = groupBy1.Count(x => x.ControllerStatus == "X" && x.IsAvailabilityDateNull == false && x.IsClosedForFollowUp != "1"),
                                FilesRejectedByClassification = groupBy1.Count(x => x.ClassificationStatus == "X" && x.IsClosedForFollowUp != "1"),
                                FilesRejectedByClassification_A = groupBy1.Count(x => x.ClassificationStatus == "X" && x.IsAvailabilityDateNull == false && x.IsClosedForFollowUp != "1"),
                                FilesToPay = groupBy1.Count(x => x.ClassificationStatus == "V" && x.ControllerStatus == "V" && x.IsClosedForFollowUp != "1" && x.CollectionOfMoneyStatus == "V" && x.IsAvailabilityDateNull == false && x.IsPaymentDateNull == true && x.IsExceptionReasonsListNull == true),
                                FilesToPay_A = groupBy1.Count(x => x.ClassificationStatus == "V" && x.ControllerStatus == "V" && x.IsClosedForFollowUp != "1" && x.CollectionOfMoneyStatus == "V" && x.IsAvailabilityDateNull == false && x.IsPaymentDateNull == true && x.IsExceptionReasonsListNull == true),
                                FilesWithoutRelease_A = groupBy1.Count(x => x.IsPaymentDateNull == false && x.IsHatraDateNull == true && x.CancelRequestStatusCode != "5" && x.IsClose == false && x.IsAvailabilityDateNull == false && x.IsClosedForFollowUp != "1"),

                            }
                            );
            declarationReferantDataSummary = qGroupIt.FirstOrDefault() ?? new DeclarationReferantDataSummary(); ;



            //declarationReferantDataSummary.FilesInProcess = declarationReferantDatas.Where(x => x.IsClosedForFollowUp != "1").Count();
            //declarationReferantDataSummary.TrackingCases = declarationReferantDatas.Where(x => x.FollowUpDate == DateTime.Today).Count();
            //declarationReferantDataSummary.FilesInOCR = declarationReferantDatas.Where(x => x.PreClassification == "P").Count();
            //declarationReferantDataSummary.FilesInSivug = declarationReferantDatas.Where(x => x.ClassificationStatus == "P").Count();
            //declarationReferantDataSummary.FilesInReview = declarationReferantDatas.Where(x => x.ControllerStatus == "P").Count();
            //declarationReferantDataSummary.FilesInCreditControl = declarationReferantDatas.Where(x => x.CollectionOfMoneyStatus == "P").Count();
            //declarationReferantDataSummary.FilesAvailableFreeOfCharge = declarationReferantDatas.Where(x => x.IsPaymentDateNull == true && !x.IsAvailabilityDateNull == false).Count();


            // declarationReferantDataSummary.AllCases = declarationReferantDatas.Count();

            //declarationReferantDataSummary.FilesInProcess_A = declarationReferantDatas.Where(x => x.IsClosedForFollowUp != "1" && x.IsAvailabilityDateNull == false).Count();
            //declarationReferantDataSummary.TrackingCases_A = declarationReferantDatas.Where(x => x.FollowUpDate == DateTime.Today && x.IsAvailabilityDateNull == false).Count();
            //declarationReferantDataSummary.FilesInOCR_A = declarationReferantDatas.Where(x => x.PreClassification == "P" && x.IsAvailabilityDateNull == false).Count();
            //declarationReferantDataSummary.FilesInSivug_A = declarationReferantDatas.Where(x => x.ClassificationStatus == "P" && x.IsAvailabilityDateNull == false).Count();
            //declarationReferantDataSummary.FilesInReview_A = declarationReferantDatas.Where(x => x.ControllerStatus == "P" && x.IsAvailabilityDateNull == false).Count();
            //declarationReferantDataSummary.FilesInCreditControl_A = declarationReferantDatas.Where(x => x.CollectionOfMoneyStatus == "P" && x.IsAvailabilityDateNull == false).Count();
            //declarationReferantDataSummary.FilesAvailableFreeOfCharge_A = declarationReferantDatas.Where(x => x.IsPaymentDateNull == true && x.IsAvailabilityDateNull == false).Count();
            //declarationReferantDataSummary.AllCases_A = declarationReferantDatas.Where(x => x.IsAvailabilityDateNull == false).Count();




            //declarationReferantDataSummary.FilesInAllInclusive = declarationReferantDatas.Where(x => x.IsHatraDateNull == false && x.RequestedCustomsDocId == 1).Count();
            //declarationReferantDataSummary.FilesInAllInclusive_A = declarationReferantDatas.Where(x => x.IsHatraDateNull == false && x.RequestedCustomsDocId == 1 && x.IsAvailabilityDateNull == false).Count();




            //declarationReferantDataSummary.FilesRejectedByController = declarationReferantDatas.Where(x => x.ControllerStatus == "X").Count();
            //declarationReferantDataSummary.FilesRejectedByController_A = declarationReferantDatas.Where(x => x.ControllerStatus == "X" && x.IsAvailabilityDateNull == false).Count();


            //declarationReferantDataSummary.FilesRejectedByClassification = declarationReferantDatas.Where(x => x.ClassificationStatus == "X").Count();
            //declarationReferantDataSummary.FilesRejectedByClassification_A = declarationReferantDatas.Where(x => x.ClassificationStatus == "X" && x.IsAvailabilityDateNull == false).Count();

            return declarationReferantDataSummary;
        }

    }


}
