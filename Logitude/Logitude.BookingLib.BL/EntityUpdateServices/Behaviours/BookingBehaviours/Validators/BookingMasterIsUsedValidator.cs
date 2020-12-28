using Logitude.BookingLib.BL.EntityPMs;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityPOCOs;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BookingLib.BL.EntityUpdateServices.Behaviours.BookingBehaviours.Validators
{
    public class BookingMasterIsUsedValidator : IServiceValidator
    {
        public bool IsUsedInBooking { get; private set; }

        public string ErrorMessage { get; private set; } = "Master field already used in another Booking";

        public BookingMasterIsUsedValidatorArgs Args { get; private set; }

        public void Validate(IServiceInitializer initializer)
        {
            
        }

        public void Validate(BookingMasterIsUsedValidatorArgs args)
        {
            this.Args = args;

            this.RunValidator();
        }

        private void RunValidator()
        {
            bool isValidating = IsValidating();

            if (isValidating)
            {
                Args.LastYearDate = TenantServerConfigration.GetCurrentDateTime(Args.Tenant).AddYears(-1);

                IsUsedInOtherBooking();

                this.ThrowException();
            }
        }

        private bool IsValidating()
        {
            bool output = false;

            if (!Args.IsHybrid)
            {
                if (Args.Tenant != 343 && Args.Tenant != 528)
                {
                    if (Args.DirectionCode == "E" && Args.TransportModeCode == "A")
                    {
                        if (!string.IsNullOrEmpty(Args.Master) && !string.IsNullOrEmpty(Args.AirlinePrefix) && !Args.IsCancelled)
                        {
                            output = true;
                        }
                    }
                }
            }

            return output;
        }

        private void IsUsedInOtherBooking()
        {
            IBookingContext context = BookingContext.GetContext(Args.Tenant);

            IQueryable<Booking> iQueryable = (from d in context.Bookings
                                              where d.Tenant == Args.Tenant
                                              && d.IsCancelled == false
                                              && d.DirectionCode == Args.DirectionCode
                                              && d.TransportModeCode == Args.TransportModeCode
                                              && d.Master == Args.Master
                                              && d.AirlinePrefix == Args.AirlinePrefix
                                              && d.CreateDate >= Args.LastYearDate
                                              select d);

            if (!string.IsNullOrEmpty(Args.BookingId))
            {
                iQueryable = iQueryable.Where(d => d.Id != Args.BookingId);
            }

            if (iQueryable.Count() > 0)
            {
                IsUsedInBooking = true;
            }
        }

        private void ThrowException()
        {
            if (Args.IsThrowingException)
            {
                if (IsUsedInBooking)
                {
                    throw new ApplicationException(ErrorMessage);
                }
            }
        }
    }

    public class BookingMasterIsUsedValidatorArgs
    {
        public int Tenant { get; set; }
        public string BookingId { get; set; }
        public string DirectionCode { get; set; }
        public string TransportModeCode { get; set; }
        public string Master { get; set; }
        public string AirlinePrefix { get; set; }
        public bool IsHybrid { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsThrowingException { get; set; } = true;
        public DateTime LastYearDate { get; internal set; }
    }
}
