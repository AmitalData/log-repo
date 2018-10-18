 
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
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BookingLib.Data.Repositories
{
   public partial class BookingLastRequestRepository:IRepository<BookingLastRequest>
   {
        
		public List<BookingLastRequest> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public IQueryable<BookingLastRequest> GetBookingLastRequestsByBookingId(string bookingId, int tenant)
        {
            IQueryable<BookingLastRequest> list = (from a in context.BookingLastRequests.Include("Carrier")
                                             where a.Tenant == tenant && a.BookingId == bookingId
                                             select a);
            return list;
        }

   }

}
   