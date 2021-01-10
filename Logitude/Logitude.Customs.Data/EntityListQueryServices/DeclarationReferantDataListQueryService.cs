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

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class DeclarationReferantDataListQueryService
    {
        private IQueryable<DeclarationReferantDataList> GetIqueryableList(IQueryable<DeclarationReferantData> iQueryable)
        {
            IQueryable<DeclarationReferantDataList> query = (from a in iQueryable.Include("CustomsVendor")
                                                             join d in context.Declarations.Include("CustomerCard").Include("DeclarationOffice").Include("DeclarationStatusType").Include("Importer")
                                                             on a.DeclarationId equals d.Id
                                                             select new DeclarationReferantDataList()
                                                             {
                                                                 Tenant = a.Tenant,

                                                                 DeclarationId = a.DeclarationId,

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

                                                                 CustomerName = d.CustomerCard.LocalName,

                                                                 TransportModeId = d.TransportModeId,

                                                                 DeclarationOfficeCode = d.DeclarationOfficeCode,
                                                                 DeclarationOfficeName= d.DeclarationOffice.LocalName,

                                                                 VendorName = a.CustomsVendor.VendorName,
                                                                 ArrivalDate = a.ArrivalDate != null ? a.ArrivalDate : a.EstimatedArrivalDate,
                                                                 ATAOrETA = a.ArrivalDate != null ? "ATA" : "ETA",

                                                                 DeclarationStatusTypeName = d.DeclarationStatusType == null ? null : d.DeclarationStatusType.LocalName,

                                                                 DeclarationStatusTypeCode = d.DeclarationStatusTypeCode,
                                                                 ExceptionReasonsList = a.ExceptionReasonsList,
                                                                 ReferentUserId = d.ReferentUserId,
                                                                 DepartmentId = d.DepartmentId,
                                                                 AvailabilityDate = d.AvailabilityDate,
 
                                                                 NewFile = a.NewFile,
                                                                 Favorite = a.Favorite,
                                                                  IsCustomerLogBoxActivated = d.CustomerCard.Customer.LogBoxActivated,
                                                                 SortedColumns = (a.NewFile && a.Favorite ? 1 : (a.NewFile ? 2 : (a.Favorite ? 3 : 4))),
                                                                 IsCancelled = d.IsCancelled,
                                                                 ClassifiedUserName = a.ClassifiedUser.Contact.LocalName,
                                                                 CollectorUserName = a.CollectorUser.Contact.LocalName,
                                                                 ControllerUserName= a.ControllerUser.Contact.LocalName, 
                                                                 LastStatusDate = a.LastStatusDate,
                                                                 LastStatusName = a.LastStatusName,
                                                                 OrderMoney = a.OrderMoney,
                                                                 StorageSiteCode=d.StorageSiteCode,
                                                                 HatraDate=d.HatraDate,
                                                                 PaymentDate=d.PaymentDate,
                                                                 TaxationDateTime=d.TaxationDateTime,
                                                                 CustomerCode = d.CustomerCard == null ? null : d.CustomerCard.Code,
                                                                 ImporterCode=d.ImporterCode,
                                                                  ProcedureCurrentCode = d.ProcedureCurrentCode,
                                                                 ImporterFile=a.ImporterFile,
                                                                 AEOImporter=d.Importer.FacilitationTypeCode,
                                                                 Team = a.ReferantTeam.LocalName,
                                                                 FileOpenDate=a.FileOpenDate,
                                                                 ProcedureCurrentName = d.GovernmentProcedureCurrent.LocalName,
                                                                 StorageSiteName=d.StorageSiteName,
                                                                 ImporterName = d.Importer != null ? d.Importer.FullName : d.ImporterName,
                                                                 IsAvailabilityDateNull= d.AvailabilityDate == null,
                                                                 IsPaymentDateNull = d.PaymentDate == null,

                                                                 DeclarationNumber = d.DeclarationNumber,
                                                                 IsHatraDateNull= d.HatraDate== null,
                                                                 RequestedCustomsDocId = "1",// waiting 4 elisheva task 


                                                             }) ;
                                                                
                                              return query;

        }

        private IQueryable<DeclarationReferantData> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeclarationReferantData> iQueryable, int tenant)
        {
            DeclarationReferantDataCustomFilters filters = new DeclarationReferantDataCustomFilters();

            var filter = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "RetrievData");
            if (filter == null)
            {

                iQueryable = filters.GetFilteredQuery(iQueryable);
                iQueryable = filters.GetFreelancerDeclarationReferantDatas(queryOperations, iQueryable, tenant,context);
            }
            iQueryable = filters.GetFreelancerDeclarationReferantDatas(queryOperations, iQueryable, tenant, context);

            return iQueryable;

        }

        public DeclarationReferantDataSummary GetQueriesCounts(int tenant)
        {
            DeclarationReferantDataSummary declarationReferantDataSummary = new DeclarationReferantDataSummary();
            IQueryable<DeclarationReferantDataList> declarationReferantDatas = (from a in context.DeclarationReferantDatas
                                                                                join d in context.Declarations
                                                                                on a.DeclarationId equals d.Id
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
                                                                                });

            declarationReferantDataSummary.FilesInProcess = declarationReferantDatas.Where(x => x.IsClosedForFollowUp != "1").Count();
            declarationReferantDataSummary.TrackingCases = declarationReferantDatas.Where(x => x.FollowUpDate == DateTime.Today).Count();
            declarationReferantDataSummary.FilesInOCR = declarationReferantDatas.Where(x => x.PreClassification == "P").Count();
            declarationReferantDataSummary.FilesInSivug = declarationReferantDatas.Where(x => x.ClassificationStatus == "P").Count();
            declarationReferantDataSummary.FilesInReview = declarationReferantDatas.Where(x => x.ControllerStatus == "P").Count();
            declarationReferantDataSummary.FilesInCreditControl = declarationReferantDatas.Where(x => x.CollectionOfMoneyStatus == "P").Count();
            declarationReferantDataSummary.FilesAvailableFreeOfCharge = declarationReferantDatas.Where(x => x.IsPaymentDateNull == true && !x.IsAvailabilityDateNull == false).Count();
        
            declarationReferantDataSummary.AllCases = declarationReferantDatas.Count();

            declarationReferantDataSummary.FilesInProcess_A = declarationReferantDatas.Where(x => x.IsClosedForFollowUp != "1" && x.IsAvailabilityDateNull == false).Count();
            declarationReferantDataSummary.TrackingCases_A = declarationReferantDatas.Where(x => x.FollowUpDate == DateTime.Today && x.IsAvailabilityDateNull == false).Count();
            declarationReferantDataSummary.FilesInOCR_A = declarationReferantDatas.Where(x => x.PreClassification == "P" && x.IsAvailabilityDateNull == false).Count();
            declarationReferantDataSummary.FilesInSivug_A = declarationReferantDatas.Where(x => x.ClassificationStatus == "P" && x.IsAvailabilityDateNull == false).Count();
            declarationReferantDataSummary.FilesInReview_A = declarationReferantDatas.Where(x => x.ControllerStatus == "P" && x.IsAvailabilityDateNull == false).Count();
            declarationReferantDataSummary.FilesInCreditControl_A = declarationReferantDatas.Where(x => x.CollectionOfMoneyStatus == "P" && x.IsAvailabilityDateNull == false).Count();
            declarationReferantDataSummary.FilesAvailableFreeOfCharge_A = declarationReferantDatas.Where(x => x.IsPaymentDateNull == true &&   x.IsAvailabilityDateNull == false).Count();
            declarationReferantDataSummary.AllCases_A = declarationReferantDatas.Where(x =>   x.IsAvailabilityDateNull == false).Count();




            declarationReferantDataSummary.FilesInAllInclusive = declarationReferantDatas.Where(x => x.IsHatraDateNull == false && x.RequestedCustomsDocId=="1").Count();
            declarationReferantDataSummary.FilesInAllInclusive_A = declarationReferantDatas.Where(x => x.IsHatraDateNull == false && x.RequestedCustomsDocId == "1" && x.IsAvailabilityDateNull == false).Count();

            

                
            declarationReferantDataSummary.FilesRejectedByController = declarationReferantDatas.Where(x => x.ControllerStatus == "X" ).Count();
            declarationReferantDataSummary.FilesRejectedByController_A = declarationReferantDatas.Where(x => x.ControllerStatus == "X" && x.IsAvailabilityDateNull == false).Count();


            declarationReferantDataSummary.FilesRejectedByClassification = declarationReferantDatas.Where(x => x.ClassificationStatus == "X").Count();
            declarationReferantDataSummary.FilesRejectedByClassification_A = declarationReferantDatas.Where(x => x.ClassificationStatus == "X" && x.IsAvailabilityDateNull == false).Count();

            return declarationReferantDataSummary;
        }

    }


}
