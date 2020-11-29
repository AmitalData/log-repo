 
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
   public partial class CourierMasterRepository:IRepository<CourierMaster>
   {
        
		public List<CourierMaster> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public bool ChcekIfCourierExists(string Id, string airlineId, string HAWB , string MAWB, int tenant)
        {
            return (from a in context.CourierMasters
                    where a.AirlineId == airlineId && a.HAWB == HAWB && a.MAWB == MAWB && a.Tenant == tenant && a.Id != Id
                    select a).Any();
        }

        public CourierMaster GetSingleCourier(string airlineId, string HAWB, string MAWB, int tenant)
        {
            return (from a in context.CourierMasters
                    where a.AirlineId == airlineId && a.HAWB == HAWB && a.MAWB == MAWB
                    select a).FirstOrDefault();
        }

        public CourierMaster GetCourierMaster(string airlineId, string HAWB, string MAWB, int tenant)
        {
            if (String.IsNullOrWhiteSpace(HAWB))
            {
                return (from a in context.CourierMasters
                        where a.AirlineId == airlineId && a.MAWB == MAWB && a.Tenant == tenant
                        select a).FirstOrDefault();
            }
            else
            {
                return (from a in context.CourierMasters
                        where a.AirlineId == airlineId && a.HAWB == HAWB && a.MAWB == MAWB && a.Tenant == tenant
                        select a).FirstOrDefault();
            }
        }

        public List<CourierMaster> GetAllOpenCourierMasters(int tenant)
        {
            return (from a in context.CourierMasters
                    where a.Tenant == tenant && a.IsOpen == true
                    select a).ToList();
        }

        public List<CourierMaster> GetAllOpenCourierMastersWithLandingDate(int tenant)
        {
            DateTime nowDate = DateTime.Now;
            DateTime dayAgoDate = DateTime.Now.AddDays(-1);
            return (from a in context.CourierMasters
                    where a.Tenant == tenant && a.IsOpen == true && a.LandingDate < nowDate && a.LandingDate > dayAgoDate
                    select a).ToList();
        }
    }

}
   