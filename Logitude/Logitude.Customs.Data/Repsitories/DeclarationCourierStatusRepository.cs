 
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

namespace Logitude.Customs.Data.Repsitories
{
   public partial class DeclarationCourierStatusRepository:IRepository<DeclarationCourierStatus>
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
                 string SelectedCustomStatusValue
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

            q = MoreFilter(SelectedBOLValue, SelectedStatusValue, SelectedTotalInvoiceValue, SelectedFastIndividualProcessValue, SelectedCustomStatusValue, q);

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
        public List<DeclarationCourierStatus> GetByMasterIDCourierManifestStatusCode(int tenant, string CourierMasterId, string CourierManifestStatusCode
            , string SelectedBOLValue, string SelectedStatusValue, string SelectedTotalInvoiceValue, string SelectedFastIndividualProcessValue, string SelectedCustomStatusValue
            )
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);
            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant).Where(r => r.CourierManifestStatusCode == CourierManifestStatusCode)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);
            q = MoreFilter(SelectedBOLValue, SelectedStatusValue, SelectedTotalInvoiceValue, SelectedFastIndividualProcessValue, SelectedCustomStatusValue, q);
            var pocos = q.ToList();
            return pocos;


        }
        public List<string> GetPendingByMasterID(int tenant, string CourierMasterId)
        {
            IQueryable<DeclarationCourierStatus> q = GetBy(tenant, CourierMasterId);
            var list = q
                .Where( r=> r.CourierPendingReasonList!=null &&  r.CourierPendingReasonList!="")
                .Select(r=>r.CourierPendingReasonList)
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
        private IQueryable<DeclarationCourierStatus> GetBy(int tenant, string CourierMasterId)
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

        public List<DeclarationCourierStatus> GetByMasterIDCourierDocumentStatus(int tenant, string CourierMasterId, string DocumentStatusCode
    , string SelectedBOLValue, string SelectedStatusValue, string SelectedTotalInvoiceValue, string SelectedFastIndividualProcessValue, string SelectedCustomStatusValue)
        {
            var repoCourierDeclaration = new CourierDeclarationRepository(this.context);
            var repoDeclaration = new DeclarationRepository(this.context);
            var q = (from dec in repoCourierDeclaration.GetByCourierMasterId(tenant, CourierMasterId)
                     join rDec in repoDeclaration.GetAll(tenant) on dec.DeclarationId equals rDec.Id
                     join status in GetAll(tenant).Where(r => r.DocumentStatusCode == DocumentStatusCode)
                     on dec.DeclarationId equals status.DeclarationId
                     orderby rDec.CourierHAWB ascending
                     select status);
            q = MoreFilter(SelectedBOLValue, SelectedStatusValue, SelectedTotalInvoiceValue, SelectedFastIndividualProcessValue, SelectedCustomStatusValue, q);
            var pocos = q.ToList();

            return pocos;

        }


        public IQueryable<DeclarationCourierStatus> MoreFilter(string SelectedBOLValue, string SelectedStatusValue, string SelectedTotalInvoiceValue, string SelectedFastIndividualProcessValue, string SelectedCustomStatusValue, IQueryable<DeclarationCourierStatus> q)
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


        public List<DeclarationCourierStatus> GetDeclarationsByIds(List<string> declarationIds,int tenant)
        {

            List<DeclarationCourierStatus> declarations = (from a in context.DeclarationCourierStatuses
                                                           where declarationIds.Contains(a.DeclarationId)
                                                           where a.Tenant == tenant
                                                           select a).ToList();

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
            var courierDecs = context.CourierDeclarations.Where(y => y.CourierMasterId == couriermasterid).Select(y=>y.DeclarationId); 
            return (context.DeclarationCourierStatuses.Count(x => x.IsClosedForFollowUp==false && courierDecs.Contains(x.DeclarationId)));
        }
    }

}
   