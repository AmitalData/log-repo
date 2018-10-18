using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Simplog.Server.Infrastructure;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data; 
using Logitude.BookingLib.Data.EntityMapping;

namespace Logitude.BookingLib.Data
{

    public interface IBookingContext : IContext
    {
   
       	 IDbSet<Booking> Bookings { get; }
		 IDbSet<BookingAnswer> BookingAnswers { get; }
		 IDbSet<BookingAnswerStatus> BookingAnswerStatus { get; }
		 IDbSet<BookingLastRequest> BookingLastRequests { get; }
		 IDbSet<BookingLevel> BookingLevels { get; }
		 IDbSet<BookingPackage> BookingPackages { get; }
		 IDbSet<BookingProduct> BookingProducts { get; }
		 IDbSet<BookingSpaceAllocation> BookingSpaceAllocations { get; }
		 IDbSet<BookingStatus> BookingStatus { get; }
		 IDbSet<FFRStatus> FFRStatus { get; }
		 IDbSet<FlightsSchedulesRequest> FlightsSchedulesRequests { get; }
		 IDbSet<FlightsSchedulesRequestStatus> FlightsSchedulesRequestStatus { get; }
		 IDbSet<FlightsSchedulesResponse> FlightsSchedulesResponses { get; }
	 
         void SetAsModified(object entity);
         void DetectChanges();
         int SaveChanges();

    }
}