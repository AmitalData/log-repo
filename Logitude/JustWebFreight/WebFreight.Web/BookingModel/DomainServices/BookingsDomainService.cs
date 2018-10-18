
namespace WebFreight.Web.BookingModel.DomainServices
{
    using Logitude.BookingLib.Data;
    using Logitude.BookingLib.Data.EntityPOCOs;
    using Logitude.BookingLib.Data.Repositories;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;    
    using System.Linq;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using WebFreight.Web.DataContracts;
    using WebFreight.Web.Helpers;
    using WebFreight.Web.Security;
    using Logitude.BookingLib.BL.EntityQueryServices;
    using System.Data.Entity.Core;

    // TODO: Create methods containing your application logic.
    [EnableClientAccess()]
    public partial class BookingsDomainService : LogitudeDomainService
    {
        private IBookingContext objectContext;
        private BookingRepository bookingRepository;
        private BookingSpaceAllocationRepository bookingSpaceAllocationRepository;
        private BookingAnswerRepository bookingAnswerRepository;
        private BookingLevelRepository bookingLevelRepository;
        private BookingStatusRepository bookingStatusRepository;
        private FFRStatusRepository ffrStatusRepository;

        private BookingSpaceAllocationQueryService bookingSpaceAllocationQuery;
        private BookingLevelQueryService bookingLevelQuery;
        private BookingStatusQueryService bookingStatusQuery;
        private FFRStatusQueryService ffrStatusQuery;

        public BookingsDataCounts GetBookingsCounts(int tenant)
        {
            BookingsDataCounts myResult = new BookingsDataCounts() { Id = tenant };

            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Booking", "READ", tenant);

            if (objectContext == null)
            {
                objectContext = BookingContext.GetContext(tenant);
            }

            bookingRepository = new BookingRepository(objectContext);
            IQueryable<Booking> allBookings = bookingRepository.GetAll(tenant);

            myResult.CreatedBookingsCount = allBookings.Where(d => d.BookingStatusCode == "CRT" && !d.IsCancelled).Count();
            myResult.WaitingForResponseCount = allBookings.Where(d => d.WaitingForResponse && !d.IsCancelled).Count();
            myResult.ConfirmedBookingsCount = allBookings.Where(d => d.BookingStatusCode == "CNF" && !d.IsCancelled).Count();
            myResult.RejectedBookingsCount = allBookings.Where(d => d.HasErrors && !d.IsCancelled).Count();
            myResult.InProgressBookingsCount = allBookings.Where(d => d.BookingStatusCode != "AWB" && !d.IsCancelled).Count();
            myResult.AllBookingsCount = allBookings.Where(d => !d.IsCancelled).Count();
            myResult.CancelledBookingsCount = allBookings.Where(d => d.IsCancelled).Count();
            
            return myResult;
        }

        protected override bool PersistChangeSet()
        {
            try
            {
                objectContext.SaveChanges();
            }

            catch (OptimisticConcurrencyException ex)
            {
                throw new Exception("Sorry you can't update this record right now it's being updated by another user");
            }

            return base.PersistChangeSet();
        }
    }
}


