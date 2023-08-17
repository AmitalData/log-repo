
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using System.Data.Entity;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class DeclarationCourierStatusRepository : IRepository<DeclarationCourierStatus>
    {


        public List<DeclarationCourierStatus> GetMulti(EntityKeyFields entityKeys)
        {

            DeclarationKeys declarationKeys = entityKeys as DeclarationKeys;

            return (from a in context.DeclarationCourierStatuses
                    where a.DeclarationId == declarationKeys.Id
                    select a).ToList();
        }
        public List<DeclarationCourierStatus> GetByMasterIDCourierDeclarationStatusCode(
                 int tenant, string CourierMasterId, string CourierDeclarationStatusCode,
                 string SelectedBOLValue,
                 string SelectedStatusValue,
                 string SelectedTotalInvoiceValue,
                 string SelectedFastIndividualProcessValue,
                 string SelectedCustomStatusValue,
                 string SelectedFinalReleaseValue
                 )
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);

            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant).Where(r => r.CourierDeclarationStatusCode == CourierDeclarationStatusCode)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);

            q = MoreFilter(SelectedBOLValue, SelectedStatusValue, SelectedTotalInvoiceValue, SelectedFastIndividualProcessValue, SelectedCustomStatusValue, SelectedFinalReleaseValue, q);

            var pocos = q.ToList();

            return pocos;

        }
        public List<DeclarationCourierStatus> GetFromExcelCourierDeclarationStatusCode(
                 int tenant, string userId, string CourierDeclarationStatusCode,
                 string SelectedBOLValue,
                 string SelectedStatusValue,
                 string SelectedTotalInvoiceValue,
                 string SelectedFastIndividualProcessValue,
                 string SelectedCustomStatusValue,
                 string SelectedFinalReleaseValue
                 )
        {
            var courierHawbFromExcelRepository = new CourierHawbFromExcelRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);

            var q = (from dec in courierHawbFromExcelRepository.GetAllByUser(tenant, userId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant).Where(r => r.CourierDeclarationStatusCode == CourierDeclarationStatusCode)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);

            q = MoreFilter(SelectedBOLValue, SelectedStatusValue, SelectedTotalInvoiceValue, SelectedFastIndividualProcessValue, SelectedCustomStatusValue, SelectedFinalReleaseValue, q);

            var pocos = q.ToList();

            return pocos;

        }
        public List<DeclarationCourierStatus> GetByMasterIDCourierPaymentStatusCode(int tenant, string CourierMasterId,
            string CourierPaymentStatusCode, string HighLowValue)
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);
            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant)
                     .Where(r => r.CourierPaymentStatusCode == CourierPaymentStatusCode)
                     .Where(r => r.HighLowValue == HighLowValue)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);
            var pocos = q.ToList();
            return pocos;


        }
        public List<DeclarationCourierStatus> GetFromExcelCourierPaymentStatusCode(int tenant, string userId,
           string CourierPaymentStatusCode, string HighLowValue)
        {
            CourierHawbFromExcelRepository courierHawbFromExcelRepository = new CourierHawbFromExcelRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);
            var q = (from dec in courierHawbFromExcelRepository.GetAllByUser(tenant, userId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant)
                     .Where(r => r.CourierPaymentStatusCode == CourierPaymentStatusCode)
                     .Where(r => r.HighLowValue == HighLowValue)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);
            var pocos = q.ToList();
            return pocos;


        }
        public List<DeclarationCourierStatus> GetByMasterIDCourierManifestStatusCode(int tenant, string CourierMasterId, string CourierManifestStatusCode
            , string SelectedBOLValue, string SelectedStatusValue, string SelectedTotalInvoiceValue, string SelectedFastIndividualProcessValue, string SelectedCustomStatusValue, string SelectedFinalReleaseValue
            )
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);
            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant).Where(r => r.CourierManifestStatusCode == CourierManifestStatusCode && r.Declaration.HatraDate == null)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);
            q = MoreFilter(SelectedBOLValue, SelectedStatusValue, SelectedTotalInvoiceValue, SelectedFastIndividualProcessValue, SelectedCustomStatusValue, SelectedFinalReleaseValue, q);
            var pocos = q.ToList();
            return pocos;


        }
        public List<DeclarationCourierStatus> GeCourierManifestStatusCodeFromExcel(int tenant,string userId, string CourierManifestStatusCode
            , string SelectedBOLValue, string SelectedStatusValue, string SelectedTotalInvoiceValue, string SelectedFastIndividualProcessValue, string SelectedCustomStatusValue, string SelectedFinalReleaseValue
            )
        {
            var courierHawbFromExcelRepository = new CourierHawbFromExcelRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);
            var q = (from dec in courierHawbFromExcelRepository.GetAllByUser(tenant, userId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant).Where(r => r.CourierManifestStatusCode == CourierManifestStatusCode && r.Declaration.HatraDate == null)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);
            q = MoreFilter(SelectedBOLValue, SelectedStatusValue, SelectedTotalInvoiceValue, SelectedFastIndividualProcessValue, SelectedCustomStatusValue, SelectedFinalReleaseValue, q);
            var pocos = q.ToList();
            return pocos;


        }
        public List<string> GetPendingByMasterID(int tenant, string CourierMasterId,string userId,Boolean IsWorkSheetFromExcel)
        {
            IQueryable<DeclarationCourierStatus> q;
            if (IsWorkSheetFromExcel)
            {
                q = GetByFromExcel(tenant, userId);
            }
            else
            {
                q = GetBy(tenant, CourierMasterId);
            }
            var list = q
                .Where(r => r.CourierPendingReasonList != null && r.CourierPendingReasonList != "")
                .Select(r => r.CourierPendingReasonList)
                .ToList();
            var myList = list
                .Select(p => p.Split(',').ToList()).ToList()
                .SelectMany(l => l)
                .Distinct()
                .ToList();
            return myList;
        }
        public List<string> GetPendingFromExcel(int tenant, string CourierMasterId)
        {
            IQueryable<DeclarationCourierStatus> q = GetBy(tenant, CourierMasterId);
            var list = q
                .Where(r => r.CourierPendingReasonList != null && r.CourierPendingReasonList != "")
                .Select(r => r.CourierPendingReasonList)
                .ToList();
            var myList = list
                .Select(p => p.Split(',').ToList()).ToList()
                .SelectMany(l => l)
                .Distinct()
                .ToList();
            return myList;
        }

        public List<DeclarationCourierStatus> GetByMasterIDDeclarationCourierStatus(int tenant, string CourierMasterId)
        {
            IQueryable<DeclarationCourierStatus> q = GetBy(tenant, CourierMasterId);
            var pocos = q.ToList();
            return pocos;
        }
        public IQueryable<DeclarationCourierStatus> GetBy(int tenant, string CourierMasterId)
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);

            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant)
                     on dec.DeclarationId equals status.DeclarationId
                     select status);
            return q;
        }
        public IQueryable<DeclarationCourierStatus> GetByFromExcel(int tenant, string userId)
        {
            var courierHawbFromExcelRepository = new CourierHawbFromExcelRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);

            var q = (from dec in courierHawbFromExcelRepository.GetAllByUser(tenant, userId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant)
                     on dec.DeclarationId equals status.DeclarationId
                     select status);
            return q;
        }

        public List<string> GetByMasterIDDeclarationList(int tenant, string CourierMasterId, out string MAWB)
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoCourierMaster = new CourierMasterRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);

            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant) on dec.DeclarationId equals status.DeclarationId
                     where rDec.DeclarationNumber != null
                     select status.DeclarationId);
            MAWB = repoCourierMaster.GetSingle(CourierMasterId, tenant)?.MAWB;
            return q.ToList();
        }
        public List<string> GetFromExcelDeclarationList(int tenant, string WorkSheeetLogUser)
        {
            var courierHawbFromExcelRepository = new CourierHawbFromExcelRepository(this.context);
            var repoCourierMaster = new CourierMasterRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);

            var q = (from dec in courierHawbFromExcelRepository.GetAllByUser(tenant, WorkSheeetLogUser)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant) on dec.DeclarationId equals status.DeclarationId
                     where rDec.DeclarationNumber != null
                     select status.DeclarationId);
            return q.ToList();
        }

        public List<DeclarationCourierStatus> GetByMasterIDCourierDocumentStatus(int tenant, string CourierMasterId, string DocumentStatusCode
    , string SelectedBOLValue, string SelectedStatusValue, string SelectedTotalInvoiceValue, string SelectedFastIndividualProcessValue, string SelectedCustomStatusValue, string SelectedFinalReleaseValue)
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);
            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant).Where(r => r.DocumentStatusCode == DocumentStatusCode)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);
            q = MoreFilter(SelectedBOLValue, SelectedStatusValue, SelectedTotalInvoiceValue, SelectedFastIndividualProcessValue, SelectedCustomStatusValue, SelectedFinalReleaseValue, q);
            var pocos = q.ToList();

            return pocos;

        }

        public List<DeclarationCourierStatus> GetFromExcelCourierDocumentStatus(int tenant, string userId, string DocumentStatusCode
   , string SelectedBOLValue, string SelectedStatusValue, string SelectedTotalInvoiceValue, string SelectedFastIndividualProcessValue, string SelectedCustomStatusValue, string SelectedFinalReleaseValue)
        {
            var courierHawbFromExcelRepository = new CourierHawbFromExcelRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);
            var q = (from dec in courierHawbFromExcelRepository.GetAllByUser(tenant, userId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant).Where(r => r.DocumentStatusCode == DocumentStatusCode)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);
            q = MoreFilter(SelectedBOLValue, SelectedStatusValue, SelectedTotalInvoiceValue, SelectedFastIndividualProcessValue, SelectedCustomStatusValue, SelectedFinalReleaseValue, q);
            var pocos = q.ToList();

            return pocos;

        }


        public IQueryable<DeclarationCourierStatus> MoreFilter(string SelectedBOLValue, string SelectedStatusValue, string SelectedTotalInvoiceValue, string SelectedFastIndividualProcessValue, string SelectedCustomStatusValue, string SelectedFinalReleaseValue, IQueryable<DeclarationCourierStatus> q)
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

            switch (SelectedFinalReleaseValue)
            {
                case "Y":
                    {
                        q = q.Where(r => r.Declaration.HatraDate.HasValue);
                        break;
                    }
                case "N":
                    {
                        q = q.Where(r => !r.Declaration.HatraDate.HasValue);
                        break;
                    }
            }


            return q;
        }


        public List<DeclarationCourierStatus> GetDeclarationsByIds(List<string> declarationIds, int tenant)
        {

            List<DeclarationCourierStatus> declarations = (from a in context.DeclarationCourierStatuses
                                                           where declarationIds.Contains(a.DeclarationId)
                                                           where a.Tenant == tenant
                                                           select a).ToList();

            return declarations;

        }
        public List<DeclarationCourierStatus> GetDeclarationsByIdsExpectDecWithHatraDate(List<string> declarationIds, int tenant)
        {

            List<DeclarationCourierStatus> declarations = (from a in context.DeclarationCourierStatuses
                                                           where declarationIds.Contains(a.DeclarationId)
                                                           where a.Tenant == tenant && a.Declaration.HatraDate == null
                                                           select a).ToList();

            return declarations;

        }
        public DeclarationCourierStatus GetDeclarationsById(string declarationIds, int tenant)
        {

            DeclarationCourierStatus declarations = (from a in context.DeclarationCourierStatuses
                                                     where declarationIds.Contains(a.DeclarationId)
                                                     where a.Tenant == tenant
                                                     select a).FirstOrDefault();

            return declarations;

        }

        public List<DeclarationCourierStatus> GetDeclarationsByPendings(List<string> declarationIds, int tenant, string pending)
        {
            //    var test = context.DeclarationCourierStatuses.Where(a=> a.Tenant == tenant && a.CourierPendingReasonList.Contains(pending));

            List<DeclarationCourierStatus> declarations = context.DeclarationCourierStatuses.Where(a => a.Tenant == tenant
            && (("," + a.CourierPendingReasonList + ",").Contains("," + pending + ","))).ToList();


            //List<DeclarationCourierStatus> declarations1 =  context.DeclarationCourierStatuses.Where(a=> declarationIds.Contains(a.DeclarationId)
            //                                               && a.Tenant == tenant && a.CourierPendingReasonList==pending).ToList();

            //List<DeclarationCourierStatus> declarations = (from a in context.DeclarationCourierStatuses
            //                                               where declarationIds.Contains(a.DeclarationId)
            //                                               && a.Tenant == tenant && a.CourierPendingReasonList.Split(',').Contains(pending)
            //                                               select a).ToList();

            return declarations;

        }

        public int Lock_forUpdateNOWAIT(string declarationIds)
        {

            var succ = (context as DbContext).FirstOrDefaultFUNOWAITWhere<DeclarationCourierStatus>(rec => rec.DeclarationId == declarationIds);
            //var oracleTransaction =Transaction.Current as OracleTransaction;
            //context.Database.

            return succ;
        }
        public int CountOpenDeclarations(string couriermasterid, int tenant)
        {
            var courierDecs = context.CourierDeclarations.Where(y => y.CourierMasterId == couriermasterid).Select(y => y.DeclarationId);
            return (context.DeclarationCourierStatuses.Count(x => x.IsClosedForFollowUp == false && courierDecs.Contains(x.DeclarationId)));
        }
    }

}
