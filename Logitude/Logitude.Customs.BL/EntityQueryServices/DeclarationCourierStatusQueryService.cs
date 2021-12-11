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

#if false
          public List<DeclarationCourierStatusPM> GetByMasterIDCourierDeclarationStatusCode(
            int tenant, string CourierMasterId, string CourierDeclarationStatusCode,
            string SelectedBOLValue,
            string SelectedStatusValue,
            string SelectedTotalInvoiceValue,
            string SelectedFastIndividualProcessValue,
            string SelectedCustomStatusValue
            )
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);

            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in repository.GetAll(tenant).Where(r => r.CourierDeclarationStatusCode == CourierDeclarationStatusCode)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);

            q = repository.MoreFilter(SelectedBOLValue, SelectedStatusValue, SelectedTotalInvoiceValue, SelectedFastIndividualProcessValue, SelectedCustomStatusValue, q);

            var pocos = q.ToList();
            return pocos.Select(r => this.GetEntityPM(r)).ToList();


        }


        private static IQueryable<DeclarationCourierStatus> MoreFilter(string SelectedBOLValue, string SelectedStatusValue, string SelectedTotalInvoiceValue, string SelectedFastIndividualProcessValue, string SelectedCustomStatusValue, IQueryable<DeclarationCourierStatus> q)
        {
            switch (SelectedBOLValue)
            {
                case "L":
                case "H":
                    {
                        q = q.Where(r => r.HighLowValue == SelectedBOLValue);
                        break;
                    }
            }
            switch (SelectedStatusValue)
            {
                case "O":
                    {
                        q = q.Where(r => r.IsClosedForFollowUp == false);

                        break;
                    }
                case "C":
                    {
                        q = q.Where(r => r.IsClosedForFollowUp == true);
                        break;
                    }
            }

            switch (SelectedTotalInvoiceValue)
            {
                case "75":
                    {
                        q = q.Where(r => r.TotalInvoiceAmountInUSD <= 75);

                        break;
                    }
                case "500":
                    {
                        q = q.Where(r => r.TotalInvoiceAmountInUSD > 75 && r.TotalInvoiceAmountInUSD <= 500);
                        break;
                    }
                case "1000":
                    {
                        q = q.Where(r => r.TotalInvoiceAmountInUSD > 500 && r.TotalInvoiceAmountInUSD <= 1000);

                        break;
                    }
            }

            switch (SelectedFastIndividualProcessValue)
            {
                case "F":
                case "I":
                    {
                        q = q.Where(r => r.FastIndividualProcessCode == SelectedFastIndividualProcessValue);
                        break;
                    }
            }

            switch (SelectedCustomStatusValue)
            {
                case "H":
                    {
                        q = q.Where(r => r.Declaration.CourierCustomStatusCode == "1");

                        break;
                    }
                case "S":
                    {
                        q = q.Where(r => r.Declaration.CourierCustomStatusCode == "2");
                        break;
                    }
            }
            return q;
        }



         public List<DeclarationCourierStatusPM> GetByMasterIDCourierManifestStatusCode(int tenant, string CourierMasterId, string CourierManifestStatusCode
            , string SelectedBOLValue, string SelectedStatusValue, string SelectedTotalInvoiceValue, string SelectedFastIndividualProcessValue, string SelectedCustomStatusValue
            )
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);
            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in repository.GetAll(tenant).Where(r => r.CourierManifestStatusCode == CourierManifestStatusCode)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);
            q = MoreFilter(SelectedBOLValue, SelectedStatusValue, SelectedTotalInvoiceValue, SelectedFastIndividualProcessValue, SelectedCustomStatusValue , q);
            var pocos = q.ToList();
            return pocos.Select(r => this.GetEntityPM(r)).ToList();


        }
         public List<DeclarationCourierStatusPM> GetByMasterIDCourierDocumentStatus(int tenant, string CourierMasterId, string DocumentStatusCode
            , string SelectedBOLValue, string SelectedStatusValue, string SelectedTotalInvoiceValue, string SelectedFastIndividualProcessValue, string SelectedCustomStatusValue)
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);
            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in repository.GetAll(tenant).Where(r => r.DocumentStatusCode == DocumentStatusCode)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);
            q = repository.MoreFilter(SelectedBOLValue, SelectedStatusValue, SelectedTotalInvoiceValue, SelectedFastIndividualProcessValue, SelectedCustomStatusValue, q);
            var pocos = q.ToList();
            return pocos.Select(r => this.GetEntityPM(r)).ToList();


        }



             public List<DeclarationCourierStatusPM> GetByMasterIDCourierPaymentStatusCode(int tenant, string CourierMasterId, 
                 string CourierPaymentStatusCode,string HighLowValue)
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);
            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in repository.GetAll(tenant)
                     .Where(r => r.CourierPaymentStatusCode == CourierPaymentStatusCode)
                     .Where(r => r.HighLowValue == HighLowValue)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);
            var pocos = q.ToList();
            return pocos.Select(r => this.GetEntityPM(r)).ToList();


        }

        public List<DeclarationCourierStatusPM> GetByMasterIDDeclarationCourierStatus(int tenant, string CourierMasterId)
        {
            IQueryable<DeclarationCourierStatus> q = GetBy(tenant, CourierMasterId);
            var pocos = q.ToList();
            return pocos.Select(r => this.GetEntityPM(r)).ToList();
        }
#endif




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



        public List<string> GetByMasterID_DeclarationIdList(int tenant, string CourierMasterId)
        {
            //List<string> declarationIdList = new List<string>();
            IQueryable<DeclarationCourierStatus> q = GetBy(tenant, CourierMasterId);
            return q.Select(r => r.DeclarationId).ToList();
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


        public DeclarationCourierStatusSummary GetQueriesCounts(int tenant)
        {
            DeclarationCourierStatusSummary declarationCourierStatusSummary = new DeclarationCourierStatusSummary();
            IQueryable<DeclarationCourierStatus> declarationCourierStatuses = (from dc in context.DeclarationCourierStatuses.Include("Declaration")
                                                                            //  join d in context.CourierDeclarations on dc.DeclarationId equals d.DeclarationId
                                                                             //  join dm in context.CourierMasters
                                                                             // on d.CourierMasterId equals dm.Id
                                                                               where dc.Tenant == tenant && dc.Declaration.IsCourierDeclaration == true
                                                                               select dc);


            var counters = (from a in declarationCourierStatuses

                            group a by 1 into groupBy1
                            select new DeclarationCourierStatusSummary
                            {

                                OpenCourierMasterCount = declarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false),
                                UnReleasedFastProcessCount = declarationCourierStatuses.Count(x => x.FastIndividualProcessCode == "F" && (x.Declaration.HatraDate == null || x.Declaration.HatraDate == DateTime.MinValue)),
                                WithoutIdCount = declarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false && x.CourierPendingReasonList.Contains("902")),
                                WithoutClassificationCount = declarationCourierStatuses.Count(x => x.IsCourierMissingClassification == true),
                                PendingPaymentCount = declarationCourierStatuses.Count(x => x.CourierPendingReasonList.Contains("900")),
                                PendingCustomsCount = declarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false && x.Declaration.CourierCustomStatusCode == "2"),
                                PendingCount = declarationCourierStatuses.Count(x => !string.IsNullOrEmpty(x.CourierPendingReasonList)),
                                AllCourierDeclarationsCount = declarationCourierStatuses.Count(),
                                CourierMasterOpenIndividualCount = declarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false && x.FastIndividualProcessCode == "I"),
                                UnReleasedIndividualCount = declarationCourierStatuses.Count(x => x.FastIndividualProcessCode == "I" && (x.Declaration.HatraDate == null || x.Declaration.HatraDate == DateTime.MinValue))
                            }); ;


      declarationCourierStatusSummary= counters.FirstOrDefault();

                        



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

    }
}
