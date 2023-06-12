 
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
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Web;
//using System.Data.Entity;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class DeclarationPendingRepository:IRepository<DeclarationPending>
   {

        public List<DeclarationPending> GetMulti(EntityKeyFields entityKeys)
        {
            DeclarationCourierStatusKeys keys = entityKeys as DeclarationCourierStatusKeys;
            List<DeclarationPending> pendings;

            //string token = HttpContext.Current.Request.Headers["Token"];
            //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            //int tenant = authToken.Tenant;
            int tenant = int.Parse(HttpContext.Current.Request.QueryString["tenant"]);

            pendings = (from a in context.DeclarationPendings
                        join cpr in context.CourierPendingReasons
                        on a.CourierPendingReasonCode equals cpr.Code
                        where a.DeclarationID == keys.DeclarationId && cpr.Tenant == tenant
                        select a).ToList();

            return pendings;
        }

        public List<DeclarationPending> GetDeclarationPendingsByDeclarationId(string declarationId, int tenant)
        {

            List<DeclarationPending> pendings;
            //pendings = (from a in context.DeclarationPendings.Include("CourierPendingReason")
            //               where a.DeclarationID == declarationId
            //               select a).ToList();
            pendings = (from a in context.DeclarationPendings
                        join cpr in context.CourierPendingReasons
                        on a.CourierPendingReasonCode equals cpr.Code
                        where a.DeclarationID == declarationId && cpr.Tenant == tenant
                        select a).ToList();

            return pendings;
        }

        public bool DeclarationHasPending(string declarationId, int tenant)
        {

            bool haspending = false;
            DeclarationPending pending = (from a in context.DeclarationPendings
                                                where a.DeclarationID == declarationId && a.Tenant == tenant
                                                select a).FirstOrDefault();
            if (pending != null)
            {
                haspending = true;
            }


            return haspending;
        }

        public string GetDeclarationPendingsByPendingId(string pending, int tenant)
        {

            List<DeclarationPending> pendings;
            pendings = (from a in context.DeclarationPendings.Include("CourierPendingReason")
                           where a.CourierPendingReasonCode == pending && a.Tenant == tenant
                           select a).ToList();


            if (pendings.Count >= 1)
            {
                return null;
            }

            return pendings.FirstOrDefault().DeclarationID;
        }


        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<DeclarationPending>(rec => rec.DeclarationID == entityKeyFields.Id);
        }

    }

}
   