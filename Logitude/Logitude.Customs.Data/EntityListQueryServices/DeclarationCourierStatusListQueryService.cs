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
        private bool _RequiredFieldErrorsForCourierDeclarationIsValid;

        private IQueryable<DeclarationCourierStatusList> GetIqueryableList(IQueryable<DeclarationCourierStatus> iQueryable)
        {
            //var qMmmnActionError = (from action in context.DeclarationMamanSpecialActions
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
            
            IQueryable<DeclarationCourierStatusList> query = (from a in iQueryable
                                                              join d in context.Declarations.Include("GovernmentProcedureCurrent").Include("CourierCustomStatus").Include("DeclarationStatusType").Include("CustomerCard").Include("Importer").Include("AgentTalkBackType")
                                                              on a.DeclarationId equals d.Id
                                                              join c in context.CourierDeclarations
                                                              on a.DeclarationId equals c.DeclarationId

                                                              select new DeclarationCourierStatusList()
                                                              {
                                                                  DeclarationId = a.DeclarationId,
                                                                  Tenant = a.Tenant,
                                                                  CourierMasterId = c.CourierMasterId,
                                                                  IsDOCTab = (a.DocumentStatusCode == "M" || a.DocumentStatusCode == "X"),
                                                                  IsSVGTab = a.IsCourierMissingClassification == true,
                                                                  IsMNFRTab = (a.CourierManifestStatusCode == "R"),
                                                                  IsDECRTab = (a.CourierDeclarationStatusCode == "R"),
                                                                  IsHOLDTab = (a.CourierPendingReasonCode != null),
                                                                  IsMNFTab = (a.CourierManifestStatusCode == "M" || a.CourierManifestStatusCode == "X"),
                                                                  IsPAYTab = a.CourierPaymentStatusCode == "R",
                                                                  IsDECTab = (a.CourierDeclarationStatusCode == "M" || a.CourierDeclarationStatusCode == "X"),
                                                                  IsACCTab = (a.StorageSiteStatusCode == "2" || a.SpecialActionStatus == "X"),
                                                                  CourierManifestStatusCode = !_RequiredFieldErrorsForCourierDeclarationIsValid ? "M" : a.CourierManifestStatusCode,
                                                                  CourierDeclarationStatusCode = a.CourierDeclarationStatusCode,
                                                                  CourierPaymentStatusCode = a.CourierPaymentStatusCode,
                                                                  IsCourierMissingClassification = a.IsCourierMissingClassification,
                                                                  IsClosedForFollowUp = a.IsClosedForFollowUp,
                                                                  HighLowValue = a.HighLowValue,
                                                                  DocumentStatusCode = a.DocumentStatusCode,
                                                                  CourierHawb = d.CourierHAWB,
                                                                  ProcedureCurrentCode = d.ProcedureCurrentCode,
                                                                  ProcedureCurrentName = d.GovernmentProcedureCurrent != null ? d.GovernmentProcedureCurrent.LocalName : null,
                                                                  CourierCustomStatusCode = d.CourierCustomStatusCode,
                                                                  CourierCustomStatusName = d.CourierCustomStatus != null ? d.CourierCustomStatus.LocalName : null,
                                                                  DeclarationStatusTypeName = d.DeclarationStatusType == null ? null : d.DeclarationStatusType.LocalName,
                                                                  ImporterCode = d.ImporterCode,
                                                                  ImporterName = d.ImporterName != null ? d.ImporterName : (d.ImporterId != null ? d.Importer.FullName : d.ImporterName),
                                                                  CustomerName = d.CustomerCard.LocalName != null ? d.CustomerCard.LocalName : d.CustomerCard.EnglishName,
                                                                  CourierSearchFields = d.CourierSearchFields,
                                                                  TotalInvoiceAmountInUSD = a.TotalInvoiceAmountInUSD,
                                                                  DeclarationNumber = d.DeclarationNumber,
                                                                  CourierPendingReasonCode = a.CourierPendingReasonCode,
                                                                  CourierPendingReasonName = a.CourierPendingReason != null ? a.CourierPendingReason.LocalName : null,
                                                                  CourierPendingReasonErrorPlace = a.CourierPendingReason != null ? a.CourierPendingReason.ErrorPlace : null,
                                                                  PendingRemarks = a.PendingRemarks,
                                                                  CourierSuspentionReasonName = d.CourierSuspentionReasonCode != null ? d.AgentTalkBackType.LocalName : null,
                                                                  AcceptanceStatusCode = d.AcceptanceStatusCode,
                                                                  //MamanStatusCode = d.MamanStatusCode,
                                                                  //MamanErrorXml = d.MamanErrorXml,
                                                                  CourierSuspentionCode = d.CourierSuspentionCode,
                                                                  CourierSuspentionName = d.CourierSuspention != null ? d.CourierSuspention.LocalName : null,
                                                                  SpecialActionStatus = a.SpecialActionStatus,
                                                                  //SpecialActionsErrorXml = ao.text,
                                                                  FastIndividualProcessCode = a.FastIndividualProcessCode,
                                                                  ManualProcessCode = a.ManualProcessCode,
                                                                  TerminalSuspentionNumber = a.TerminalSuspentionNumber,
                                                                  LastMileStatusCode = a.LastMileStatusCode,
                                                                  LastMileStatusDate = a.LastMileStatusDate,
                                                                  LastMileStatusRemarks = a.LastMileStatusRemarks,
                                                                  StorageSiteStatusCode = a.StorageSiteStatusCode,
                                                                  StorageSiteStatusName = a.MamanStatus != null ? a.MamanStatus.LocalName : null,
                                                                  StorageSiteErrorText = a.StorageSiteErrorText,
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

        private IQueryable<DeclarationCourierStatus> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeclarationCourierStatus> iQueryable, int tenant)
        {
            //filters.addAdditionalFilter("CourierMasterId", this.entityPM.Id, null, null, "Equals", false, false, false, "string");
            var courierMasterIdF = queryOperations.QueryFilterItems.Where(r => r.FieldName == "CourierMasterId").FirstOrDefault();
            if (courierMasterIdF != null)
            {
                string courierMasterId = (string)courierMasterIdF.FieldValue;
                _RequiredFieldErrorsForCourierDeclarationIsValid = InjectionUtil.GetRequiredFieldErrorsForCourierDeclarationIsValid(courierMasterId, tenant);
            }
            return iQueryable;
        }
    }


}
	