 
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

namespace Logitude.Customs.Data.Repsitories
{
   public partial class CourierPendingReasonRepository:IRepository<CourierPendingReason>
   {
        
		public List<CourierPendingReason> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<CourierPendingReason> GetCourierPendingReasonByUnifreightStatus(string unifreightStatusCode)
        {
            List<CourierPendingReason> selectedcourierPendingReasons = (from courierPendingReasons in context.CourierPendingReasons
                                              where courierPendingReasons.UnifreightStatusCode == unifreightStatusCode && !courierPendingReasons.Inactive
                                              select courierPendingReasons).ToList();
            return selectedcourierPendingReasons;
        }

        public CourierPendingReason GetByCode(string code, int tenant)
        {
            return (from a in context.CourierPendingReasons
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
        public List<CourierPendingReason> GetPendingReasonsWithMamanSuspendedCode(int tenant)
        {
            List<CourierPendingReason> selectedcourierPendingReasons = (from courierPendingReasons in context.CourierPendingReasons
                                                                        where courierPendingReasons.Tenant == tenant && !courierPendingReasons.Inactive && courierPendingReasons.MamanSuspendedCode != null
                                                                        select courierPendingReasons).ToList();
            return selectedcourierPendingReasons;
        }

        public List<CourierPendingReason> GetPendingReasonsWithSwissportSuspendedCode(int tenant)
        {
            List<CourierPendingReason> selectedcourierPendingReasons = (from courierPendingReasons in context.CourierPendingReasons
                                                                        where courierPendingReasons.Tenant == tenant && !courierPendingReasons.Inactive && courierPendingReasons.SwissportSuspendedCode != null
                                                                        select courierPendingReasons).ToList();
            return selectedcourierPendingReasons;
        }
        public List<CourierPendingReason> GetPendingReasonsWithOverseasSuspendedCode(int tenant)
        {
            List<CourierPendingReason> selectedcourierPendingReasons = (from courierPendingReasons in context.CourierPendingReasons
                                                                        where courierPendingReasons.Tenant == tenant && !courierPendingReasons.Inactive && courierPendingReasons.OverseasSuspendedCode != null
                                                                        select courierPendingReasons).ToList();
            return selectedcourierPendingReasons;
        }
        public Boolean IsActive(string code,int tenant)
        {
            var status = (from a in context.CourierPendingReasons
                          where a.Code == code && a.Tenant == tenant
                          select a).FirstOrDefault();
            return !(status == null || status.Inactive);
        }
        /* public List<CourierPendingReason> GetByDecdeclarationId(string declarationId,int tenant)
         {
             var status = (from a in context.CourierPendingReasons
                           where a.dec == code && a.Tenant == tenant
                           select a).;
         }*/

    }

}
   