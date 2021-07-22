 
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
        public Boolean IsActive(string code,int tenant)
        {
            var status = (from a in context.CourierPendingReasons
                          where a.Code == code && a.Tenant == tenant
                          select a).FirstOrDefault();
            if(status.Inactive)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }

}
   