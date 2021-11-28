
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
    public partial class CourierMasterRepository : IRepository<CourierMaster>
    {

        public List<CourierMaster> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public bool ChcekIfCourierExists(string Id, string airlineId, string HAWB, string MAWB, int tenant)
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

        public List<CourierMaster> AllCourierMastersWithLandingDateBetweenTwoDates(int tenant, DateTime fromDate, DateTime toDate, string integratorCode)
        {
            var query = (from a in context.CourierMasters
                         where a.Tenant == tenant && a.LandingDate <= toDate && a.LandingDate >= fromDate
                         select a);
            if (integratorCode != "" && integratorCode != null && integratorCode != "undefined")
            {
                query = query.Where(x => x.IntegratorCode == integratorCode);
            }
            return query.ToList();
        }
        public List<dynamic> GetAllCourierMasterForLastmileReport(DateTime? hatraFromDate, DateTime? hatraToDate, DateTime? lastMileFromDate, DateTime? LastMileToDate, string airline, string trucker, string courierHawb, int tenant)
        {
            var query = (from cd in context.CourierDeclarations
                         join c in context.CourierMasters on cd.CourierMasterId equals c.Id
                         join s in context.DeclarationCourierStatuses on cd.DeclarationId equals s.DeclarationId
                         join d in context.Declarations on cd.DeclarationId equals d.Id
                         where cd.Tenant == tenant
                         select new
                         {
                             LastMileDate = s.Delivered ? s.LastMileStatusDate : null,
                             Mawb = d.CourierHAWB,
                             IntegratorCode = c.IntegratorCode,
                             IntegratorName = c.Card != null ? c.Card.LocalName : null,
                             Airline = c.AirlineId,
                             TruckerName = s.Trucker.Card.LocalName,
                             TruckerId = s.TruckerId,
                             LastMileServiceType = s.LastMileServiceType,
                             LastMileStatusName = s.LastMileStatusName,
                             LastMileStatusDate = s.LastMileStatusDate,
                             EstimatedArrivalDate = c.EstimatedArrivalDate,
                             HatraDate = d.HatraDate,
                             TerminalReleaseDate = s.TerminalReleaseDate

                         });


            if (hatraFromDate.HasValue)
            {
                query = query.Where(x => DbFunctions.TruncateTime(x.HatraDate) <= hatraToDate && DbFunctions.TruncateTime(x.HatraDate) >= hatraFromDate);
            }
            if (lastMileFromDate.HasValue)
            {
                query = query.Where(x => DbFunctions.TruncateTime(x.LastMileDate) <= LastMileToDate && DbFunctions.TruncateTime(x.LastMileDate) >= lastMileFromDate);
            }
            if (courierHawb != "" && courierHawb != "null" && courierHawb != "undefined")

            {
                query = query.Where(x => x.Mawb == courierHawb);
            }
            if (airline != "" && airline != "null" && airline != "undefined")
            {
                query = query.Where(x => x.Airline == airline);
            }
            if (trucker != "" && trucker != "null" && trucker != "undefined")
            {
                query = query.Where(x => x.TruckerId == trucker);
            }
            query.OrderBy(x => x.IntegratorCode).ThenBy(x => x.TruckerId).ThenBy(x => x.LastMileServiceType);
            return query.ToList<dynamic>();
        }
    }

}
