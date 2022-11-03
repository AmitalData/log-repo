using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.ShipmentsModel.Tools.Validating
{
    public class RoutingDatesValidator
    {
        private ShipmentPM shipmentPM;
        private Shipment shipmentPOCO;
        public RoutingDatesValidator(ShipmentPM entityPM, Shipment entityPoco)
        {
            this.shipmentPM = entityPM;
            this.shipmentPOCO = entityPoco;
        }

        public void Validate()
        {
            //this.ValidatePreCarriage();
            //this.ValidateOnCarriage();
            //this.ValidateMainCarriage();
            //this.ValidateTransshipment1();
            //this.ValidateTransshipment2();
            //this.ValidateTransshipment3();
        }

        public static bool IsActualDateValid(DateTime? myDate, int tenant)
        {
            if (myDate == null) return true;

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).AddHours(24);
            if (myDate > todayDate)
            {
                return false;
            }

            return true;
        }
        public static bool IsRoutingLegDatesValid(DateTime? date1, DateTime? date2)
        {
            if (date1 == null || date2 == null) return true;

            if (date1 > date2.Value.AddHours(24))
            {
                return false;
            }

            return true;
        }
        public static bool IsDateSeriesBiggerNotEqual(DateTime? date1, DateTime? date2)
        {
            if (date1 == null || date2 == null) return false;

            if (date1 > date2)
            {
                return true;
            }

            return false;
        }
        public static bool IsDateBigger(DateTime? date1, DateTime? date2)
        {
            if (date1 == null || date2 == null) return false;

            if (date1 >= date2)
            {
                return true;
            }

            return false;
        }
        public static bool IsDateSmaller(DateTime? date1, DateTime? date2)
        {
            if (date1 == null || date2 == null) return false;

            if (date1 <= date2)
            {
                return true;
            }

            return false;
        }
    }
}
