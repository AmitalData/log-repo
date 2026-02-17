 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.BookingLib.Data.Repositories
{
    public partial class BookingRepository : IRepository<Booking>
    {
        public List<Booking> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Booking GetBookingByMasterAndAirline(string myMasterField, string myAirlineId, int myTenant)
        {
            Booking myEntity = null;

            IQueryable<Booking> iQueryable = (from d in context.Bookings
                                              where d.Tenant == myTenant
                                              && d.IsCancelled == false
                                              && d.Master == myMasterField
                                              select d);

            myEntity = iQueryable.Where(d => d.InterlineId == myAirlineId).FirstOrDefault();

            if (myEntity == null)
            {
                myEntity = iQueryable.Where(d => d.MainCarriageCarrierId == myAirlineId).FirstOrDefault();
            }

            return myEntity;
        }

        public bool IsMasterFieldUsed(string myMasterField, string myAirlinePrefixField, string myEntityId, int myTenant, string myDirectionCode, string myTransportModeCode)
        {
            bool myResult = false;

            if (!string.IsNullOrEmpty(myMasterField) && !string.IsNullOrEmpty(myAirlinePrefixField))
            {                
                IQueryable<Booking> iQueryable = (from d in context.Bookings
                                                  where d.Tenant == myTenant
                                                  && d.IsCancelled == false
                                                  && d.DirectionCode == myDirectionCode
                                                  && d.TransportModeCode == myTransportModeCode
                                                  && d.Master == myMasterField
                                                  && d.AirlinePrefix == myAirlinePrefixField
                                                  select d);

                if (!string.IsNullOrEmpty(myEntityId))
                {
                    iQueryable = iQueryable.Where(d => d.Id != myEntityId);
                }

                if (iQueryable.Count() > 0)
                {
                    myResult = true;
                }
            }

            return myResult;
        }

        public IQueryable<Booking> GetAllFromIdList(List<string> ids, int tenant)
        {
            IQueryable<Booking> entities = (from a in context.Bookings where a.Tenant == tenant && ids.Contains(a.Id) select a);
            return entities;
        }

        public IQueryable<Booking> GetAll()
        {
            return from a in context.Bookings select a;
        }
    }
}
   