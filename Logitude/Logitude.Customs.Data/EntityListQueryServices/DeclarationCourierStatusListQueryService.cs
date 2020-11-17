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
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class DeclarationCourierStatusListQueryService
    {
        public bool RequiredFieldErrorsForCourierDeclarationIsValid;

        private IQueryable<DeclarationCourierStatusList> GetIqueryableList(IQueryable<DeclarationCourierStatus> iQueryable)
        {
            //            .Where( r=> r.MamanSpecialActionStatusCode== "2" )
            //            group action by action.DeclarationId into gaction
            //            select new
            //            {
            //                id = gaction.Key,
            //                text =
            //                gaction.Select(r => r.MamanSpecialActionsErrorXml).Aggregate((b4, after) => b4 + " " + after),
            //                //string.Join(" ", gaction.Select(r => r.MamanSpecialActionsErrorXml)),

            //                //(string)gaction.Aggregate((b4, after) => b4.MamanLabelText1 + " "+ after.DeclarationId),
            //            });
            //bool test = false    ;
            //if (test)
            //{
            //    var resQ = qMmmnActionError.ToList();
            //}        
            //TestSql(iQueryable);

            var qDeclarationPaymentPendingHold =
            (from p in context.DeclarationPendings
             where p.Status == "A"
             group p by p.DeclarationID into g
             select new MyJoin
             {
                 DeclarationId = g.Key,
                 ErrorPlace = g.Any(r => r.CourierPendingReason.ErrorPlace == "1"),

                 //CourierPendingReason1stName = g.DefaultIfEmpty(
                 //new DeclarationPending()
                 //{
                 //    CourierPendingReason = new CourierPendingReason { }
                 //})
                 //.FirstOrDefault().CourierPendingReason.LocalName

                 CourierPendingReason1stName = g.Any() ? g.FirstOrDefault().CourierPendingReason.LocalName : null


             }
            );


            IQueryable<DeclarationCourierStatusList> query = (from a in iQueryable

                                                              join d in context.Declarations.Include("GovernmentProcedureCurrent").Include("CourierCustomStatus").Include("DeclarationStatusType").Include("CustomerCard").Include("Importer").Include("AgentTalkBackType")
                                                              on a.DeclarationId equals d.Id
                                                              join c in context.CourierDeclarations
                                                              on a.DeclarationId equals c.DeclarationId



                                                              join errorPlace in qDeclarationPaymentPendingHold
                                                              on a.DeclarationId equals errorPlace.DeclarationId
                                                              into errorPlaceOuterJoin
                                                              from errorPlaceOuterJoinNullable in errorPlaceOuterJoin.DefaultIfEmpty()

                                                              join cm in context.CourierMasters on c.CourierMasterId equals cm.Id

                                                              //join pendingListNames in qDeclarationPendingListNames
                                                              //on a.DeclarationId equals pendingListNames.DeclarationId
                                                              //into pendingListNamesOuterJoin
                                                              //from pendingListNamesOuterJoinNullable in pendingListNamesOuterJoin.DefaultIfEmpty()
                                                              //from pendingListNamesOuterJoinNullable in pendingListNamesOuterJoin.ToList().ToString()


                                                              //join declarationPendings in context.DeclarationPendings
                                                              //on a.DeclarationId equals declarationPendings.DeclarationID
                                                              //into declarationPendingsJoin
                                                              //from declarationPendingsListNames in declarationPendingsJoin.Where(r => r.Status == "A").ToList()

                                                              select new DeclarationCourierStatusList()
                                                              {
                                                                  DeclarationId = a.DeclarationId,
                                                                  Tenant = a.Tenant,
                                                                  CourierMasterId = c.CourierMasterId,
                                                                  IsDOCTab = (a.DocumentStatusCode == "M" || a.DocumentStatusCode == "X"),
                                                                  IsSVGTab = a.IsCourierMissingClassification == true,
                                                                  IsMNFRTab = (a.CourierManifestStatusCode == "R"),
                                                                  IsDECRTab = (a.CourierDeclarationStatusCode == "R"),
                                                                  //IsHOLDTab = (a.CourierPendingReasonCode != null),
                                                                  IsHOLDTab = (a.CourierPendingReasonList != null),
                                                                  IsMNFTab = (a.CourierManifestStatusCode == "M" || a.CourierManifestStatusCode == "X"),
                                                                  IsPAYTab = a.CourierPaymentStatusCode == "R",
                                                                  IsDECTab = (a.CourierDeclarationStatusCode == "M" || a.CourierDeclarationStatusCode == "X"),
                                                                  IsACCTab = (a.StorageSiteStatusCode == "2" || a.SpecialActionStatus == "X"),
                                                                  CourierManifestStatusCode = !RequiredFieldErrorsForCourierDeclarationIsValid ? "M" : a.CourierManifestStatusCode,
                                                                  CourierDeclarationStatusCode = a.CourierDeclarationStatusCode,
                                                                  CourierPaymentStatusCode = a.CourierPaymentStatusCode,
                                                                  IsCourierMissingClassification = a.IsCourierMissingClassification,
                                                                  IsClosedForFollowUp = a.IsClosedForFollowUp,
                                                                  HighLowValue = a.HighLowValue,
                                                                  DocumentStatusCode = a.DocumentStatusCode,
                                                                  SortedDocumentStatusCode= (a.DocumentStatusCode == "M" || a.DocumentStatusCode == "X")? "M" : a.DocumentStatusCode,
                                                                  SortedCourierDeclarationStatus=(a.CourierDeclarationStatusCode == "M" || a.CourierDeclarationStatusCode == "X") ? "M" : a.CourierDeclarationStatusCode,
                                                                  SortedCourierManifestStatus = (a.CourierManifestStatusCode == "M" || a.CourierManifestStatusCode == "X") ? "M" : a.CourierManifestStatusCode,
                                                                  CourierHawb = d.CourierHAWB,
                                                                  ProcedureCurrentCode = d.ProcedureCurrentCode,
                                                                  ProcedureCurrentName = d.GovernmentProcedureCurrent != null ? d.GovernmentProcedureCurrent.LocalName : null,
                                                                  CourierCustomStatusCode = d.CourierCustomStatusCode,
                                                                  CourierCustomStatusName = d.CourierCustomStatus != null ? d.CourierCustomStatus.LocalName : null,
                                                                  DeclarationStatusTypeName = d.DeclarationStatusType == null ? null : d.DeclarationStatusType.LocalName,
                                                                  ImporterCode = d.ImporterCode,
                                                                  ImporterName = d.ImporterName != null ? d.ImporterName : (d.ImporterId != null ? d.Importer.FullName : d.ImporterName),
                                                                  SortedImporterCode=d.ImporterCode,
                                                                  CustomerName = d.CustomerCard.LocalName != null ? d.CustomerCard.LocalName : d.CustomerCard.EnglishName,
                                                                  CourierSearchFields = d.CourierSearchFields,
                                                                  TotalInvoiceAmountInUSD = a.TotalInvoiceAmountInUSD,
                                                                  DeclarationNumber = d.DeclarationNumber,
                                                                  //CourierPendingReasonCode = a.CourierPendingReasonCode,
                                                                  //CourierPendingReasonName = a.CourierPendingReason != null ? a.CourierPendingReason.LocalName : null,


                                                                  CourierPendingReasonErrorPlace = errorPlaceOuterJoinNullable != null ?
                                                                  (
                                                                  errorPlaceOuterJoinNullable.ErrorPlace == true ? "1" : null)
                                                                  : null,

                                                                  CourierPendingReasonName = errorPlaceOuterJoinNullable != null ?
                                                                  errorPlaceOuterJoinNullable.CourierPendingReason1stName
                                                                  : null,
                                                                  //CourierPendingReasonName = string.Join(",", pendingListNamesOuterJoin.Select(p => p.ToString())),


                                                                  //PendingRemarks = a.PendingRemarks,
                                                                  //PendingRemarks = declarationPendingsListNames.CourierPendingReason.LocalName,


                                                                  CourierSuspentionReasonName = d.CourierSuspentionReasonCode != null ? d.AgentTalkBackType.LocalName : null,
                                                                  AcceptanceStatusCode = d.AcceptanceStatusCode,
                                                                  CourierSuspentionCode = d.CourierSuspentionCode,
                                                                  CourierSuspentionName = d.CourierSuspention != null ? d.CourierSuspention.LocalName : null,
                                                                  SpecialActionStatus = a.SpecialActionStatus,
                                                                  //SpecialActionsErrorXml = ao.text,
                                                                  FastIndividualProcessCode = a.FastIndividualProcessCode,
                                                                  ManualProcessCode = a.ManualProcessCode,
                                                                  TerminalSuspentionNumber = a.TerminalSuspentionNumber,
                                                                  LastMileStatusCode = a.LastMileStatusCode,
                                                                  LastMileStatusDate = a.LastMileStatusDate,
                                                                  LastMileStatusName = a.LastMileStatusName,
                                                                  LastMileStatusRemarks = a.LastMileStatusRemarks,
                                                                  StorageSiteStatusCode = a.StorageSiteStatusCode,
                                                                  StorageSiteStatusName = a.MamanStatus != null ? a.MamanStatus.LocalName : null,
                                                                  StorageSiteErrorText = a.StorageSiteErrorText,
                                                                  CourierPendingReasonList = a.CourierPendingReasonList,


                                                                  AirlineId = cm.CustomsAirline.AirlinePrefix,
                                                                  MAWB = cm.MAWB,
                                                                  MasterGrossMassMeasure = cm.GrossMassMeasure,
                                                                  MasterPackageQuantity = cm.PackageQuantity,
                                                                  MasterCreateDateTime = cm.CreateDateTime,
                                                                  MasterGatewayPortCode = cm.GatewayPortCode,
                                                                  MasterEstimatedArrivalDate = cm.EstimatedArrivalDate,
                                                                  MasterStorageSiteCode = cm.StorageSiteCode,
                                                                  MasterHAWB = cm.HAWB,
                                                                  CustomFileNo = d.CustomFileNo,
                                                                  TruckerId=a.Trucker.Card.Code
                                                              });


            return query;
        }

        private void TestSql(IQueryable<DeclarationCourierStatus> iQueryable)
        {
            using (var logger = (context as DbContextBase).CreateLogger())
            {
                var aa = (from a in iQueryable
                          join d in context.Declarations.Include("GovernmentProcedureCurrent").Include("CourierCustomStatus").Include("DeclarationStatusType").Include("CustomerCard").Include("Importer").Include("AgentTalkBackType")
                          on a.DeclarationId equals d.Id
                          join c in context.CourierDeclarations
                          on a.DeclarationId equals c.DeclarationId


                          //join mmnAction in qMmmnActionError
                          //on a.DeclarationId equals mmnAction.id
                          //into leftJoin
                          //from ao in leftJoin.DefaultIfEmpty()


                          select new DeclarationCourierStatusList()
                          {
                              CourierPendingReasonCode = a.CourierPendingReasonCode,
                              CourierPendingReasonName = a.CourierPendingReason != null ? a.CourierPendingReason.LocalName : null,
                              CourierPendingReasonErrorPlace = a.CourierPendingReason != null ? a.CourierPendingReason.ErrorPlace : null,
                              PendingRemarks = a.PendingRemarks,
                              CourierSuspentionReasonName = d.CourierSuspentionReasonCode != null ? d.AgentTalkBackType.LocalName : null
                          });
                aa.ToList();
                var sql = logger.ToString();
            }
        }

        private IQueryable<DeclarationCourierStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DeclarationCourierStatus> iQueryable, int tenant)
        {
            //filters.addAdditionalFilter("CourierMasterId", this.entityPM.Id, null, null, "Equals", false, false, false, "string");
            var courierMasterIdF = queryOperations.QueryFilterItems.Where(r => r.FieldName == "CourierMasterId").FirstOrDefault();
            if(courierMasterIdF != null)
            {
                string courierMasterId = (string)courierMasterIdF.FieldValue;
                RequiredFieldErrorsForCourierDeclarationIsValid = InjectionUtil.GetRequiredFieldErrorsForCourierDeclarationIsValid(courierMasterId, tenant);
            }
            return iQueryable;
        }

        public IQueryable<DeclarationCourierStatusList> GetByCourierMasterId(string courierMasterId, int tenant)
        {
            IQueryable<DeclarationCourierStatus> DeclarationCourierStatusQuery = (from a in context.DeclarationCourierStatuses
                                                                                  where a.Tenant  == tenant
                                                                                  select a);


            IQueryable<DeclarationCourierStatusList> q = GetIqueryableList(DeclarationCourierStatusQuery);
            q = q.Where(r => r.CourierMasterId == courierMasterId);


            return q;

        }

    }

    public class MyJoin
    {
        public bool ErrorPlace { get; set; }
    public string CourierPendingReason1stName { get;  set; }
    internal string DeclarationId { get; set; }
    }
}
	