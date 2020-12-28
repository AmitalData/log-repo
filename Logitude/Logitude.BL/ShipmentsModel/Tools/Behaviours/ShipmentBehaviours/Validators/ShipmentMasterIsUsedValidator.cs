using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.Initializers;
using Logitude.BookingLib.Data;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Server.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours.ShipmentBehaviours.Validators
{
    // ShipmentMasterIsUsedValidator
    // ShipmentMasterFieldValidator

    public class ShipmentMasterIsUsedValidator : IServiceValidator
    {
        public bool IsUsedInShipment { get; private set; }

        public bool IsUsedInBooking { get; private set; }

        public string ShipmentErrorMessage { get; private set; } = "Master field already used in another Shipment";

        public string BookingErrorMessage { get; private set; } = "Master field already used in another Booking";

        public ShipmentMasterIsUsedValidatorArgs Args { get; private set; }

        public void Validate(IServiceInitializer initializer)
        {
            ShipmentServiceInitializer serviceInitializer = (ShipmentServiceInitializer)initializer;

            this.Args = new ShipmentMasterIsUsedValidatorArgs
            {
                Tenant = serviceInitializer.Tenant,
                ShipmentId = serviceInitializer.EntityPM.Id,
                BookingId = serviceInitializer.EntityPM.BookingId,
                DirectionId = serviceInitializer.EntityPM.DirectionId,
                TransportModeId = serviceInitializer.EntityPM.TransportModeId,
                ShipmentLevelCode = serviceInitializer.EntityPM.ShipmentLevelCode,
                Master = serviceInitializer.EntityPM.Master,
                AirlinePrefix = serviceInitializer.EntityPM.AirlinePrefix,
                IsHybrid = serviceInitializer.EntityPM.IsHybrid,
                IsCancelled = serviceInitializer.EntityPM.IsCancelled,
            };

            this.RunValidator();
        }

        public void Validate(ShipmentMasterIsUsedValidatorArgs args)
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

                IsUsedInOtherShipment();

                if (!IsUsedInShipment)
                {
                    IsUsedInOtherBooking();
                }

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
                    if (Args.DirectionId == "E" && Args.TransportModeId == "A")
                    {
                        if (Args.ShipmentLevelCode == "C" || Args.ShipmentLevelCode == "D")
                        {
                            if (!string.IsNullOrEmpty(Args.Master) && !string.IsNullOrEmpty(Args.AirlinePrefix) && !Args.IsCancelled)
                            {
                                output = true;
                            }
                        }
                    }
                }
            }

            return output;
        }

        private void IsUsedInOtherShipment()
        {
            IShipmentsContext context = ShipmentsContext.GetContext(Args.Tenant);

            var iQueryable = (from myShipment in context.Shipments
                              join db_Masters in context.ShipmentMasterDatas
                              on myShipment.MasterShipmentDataId equals db_Masters.Id into ShipmentsMasters
                              from myMasterData in ShipmentsMasters.DefaultIfEmpty()

                              where myShipment.Tenant == Args.Tenant
                              && (myShipment.ShipmentLevelCode == "C" || myShipment.ShipmentLevelCode == "D")
                              && myShipment.IsCancelled == false
                              && myShipment.DirectionId == Args.DirectionId
                              && myShipment.TransportModeId == Args.TransportModeId
                              && myMasterData.Master == Args.Master
                              && myMasterData.AirlinePrefix == Args.AirlinePrefix
                              && myShipment.OperationalDate >= Args.LastYearDate
                              select myShipment);

            if (!string.IsNullOrEmpty(Args.ShipmentId))
            {
                iQueryable = iQueryable.Where(d => d.Id != Args.ShipmentId);
            }

            if (iQueryable.Count() > 0)
            {
                IsUsedInShipment = true;
            }
        }

        private void IsUsedInOtherBooking()
        {
            IBookingContext context = BookingContext.GetContext(Args.Tenant);

            IQueryable<Booking> iQueryable = (from d in context.Bookings
                                              where d.Tenant == Args.Tenant
                                              && d.IsCancelled == false
                                              && d.DirectionCode == Args.DirectionId
                                              && d.TransportModeCode == Args.TransportModeId
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
                if (IsUsedInShipment)
                {
                    throw new ApplicationException(ShipmentErrorMessage);
                }

                else if (IsUsedInBooking)
                {
                    throw new ApplicationException(BookingErrorMessage);
                }
            }
        }
    }

    public class ShipmentMasterIsUsedValidatorArgs
    {
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public string BookingId { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public string ShipmentLevelCode { get; set; }
        public string Master { get; set; }
        public string AirlinePrefix { get; set; }
        public bool IsHybrid { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsThrowingException { get; set; } = true;
        public DateTime LastYearDate { get; internal set; }
    }
}
