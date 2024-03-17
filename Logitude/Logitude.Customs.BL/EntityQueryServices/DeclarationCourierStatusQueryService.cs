using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.DataContracts;
using System.Data.Entity;
using System.Diagnostics;
namespace Logitude.Customs.BL.EntityQueryServices
{

    public partial class DeclarationCourierStatusQueryService : EntityQueryService<DeclarationCourierStatus, DeclarationCourierStatusKeys, DeclarationCourierStatusPM, object, DeclarationCourierStatusKeys>
    {


        public override void GetComposition(EntityKeyFields entityKeys, DeclarationCourierStatusPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            DeclarationCourierStatusKeys DeclarationCourierStatusKeys = entityKeys as DeclarationCourierStatusKeys;
            DeclarationPendingQueryService declarationPendingQueryService = new DeclarationPendingQueryService(context);

            entityPM.DeclarationPendings = declarationPendingQueryService.GetMulti(DeclarationCourierStatusKeys, true);

            base.GetComposition(entityKeys, entityPM);
        }




        public List<KeyValuePair<string, string>> GetByMasterIDStorageSiteCode(int tenant, string CourierMasterId,
         List<string> storageSiteCodeList)
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDecConsignment = new ConsignmentRepository(this.context);
            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDecConsignment in repoDecConsignment.GetAll(tenant).Where(r => storageSiteCodeList.Contains(r.StorageSiteCode))
                     on dec.DeclarationId equals rDecConsignment.DeclarationId
                     select new { rDecConsignment.StorageSiteCode, rDecConsignment.DeclarationId });
            var anyList = q.ToList();
            var res = new List<KeyValuePair<string, string>>();
            res = anyList.Select(r => new KeyValuePair<string, string>(r.DeclarationId, r.StorageSiteCode)).ToList();
            return res;
        }
        public List<KeyValuePair<string, string>> GetFromExcelStorageSiteCode(int tenant, string userId,
        List<string> storageSiteCodeList)
        {
            var courierHawbFromExcelRepository = new CourierHawbFromExcelRepository(this.context);
            var repoDecConsignment = new ConsignmentRepository(this.context);
            var q = (from dec in courierHawbFromExcelRepository.GetAllByUser(tenant, userId)
                     join rDecConsignment in repoDecConsignment.GetAll(tenant).Where(r => storageSiteCodeList.Contains(r.StorageSiteCode))
                     on dec.DeclarationId equals rDecConsignment.DeclarationId
                     select new { rDecConsignment.StorageSiteCode, rDecConsignment.DeclarationId });
            var anyList = q.ToList();
            var res = new List<KeyValuePair<string, string>>();
            res = anyList.Select(r => new KeyValuePair<string, string>(r.DeclarationId, r.StorageSiteCode)).ToList();
            return res;
        }



        public List<string> GetByMasterID_DeclarationIdList(int tenant, string CourierMasterId, bool IsWorkSheetFromExcel=false,string userId=null)
        {
            var declarationCourierStatusRepository = new DeclarationCourierStatusRepository(context);

            IQueryable<DeclarationCourierStatus> q = null;

            if (IsWorkSheetFromExcel)
            {
                q = declarationCourierStatusRepository.GetByFromExcel(tenant, userId);
            }
            else
            {
                q = declarationCourierStatusRepository.GetBy(tenant, CourierMasterId);
            }


            //List<string> declarationIdList = new List<string>();
            // IQueryable<DeclarationCourierStatus> q = GetBy(tenant, CourierMasterId);
            return q.Select(r => r.DeclarationId).ToList();
        }
        public List<DeclarationDataForSlaReport> GetDeclarationDataForSlaReportByMasterId(int tenant, string CourierMasterId)
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);

            return  (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                    join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                    join status in repository.GetAll(tenant)
                    on dec.DeclarationId equals status.DeclarationId
                    select new DeclarationDataForSlaReport
                    {
                         CourierHawb=rDec.CourierHAWB,
                         Delivered=status.Delivered,
                         HatraDate=rDec.HatraDate,
                         LastMileStatusDate=status.LastMileStatusDate,
                         TerminalReleaseDate=status.TerminalReleaseDate,
                    }).ToList();
        }

        public List<DeclarationCourierStatusPM> GetByDeclarationIdList(int tenant, List<string> declarationIdList)
        {
            var q = repository.GetAll(tenant).Where(r => declarationIdList.Contains(r.DeclarationId));
            var pocos = q.ToList();
            return pocos.Select(r => this.GetEntityPM(r)).ToList();

        }

        public IQueryable<DeclarationCourierStatus> GetBy(int tenant, string CourierMasterId)
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);

            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in repository.GetAll(tenant)
                     on dec.DeclarationId equals status.DeclarationId
                     select status);
            return q;
        }

        public List<DeclarationCourierStatusPM> GetDeclarationsByIds(List<string> ids, int tenant)
        {
            var repository = new DeclarationCourierStatusRepository(this.context);
            List<DeclarationCourierStatus> declarations = repository.GetDeclarationsByIds(ids, tenant);
            DeclarationCourierStatusDataMapping mappings = new DeclarationCourierStatusDataMapping();
            List<DeclarationCourierStatusPM> declarationPMs = new List<DeclarationCourierStatusPM>();
            foreach (DeclarationCourierStatus declarationItem in declarations)
            {
                DeclarationCourierStatusPM declarationCourierStatusPM = new DeclarationCourierStatusPM();
                mappings.CustomPOCOToPM(declarationCourierStatusPM, declarationItem);
                mappings.POCOToPM(declarationCourierStatusPM, declarationItem);
                GetComposition(new DeclarationKeys() { Id = declarationItem.DeclarationId, }, declarationCourierStatusPM);
                declarationPMs.Add(declarationCourierStatusPM);
            }
            return declarationPMs;
        }



        public List<DeclarationCourierPendingTabRecord> GetWorkSpacePendingTab(int tenant, string IntegratorId)
        {
            IQueryable<DeclarationCourierStatus> qDeclarationCourierStatuses = GetQueryableDeclarationCourierStatuses(tenant, IntegratorId);
            var qDeclarationIDCourierPendingReasonCode = (from dcs in qDeclarationCourierStatuses
                                                          join dp in context.DeclarationPendings on dcs.DeclarationId equals dp.DeclarationID

                                                          where dp.Status == "A" // Active
                                                          select new { dp.CourierPendingReasonCode, dp.DeclarationID }
                    );
            var mygQ = (from DeclarationIDCourierPendingReasonCode in qDeclarationIDCourierPendingReasonCode
                            //CourierPendingReasonName = cpr.LocalName ?? cpr.EnglishName,
                        group DeclarationIDCourierPendingReasonCode by DeclarationIDCourierPendingReasonCode.CourierPendingReasonCode into CourierPendingReasonCodeGroupByCod
                        select new { Code = CourierPendingReasonCodeGroupByCod.Key, Count = CourierPendingReasonCodeGroupByCod.Count() }
                     );
            bool test55 = false;
            if (test55)
            {

            }
            var mygQWithLocalName = (
                from gb in mygQ
                join cpr in context.CourierPendingReasons on gb.Code equals cpr.Code
                select new DeclarationCourierPendingTabRecord { Code = gb.Code, Name = cpr.LocalName, Count = gb.Count }
                );
            var res = mygQWithLocalName.ToList().OrderByDescending(r => r.Count).ToList();
            return res;


        }
        public DeclarationCourierStatusSummary GetQueriesCounts_Clasic(int tenant, string IntegratorId)
        {
            DeclarationCourierStatusSummary declarationCourierStatusSummary = new DeclarationCourierStatusSummary();
            IQueryable<DeclarationCourierStatus> qDeclarationCourierStatuses = GetQueryableDeclarationCourierStatuses(tenant, IntegratorId);

            var counters = (from a in qDeclarationCourierStatuses

                            group a by 1 into groupBy1
                            select new DeclarationCourierStatusSummary
                            {

                                OpenCourierMasterCount = qDeclarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false),
                                UnReleasedFastProcessCount = qDeclarationCourierStatuses.Count(x => x.FastIndividualProcessCode == "F" && (x.Declaration.HatraDate == null || x.Declaration.HatraDate == DateTime.MinValue)),
                                WithoutIdCount = qDeclarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false && x.CourierPendingReasonList.Contains("902")),
                                WithoutClassificationCount = qDeclarationCourierStatuses.Count(x => x.IsCourierMissingClassification == true),
                                PendingPaymentCount = qDeclarationCourierStatuses.Count(x => x.CourierPendingReasonList.Contains("900")),
                                PendingCustomsCount = qDeclarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false && x.Declaration.CourierCustomStatusCode == "2"),
                                PendingCount = qDeclarationCourierStatuses.Count(x => !string.IsNullOrEmpty(x.CourierPendingReasonList)),
                                AllCourierDeclarationsCount = qDeclarationCourierStatuses.Count(),
                                CourierMasterOpenIndividualCount = qDeclarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false && x.FastIndividualProcessCode == "I"),
                                UnReleasedIndividualCount = qDeclarationCourierStatuses.Count(x => x.FastIndividualProcessCode == "I" && (x.Declaration.HatraDate == null || x.Declaration.HatraDate == DateTime.MinValue))
                            }); ;


            declarationCourierStatusSummary = counters.FirstOrDefault();





            //declarationCourierStatusSummary.OpenCourierMasterCount = declarationCourierStatuses.Where(x => x.IsClosedForFollowUp == false).Count();
            //declarationCourierStatusSummary.UnReleasedFastProcessCount = declarationCourierStatuses.Where(x => x.FastIndividualProcessCode == "F" && (x.Declaration.HatraDate == null || x.Declaration.HatraDate == DateTime.MinValue)).Count();
            //declarationCourierStatusSummary.WithoutIdCount = declarationCourierStatuses.Where(x => x.IsClosedForFollowUp == false && x.CourierPendingReasonList.Contains("902")).Count();
            //declarationCourierStatusSummary.WithoutClassificationCount = declarationCourierStatuses.Where(x => x.IsCourierMissingClassification == true).Count();
            //declarationCourierStatusSummary.PendingPaymentCount = declarationCourierStatuses.Where(x => x.CourierPendingReasonList.Contains("900")).Count();
            //declarationCourierStatusSummary.PendingCustomsCount = declarationCourierStatuses.Where(x => x.IsClosedForFollowUp == false && x.Declaration.CourierCustomStatusCode == "2").Count();
            //declarationCourierStatusSummary.PendingCount = declarationCourierStatuses.Where(x => !string.IsNullOrEmpty(x.CourierPendingReasonList)).Count();
            //declarationCourierStatusSummary.AllCourierDeclarationsCount = declarationCourierStatuses.Count();

            //declarationCourierStatusSummary.CourierMasterOpenIndividualCount = declarationCourierStatuses.Where(x => x.IsClosedForFollowUp == false && x.FastIndividualProcessCode == "I").Count();
            //declarationCourierStatusSummary.UnReleasedIndividualCount = declarationCourierStatuses.Where(x => x.FastIndividualProcessCode == "I" && (x.Declaration.HatraDate == null || x.Declaration.HatraDate == DateTime.MinValue)).Count();

            return declarationCourierStatusSummary;

        }

        private  IQueryable<DeclarationCourierStatus> GetQueryableDeclarationCourierStatuses(int tenant, string IntegratorId, ICustomContext mycontext=null)
        {

            var curcontext=mycontext ?? context;
            IQueryable<DeclarationCourierStatus> declarationCourierStatuses = (from dc in curcontext.DeclarationCourierStatuses select dc);
            if (string.IsNullOrWhiteSpace(IntegratorId) || IntegratorId == "null")
            {
                declarationCourierStatuses = (from dcs in curcontext.DeclarationCourierStatuses//.Include("Declaration")
                                                                                               //  join d in context.CourierDeclarations on dc.DeclarationId equals d.DeclarationId
                                                                                               //  join dm in context.CourierMasters
                                                                                               // on d.CourierMasterId equals dm.Id
                                              join d in curcontext.Declarations on dcs.DeclarationId equals d.Id
                                              where dcs.Tenant == tenant

                                                                                    //&& dc.Declaration.IsCourierDeclaration == true 
                                                                                    && d.IsCourierDeclaration == true && d.AmendmentDontDisplayInList == false && d.IsCancelled != true
                                                                                    && dcs.IsClosedForFollowUp != true


                                              select dcs);
            }
            else
            {
                declarationCourierStatuses = (from dcs in curcontext.DeclarationCourierStatuses//.Include("Declaration")

                                              join d in curcontext.Declarations on dcs.DeclarationId equals d.Id

                                              join cd in curcontext.CourierDeclarations on dcs.DeclarationId equals cd.DeclarationId
                                              join dm in curcontext.CourierMasters
                                              on cd.CourierMasterId equals dm.Id
                                              where dcs.Tenant == tenant
                                              //&& dcs.Declaration.IsCourierDeclaration == true 
                                              && dm.IntegratorCode == IntegratorId

                                              && d.IsCourierDeclaration == true && d.AmendmentDontDisplayInList == false && d.IsCancelled != true
                                              && dcs.IsClosedForFollowUp != true

                                              select dcs);
            }

            return declarationCourierStatuses;
        }


        private IQueryable<DeclarationCourierStatusDeclaration> GetQueryableDeclarationCourierStatusesWithDeclaration(int tenant, string IntegratorId, ICustomContext mycontext = null)
        {
            var curcontext = mycontext ?? context;
            IQueryable<DeclarationCourierStatus> declarationCourierStatuses = (from dc in curcontext.DeclarationCourierStatuses select dc);

            IQueryable<DeclarationCourierStatusDeclaration> query;

            if (string.IsNullOrWhiteSpace(IntegratorId) || IntegratorId == "null")
            {
                query = (from dcs in curcontext.DeclarationCourierStatuses
                            join d in curcontext.Declarations on dcs.DeclarationId equals d.Id
                            where d.Tenant == tenant
                                // && d.IsCourierDeclaration == true 
                                && d.AmendmentDontDisplayInList == false && d.IsCancelled != true
                                && dcs.IsClosedForFollowUp == false
                            select new DeclarationCourierStatusDeclaration { DeclarationCourierStatus = dcs, Declaration = d});
            }
            else
            {
                query = (from dcs in curcontext.DeclarationCourierStatuses//.Include("Declaration")

                            join d in curcontext.Declarations on dcs.DeclarationId equals d.Id

                            join cd in curcontext.CourierDeclarations on dcs.DeclarationId equals cd.DeclarationId
                            join dm in curcontext.CourierMasters
                            on cd.CourierMasterId equals dm.Id
                            where dcs.Tenant == tenant
                            && dm.IntegratorCode == IntegratorId
                            // && d.IsCourierDeclaration == true 
                            && d.AmendmentDontDisplayInList == false && d.IsCancelled != true
                            && dcs.IsClosedForFollowUp == false

                            select new DeclarationCourierStatusDeclaration  { DeclarationCourierStatus = dcs, Declaration = d });
            }
            return query;
        }


        public DeclarationCourierStatusSummary GetQueriesCountsMulti(int tenant, string integratorId)
        {
            DeclarationCourierStatusSummary declarationCourierStatusSummary = new DeclarationCourierStatusSummary();
            //IQueryable<DeclarationCourierStatusCountDTO> query = GetIQueryableDeclarationCourierStatusCountDTO(tenant, integratorId);

            var my = new DeclarationCourierStatusSummary();
            var listOfTast = new List<Task>();
            var stopwatch = Stopwatch.StartNew();

            var tUnReleasedFastProcessCount = Task.Run(() =>
            {
                stopwatch = Stopwatch.StartNew();
                my.UnReleasedFastProcessCount = GetIQueryableDeclarationCourierStatusCountDTO(tenant, integratorId).Where(x => x.FastIndividualProcessCode == "F" && x.DeclarationHatraDate == null).Count();
                my.TookUnReleasedFastProcessCount = stopwatch.ElapsedMilliseconds;
            });
            listOfTast.Add(tUnReleasedFastProcessCount);

            var tPendingCount = Task.Run(() =>
            {
                stopwatch = Stopwatch.StartNew();
                var query = GetIQueryableDeclarationCourierStatusCountDTO(tenant, integratorId).Where(x => x.CourierPendingReasonList != null);
                var response = (from a in query
                                group a by 1 into g
                                select new
                                {
                                    PendingCount = g.Count(x => !string.IsNullOrEmpty(x.CourierPendingReasonList)),
                                    PendingPaymentCount = g.Count(x => x.CourierPendingReasonList.Contains("900")),
                                    WithoutIdCount = g.Count(x => x.CourierPendingReasonList.Contains("902"))
                                }).Single();
                my.TookPendingCount = stopwatch.ElapsedMilliseconds;

                my.PendingCount = response.PendingCount;
                my.WithoutIdCount = response.WithoutIdCount;
                my.PendingPaymentCount = response.PendingPaymentCount;
            });
            listOfTast.Add(tPendingCount);

            var tWithoutClassificationCount = Task.Run(() =>
            {
                stopwatch = Stopwatch.StartNew();
                my.WithoutClassificationCount = GetIQueryableDeclarationCourierStatusCountDTO(tenant, integratorId).Where(x => x.IsCourierMissingClassification == true).Count();
                my.TookWithoutClassificationCount = stopwatch.ElapsedMilliseconds;
            });
            listOfTast.Add(tWithoutClassificationCount);

            var tPendingCustomsCount = Task.Run(() =>
            {
                stopwatch = Stopwatch.StartNew();
                var query = GetIQueryableDeclarationCourierStatusCountDTO(tenant, integratorId);
                var queryWithGroup = (from a in query
                              group a by a.DeclarationCourierCustomStatusCode into g
                              select new
                              {
                                  DeclarationCourierCustomStatusCode = g.Key,
                                  Count = g.Count(),
                              });
                var response = queryWithGroup.ToList();
                my.TookPendingCustomsCount = stopwatch.ElapsedMilliseconds;

                var total = 0;
                foreach(var result in response)
                {
                    if (result.DeclarationCourierCustomStatusCode == "2")
                    {
                        my.PendingCustomsCount = result.Count;
                    }
                    total += result.Count;
                }
                my.OpenCourierMasterCount = total;
            });
            listOfTast.Add(tPendingCustomsCount);

            //    var tAllCourierDeclarationsCount = Task.Run(() =>
            //    {
            //        my.AllCourierDeclarationsCount = GetIQueryableDeclarationCourierStatusCountDTO(tenant).Count();
            //        my.TookAllCourierDeclarationsCount = stopwatch.ElapsedMilliseconds;
            //    }
            //);
            //    listOfTast.Add(tAllCourierDeclarationsCount);

            var tCourierMasterOpenIndividualCount = Task.Run(() =>
            {
                stopwatch = Stopwatch.StartNew();
                my.CourierMasterOpenIndividualCount = GetIQueryableDeclarationCourierStatusCountDTO(tenant, integratorId).Where(x => x.FastIndividualProcessCode == "I").Count();
                my.TookCourierMasterOpenIndividualCount = stopwatch.ElapsedMilliseconds;
            });
            listOfTast.Add(tCourierMasterOpenIndividualCount);

            var tUnReleasedIndividualCount = Task.Run(() =>
            {
                stopwatch = Stopwatch.StartNew();
                my.UnReleasedIndividualCount = GetIQueryableDeclarationCourierStatusCountDTO(tenant, integratorId).Where(x => x.FastIndividualProcessCode == "I" && x.DeclarationHatraDate == null).Count();
                my.TookUnReleasedIndividualCount = stopwatch.ElapsedMilliseconds;
            });
            listOfTast.Add(tUnReleasedIndividualCount);

            Task.WaitAll(listOfTast.ToArray());

            //      var counters = (from a in declarationCourierStatuses

            //                      group a by 1 into groupBy1
            //                      select new DeclarationCourierStatusSummary
            //                      {

            //                          OpenCourierMasterCount = declarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false),
            //                          UnReleasedFastProcessCount = declarationCourierStatuses.Count(x => x.FastIndividualProcessCode == "F" && (x.DeclarationHatraDate == null || x.DeclarationHatraDate == DateTime.MinValue)),

            //                          WithoutIdCount = declarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false && x.CourierPendingReasonList.Contains("902")),
            //                          PendingPaymentCount = declarationCourierStatuses.Count(x => x.CourierPendingReasonList.Contains("900")),
            //                          PendingCount = declarationCourierStatuses.Count(x => !string.IsNullOrEmpty(x.CourierPendingReasonList)),


            //                          WithoutClassificationCount = declarationCourierStatuses.Count(x => x.IsCourierMissingClassification == true),
            //                          PendingCustomsCount = declarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false && x.DeclarationCourierCustomStatusCode == "2"),
            //                          AllCourierDeclarationsCount = declarationCourierStatuses.Count(),
            //                          CourierMasterOpenIndividualCount = declarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false && x.FastIndividualProcessCode == "I"),
            //                          UnReleasedIndividualCount = declarationCourierStatuses.Count(x => x.FastIndividualProcessCode == "I" && (x.DeclarationHatraDate == null || x.DeclarationHatraDate == DateTime.MinValue))
            //                      }); ;


            //declarationCourierStatusSummary= counters.FirstOrDefault();

            declarationCourierStatusSummary = my;





            return declarationCourierStatusSummary;

        }

        private IQueryable<DeclarationCourierStatusCountDTO> GetIQueryableDeclarationCourierStatusCountDTO(int tenant,string IntegratorId)
        {
            var mycontext = CustomContext.GetContext(tenant);
            IQueryable<DeclarationCourierStatusDeclaration> qDeclarationCourierStatuses = GetQueryableDeclarationCourierStatusesWithDeclaration(tenant, IntegratorId, mycontext);

            IQueryable<DeclarationCourierStatusCountDTO> declarationCourierStatuses =
                (from dc in qDeclarationCourierStatuses //mycontext.DeclarationCourierStatuses//.Include("Declaration")

                 //join d in mycontext.Declarations on dc.DeclarationId equals d.Id
                 //  join dm in context.CourierMasters
                 // on d.CourierMasterId equals dm.Id
                 where dc.DeclarationCourierStatus.Tenant == tenant
                 select new DeclarationCourierStatusCountDTO
                 {
                     CourierPendingReasonList = dc.DeclarationCourierStatus.CourierPendingReasonList,
                     FastIndividualProcessCode = dc.DeclarationCourierStatus.FastIndividualProcessCode,
                     IsCourierMissingClassification = dc.DeclarationCourierStatus.IsCourierMissingClassification,
                     DeclarationCourierCustomStatusCode = dc.Declaration.CourierCustomStatusCode,
                     DeclarationHatraDate = dc.Declaration.HatraDate,
                 }
                      );
            return declarationCourierStatuses;
        }

        public DeclarationCourierStatusSummary GetQueriesCounts(int tenant, string IntegratorId, bool multi = true)
        {

            if (multi)
            {
                return GetQueriesCountsMulti(tenant, IntegratorId);
            }
            else
            {
                return GetQueriesCounts_Clasic(tenant, IntegratorId);
            }
        }

        public IQueryable<string> GetQCustomsWithheld(int tenant)
        {
            ICustomContext context = MainContext as CustomContext;

            var q = (from dcs in this.repository.GetAll(tenant)
                  .Where(x => x.IsClosedForFollowUp == false)
                     join d in context.Declarations
                     .Where(x => x.Tenant == tenant
                     && x.AmendmentDontDisplayInList == false
                     && x.IsCancelled == false
                     && x.CourierCustomStatusCode == "2")
                     on dcs.DeclarationId equals d.Id
                     select d.Id);


            return q;

        }
    }

    class DeclarationCourierStatusDeclaration
    {
        public DeclarationCourierStatus DeclarationCourierStatus { get; internal set; }
        public Declaration Declaration { get; internal set; }
    }

    class DeclarationCourierStatusCountDTO
    {
        public bool IsClosedForFollowUp { get; internal set; }
        public string FastIndividualProcessCode { get; internal set; }
        public string CourierPendingReasonList { get; internal set; }
        public bool IsCourierMissingClassification { get; internal set; }
        public DateTime? DeclarationHatraDate { get; internal set; }
        public string DeclarationCourierCustomStatusCode { get; internal set; }

        //public string CourierPendingReasonList { get; set; }
    }

    public class DeclarationDataForSlaReport
    {
        public DateTime? TerminalReleaseDate { get; set; }
        public bool Delivered { get; set; }
        public DateTime? LastMileStatusDate { get; set; }
        public string CourierHawb { get; set; }
        public DateTime? HatraDate { get; set; }
    }
}
