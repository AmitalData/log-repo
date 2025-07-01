using System;

namespace Logitude.CustomsMessaging.Utils
{
    public class DirectionsUtil
    {

        public static String ShippingDirection(String shippingDirection)
        {
            switch (shippingDirection?.ToUpper())
            {
                case "IMPORT":
                    return "I";

                case "EXPORT":
                    return "E";

                default: return shippingDirection;

            }
        }
    }
}
