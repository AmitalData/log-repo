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
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.Repsitories;
using Devart.Data.Linq;
using System.Security.Cryptography;

namespace Logitude.Customs.Data.EntityListQueryServices
{

    public partial class DeclarationCourierStatusListQueryService
    {
        public bool RequiredFieldErrorsForCourierDeclarationIsValid;

        private IQueryable<DeclarationCourierStatusList> GetIqueryableList(IQueryable<DeclarationCourierStatus> iQueryable)
        {
            var FromExcelQueryJoin = (from courierhawb in context.CourierHawbFromExcels
                                      where courierhawb.CreatedByUser.Id == this.isFilter && courierhawb.NotFound != true
                                      select courierhawb);
            if (string.IsNullOrEmpty(this.isFilter))
            {
                FromExcelQueryJoin = context.CourierHawbFromExcels.Where(r => r.DeclarationId == "-1");
            }

			IQueryable<DeclarationCourierStatusList> query = (from a in iQueryable.Include("Trucker")

                                                              join d in context.Declarations.Include("GovernmentProcedureCurrent").Include("CourierCustomStatus").Include("DeclarationStatusType").Include("CustomerCard").Include("Importer").Include("AgentTalkBackType")
                                                              on a.DeclarationId equals d.Id
                                                              join c in context.CourierDeclarations
                                                              on a.DeclarationId equals c.DeclarationId

                                                              join cm in context.CourierMasters on c.CourierMasterId equals cm.Id

                                                              join recFromExcelQueryJoin in FromExcelQueryJoin
                                                              on a.DeclarationId equals recFromExcelQueryJoin.DeclarationId into qFromExcelJoin
                                                              from myJoinFromExcel in qFromExcelJoin.DefaultIfEmpty()

                                                              select new DeclarationCourierStatusList()
                                                              {
                                                                  DeclarationId = a.DeclarationId,
                                                                  Tenant = a.Tenant,
                                                                  CourierMasterId = c.CourierMasterId,
                                                                  IsDOCTab = (a.DocumentStatusCode == "M" || a.DocumentStatusCode == "X"),
                                                                  IsSVGTab = a.IsCourierMissingClassification == true,
                                                                  IsMNFRTab = (a.CourierManifestStatusCode == "R"),
                                                                  IsDECRTab = (a.CourierDeclarationStatusCode == "R"),
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
                                                                  SortedDocumentStatusCode = (a.DocumentStatusCode == "M" || a.DocumentStatusCode == "X") ? "M" : a.DocumentStatusCode,
                                                                  SortedCourierDeclarationStatus = (a.CourierDeclarationStatusCode == "M" || a.CourierDeclarationStatusCode == "X") ? "M" : a.CourierDeclarationStatusCode,
                                                                  SortedCourierManifestStatus = (a.CourierManifestStatusCode == "M" || a.CourierManifestStatusCode == "X") ? "M" : a.CourierManifestStatusCode,
                                                                  CourierHawb = d.CourierHAWB,
                                                                  ProcedureCurrentCode = d.ProcedureCurrentCode,
                                                                  ProcedureCurrentName = d.GovernmentProcedureCurrent != null ? d.GovernmentProcedureCurrent.LocalName : null,
                                                                  CourierCustomStatusCode = d.CourierCustomStatusCode,
                                                                  CourierCustomStatusName = d.CourierCustomStatus != null ? d.CourierCustomStatus.LocalName : null,
                                                                  DeclarationStatusTypeName = d.DeclarationStatusType == null ? null : d.DeclarationStatusType.LocalName,
                                                                  HatraDate = d.HatraDate,
                                                                  ImporterCode = d.ImporterCode,
                                                                  CasualImporterTel = d.CasualImporterTel,
                                                                  ImporterAddress = d.ImporterAddress,
                                                                  ImporterName = d.ImporterName != null ? d.ImporterName : (d.ImporterId != null ? d.Importer.FullName : d.ImporterName),
                                                                  SortedImporterCode = d.ImporterCode,
                                                                  CustomerName = d.CustomerCard.LocalName != null ? d.CustomerCard.LocalName : d.CustomerCard.EnglishName,
                                                                  CourierSearchFields = d.CourierSearchFields,
                                                                  TotalInvoiceAmountInUSD = a.TotalInvoiceAmountInUSD,
                                                                  DeclarationNumber = d.DeclarationNumber,
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
                                                                  StorageSiteStatusCodeText = a.StorageSiteStatusCode,
                                                                  StorageSiteStatusName = a.MamanStatus != null ? a.MamanStatus.LocalName : null,
                                                                  StorageSiteErrorText = a.StorageSiteErrorText,
                                                                  CourierPendingReasonList = a.CourierPendingReasonList,
                                                                  NotApprovedPendingList = a.NotApprovedPendingList,


                                                                  AirlineId = cm.CustomsAirline.AirlinePrefix,

                                                                  IntegratorName = cm.Card != null ? cm.Card.LocalName : null,
                                                                  MAWB = cm.MAWB,
                                                                  MasterGrossMassMeasure = cm.GrossMassMeasure,
                                                                  MasterPackageQuantity = cm.PackageQuantity,
                                                                  MasterCreateDateTime = cm.CreateDateTime,
                                                                  MasterGatewayPortCode = cm.GatewayPortCode,
                                                                  MasterEstimatedArrivalDate = cm.EstimatedArrivalDate,
                                                                  MasterStorageSiteCode = cm.StorageSiteCode,
                                                                  DeclarationStorageSiteCode = d.StorageSiteCode,
                                                                  MasterHAWB = cm.HAWB,
                                                                  TerminalReleaseDate = a.TerminalReleaseDate,
                                                                  CustomFileNo = d.CustomFileNo,
                                                                  TruckerId = a.Trucker.Card.Code,
                                                                  CrateNumber = a.CrateNumber,
                                                                  TruckerName = a.Trucker.Card.LocalName,
                                                                  AmendmentDontDisplayInList = d.AmendmentDontDisplayInList,
                                                                  IsAmendment = d.IsAmendment == true ? true : false,
                                                                  CargoDescription = d.CargoDescription,
                                                                  FinalRelease = !d.HatraDate.HasValue,
                                                                  CasualSupplierName = d.CasualSupplierName,
                                                                  Delivered = a.Delivered,
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
        private string isFilter;
        private IQueryable<DeclarationCourierStatus> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeclarationCourierStatus> iQueryable, int tenant)
        {
            //filters.addAdditionalFilter("CourierMasterId", this.entityPM.Id, null, null, "Equals", false, false, false, "string");
            var courierMasterIdF = queryOperations.QueryFilterItems.Where(r => r.FieldName == "CourierMasterId").FirstOrDefault();
            if (courierMasterIdF != null)
            {
                string courierMasterId = (string)courierMasterIdF.FieldValue;
                RequiredFieldErrorsForCourierDeclarationIsValid = InjectionUtil.GetRequiredFieldErrorsForCourierDeclarationIsValid(courierMasterId, tenant);
            }
            var filter = queryOperations.QueryFilterItems.FirstOrDefault(x => x.FieldName == "NotApprovedPendingList");
            if (filter != null)
            {
                iQueryable = iQueryable.Where(d => d.NotApprovedPendingList != null);
            }
            var CourierPendingFilter = queryOperations.QueryFilterItems.Where(r => r.FieldName == "CourierPendingReasonList").FirstOrDefault();
            if (CourierPendingFilter != null)
            {
                string myFilter = CourierPendingFilter.FieldValue.ToString();
                iQueryable = iQueryable.Where(x => x.CourierPendingReasonList.StartsWith(myFilter + ",") || x.CourierPendingReasonList.Contains("," + myFilter + ",") || x.CourierPendingReasonList.EndsWith("," + myFilter) || x.CourierPendingReasonList.Equals(myFilter));

            }
            this.isFilter = null;
            var CourierHawbsFromExcel = queryOperations.QueryFilterItems.Where(r => r.FieldName == "CourierHawbsFromExcel").FirstOrDefault();
            if (CourierHawbsFromExcel != null)
            {
                RequiredFieldErrorsForCourierDeclarationIsValid = true;
                this.isFilter = CourierHawbsFromExcel.FieldValue.ToString();
                IQueryable<CourierHawbFromExcel> FromExcelQueryJoin = (from courierhawb in context.CourierHawbFromExcels
                                                                       where courierhawb.CreatedByUser.Id == this.isFilter && courierhawb.NotFound != true
                                                                       select courierhawb);
                iQueryable = iQueryable.Where(d => FromExcelQueryJoin.Select(de => de.DeclarationId).Contains(d.DeclarationId));
            }


            return iQueryable;
        }

        public IQueryable<DeclarationCourierStatusList> GetByCourierMasterId(string courierMasterId, int tenant, string userId = null, bool IsWorkSheetFromExcel = false)
        {
            IQueryable<DeclarationCourierStatus> DeclarationCourierStatusQuery = (from a in context.DeclarationCourierStatuses
                                                                                  where a.Tenant == tenant
                                                                                  select a);


            IQueryable<DeclarationCourierStatusList> q = GetIqueryableList(DeclarationCourierStatusQuery);
            if (IsWorkSheetFromExcel)
            {
                var hawbsFromExcel = (from a in context.CourierHawbFromExcels
                                      where a.Tenant == tenant && a.CreatedByUserId == userId && a.NotFound != true
                                      select a);
                q = (from cd in hawbsFromExcel
                     join dStatus in q
                     on cd.DeclarationId equals dStatus.DeclarationId
                     select dStatus);
            }
            else
            {
                q = q.Where(r => r.CourierMasterId == courierMasterId);
            }

            return q;

        }

        public IQueryable<DeclarationCourierStatusList> GetVirtual(int tenant)
        {
            IQueryable<DeclarationCourierStatus> DeclarationCourierStatusQuery = (from a in context.DeclarationCourierStatuses
                                                                                  where a.Tenant == tenant
                                                                                  select a);
            IQueryable<DeclarationCourierStatusList> q = GetIqueryableList(DeclarationCourierStatusQuery);
            return q;
        }

        public IQueryable<DeclarationCourierStatusList> GetDeclarationCourierStatusforPendingBulkFeed(QueryOperations queryOperations, int tenant)
        {
            IQueryable<DeclarationCourierStatus> iQueryable = (from a in context.DeclarationCourierStatuses
                                                               where a.Tenant == tenant
                                                               select a);
            string courierMasterId = queryOperations.QueryFilterItems.Where(r => r.FieldName == "CourierMasterId").FirstOrDefault()?.FieldValue.ToString();
            this.isFilter = null;
            var CourierHawbsFromExcel = queryOperations.QueryFilterItems.Where(r => r.FieldName == "CourierHawbsFromExcel").FirstOrDefault();
            if (CourierHawbsFromExcel != null)
            {
                //courierMasterId = "-1";
                this.isFilter = CourierHawbsFromExcel.FieldValue.ToString();
                IQueryable<CourierHawbFromExcel> FromExcelQueryJoin = (from courierhawb in context.CourierHawbFromExcels
                                                                       where courierhawb.CreatedByUser.Id == this.isFilter && courierhawb.NotFound != true
                                                                       select courierhawb);
                iQueryable = iQueryable.Where(d => FromExcelQueryJoin.Select(de => de.DeclarationId).Contains(d.DeclarationId));
            }

            // var qDeclarationPaymentPendingHold =
            //(from p in context.DeclarationPendings
            // where p.Status == "A"
            // group p by p.DeclarationID into g
            // select new MyJoin
            // {
            //     DeclarationId = g.Key,
            //     //ErrorPlace = g.Any(r => r.CourierPendingReason.ErrorPlace == "1"),
            //     CourierPendingReason1stName = g.Any() ? g.FirstOrDefault().CourierPendingReason.LocalName : null,
            // });

            var q1 = (
                from dcs in iQueryable.Include("Declarations").Include("Importer")

                join cd in context.CourierDeclarations on dcs.DeclarationId equals cd.DeclarationId
                join cm in context.CourierMasters on cd.CourierMasterId equals cm.Id


                //join dcs in context.DeclarationCourierStatuses on cd.Declaration.Id equals dcs.DeclarationId

                join dp in context.DeclarationPendings.Include("CourierPendingReason").Where(x => x.Status == "A")
                     on dcs.DeclarationId equals dp.DeclarationID into dpjoin

                //join errorPlace in qDeclarationPaymentPendingHold on dcs.DeclarationId equals errorPlace.DeclarationId into errorPlaceOuterJoin
                from dpj in dpjoin.Take(1).DefaultIfEmpty()

                join cp in context.ConsignmentPackages on cd.Declaration.Id equals cp.DeclarationId into cpjoin
                from cj in cpjoin.Where(t => t.PackageMeasureQualifierCode == "2" && t.GrossMassMeasure.HasValue).DefaultIfEmpty()

                join s in context.SupplierInvoices on cd.Declaration.Id equals s.DeclarationId into sjoin
                from sj in sjoin.Take(1).DefaultIfEmpty()

                group cj by new
                {
                    CourierMasterId = cd.CourierMasterId,
                    DeclarationId = cd.DeclarationId,
                    CourierHawb = cd.Declaration.CourierHAWB,
                    ImporterCode = cd.Declaration.ImporterCode,
                    CasualSupplierName = cd.Declaration.CasualSupplierName,
                    ImporterName = cd.Declaration.ImporterName != null ? cd.Declaration.ImporterName : (cd.Declaration.ImporterId != null ? cd.Declaration.Importer.FullName : cd.Declaration.ImporterName),
                    CargoDescription = cd.Declaration.CargoDescription,
                    CasualSupplierAddress = cd.Declaration.CasualImporterAddress1 + ", " + cd.Declaration.CasualImporterAddress2,
                    CasualImporterCity = cd.Declaration.CasualImporterCity,
                    TotalInvoiceAmountInUSD = dcs.TotalInvoiceAmountInUSD,
                    IncoTermCode = sj != null ? sj.IncotermCode : "",
                    CourierSearchFields = cd.Declaration.CourierSearchFields,
                    FastIndividualProcessCode = dcs.FastIndividualProcessCode,
                    CourierPendingReasonList = dcs.CourierPendingReasonList,
                    CourierPendingReasonName = dpj.CourierPendingReason.LocalName != null ? dpj.CourierPendingReason.LocalName : null,
                    MissedDocumentStatusCode = dcs.MissedDocumentStatusCode,
                    Tenant = dcs.Tenant,

                } into t2
                select new DeclarationCourierStatusList
                {
                    Tenant = t2.Key.Tenant,
                    CourierMasterId = t2.Key.CourierMasterId,
                    DeclarationId = t2.Key.DeclarationId,
                    CourierHawb = t2.Key.CourierHawb,
                    ImporterCode = t2.Key.ImporterCode,
                    ImporterName = t2.Key.ImporterName,
                    CargoDescription = t2.Key.CargoDescription,
                    CasualSupplierAddress = t2.Key.CasualSupplierAddress,
                    CasualImporterCity = t2.Key.CasualImporterCity,
                    TotalInvoiceAmountInUSD = t2.Key.TotalInvoiceAmountInUSD,
                    GrossMassMeasure = t2.Sum(t => t.GrossMassMeasure != null ? t.GrossMassMeasure.Value : 0),
                    IncoTermCode = t2.Key.IncoTermCode,
                    CourierSearchFields = t2.Key.CourierSearchFields,
                    FastIndividualProcessCode = t2.Key.FastIndividualProcessCode,
                    CourierPendingReasonList = t2.Key.CourierPendingReasonList,
                    CourierPendingReasonName = t2.Key.CourierPendingReasonName,
                    MissedDocumentStatusCode = t2.Key.MissedDocumentStatusCode,
                    CasualSupplierName = t2.Key.CasualSupplierName,
                });


            q1 = q1.OrderBy(x => x.DeclarationId).Where(x => x.Tenant == tenant);

            return q1;
        }

        public List<DeclarationCourierStatusList> GetDeclarationCourierStatusListPendingBulk(QueryOperations queryOperations, int tenant)
        {
            int skippedPorts = queryOperations.PageIndex;

            GenericSort sortClass = new GenericSort();

            IQueryable<DeclarationCourierStatusList> query2 = GetDeclarationCourierStatusforPendingBulkQuery(queryOperations, tenant);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(DeclarationCourierStatusList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> DeclarationCourierStatusObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Customs.DeclarationCourierStatus", tenant).ToList();

                ObjectField objectField = (from a in DeclarationCourierStatusObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    if (objectField.IsCustom)
                    {
                        query2 = sortClass.GetSorterQuery<DeclarationCourierStatusList, string>(queryOperations, query2);
                    }
                    else if (queryOperations.SortByColumnName == "ImporterCode")
                        query2 = sortClass.GetSorterQuery<DeclarationCourierStatusList, string>(queryOperations, query2);
                    else
                    {
                        switch (objectField.DataTypeCode.ToLower())
                        {
                            case "ntext":
                            case "text":
                                {
                                    query2 = sortClass.GetSorterQuery<DeclarationCourierStatusList, string>(queryOperations, query2);
                                    break;
                                }
                            case "sigdouble":
                            case "double":
                                {
                                    query2 = sortClass.GetSorterQuery<DeclarationCourierStatusList, double>(queryOperations, query2);
                                    break;
                                }
                            case "date":
                            case "datetime":
                                {
                                    query2 = sortClass.GetSorterQuery<DeclarationCourierStatusList, DateTime>(queryOperations, query2);
                                    break;
                                }
                            case "unsinteger":
                            case "integer":
                                {
                                    query2 = sortClass.GetSorterQuery<DeclarationCourierStatusList, int>(queryOperations, query2);
                                    break;
                                }
                            case "boolean":
                                {
                                    query2 = sortClass.GetSorterQuery<DeclarationCourierStatusList, bool>(queryOperations, query2);
                                    break;
                                }
                            case "unsdecimal":
                            case "decimal":
                                {
                                    query2 = sortClass.GetSorterQuery<DeclarationCourierStatusList, decimal>(queryOperations, query2);
                                    break;
                                }
                            default:
                                {
                                    query2 = query2.OrderByDescending(d => d.CourierHawb);
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.CourierHawb);
            }
            if (!queryOperations.GetAll)
            {
                query2 = query2.Skip(skippedPorts);
                query2 = query2.Take(queryOperations.PageSize);
            }
            return query2.ToList();
        }

        public int GetDeclarationCourierStatusforPendingBulkFeedListCount(QueryOperations queryOperations, int tenant)
        {
            IQueryable<DeclarationCourierStatusList> query2 = GetDeclarationCourierStatusforPendingBulkQuery(queryOperations, tenant);
            int count = query2.ToList().Count();
            return count;
        }

        private IQueryable<DeclarationCourierStatusList> GetDeclarationCourierStatusforPendingBulkQuery(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();

            IQueryable<DeclarationCourierStatus> iQueryable = (from a in context.DeclarationCourierStatuses
                                                               where a.Tenant == tenant
                                                               select a);

            iQueryable = ApplyCustomFilters(queryOperations, iQueryable, tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            iQueryable = filter.GetFilteredQuery<DeclarationCourierStatus>(nonListQueryOperation, iQueryable);

            IQueryable<DeclarationCourierStatusList> query2 = GetDeclarationCourierStatusforPendingBulkFeed(queryOperations, tenant);

            var cargoDescriptionF = queryOperations.QueryFilterItems.Where(r => r.FieldName == "CargoDescription").FirstOrDefault();
            if (cargoDescriptionF != null && !string.IsNullOrEmpty(cargoDescriptionF.FieldValue?.ToString()))
            {
                string description = cargoDescriptionF.FieldValue.ToString().ToLower();
                query2 = query2.Where(x => x.CargoDescription.ToLower().Contains(description));
            }

            var casualSupplierNameF = queryOperations.QueryFilterItems.Where(r => r.FieldName == "CasualSupplierName").FirstOrDefault();
            if (casualSupplierNameF != null && !string.IsNullOrEmpty(casualSupplierNameF.FieldValue?.ToString()))
            {
                string casualSupplierName = casualSupplierNameF.FieldValue.ToString().ToLower();
                query2 = query2.Where(x => x.CasualSupplierName.ToLower().Contains(casualSupplierName));
            }
            var importerNameF = queryOperations.QueryFilterItems.Where(r => r.FieldName == "ImporterName").FirstOrDefault();
            if (importerNameF != null && !string.IsNullOrEmpty(importerNameF.FieldValue?.ToString()))
            {
                string importerName = importerNameF.FieldValue.ToString().ToLower();
                query2 = query2.Where(x => x.ImporterName.ToLower().Contains(importerName));
            }
            query2 = filter.GetFilteredQuery<DeclarationCourierStatusList>(listQueryOperation, query2);
            return query2;
        }

        public int GetCountGroupByStorageCode(int tenant, string courierMasterId = null, List<string> declarationsList = null)
        {
            IQueryable<GetStorageSiteByIdResult> query;

            if (declarationsList != null && declarationsList.Count > 0)
            {
                query = (from dcs in context.DeclarationCourierStatuses
                         join c in context.Consignments on dcs.DeclarationId equals c.DeclarationId
                         where declarationsList.Contains(dcs.DeclarationId)
                         select new GetStorageSiteByIdResult { Tenant = dcs.Tenant, CourierPaymentStatusCode = dcs.CourierPaymentStatusCode, StorageSiteCode = c.StorageSiteCode });
            }
            else
            {
                query = (from cd in context.CourierDeclarations
                         join dcs in context.DeclarationCourierStatuses on cd.DeclarationId equals dcs.DeclarationId
                         join c in context.Consignments on cd.DeclarationId equals c.DeclarationId
                         where cd.CourierMasterId == courierMasterId
                         select new GetStorageSiteByIdResult { Tenant = cd.Tenant, CourierPaymentStatusCode = dcs.CourierPaymentStatusCode, StorageSiteCode = c.StorageSiteCode });
            }

            // get data for specific tenant and only for courier that are ready for payments and group it by the storage site
            var querynew = query.Where(a => a.Tenant == tenant && a.CourierPaymentStatusCode == "R")
                .GroupBy(a => a.StorageSiteCode)
                .Select(group => new { StorageSite = group.Key, Total = group.Count() });

            var response = querynew.ToList();

            // return the count of storage sites for the courier ID / declaration IDs
            return response.Count;
        }

        public IQueryable<DeclarationCourierStatusList> GetDeclarationStatusesByFromExcel(int tenant, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return Enumerable.Empty<DeclarationCourierStatusList>().AsQueryable();
            }

            var courierHawbFromExcelRepository = new CourierHawbFromExcelRepository(this.context);

            var query = from dec in courierHawbFromExcelRepository.GetAllByUser(tenant, userId)
                        join rDec in context.Declarations on dec.DeclarationId equals rDec.Id
                        join dcs in context.DeclarationCourierStatuses on dec.DeclarationId equals dcs.DeclarationId
                        where dcs.Tenant == tenant 
                        && !string.IsNullOrEmpty(rDec.DeclarationNumber)
                        select new DeclarationCourierStatusList
                        {
                            CourierDeclarationStatusCode = dcs.CourierDeclarationStatusCode,
                            DeclarationId = dcs.DeclarationId,
                        };

            return query;
        }
        public IQueryable<DeclarationCourierStatusList> GetDeclarationStatusesByCourierMaster(int tenant, string courierMasterId)
        {
            var query = (from dcs in context.DeclarationCourierStatuses
                        join cd in context.CourierDeclarations on dcs.DeclarationId equals cd.DeclarationId
                        join cm in context.CourierMasters on cd.CourierMasterId equals cm.Id
                        join dec in context.Declarations on dcs.DeclarationId equals dec.Id
                        where dcs.Tenant == tenant
                              && cm.Id == courierMasterId
                              && !string.IsNullOrEmpty(dec.DeclarationNumber)

                        select new DeclarationCourierStatusList
                        {
                            CourierDeclarationStatusCode = dcs.CourierDeclarationStatusCode,
                            DeclarationId = dcs.DeclarationId,
                        });
            return query;
        }

        public IQueryable<DeclarationCourierStatusList> GetDeclarationsByIds(List<string> declarationIds, int tenant)
        {
            var query = (from dcs in context.DeclarationCourierStatuses
                         join dec in context.Declarations on dcs.DeclarationId equals dec.Id
                         where dcs.Tenant == tenant
                         && declarationIds.Contains(dcs.DeclarationId)
                         && !string.IsNullOrEmpty(dec.DeclarationNumber)
                         select new DeclarationCourierStatusList
                         {
                             DeclarationId = dcs.DeclarationId,
                             CourierDeclarationStatusCode = dcs.CourierDeclarationStatusCode,
                             DeclarationNumber = dec.DeclarationNumber,
                             CustomFileNo = dec.CustomFileNo,
                             Tenant = tenant,
                         });
            return query;
        }


    }

    public class GetStorageSiteByIdResult
    {
        public int Tenant { get; set; }
        public string CourierPaymentStatusCode { get; set; }
        public string StorageSiteCode { get; set; }
    }

    public class MyJoin
    {
        public bool ErrorPlace { get; set; }
        public string CourierPendingReason1stName { get; set; }
        public string CourierPendingReasonNameList { get; set; }
        internal string DeclarationId { get; set; }
    }

    public class FromExcelJoin
    {
        internal string DeclarationId { get; set; }
    }
}
